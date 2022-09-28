import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';

import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditAssetRegistrationDto, AssetRegistrationsServiceProxy } from '../shared/services/assetRegistration.service';
import { InventoryLookupTableModalComponent } from '@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { AssetRegistrationDetailServiceProxy, CreateOrEditAssetRegistrationDetailDto } from '../shared/services/assetRegistrationDetail.service';
import { SalesLookupTableModalComponent } from '@app/finders/supplyChain/sales/sales-lookup-table-modal.component';

@Component({
    selector: 'createOrEditAssetRegistrationModal',
    templateUrl: './create-or-edit-assetRegistration-modal.component.html'
})
export class CreateOrEditAssetRegistrationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('LocationFinder', { static: true }) locationFinder: InventoryLookupTableModalComponent;
    @ViewChild('accAssetFinder', { static: true }) accAssetFinder: FinanceLookupTableModalComponent;
    @ViewChild('accDeprFinder', { static: true }) accDeprFinder: FinanceLookupTableModalComponent;
    @ViewChild('accExpFinder', { static: true }) accExpFinder: FinanceLookupTableModalComponent;
    @ViewChild('ItemFinder', { static: true }) itemFinder: InventoryLookupTableModalComponent;
    @ViewChild('SalesLookupTableModal', { static: true }) SalesLookupTableModal: SalesLookupTableModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    flag: boolean;

    assetRegistration: CreateOrEditAssetRegistrationDto = new CreateOrEditAssetRegistrationDto();
    assetRegistrationDetail: CreateOrEditAssetRegistrationDetailDto = new CreateOrEditAssetRegistrationDetailDto();

    regDate: Date;
    purchaseDate: Date;
    expiryDate: Date;
    depStartDate: Date;
    lastDepDate: Date;
    audtDate: Date;
    createDate: Date;
    locationName: string;
    assetAccName: string;
    accDeprName: string;
    accExpName: string;
    itemName: string;
    depProcess: boolean;
    checkedval: boolean;
    LocCheckVal: boolean;
    target: string
    refName: string;
    constructor(
        injector: Injector,
        private _assetRegistrationServiceProxy: AssetRegistrationsServiceProxy,
        private _assetRegistrationDetailServiceProxy: AssetRegistrationDetailServiceProxy
    ) {
        super(injector);
    }

    show(assetRegistrationId?: number): void {
        this.regDate = moment().toDate();
        this.purchaseDate = null;
        this.expiryDate = null;
        this.depStartDate = null;
        this.lastDepDate = null;
        this.audtDate = null;
        this.createDate = null;
        if (!assetRegistrationId) {
            this.assetRegistration = new CreateOrEditAssetRegistrationDto();
            this.assetRegistration.id = assetRegistrationId;
            this.assetRegistration.depMethod = 1;
            this.getMaxID();
            this.depProcess = false;
            this.flag = false;
            this.active = true;
            this.modal.show();
        } else {
            this._assetRegistrationServiceProxy.getAssetRegistrationForEdit(assetRegistrationId).subscribe(result => {
                this.assetRegistration = result.assetRegistration;
                debugger;
                this.itemName = result.itemName;
                this.locationName = result.locationName;
                this.assetAccName = result.assetAccName;
                this.accExpName = result.accExpName;
                this.accDeprName = result.accDeprName;
                this.refName = result.refName;
                debugger
                if (this.assetRegistration.regDate) {
                    this.regDate = this.assetRegistration.regDate.toDate();
                }
                if (this.assetRegistration.purchaseDate) {
                    this.purchaseDate = this.assetRegistration.purchaseDate.toDate();
                }
                if (this.assetRegistration.expiryDate) {
                    this.expiryDate = this.assetRegistration.expiryDate.toDate();
                }
                if (this.assetRegistration.depStartDate) {
                    this.depStartDate = this.assetRegistration.depStartDate.toDate();
                }
                if (this.assetRegistration.lastDepDate) {
                    this.lastDepDate = this.assetRegistration.lastDepDate.toDate();
                }
                if (this.assetRegistration.audtDate) {
                    this.audtDate = this.assetRegistration.audtDate.toDate();
                }
                if (this.assetRegistration.createDate) {
                    this.createDate = this.assetRegistration.createDate.toDate();
                }
                this.flag = true;
                this.depProcess = true;
                this.active = true;
                this.modal.show();
            });
        }
    }

    getMaxID() {
        this._assetRegistrationServiceProxy.GetAssetRegMaxId().subscribe(res => {
            debugger;
            this.assetRegistration.fmtAssetID = res[1];
            this.assetRegistration.assetID = Number(res[0]);


        })
    }
    SetDefaultRecord(result: any) {
        console.log(result);
        this.assetRegistration.locID = result.currentLocID;
        this.locationName = result.currentLocName;
        this.checkedval = result.cDateOnly;
        if (result.allowLocID == false) {
            this.LocCheckVal = false;
        } else {
            this.LocCheckVal = true;
        }
        //this.typeDesc=result.transTypeName;
    }

    save(): void {
        this.saving = true;


        if (this.regDate) {
            if (!this.assetRegistration.regDate) {
                this.assetRegistration.regDate = moment(this.regDate).startOf('day');
            }
            else {
                this.assetRegistration.regDate = moment(this.regDate);
            }
        }
        else {
            this.assetRegistration.regDate = null;
        }
        if (this.purchaseDate) {
            if (!this.assetRegistration.purchaseDate) {
                this.assetRegistration.purchaseDate = moment(this.purchaseDate).startOf('day');
            }
            else {
                this.assetRegistration.purchaseDate = moment(this.purchaseDate);
            }
        }
        else {
            this.assetRegistration.purchaseDate = null;
        }
        if (this.expiryDate) {
            if (!this.assetRegistration.expiryDate) {
                this.assetRegistration.expiryDate = moment(this.expiryDate).startOf('day');
            }
            else {
                this.assetRegistration.expiryDate = moment(this.expiryDate);
            }
        }
        else {
            this.assetRegistration.expiryDate = null;
        }
        if (this.depStartDate) {
            if (!this.assetRegistration.depStartDate) {
                this.assetRegistration.depStartDate = moment(this.depStartDate).startOf('day');
            }
            else {
                this.assetRegistration.depStartDate = moment(this.depStartDate);
            }
        }
        else {
            this.assetRegistration.depStartDate = null;
        }
        if (this.lastDepDate) {
            if (!this.assetRegistration.lastDepDate) {
                this.assetRegistration.lastDepDate = moment(this.lastDepDate).startOf('day');
            }
            else {
                this.assetRegistration.lastDepDate = moment(this.lastDepDate);
            }
        }
        else {
            this.assetRegistration.lastDepDate = null;
        }
        if (this.audtDate) {
            if (!this.assetRegistration.audtDate) {
                this.assetRegistration.audtDate = moment(this.audtDate).startOf('day');
            }
            else {
                this.assetRegistration.audtDate = moment(this.audtDate);
            }
        }
        else {
            this.assetRegistration.audtDate = null;
        }
        if (this.createDate) {
            if (!this.assetRegistration.createDate) {
                this.assetRegistration.createDate = moment(this.createDate).startOf('day');
            }
            else {
                this.assetRegistration.createDate = moment(this.createDate);
            }
        }
        else {
            this.assetRegistration.createDate = null;
        }


        this._assetRegistrationServiceProxy.createOrEdit(this.assetRegistration)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    CalBookValue(params) {

        this.assetRegistration.bookValue = params;
    }


    openLocationFinder() {
        if (this.LocCheckVal == true) {
            this.locationFinder.id = null;
            this.locationFinder.displayName = this.locationName;
            this.locationFinder.show("Location");
        }

    }

    setLocationFinder() {
        this.assetRegistration.locID = null;
        this.locationName = '';
    }

    getNewLocationId() {
        this.assetRegistration.locID = Number(this.locationFinder.id);
        this.locationName = this.locationFinder.displayName;
    }

    openItemFinder() {
        this.itemFinder.id = null;
        this.itemFinder.displayName = this.locationName;
        this.itemFinder.show("Items");
    }

    setItemFinder() {
        this.assetRegistration.itemID = null;
        this.itemName = '';
    }

    getNewItemId() {
        this.assetRegistration.itemID = this.itemFinder.id;
        this.itemName = this.itemFinder.displayName;
    }

    openAccountsFinder(param) {
        debugger;
        if (param == "accAsset") {
            this.accAssetFinder.id = this.assetRegistration.accAsset;
            this.accAssetFinder.displayName = this.assetAccName;
            this.accAssetFinder.show("ChartOfAccount", "");
        } else if (param == "accDepr") {
            this.accDeprFinder.id = this.assetRegistration.accDepr;
            this.accDeprFinder.displayName = this.accDeprName;
            this.accDeprFinder.show("ChartOfAccount", "");
        }
        else if (param == "accExp") {
            this.accExpFinder.id = this.assetRegistration.accExp;
            this.accExpFinder.displayName = this.accExpName;
            this.accExpFinder.show("ChartOfAccount", "");
        }
    }

    setAccountsFinder(param) {
        if (param == "accAsset") {
            this.assetRegistration.accAsset = null;
            this.assetAccName = '';
        } else if (param == "accDepr") {
            this.assetRegistration.accDepr = null;
            this.accDeprName = '';
        } else if (param == "accExp") {
            this.assetRegistration.accExp = null;
            this.accExpName = '';
        }
    }

    getNewAccountsId(param) {
        debugger;
        if (param == "accAsset") {
            this.assetRegistration.accAsset = this.accAssetFinder.id;
            this.assetAccName = this.accAssetFinder.displayName;
        } else if (param == "accDepr") {
            this.assetRegistration.accDepr = this.accDeprFinder.id;
            this.accDeprName = this.accDeprFinder.displayName;
        } else if (param == "accExp") {
            this.assetRegistration.accExp = this.accExpFinder.id;
            this.accExpName = this.accExpFinder.displayName;
        }
    }


    getDepreciation() {
        this.assetRegistration.lastDepAmount = 0;
        if (this.assetRegistration.bookValue && this.assetRegistration.depRate && this.assetRegistration.bookValue > 0 && this.assetRegistration.depRate > 0) {
            this.assetRegistration.lastDepAmount = (this.assetRegistration.bookValue - this.assetRegistration.accumulatedDepAmt) * this.assetRegistration.depRate / 100;
            this.assetRegistration.accumulatedDepAmt += this.assetRegistration.lastDepAmount;
            this.assetRegistration.bookValue -= this.assetRegistration.accumulatedDepAmt;
            this.lastDepDate = moment().toDate();
            this.assetRegistration.lastDepDate = moment(this.lastDepDate).startOf('day');


            if (this.regDate) {
                if (!this.assetRegistration.regDate) {
                    this.assetRegistration.regDate = moment(this.regDate).startOf('day');
                }
                else {
                    this.assetRegistration.regDate = moment(this.regDate);
                }
            }
            else {
                this.assetRegistration.regDate = null;
            }
            if (this.purchaseDate) {
                if (!this.assetRegistration.purchaseDate) {
                    this.assetRegistration.purchaseDate = moment(this.purchaseDate).startOf('day');
                }
                else {
                    this.assetRegistration.purchaseDate = moment(this.purchaseDate);
                }
            }
            else {
                this.assetRegistration.purchaseDate = null;
            }
            if (this.expiryDate) {
                if (!this.assetRegistration.expiryDate) {
                    this.assetRegistration.expiryDate = moment(this.expiryDate).startOf('day');
                }
                else {
                    this.assetRegistration.expiryDate = moment(this.expiryDate);
                }
            }
            else {
                this.assetRegistration.expiryDate = null;
            }
            if (this.depStartDate) {
                if (!this.assetRegistration.depStartDate) {
                    this.assetRegistration.depStartDate = moment(this.depStartDate).startOf('day');
                }
                else {
                    this.assetRegistration.depStartDate = moment(this.depStartDate);
                }
            }
            else {
                this.assetRegistration.depStartDate = null;
            }
            if (this.lastDepDate) {
                if (!this.assetRegistration.lastDepDate) {
                    this.assetRegistration.lastDepDate = moment(this.lastDepDate).startOf('day');
                }
                else {
                    this.assetRegistration.lastDepDate = moment(this.lastDepDate);
                }
            }
            else {
                this.assetRegistration.lastDepDate = null;
            }
            if (this.audtDate) {
                if (!this.assetRegistration.audtDate) {
                    this.assetRegistration.audtDate = moment(this.audtDate).startOf('day');
                }
                else {
                    this.assetRegistration.audtDate = moment(this.audtDate);
                }
            }
            else {
                this.assetRegistration.audtDate = null;
            }
            if (this.createDate) {
                if (!this.assetRegistration.createDate) {
                    this.assetRegistration.createDate = moment(this.createDate).startOf('day');
                }
                else {
                    this.assetRegistration.createDate = moment(this.createDate);
                }
            }
            else {
                this.assetRegistration.createDate = null;
            }

            this.assetRegistrationDetail.assetID = this.assetRegistration.assetID;
            this.assetRegistrationDetail.assetLife = this.assetRegistration.assetLife;
            this.assetRegistrationDetail.bookValue = this.assetRegistration.bookValue;
            this.assetRegistrationDetail.depMethod = this.assetRegistration.depMethod;
            this.assetRegistrationDetail.depStartDate = this.assetRegistration.depStartDate;
            this.assetRegistrationDetail.lastDepAmount = this.assetRegistration.lastDepAmount;
            this.assetRegistrationDetail.lastDepDate = this.assetRegistration.lastDepDate;
            this.assetRegistrationDetail.accumulatedDepAmt = this.assetRegistration.accumulatedDepAmt;

            this.assetRegistration.createAssetRegistrationDetail = this.assetRegistrationDetail;


            this._assetRegistrationServiceProxy.createOrEdit(this.assetRegistration)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    // this.close();
                    // this.modalSave.emit(null);
                });
            debugger;

            this.depProcess = false;
        }
        else {
            abp.message.error("Book Value and Depreciation Rate Can't be Empty", "Null Values")
        }

    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }


    //=====================Sale Refrence Model================
    openSelectReferenceModal() {
        this.target = "Reference";
        this.SalesLookupTableModal.id = String(this.assetRegistration.refID);
        this.SalesLookupTableModal.displayName = this.refName;
        this.SalesLookupTableModal.show(this.target, 'FA');
    }
    getNewReference() {
        debugger;
        this.assetRegistration.refID = this.SalesLookupTableModal.id != undefined ? Number(this.SalesLookupTableModal.id) : undefined;
        this.refName = this.SalesLookupTableModal.displayName;
    }
    setReferenceIDNull() {
        this.assetRegistration.refID = null;
        this.refName = null;
    }
    //=====================Sale Refrence Model================
}
