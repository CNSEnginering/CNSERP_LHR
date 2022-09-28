import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { GLOptionsServiceProxy, CreateOrEditGLOptionDto, APOptionsServiceProxy, ChartofControlsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';

@Component({
    selector: 'createOrEditGLOptionModal',
    templateUrl: './create-or-edit-glOption-modal.component.html'
})
export class CreateOrEditGLOptionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    editMode: boolean = false;
    glOption: CreateOrEditGLOptionDto = new CreateOrEditGLOptionDto();

    audtdate: Date;
    chartofControlId = '';
    chartofControlId1 = '';
    CompanyName = '';
    Address = '';
    City = '';
    Province = '';
    Country = '';
    acc1 = false;
    acc2 = false;
    target: string;


    constructor(
        injector: Injector,
        private _glOptionsServiceProxy: GLOptionsServiceProxy,
        private _apOptionsServiceProxy: APOptionsServiceProxy,
        private _chartofControlsServiceProxy: ChartofControlsServiceProxy
    ) {
        super(injector);
    }

    show(glOptionId?: number): void {
        this.audtdate = null;

        this._apOptionsServiceProxy.getCompanyProfileData().subscribe(result => {
            debugger;
            this.CompanyName = result.companyName;
            this.Address = result.address;
            this.City = result.city;
            this.Province = result.state;
            this.Country = result.country;
        })

        if (!glOptionId) {
            this.glOption = new CreateOrEditGLOptionDto();
            this.glOption.id = glOptionId;
            this.glOption.instrumentNo = false;
            this.chartofControlId = '';
            this.chartofControlId1 = '';
            this.glOption.docFrequency = 1;
            this.editMode = false;
            this.active = true;
            this.glOption.financePoint = 0;
            this.modal.show();
        } else {
            this.editMode = true;
            this._glOptionsServiceProxy.getGLOptionForEdit(glOptionId).subscribe(result => {
                this.glOption = result.glOption;

                if (result.glOption.defaultclacc) {
                    this._chartofControlsServiceProxy.getChartofControlForEdit(result.glOption.defaultclacc).subscribe(result => {
                        debugger;
                        this.chartofControlId = result.chartofControl.accountName;
                    });
                }
                if (result.glOption.stockctrlacc) {
                    this._chartofControlsServiceProxy.getChartofControlForEdit(result.glOption.stockctrlacc).subscribe(result => {
                        debugger;
                        this.chartofControlId1 = result.chartofControl.accountName;
                    });
                }

                if (this.glOption.audtdate) {
                    this.audtdate = this.glOption.audtdate.toDate();
                }
                this.chartofControlId = result.chartofControlId;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;


        this.glOption.audtdate = moment();
        this.glOption.audtuser = this.appSession.user.userName;
        this._glOptionsServiceProxy.createOrEdit(this.glOption)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }


    getNewFinanceModal() {
        debugger;
        this.getNewChartOfAC(this.target);
    }

    //=====================Chart of Ac Model================
    openSelectChartofACModal(ac) {
        debugger;
        this.target = "ChartOfAccount";
        if (ac == "Retained") {
            this.FinanceLookupTableModal.id = this.glOption.defaultclacc;
            this.FinanceLookupTableModal.displayName = this.chartofControlId;
            this.FinanceLookupTableModal.show(this.target);
        }
        if (ac == "Purchase") {
            this.FinanceLookupTableModal.id = this.glOption.stockctrlacc;
            this.FinanceLookupTableModal.displayName = this.chartofControlId1;
            this.FinanceLookupTableModal.show(this.target);
        }
        this.target = ac;
    }

    setAccountIDNull(ac) {
        if (ac == "Retained") {
            this.glOption.defaultclacc = '';
            this.chartofControlId = '';
        }
        if (ac == "Purchase") {
            this.glOption.stockctrlacc = '';
            this.chartofControlId1 = '';
        }
    }

    getNewChartOfAC(ac) {
        debugger;
        if (ac == "Retained") {
            this.glOption.defaultclacc = this.FinanceLookupTableModal.id;
            this.chartofControlId = this.FinanceLookupTableModal.displayName;
        }
        if (ac == "Purchase") {
            this.glOption.stockctrlacc = this.FinanceLookupTableModal.id;
            this.chartofControlId1 = this.FinanceLookupTableModal.displayName;
        }
    }
    //=====================Chart of Ac Model===============

    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
