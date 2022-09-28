import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    OnInit
} from "@angular/core";
// import { InventoryGlLinkDto } from '../shared/dto/inventory-glLink-dto';
// import { InventoryGlLinkService } from '../shared/services/inventory-gl-link.service';
import { AppComponentBase } from "@shared/common/app-component-base";

// import { InventoryGlLinkLookupTableModalComponent } from '../FinderModals/InventoryGlLink-lookup-table-modal.component';
// import { IcSegment1ServiceProxy } from '../shared/services/ic-segment1-service';
import { FinanceLookupTableModalComponent } from "@app/finders/finance/finance-lookup-table-modal.component";
import { InventoryLookupTableModalComponent } from "@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component";
import { ModalDirective, esLocale } from "ngx-bootstrap";
import { GlChequesService } from "@app/main/finance/shared/services/glCheques.service";
import { GlChequesDto } from "@app/main/finance/shared/dto/GlCheques-dto";
import { stringify } from "querystring";
import { CommonServiceLookupTableModalComponent } from "@app/finders/commonService/commonService-lookup-table-modal.component";
import { VoucherEntryServiceProxy, CreateOrEditGLTRHeaderDto, CreateOrEditGLTRDetailDto, VoucherEntryDto, FiscalCalendersServiceProxy } from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs/operators";
import * as moment from "moment";
import { NullAstVisitor } from "@angular/compiler";

@Component({
    selector: "glCheques",
    templateUrl: "./create-or-edit-GlCheques-modal.component.html"
})
export class CreateOrEditGlChequesModalComponent extends AppComponentBase implements OnInit {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    //@ViewChild('InventoryGlLinkLookupTableModal', { static: true }) InventoryGlLinkLookupTableModal: InventoryGlLinkLookupTableModalComponent;
    @ViewChild("FinanceLookupTableModal", { static: true })
    financeLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild("InventoryLookupTableModal", { static: true })
    inventoryLookupTableModal: InventoryLookupTableModalComponent;
    @ViewChild("commonServiceLookupTableModal", { static: true })
    commonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
    gltrHeader: CreateOrEditGLTRHeaderDto = new CreateOrEditGLTRHeaderDto();
    gltrDetail: CreateOrEditGLTRDetailDto = new CreateOrEditGLTRDetailDto();
    gltrDetailArr: CreateOrEditGLTRDetailDto[] = Array<CreateOrEditGLTRDetailDto>();
    voucherEntry: VoucherEntryDto = new VoucherEntryDto();
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    formValid: boolean = false;
    active = false;
    saving = false;
    transCheck: boolean = false;
    glCheques: GlChequesDto = new GlChequesDto();
    target: any;
    type: String;
    editMode: boolean = false;
    validDate: boolean = false;
    constructor(
        injector: Injector,
        private _voucherEntryServiceProxy: VoucherEntryServiceProxy,
        private _fiscalCalendarsServiceProxy: FiscalCalendersServiceProxy,
        //private _inventoryGlLinkservice: InventoryGlLinkService,
        //private _IcSegment1ServiceProxy: IcSegment1ServiceProxy,
        private _service: GlChequesService
    ) {
        super(injector);
    }
    ngOnInit(): void {
        // this.chequeDate(null);
    }

    getDocNo() {
        this._service.getDocNo(this.glCheques.typeID).subscribe(res => {
            this.glCheques.docID = res["result"];
        });
    }

    approve() {
        this.voucherEntry.gltrHeader = new CreateOrEditGLTRHeaderDto();
        this.voucherEntry.gltrDetail = new Array<CreateOrEditGLTRDetailDto>();
        this._service.getGlConfigAccount(
            this.glCheques.typeID == "1" ? "BR" : "BP"
            , this.glCheques.bankAccountID).subscribe(
                data => {
                    this._voucherEntryServiceProxy
                        .getMaxDocId(
                            this.glCheques.typeID == "1" ? "BR" : "BP",
                            true,
                            moment(this.glCheques.chequeDate).format("LLLL")
                        )
                        .subscribe(result => {
                            this.voucherEntry.gltrHeader.docNo = this.glCheques.docID;
                            this.voucherEntry.gltrHeader.fmtDocNo = result;
                            this.voucherEntry.gltrHeader.docDate = moment(this.glCheques.chequeDate);
                            this.voucherEntry.gltrHeader.docMonth = moment(this.glCheques.chequeDate).month() + 1;
                            this.voucherEntry.gltrHeader.amount = this.glCheques.chequeAmt;
                            this.voucherEntry.gltrHeader.configID = this.glCheques.configID;
                            this.voucherEntry.gltrHeader.chNumber = this.glCheques.chequeNo;
                            this.voucherEntry.gltrHeader.reference = this.glCheques.partyName;
                            this.voucherEntry.gltrHeader.chType = this.glCheques.chType;
                            this.voucherEntry.gltrHeader.locId = this.glCheques.locationID;
                            this.voucherEntry.gltrHeader.approved = true;
                            this.voucherEntry.gltrHeader.posted = false;
                            this.voucherEntry.gltrHeader.bookID = this.glCheques.typeID == "1" ? "BR" : "BP";
                            this.voucherEntry.gltrHeader.auditTime = moment();
                            this.voucherEntry.gltrHeader.auditUser = this.appSession.user.userName;
                            this.voucherEntry.gltrHeader.narration = "PDC - " +
                                (this.glCheques.typeID == "1" ? "AR" : "AP")
                                + " Voucher No :" + this.glCheques.docID.toString() + ", Party Name: "
                                + this.glCheques.partyName;

                            //if(this.glCheques.typeID=="1"){
                            debugger
                            this.gltrDetailArr = new Array<CreateOrEditGLTRDetailDto>();
                            this.gltrDetail = new CreateOrEditGLTRDetailDto();
                            this.gltrDetail.accountID = data["result"];
                            this.gltrDetail.subAccID = 0;
                            this.gltrDetail.amount = this.glCheques.typeID == "1" ?
                                this.voucherEntry.gltrHeader.amount : -this.voucherEntry.gltrHeader.amount;
                            this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                            this.gltrDetail.isAuto = true;
                            this.gltrDetail.locId = this.glCheques.locationID;
                            this.gltrDetail.narration = this.glCheques.remarks;
                            this.gltrDetailArr.push(this.gltrDetail);

                            this.gltrDetail = new CreateOrEditGLTRDetailDto();
                            this.gltrDetail.accountID = this.glCheques.accountID;
                            this.gltrDetail.subAccID = this.glCheques.partyID;
                            this.gltrDetail.amount = this.glCheques.typeID == "1" ?
                                -this.voucherEntry.gltrHeader.amount : this.voucherEntry.gltrHeader.amount;
                            this.gltrDetail.chequeNo = this.voucherEntry.gltrHeader.chNumber;
                            this.gltrDetail.isAuto = false;
                            this.gltrDetail.locId = this.glCheques.locationID;
                            this.gltrDetail.narration = this.glCheques.remarks;
                            this.gltrDetailArr.push(this.gltrDetail);

                            this.voucherEntry.gltrDetail.unshift(...this.gltrDetailArr);
                            //  }

                            if (!this.voucherEntry.gltrHeader.id) {
                                this.gltrHeader.createdOn = moment();
                                this.gltrHeader.createdBy = this.appSession.user.userName;
                            }
                            debugger;
                            this._service.ProcessVoucherEntry(this.voucherEntry)
                                .pipe(finalize(() => { this.saving = false; }))
                                .subscribe((res) => {
                                    debugger
                                    //this.notify.info(this.l('SavedSuccessfully'));
                                    //this.message.confirm("Press 'Yes' for create new voucher",this.l('SavedSuccessfully'),(isConfirmed) => {
                                    // if (isConfirmed) {
                                    this.glCheques.linkDetID = res["result"][0]["id"];
                                    this.glCheques.posted = true;
                                    //     this.save();
                                    //     //this.modalSave.emit(null); 
                                    // }else{
                                    this.save();
                                    //  }
                                    // });
                                });
                        });
                });
    }

    getDateParams(val?) {
        debugger
        this._fiscalCalendarsServiceProxy.getFiscalYearStatus(moment(this.glCheques.chequeDate), this.type == " (AR)" ? 'AR' : 'AP').subscribe(
            result => {
                if (result == true) {
                    this.validDate = true;
                    //this.gltrHeader.docMonth = moment(this.gltrHeader.docDate).month() + 1;
                    //this.DocYear = moment(this.gltrHeader.docDate).year();
                    // this._voucherEntryServiceProxy
                    // 	.getMaxDocId(this.gltrHeader.bookID, true, moment(this.gltrHeader.docDate).format("LLLL"))
                    // 	.subscribe(result => {
                    // 		this.gltrHeader.fmtDocNo = "";
                    // 		this.gltrHeader.fmtDocNo = result;
                    // 	});
                }
                else {
                    this.notify.info("This Date Is Locked");
                    this.validDate = false;
                    // this.gltrHeader.docDate = moment();
                    // this.gltrHeader.docMonth = moment(this.gltrHeader.docDate).month() + 1;
                    // this.DocYear = moment(this.gltrHeader.docDate).year();
                }
                this.checkFormValid();
            }
        )
    }

    show(id?: number, type?: string): void {
        this.type = type;
        this.active = true;
        this.transCheck = false;
        if (!id) {
            debugger
            this.editMode = false;
           
            
            //this.formValid = false;
            this.glCheques = new GlChequesDto();
            this.glCheques.posted = false;
            if (this.type == " (AR)") {
                this.glCheques.typeID = "1";
                this.glCheques.chequeStatus='1';
            }
            else if (this.type == " (AP)") {
                this.glCheques.typeID = "0";
                this.glCheques.chequeStatus='2';
            }
            //this.glCheques.typeID = "0";
            this.getDocNo();
            this.getDateParams();
            this.formValid = false;
            //this.glCheques.chequeStatus = "1";
            this.glCheques.entryDate = new Date();
            // this.getDateParams();
        } else {
            this.editMode = true;
            this.primengTableHelper.showLoadingIndicator();
            this._service.getDataForEdit(id).subscribe(data => {
                this.glCheques.id = data["result"]["glCheque"]["id"];
                this.glCheques.locationID =
                    data["result"]["glCheque"]["locationID"];
                this.glCheques.locDesc = data["result"]["glCheque"]["locDesc"];
                this.glCheques.typeID = data["result"]["glCheque"]["typeID"];
                this.glCheques.accountID =
                    data["result"]["glCheque"]["accountID"];
                this.glCheques.accountName =
                    data["result"]["glCheque"]["accountName"];
                this.glCheques.bankID = data["result"]["glCheque"]["bankID"];
                this.glCheques.bankName =
                    data["result"]["glCheque"]["bankName"];
                this.glCheques.chequeDate = new Date(
                    data["result"]["glCheque"]["chequeDate"]
                );
                this.glCheques.chequeAmt =
                    data["result"]["glCheque"]["chequeAmt"];
                this.glCheques.chequeStatus =
                    data["result"]["glCheque"]["chequeStatus"];
                this.glCheques.docID = data["result"]["glCheque"]["docID"];
                this.glCheques.entryDate = new Date(
                    data["result"]["glCheque"]["entryDate"]
                );
                this.glCheques.partyBank =
                    data["result"]["glCheque"]["partyBank"];
                this.glCheques.partyID = data["result"]["glCheque"]["partyID"];
                this.glCheques.partyName =
                    data["result"]["glCheque"]["partyName"];
                this.glCheques.remarks = data["result"]["glCheque"]["remarks"];
                this.glCheques.chequeNo =
                    data["result"]["glCheque"]["chequeNo"];
                this.glCheques.chType =
                    data["result"]["glCheque"]["chType"];
                this.glCheques.bankAccountID =
                    data["result"]["glCheque"]["bankAccountID"];
                this.glCheques.configID =
                    data["result"]["glCheque"]["configID"];
                this.glCheques.posted = data["result"]["glCheque"]["posted"];
                this.glCheques.statusDate = data["result"]["glCheque"]["statusDate"] != undefined ? new Date(data["result"]["glCheque"]["statusDate"]) : undefined;
                this.getDateParams();

            });
        }
        this.modal.show();
        //this.getDateParams();

        // console.log("In show modal");
        // debugger;
        // setTimeout(() => {
        //     if (this.glCheques.chequeDate != undefined && this.glCheques.entryDate != undefined &&
        //         this.glCheques.chequeDate.toLocaleDateString() < this.glCheques.entryDate.toLocaleDateString()) {
        //         // this.validDate = false;
        //         this.message.warn("Cheque date cannot be less than entry date");
        //     }
        // }, 2000);
    }

    handleChange(event: any) {
        debugger;
        this.checkFormValid();
    }

    save(): void {
        this.saving = true;


        //this.glCheques.entryDate.setDate(this.glCheques.entryDate.getDate() + 1);
        //this.glCheques.chequeDate.setDate(this.glCheques.chequeDate.getDate() + 1);

        if (moment(new Date()).format("A") === "AM" &&
            !this.glCheques.id && (moment(new Date()).month() + 1) == this.glCheques.entryDate.getMonth()) {

            this.glCheques.entryDate = this.glCheques.entryDate;
        }
        else {
            this.glCheques.entryDate = moment(this.glCheques.entryDate).endOf('day').toDate();
        }



        if (moment(new Date()).format("A") === "AM" &&
            !this.glCheques.id && (moment(new Date()).month() + 1) == this.glCheques.chequeDate.getMonth()) {

            this.glCheques.chequeDate = this.glCheques.chequeDate;
        }
        else {
            this.glCheques.chequeDate = moment(this.glCheques.chequeDate).endOf('day').toDate();
        }



        if (this.glCheques.chequeStatus == "4" || this.glCheques.chequeStatus == "3") {

            //this.glCheques.statusDate.setDate(this.glCheques.statusDate.getDate() + 1);

            //debugger;
            if (moment(new Date()).format("A") === "AM" &&
                !this.glCheques.id && (moment(new Date()).month() + 1) == this.glCheques.statusDate.getMonth()) {

                this.glCheques.statusDate = this.glCheques.statusDate;
            }
            else {
                this.glCheques.statusDate = moment(this.glCheques.statusDate).endOf('day').toDate();
            }


        }

        this._service.create(this.glCheques).subscribe(() => {
            this.saving = false;
            this.notify.info(this.l("SavedSuccessfully"));
            this.close();
            this.modalSave.emit(null);
        });
        this.close();
    }

    openModal(type: string) {
        this.target = type;
        this.inventoryLookupTableModal.id = "";
        this.inventoryLookupTableModal.displayName = "";
        this.financeLookupTableModal.id = "";
        this.financeLookupTableModal.displayName = "";
        this.commonServiceLookupTableModal.id = "";
        this.commonServiceLookupTableModal.displayName = "";
        switch (type) {
            case "AccId":
                this.financeLookupTableModal.show("ChartOfAccount", "true");
                break;
            case "Loc":
                this.financeLookupTableModal.show("GLLocation");
                break;
            case "Party":
                this.glCheques.accountID != ""
                    ? this.financeLookupTableModal.show(
                        "SubLedger",
                        this.glCheques.accountID
                    )
                    : this.notify.info("First Select Account");
                break;
            case "Bank":
                this.commonServiceLookupTableModal.show("Bank");
                break;
                this.checkFormValid();
        }
    }

    getData() {
        switch (this.target) {
            case "AccId":
                this.glCheques.accountID = this.financeLookupTableModal.id;
                this.glCheques.accountName = this.financeLookupTableModal.displayName;
                this.glCheques.partyID = undefined;
                break;
            case "Party":
                this.glCheques.partyID =
                    Number(this.financeLookupTableModal.id) == 0
                        ? undefined
                        : Number(this.financeLookupTableModal.id);
                this.glCheques.partyName = this.financeLookupTableModal.displayName;
                break;
            case "Loc":
                this.glCheques.locationID =
                    Number(this.financeLookupTableModal.id) == 0
                        ? undefined
                        : Number(this.financeLookupTableModal.id);
                this.glCheques.locDesc = this.financeLookupTableModal.displayName;
                break;
            case "Bank":
                this.glCheques.bankID = this.commonServiceLookupTableModal.id;
                this.glCheques.bankName = this.commonServiceLookupTableModal.displayName;
                this.glCheques.bankAccountID = this.commonServiceLookupTableModal.accountId;
                this._service.getGlConfigId(this.glCheques.typeID == "1" ? "BR" : "BP", this.glCheques.bankAccountID).subscribe
                    (
                        data => {
                            this.glCheques.configID = data["result"];
                        }
                    )
                break;
        }
        this.checkFormValid();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
    chequeDate(event: any) {

        this.getDateParams(null);
        debugger;
        this.glCheques.chequeDate = event;
        this.glCheques.chequeDate = new Date(this.glCheques.chequeDate.getFullYear(), this.glCheques.chequeDate.getMonth(), this.glCheques.chequeDate.getDate())
        this.glCheques.entryDate = new Date(this.glCheques.entryDate.getFullYear(), this.glCheques.entryDate.getMonth(), this.glCheques.entryDate.getDate())


        if (this.glCheques.statusDate != undefined) {

            if (this.glCheques.chequeDate < this.glCheques.entryDate &&
                this.glCheques.chequeDate > this.glCheques.statusDate
            ) {
                //this.validDate = false;
                this.message.warn("Cheque date cannot be less than entry date and greater than clearing date");

            }
            else if (this.glCheques.chequeDate < this.glCheques.entryDate) {
                this.message.warn("Cheque date cannot be less than entry date");
            }
            else {
                this.statusDate(this.glCheques.statusDate);
            }
        }
        else if (this.glCheques.chequeDate < this.glCheques.entryDate) {
            //this.validDate = false;
            this.message.warn("Cheque date cannot be less than entry date");

        }

        this.checkFormValid();
    }
    entryDate(event: any) {
        this.getDateParams(null);
        this.glCheques.entryDate = event;
        this.glCheques.chequeDate = new Date(this.glCheques.chequeDate.getFullYear(), this.glCheques.chequeDate.getMonth(), this.glCheques.chequeDate.getDate())
        this.glCheques.entryDate = new Date(this.glCheques.entryDate.getFullYear(), this.glCheques.entryDate.getMonth(), this.glCheques.entryDate.getDate())
        debugger;
        if (this.glCheques.chequeDate != undefined &&
            this.glCheques.chequeDate < this.glCheques.entryDate) {
            // this.validDate = false;
            this.message.warn("Cheque date cannot be less than entry date");
        }

        this.checkFormValid();
    }

    statusDate(event: any) {
        this.getDateParams(null);
        this.glCheques.statusDate = event;
        this.glCheques.statusDate = new Date(this.glCheques.statusDate.getFullYear(), this.glCheques.statusDate.getMonth(), this.glCheques.statusDate.getDate());
        this.glCheques.chequeDate = new Date(this.glCheques.chequeDate.getFullYear(), this.glCheques.chequeDate.getMonth(), this.glCheques.chequeDate.getDate());
        debugger;
        if (this.glCheques.statusDate != undefined &&
            this.glCheques.statusDate < this.glCheques.chequeDate) {
            // this.validDate = false;
            this.message.warn("Clearing date cannot be less than cheque date");
        }

        this.checkFormValid();
    }

    chequeStatus(event: any) {
        this.checkFormValid();
    }

    checkFormValid() {
        debugger;
        if (
            this.glCheques.docID == undefined ||
            this.glCheques.typeID == undefined ||
            this.glCheques.partyID == undefined ||
            this.glCheques.accountID == undefined ||
            this.glCheques.accountID == "" ||
            this.glCheques.entryDate == undefined ||
            this.glCheques.chequeDate == undefined ||
            this.glCheques.locationID == undefined ||
            this.glCheques.locDesc == undefined ||
            this.glCheques.bankID == undefined ||
            this.glCheques.bankID == "" ||
            this.glCheques.chequeNo == undefined ||
            this.glCheques.chequeNo == "" ||
            this.glCheques.chequeDate == undefined ||
            this.glCheques.entryDate == undefined ||
            this.glCheques.chequeAmt == undefined ||
            this.glCheques.remarks == undefined ||
            this.glCheques.remarks == "" ||
            this.glCheques.bankAccountID == undefined ||
            this.glCheques.chType == undefined ||
            this.glCheques.chequeStatus == undefined ||
            this.validDate == false ||
            (this.glCheques.chequeDate != undefined &&
                this.glCheques.chequeDate < this.glCheques.entryDate) ||
            ((this.glCheques.chequeStatus == "4" || this.glCheques.chequeStatus == "3") &&
                this.glCheques.statusDate == undefined) ||
            (this.glCheques.statusDate != undefined &&
                this.glCheques.statusDate < this.glCheques.chequeDate)
        ) {
            this.formValid = false;
        } else {
            this.formValid = true;
        }
    }
}
