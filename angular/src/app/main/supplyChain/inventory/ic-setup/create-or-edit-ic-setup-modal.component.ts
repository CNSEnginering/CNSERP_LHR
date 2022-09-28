import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ICSetupDto } from '../shared/dto/ic-setup-dto';
import { ICSetupsService } from '../shared/services/ic-setup.service';
import { GetDataService } from '../shared/services/get-data.service';
import { GetDataViewDto } from '../shared/dto/get-data-dto';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';


@Component({
    selector: 'createOrEditICSetupModal',
    templateUrl: './create-or-edit-ic-setup-modal.component.html'
})
export class CreateOrEditICSetupModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    icSetup: ICSetupDto = new ICSetupDto();

    createadOn: Date;
    books: any;
    SaleType:any;
    locations: any;
    CurrencyList: any;
    target: string;


    constructor(
        injector: Injector,
        private _icSetupsService: ICSetupsService,
        private _getDataService: GetDataService
    ) {
        super(injector);
    }

    show(icSetupId?: number): void {
        this.createadOn = null;
        this.getGLBooks("GLBooks");
        this.getCurrency("Currency");
        this.getLocations("ICLocationsForAdmin");
        this.getSaleType("SaleType");
        if (!icSetupId) {
            this.icSetup = new ICSetupDto();
            this.icSetup.id = icSetupId;
            this.icSetup.prBookID = "";
            this.icSetup.rtBookID = "";
            this.icSetup.slBookID = "";
            this.icSetup.srBookID = "";
            this.icSetup.cnsBookID = "";
            this.icSetup.trBookID = "";
            this.icSetup.asmBookID = "";
            this.icSetup.costingMethod = 0;
            this.icSetup.currentLocID = 0;
            this.icSetup.glSegLink = 1;
            this.icSetup.conType = 1;

            this.active = true;
            this.modal.show();
        } else {
            this._icSetupsService.getICSetupForEdit(icSetupId).subscribe(result => {
                this.icSetup = result;

                if (this.icSetup.createadOn) {
                    this.createadOn = this.icSetup.createadOn.toDate();
                }

                this.active = true;
                this.modal.show();
            });
        }
    }

    getLocations(target: string): void {
        debugger;
        this._getDataService.getList(target).subscribe(result => {
            this.locations = result;
        });
    }
    getCurrency(target: string): void {
        debugger;
        this._getDataService.getList(target).subscribe(result => {
            this.CurrencyList = result;
        });
    }
    getSaleType(target: string): void {
        debugger;
        this._getDataService.getList(target).subscribe(result => {
           
            this.SaleType = result;
        });
    }
    

    getGLBooks(target: string): void {
        debugger;
        this._getDataService.getList(target).subscribe(result => {
            this.books = result;
        });
    }

    save(): void {

        if (this.icSetup.segment1 == null || this.icSetup.segment1 == null || this.icSetup.segment3==null) {
            this.notify.error(this.l('Please Enter Segments'));
            return;
        }

        this.saving = true;


        if (this.createadOn) {
            if (!this.icSetup.createadOn) {
                this.icSetup.createadOn = moment(this.createadOn).startOf('day');
            }
            else {
                this.icSetup.createadOn = moment(this.createadOn);
            }
        }
        else {
            this.icSetup.createadOn = null;
        }

        if (!this.icSetup.id) {
            this.icSetup.createadOn = moment();
            this.icSetup.createdBy = this.appSession.user.userName;
        }
        this._icSetupsService.createOrEdit(this.icSetup)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }


    //=====================Books Model================
    openSelectBooksModal(book) {
        debugger;
        this.target = book;
        switch (book) {
            case "PR":
                this.FinanceLookupTableModal.id = this.icSetup.prBookID;
                break;
            case "RT":
                this.FinanceLookupTableModal.id = this.icSetup.rtBookID;
                break;
            case "SL":
                this.FinanceLookupTableModal.id = this.icSetup.slBookID;
                break;
            case "SR":
                this.FinanceLookupTableModal.id = this.icSetup.srBookID;
                break;
            case "CM":
                this.FinanceLookupTableModal.id = this.icSetup.cnsBookID;
                break;
            case "TR":
                this.FinanceLookupTableModal.id = this.icSetup.trBookID;
                break;
            case "AS":
                this.FinanceLookupTableModal.id = this.icSetup.asmBookID;
                break;
        }
        this.FinanceLookupTableModal.show("GLBooks","true");
    }

    setBookIDNull(book) {
        switch (book) {
            case "PR":
                this.icSetup.prBookID = '';
                break;
            case "RT":
                this.icSetup.rtBookID = '';
                break;
            case "SL":
                this.icSetup.slBookID = '';
                break;
            case "SR":
                this.icSetup.srBookID = '';
                break;
            case "CM":
                this.icSetup.cnsBookID = '';
                break;
            case "TR":
                this.icSetup.trBookID = '';
                break;
            case "AS":
                this.icSetup.asmBookID = '';
                break;
        }
    }

    getNewFinanceModal() {
        debugger;
        switch (this.target) {
            case "PR":
                this.icSetup.prBookID = this.FinanceLookupTableModal.id;
                break;
            case "RT":
                this.icSetup.rtBookID = this.FinanceLookupTableModal.id;
                break;
            case "SL":
                this.icSetup.slBookID = this.FinanceLookupTableModal.id;
                break;
            case "SR":
                this.icSetup.srBookID = this.FinanceLookupTableModal.id;
                break;
            case "CM":
                this.icSetup.cnsBookID = this.FinanceLookupTableModal.id;
                break;
            case "TR":
                this.icSetup.trBookID = this.FinanceLookupTableModal.id;
                break;
            case "AS":
                this.icSetup.asmBookID = this.FinanceLookupTableModal.id;
                break;
        }
    }

    //=====================Books Model================




    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
