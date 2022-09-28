import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { finalize } from "rxjs/operators";
import { AppComponentBase } from "@shared/common/app-component-base";
import * as moment from "moment";
import { CreateOrEditCRDRNoteDto } from "../../shared/dto/crdrNote-dto";
import { CRDRNoteServiceProxy } from "../../shared/services/crdrNote.service";
import { FinanceLookupTableModalComponent } from "@app/finders/finance/finance-lookup-table-modal.component";
import {
    CreateOrEditGLTRDetailDto,
    CreateOrEditGLTRHeaderDto,
    VoucherEntryDto,
    VoucherEntryServiceProxy,
    FiscalCalendersServiceProxy
} from "@shared/service-proxies/service-proxies";
import { GlChequesService } from "@app/main/finance/shared/services/glCheques.service";
@Component({
    selector: "createOrEditCRDRNoteModal",
    templateUrl: "./create-or-edit-crdrNote-modal.component.html"
})
export class CreateOrEditCRDRNoteModalComponent extends AppComponentBase {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild("FinanceLookupTableModal", { static: true })
    FinanceLookupTableModal: FinanceLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    processing = false;
    gltrHeader: CreateOrEditGLTRHeaderDto = new CreateOrEditGLTRHeaderDto();
    gltrDetail: CreateOrEditGLTRDetailDto = new CreateOrEditGLTRDetailDto();
    gltrDetailArr: CreateOrEditGLTRDetailDto[] = Array<
        CreateOrEditGLTRDetailDto
    >();
    voucherEntry: VoucherEntryDto = new VoucherEntryDto();
    validDate:boolean;
    crdrNote: CreateOrEditCRDRNoteDto = new CreateOrEditCRDRNoteDto();

    createDate: Date;
    audtDate: Date;
    docDate: Date;
    partyInvDate: Date;
    target: string;
    locDesc: string;
    accDesc: string;
    headerName: string;
    partyDesc: string;
    stkDesc: string;
    docTypeName: string;

    constructor(
        injector: Injector,
        private _crdrNoteServiceProxy: CRDRNoteServiceProxy,
        private _service: GlChequesService,
        private _voucherEntryServiceProxy: VoucherEntryServiceProxy,
        private _fiscalCalendarsServiceProxy: FiscalCalendersServiceProxy
    ) {
        super(injector);
    }
    approve() {
        this.processing = true;
        debugger;
        this._voucherEntryServiceProxy.getBaseCurrency().subscribe(result => {
            debugger;
            if (result) {
                this.voucherEntry.gltrHeader = new CreateOrEditGLTRHeaderDto();
                this.voucherEntry.gltrDetail = new Array<
                    CreateOrEditGLTRDetailDto
                >();
                this.voucherEntry.gltrHeader.curid = result.id;
                this.voucherEntry.gltrHeader.currate = result.currRate;
                this._voucherEntryServiceProxy
                    .getMaxDocId(
                        "JV",
                        true,
                        moment(this.crdrNote.docDate).format("LLLL")
                    )
                    .subscribe(result => {
                        this.voucherEntry.gltrHeader.docNo = this.crdrNote.docNo;
                        this.voucherEntry.gltrHeader.fmtDocNo = result;
                        this.voucherEntry.gltrHeader.docDate = moment(
                            this.crdrNote.docDate
                        );
                        this.voucherEntry.gltrHeader.amount = this.crdrNote.partyInvAmount;
                        this.voucherEntry.gltrHeader.configID = 0;
                        //this.voucherEntry.gltrHeader.chNumber = this.gltrHeader.chequeNo;
                        //this.voucherEntry.gltrHeader.reference = this.gltrHeader.partyName;
                        // this.voucherEntry.gltrHeader.chType = this.gltrHeader.ch;
                        this.voucherEntry.gltrHeader.locId = this.crdrNote.locID;
                        this.voucherEntry.gltrHeader.approved = true;
                        //this.voucherEntry.gltrHeader.curid = "PKR";
                        debugger;
                        this.voucherEntry.gltrHeader.docMonth =
                            moment(this.crdrNote.docDate).month() + 1;
                        // this.voucherEntry.gltrHeader.currate = 1;
                        this.voucherEntry.gltrHeader.posted = false;
                        this.voucherEntry.gltrHeader.bookID = "JV";
                        this.voucherEntry.gltrHeader.auditTime = moment();
                        this.voucherEntry.gltrHeader.auditUser = this.appSession.user.userName;
                        this.voucherEntry.gltrHeader.narration =
                            this.docTypeName +" " +
                            this.crdrNote.transType +
                            " Document No :" +
                            this.crdrNote.docNo.toString() +
                            ", Party Name: " +
                            this.partyDesc
                            ", Invoice No: " + this.crdrNote.invoiceNo.toString();;

                        // this.voucherEntry.gltrHeader.reference = 
                        // "Document No :" + this.crdrNote.docNo.toString() +  
                        //  ", Invoice No: " + this.crdrNote.invoiceNo.toString();
                        //if(this.glCheques.typeID=="1"){
                        this.gltrDetailArr = new Array<
                            CreateOrEditGLTRDetailDto
                        >();
                        // this.docTypeName = typeName;
                        // this.partyInvDate = null;
                        debugger;
                        if (this.docTypeName == "AR") {
                            if (this.crdrNote.transType == "DR") {
                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.crdrNote.stkAccID;
                                this.gltrDetail.subAccID = 0;
                                this.gltrDetail.amount = -this.crdrNote.amount;
                                //this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = false;
                                this.gltrDetail.locId = this.crdrNote.locID;
                                this.gltrDetailArr.push(this.gltrDetail);

                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.crdrNote.accountID;
                                this.gltrDetail.subAccID = this.crdrNote.subAccID;
                                this.gltrDetail.amount = this.crdrNote.amount;
                                //this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = false;
                                this.gltrDetail.locId = this.crdrNote.locID;
                            } else {
                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.crdrNote.stkAccID;
                                this.gltrDetail.subAccID = 0;
                                this.gltrDetail.amount = this.crdrNote.amount;
                                //this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = false;
                                this.gltrDetail.locId = this.crdrNote.locID;
                                this.gltrDetailArr.push(this.gltrDetail);

                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.crdrNote.accountID;
                                this.gltrDetail.subAccID = this.crdrNote.subAccID;
                                this.gltrDetail.amount = -this.crdrNote.amount;
                                //this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = false;
                                this.gltrDetail.locId = this.crdrNote.locID;
                            }
                        } else {
                            if (this.crdrNote.transType == "DR") {
                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.crdrNote.stkAccID;
                                this.gltrDetail.subAccID = 0;
                                this.gltrDetail.amount = this.crdrNote.amount;
                                //this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = false;
                                this.gltrDetail.locId = this.crdrNote.locID;
                                this.gltrDetailArr.push(this.gltrDetail);

                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.crdrNote.accountID;
                                this.gltrDetail.subAccID = this.crdrNote.subAccID;
                                this.gltrDetail.amount = -this.crdrNote.amount;
                                //this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = false;
                                this.gltrDetail.locId = this.crdrNote.locID;
                            } else {
                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.crdrNote.stkAccID;
                                this.gltrDetail.subAccID = 0;
                                this.gltrDetail.amount = -this.crdrNote.amount;
                                //this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = false;
                                this.gltrDetail.locId = this.crdrNote.locID;
                                this.gltrDetailArr.push(this.gltrDetail);

                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.crdrNote.accountID;
                                this.gltrDetail.subAccID = this.crdrNote.subAccID;
                                this.gltrDetail.amount = this.crdrNote.amount;
                                //this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = false;
                                this.gltrDetail.locId = this.crdrNote.locID;
                            }
                        }

                        this.gltrDetailArr.push(this.gltrDetail);

                        this.voucherEntry.gltrDetail.unshift(
                            ...this.gltrDetailArr
                        );
                        //  }

                        if (!this.voucherEntry.gltrHeader.id) {
                            this.gltrHeader.createdOn = moment();
                            this.gltrHeader.createdBy = this.appSession.user.userName;
                        }

                        this._service
                            .ProcessVoucherEntry(this.voucherEntry)
                            .pipe(
                                finalize(() => {
                                    this.saving = false;
                                })
                            )
                            .subscribe(res => {
                                this.notify.info(this.l("SavedSuccessfully"));
                                this.crdrNote.linkDetID =
                                    res["result"][0]["id"];
                                this.crdrNote.posted = true;
                                debugger;
                                this._crdrNoteServiceProxy
                                    .createOrEdit(this.crdrNote)
                                    .pipe(
                                        finalize(() => {
                                            this.saving = false;
                                        })
                                    )
                                    .subscribe(() => {
                                        this.notify.info(
                                            this.l("SavedSuccessfully")
                                        );
                                        this.processing = false;
                                        this.close();
                                        this.modalSave.emit(null);
                                    });
                            });
                    });
            }
        });
    }

    getDateParams(val?) {
        debugger
        this._fiscalCalendarsServiceProxy.getFiscalYearStatus(moment(this.docDate), this.crdrNote.typeID == 1 ? 'AR' : 'AP').subscribe(
            result => {
                if (result == true) {
                    this.validDate = true;
                }
                else {
                    this.notify.info("This Date Is Locked");
                    this.validDate = false;
                }
            }
        )
    }

    show(
        CRDRNoteId?: number,
        maxID?: number,
        typeID?: number,
        typeName?: string
    ): void {
        debugger;
        this.locDesc = "";
        this.accDesc = "";
        this.partyDesc = "";
        this.stkDesc = "";
        this.headerName = "";

        debugger;
        if (!CRDRNoteId) {
            debugger;
            this.crdrNote = new CreateOrEditCRDRNoteDto();
            this.crdrNote.id = CRDRNoteId;
            this.crdrNote.transType = "-1";
            this.docTypeName = typeName;
            this.docDate = new Date();
            this.crdrNote.active = true;
            this.crdrNote.docNo = maxID;
            this.crdrNote.typeID = typeID;
            this.crdrNote.transType = "DR";
            this.headerName =
                this.l("CreateNewCRDRNote") + " (" + typeName + ")";
            this.active = true;
            this.getDateParams();
            this.modal.show();
        } else {
            debugger;
            this._crdrNoteServiceProxy
                .getCRDRNoteForEdit(CRDRNoteId)
                .subscribe(result => {
                    debugger;
                    this.crdrNote = result.crdrNote;
                    this.getDateParams();
                    if (this.crdrNote.locID) {
                        this._crdrNoteServiceProxy
                            .getLocationName(this.crdrNote.locID)
                            .subscribe(result => {
                                this.locDesc = result;
                            });
                    }
                    if (this.crdrNote.accountID) {
                        this._crdrNoteServiceProxy
                            .getChartOfAccountName(
                                this.crdrNote.accountID,
                                "AccountID"
                            )
                            .subscribe(result => {
                                this.accDesc = result;
                            });
                    }
                    if (this.crdrNote.stkAccID) {
                        this._crdrNoteServiceProxy
                            .getChartOfAccountName(
                                this.crdrNote.stkAccID,
                                "StkAccID"
                            )
                            .subscribe(result => {
                                this.stkDesc = result;
                            });
                    }
                    if (this.crdrNote.subAccID) {
                        this._crdrNoteServiceProxy
                            .getSubLedgerName(
                                this.crdrNote.subAccID,
                                this.crdrNote.accountID
                            )
                            .subscribe(result => {
                                this.partyDesc = result;
                            });
                    }
                    if (this.crdrNote.typeID == 0) {
                        this.docTypeName = "AP";
                        this.headerName =
                            this.l("EditCRDRNote") + " (" + "AP" + ")";
                    } else if (this.crdrNote.typeID == 1) {
                        this.docTypeName = "AR";
                        this.headerName =
                            this.l("EditCRDRNote") + " (" + "AR" + ")";
                    }
                    this.docDate = moment(this.crdrNote.docDate).toDate();
                    this.partyInvDate = moment(
                        this.crdrNote.partyInvDate
                    ).toDate();
                    this.active = true;
                    this.modal.show();            
                });
        }
    }

    save(): void {
        this.saving = true;
        debugger;

        this.crdrNote.audtDate = moment();
        this.crdrNote.audtUser = this.appSession.user.userName;

        this.crdrNote.docDate = moment(this.docDate);
        this.crdrNote.partyInvDate = moment(this.partyInvDate);

        if (!this.crdrNote.id) {
            this.crdrNote.createDate = moment();
            this.crdrNote.createdBy = this.appSession.user.userName;
        }

        this._crdrNoteServiceProxy
            .createOrEdit(this.crdrNote)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                debugger;
                this.notify.info(this.l("SavedSuccessfully"));
                this.close();
                this.modalSave.emit(null);
            });
    }

    getNewFinanceModal() {
        debugger;
        switch (this.target) {
            case "GLLocation":
                this.getNewLocation();
                break;
            case "ChartOfAccount":
                this.getNewAccountID();
                break;
            case "StockAccount":
                this.getNewStockAccountID();
                break;
            case "SubLedger":
                this.getSubAccID();
                break;
            case "InvoiceNo":
                this.getInvoiceNo();
                break;
            default:
                break;
        }
    }

    //---------------------Location-------------------------//
    openLocationModal() {
        this.target = "GLLocation";
        this.FinanceLookupTableModal.id = String(this.crdrNote.locID);
        this.FinanceLookupTableModal.displayName = this.locDesc;
        this.FinanceLookupTableModal.show(this.target);
    }
    setLocationNull() {
        this.crdrNote.locID = null;
        this.locDesc = "";
    }
    getNewLocation() {
        debugger;
        this.crdrNote.locID = Number(this.FinanceLookupTableModal.id);
        this.locDesc = this.FinanceLookupTableModal.displayName;
    }
    //------------------------------------------------------//
    //=====================Chart of Ac Model================
    openAccountIDModal() {
        debugger;
        this.target = "ChartOfAccount";
        this.FinanceLookupTableModal.id = this.crdrNote.accountID;
        this.FinanceLookupTableModal.displayName = this.accDesc;
        this.FinanceLookupTableModal.show(this.target);
    }

    setAccountIDNull() {
        this.crdrNote.accountID = "";
        this.accDesc = "";
        this.setSubAccIDNull();
    }

    getNewAccountID() {
        debugger;
        this.crdrNote.accountID = this.FinanceLookupTableModal.id;
        this.accDesc = this.FinanceLookupTableModal.displayName;
    }
    //=====================Chart of Ac Model================//
    //=====================Chart of Stock Ac Model================
    openStkAccIDModal() {
        debugger;
        this.target = "StockAccount";
        this.FinanceLookupTableModal.id = this.crdrNote.stkAccID;
        this.FinanceLookupTableModal.displayName = this.stkDesc;
        this.FinanceLookupTableModal.show("ChartOfAccount");
    }

    setStkAccIDNull() {
        this.crdrNote.stkAccID = "";
        this.stkDesc = "";
    }

    getNewStockAccountID() {
        debugger;
        this.crdrNote.stkAccID = this.FinanceLookupTableModal.id;
        this.stkDesc = this.FinanceLookupTableModal.displayName;
    }
    //=====================Chart of Stock Ac Model================//
    //=====================Sub Account Model================
    openSubAccIDModal() {
        debugger;
        var account = this.crdrNote.accountID;
        if (account == "" || account == null) {
            this.message.warn(
                this.l("Please select account first"),
                "Account Required"
            );
            return;
        }
        this.target = "SubLedger";
        this.FinanceLookupTableModal.id = String(this.crdrNote.subAccID);
        this.FinanceLookupTableModal.displayName = this.partyDesc;
        this.FinanceLookupTableModal.show(this.target, account);
    }

    setSubAccIDNull() {
        this.crdrNote.subAccID = null;
        this.partyDesc = "";
    }

    getSubAccID() {
        this.crdrNote.subAccID = Number(this.FinanceLookupTableModal.id);
        this.partyDesc = this.FinanceLookupTableModal.displayName;
    }
    //=====================Sub Account Model================
    //=====================Chart of Stock Ac Model================
    openInvoiceNoModal() {
        debugger;
        this.target = "InvoiceNo";
        this.FinanceLookupTableModal.id = String(this.crdrNote.invoiceNo);
        this.FinanceLookupTableModal.displayName = this.crdrNote.partyInvNo;
        this.FinanceLookupTableModal.docDate = null;
        this.FinanceLookupTableModal.amount = this.crdrNote.partyInvAmount;
        this.FinanceLookupTableModal.show(this.target, this.docTypeName,this.crdrNote.accountID,null,this.crdrNote.subAccID);
    }

    setInvoiceNoNull() {
        this.crdrNote.invoiceNo = null;
        this.crdrNote.partyInvNo = "";
        this.crdrNote.partyInvDate = null;
        this.crdrNote.partyInvAmount = null;
    }

    getInvoiceNo() {
        debugger;
        this.crdrNote.invoiceNo = Number(this.FinanceLookupTableModal.id);
        this.crdrNote.partyInvNo = this.FinanceLookupTableModal.displayName;
        this.partyInvDate = this.FinanceLookupTableModal.docDate;
        this.crdrNote.partyInvAmount = this.FinanceLookupTableModal.amount;
    }
    //=====================Chart of Stock Ac Model================//

    //     if (!this.crdrNote.id) {
    //         this.crdrNote.createDate = moment();
    //         this.crdrNote.createdBy = this.appSession.user.userName;
    //     }

    //     this._crdrNoteServiceProxy
    //         .createOrEdit(this.crdrNote)
    //         .pipe(
    //             finalize(() => {
    //                 this.saving = false;
    //             })
    //         )
    //         .subscribe(() => {
    //             debugger;
    //             this.notify.info(this.l("SavedSuccessfully"));
    //             this.close();
    //             this.modalSave.emit(null);
    //         });
    // }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
