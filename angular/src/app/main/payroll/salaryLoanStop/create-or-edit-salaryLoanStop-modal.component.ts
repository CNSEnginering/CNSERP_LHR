import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { finalize } from "rxjs/operators";
import { AppComponentBase } from "@shared/common/app-component-base";
import { CreateOrEditSalaryLoanStopDto } from "../shared/dto/SalaryLoanStop-dto";
import { SalaryLoanStopService } from "../shared/services/SalaryLoanStop.service";
import { PayRollLookupTableModalComponent } from "@app/finders/payRoll/payRoll-lookup-table-modal.component";
import { CheckboxCellComponent } from "@app/shared/common/checkbox-cell/checkbox-cell.component";

@Component({
    selector: "createOrEditSalaryLoanStopModal",
    templateUrl: "./create-or-edit-salaryLoanStop-modal.component.html",
})
export class CreateOrEditSalaryLoanStopModalComponent extends AppComponentBase {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild("PayRollLookupTableModal", { static: true })
    payRollLookupTableModal: PayRollLookupTableModalComponent;
    active = false;
    saving = false;
    employeeName: string = "";
    processing = false;
    private setParms;
    check: boolean;
    salaryYear: number;
    salaryMonth: number;
    typeID: number;
    remarks: string;

    SalaryLoanStop: CreateOrEditSalaryLoanStopDto;
    SalaryLoanStopDetail: CreateOrEditSalaryLoanStopDto[] = [];

    target: string = "";
    constructor(
        injector: Injector,
        private _SalaryLoanStopService: SalaryLoanStopService
    ) {
        super(injector);
    }
    openEmployeeModal() {
        this.target = "Employee";
        this.payRollLookupTableModal.id = String(
            this.SalaryLoanStop.employeeID
        );
        this.payRollLookupTableModal.displayName = String(this.employeeName);
        this.payRollLookupTableModal.show(this.target);
    }
    show(id?: number): void {
        debugger;

        if (!id) {
            this.SalaryLoanStop = new CreateOrEditSalaryLoanStopDto();
            this.SalaryLoanStop.id = id;

            this.active = true;
            this.modal.show();
        } else {
            this._SalaryLoanStopService
                .getSalaryLoanStopForEdit(id)
                .subscribe((result) => {
                    this.SalaryLoanStop = result.SalaryLoanStop;
                    this.salaryYear = result.SalaryLoanStop.salaryYear;
                    this.salaryMonth = result.SalaryLoanStop.salaryMonth;
                    this.remarks = result.SalaryLoanStop.remarks;
                    this.typeID = result.SalaryLoanStop.typeID;
                    this.active = true;
                    this.modal.show();
                });
        }
    }
    setEmployeeNull() {
        this.SalaryLoanStop.employeeID = null;
        this.employeeName = "";
    }

    getNewEmployee() {
        this.SalaryLoanStop.employeeID = Number(
            this.payRollLookupTableModal.id
        );
        this.employeeName = this.payRollLookupTableModal.displayName;
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

    save(): void {
        debugger;
        this.SalaryLoanStopDetail = [];
        this.gridApi.forEachNode((node) => {
            this.SalaryLoanStop = new CreateOrEditSalaryLoanStopDto();
            this.SalaryLoanStop.id=node.data.stopSalaryID;
            this.SalaryLoanStop.salaryMonth = this.salaryMonth;
            this.SalaryLoanStop.salaryYear = this.salaryYear;
            this.SalaryLoanStop.typeID = this.typeID;
            this.SalaryLoanStop.employeeID = node.data.employeeID;
            this.SalaryLoanStop.include = node.data.include;
            this.SalaryLoanStop.loanID = node.data.loanID;
            this.SalaryLoanStop.remarks = this.remarks;
            this.SalaryLoanStopDetail.push(this.SalaryLoanStop);
        });
        this.saving = true;
        this._SalaryLoanStopService
            .createOrEdit(this.SalaryLoanStopDetail)
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

    close(): void {
        debugger;
        this.active = false;
        this.modal.hide();
    }

    //==================================Grid=================================
    protected gridApi;
    protected gridColumnApi;
    protected rowData = [];
    protected rowSelection;
    columnDefs = [
        {
            headerName: this.l("SrNo"),
            field: "srNo",
            sortable: true,
            width: 60,
            valueGetter: "node.rowIndex+1",
        },
        {
            headerName: this.l("StopID"),
            field: "stopSalaryID",
            sortable: true,
            filter: true,
            width: 120,
            resizable: true,
        },
        {
            headerName: this.l("EmployeeID"),
            field: "employeeID",
            sortable: true,
            filter: true,
            width: 120,
            resizable: true,
        },
        {
            headerName: this.l("EmployeeName"),
            field: "employeeName",
            sortable: true,
            filter: true,
            width: 120,
            resizable: true,
        },
        {
            headerName: this.l("Amount"),
            field: "amount",
            sortable: true,
            filter: false,
            width: 120,
            resizable: true,
        },
        {
            headerName: this.l("Stop"),
            field: "include",
            sortable: true,
            filter: false,
            width: 120,
            resizable: true,
            cellRendererFramework: CheckboxCellComponent,
        },
        {
            headerName: this.l("LoanID"),
            field: "loanID",
            sortable: true,
            filter: false,
            width: 120,
            resizable: true,
        },
    ];

    onGridReady(params) {
        debugger;
        this.rowData = [];

        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        params.api.sizeColumnsToFit();
        this.rowSelection = "multiple";
        if (this.SalaryLoanStop.id) {
            this.onAddRow();
        }
    }

    onAddRow(): void {
        debugger;
        if (this.typeID == undefined) {
            this.message.warn("Please select Amount Type");
            return;
        }
        var index = this.gridApi.getDisplayedRowCount();
        if (this.typeID == 1) {
            this.processing = true;
            this._SalaryLoanStopService
                .getEmployeesLoan(this.salaryYear, this.salaryMonth)
                .subscribe((result) => {
                    debugger;
                    console.log(result);
                    this.rowData = [];
                    this.rowData = result["result"];
                    this.processing = false;
                });
        } else if (this.typeID == 2) {
            this.processing = true;
            this._SalaryLoanStopService
                .getEmployeeSalaries(this.salaryYear, this.salaryMonth)
                .subscribe((result) => {
                    debugger;
                    console.log(result);
                    this.rowData = [];
                    this.rowData = result["result"];
                    this.processing = false;
                });
        }else if (this.typeID == 3) {
            this.processing = true;
            this._SalaryLoanStopService
                .getAllowance(this.salaryYear, this.salaryMonth)
                .subscribe((result) => {
                    debugger;
                    console.log(result);
                    this.rowData = [];
                    this.rowData = result["result"];
                    this.processing = false;
                });
        }
        this.gridApi.refreshCells();
    }

    onCellValueChanged(params) {
        debugger;
        this.gridApi.refreshCells();
    }

    //==================================Grid=================================
}
