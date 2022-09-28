import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { BanksServiceProxy, CreateOrEditBankDto,ChartofControlsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';


@Component({
    selector: 'createOrEditBankModal',
    templateUrl: './create-or-edit-bank-modal.component.html'
})
export class CreateOrEditBankModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    bank: CreateOrEditBankDto = new CreateOrEditBankDto();
    //chartofControl: CreateOrEditChartofControlDto = new CreateOrEditChartofControlDto();

    inactdate: Date;
    audtdate: Date;
    chartofControlName = '';

    IDACCTWOFF = '';
    NameActtWoff = '';
    NameAcctcrcard = '';


    constructor(
        injector: Injector,
        private _banksServiceProxy: BanksServiceProxy,
        private _chartofControlsServiceProxy: ChartofControlsServiceProxy
    ) {
        super(injector);
    }

    show(bankId?: number): void {
this.inactdate = null;
this.audtdate = null;

        if (!bankId) {
            this.bank = new CreateOrEditBankDto();
            this.bank.id = bankId;
            this.chartofControlName = '';
            this.NameActtWoff = '';
            this.NameAcctcrcard = '';
            this.bank.docType=1;

            this.active = true;
            this.modal.show();
        } else {
            this._banksServiceProxy.getBankForEdit(bankId).subscribe(result => {
                
                this.bank = result.bank; 
                if(result.bank.idacctbank){
                   this._chartofControlsServiceProxy.getChartofControlForEdit(result.bank.idacctbank).subscribe(result => {
                
                        this.chartofControlName=result.chartofControl.accountName;
                   });
                }
                 if(result.bank.idacctwoff){
                    this._chartofControlsServiceProxy.getChartofControlForEdit(result.bank.idacctwoff).subscribe(result => {
                        
                        this.NameActtWoff=result.chartofControl.accountName;
                   });
                 }
                 if(result.bank.idacctcrcard){
                    this._chartofControlsServiceProxy.getChartofControlForEdit(result.bank.idacctcrcard).subscribe(result => {
                        
                        this.NameAcctcrcard=result.chartofControl.accountName;
                   });
                 }
                
                if (this.bank.inactdate) {
					this.inactdate = this.bank.inactdate.toDate();
                }
                if (this.bank.audtdate) {
					this.audtdate = this.bank.audtdate.toDate();
                }
                this.chartofControlName = result.chartofControlId;
                this.active = true;
                this.modal.show();
            });
        }
    }

    // filterAccountNoDesc(accountId:string){
    //     debugger;
    //     var desc='';
    //     this._chartofControlsServiceProxy.getChartofControlForEdit(accountId).subscribe(result => {
    //          debugger;
    //           desc=result.chartofControl.accountName;
    //     });
    //      return desc;
    // }

    save(): void {
            this.saving = true;

			
            this.bank.audtdate =moment();
            this.bank.audtuser = this.appSession.user.userName;
            this._banksServiceProxy.createOrEdit(this.bank)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(result => {
                 debugger;
                 if(result=="Present"){
                    this.notify.warn(this.l('AlreadyPresent'));
                 }else{
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modalSave.emit(null);
                 }
             });
    }
acc1= false;
acc2 = false;
acc3 = false;

        openSelectChartofControlModal(acc:boolean) {
            this.acc1 = acc;

        this.FinanceLookupTableModal.id = this.bank.idacctbank;
        this.FinanceLookupTableModal.displayName = this.chartofControlName;
        this.FinanceLookupTableModal.show("ChartOfAccount");
    }

    openSelectChartofControlModal1(acc:boolean) {
        this.acc2 = acc;
        this.FinanceLookupTableModal.id = this.bank.idacctwoff;
        this.FinanceLookupTableModal.displayName = this.NameActtWoff;
        this.FinanceLookupTableModal.show("ChartOfAccount");
    }

    openSelectChartofControlModal2(acc:boolean) {
        this.acc3 = acc;
        this.FinanceLookupTableModal.id = this.bank.idacctcrcard;
        this.FinanceLookupTableModal.displayName = this.NameAcctcrcard;
        this.FinanceLookupTableModal.show("ChartOfAccount");
    }


        setAccountIDNull() {
        this.bank.idacctbank = null;
        this.chartofControlName = '';
    }


    getNewIDACCTBANK() {

        debugger;
        if (this.acc1) {
           
            this.bank.idacctbank = this.FinanceLookupTableModal.id;
            this.chartofControlName = this.FinanceLookupTableModal.displayName;
        }
        
        if (this.acc2) {
            this.bank.idacctwoff = this.FinanceLookupTableModal.id;
            this.NameActtWoff = this.FinanceLookupTableModal.displayName;    
        }

        if (this.acc3) {
            debugger;
            this.bank.idacctcrcard = this.FinanceLookupTableModal.id;
            this.NameAcctcrcard = this.FinanceLookupTableModal.displayName;    
        }


        this.acc1 = false;
        this.acc2 = false;
        this.acc3 = false;

       
    }

   

    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
