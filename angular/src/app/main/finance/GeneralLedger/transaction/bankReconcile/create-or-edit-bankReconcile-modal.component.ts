import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { AppComponentBase } from "@shared/common/app-component-base";
import * as moment from "moment";
import { AllCommunityModules, Module } from "@ag-grid-community/all-modules";

import { CreateOrEditGLReconHeaderDto } from "@app/main/finance/shared/dto/glReconHeader-dto";
import { CreateOrEditGLReconDetailsDto } from "@app/main/finance/shared/dto/glReconDetails-dto";

import { GetDataService } from "@app/main/supplyChain/inventory/shared/services/get-data.service";
import { GLReconHeadersService } from "@app/main/finance/shared/services/glReconHeader.service";
import { GLReconDetailsService } from "@app/main/finance/shared/services/glReconDetails.service";
import { BankReconcileServiceProxy } from "@app/main/finance/shared/services/bkReconcile.service";
import { finalize } from "rxjs/operators";
import { CommonServiceLookupTableModalComponent } from "@app/finders/commonService/commonService-lookup-table-modal.component";
import { CheckboxCellComponent } from "@app/shared/common/checkbox-cell/checkbox-cell.component";
import { DateEditorComponent } from "./date-editor.component";
import { DateRendererComponent } from "./date-renderer.component";
import { GLTRHeadersServiceProxy } from "@shared/service-proxies/service-proxies";
import { AgGridExtend } from "@app/shared/common/ag-grid-extend/ag-grid-extend";
import { LogComponent } from "@app/finders/log/log.component";
@Component({
    selector: "createOrEditBankReconcileModal",
    templateUrl: "./create-or-edit-bankReconcile-modal.component.html",
})
export class CreateOrEditBankReconcileModalComponent extends AppComponentBase {
    public modules: Module[] = AllCommunityModules;

    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild("bankfinderModal", { static: true })
    bankfinderModal: CommonServiceLookupTableModalComponent;
    @ViewChild('LogTableModal', { static: true }) LogTableModal: LogComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    //public modules: Module[];

    isUpdate: boolean;

    bsValue = new Date();
    active = false;
    saving = false;
    private components;
    bankAccountId: string;
    description = "";
    totalQty = 0;
    totalAmount = 0;
    private setParms;
    locations: any;
    LockDocDate: Date;
    ccDesc: string;
    glReconHeader: CreateOrEditGLReconHeaderDto = new CreateOrEditGLReconHeaderDto();
    gladjDetails: CreateOrEditGLReconDetailsDto = new CreateOrEditGLReconDetailsDto();
    agGridExtend: AgGridExtend = new AgGridExtend();

    auditTime: Date;
    docDate: any;
    processing = false;

    constructor(
        injector: Injector,
        private _glReconHeadersServiceProxy: GLReconHeadersService,
        private _glReconDetailsServiceProxy: GLReconDetailsService,
        private _bankReconcileServiceProxy: BankReconcileServiceProxy,
        private _getDataService: GetDataService,
        private _GLTRHeadersServiceProxy: GLTRHeadersServiceProxy
    ) {
        super(injector);
        this.components = { datePicker: getDatePicker() };
    }

    show(flag: boolean | undefined, reconcileHeaderId?: number): void {
        debugger;
        if (!flag) {
            this.glReconHeader = new CreateOrEditGLReconHeaderDto();
            this.glReconHeader.id = reconcileHeaderId;
            this.glReconHeader.flag = flag;
            this.active = true;
            this._bankReconcileServiceProxy.GetMaxDocNo().subscribe((data) => {
                debugger;
                this.glReconHeader.docNo = data["result"];
            });
            this.modal.show();
        } else {
            debugger;
            this._glReconHeadersServiceProxy
                .getGLReconHeaderForEdit(reconcileHeaderId)
                .subscribe((result) => {
                    debugger;
                    this.glReconHeader = result.gLReconHeader;
                    this.rowData = result.gLReconHeader.bankReconcileDetail;
                    this.bsValue.setFullYear(
                        this.glReconHeader.docDate.year(),
                        this.glReconHeader.docDate.month(),
                        this.glReconHeader.docDate.date()
                    );
                    this.glReconHeader.flag = flag;
                    this.isUpdate = flag;
                    this.active = true;
                    this.modal.show();
                });
        }
    }
    OpenLog(){
        debugger
       this.LogTableModal.show(this.glReconHeader.docNo,'BankReconcile');
    }
    //==================================Grid=================================
    private gridApi;
    private gridColumnApi;
    private rowData: any;
    private rowSelection;
    columnDefs = [
        {
            headerName: this.l("Config"),
            field: "configID",
            sortable: true,
            width: 50,
        },
        {
            headerName: this.l("Voucher Id"),
            field: "fmtDocNo",
            sortable: true,
            width: 80,
            editable: false,
            resizable: true,
        },
        {
            headerName: this.l("VoucherDate"),
            field: "voucherDate",
            sortable: true,
            width: 100,
            resizable: true,
            cellRendererFramework: DateRendererComponent,
        },
        {
            headerName: this.l("Clearing Date"),
            field: "clearingDate",
            editable: true,
            width: 80,
            cellEditorFramework: DateEditorComponent,
            cellRendererFramework: DateRendererComponent,
        },
        {
            headerName: this.l("InstrumentNo"),
            field: "chNumber",
            sortable: true,
            width: 50,
            editable: false,
        },
        {
            headerName: this.l("Dr"),
            field: "Dr",
            width: 80,
            type: "numericColumn",
            resizable: true,
            valueFormatter: this.agGridExtend.formatNumber,
        },
        {
            headerName: this.l("Cr"),
            field: "Cr",
            width: 80,
            type: "numericColumn",
            resizable: true,
            valueFormatter: this.agGridExtend.formatNumber,
        },
        {
            headerName: this.l("Reconcile"),
            field: "include",
            width: 70,
            cellRendererFramework: CheckboxCellComponent,
        },
        {
            headerName: this.l("GlDetID"),
            field: "glDetID",
            hide: true,
            width: 20,
        },
    ];

    headerCheckboxSelection(params) {
        debugger;
        if (params.target.checked) {
            var apo = this.gridApi;
            this.gridApi.forEachNode((element) => {
                element.data.include = true;
                debugger;
            });
        } else {
            this.gridApi.forEachNode((element) => {
                element.data.include = false;
                debugger;
            });
        }
        this.gridApi.refreshCells();
        this.calculations();
    }

    datePickerCellRendererFunc(params) {
        debugger;
        return '<input type="date">';
    }

    onGridReady(params) {
        debugger;
        this.rowData = [];
        if (this.isUpdate) {
            this.rowData = this.glReconHeader.bankReconcileDetail;
        }
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        params.api.sizeColumnsToFit();
        this.rowSelection = "single";
    }

    onAddRow(): void {
        debugger;
        if (this.glReconHeader.bankID) {
            this._glReconDetailsServiceProxy
                .GetListOFdetail(
                    this.glReconHeader.bankID,
                    moment(this.bsValue).format("YYYY/MM/DD")
                )
                .subscribe((res) => {
                    debugger;
                    if (this.isUpdate) {
                        var newItems = res.items;
                        this.rowData = res.items;
                        //this.gridApi.updateRowData({ add: newItems });
                    } else {
                        this.rowData = res.items;
                    }

                    this.glReconHeader.unClDepAmt = 0;
                    this.glReconHeader.unClPayAmt = 0;
                    this.glReconHeader.clDepAmt = 0;
                    this.glReconHeader.clPayAmt = 0;
                    this.glReconHeader.unClItems = 0;
                    this.glReconHeader.clItems = 0;
                    this.rowData.forEach((element) => {
                        this.glReconHeader.unClDepAmt += parseFloat(element.Dr);
                        this.glReconHeader.unClPayAmt += parseFloat(element.Cr);
                        element.Cr = element.Cr.toFixed(2);
                        element.Dr = element.Dr.toFixed(2);
                    });
                    this.glReconHeader.unClPayAmt = Number(
                        this.glReconHeader.unClPayAmt.toFixed(2)
                    );
                    this.glReconHeader.unClDepAmt = Number(
                        this.glReconHeader.unClDepAmt.toFixed(2)
                    );
                    this.glReconHeader.unClItems = this.rowData.length;
                });
        } else {
            abp.message.info("Please Enter Bank First", "Missing");
        }
        // debugger;
        this.gridApi.refreshCells();
    }

    onBtStartEditing(index, col) {
        debugger;
        this.gridApi.setFocusedCell(index, col);
        this.gridApi.startEditingCell({
            rowIndex: index,
            colKey: col,
        });
    }

    onSelectionChanged() {
        debugger;
        var selectedRows = this.gridApi.getSelectedRows();
        this.calculations();
    }

    onRemoveSelected() {
        debugger;
        var selectedData = this.gridApi.getSelectedRows();
        this.gridApi.updateRowData({ remove: selectedData });
        this.gridApi.refreshCells();
        this.calculations();
    }

    calculations() {
        debugger;
        var amt = 0;
        this.glReconHeader.unClItems = 0;
        this.glReconHeader.unClDepAmt = 0;
        this.glReconHeader.unClPayAmt = 0;
        this.glReconHeader.clItems = 0;
        this.glReconHeader.clDepAmt = 0;
        this.glReconHeader.clPayAmt = 0;
        this.glReconHeader.endBalance = 0;
        this.gridApi.forEachNode((node) => {
            if (node.data.include) {
                this.glReconHeader.clPayAmt += parseFloat(node.data.Cr);
                this.glReconHeader.clDepAmt += parseFloat(node.data.Dr);
                this.glReconHeader.clItems += 1;
                amt += parseFloat(node.data.Dr) - parseFloat(node.data.Cr);
            } else {
                this.glReconHeader.unClPayAmt += parseFloat(node.data.Cr);
                this.glReconHeader.unClDepAmt += parseFloat(node.data.Dr);
                this.glReconHeader.unClItems += 1;
            }
        });
        debugger;
        this.glReconHeader.reconcileAmt = Number(amt.toFixed(2)); // Math.abs(Number(amt.toFixed(2)));
        this.glReconHeader.statementAmt =
            this.glReconHeader.reconcileAmt + this.glReconHeader.beginBalance;
        this.glReconHeader.statementAmt = Number(
            this.glReconHeader.statementAmt.toFixed(2)
        );
        this.glReconHeader.clPayAmt = Number(
            this.glReconHeader.clPayAmt.toFixed(2)
        );
        this.glReconHeader.clDepAmt = Number(
            this.glReconHeader.clDepAmt.toFixed(2)
        );
        this.glReconHeader.unClPayAmt = Number(
            this.glReconHeader.unClPayAmt.toFixed(2)
        );
        this.glReconHeader.unClDepAmt = Number(
            this.glReconHeader.unClDepAmt.toFixed(2)
        );
        this.calculateDiff(null);
    }

    calculateDiff(params) {
        debugger;

        if (
            this.glReconHeader.endBalance != null &&
            this.glReconHeader.reconcileAmt
        ) {
            this.glReconHeader.statementAmt = Number(
                (
                    this.glReconHeader.beginBalance +
                    this.glReconHeader.reconcileAmt
                ).toFixed(2)
            );
            this.glReconHeader.diffAmount = Number(
                (
                    this.glReconHeader.endBalance -
                    this.glReconHeader.statementAmt
                ).toFixed(2)
            );
        } else {
            this.glReconHeader.diffAmount = 0;
        }
    }

    onCellValueChanged(params) {
        debugger;
        this.calculations();
    }

    onCellClicked(params) {
        debugger;
        if (params.column.colId == "include") {
            this.calculations();
        } else {
        }
    }

    onCellEditingStarted(params) {
        debugger;
    }

    //==================================Grid=================================

    save(): void {
        // if (
        //     moment(this.LockDocDate).month() + 1 !=
        //     moment(this.glReconHeader.docDate).month() + 1 &&
        //     this.glReconHeader.id != null
        // ) {
        //     this.notify.error(this.l("Document month not changeable"));
        //     return;
        // }

        if (this.glReconHeader.bankID == null) {
            this.notify.error(this.l("Please select Bank ID"));
            return;
        }
        this.bsValue.setDate(this.bsValue.getDate());
        this.glReconHeader.docDate = moment(this.bsValue);

        if (this.glReconHeader.docDate == null) {
            this.notify.error(this.l("Please select Reconcile Date"));
            return;
        }

        var count = this.gridApi.getDisplayedRowCount();

        if (count == 0) {
            this.notify.error(this.l("Please Enter Grid Data"));
            return;
        }
        var rowData = [];

        this.gridApi.forEachNode((node) => {
            //if (node.data.include) {
            rowData.push(node.data);
            // }
        });

        this.glReconHeader.audtDate = moment();
        this.glReconHeader.audtUser = this.appSession.user.userName;
        if (!this.glReconHeader.id) {
            this.glReconHeader.createdDate = moment();
            this.glReconHeader.createdBy = this.appSession.user.userName;
        }

        let checkClearDate = false;
        rowData.forEach(function (value) {
            debugger;
            if (value.include == true && value.clearingDate == null) {
                checkClearDate = true;
            }
        });

        if (checkClearDate) {
            debugger;
            this.notify.error(
                this.l("Please Enter Clearing Date for included vouchers.")
            );
            return;
        }

        this.saving = true;

        this.glReconHeader.bankReconcileDetail = rowData;
        this.glReconHeader.docID =
            this.glReconHeader.bankID +
            "-" +
            moment(this.bsValue).format("YYYYMMDD");
        debugger;

        this._bankReconcileServiceProxy
            .CreateOrEditBkReconcile(this.glReconHeader)
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

    approveBnkReconcile(Id: number, approve) {
        debugger;
        this.message.confirm("", (isConfirmed) => {
            if (isConfirmed) {
                this._bankReconcileServiceProxy
                    .ApproveBankReconcile(Id, approve)
                    .subscribe(() => {
                        if (approve == true) {
                            this.notify.success(this.l("SuccessfullyApproved"));
                            this.close();
                            this.modalSave.emit(null);
                        } else {
                            this.notify.success(
                                this.l("SuccessfullyUnApproved")
                            );
                            this.close();
                            this.modalSave.emit(null);
                        }
                    });
            }
        });
    }

    //=====================Item Model================
    openBankModal() {
        debugger;
        this.bankfinderModal.id = null;
        this.bankfinderModal.displayName = "";
        this.bankfinderModal.mode = "reconcilation";
        this.bankfinderModal.show("Bank");
    }

    setBankIdNull() {
        this.glReconHeader.bankID = null;
        this.glReconHeader.bankName = "";
    }

    getNewbank() {
        debugger;
        this.glReconHeader.bankID = this.bankfinderModal.id;
        this.glReconHeader.bankName = this.bankfinderModal.displayName;
        this.bankAccountId = this.bankfinderModal.accountId;
        if (this.glReconHeader.bankID) {
            this.getClosingBalance(null);
        }
    }

    getClosingBalance(params) {
        // this._GLTRHeadersServiceProxy
        //     .closingBalance(this.bankAccountId, moment(this.bsValue))
        //     .subscribe(res => {
        this._glReconHeadersServiceProxy
            .getBeginningBal(this.glReconHeader.bankID)
            .subscribe((res) => {
                this.glReconHeader.beginBalance = res;
                this.glReconHeader.reconcileAmt = 0;
                this.glReconHeader.statementAmt = this.glReconHeader.beginBalance;
                this.glReconHeader.statementAmt = Number(
                    this.glReconHeader.statementAmt.toFixed(2)
                );
                this.glReconHeader.diffAmount = 0;
                this.glReconHeader.unClDepAmt = 0;
                this.glReconHeader.unClPayAmt = 0;
                this.glReconHeader.clDepAmt = 0;
                this.glReconHeader.clPayAmt = 0;
                this.glReconHeader.unClItems = 0;
                this.glReconHeader.clItems = 0;
                this.glReconHeader.endBalance=0;
            });
        //     debugger;
        //     console.log(res);
        // });
        this.calculations();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}

function getDatePicker() {
    function Datepicker() {}
    Datepicker.prototype.init = function (params) {
        this.eInput = document.createElement("input");
        this.eInput.value = params.value;
        this.$(this.eInput).datepicker({ dateFormat: "dd/mm/yy" });
    };
    Datepicker.prototype.getGui = function () {
        return this.eInput;
    };
    Datepicker.prototype.afterGuiAttached = function () {
        this.eInput.focus();
        this.eInput.select();
    };
    Datepicker.prototype.getValue = function () {
        return this.eInput.value;
    };
    Datepicker.prototype.destroy = function () {};
    Datepicker.prototype.isPopup = function () {
        return false;
    };
    return Datepicker;
}
