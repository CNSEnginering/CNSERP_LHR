import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    Input,
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { finalize } from "rxjs/operators";
import {
    AccountSubLedgersServiceProxy,
    CreateOrEditAccountSubLedgerDto,
} from "@shared/service-proxies/service-proxies";
import { AppComponentBase } from "@shared/common/app-component-base";
import * as moment from "moment";
import { AccountSubLedgerChartofControlLookupTableModalComponent } from "./accountSubLedger-chartofControl-lookup-table-modal.component";
import { AccountSubLedgerTaxAuthorityLookupTableModalComponent } from "./accountSubLedger-taxAuthority-lookup-table-modal.component";
import { AccountSubledgerLookupComponent } from "./account-subledger-lookup.component";
import { THIS_EXPR } from "@angular/compiler/src/output/output_ast";
import { ItemPricingLookupTableModalComponent } from "@app/main/supplyChain/inventory/FinderModals/itemPricing-lookup-table-modal.component";
import { FinanceLookupTableModalComponent } from "@app/finders/finance/finance-lookup-table-modal.component";
import { CommonServiceLookupTableModalComponent } from "@app/finders/commonService/commonService-lookup-table-modal.component";
import { LegderTypeComboboxService } from "@app/shared/common/legdertype-combobox/legdertype-combobox.service";
import { ItemPricingDto } from "@app/main/supplyChain/inventory/shared/dto/itemPricing-dto";

import { ItemPricingService } from "@app/main/supplyChain/inventory/shared/services/itemPricing.service";

// import { CommonModule } from '@angular/common';
// import { NgModule } from '@angular/core';
// import { FormsModule } from '@angular/forms';
// import { CustomerComponent } from './customer.component';
// import { NewItemDirective } from './new-item.directive';
// import { OrdersPipe } from './orders.pipe';

// import{NgModule} from '@angular/core';
// import {CommonModule} from '@angular/common';
// @NgModule({
//    imports:[
//      CommonModule,
//      ItemPricingLookupTableModalComponent

//    ]
// })


@Component({
    selector: "createOrEditAccountSubLedgerModal",
    templateUrl: "./create-or-edit-accountSubLedger-modal.component.html",
})


export class CreateOrEditAccountSubLedgerModalComponent extends AppComponentBase {
    @Input() mode: string;
    editMode: boolean = false;
    vendorMaster: boolean;
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild("accountSubLedgerChartofControlLookupTableModal", {
        static: true,
    })
    itemPricingDto: ItemPricingDto = new ItemPricingDto();
    itemPricingDetailData: ItemPricingDto[] = new Array<ItemPricingDto>();
    
    accountSubLedgerChartofControlLookupTableModal: AccountSubLedgerChartofControlLookupTableModalComponent;
    @ViewChild("accountSubLedgerTaxAuthorityLookupTableModal", { static: true })
    accountSubLedgerTaxAuthorityLookupTableModal: AccountSubLedgerTaxAuthorityLookupTableModalComponent;
    @ViewChild("ItemPricingLookupTableModal", { static: true })
    ItemPricingLookupTableModal: ItemPricingLookupTableModalComponent;
    @ViewChild("accountSubledgerLookup", { static: true })
    accountSubledgerLookup: AccountSubledgerLookupComponent;
    @ViewChild("FinanceLookupTableModal", { static: true })
    FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild("ParentaccountModal", { static: true })
    ParentaccountModal: FinanceLookupTableModalComponent;
    @ViewChild("CommonServiceLookupTableModal", { static: true })
    CommonServiceLookupTableModal: CommonServiceLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    data: any;
    active = false;
    saving = false;
    flag = false;

    accountSubLedger: CreateOrEditAccountSubLedgerDto = new CreateOrEditAccountSubLedgerDto();
   
    audtdate: Date;
    priceList: string;
    itemPriceLIst="";
    itemPriceID="0";

    chartofControlAccountName = "";
    taxAuthorityTAXAUTHDESC = "";
    salesTaxAuthDESC: string;
    picker: string;
    TaxClasses: any[];
 
    SLGrp = "";
    linked = false;
    LinkedSubAccountName: string;
    filterText: string;
    editAccountSubLedger = "Edit Sub Ledger";
    createNewAccountSubLedger = "Create Sub Ledger";
    target: string;
    ledgerTypes: any[];
    LinkedAccountName: string;
    salesTaxClasses: any[];

    constructor(
        injector: Injector,
        private _accountSubLedgersServiceProxy: AccountSubLedgersServiceProxy,
        private _LegderTypeComboboxService: LegderTypeComboboxService,
        private _itemPricingService: ItemPricingService
    ) {
        super(injector);
    }
    editCreate(headerEdit: string, headerCreate: string) {
        this.editAccountSubLedger = headerEdit;
        this.createNewAccountSubLedger = headerCreate;
    }

    show(
        IsUpdate: boolean = false,
        accountSubLedgerId?: number,
        AccountID?: string
    ): void {
        this.audtdate = null;
        debugger;
        if (!IsUpdate) {
            this.accountSubLedger.linked = false;
            this.LinkedSubAccountName = "";
            this.LinkedAccountName = "";
            this.getLedgerTypes();
            if (this.vendorMaster == true) {
                if (this.mode == "vendorMaster") {
                    this.createNewAccountSubLedger = "Create Vendor Master";
                    this.accountSubLedger = new CreateOrEditAccountSubLedgerDto();
                    this._accountSubLedgersServiceProxy
                        .getAccountSubLedgerForCreate(this.mode)
                        .subscribe((res) => {
                            this.accountSubLedger.accountID = res.accountId;
                            this.accountSubLedger.id = res.subAccountId;
                            this.chartofControlAccountName = res.accountDesc;
                            this.itemPriceLIst = res.itemPriceLIst;
                            // this.chartofControlAccountName = '';
                            this.taxAuthorityTAXAUTHDESC = "";
                            this.salesTaxAuthDESC = "";
                            this.accountSubLedger.city = "";
                            this.accountSubLedger.ledgerType = 0;
                            this.accountSubLedger.slType = 1;                           
                            this.accountSubLedger.active = true;
                            this.flag = IsUpdate;
                            this.active = true;
                            this.modal.show();
                        });
                } else if (this.mode == "customerMaster") {
                    debugger;
                    this.accountSubLedger = new CreateOrEditAccountSubLedgerDto();

                    this.createNewAccountSubLedger = "Create Customer Master";
                    this._accountSubLedgersServiceProxy
                        .getAccountSubLedgerForCreate(this.mode)
                        .subscribe((res) => {
                            this.accountSubLedger.accountID = res.accountId;
                            this.accountSubLedger.id = res.subAccountId;
                            this.chartofControlAccountName = res.accountDesc;
                            this.itemPriceLIst = res.itemPriceLIst;
                            // this.chartofControlAccountName = '';
                            this.taxAuthorityTAXAUTHDESC = "";
                            this.salesTaxAuthDESC = "";
                            this.accountSubLedger.city = "";
                            this.accountSubLedger.slType = 2;
                            this.accountSubLedger.ledgerType = 0;
                            this.accountSubLedger.active = true;
                            this.flag = IsUpdate;
                            this.active = true;
                            this.modal.show();
                        });
                }
            } else {
                this.accountSubLedger = new CreateOrEditAccountSubLedgerDto();
                this.chartofControlAccountName = "";
                this.itemPriceLIst = "";
                this.taxAuthorityTAXAUTHDESC = "";
                this.salesTaxAuthDESC = "";
                this.accountSubLedger.city = "";
                this.accountSubLedger.countryID = 0;
                this.accountSubLedger.provinceID = 0;
                this.accountSubLedger.slType = 0;
                this.accountSubLedger.ledgerType = 0;
                this.accountSubLedger.active = true;

                this.flag = IsUpdate;
                this.active = true;
                this.modal.show();
            }
        } else {
            debugger;
            this.getLedgerTypes();
            this._accountSubLedgersServiceProxy
                .getAccountSubLedgerForEdit(accountSubLedgerId, AccountID)
                .subscribe((result) => {
                    this.accountSubLedger = result.accountSubLedger;
                    if (this.mode == "customerMaster") {
                        this.editAccountSubLedger = "Edit Customer Master";
                    }
                    if (this.mode == "vendorMaster") {
                        this.editAccountSubLedger = "Create Vendor Master";
                    }
                    if (this.accountSubLedger.audtdate) {
                        this.audtdate = this.accountSubLedger.audtdate.toDate();
                    }

                    // this._accountSubLedgersServiceProxy.getAllAccountSubledger_lookup(
                    //     this.accountSubLedger.parentID,
                    //    '',
                    //    0,
                    //    5

                    // ).subscribe(result => {
                    //     debugger;
                    //     this.LinkedSubAccountName = result.items[0].displayName;
                    // });
                    debugger;

                    this.chartofControlAccountName =
                        result.chartofControlAccountName;
                        this.itemPriceLIst = 
                        result.itemPriceLIst;
                    this.taxAuthorityTAXAUTHDESC =
                        result.taxAuthorityTAXAUTHDESC;
                        if(this.accountSubLedger.itemPriceID!= undefined && this.accountSubLedger.itemPriceID!=null){

                        }
                    if (this.accountSubLedger.sttaxauth) {
                        this._accountSubLedgersServiceProxy
                            .getSalesTaxDesc(this.accountSubLedger.sttaxauth)
                            .subscribe((result) => {
                                this.salesTaxAuthDESC = result;
                            });
                    }
                    this.LinkedAccountName = result.parentAccountName;
                    this.LinkedSubAccountName = result.parentSubAccountName;
                    if (this.accountSubLedger.taxauth != undefined) {
                        this._accountSubLedgersServiceProxy
                            .getAllTaxClassesForCombobox(
                                this.accountSubLedger.taxauth
                            )
                            .subscribe((result) => {
                                this.TaxClasses = result.items;
                            });
                    }
                    if (this.accountSubLedger.sttaxauth != undefined) {
                        this._accountSubLedgersServiceProxy
                            .getAllTaxClassesForCombobox(
                                this.accountSubLedger.sttaxauth
                            )
                            .subscribe((result) => {
                                this.salesTaxClasses = result.items;
                            });
                    }

                    this.flag = IsUpdate;
                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        debugger;
        this.saving = true;
        this.accountSubLedger.flag = this.flag;
        //this.accountSubLedger.linked = this.linked;
        if (this.audtdate) {
            if (!this.accountSubLedger.audtdate) {
                this.accountSubLedger.audtdate = moment(this.audtdate).startOf(
                    "day"
                );
            } else {
                this.accountSubLedger.audtdate = moment(this.audtdate);
            }
        } else {
            this.accountSubLedger.audtdate = null;
        }
        this.accountSubLedger.subAccID = this.accountSubLedger.id.toString();

        this._accountSubLedgersServiceProxy
            .createOrEdit(this.accountSubLedger)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.message.confirm(
                    "Press 'Yes' for create new subledger",
                    this.l("SavedSuccessfully"),
                    (isConfirmed) => {
                        if (isConfirmed) {
                            var accId = this.accountSubLedger.accountID;
                            this.show(false);
                            this.modalSave.emit(null);
                            this.accountSubLedger.accountID = accId;
                            this._accountSubLedgersServiceProxy
                                .maxid(accId)
                                .subscribe((result) => {
                                    this.accountSubLedger.id = result;
                                });
                        } else {
                            //this.notify.info(this.l('SavedSuccessfully'));
                            this.close();
                            this.modalSave.emit(null);
                        }
                    }
                );
            });
    }
    getNewFinanceModal() {
        debugger;
        switch (this.target) {
            case "ChartOfAccount":
                this.getNewChartofControlId();
                break;
        case "SLGrp":
                this.getslGrp();
                break;    

            default:
                break;
        }
    }
    getNewCommonServiceModal() {
        debugger;
        switch (this.target) {
            case "TaxAuthority":
                this.getNewTaxAuthorityId();
                break;

            default:
                break;
        }
    }
    
    

    openSelectChartofControlModal() {

        debugger;
        this.target = "ChartOfAccount";
        this.FinanceLookupTableModal.id = this.accountSubLedger.accountID;
        this.FinanceLookupTableModal.displayName = this.chartofControlAccountName;
        if (this.mode == "customerMaster") {
            this.FinanceLookupTableModal.show(this.target, "", "Customer");

        } 
        else if (this.mode == "vendorMaster") {
            this.FinanceLookupTableModal.show(this.target, "", "Vendor");
        } else this.FinanceLookupTableModal.show(this.target, "true");
    }


    openSelectPriceListModal() {
debugger;
      
        this.target = "PRICEList";
        this.FinanceLookupTableModal.id = this.accountSubLedger.accountID;
        this.FinanceLookupTableModal.displayName = this.chartofControlAccountName;
        if (this.mode == "customerMaster") {
            this.FinanceLookupTableModal.show(this.target, "", "Customer");

        } 
        else if (this.mode == "vendorMaster") {
            this.FinanceLookupTableModal.show(this.target, "", "Vendor");
        } else this.FinanceLookupTableModal.show(this.target, "true");
    }


  

    openSelectTaxAuthorityModal(type: string) {
        this.target = "TaxAuthority";
        if (type == "Income") {
            this.CommonServiceLookupTableModal.id = this.accountSubLedger.taxauth;
            this.CommonServiceLookupTableModal.displayName = this.taxAuthorityTAXAUTHDESC;
        } else if (type == "Sales") {
            this.CommonServiceLookupTableModal.id = this.accountSubLedger.sttaxauth;
            this.CommonServiceLookupTableModal.displayName = this.salesTaxAuthDESC;
        }
        this.picker = type;
        this.CommonServiceLookupTableModal.show(this.target);
    }

    setChartofControlIdNull() {
        this.accountSubLedger.accountID = null;
        this.chartofControlAccountName = "";
        this.accountSubLedger.id = null;
        this.accountSubLedger.slGrpName = "";
    }
    setGrpIdNull(){
        this.accountSubLedger.slGrpId=null;
        this.accountSubLedger.slGrpName="";
    }

    setTaxAuthorityIdNull(type: string) {
        if (type == "Income") {
            this.accountSubLedger.taxauth = null;
            this.taxAuthorityTAXAUTHDESC = "";
        } else if (type == "Sales") {
            this.accountSubLedger.sttaxauth = null;
            this.salesTaxAuthDESC = "";
        }
    }

    getNewChartofControlId() {
        this.accountSubLedger.accountID = this.FinanceLookupTableModal.id;
        this.chartofControlAccountName = this.FinanceLookupTableModal.displayName;
        if (this.accountSubLedger.accountID != undefined) {
            this.getLedgerTypes();

            debugger;

            this._accountSubLedgersServiceProxy
                .maxid(this.accountSubLedger.accountID)
                .subscribe((result) => {
                    this.accountSubLedger.id = result;
                });
        }
    }

    getNewTaxAuthorityId() {
        debugger;
        if (this.picker == "Income") {
            this.accountSubLedger.taxauth = this.CommonServiceLookupTableModal.id;
            this.taxAuthorityTAXAUTHDESC = this.CommonServiceLookupTableModal.displayName;
        } else if (this.picker == "Sales") {
            this.accountSubLedger.sttaxauth = this.CommonServiceLookupTableModal.id;
            this.salesTaxAuthDESC = this.CommonServiceLookupTableModal.displayName;
        }
        if (this.accountSubLedger.taxauth != undefined) {
            this._accountSubLedgersServiceProxy
                .getAllTaxClassesForCombobox(this.accountSubLedger.taxauth)
                .subscribe((result) => {
                    this.TaxClasses = result.items;
                });
        }

        if (this.accountSubLedger.sttaxauth != undefined) {
            this._accountSubLedgersServiceProxy
                .getAllTaxClassesForCombobox(this.accountSubLedger.sttaxauth)
                .subscribe((result) => {
                    this.salesTaxClasses = result.items;
                });
        }
    }

    getNewSubledgerId() {
        debugger;
        this.accountSubLedger.parentSubID = Number(
            this.accountSubledgerLookup.id
        );
        this.LinkedSubAccountName = this.accountSubledgerLookup.displayName;
    }
    getslGrp(){
        debugger;
        this.accountSubLedger.slGrpId=this.FinanceLookupTableModal.id;
        this.SLGrp=this.FinanceLookupTableModal.displayName;
    }

    getNewParentAccountId() {
        debugger;
        this.accountSubLedger.parentID = this.ParentaccountModal.id;
        this.LinkedAccountName = this.ParentaccountModal.displayName;
    }

    openSelectAccountModal() {
        debugger
        this.ParentaccountModal.id = this.accountSubLedger.parentID;
        this.ParentaccountModal.displayName = this.LinkedAccountName;
        this.ParentaccountModal.show("ChartOfAccount");
    }

    openSLGrpModel(){
        debugger;
        this.target="SLGrp";
        this.FinanceLookupTableModal.id = this.accountSubLedger.slGrpId;
        this.FinanceLookupTableModal.displayName=this.accountSubLedger.slGrpName;
        this.FinanceLookupTableModal.show(this.target, "true");
    }

    openSelectSubLedgerAccountModal() {
        this.accountSubledgerLookup.id = this.accountSubLedger.parentID;
        this.accountSubledgerLookup.displayName = this.LinkedSubAccountName;
        this.accountSubledgerLookup.show(this.accountSubLedger.parentID);
    }

    setAccountSubledgerIdNull(clear: boolean) {
        debugger;
        if (!this.accountSubLedger.linked || !clear) {
            this.accountSubLedger.parentSubID = null;
            this.LinkedSubAccountName = "";
        }
    }

    setAccountIdNull(clear: boolean) {
        debugger;
        if (!this.accountSubLedger.linked || !clear) {
            this.accountSubLedger.parentID = null;
            this.LinkedAccountName = "";
            this.accountSubLedger.parentSubID = null;
            this.LinkedSubAccountName = "";
        }
    }

    close(): void {
        this.flag = false;
        this.active = false;
        this.modal.hide();
    }

    ngOnInit() {
        if (this.mode) {
            this.vendorMaster = true;
        } else {
            this.vendorMaster = false;
        }
        console.log("Create Account Sub Ledger:" + this.mode);
    }

    getLedgerTypes() {
        this._LegderTypeComboboxService
            .getLedgerTypesForCombobox("")
            .subscribe((res) => {
                debugger;
                this.ledgerTypes = res.items;
                this.accountSubLedger.slType = this.FinanceLookupTableModal.sltype;
            });
    }
 //// Item Pricing Modal <mz>///

 openSelectItemPricingModal() {
    debugger;
    this.target = "Item Pricing";
    this.FinanceLookupTableModal.id = this.accountSubLedger.accountID;
    this.FinanceLookupTableModal.displayName = this.chartofControlAccountName;
    if (this.mode == "customerMaster") {
        this.FinanceLookupTableModal.show(this.target, "", "Customer");
    } else if (this.mode == "vendorMaster") {
        this.FinanceLookupTableModal.show(this.target, "", "Vendor");
    } else this.FinanceLookupTableModal.show(this.target, "true");
}
openlookUpModal(type: string) {
    debugger;
    this.ItemPricingLookupTableModal.data = null;
    this.ItemPricingLookupTableModal.show(type);
}
getLookUpData() {
    debugger
    if (this.ItemPricingLookupTableModal.type == "PriceList") {
        debugger;
       this.accountSubLedger.itemPriceID= this.ItemPricingLookupTableModal.data.priceList;
       this.itemPriceLIst= this.ItemPricingLookupTableModal.data.priceListName;
      
        // if (typeof this.ItemPricingLookupTableModal.data != "object") {
        // if (typeof this.ItemPricingLookupTableModal.data != "object") {
        
        // }
    }


    // else {
    //     //if (typeof this.ItemPricingLookupTableModal.data != "object") {
    //     this.itemPricingDto.itemID = this.ItemPricingLookupTableModal.data.itemId;
    //     this.itemPricingDto.itemDesc = this.ItemPricingLookupTableModal.data.descp;
    //     // }
    // }

    // this.checkItemIdAgainstPriceList();
    // this.CheckPriceListExists();
}
setItemPricingIdNull() {
    this.accountSubLedger.itemPriceID=null;

}


// CheckPriceListExists() {
//     debugger;
//     this._itemPricingService
//         .CheckPriceListExists(this.priceList)
//         .subscribe((data) => {
//             if (data["result"] == true) {
//                 this.notify.info("This Price List Already Exists");
//                 this.priceListExists = true;
//             } else {
//                 this.priceListExists = false;
//             }
//             this.checkFormValid();
//         });
// }

}
