import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    HostListener,
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { finalize } from "rxjs/operators";
import {
    GLTRHeadersServiceProxy,
    GLTRDetailsServiceProxy,
    VoucherEntryServiceProxy,
    AccountsPostingsServiceProxy,
    CreateOrEditGLTRHeaderDto,
    CreateOrEditGLTRDetailDto,
    VoucherEntryDto,
    throwException,
    FiscalCalendersServiceProxy,
    GLTRHeaderDto,
} from "@shared/service-proxies/service-proxies";
import { AppComponentBase } from "@shared/common/app-component-base";
import * as moment from "moment";
import { FinanceLookupTableModalComponent } from "@app/finders/finance/finance-lookup-table-modal.component";
import { AgGridExtend } from "@app/shared/common/ag-grid-extend/ag-grid-extend";
import { Lightbox } from "ngx-lightbox";
import { AppConsts } from "@shared/AppConsts";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";
import { debug } from "console";
import { LogComponent } from "@app/finders/log/log.component";

@Component({
    selector: "createOrEditVoucherEntryModal",
    templateUrl: "./create-or-edit-voucherEntry-modal.component.html",
})
export class CreateOrEditVoucherEntryModalComponent extends AppComponentBase {
    @ViewChild("reportviewrModalComponent", { static: false })
    reportviewrModalComponent: ReportviewrModalComponent;
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild("FinanceLookupTableModal", { static: true })
    FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('LogTableModal', { static: true }) LogTableModal: LogComponent;

    active = false;
    saving = false;
    DocYear = 0;
    accountID = "";
    accountDesc = "";
    subAccID = "";
    subAccDesc = "";
    totalCredit = 0;
    totalDebit = 0;
    totalBalance = 0;
    BookNameShow = "";
    private setParms;
    NormalEntry = 0;
    private glLocationList;
    accountDescB = "";
    LockDocDate: Date;
    isIntegrated = false;
    curid: string;
    curRate: number;
    //chequeNo:any;
    dateValid: boolean = false;
    url: string;
    uploadUrl: string;
    uploadedFiles: any[] = [];
    checkImage: boolean = false;
    image: any[] = [];
    instrumentNoChk: boolean = false;
    readonly voucherEntryAppId = 5;
    readonly appName = "VoucherEntry";
    overDraftLimit: number;
    overDraftLimitApplies: boolean;
    overDraftLimitExceeds: boolean;

    gltrHeader: CreateOrEditGLTRHeaderDto = new CreateOrEditGLTRHeaderDto();
    gltrDetail: CreateOrEditGLTRDetailDto = new CreateOrEditGLTRDetailDto();
    voucherEntry: VoucherEntryDto = new VoucherEntryDto();
    agGridExtend: AgGridExtend = new AgGridExtend();

    auditTime: Date;
    docDate: Date;
    accountIdC = "";
    target: any;
    errorFlag: boolean = false;
    sortKeys: string[] = [];
    editMode: boolean = false;
    @ViewChild("voucherEntryForm", { static: false }) voucherEntryForm: any;

    constructor(
        injector: Injector,
        private _gltrHeadersServiceProxy: GLTRHeadersServiceProxy,
        private _gltrDetailsServiceProxy: GLTRDetailsServiceProxy,
        private _voucherEntryServiceProxy: VoucherEntryServiceProxy,
        private _accountsPostingsServiceProxy: AccountsPostingsServiceProxy,
        private _lightbox: Lightbox,
        private _fiscalCalendarsServiceProxy: FiscalCalendersServiceProxy
    ) {
        super(injector);
    }

    checkOverDraftLimit() {
        if (
            this.gltrHeader.bookID == "BP" &&
            this.overDraftLimitApplies == true
        ) {
            this._voucherEntryServiceProxy
                .GetOverDraftLimit(
                    this.accountIdC,
                    this.gltrHeader.id > 0 ? this.gltrHeader.docNo : 0,
                    this.gltrHeader.bookID
                )
                .subscribe((data) => {
                    this.overDraftLimit = data["result"];
                    if (this.overDraftLimit == 0)
                        this.overDraftLimitExceeds = true;
                    else if (this.overDraftLimit < this.totalBalance)
                        this.overDraftLimitExceeds = true;
                    else this.overDraftLimitExceeds = false;
                });
        }
    }
    OpenLog(){
        debugger
       this.LogTableModal.show(this.gltrHeader.fmtDocNo,this.gltrHeader.bookID);
    }
    show(
        gltrHeaderId?: number,
        maxId?: number,
        fmtDocNo?: number,
        cbookId?: string,
        bookName?: string
    ): void {
        this.auditTime = null;
        this.url = null;
        this.uploadUrl = null;
        this.image = [];
        this.uploadedFiles = [];
        this.checkImage = true;
        debugger;

        this._voucherEntryServiceProxy.getGLLocData().subscribe((resultL) => {
            debugger;
            this.glLocationList = resultL;
        });
        this._gltrHeadersServiceProxy
            .getInstrumentNoChk()
            .subscribe((resultL) => {
                this.instrumentNoChk = resultL["result"];
            });



        if (!gltrHeaderId) {
            debugger;
            this.gltrHeader = new CreateOrEditGLTRHeaderDto();
            this.gltrHeader.id = gltrHeaderId;
            this.gltrHeader.docDate = moment().endOf("day");
            this.gltrHeader.docMonth = moment().month() + 1;
            this.DocYear = moment().year();
            this.gltrHeader.docNo = maxId;
            this.gltrHeader.bookID = cbookId;
            this.gltrHeader.locId = 1;
            this.BookNameShow = bookName;
            this.accountIdC = "";
            this.accountDescB = "";
            this.totalCredit = 0;
            this.totalDebit = 0;
            this.totalBalance = 0;
            this.gltrHeader.chType = 1;
            this.gltrHeader.fmtDocNo = fmtDocNo;
            this.overDraftLimit = 0;
            this.getDateParams(null, null);
            this.shByNormalEntry(cbookId);
            this.gltrHeader.bookID == "BP"
                ? (this.overDraftLimitExceeds = true)
                : (this.overDraftLimitExceeds = false);
            this.editMode = false;
            this.active = true;
            this.modal.show();
        } else {
            this._gltrHeadersServiceProxy
                .getGLTRHeaderForEdit(gltrHeaderId)
                .subscribe((result) => {
                    debugger;
                    this.editMode = true;
                    this.gltrHeader = result.gltrHeader;

                    this.gltrHeader.bookID == "BP"
                        ? (this.overDraftLimitExceeds = true)
                        : (this.overDraftLimitExceeds = false);

                    this._gltrHeadersServiceProxy
                        .getImage(
                            this.voucherEntryAppId,
                            result.gltrHeader.docNo
                        )
                        .subscribe((fileResult) => {
                            debugger;
                            if (fileResult != null) {
                                this.url =
                                    "data:image/jpeg;base64," + fileResult;
                                const album = {
                                    src: this.url,
                                };
                                this.image.push(album);
                                this.checkImage = false;
                            }
                        });

                    if (this.gltrHeader.auditTime) {
                        this.auditTime = this.gltrHeader.auditTime.toDate();
                    }

                    this.LockDocDate = this.gltrHeader.docDate.toDate();

                    this.DocYear = moment(this.gltrHeader.docDate).year();

                    this.shByNormalEntry(result.gltrHeader.bookID);

                    this.BookNameShow = "";
                    this.BookNameShow =
                        "Edit Voucher Entry (" + this.gltrHeader.bookID + ")";

                    this._gltrDetailsServiceProxy
                        .filterGLTRDData(gltrHeaderId)
                        .subscribe((resultD) => {
                            debugger;
                            var rData = [];
                            var totalSDebit = 0;
                            var totalSCredit = 0;
                            resultD["items"].forEach((element) => {
                                // if(element.gltrDetail.accountID){
                                //     this._voucherEntryServiceProxy.getAccountDesc(element.gltrDetail.accountID).subscribe(resultAC => {
                                //         debugger;
                                //         if(resultAC){
                                //             element.gltrDetail['accountDesc']=resultAC;
                                //         }
                                //     });
                                // }
                                // if(element.gltrDetail.subAccID){
                                //     this._voucherEntryServiceProxy.getSubledgerDesc(element.gltrDetail.accountID,element.gltrDetail.subAccID).subscribe(resultSA => {
                                //         debugger;
                                //         if(resultSA){
                                //             element.gltrDetail['subAccDesc']=resultSA;
                                //         }
                                //     });
                                // }
                                if (element.gltrDetail.isAuto == false) {
                                    debugger;
                                    rData.push(element.gltrDetail);
                                    // switch (this.NormalEntry) {
                                    //     case 1:
                                    //         element.gltrDetail['credit']=Math.abs(element.gltrDetail.amount);
                                    //         element.gltrDetail['debit']=0;
                                    //         break;
                                    //     case 2:
                                    //         element.gltrDetail['debit']=element.gltrDetail.amount;
                                    //         element.gltrDetail['credit']=0;
                                    //         break;
                                    //     default:
                                    if (element.gltrDetail.amount < 0) {
                                        element.gltrDetail["credit"] = Math.abs(
                                            element.gltrDetail.amount
                                        );
                                        element.gltrDetail["debit"] = 0;
                                    } else {
                                        element.gltrDetail["debit"] =
                                            element.gltrDetail.amount;
                                        element.gltrDetail["credit"] = 0;
                                    }
                                    // break;
                                    // }

                                    totalSDebit += parseFloat(
                                        element.gltrDetail["debit"]
                                    );
                                    totalSCredit += parseFloat(
                                        element.gltrDetail["credit"]
                                    );
                                } else {
                                    debugger;
                                    this.accountIdC =
                                        element.gltrDetail.accountID;
                                    if (this.gltrHeader.bookID == "BP") {
                                        this._voucherEntryServiceProxy
                                            .GetWeatherBankTypeISOverDraftOrNot(
                                                this.accountIdC
                                            )
                                            .subscribe((result) => {
                                                this.overDraftLimitApplies =
                                                    result["result"];
                                                if (
                                                    this
                                                        .overDraftLimitApplies ==
                                                    true
                                                )
                                                    this.checkOverDraftLimit();
                                                else {
                                                    this.overDraftLimitExceeds = false;
                                                    this.overDraftLimit = 0;
                                                }
                                            });
                                    }
                                    this._voucherEntryServiceProxy
                                        .getAccountDesc(
                                            element.gltrDetail.accountID
                                        )
                                        .subscribe((resultAC) => {
                                            debugger;
                                            if (resultAC) {
                                                this.accountDescB = resultAC;
                                            }
                                        });
                                }
                            });

                            this.rowData = [];
                            this.rowData = rData;

                            this.totalDebit = totalSDebit;
                            this.totalCredit = totalSCredit;
                            this.totalBalance = Math.abs(
                                parseFloat(
                                    (totalSDebit - totalSCredit).toFixed(2)
                                )
                            );
                        });

                    this.active = true;
                    debugger;
                    this.modal.show();
                    this.getDateParams(null, true);
                });
        }
    }
    onOptionsSelected(event){
        debugger
        const value = event.target.value;
       // this.selected = value;
       if(value==4 || value==5){
         this.gltrHeader.chNumber="-";
       }else{
        this.gltrHeader.chNumber="";
       }
      
   }
    postVoucher(vocherId: number, posted) {
        debugger;
        this.message.confirm("", (isConfirmed) => {
            if (isConfirmed) {
                this._accountsPostingsServiceProxy
                    .postingData([vocherId], "AccountsPosting", posted)
                    .subscribe(() => {
                        this.notify.success(this.l("SuccessfullyPosted"));

                        this.close();
                        this.modalSave.emit(null);
                    });
            }
        });
    }

    approveVoucher(vocherId: number, approve) {
        debugger;
        this.message.confirm("", (isConfirmed) => {
            if (isConfirmed) {
                this._accountsPostingsServiceProxy
                    .postingData([vocherId], "AccountsApproval", approve)
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

    shByNormalEntry(bookId: string) {
        this._voucherEntryServiceProxy
            .getBookNormalEntry(bookId)
            .subscribe((result) => {
                debugger;
                this.NormalEntry = result.normalEntry;
                this.isIntegrated = result.integrated;
                if (result.normalEntry == 0) {
                    this.message.warn(
                        bookId + " book not found",
                        "Book Required"
                    );
                    return;
                }
                if (result.normalEntry == 1) {
                    // this.gridColumnApi.setColumnVisible("debit", false);
                    //this.gridApi.sizeColumnsToFit();
                    this.gridColumnApi.getColumn(
                        "debit"
                    ).colDef.editable = false;
                    this.gridApi.refreshCells();
                }
                if (result.normalEntry == 2) {
                    // this.gridColumnApi.setColumnVisible("credit", false);
                    // this.gridApi.sizeColumnsToFit();
                    this.gridColumnApi.getColumn(
                        "credit"
                    ).colDef.editable = false;
                    this.gridApi.refreshCells();
                }
            });
    }

    //==================================Grid=================================
    private gridApi;
    private gridColumnApi;
    private rowData;
    private rowSelection;
    columnDefs = [
        {
            headerName: this.l("SrNo"),
            field: "srNo",
            sortable: true,
            width: 50,
            valueGetter: "node.rowIndex+1",
        },
        {
            headerName: this.l("AccountID"),
            field: "accountID",
            sortable: true,
            filter: true,
            width: 100,
            editable: false,
            resizable: true,
        },
        {
            headerName: this.l(""),
            field: "addAccId",
            width: 15,
            editable: false,
            cellRenderer: this.accIdCellRendererFunc,
            resizable: false,
        },
        {
            headerName: this.l("AccountDesc"),
            field: "accountDesc",
            sortable: true,
            filter: true,
            width: 200,
            resizable: true,
        },
        {
            headerName: this.l("Subledger"),
            field: "subAccID",
            sortable: true,
            filter: true,
            width: 80,
            editable: false,
            resizable: true,
        },
        {
            headerName: this.l(""),
            field: "addsubAccId",
            width: 15,
            editable: false,
            cellRenderer: this.subAccIDCellRendererFunc,
            resizable: false,
        },
        {
            headerName: this.l("IsSL"),
            field: "isSL",
            width: 20,
            resizable: true,
            hide: true,
        },
        {
            headerName: this.l("SubledgerDesc"),
            field: "subAccDesc",
            sortable: true,
            filter: true,
            width: 150,
            resizable: true,
        },
        {
            headerName: this.l("Narration"),
            field: "narration",
            editable: true,
            resizable: true,
        },
        {
            headerName: this.l("ChequeNo"),
            field: "chequeNo",
            sortable: true,
            width: 150,
            editable: true,
            resizable: true,

        },
        {
            headerName: this.l("Debit"),
            field: "debit",
            sortable: true,
            width: 100,
            editable: true,
            type: "numericColumn",
            resizable: true,
            valueFormatter: this.agGridExtend.formatNumber,
        },
        {
            headerName: this.l("Credit"),
            field: "credit",
            sortable: true,
            width: 100,
            editable: true,
            type: "numericColumn",
            resizable: true,
            valueFormatter: this.agGridExtend.formatNumber,
        },
    ];

    onGridReady(params) {
        debugger;
        if (this.gltrHeader.bookID == "BR" || this.gltrHeader.bookID == "BP") {
            params.columnApi.setColumnVisible('chequeNo', true)
        }
        else {
            params.columnApi.setColumnVisible('chequeNo', false)
        }
        this.rowData = [];
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        params.api.sizeColumnsToFit();


        this.rowSelection = "multiple";
    }

    onAddRow(): void {
        debugger;
        var index = this.gridApi.getDisplayedRowCount();
        var newItem = this.createNewRowData();
        this.gridApi.updateRowData({ add: [newItem] });
        this.gridApi.refreshCells();
        this.onBtStartEditing(index, "addAccId");
    }

    subAccIDCellRendererFunc(params) {
        debugger;
        return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" (click)="openSelectGLSubledgerModal(params)"></i>';
    }

    accIdCellRendererFunc(params) {
        debugger;
        return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" (click)="openSelectChartofControlModal(params)"></i>';
    }

    onBtStartEditing(index, col) {
        debugger;
        this.gridApi.setFocusedCell(index, col);
        this.gridApi.startEditingCell({
            rowIndex: index,
            colKey: col,
        });
    }

    onRemoveSelected() {
        debugger;
        var selectedData = this.gridApi.getSelectedRows();
        this.gridApi.updateRowData({ remove: selectedData });
        this.gridApi.refreshCells();
        this.calculations();
    }

    createNewRowData() {
        debugger;
        var newData = {
            accountID: "",
            subAccID: "0",
            narration: this.gltrHeader.narration,
            chequeNo: this.gltrHeader.chNumber,
            debit: "0",
            credit: "0",
            isSL: false,
        };
        return newData;
    }

    calculations() {
        debugger;
        var totalSDebit = 0;
        var totalSCredit = 0;
        this.gridApi.forEachNode((node) => {
            debugger;
            if (node.data.debit != "" || node.data.credit != "") {
                totalSDebit += parseFloat(node.data.debit);
                totalSCredit += parseFloat(node.data.credit);
            }
        });
        this.totalDebit = totalSDebit;
        this.totalCredit = totalSCredit;
        this.totalBalance = Math.abs(
            parseFloat((totalSDebit - totalSCredit).toFixed(2))
        );
    }

    cellClicked(params) {
        debugger;
        if (params.column["colId"] == "addAccId") {
            this.setParms = params;
            this.openSelectChartofControlModal();
        }
        if (params.column["colId"] == "addsubAccId") {
            this.setParms = params;
            this.openSelectGLSubledgerModal();
        }
    }

    onCellValueChanged(params) {
        debugger;
        if (params.column["colId"] === "debit") {
            if (
                (parseFloat(params.data.credit) > 0 ||
                    parseFloat(params.data.credit) !== NaN) &&
                parseFloat(params.data.debit) > 0
            ) {
                params.data.credit = 0;
            }
        } else if (params.column["colId"] === "credit") {
            if (
                (parseFloat(params.data.debit) > 0 ||
                    parseFloat(params.data.debit) !== NaN) &&
                parseFloat(params.data.credit) > 0
            ) {
                params.data.debit = 0;
            }
        }
        this.calculations();
        this.gridApi.refreshCells();
        if (this.gltrHeader.bookID == "BP") this.checkOverDraftLimit();
        else {
            this.overDraftLimitExceeds = false;
            this.overDraftLimit = 0;
        }
    }

    //==================================Grid=================================

    saveAndNew(): void {
        debugger;
        this.errorFlag = false;
        // this.message.confirm(
        // 	'Save Voucher',
        // 	(isConfirmed) => {
        // 		if (isConfirmed) {

        if (moment(this.gltrHeader.docDate) > moment().endOf("day")) {
            this.message.warn(
                "Voucher date greater than current date",
                "Voucher Date Greater"
            );
            return;
        }

        if (
            moment(this.LockDocDate).month() + 1 != this.gltrHeader.docMonth &&
            this.gltrHeader.id != null
        ) {
            this.message.warn(
                "Document month not changeable",
                "Document Month Error"
            );
            return;
        }

        if (
            moment(this.gltrHeader.docDate).month() + 1 !=
            this.gltrHeader.docMonth
        ) {
            this.message.warn(
                this.l("Document month not according to documrnt date"),
                "Document Month Error"
            );
            return;
        }

        if (
            this.gltrHeader.locId == null ||
            this.gltrHeader.locId.toString() == "0"
        ) {
            this.message.warn(
                this.l("Please select location"),
                "Location required"
            );
            return;
        }

        if (this.gridApi.getDisplayedRowCount() <= 0) {
            this.message.warn("No details found", "Details Required");
            return;
        }

        // this.gridApi.forEachNode(node => {
        //     debugger
        //     if (node.data.isSL && (node.data.subAccID=="" || node.data.subAccID==0)) {
        //         this.message.warn("Subledger Account not found at row " + Number(node.rowIndex + 1), "Subledger Required");
        //         this.errorFlag = true;
        //         return;
        //     } else {
        //         this.errorFlag = false;
        //     }
        // });

        // if (this.errorFlag) {
        //     return;
        // }

        this.gridApi.forEachNode((node) => {
            debugger;
            if (
                node.data.isSL &&
                (node.data.subAccID == "" || node.data.subAccID == 0)
            ) {
                this.message.warn(
                    "Subledger Account not found at row " +
                    Number(node.rowIndex + 1)
                );
                //, "Subledger Required");
                this.errorFlag = true;
                //return;
            }
            // else {
            //     this.errorFlag = false;
            // }
        });

        if (this.errorFlag) {
            debugger;
            return;
        }

        if (this.totalBalance == 0) {
            this.message.warn(
                this.l("Balance amount not greater than zero"),
                "Balance Amount Zero"
            );
            return;
        }

        if (this.NormalEntry == 3) {
            var total =
                this.totalBalance -
                Math.abs(this.totalDebit - this.totalCredit);
            if (total != 0) {
                this.message.warn(
                    this.l("Credit and debit amount not equal"),
                    "Credit/Debit Equal"
                );
                return;
            }
        }

        this.saving = true;

        var rowData = [];
        this.gridApi.forEachNode((node) => {
            if (this.gltrHeader.bookID == "JV") {
                if (node.data.credit == 0 && node.data.debit != 0) {
                    rowData.push(node.data);
                    node.data["amount"] = node.data.debit;
                }
                if (node.data.debit == 0 && node.data.credit != 0) {
                    rowData.push(node.data);
                    node.data["amount"] = -node.data.credit;
                }
            } else {
                if (node.data.credit == 0 && node.data.debit != 0) {
                    rowData.push(node.data);
                    node.data["amount"] = node.data.debit;
                }
                if (node.data.debit == 0 && node.data.credit != 0) {
                    rowData.push(node.data);
                    node.data["amount"] = -node.data.credit;
                }
            }
        });

        var amountType = "";
        var amount = 0;
        if (this.totalDebit > 0) {
            amountType = this.gltrHeader.narration;
            amount = -this.totalBalance;
        }
        if (this.totalCredit > 0) {
            amountType = this.gltrHeader.narration;
            amount = this.totalBalance;
        }

        if (this.gltrHeader.bookID != "JV") {
            rowData.push({
                accountID: this.accountIdC,
                subAccID: "0",
                narration: amountType,
                chequeNo: "",
                isAuto: true,
                amount: amount,
            });
        }

        this.voucherEntry.gltrDetail = rowData;
        this.voucherEntry.gltrHeader = this.gltrHeader;

        this.gltrHeader.auditTime = moment();
        this.gltrHeader.auditUser = this.appSession.user.userName;

        if (!this.gltrHeader.id) {
            this.gltrHeader.createdOn = moment();
            this.gltrHeader.createdBy = this.appSession.user.userName;
        }

        if (
            moment(new Date()).format("A") === "AM" &&
            !this.gltrHeader.id &&
            moment(new Date()).month() + 1 == this.gltrHeader.docMonth
        ) {
            this.gltrHeader.docDate = moment(this.gltrHeader.docDate);
        } else {
            this.gltrHeader.docDate = moment(this.gltrHeader.docDate).endOf(
                "day"
            );
        }

        this.gltrHeader.amount = this.totalBalance;

        debugger;

        this._voucherEntryServiceProxy
            .createOrEditVoucherEntry(this.voucherEntry)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                debugger;
                //this.notify.info(this.l('SavedSuccessfully'));
                //this.message.confirm("Press 'Yes' for create new voucher", this.l('SavedSuccessfully'), (isConfirmed) => {
                //if (isConfirmed) {
                // this.gltrHeader.docNo = Number(this.gltrHeader.docNo) + 1;
                if (this.editMode == false) {
                    this._voucherEntryServiceProxy
                        .getMaxDocId(
                            this.gltrHeader.bookID,
                            true,
                            moment(this.gltrHeader.docDate).format("LLLL")
                        )
                        .subscribe((result) => {
                            this.gltrHeader.fmtDocNo = result;
                        });
                    this.rowData = [];
                    this.modalSave.emit(null);
                    this.checkOverDraftLimit();
                }
                else {
                    this.close();
                    this.modalSave.emit(null);
                }

                // } else {
                // 	this.close();
                // 	this.modalSave.emit(null);
                // }
                //});
            });
        // 		}
        // 	}
        // );
    }

    saveAndClose(print: Boolean): void {
        debugger;
        this.errorFlag = false;
        // this.message.confirm(
        // 	'Save Voucher',
        // 	(isConfirmed) => {
        // 		if (isConfirmed) {

        if (moment(this.gltrHeader.docDate) > moment().endOf("day")) {
            this.message.warn(
                "Document date greater than current date",
                "Document Date Greater"
            );
            return;
        }

        if (
            moment(this.LockDocDate).month() + 1 != this.gltrHeader.docMonth &&
            this.gltrHeader.id != null
        ) {
            this.message.warn(
                "Document month not changeable",
                "Document Month Error"
            );
            return;
        }

        if (
            moment(this.gltrHeader.docDate).month() + 1 !=
            this.gltrHeader.docMonth
        ) {
            this.message.warn(
                this.l("Document month not according to documrnt date"),
                "Document Month Error"
            );
            return;
        }

        if (
            this.gltrHeader.locId == null ||
            this.gltrHeader.locId.toString() == "0"
        ) {
            this.message.warn(
                this.l("Please select location"),
                "Location required"
            );
            return;
        }

        if (this.gridApi.getDisplayedRowCount() <= 0) {
            this.message.warn("No details found", "Details Required");
            return;
        }

        // this.gridApi.forEachNode(node => {
        //     debugger
        //     if (node.data.isSL && (node.data.subAccID=="" || node.data.subAccID==0)) {
        //         this.message.warn("Subledger Account not found at row " + Number(node.rowIndex + 1), "Subledger Required");
        //         this.errorFlag = true;
        //         return;
        //     } else {
        //         this.errorFlag = false;
        //     }
        // });

        // if (this.errorFlag) {
        //     return;
        // }

        this.gridApi.forEachNode((node) => {
            debugger;
            if (
                node.data.isSL &&
                (node.data.subAccID == "" || node.data.subAccID == 0)
            ) {
                this.message.warn(
                    "Subledger Account not found at row " +
                    Number(node.rowIndex + 1)
                );
                //, "Subledger Required");
                this.errorFlag = true;
                //return;
            }
            // else {
            //     this.errorFlag = false;
            // }
        });

        if (this.errorFlag) {
            debugger;
            return;
        }

        if (this.totalBalance == 0) {
            this.message.warn(
                this.l("Balance amount not greater than zero"),
                "Balance Amount Zero"
            );
            return;
        }

        if (this.NormalEntry == 3) {
            var total =
                this.totalBalance -
                Math.abs(this.totalDebit - this.totalCredit);
            if (total != 0) {
                this.message.warn(
                    this.l("Credit and debit amount not equal"),
                    "Credit/Debit Equal"
                );
                return;
            }
        }

        this.saving = true;

        var rowData = [];
        this.gridApi.forEachNode((node) => {
            if (this.gltrHeader.bookID == "JV") {
                if (node.data.credit == 0 && node.data.debit != 0) {
                    rowData.push(node.data);
                    node.data["amount"] = node.data.debit;
                }
                if (node.data.debit == 0 && node.data.credit != 0) {
                    rowData.push(node.data);
                    node.data["amount"] = -node.data.credit;
                }
            } else {
                if (node.data.credit == 0 && node.data.debit != 0) {
                    rowData.push(node.data);
                    node.data["amount"] = node.data.debit;
                }
                if (node.data.debit == 0 && node.data.credit != 0) {
                    rowData.push(node.data);
                    node.data["amount"] = -node.data.credit;
                }
            }
        });

        var amountType = "";
        var amount = 0;
        if (this.totalDebit > 0) {
            amountType = this.gltrHeader.narration;
            amount = -this.totalBalance;
        }
        if (this.totalCredit > 0) {
            amountType = this.gltrHeader.narration;
            amount = this.totalBalance;
        }

        if (this.gltrHeader.bookID != "JV") {
            rowData.push({
                accountID: this.accountIdC,
                subAccID: "0",
                narration: amountType,
                chequeNo: "",
                isAuto: true,
                amount: amount,
            });
        }

        this.voucherEntry.gltrDetail = rowData;
        this.voucherEntry.gltrHeader = this.gltrHeader;

        this.gltrHeader.auditTime = moment();
        this.gltrHeader.auditUser = this.appSession.user.userName;

        if (!this.gltrHeader.id) {
            this.gltrHeader.createdOn = moment();
            this.gltrHeader.createdBy = this.appSession.user.userName;
        }

        if (
            moment(new Date()).format("A") === "AM" &&
            !this.gltrHeader.id &&
            moment(new Date()).month() + 1 == this.gltrHeader.docMonth
        ) {
            this.gltrHeader.docDate = moment(this.gltrHeader.docDate);
        } else {
            this.gltrHeader.docDate = moment(this.gltrHeader.docDate).endOf(
                "day"
            );
        }

        this.gltrHeader.amount = this.totalBalance;

        debugger;

        this._voucherEntryServiceProxy
            .createOrEditVoucherEntry(this.voucherEntry)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                //this.notify.info(this.l('SavedSuccessfully'));
                //this.message.confirm("Press 'Yes' for create new voucher", this.l('SavedSuccessfully'), (isConfirmed) => {
                //if (isConfirmed) {
                // this.gltrHeader.docNo = Number(this.gltrHeader.docNo) + 1;
                // this.rowData = [];
                // this.modalSave.emit(null);
                // } else {

                if (print == true) {
                    this.getReport(this.voucherEntry.gltrHeader);
                    this.modalSave.emit(null);
                } else {
                    this.close();
                    this.modalSave.emit(null);
                }
                // }
                //});
            });
        // 		}
        // 	}
        // );
    }

    getReport(gltrHeader: CreateOrEditGLTRHeaderDto) {
        this._voucherEntryServiceProxy.getBaseCurrency().subscribe((result) => {
            if (result) {
                this.curid = result.id;
                this.curRate = result.currRate;
            }

            //this.visible = true;
            var rptObj = JSON.stringify({
                bookId: gltrHeader.bookID,
                year: gltrHeader.docDate.toDate().getFullYear(),
                month: gltrHeader.docMonth,
                locId: 0,
                fromConfigId: gltrHeader.configID,
                toConfigId: gltrHeader.configID,
                fromDoc: gltrHeader.docNo,
                toDoc: gltrHeader.docNo,
                tenantId: this.appSession.tenantId,
            });
            //this.reportUrl = "CashReceipt";

            let repParams = "";
            if (gltrHeader.bookID !== undefined)
                repParams += encodeURIComponent("" + gltrHeader.bookID) + "$";
            if (gltrHeader.docDate !== undefined)
                repParams +=
                    encodeURIComponent(
                        "" + gltrHeader.docDate.toDate().getFullYear()
                    ) + "$";
            if (gltrHeader.docMonth !== undefined)
                repParams += encodeURIComponent("" + gltrHeader.docMonth) + "$";
            if (gltrHeader.locId !== undefined)
                repParams += encodeURIComponent("" + gltrHeader.locId) + "$";
            if (gltrHeader.configID !== undefined)
                repParams += encodeURIComponent("" + gltrHeader.configID) + "$";
            if (gltrHeader.configID !== undefined)
                repParams += encodeURIComponent("" + gltrHeader.configID) + "$";
            if (gltrHeader.docNo !== undefined)
                repParams += encodeURIComponent("" + gltrHeader.docNo) + "$";
            if (gltrHeader.docNo !== undefined)
                repParams += encodeURIComponent("" + gltrHeader.docNo) + "$";

            repParams += encodeURIComponent("" + this.curid) + "$";
            repParams += encodeURIComponent("" + this.curRate) + "$";
            repParams = repParams.replace(/[?$]&/, "");
            // this.reportUrl = "CashReceipt";
            this.reportviewrModalComponent.show("CashReceipt", repParams);
        });
        //this.reportviewrModalComponent.show();
    }

    save(): void {
        debugger;
        this.errorFlag = false;
        this.message.confirm("Save Voucher", (isConfirmed) => {
            if (isConfirmed) {
                if (moment(this.gltrHeader.docDate) > moment().endOf("day")) {
                    this.message.warn(
                        "Document date greater than current date",
                        "Document Date Greater"
                    );
                    return;
                }

                if (
                    moment(this.LockDocDate).month() + 1 !=
                    this.gltrHeader.docMonth &&
                    this.gltrHeader.id != null
                ) {
                    this.message.warn(
                        "Document month not changeable",
                        "Document Month Error"
                    );
                    return;
                }

                if (
                    moment(this.gltrHeader.docDate).month() + 1 !=
                    this.gltrHeader.docMonth
                ) {
                    this.message.warn(
                        this.l("Document month not according to documrnt date"),
                        "Document Month Error"
                    );
                    return;
                }

                if (
                    this.gltrHeader.locId == null ||
                    this.gltrHeader.locId.toString() == "0"
                ) {
                    this.message.warn(
                        this.l("Please select location"),
                        "Location required"
                    );
                    return;
                }

                if (this.gridApi.getDisplayedRowCount() <= 0) {
                    this.message.warn("No details found", "Details Required");
                    return;
                }

                // this.gridApi.forEachNode(node => {
                //     debugger
                //     if (node.data.isSL && (node.data.subAccID=="" || node.data.subAccID==0)) {
                //         this.message.warn("Subledger Account not found at row " + Number(node.rowIndex + 1), "Subledger Required");
                //         this.errorFlag = true;
                //         return;
                //     } else {
                //         this.errorFlag = false;
                //     }
                // });

                // if (this.errorFlag) {
                //     return;
                // }

                this.gridApi.forEachNode((node) => {
                    debugger;
                    if (
                        node.data.isSL &&
                        (node.data.subAccID == "" || node.data.subAccID == 0)
                    ) {
                        this.message.warn(
                            "Subledger Account not found at row " +
                            Number(node.rowIndex + 1)
                        );
                        //, "Subledger Required");
                        this.errorFlag = true;
                        //return;
                    }
                    // else {
                    //     this.errorFlag = false;
                    // }
                });

                if (this.errorFlag) {
                    debugger;
                    return;
                }

                if (this.totalBalance == 0) {
                    this.message.warn(
                        this.l("Balance amount not greater than zero"),
                        "Balance Amount Zero"
                    );
                    return;
                }

                if (this.NormalEntry == 3) {
                    var total =
                        this.totalBalance -
                        Math.abs(this.totalDebit - this.totalCredit);
                    if (total != 0) {
                        this.message.warn(
                            this.l("Credit and debit amount not equal"),
                            "Credit/Debit Equal"
                        );
                        return;
                    }
                }

                this.saving = true;

                var rowData = [];
                this.gridApi.forEachNode((node) => {
                    if (this.gltrHeader.bookID == "JV") {
                        if (node.data.credit == 0 && node.data.debit != 0) {
                            rowData.push(node.data);
                            node.data["amount"] = node.data.debit;
                        }
                        if (node.data.debit == 0 && node.data.credit != 0) {
                            rowData.push(node.data);
                            node.data["amount"] = -node.data.credit;
                        }
                    } else {
                        if (node.data.credit == 0 && node.data.debit != 0) {
                            rowData.push(node.data);
                            node.data["amount"] = node.data.debit;
                        }
                        if (node.data.debit == 0 && node.data.credit != 0) {
                            rowData.push(node.data);
                            node.data["amount"] = -node.data.credit;
                        }
                    }
                });

                var amountType = "";
                var amount = 0;
                if (this.totalDebit > 0) {
                    amountType = this.gltrHeader.narration;
                    amount = -this.totalBalance;
                }
                if (this.totalCredit > 0) {
                    amountType = this.gltrHeader.narration;
                    amount = this.totalBalance;
                }

                if (this.gltrHeader.bookID != "JV") {
                    rowData.push({
                        accountID: this.accountIdC,
                        subAccID: "0",
                        narration: amountType,
                        chequeNo: "",
                        isAuto: true,
                        amount: amount,
                    });
                }

                this.voucherEntry.gltrDetail = rowData;
                this.voucherEntry.gltrHeader = this.gltrHeader;

                this.gltrHeader.auditTime = moment();
                this.gltrHeader.auditUser = this.appSession.user.userName;

                if (!this.gltrHeader.id) {
                    this.gltrHeader.createdOn = moment();
                    this.gltrHeader.createdBy = this.appSession.user.userName;
                }

                if (
                    moment(new Date()).format("A") === "AM" &&
                    !this.gltrHeader.id &&
                    moment(new Date()).month() + 1 == this.gltrHeader.docMonth
                ) {
                    this.gltrHeader.docDate = moment(this.gltrHeader.docDate);
                } else {
                    this.gltrHeader.docDate = moment(
                        this.gltrHeader.docDate
                    ).endOf("day");
                }

                this.gltrHeader.amount = this.totalBalance;

                debugger;

                this._voucherEntryServiceProxy
                    .createOrEditVoucherEntry(this.voucherEntry)
                    .pipe(
                        finalize(() => {
                            this.saving = false;
                        })
                    )
                    .subscribe(() => {
                        //this.notify.info(this.l('SavedSuccessfully'));
                        if (this.editMode == false) {
                            this.message.confirm(
                                "Press 'Yes' for create new voucher",
                                this.l("SavedSuccessfully"),
                                (isConfirmed) => {
                                    if (isConfirmed) {
                                        //.gltrHeader.fmtDocNo = (Number(this.gltrHeader.fmtDocNo) + 1).toString();
                                        this._voucherEntryServiceProxy
                                            .getMaxDocId(
                                                this.gltrHeader.bookID,
                                                true,
                                                moment(
                                                    this.gltrHeader.docDate
                                                ).format("LLLL")
                                            )
                                            .subscribe((result) => {
                                                this.gltrHeader.fmtDocNo = result;
                                            });
                                        this.rowData = [];
                                        this.modalSave.emit(null);
                                        this.checkOverDraftLimit();
                                    } else {
                                        this.close();
                                        this.modalSave.emit(null);
                                    }
                                }
                            );
                        }  else {
                            this.close();
                            this.modalSave.emit(null);
                        }
                    });
            }
        });
    }

    getChequeBookDetail() {
        debugger;
        this.gltrHeader.chNumber = this.FinanceLookupTableModal.id;
    }

    getNewFinanceModal() {
        debugger;
        switch (this.target) {
            case "GLConfig":
                this.getNewGLCONFIGId();
                break;
            case "SubLedger":
                this.getNewGLSubledgerId();
                break;
            case "ChartOfAccount":
                this.getNewChartofControlId();
                break;
            case "ChequeBookDetail":
                this.getChequeBookDetail();
                break;

            default:
                break;
        }
    }

    //=====================GL Config Model================
    openSelectGLCONFIGModal() {
        debugger;
        this.target = "GLConfig";
        this.FinanceLookupTableModal.id = String(this.gltrHeader.configID);
        this.FinanceLookupTableModal.accountID = this.accountIdC;
        this.FinanceLookupTableModal.displayName = this.accountDescB;
        this.FinanceLookupTableModal.show(this.target, this.gltrHeader.bookID);
    }

    setGLCONFIGIdNull() {
        this.gltrHeader.glconfigId = null;
        this.accountIdC = "";
        this.accountDescB = "";
    }

    getNewGLCONFIGId() {
        this.gltrHeader.configID = Number(this.FinanceLookupTableModal.id);
        this.accountIdC = this.FinanceLookupTableModal.accountID;
        this.accountDescB = this.FinanceLookupTableModal.displayName;
        if (this.gltrHeader.bookID == "BP") {
            this._voucherEntryServiceProxy
                .GetWeatherBankTypeISOverDraftOrNot(this.accountIdC)
                .subscribe((result) => {
                    this.overDraftLimitApplies = result["result"];
                    if (this.overDraftLimitApplies == true)
                        this.checkOverDraftLimit();
                    else {
                        this.overDraftLimitExceeds = false;
                        this.overDraftLimit = 0;
                    }
                });
        }
    }
    //=====================GL Config Model=================

    //=====================Account Code Model==============
    openSelectChartofControlModal() {
        debugger;
        this.target = "ChartOfAccount";
        this.FinanceLookupTableModal.id = this.accountID;
        this.FinanceLookupTableModal.displayName = this.accountDesc;
        this.FinanceLookupTableModal.show(this.target);
    }

    setChartofControlIdNull() {
        debugger;
        this.setParms.data.accountID = "";
        this.setParms.data.accountDesc = "";
    }

    getNewChartofControlId() {
        debugger;
        if (
            this.setParms.data.accountID != "" &&
            this.FinanceLookupTableModal.id == ""
        ) {
            this.onBtStartEditing(this.setParms.rowIndex, "narration");
            return;
        }
        this.setParms.data.accountID = this.FinanceLookupTableModal.id;
        this.setParms.data.accountDesc = this.FinanceLookupTableModal.displayName;
        this.setGLSubledgerIdNull();
        this.gridApi.refreshCells();
        if (this.FinanceLookupTableModal.subledger == true) {
            this.setParms.data.isSL = true;
            this.onBtStartEditing(this.setParms.rowIndex, "addsubAccId");
        } else {
            this.setParms.data.isSL = false;
            this.onBtStartEditing(this.setParms.rowIndex, "narration");
        }
    }

    openInstrumentNo() {
        if (this.accountIdC != "") {
            this.target = "ChequeBookDetail";
            this.FinanceLookupTableModal.id = "";
            this.FinanceLookupTableModal.displayName = "";
            this.FinanceLookupTableModal.show(
                "ChequeBookDetail",
                this.accountIdC,
                "",
                " Instrument No"
            );
            //this.FinanceLookupTableModal.show(this.target,this.accountIdC);
        } else {
            this.message.confirm("Please select config first");
        }
    }

    //=====================Account Code Model==============

    //=====================GLSubledger Model==============
    openSelectGLSubledgerModal() {
        debugger;
        this.target = "SubLedger";
        this.FinanceLookupTableModal.id = this.subAccID;
        this.FinanceLookupTableModal.displayName = this.subAccDesc;
        this.FinanceLookupTableModal.show(
            this.target,
            this.setParms.data.accountID
        );
    }

    setGLSubledgerIdNull() {
        debugger;
        this.setParms.data.subAccID = 0;
        this.setParms.data.subAccDesc = "";
    }

    getNewGLSubledgerId() {
        debugger;
        this.setParms.data.subAccID = this.FinanceLookupTableModal.id;
        this.setParms.data.subAccDesc = this.FinanceLookupTableModal.displayName;
        this.gridApi.refreshCells();
        this.onBtStartEditing(this.setParms.rowIndex, "narration");
    }

    //=====================GLSubledger Model==============

    // getDateParams(val){
    //     debugger;
    //     this.gltrHeader.docMonth = moment(this.gltrHeader.docDate).month()+1;
    // 	this.DocYear = moment(this.gltrHeader.docDate).year();
    // 	this._voucherEntryServiceProxy
    // 	.getMaxDocId(this.gltrHeader.bookID, true, moment(this.gltrHeader.docDate).format("LLLL"))
    // 	.subscribe(result => {
    // 		this.gltrHeader.fmtDocNo = "";
    // 		this.gltrHeader.fmtDocNo = result;
    // 	});
    // }
    getDateParams(val?, checkForDocId?) {
        if(!this.gltrHeader.id){
        this._fiscalCalendarsServiceProxy
            .getFiscalYearStatus(this.gltrHeader.docDate, "GL")
            .subscribe((result) => {
                debugger;
                if (result == true) {
                    if (checkForDocId == null || checkForDocId == true) {
                        this.gltrHeader.docMonth =
                            moment(this.gltrHeader.docDate).month() + 1;
                        this.DocYear = moment(this.gltrHeader.docDate).year();
                        this._voucherEntryServiceProxy
                            .getMaxDocId(
                                this.gltrHeader.bookID,
                                true,
                                moment(this.gltrHeader.docDate).format("LLLL")
                            )
                            .subscribe((result) => {
                                if (
                                    this.gltrHeader.id == null ||
                                    this.gltrHeader.id == undefined
                                ) {
                                    this.gltrHeader.fmtDocNo = undefined;
                                    this.gltrHeader.fmtDocNo = result;
                                }
                            });
                    }
                    this.dateValid = true;
                } else {
                    this.notify.info("This Date Is Locked");
                    // this.gltrHeader.docDate = moment();
                    // this.gltrHeader.docMonth = moment(this.gltrHeader.docDate).month() + 1;
                    // this.DocYear = moment(this.gltrHeader.docDate).year();
                    // this.gltrHeader.docDate = moment();
                    this.dateValid = false;
                }
            });
        }else
		{
		this.dateValid = true;
	 }
    }
    //===========================File Attachment=============================
    onBeforeUpload(event): void {
        debugger;
        this.uploadUrl =
            AppConsts.remoteServiceBaseUrl + "/DemoUiComponents/UploadFiles?";
        if (this.voucherEntryAppId !== undefined)
            this.uploadUrl +=
                "APPID=" +
                encodeURIComponent("" + this.voucherEntryAppId) +
                "&";
        if (this.appName !== undefined)
            this.uploadUrl +=
                "AppName=" + encodeURIComponent("" + this.appName) + "&";
        if (this.gltrHeader.docNo !== undefined)
            this.uploadUrl +=
                "DocID=" + encodeURIComponent("" + this.gltrHeader.docNo) + "&";
        this.uploadUrl = this.uploadUrl.replace(/[?&]$/, "");
    }

    onUpload(event): void {
        this.checkImage = true;
        for (const file of event.files) {
            this.uploadedFiles.push(file);
        }
    }
    //===========================File Attachment=============================
    open(): void {
        debugger;
        this._lightbox.open(this.image);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this._lightbox.close();
    }

    //===========================Host Listener=============================
    @HostListener("window:keydown", ["$event"])
    handleKeyDown(event: KeyboardEvent) {
        debugger;
        if (
            this.sortKeys[0] === "Control" &&
            this.sortKeys.length <= 3 &&
            this.sortKeys.length > 0
        ) {
            event.preventDefault();
        }
        this.sortKeys.push(event.key);
        if (this.sortKeys.length == 2) {
            debugger;
            if (
                this.sortKeys[0] === "Control" &&
                this.sortKeys[1] == "s" &&
                this.voucherEntryForm.valid == true
            ) {
                //event.preventDefault();
                this.saveAndClose(false);
                this.sortKeys.length = 0;
            }
            if (
                this.sortKeys[0] === "Control" &&
                this.sortKeys[1] == "p" &&
                this.voucherEntryForm.valid == true
            ) {
                //event.preventDefault();
                this.saveAndClose(true);
                this.sortKeys.length = 0;
            } else if (
                this.sortKeys[0] === "Control" &&
                this.sortKeys[1] == "n" &&
                this.voucherEntryForm.valid == true
            ) {
                this.saveAndNew();
                this.sortKeys.length = 0;
            }

            if (this.sortKeys[0] === "Control" && this.sortKeys[1] != "Shift") {
                this.sortKeys.length = 0;
            } else if (
                (this.sortKeys[0] === "Control" &&
                    this.sortKeys[1] == "Shift") ||
                (this.sortKeys[0] === "Control" && this.sortKeys[1] == "n") ||
                (this.sortKeys[0] === "Control" && this.sortKeys[1] == "p")
            ) {
                //this.sortKeys.length = 0;
            } else {
                this.sortKeys.length = 0;
            }
        } else if (this.sortKeys.length == 3 || this.sortKeys.length > 3) {
            if (
                this.sortKeys[0] === "Control" &&
                this.sortKeys[1] == "Shift" &&
                (this.sortKeys[2] == "c" || this.sortKeys[2] == "C")
            ) {
                this.sortKeys.length = 0;
                this.saveAndClose(false);
            }
        }
    }
}
