import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    OnInit,
    Input,
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
import { CheckboxCellComponent } from "@app/shared/common/checkbox-cell/checkbox-cell.component";
import { EmployeesServiceProxy } from "../shared/services/employee-service";
import { EmployeesDto } from "../shared/dto/employee-dto";
import { EarningTypesServiceProxy } from "../shared/services/earningTypes.service";
import { GetEarningTypesForViewDto } from "../shared/dto/earningTypes-dto";
import { AdjustmentDocType, AdjHDto, AdjDetail } from "../shared/dto/AdjH-dto";
import { EmployeeAdjDDto } from "../shared/dto/employeeDeductionD-dto";
import { AdjustmentHServiceProxy } from "../shared/services/adjustmentH.service";
import { CreateOrEditEmployeeEarningsDto } from "../shared/dto/employeeEarnings-dto";

@Component({
    selector: "createOrEditEmployeeDeductionsModal",
    templateUrl: "./create-or-edit-employeeDeductions-modal.component.html",
})
export class CreateOrEditEmployeeDeductionsModalComponent extends AppComponentBase {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild("PayRollLookupTableModal", { static: true })
    PayRollLookupTableModal: PayRollLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    deductionActive: boolean = false;
    salaryYear: string;
    salaryMonth: string;
    docType: string;
    bsValue = new Date();
    remarks: string;
    headerInfo: AdjHDto = new AdjHDto();
    @Input() mode: string = "";
    adjTypeDetails: any;
    activeEmployees: EmployeesDto[];
    // employeeDeductions: EmployeeAdjDDto;
    target: string;
    deductionDate: Date;
    amount: number;
    emptyNumberFilter: number;
    emptyStringFilter: string;
    emptyDateFilter: moment.Moment;

    deductionTypesDtoArray: GetDeductionTypesForViewDto[];

    earningTypesDtoArray: GetEarningTypesForViewDto[];

    constructor(
        injector: Injector,
        private _employeeServiceProxy: EmployeesServiceProxy,
        private _employeeDeductionsServiceProxy: EmployeeDeductionsServiceProxy,
        private _deductionTypesServiceProxy: DeductionTypesServiceProxy,
        private _earningTypesServiceProxy: EarningTypesServiceProxy,
        private _adjHServiceProxy: AdjustmentHServiceProxy
    ) {
        super(injector);
    }

    Clone(adjHId?: number, type?: string): void {
        if (type == undefined) {
            this.docType = "deduction";
        } else {
            this.docType = type;
        }

        if (this.docType == "earnings") {
            this._earningTypesServiceProxy
                .getAll(
                    this.emptyStringFilter,
                    this.emptyNumberFilter,
                    this.emptyNumberFilter,
                    this.emptyStringFilter,
                    -1,
                    this.emptyStringFilter,
                    this.emptyDateFilter,
                    this.emptyDateFilter,
                    this.emptyStringFilter,
                    this.emptyDateFilter,
                    this.emptyDateFilter,
                    this.emptyStringFilter,
                    0,
                    500
                )
                .subscribe((result) => {
                    this.earningTypesDtoArray = result.items;
                });
        } else {
            this._deductionTypesServiceProxy
                .getAll(
                    this.emptyStringFilter,
                    this.emptyNumberFilter,
                    this.emptyNumberFilter,
                    this.emptyStringFilter,
                    -1,
                    this.emptyStringFilter,
                    this.emptyDateFilter,
                    this.emptyDateFilter,
                    this.emptyStringFilter,
                    this.emptyDateFilter,
                    this.emptyDateFilter,
                    this.emptyStringFilter,
                    0,
                    500
                )
                .subscribe((result) => {
                    this.deductionTypesDtoArray = result.items;
                });
        }

        if (!adjHId) {
           
        } else {
            
            this._adjHServiceProxy
                .getAdjHForEdit(adjHId)
                .subscribe((result) => {
                    debugger;
                    this.headerInfo = result.AdjHDto;
                    //this.headerInfo.id=0;
                    this.salaryMonth = (
                        "0" + String(this.headerInfo.salaryMonth)
                    ).slice(-2);
                    this.salaryYear = String(this.headerInfo.salaryYear);
                    this.remarks =
                        this.headerInfo.docType == AdjustmentDocType.Earning
                            ? this.headerInfo.adjDetails.earningDetail[0]
                                  .remarks
                            : this.headerInfo.adjDetails.deductionDetail[0]
                                  .remarks;
                    this.amount =
                        this.headerInfo.docType == AdjustmentDocType.Earning
                            ? this.headerInfo.adjDetails.earningDetail[0].amount
                            : this.headerInfo.adjDetails.deductionDetail[0]
                                  .amount;
                    this.deductionActive =
                        this.headerInfo.docType == AdjustmentDocType.Earning
                            ? this.headerInfo.adjDetails.earningDetail[0].active
                            : this.headerInfo.adjDetails.deductionDetail[0]
                                  .active;
                                
                    this.active = true;
                   
                    this._adjHServiceProxy.getMaxDocID().subscribe((result) => {
                        this.headerInfo.docID = result["result"];
                    });
                    this.headerInfo.id=-1;
                   // this.modal.show();
                   
                });
            
         //  this.save();
        }
        this.save();
    }

    show(adjHId?: number, type?: string): void {
        if (type == undefined) {
            this.docType = "deduction";
        } else {
            this.docType = type;
        }

        if (this.docType == "earnings") {
            this._earningTypesServiceProxy
                .getAll(
                    this.emptyStringFilter,
                    this.emptyNumberFilter,
                    this.emptyNumberFilter,
                    this.emptyStringFilter,
                    -1,
                    this.emptyStringFilter,
                    this.emptyDateFilter,
                    this.emptyDateFilter,
                    this.emptyStringFilter,
                    this.emptyDateFilter,
                    this.emptyDateFilter,
                    this.emptyStringFilter,
                    0,
                    500
                )
                .subscribe((result) => {
                    this.earningTypesDtoArray = result.items;
                });
        } else {
            this._deductionTypesServiceProxy
                .getAll(
                    this.emptyStringFilter,
                    this.emptyNumberFilter,
                    this.emptyNumberFilter,
                    this.emptyStringFilter,
                    -1,
                    this.emptyStringFilter,
                    this.emptyDateFilter,
                    this.emptyDateFilter,
                    this.emptyStringFilter,
                    this.emptyDateFilter,
                    this.emptyDateFilter,
                    this.emptyStringFilter,
                    0,
                    500
                )
                .subscribe((result) => {
                    this.deductionTypesDtoArray = result.items;
                });
        }

        if (!adjHId) {
            // this.adjTypeDetails = new CreateOrEditEmployeeDeductionsDto();
            // this.adjTypeDetails.id = employeeDeductionsId;
            this.headerInfo = new AdjHDto();
            this.headerInfo.docdate = new Date();
            this.salaryYear = String(this.headerInfo.docdate.getFullYear());
            this.salaryMonth = String(this.headerInfo.docdate.getMonth() + 1);
            this.headerInfo.docType =
                this.docType == "earnings"
                    ? AdjustmentDocType.Earning
                    : AdjustmentDocType.Deduction;
            // this.deductionDate=moment().format("DD/MM/YYYY");
            // this.salaryYear=this.deductionDate.substr(6, 4);
            // this.salaryMonth=this.deductionDate.substr(3,2);
            //this.adjTypeDetails.active = true;

            // this._employeeDeductionsServiceProxy.getMaxDeductionId().subscribe(result => {
            //     this.adjTypeDetails. = result;
            // });

            this._adjHServiceProxy.getMaxDocID().subscribe((result) => {
                this.headerInfo.docID = result["result"];
            });

            this.active = true;
            this.modal.show();
        } else {
            this._adjHServiceProxy
                .getAdjHForEdit(adjHId)
                .subscribe((result) => {
                    debugger;
                    this.headerInfo = result.AdjHDto;
                    this.salaryMonth = (
                        "0" + String(this.headerInfo.salaryMonth)
                    ).slice(-2);
                    this.salaryYear = String(this.headerInfo.salaryYear);
                    this.remarks =
                        this.headerInfo.docType == AdjustmentDocType.Earning
                            ? this.headerInfo.adjDetails.earningDetail[0]
                                  .remarks
                            : this.headerInfo.adjDetails.deductionDetail[0]
                                  .remarks;
                    this.amount =
                        this.headerInfo.docType == AdjustmentDocType.Earning
                            ? this.headerInfo.adjDetails.earningDetail[0].amount
                            : this.headerInfo.adjDetails.deductionDetail[0]
                                  .amount;
                    this.deductionActive =
                        this.headerInfo.docType == AdjustmentDocType.Earning
                            ? this.headerInfo.adjDetails.earningDetail[0].active
                            : this.headerInfo.adjDetails.deductionDetail[0]
                                  .active;
                    this.active = true;
                    this.modal.show();
                });

            // this._employeeDeductionsServiceProxy
            //     .getEmployeeDeductionsForEdit(employeeDeductionsId)
            //     .subscribe((result) => {
            //         // this.adjTypeDetails = result.employeeDeductions;

            //         this.salaryMonth = (
            //             "0" + String(this.adjTypeDetails.salaryMonth)
            //         ).slice(-2);
            //         this.salaryYear = String(
            //             this.adjTypeDetails.salaryYear
            //         );
            //         //this.adjTypeDetails.deductionDate==null || undefined ? this.deductionDate="" : this.deductionDate=moment(this.adjTypeDetails.deductionDate).format("DD/MM/YYYY");

            //         this.active = true;
            //         this.modal.show();
            //     });
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
debugger
        this.saving = true;
        this.headerInfo.adjDetails = new AdjDetail();
        this.headerInfo.adjDetails.deductionDetail = [];
        this.headerInfo.adjDetails.earningDetail = [];
        this.headerInfo.createdBy = this.appSession.user.userName;
        this.headerInfo.createDate = new Date();
        this.headerInfo.audtUser = this.appSession.user.userName;
        this.headerInfo.audtDate = new Date();
        this.headerInfo.salaryYear = Number(this.salaryYear);
        this.headerInfo.salaryMonth = Number(this.salaryMonth);
    
        this.headerInfo.docType =
            this.docType == "earnings"
                ? AdjustmentDocType.Earning
                : AdjustmentDocType.Deduction;
        debugger;
        this.rowData.forEach((item) => {
            if (item.include) {
                this.adjTypeDetails =
                    this.headerInfo.docType == AdjustmentDocType.Earning
                        ? new CreateOrEditEmployeeEarningsDto()
                        : new CreateOrEditEmployeeDeductionsDto();
                if ( this.headerInfo.docType == AdjustmentDocType.Earning) {
                    this.adjTypeDetails.earningTypeID = this.headerInfo.typeID;
                } else {
                    this.adjTypeDetails.deductionType = this.headerInfo.typeID;
                }
                this.adjTypeDetails.amount = item.amount;
                this.adjTypeDetails.remarks = this.remarks;
                this.adjTypeDetails.employeeID = item.employeeID;
                this.adjTypeDetails.employeeName = item.employeeName;
                this.adjTypeDetails.salaryMonth = Number(this.salaryMonth);
                this.adjTypeDetails.salaryYear = Number(this.salaryYear);
                if ( this.headerInfo.docType == AdjustmentDocType.Earning) {
                    this.adjTypeDetails.earningDate = this.headerInfo.docdate;
                } else {
                    this.adjTypeDetails.deductionDate = this.headerInfo.docdate;
                }
                this.adjTypeDetails.createDate = this.headerInfo.createDate;
                this.adjTypeDetails.createdBy = this.headerInfo.createdBy;
                this.adjTypeDetails.audtDate = this.headerInfo.audtDate;
                this.adjTypeDetails.audtUser = this.headerInfo.audtUser;
                this.adjTypeDetails.active = this.deductionActive;
                if ( this.headerInfo.docType == AdjustmentDocType.Earning) {
                    this.headerInfo.adjDetails.earningDetail.push(
                        this.adjTypeDetails
                    );
                } else {
                    this.headerInfo.adjDetails.deductionDetail.push(
                        this.adjTypeDetails
                    );
                }
            }
        });

        this._adjHServiceProxy
            .createOrEdit(this.headerInfo)
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

        // this._employeeDeductionsServiceProxy.createOrEdit(this.employeeDe)
        //     .pipe(finalize(() => { this.saving = false; }))
        //     .subscribe(() => {
        //         this.notify.info(this.l('SavedSuccessfully'));
        //         this.close();
        //         this.modalSave.emit(null);
        //     });
    }

    onChange(newvalue) {
        this.salaryYear = String(newvalue.getFullYear());
        this.salaryMonth = ("0" + String(newvalue.getMonth() + 1)).slice(-2);

        this.headerInfo.salaryYear = Number(this.salaryYear);
        this.headerInfo.salaryMonth = Number(this.salaryMonth);
    }

    openEmployeeModal() {
        debugger;
        this.target = "Employee";
        this.PayRollLookupTableModal.id = String(
            this.adjTypeDetails.employeeID
        );
        this.PayRollLookupTableModal.displayName = this.adjTypeDetails.employeeName;
        this.PayRollLookupTableModal.show(this.target);
    }

    setEmployeeNull() {
        this.adjTypeDetails.employeeID = null;
        this.adjTypeDetails.employeeName = "";
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
        this.adjTypeDetails.employeeID = Number(
            this.PayRollLookupTableModal.id
        );
        this.adjTypeDetails.employeeName = this.PayRollLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ////GriD//////
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
            editable: true,
        },
        {
            headerName: this.l("Select"),
            field: "include",
            sortable: true,
            filter: false,
            width: 120,
            resizable: true,
            cellRendererFramework: CheckboxCellComponent,
        },
    ];

    onGridReady(params) {
        debugger;
        this.rowData = [];
        if (this.headerInfo.id) {
            this.rowData =
            this.headerInfo.docType == AdjustmentDocType.Earning
                    ? this.headerInfo.adjDetails.earningDetail
                    : this.headerInfo.adjDetails.deductionDetail;
            this.rowData.forEach((item) => (item.include = true));
        }
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        params.api.sizeColumnsToFit();
        this.rowSelection = "multiple";
    }

    onCellValueChanged(params) {
        debugger;
        this.gridApi.refreshCells();
    }

    getemployees() {
        this._employeeServiceProxy.getActiveEmployees().subscribe((result) => {
            debugger;

            this.activeEmployees = result["result"];
            this.rowData = this.activeEmployees;
            this.rowData.map((node) => {
                node.amount = this.amount;
            });
            this.gridApi.refreshCells();
        });
    }
}
