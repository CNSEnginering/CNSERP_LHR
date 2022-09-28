import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FiscalCalendersServiceProxy, FiscalCalenderDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditFiscalCalenderModalComponent } from './create-or-edit-fiscalCalender-modal.component';
import { ViewFiscalCalenderModalComponent } from './view-fiscalCalender-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { isNull } from 'util';

@Component({
    templateUrl: './fiscalCalenders.component.html',
   // styleUrls: ['./fiscalCalender.cpmponent.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class FiscalCalendersComponent extends AppComponentBase {

    @ViewChild('createOrEditFiscalCalenderModal', { static: true }) createOrEditFiscalCalenderModal: CreateOrEditFiscalCalenderModalComponent;
    @ViewChild('viewFiscalCalenderModalComponent', { static: true }) viewFiscalCalenderModal: ViewFiscalCalenderModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false; 
    filterText = '';
    maxPeriodFilter : number;
		maxPeriodFilterEmpty : number;
		minPeriodFilter : number;
		minPeriodFilterEmpty : number;
    maxStartDateFilter : moment.Moment;
		minStartDateFilter : moment.Moment;
    maxEndDateFilter : moment.Moment;
		minEndDateFilter : moment.Moment;
    glFilter = -1;
    apFilter = -1;
    arFilter = -1;
    inFilter = -1;
    poFilter = -1;
    oeFilter = -1;
    bkFilter = -1;
    hrFilter = -1;
    prFilter = -1;
    maxCreatedByFilter : number;
	maxCreatedByFilterEmpty : number;
	minCreatedByFilter : number;
	minCreatedByFilterEmpty : number;
    maxCreatedDateFilter : moment.Moment;
	minCreatedDateFilter : moment.Moment;
    maxEditDateFilter : moment.Moment;
	minEditDateFilter : moment.Moment;
    maxEditUserFilter : number;
	maxEditUserFilterEmpty : number;
	minEditUserFilter : number;
    minEditUserFilterEmpty : number;

    calendarEdit: FiscalCalenderDto;
    actionStatus: boolean = false;
    activeStatus: boolean = false;




    constructor(
        injector: Injector,
        private _fiscalCalendersServiceProxy: FiscalCalendersServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);       
    }

    getFiscalCalenders(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._fiscalCalendersServiceProxy.getAll(
            this.filterText,
            this.maxPeriodFilter == null ? this.maxPeriodFilterEmpty: this.maxPeriodFilter,
            this.minPeriodFilter == null ? this.maxPeriodFilter: this.maxPeriodFilter,
            this.maxStartDateFilter,
            this.minStartDateFilter,
            this.maxEndDateFilter,
            this.minEndDateFilter,
            this.glFilter,
            this.apFilter,
            this.arFilter,
            this.inFilter,
            this.poFilter,
            this.oeFilter,
            this.bkFilter,
            this.hrFilter,
            this.prFilter,
            this.maxCreatedByFilter == null ? this.maxCreatedByFilterEmpty: this.maxCreatedByFilter,
            this.minCreatedByFilter == null ? this.minCreatedByFilterEmpty: this.minCreatedByFilter,
            this.maxCreatedDateFilter,
            this.minCreatedDateFilter,
            this.maxEditDateFilter,
            this.minEditDateFilter,
            this.maxEditUserFilter == null ? this.maxEditUserFilterEmpty: this.maxEditUserFilter,
            this.minEditUserFilter == null ? this.minEditUserFilterEmpty: this.minEditUserFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            debugger;
            
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;          
            this.primengTableHelper.hideLoadingIndicator();          
            this.calendarEdit = result.items[0].fiscalCalender;

            this._fiscalCalendersServiceProxy.getFiscalCalenderForEdit(this.calendarEdit .id).subscribe(result => {
                debugger;
                this.actionStatus  = result.fiscalCalender.isLocked;
                this.activeStatus = result.fiscalCalender.isActive;              
                
            });
            
           
        });

        
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createFiscalCalender(): void {
        this.createOrEditFiscalCalenderModal.show();
    }

   

    deleteFiscalCalender(fiscalCalender: FiscalCalenderDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._fiscalCalendersServiceProxy.delete(fiscalCalender.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._fiscalCalendersServiceProxy.getFiscalCalendersToExcel(
        this.filterText,
            this.maxPeriodFilter == null ? this.maxPeriodFilterEmpty: this.maxPeriodFilter,
            this.minPeriodFilter == null ? this.minPeriodFilterEmpty: this.minPeriodFilter,
            this.maxStartDateFilter,
            this.minStartDateFilter,
            this.maxEndDateFilter,
            this.minEndDateFilter,
            this.glFilter,
            this.apFilter,
            this.arFilter,
            this.inFilter,
            this.poFilter,
            this.oeFilter,
            this.bkFilter,
            this.hrFilter,
            this.prFilter,
            this.maxCreatedByFilter == null ? this.maxCreatedByFilterEmpty: this.maxCreatedByFilter,
            this.minCreatedByFilter == null ? this.minCreatedByFilterEmpty: this.minCreatedByFilter,
            this.maxCreatedDateFilter,
            this.minCreatedDateFilter,
            this.maxEditDateFilter,
            this.minEditDateFilter,
            this.maxEditUserFilter == null ? this.maxEditUserFilterEmpty: this.maxEditUserFilter,
            this.minEditUserFilter == null ? this.minEditUserFilterEmpty: this.minEditUserFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
