import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditGLTransferDto } from '@app/main/finance/shared/dto/glTransfer-dto';
import { GLTransferServiceProxy } from '@app/main/finance/shared/services/glTransfer.service';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { CommonServiceLookupTableModalComponent } from '@app/finders/commonService/commonService-lookup-table-modal.component';
import { CreateOrEditGLTRHeaderDto, CreateOrEditGLTRDetailDto, VoucherEntryDto, VoucherEntryServiceProxy } from '@shared/service-proxies/service-proxies';
import { GlChequesService } from '@app/main/finance/shared/services/glCheques.service';
@Component({
    selector: 'createOrEditGLTransferModal',
    templateUrl: './create-or-edit-glTransfer-modal.component.html'
})
export class CreateOrEditGLTransferModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild("FinanceLookupTableModal", { static: true })
    FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild('commonServiceLookupTableModal', { static: true }) commonServiceLookupTableModal: CommonServiceLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    processing = false;
    glTransfer: CreateOrEditGLTransferDto = new CreateOrEditGLTransferDto();
    gltrHeader: CreateOrEditGLTRHeaderDto = new CreateOrEditGLTRHeaderDto();
    gltrDetail: CreateOrEditGLTRDetailDto = new CreateOrEditGLTRDetailDto();
    gltrDetailArr: CreateOrEditGLTRDetailDto[] = Array<CreateOrEditGLTRDetailDto>();

    createDate: Date;
    audtDate: Date;
    docDate: Date;
    transferDate: Date;
    editMode: boolean;
    fromLocDesc: string;
    toLocDesc: string;
    fromBankName: string;
    fromBankDesc: string;
    toBankName: string;
    toBankDesc: string;
    fromAccDesc: string;
    toAccDesc: string;
    target: any;
    picker: string;
    voucherEntry: VoucherEntryDto = new VoucherEntryDto();


    constructor(
        injector: Injector,
        private _glTransferServiceProxy: GLTransferServiceProxy,
        private _service: GlChequesService,
        private _voucherEntryServiceProxy: VoucherEntryServiceProxy,
    ) {
        super(injector);
    }


    approve(type) {
        this.processing = true;
        this.voucherEntry.gltrHeader = new CreateOrEditGLTRHeaderDto();
        this.voucherEntry.gltrDetail = new Array<CreateOrEditGLTRDetailDto>();
        this._service.getGlConfigId(
            type
            , this.glTransfer.frombankaccid).subscribe(
                data => {
                    this._voucherEntryServiceProxy
                        .getMaxDocId(
                            type,
                            true,
                            moment(this.glTransfer.docdate).format("LLLL")
                        )
                        .subscribe(result => {

                            this.voucherEntry.gltrHeader.docNo = this.glTransfer.docid;
                            this.voucherEntry.gltrHeader.fmtDocNo = result;
                            this.voucherEntry.gltrHeader.docDate = moment(this.glTransfer.transferdate);
                            this.voucherEntry.gltrHeader.docMonth = moment(this.glTransfer.transferdate).month() + 1;
                            this.voucherEntry.gltrHeader.amount = this.glTransfer.transferamount;
                            this.voucherEntry.gltrHeader.configID = data["result"];
                            // this.voucherEntry.gltrHeader.chNumber = this.glTransfer.chequeNo;
                            //this.voucherEntry.gltrHeader.reference = this.glTransfer.partyName;
                            //this.voucherEntry.gltrHeader.chType = this.glTransfer.chType;
                            this.voucherEntry.gltrHeader.locId = (type == "BP" || type == "CP") ?
                                this.glTransfer.fromlocid : this.glTransfer.tolocid;
                            this.voucherEntry.gltrHeader.approved = true;
                            this.voucherEntry.gltrHeader.posted = false;
                            this.voucherEntry.gltrHeader.bookID = type;
                            this.voucherEntry.gltrHeader.auditTime = moment();
                            this.voucherEntry.gltrHeader.chType = this.glTransfer.chType;
                            this.voucherEntry.gltrHeader.chNumber = this.glTransfer.chNumber;
                            this.voucherEntry.gltrHeader.auditUser = this.appSession.user.userName;
                            this.voucherEntry.gltrHeader.narration = "Gltransfer - "
                                + " Voucher No :" + this.glTransfer.docid.toString();

                            //if(this.glCheques.typeID=="1"){
                            this.gltrDetailArr = new Array<CreateOrEditGLTRDetailDto>();


                            if (type == "CP") {
                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.glTransfer.frombankaccid;
                                this.gltrDetail.subAccID = 0;
                                this.gltrDetail.amount = -this.voucherEntry.gltrHeader.amount;
                                //this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = true;
                                this.gltrDetail.locId = this.gltrHeader.locId;
                                this.gltrDetailArr.push(this.gltrDetail);

                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.glTransfer.fromaccid;
                                this.gltrDetail.subAccID = 0;
                                this.gltrDetail.amount = this.voucherEntry.gltrHeader.amount;
                                this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = false;
                                this.gltrDetail.locId = this.gltrHeader.locId;
                            }
                            else if (type == "CR") {
                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.glTransfer.tobankaccid;
                                this.gltrDetail.subAccID = 0;
                                this.gltrDetail.amount = this.voucherEntry.gltrHeader.amount;
                                //this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = true;
                                this.gltrDetail.locId = this.gltrHeader.locId;
                                this.gltrDetailArr.push(this.gltrDetail);

                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.glTransfer.toaccid;
                                this.gltrDetail.subAccID = 0;
                                this.gltrDetail.amount = -this.voucherEntry.gltrHeader.amount;
                                //this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = false;
                                this.gltrDetail.locId = this.gltrHeader.locId;
                            }
                            else if (type == "BP") {
                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.glTransfer.frombankaccid;
                                this.gltrDetail.subAccID = 0;
                                this.gltrDetail.amount = -this.voucherEntry.gltrHeader.amount;
                                //this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = true;
                                this.gltrDetail.locId = this.gltrHeader.locId;
                                this.gltrDetailArr.push(this.gltrDetail);

                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.glTransfer.fromaccid;
                                this.gltrDetail.subAccID = 0;
                                this.gltrDetail.amount = this.voucherEntry.gltrHeader.amount;
                                this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = false;
                                this.gltrDetail.locId = this.gltrHeader.locId;
                            }
                            else {
                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.glTransfer.tobankaccid;
                                this.gltrDetail.subAccID = 0;
                                this.gltrDetail.amount = this.voucherEntry.gltrHeader.amount;
                                //this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = true;
                                this.gltrDetail.locId = this.gltrHeader.locId;
                                this.gltrDetailArr.push(this.gltrDetail);

                                this.gltrDetail = new CreateOrEditGLTRDetailDto();
                                this.gltrDetail.accountID = this.glTransfer.toaccid;
                                this.gltrDetail.subAccID = 0;
                                this.gltrDetail.amount = -this.voucherEntry.gltrHeader.amount;
                                //this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                                this.gltrDetail.isAuto = false;
                                this.gltrDetail.locId = this.gltrHeader.locId;
                            }

                            this.gltrDetailArr.push(this.gltrDetail);

                            this.voucherEntry.gltrDetail.unshift(...this.gltrDetailArr);
                            //  }

                            if (!this.voucherEntry.gltrHeader.id) {
                                this.gltrHeader.createdOn = moment();
                                this.gltrHeader.createdBy = this.appSession.user.userName;
                            }

                            this._service.ProcessVoucherEntry(this.voucherEntry)
                                .pipe(finalize(() => { this.processing = false; }))
                                .subscribe((res) => {
                                    debugger
                                    if (type == "BP")
                                        this.glTransfer.linkDetIDBP = res["result"][0]["id"];
                                    else if (type == "CP")
                                        this.glTransfer.linkDetIDCP = res["result"][0]["id"];
                                    else if (type == "CR")
                                        this.glTransfer.linkDetIDCR = res["result"][0]["id"];
                                    else
                                        this.glTransfer.linkDetIDBR = res["result"][0]["id"];

                                    this.glTransfer.posted = true;
                                    this.save();
                                });
                        });
                });
    }

    show(GLTransferId?: number): void {
        this.fromLocDesc = "";
        this.toLocDesc = "";
        this.fromBankName = "";
        this.fromBankDesc = "";
        this.toBankName = "";
        this.toBankDesc = "";
        this.fromAccDesc = "";
        this.toAccDesc = "";

        debugger;
        if (!GLTransferId) {
            debugger;
            this.editMode = false;
            this.glTransfer = new CreateOrEditGLTransferDto();
            this.glTransfer.id = GLTransferId;
            this.glTransfer.status = true;
            this.docDate = new Date();
            this.transferDate = new Date();
            this._glTransferServiceProxy.getMaxGLTransferId().subscribe(result => {
                this.glTransfer.docid = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            this.editMode = true;
            debugger;
            this._glTransferServiceProxy.getGLTransferForEdit(GLTransferId).subscribe(result => {
                debugger;
                this.glTransfer = result.glTransfer;
                console.log(this.glTransfer);
                this.docDate = moment(this.glTransfer.docdate).toDate();
                this.transferDate = moment(this.glTransfer.transferdate).toDate();


                if (this.glTransfer.fromlocid) {
                    this._glTransferServiceProxy.getLocationName(this.glTransfer.fromlocid, "FromLoc")
                        .subscribe(result => {
                            this.fromLocDesc = result;
                        });
                }
                if (this.glTransfer.tolocid) {
                    this._glTransferServiceProxy.getLocationName(this.glTransfer.tolocid, "ToLoc")
                        .subscribe(result => {
                            this.toLocDesc = result;
                        });
                }
                if (this.glTransfer.frombankid) {
                    this._glTransferServiceProxy.getBankName(this.glTransfer.frombankid, "FromBank")
                        .subscribe(result => {
                            this.fromBankName = result;
                        });
                }
                if (this.glTransfer.tobankid) {
                    this._glTransferServiceProxy.getBankName(this.glTransfer.tobankid, "ToBank")
                        .subscribe(result => {
                            this.toBankName = result;
                        });
                }
                if (this.glTransfer.frombankaccid) {
                    this._glTransferServiceProxy.getChartOfAccountName(this.glTransfer.frombankaccid, "FromBankAcc")
                        .subscribe(result => {
                            this.fromBankDesc = result;
                        });
                }

                if (this.glTransfer.tobankaccid) {
                    this._glTransferServiceProxy.getChartOfAccountName(this.glTransfer.tobankaccid, "ToBankAcc")
                        .subscribe(result => {
                            this.toBankDesc = result;
                        });
                }

                if (this.glTransfer.fromaccid) {
                    this._glTransferServiceProxy.getChartOfAccountName(this.glTransfer.fromaccid, "FromAcc")
                        .subscribe(result => {
                            this.fromAccDesc = result;
                        });
                }

                if (this.glTransfer.toaccid) {
                    this._glTransferServiceProxy.getChartOfAccountName(this.glTransfer.toaccid, "ToAcc")
                        .subscribe(result => {
                            this.toAccDesc = result;
                        });
                }


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        debugger;


        this.glTransfer.audtdate = moment();
        this.glTransfer.audtuser = this.appSession.user.userName;

        this.glTransfer.docdate = moment(this.docDate);
        this.glTransfer.transferdate = moment(this.transferDate);

        if (!this.glTransfer.id) {
            this.glTransfer.createdOn = moment();
            this.glTransfer.createdBy = this.appSession.user.userName;
        }

        this._glTransferServiceProxy.createOrEdit(this.glTransfer)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                debugger;
                this.notify.info(this.l('SavedSuccessfully'));
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
                this.getNewAccount();
                break;
            case "ChequeBookDetail":
                this.getChequeBookDetail();
            default:
                break;

        }
    }

    getChequeBookDetail() {
        debugger
        this.glTransfer.chNumber = this.FinanceLookupTableModal.id;
    }
    getNewCommonServiceModal() {
        debugger;
        switch (this.target) {
            case "Bank":
                this.getNewBank();
                break;
            default:
                break;

        }
    }
    //---------------------Location-------------------------//
    openLocationModal(type: string) {

        this.target = "GLLocation";
        if (type == "FromLoc") {
            this.FinanceLookupTableModal.id = String(this.glTransfer.fromlocid);
            this.FinanceLookupTableModal.displayName = this.fromLocDesc;
            this.FinanceLookupTableModal.show(this.target);
        }
        else if (type == "ToLoc") {
            this.FinanceLookupTableModal.id = String(this.glTransfer.tolocid);
            this.FinanceLookupTableModal.displayName = this.toLocDesc;
            this.FinanceLookupTableModal.show(this.target);
        }
        this.picker = type;

    }
    setLocationNull(type: string) {
        if (type == "FromLoc") {
            this.glTransfer.fromlocid = null;
            this.fromLocDesc = "";
        }
        else if (type == "ToLoc") {
            this.glTransfer.tolocid = null;
            this.toLocDesc = "";
        }
    }
    getNewLocation() {
        debugger;
        if (this.picker == "FromLoc") {
            this.glTransfer.fromlocid = Number(this.FinanceLookupTableModal.id);
            this.fromLocDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (this.picker == "ToLoc") {
            this.glTransfer.tolocid = Number(this.FinanceLookupTableModal.id);
            this.toLocDesc = this.FinanceLookupTableModal.displayName;
        }
    }
    //------------------------------------------------------//
    //=====================Chart of Ac Model================
    openAccountModal(type: string) {
        debugger;
        this.target = "ChartOfAccount";
        if (type == "FromAcc") {
            this.FinanceLookupTableModal.id = this.glTransfer.fromaccid;
            this.FinanceLookupTableModal.displayName = this.fromAccDesc;
            this.FinanceLookupTableModal.show(this.target, "false");
        }

        else if (type == "ToAcc") {
            this.FinanceLookupTableModal.id = this.glTransfer.toaccid;
            this.FinanceLookupTableModal.displayName = this.toAccDesc;
            this.FinanceLookupTableModal.show(this.target, "false");
        }
        this.picker = type;
    }

    setAccountNull(type: string) {
        if (type == "FromAcc") {
            this.glTransfer.fromaccid = "";
            this.fromAccDesc = "";
        }

        else if (type == "ToAcc") {
            this.glTransfer.toaccid = "";
            this.toAccDesc = "";
        }
    }

    getNewAccount() {
        debugger;
        if (this.picker == "FromAcc") {
            this.glTransfer.fromaccid = this.FinanceLookupTableModal.id;
            this.fromAccDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (this.picker == "ToAcc") {
            this.glTransfer.toaccid = this.FinanceLookupTableModal.id;
            this.toAccDesc = this.FinanceLookupTableModal.displayName;
        }
    }
    //=====================Chart of Ac Model================//
    //=====================Chart of Ac Model================
    openBankModal(type: string) {
        debugger;
        this.target = "Bank";
        if (type == "FromBank") {
            this.commonServiceLookupTableModal.id = this.glTransfer.frombankid;
            this.commonServiceLookupTableModal.displayName = this.fromBankName;
            this.commonServiceLookupTableModal.accountId = this.glTransfer.frombankaccid;
            this.commonServiceLookupTableModal.show(this.target);
        }

        else if (type == "ToBank") {
            this.commonServiceLookupTableModal.id = this.glTransfer.tobankid;
            this.commonServiceLookupTableModal.displayName = this.toBankName;
            this.commonServiceLookupTableModal.accountId = this.glTransfer.tobankaccid;
            this.commonServiceLookupTableModal.show(this.target);
        }
        this.picker = type;
    }

    setBankNull(type: string) {
        if (type == "FromBank") {
            this.glTransfer.frombankid = "";
            this.fromBankName = "";
            this.glTransfer.frombankaccid = "";
            this.fromBankDesc = "";
        }

        else if (type == "ToBank") {
            this.glTransfer.tobankid = "";
            this.toBankName = "";
            this.glTransfer.tobankaccid = "";
            this.toBankDesc = "";
        }
    }


    openInstrumentNo() {
        debugger
        if (this.glTransfer.frombankaccid != "") {
            this.target = "ChequeBookDetail";
            this.FinanceLookupTableModal.id = "";
            this.FinanceLookupTableModal.displayName = "";
            this.FinanceLookupTableModal.show("ChequeBookDetail", this.glTransfer.frombankaccid, "", " Instrument No");
            //this.FinanceLookupTableModal.show(this.target,this.accountIdC);
        }
        else {
            this.message.confirm("Please select bank account first");
        }
    }

    getNewBank() {
        debugger;
        if (this.picker == "FromBank") {
            this.glTransfer.frombankid = this.commonServiceLookupTableModal.id;
            this.fromBankName = this.commonServiceLookupTableModal.displayName;
            this.glTransfer.frombankaccid = this.commonServiceLookupTableModal.accountId;
            if (this.glTransfer.frombankaccid) {
                this._glTransferServiceProxy.getChartOfAccountName(this.glTransfer.frombankaccid, "FromBankAcc")
                    .subscribe(result => {
                        this.fromBankDesc = result;
                    });
            }
        }
        else if (this.picker == "ToBank") {
            this.glTransfer.tobankid = this.commonServiceLookupTableModal.id;
            this.toBankName = this.commonServiceLookupTableModal.displayName;
            this.glTransfer.tobankaccid = this.commonServiceLookupTableModal.accountId;
            if (this.glTransfer.tobankaccid) {
                this._glTransferServiceProxy.getChartOfAccountName(this.glTransfer.tobankaccid, "ToBankAcc")
                    .subscribe(result => {
                        this.toBankDesc = result;
                    });
            }
        }
    }
    //=====================Chart of Ac Model================//

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
