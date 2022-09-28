import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AROptionsServiceProxy, ChartofControlsServiceProxy, CreateOrEditAROptionDto, APOptionsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CommonServiceLookupTableModalComponent } from '@app/finders/commonService/commonService-lookup-table-modal.component';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';


@Component({
    selector: 'createOrEditAROptionModal',
    templateUrl: './create-or-edit-arOption-modal.component.html'
})
export class CreateOrEditAROptionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('CommonServiceLookupTableModal', { static: true }) CommonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;



    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    arOption: CreateOrEditAROptionDto = new CreateOrEditAROptionDto();

    audtdate: Date;
    bankBANKID = '';
    currencyRateId = '';
    chartofControlId = '';

    ContactPerson = '';
    PhoneNo = '';
    target: string;


    constructor(
        injector: Injector,
        private _arOptionsServiceProxy: AROptionsServiceProxy,
        private _apOptionsServiceProxy: APOptionsServiceProxy,
        private _chartofControlsServiceProxy: ChartofControlsServiceProxy
    ) {
        super(injector);
    }

    show(arOptionId?: number): void {
        this.audtdate = null;
        this._apOptionsServiceProxy.getCompanyProfileData().subscribe(result => {
            debugger;
            this.ContactPerson = result.contperson;
            this.PhoneNo = result.contphone;
        })
        if (!arOptionId) {

            this.arOption = new CreateOrEditAROptionDto();
            this.arOption.id = arOptionId;
            this.bankBANKID = '';
            this.currencyRateId = '';
            this.chartofControlId = '';

            this.active = true;
            this.modal.show();
        } else {
            this._arOptionsServiceProxy.getAROptionForEdit(arOptionId).subscribe(result => {
                debugger;
                this.arOption = result.arOption;

                if (this.arOption.audtdate) {
                    this.audtdate = this.arOption.audtdate.toDate();
                }

                this.currencyRateId = result.currencyRateId;
                this.bankBANKID = result.bankBANKID;
                this.chartofControlId = result.chartofControlId;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this.arOption.audtdate = moment();
        this.arOption.audtuser = this.appSession.user.userName;
        this._arOptionsServiceProxy.createOrEdit(this.arOption)
            .pipe(finalize(() => { this.saving = false; }))
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
            // case "SubLedger":
            //     this.getNewSubAcc(); 
            //     break;
            // case "WHTerm":
            //     this.getNewWHTerm();
            //     break;     
            default:
                break;
        }
    }

    openSelectBankModal() {
        this.target="Bank";
        this.CommonServiceLookupTableModal.id = this.arOption.defbankid;
        this.CommonServiceLookupTableModal.displayName = this.bankBANKID;
        this.CommonServiceLookupTableModal.show(this.target);
    }
    openSelectCurrencyRateModal() {
        this.target="Currency"
        this.CommonServiceLookupTableModal.id = this.arOption.defcurrcode;
        this.CommonServiceLookupTableModal.displayName = this.currencyRateId;
        this.CommonServiceLookupTableModal.show(this.target);
    }
    openSelectChartofControlModal() {
        this.target="ChartOfAccount"
        this.FinanceLookupTableModal.id = this.arOption.defcusctrlacc;
        this.FinanceLookupTableModal.displayName = this.chartofControlId;
        this.FinanceLookupTableModal.show(this.target);
    }


    setBankIdNull() {
        this.arOption.defbankid = '';
        this.bankBANKID = '';
    }
    setCurrencyRateIdNull() {
        this.arOption.defcurrcode = '';
        this.currencyRateId = '';
    }
    setChartofControlIdNull() {
        this.arOption.defcusctrlacc = '';
        this.chartofControlId = '';
    }


    getNewBankId() {
        this.arOption.defbankid = this.CommonServiceLookupTableModal.id;
        this.bankBANKID = this.CommonServiceLookupTableModal.displayName;
    }
    getNewCurrencyRateId() {
        this.arOption.defcurrcode = this.CommonServiceLookupTableModal.id;
        this.currencyRateId = this.CommonServiceLookupTableModal.displayName;
    }
    getNewChartofControlId() {
        this.arOption.defcusctrlacc = this.FinanceLookupTableModal.id;
        this.chartofControlId = this.FinanceLookupTableModal.displayName;
    }


    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
