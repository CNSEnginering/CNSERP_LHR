import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { GLCONFIGServiceProxy, CreateOrEditGLCONFIGDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { GLCONFIGGLBOOKSLookupTableModalComponent } from './glconfig-glbooks-lookup-table-modal.component';
import { GLCONFIGChartofControlLookupTableModalComponent } from './glconfig-chartofControl-lookup-table-modal.component';
import { GLCONFIGAccountSubLedgerLookupTableModalComponent } from './glconfig-accountSubLedger-lookup-table-modal.component';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';


@Component({
    selector: 'createOrEditGLCONFIGModal',
    templateUrl: './create-or-edit-glconfig-modal.component.html'
})
export class CreateOrEditGLCONFIGModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('glconfigGLBOOKSLookupTableModal', { static: true }) glconfigGLBOOKSLookupTableModal: GLCONFIGGLBOOKSLookupTableModalComponent;
    @ViewChild('glconfigChartofControlLookupTableModal', { static: true }) glconfigChartofControlLookupTableModal: GLCONFIGChartofControlLookupTableModalComponent;
    @ViewChild('glconfigAccountSubLedgerLookupTableModal', { static: true }) glconfigAccountSubLedgerLookupTableModal: GLCONFIGAccountSubLedgerLookupTableModalComponent;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    glconfig: CreateOrEditGLCONFIGDto = new CreateOrEditGLCONFIGDto();

            audtdate: Date;
    glbooksBookName = '';
    chartofControlAccountName = '';
    accountSubLedgerSubAccName = '';
    target: any;


    constructor(
        injector: Injector,
        private _glconfigServiceProxy: GLCONFIGServiceProxy
    ) {
        super(injector);
    }

    show(glconfigId?: string): void {
this.audtdate = null;

        if (!glconfigId) {
            this.glconfig = new CreateOrEditGLCONFIGDto();
            this.glconfig.id = glconfigId;
            this.glbooksBookName = '';
            this.chartofControlAccountName = '';
            this.accountSubLedgerSubAccName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._glconfigServiceProxy.getGLCONFIGForEdit(glconfigId).subscribe(result => {
                this.glconfig = result.glconfig;

                if (this.glconfig.audtdate) {
					this.audtdate = this.glconfig.audtdate.toDate();
                }
                this.glbooksBookName = result.glbooksBookName;
                this.chartofControlAccountName = result.chartofControlAccountName;
                this.accountSubLedgerSubAccName = result.accountSubLedgerSubAccName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
        if (this.audtdate) {
            if (!this.glconfig.audtdate) {
                this.glconfig.audtdate = moment(this.audtdate).startOf('day');
            }
            else {
                this.glconfig.audtdate = moment(this.audtdate);
            }
        }
        else {
            this.glconfig.audtdate = null;
        }
            this._glconfigServiceProxy.createOrEdit(this.glconfig)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {

                
                    this.notify.info(this.l("SavedSuccessfully"));
                    this.message.confirm("Press 'Yes' for New GL Config",this.l('SavedSuccessfully'),( (isConfirmed) =>  {
                        if (isConfirmed) {
                            //let seg1ID = this.glconfig.id;
                            //this.glconfig = new CreateOrEditGLCONFIGDto();
    
                            this.chartofControlAccountName = '';
                            this.accountSubLedgerSubAccName = '';
                            this.glconfig.accountID = '';
                            this.glconfig.subAccID = undefined;
                            this._glconfigServiceProxy.maxidConfig(this.glconfig.glbooksId).subscribe(result => {

                                this.glconfig.configID = result;
                                //this.glconfig.subAccID = 0;
                            });
                            // this.flag = false;
                            this.modalSave.emit(null);
                        }
                        else
                        {
                            this.notify.info(this.l('SavedSuccessfully'));
                            this.close();
                            this.modalSave.emit(null);
                        }
                    }));
                }
                // this.notify.info(this.l('SavedSuccessfully'));
                // this.close();
                // this.modalSave.emit(null);



             );
    }

    getNewFinanceModal()
    {
        debugger;
        switch (this.target) {
            case "GLBooks":
                this.getNewGLBOOKSId();
                break;
            case "ChartOfAccount":
                this.getNewChartofControlId();
                break; 
            case "SubLedger":
                this.getNewAccountSubLedgerId();
                break;

            default:
                break;
        }
    }

        openSelectGLBOOKSModal() {
            this.target="GLBooks";
            this.FinanceLookupTableModal.id = this.glconfig.glbooksId;
            this.FinanceLookupTableModal.displayName = this.glbooksBookName;
            this.FinanceLookupTableModal.show(this.target);
    }
        openSelectChartofControlModal() {
            this.target="ChartOfAccount";
            this.FinanceLookupTableModal.id = this.glconfig.chartofControlId;
            this.FinanceLookupTableModal.displayName = "";
            this.glconfig.accountSubLedgerId = 0;
            this.glconfig.subAccID = 0;
            this.accountSubLedgerSubAccName = null;
            this.FinanceLookupTableModal.show(this.target);
    }
        openSelectAccountSubLedgerModal() {
            this.target="SubLedger";
            this.glconfig.accountSubLedgerId = 0;
            this.accountSubLedgerSubAccName = null;
            this.FinanceLookupTableModal.id = String(this.glconfig.accountSubLedgerId);
            this.FinanceLookupTableModal.displayName = this.accountSubLedgerSubAccName;
            this.FinanceLookupTableModal.show(this.target, this.FinanceLookupTableModal.id);
    }


        setGLBOOKSIdNull() {
        this.glconfig.glbooksId = null;
        this.glbooksBookName = '';
    }
        setChartofControlIdNull() {
        this.glconfig.chartofControlId = null;
        this.chartofControlAccountName = '';
    }
        setAccountSubLedgerIdNull() {
        this.glconfig.accountSubLedgerId = null;
        this.accountSubLedgerSubAccName = '';
    }


        getNewGLBOOKSId() {
           
        this.glconfig.glbooksId = this.FinanceLookupTableModal.id;
        this.glbooksBookName = this.FinanceLookupTableModal.displayName;
        this.glconfig.bookID = this.FinanceLookupTableModal.id;
        this._glconfigServiceProxy.maxidConfig(this.glconfig.glbooksId).subscribe(result => {

            this.glconfig.configID = result;
            //this.glconfig.subAccID = 0;
        });
    }
        getNewChartofControlId() {
           
        this.chartofControlAccountName = this.FinanceLookupTableModal.displayName;

        this.glconfig.accountID = this.FinanceLookupTableModal.id;

    }
        getNewAccountSubLedgerId() {
        
        this.glconfig.accountSubLedgerId = Number(this.FinanceLookupTableModal.id);
        this.accountSubLedgerSubAccName = this.FinanceLookupTableModal.displayName;
        this.glconfig.subAccID = Number(this.FinanceLookupTableModal.id);
    }


    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
