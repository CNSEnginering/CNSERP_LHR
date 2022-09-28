import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    OnInit,
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { finalize } from "rxjs/operators";
import { AppComponentBase } from "@shared/common/app-component-base";
import * as moment from "moment";
import { CreateOrEditEmployeeDeductionsDto } from "../shared/dto/employeeDeductions-dto";
import { EmployeeDeductionsServiceProxy } from "../shared/services/employeeDeductions.service";
import { PayRollLookupTableModalComponent } from "@app/finders/payRoll/payRoll-lookup-table-modal.component";
import { DeductionTypesServiceProxy } from "../shared/services/deductionTypes.service";
import { GetDeductionTypesForViewDto } from "../shared/dto/deductionTypes-dto";
import { CreateOrEditEmployeeAdvancesDto } from "../shared/dto/advances-dto";
import { EmployeeAdvancesServiceProxy } from "../shared/services/employeeAdvances.service";

@Component({
    selector: "createOrEditEmployeeAdvancesModal",
    templateUrl: "./create-or-edit-employee-advances-modal.component.html",
})
export class CreateOrEditEmployeeAdvancesModalComponent extends AppComponentBase {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild("PayRollLookupTableModal", { static: true })
    PayRollLookupTableModal: PayRollLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    salaryYear: string;
    salaryMonth: string;

    bsValue = new Date();

    employeeAdvances: CreateOrEditEmployeeAdvancesDto = new CreateOrEditEmployeeAdvancesDto();

    target: string;
    advanceDate: Date;
    validAdvance: boolean = false;

    emptyNumberFilter: number;
    emptyStringFilter: string;
    emptyDateFilter: moment.Moment;

    constructor(
        injector: Injector,
        private _employeeAdvancesService: EmployeeAdvancesServiceProxy
    ) {
        super(injector);
    }

    show(employeeAdvancesId?: number): void {
        if (!employeeAdvancesId) {
            this.employeeAdvances = new CreateOrEditEmployeeAdvancesDto();
            this.employeeAdvances.id = employeeAdvancesId;
            this.advanceDate = new Date();
            this.salaryYear = String(this.advanceDate.getFullYear());
            this.salaryMonth = String(this.advanceDate.getMonth() + 1);

            // this.deductionDate=moment().format("DD/MM/YYYY");
            // this.salaryYear=this.deductionDate.substr(6, 4);
            // this.salaryMonth=this.deductionDate.substr(3,2);
            this.employeeAdvances.active = true;

            this._employeeAdvancesService
                .getMaxAdvanceId()
                .subscribe((result) => {
                    this.employeeAdvances.advanceID = result;
                });

            this.active = true;
            this.modal.show();
        } else {
            this._employeeAdvancesService
                .getEmployeeAdvancesForEdit(employeeAdvancesId)
                .subscribe((result) => {
                    this.employeeAdvances = result.employeeAdvances;

                    this.salaryMonth = (
                        "0" + String(this.employeeAdvances.salaryMonth)
                    ).slice(-2);
                    this.salaryYear = String(this.employeeAdvances.salaryYear);
                    //this.employeeDeductions.deductionDate==null || undefined ? this.deductionDate="" : this.deductionDate=moment(this.employeeDeductions.deductionDate).format("DD/MM/YYYY");

                    this.advanceDate = moment(
                        this.employeeAdvances.advanceDate
                    ).toDate();

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        if (this.salaryYear == "Select Year") {
            this.notify.error(this.l("Please select Salary Year"));
            return;
        }

        if (this.salaryMonth == "Select Month") {
            this.notify.error(this.l("Please select Salary Month"));
            return;
        }

        this.saving = true;

        this.employeeAdvances.audtDate = moment();
        this.employeeAdvances.audtUser = this.appSession.user.userName;

        this.employeeAdvances.advanceDate = moment(this.advanceDate);

        if (!this.employeeAdvances.id) {
            this.employeeAdvances.createDate = moment();
            this.employeeAdvances.createdBy = this.appSession.user.userName;
        }

        this.employeeAdvances.salaryMonth = Number(this.salaryMonth);
        this.employeeAdvances.salaryYear = Number(this.salaryYear);

        this._employeeAdvancesService
            .createOrEdit(this.employeeAdvances)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l("SavedSuccessfully"));
                this.close();
                this.modalSave.emit(null);
            });
    }

    onChange(newvalue) {
        this.salaryYear = String(newvalue.getFullYear());
        this.salaryMonth = ("0" + String(newvalue.getMonth() + 1)).slice(-2);
    }

    openEmployeeModal() {
        debugger;
        this.target = "Employee";
        this.PayRollLookupTableModal.id = String(
            this.employeeAdvances.employeeID
        );
        this.PayRollLookupTableModal.displayName = this.employeeAdvances.employeeName;
        this.PayRollLookupTableModal.show(this.target);
    }

    setEmployeeNull() {
        this.employeeAdvances.employeeID = null;
        this.employeeAdvances.employeeName = "";
    }
    getNewPayRollModal() {
        switch (this.target) {
            case "Employee":
                this.getNewEmployee();
                break;
            default:
                break;
        }
    }

    getNewEmployee() {
        this.employeeAdvances.employeeID = Number(
            this.PayRollLookupTableModal.id
        );
        this.employeeAdvances.employeeName = this.PayRollLookupTableModal.displayName;
    }

    isValidAdvance(e) {
        debugger;
        if (
            this.employeeAdvances.employeeID != undefined &&
            e.target.value != ""
        ) {
            let amount: number = parseInt(e.target.value.replace(",", ""));
            this._employeeAdvancesService
                .isValidAdvanceAmount(amount, this.employeeAdvances.employeeID)
                .subscribe((res) => {
                    if (res["result"] == false) {
                        this.validAdvance = false;
                        this.message.warn(
                            "Advance amount is greater than employee's salary"
                        );
                    } else {
                        this.validAdvance = true;
                    }
                });
        } else return false;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
