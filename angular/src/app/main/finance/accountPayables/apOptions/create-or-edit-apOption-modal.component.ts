import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { APOptionsServiceProxy,ChartofControlsServiceProxy, CreateOrEditAPOptionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CommonServiceLookupTableModalComponent } from '@app/finders/commonService/commonService-lookup-table-modal.component';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';


@Component({
    selector: 'createOrEditAPOptionModal',
    templateUrl: './create-or-edit-apOption-modal.component.html'
})
export class CreateOrEditAPOptionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('commonServiceLookupTableModal', { static: true }) commonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false; 
    saving = false;

    apOption: CreateOrEditAPOptionDto = new CreateOrEditAPOptionDto();

            audtdate: Date;
    currencyRateId = '';
    bankBANKID = '';
    chartofControlId = '';

    ContactPerson='';
    PhoneNo='';
    target:string;


    constructor(
        injector: Injector,
        private _apOptionsServiceProxy: APOptionsServiceProxy,
        private _chartofControlsServiceProxy: ChartofControlsServiceProxy
    ) {
        super(injector);
    }

    show(apOptionId?: number): void {
this.audtdate = null;
    this._apOptionsServiceProxy.getCompanyProfileData().subscribe(result=>{
        debugger;
        this.ContactPerson=result.contperson;
        this.PhoneNo=result.contphone;
    }) 
        if (!apOptionId) {
            this.apOption = new CreateOrEditAPOptionDto();
            this.apOption.id = apOptionId;
            this.currencyRateId = '';
            this.bankBANKID = '';
            this.chartofControlId = '';

            this.active = true;
            this.modal.show();
        } else {
            this._apOptionsServiceProxy.getAPOptionForEdit(apOptionId).subscribe(result => {
                debugger;
                this.apOption = result.apOption;

                if (this.apOption.audtdate) {
					this.audtdate = this.apOption.audtdate.toDate();
                }
                this.currencyRateId = result.currencyRateId;
                this.bankBANKID =result.bankBANKID;
                this.chartofControlId=result.chartofControlId;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

            this.apOption.audtdate =moment();
            this.apOption.audtuser = this.appSession.user.userName;
            this._apOptionsServiceProxy.createOrEdit(this.apOption)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    } 

    getNewCommonServiceModal() {
        switch (this.target) {
            case "Bank":
                this.getNewBankId();
                break;
            case "Currency":
                this.getNewCurrencyRateId();
                break;

            default:
                break;
        }
    }
    getNewFinanceModal()
    {
        debugger;
        switch (this.target) {
            case "ChartOfAccount":
                this.getNewChartofControlId();
                break;  
            default:
                break;
        }
    }

    openSelectCurrencyRateModal() {
        this.target="Currency";
        this.commonServiceLookupTableModal.id = this.apOption.defcurrcode;
        this.commonServiceLookupTableModal.displayName = this.currencyRateId;
        this.commonServiceLookupTableModal.show(this.target);
    }
    openSelectBankModal() {
        debugger;
        this.target="Bank";
        this.commonServiceLookupTableModal.id = this.apOption.defbankid;
        this.commonServiceLookupTableModal.displayName = this.bankBANKID;
        this.commonServiceLookupTableModal.show(this.target);
    }
    openSelectChartofControlModal() {
        this.target="ChartOfAccount"
        this.FinanceLookupTableModal.id = this.apOption.defvenctrlacc;
        this.FinanceLookupTableModal.displayName = this.chartofControlId;
        this.FinanceLookupTableModal.show(this.target);
    }


    setCurrencyRateIdNull() {
        this.apOption.defcurrcode = '';
        this.currencyRateId = '';
    }
    setBankIdNull() {
        this.apOption.defbankid = '';
        this.bankBANKID = '';
    }
    setChartofControlIdNull() {
        this.apOption.defvenctrlacc = '';
        this.chartofControlId = '';
    }


    getNewCurrencyRateId() {
        this.apOption.defcurrcode = this.commonServiceLookupTableModal.id;
        this.currencyRateId = this.commonServiceLookupTableModal.displayName;
    }
    getNewBankId() {
        debugger;
        this.apOption.defbankid = this.commonServiceLookupTableModal.id;
        this.bankBANKID = this.commonServiceLookupTableModal.displayName;
    }
    getNewChartofControlId() {
        this.apOption.defvenctrlacc = this.FinanceLookupTableModal.id;
        this.chartofControlId = this.FinanceLookupTableModal.displayName;
    }


    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
