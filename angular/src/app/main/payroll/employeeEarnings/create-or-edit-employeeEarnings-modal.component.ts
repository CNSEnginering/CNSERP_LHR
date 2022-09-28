import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditEmployeeEarningsDto } from '../shared/dto/employeeEarnings-dto';
import { EmployeeEarningsServiceProxy } from '../shared/services/employeeEarnings.service';
import { PayRollLookupTableModalComponent } from '@app/finders/payRoll/payRoll-lookup-table-modal.component';
import { GetEarningTypesForViewDto } from '../shared/dto/earningTypes-dto';
import { EarningTypesServiceProxy } from '../shared/services/earningTypes.service';

@Component({
    selector: 'createOrEditEmployeeEarningsModal',
    templateUrl: './create-or-edit-employeeEarnings-modal.component.html'
})
export class CreateOrEditEmployeeEarningsModalComponent extends AppComponentBase {


    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('PayRollLookupTableModal', { static: true }) PayRollLookupTableModal: PayRollLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    salaryYear: string;
    salaryMonth: string;


    employeeEarnings: CreateOrEditEmployeeEarningsDto = new CreateOrEditEmployeeEarningsDto();

    target: string;

    years: any;
    earningDate: Date;

    emptyNumberFilter: number;
    emptyStringFilter: string;
    emptyDateFilter: moment.Moment;

    earningTypesDtoArray: GetEarningTypesForViewDto[];

    constructor(
        injector: Injector,
        private _employeeEarningsServiceProxy: EmployeeEarningsServiceProxy,
        private _earningTypesServiceProxy: EarningTypesServiceProxy

    ) {
        super(injector);
    }



    show(employeeEarningsId?: number): void {
        debugger;
        this._earningTypesServiceProxy.getAll(this.emptyStringFilter, this.emptyNumberFilter, this.emptyNumberFilter, this.emptyStringFilter, -1, this.emptyStringFilter, this.emptyDateFilter, this.emptyDateFilter, this.emptyStringFilter, this.emptyDateFilter, this.emptyDateFilter, this.emptyStringFilter, 0, 500)
            .subscribe(result => {
                this.earningTypesDtoArray = result.items;
            });

        if (!employeeEarningsId) {
            this.employeeEarnings = new CreateOrEditEmployeeEarningsDto();
            this.employeeEarnings.id = employeeEarningsId;
            this.employeeEarnings.earningTypeID = 0;
            this.earningDate=new Date();
            this.salaryYear=String(this.earningDate.getFullYear());
            this.salaryMonth=String(this.earningDate.getMonth() + 1);
            // this.earningDate = moment().format("DD/MM/YYYY");
            // this.salaryYear = this.earningDate.substr(6, 4);
            // this.salaryMonth = this.earningDate.substr(3, 2);

            this.employeeEarnings.active = true;

            this._employeeEarningsServiceProxy.getMaxEarningId().subscribe(result => {
                this.employeeEarnings.earningID = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            this._employeeEarningsServiceProxy.getEmployeeEarningsForEdit(employeeEarningsId).subscribe(result => {
                this.employeeEarnings = result.employeeEarnings;

                this.salaryMonth= ('0' + String(this.employeeEarnings.salaryMonth)).slice(-2);
                this.salaryYear=String(this.employeeEarnings.salaryYear);
               this.earningDate=moment(this.employeeEarnings.earningDate).toDate();

                //this.employeeEarnings.earningDate==null || undefined ? this.earningDate="" : this.earningDate=moment(this.employeeEarnings.earningDate).format("DD/MM/YYYY");

                this.active = true;
                this.modal.show();
            });
        }
    }


    save(): void {
        debugger;
        if (this.salaryYear == 'Select Year') {
            this.notify.error(this.l('Please select Salary Year'));
            return;
        }

        if (this.salaryMonth == 'Select Month') {
            this.notify.error(this.l('Please select Salary Month'));
            return;
        }

        this.saving = true;

        this.employeeEarnings.audtDate = moment();
        this.employeeEarnings.audtUser = this.appSession.user.userName;

        this.employeeEarnings.earningDate = moment(this.earningDate);

        if (!this.employeeEarnings.id) {
            this.employeeEarnings.createDate = moment();
            this.employeeEarnings.createdBy = this.appSession.user.userName;
        }

        this.employeeEarnings.salaryMonth = Number(this.salaryMonth);
        this.employeeEarnings.salaryYear = Number(this.salaryYear);

        this._employeeEarningsServiceProxy.createOrEdit(this.employeeEarnings)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    onChange(newvalue)
    {
        this.salaryYear=String(newvalue.getFullYear());
        this.salaryMonth=('0' + String(newvalue.getMonth()+1)).slice(-2);
    }

    openEmployeeModal() {
        debugger;
        this.target = "Employee";
        this.PayRollLookupTableModal.id = String(this.employeeEarnings.employeeID);
        this.PayRollLookupTableModal.displayName = this.employeeEarnings.employeeName;
        this.PayRollLookupTableModal.show(this.target);
    }

    setEmployeeNull() {
        this.employeeEarnings.employeeID = null;
        this.employeeEarnings.employeeName = "";
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
        this.employeeEarnings.employeeID = Number(this.PayRollLookupTableModal.id);
        this.employeeEarnings.employeeName = this.PayRollLookupTableModal.displayName;
        console.log(this.employeeEarnings.employeeName);
    }





    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
