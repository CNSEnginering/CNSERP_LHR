import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
// import { InventoryGlLinkDto } from '../shared/dto/inventory-glLink-dto';
// import { InventoryGlLinkService } from '../shared/services/inventory-gl-link.service';
import { AppComponentBase } from '@shared/common/app-component-base';

// import { InventoryGlLinkLookupTableModalComponent } from '../FinderModals/InventoryGlLink-lookup-table-modal.component';
// import { IcSegment1ServiceProxy } from '../shared/services/ic-segment1-service';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { InventoryLookupTableModalComponent } from '@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component';
import { ModalDirective } from 'ngx-bootstrap';
import { SaleAccountsService } from '../shared/services/saleAccounts.service';
import { saleAccountsDto } from '../shared/dtos/saleAccounts-dto';


@Component({
    selector: 'SaleAccountsModal',
    templateUrl: './create-or-edit-SaleAccounts-modal.component.html'
})
export class CreateOrEditSaleAccountsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    //@ViewChild('InventoryGlLinkLookupTableModal', { static: true }) InventoryGlLinkLookupTableModal: InventoryGlLinkLookupTableModalComponent;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild('InventoryLookupTableModal', { static: true }) InventoryLookupTableModal: InventoryLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    formValid:boolean = true;
    active = false;
    saving = false;
    transCheck: boolean = false;
    saleAccounts: saleAccountsDto = new saleAccountsDto();
    target: any;
    editMode:boolean = false;
    LocCheckVal:boolean;

    constructor(
        injector: Injector,
        //private _inventoryGlLinkservice: InventoryGlLinkService,
        //private _IcSegment1ServiceProxy: IcSegment1ServiceProxy,
        private _saleAccountsService: SaleAccountsService,
    ) {
        super(injector);
    }

    show(id?: number): void {
        this.active = true;
        this.transCheck = false;
        if (!id) {
            this.editMode = false;
            this.formValid = true;
            this.saleAccounts = new saleAccountsDto();
        }
        else {
            this.editMode = true;
            this.formValid = false;
             this.primengTableHelper.showLoadingIndicator();
             this._saleAccountsService.getDataForEdit(id).subscribe(data => {
                 console.log(data);
                this.saleAccounts.id = data["result"]["oecoll"]["id"];
                this.saleAccounts.locId = data["result"]["oecoll"]["locID"];
                this.saleAccounts.locDesc = data["result"]["oecoll"]["locName"];
                this.saleAccounts.typeId = data["result"]["oecoll"]["typeID"];
                this.saleAccounts.typeDesc = data["result"]["oecoll"]["typeDesc"];
                this.saleAccounts.salesACC = data["result"]["oecoll"]["salesACC"];
                this.saleAccounts.salesACCDesc = data["result"]["oecoll"]["salesACCDesc"];
                this.saleAccounts.salesRetACC = data["result"]["oecoll"]["salesRetACC"];
                this.saleAccounts.salesRetACCDesc = data["result"]["oecoll"]["salesRetACCDesc"];
                this.saleAccounts.chAccountID = data["result"]["oecoll"]["chAccountID"];
                this.saleAccounts.chAccountIDDesc = data["result"]["oecoll"]["chAccountIDDesc"];
                this.saleAccounts.cogsacc = data["result"]["oecoll"]["cogsacc"];
                this.saleAccounts.cogsaccDesc = data["result"]["oecoll"]["cogsaccDesc"];
                this.saleAccounts.writeOffAcc = data["result"]["oecoll"]["writeOffAcc"];
                this.saleAccounts.writeOffAccDesc = data["result"]["oecoll"]["writeOffAccDesc"];
                this.saleAccounts.discAcc = data["result"]["oecoll"]["discAcc"];
                this.saleAccounts.discAccDesc = data["result"]["oecoll"]["discAccDesc"];
                this.saleAccounts.refundableAcc = data["result"]["oecoll"]["refundableAcc"];
                this.saleAccounts.refundableAccDesc = data["result"]["oecoll"]["refundableAccDesc"];
                this.saleAccounts.payableAcc = data["result"]["oecoll"]["payableAcc"];
                this.saleAccounts.payableAccDesc = data["result"]["oecoll"]["payableAccDesc"];
            });
        }
        this.modal.show();
    }
    SetDefaultRecord(result:any){
        console.log(result);
          this.saleAccounts.locId=result.currentLocID;
          this.saleAccounts.locDesc=result.currentLocName;
          this.saleAccounts.typeId=result.transType;
          this.saleAccounts.typeDesc=result.transTypeName;
          this.LocCheckVal=result.allowLocID;
        
      }

    save(): void {
        this.saving = true;
        this._saleAccountsService.create(this.saleAccounts)
            .subscribe(() => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
        this.close();
    }

    getNewFinanceModal() {
        debugger
        this.getNewChartOfAC(this.target);
        this.checkTransTypeExists();
        this.checkFormValid();
    }
    getNewInventoryModal() {
        if (this.target == "Location")
            this.getNewLocation();
        else if (this.target == "TransactionType")
            this.getNewTransType();

            this.checkFormValid();
    }

    //////////////////////////////////////Chart of Account///////////////////////////////////////////////
    openSelectChartofACModal(ac) {
        this.target = "ChartOfAccount";
        if (ac == "salesACC") {
            this.FinanceLookupTableModal.id = this.saleAccounts.salesACC;
            this.FinanceLookupTableModal.displayName = this.saleAccounts.salesACC;
            this.FinanceLookupTableModal.show(this.target);
        }
        else if (ac == "salesRetACC") {
            this.FinanceLookupTableModal.id = this.saleAccounts.salesRetACC;
            this.FinanceLookupTableModal.displayName = this.saleAccounts.salesRetACC;
            this.FinanceLookupTableModal.show(this.target);
        }
        else if (ac == "cogsacc") {
            this.FinanceLookupTableModal.id = this.saleAccounts.cogsacc;
            this.FinanceLookupTableModal.displayName = this.saleAccounts.cogsacc;
            this.FinanceLookupTableModal.show(this.target);
        }
        else if (ac == "chAccountID") {
            this.FinanceLookupTableModal.id = this.saleAccounts.chAccountID;
            this.FinanceLookupTableModal.displayName = this.saleAccounts.chAccountID;
            this.FinanceLookupTableModal.show(this.target,"true");
        }
        else if (ac == "discAcc") {
            this.FinanceLookupTableModal.id = this.saleAccounts.discAcc;
            this.FinanceLookupTableModal.displayName = this.saleAccounts.discAccDesc;
            this.FinanceLookupTableModal.show(this.target);
        }
        else if (ac == "writeOffAcc") {
            this.FinanceLookupTableModal.id = this.saleAccounts.writeOffAcc;
            this.FinanceLookupTableModal.displayName = this.saleAccounts.writeOffAccDesc;
            this.FinanceLookupTableModal.show(this.target);
        }
        else if (ac == "payableAcc") {
            this.FinanceLookupTableModal.id = this.saleAccounts.payableAcc;
            this.FinanceLookupTableModal.displayName = this.saleAccounts.payableAccDesc;
            this.FinanceLookupTableModal.show(this.target);
        }
        else if (ac == "refundableAcc") {
            this.FinanceLookupTableModal.id = this.saleAccounts.refundableAcc;
            this.FinanceLookupTableModal.displayName = this.saleAccounts.refundableAccDesc;
            this.FinanceLookupTableModal.show(this.target);
        }
        this.target = ac;
    }

    getNewChartOfAC(ac) {
        if (ac == "salesACC") {
            this.saleAccounts.salesACC = this.FinanceLookupTableModal.id;
            this.saleAccounts.salesACCDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (ac == "salesRetACC") {
            this.saleAccounts.salesRetACC = this.FinanceLookupTableModal.id;
            this.saleAccounts.salesRetACCDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (ac == "cogsacc") {
            this.saleAccounts.cogsacc = this.FinanceLookupTableModal.id;
            this.saleAccounts.cogsaccDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (ac == "chAccountID") {
            this.saleAccounts.chAccountID = this.FinanceLookupTableModal.id;
            this.saleAccounts.chAccountIDDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (ac == "discAcc") {
            this.saleAccounts.discAcc = this.FinanceLookupTableModal.id;
            this.saleAccounts.discAccDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (ac == "writeOffAcc") {
            this.saleAccounts.writeOffAcc = this.FinanceLookupTableModal.id;
            this.saleAccounts.writeOffAccDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (ac == "payableAcc") {
            this.saleAccounts.payableAcc = this.FinanceLookupTableModal.id;
            this.saleAccounts.payableAccDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (ac == "refundableAcc") {
            this.saleAccounts.refundableAcc = this.FinanceLookupTableModal.id;
            this.saleAccounts.refundableAccDesc = this.FinanceLookupTableModal.displayName;
        }
    }

    openLocationModal() {
        if(this.LocCheckVal==true){
            this.target = "Location";
            this.InventoryLookupTableModal.id = String(this.saleAccounts.locId);
            this.InventoryLookupTableModal.displayName = this.saleAccounts.locDesc;
            this.InventoryLookupTableModal.show(this.target);
        }
     

    }
    getNewLocation() {
        this.saleAccounts.locId = Number(this.InventoryLookupTableModal.id);
        this.saleAccounts.locDesc = this.InventoryLookupTableModal.displayName;
        this.checkTransTypeExists();
    }

    getNewTransType() {
        this.saleAccounts.typeId = this.InventoryLookupTableModal.id;
        this.saleAccounts.typeDesc = this.InventoryLookupTableModal.displayName;
        this.checkTransTypeExists();
    }

    openTransModal() {
        this.target = "TransactionType";
        this.InventoryLookupTableModal.id = String(this.saleAccounts.typeId);
        this.InventoryLookupTableModal.displayName = this.saleAccounts.typeDesc;
        this.InventoryLookupTableModal.show(this.target);
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }


    checkTransTypeExists() {
        if(this.editMode == false){
            if (this.saleAccounts.locId > 0 && this.saleAccounts.typeId != undefined)
            this._saleAccountsService.GetTransAgainstLoc(this.saleAccounts.locId, 
                this.saleAccounts.typeId)
                .subscribe((res: any) => {
                    this.formValid = res.result;
                    if (this.formValid == true)
                        this.notify.info(this.l('Trans Type Against This Location Already Exists'));
                });
        }
    }

    checkFormValid(){
        debugger
        console.log(this.saleAccounts);
        if(this.saleAccounts.locId > 0 && this.saleAccounts.typeId != ""
        && this.saleAccounts.typeId != undefined
        ){
            this.formValid = false;
        }
        else
        {
            this.formValid = true;
        }
       // this.checkTransTypeExists();
    }
}

