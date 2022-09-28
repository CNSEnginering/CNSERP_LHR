import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CurrencyRatesServiceProxy, CurrencyRateDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditCurrencyRateModalComponent } from './create-or-edit-currencyRate-modal.component';
import { ViewCurrencyRateModalComponent } from './view-currencyRate-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './currencyRates.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class CurrencyRatesComponent extends AppComponentBase {

    @ViewChild('createOrEditCurrencyRateModal', { static: true }) createOrEditCurrencyRateModal: CreateOrEditCurrencyRateModalComponent;
    @ViewChild('viewCurrencyRateModalComponent', { static: true }) viewCurrencyRateModal: ViewCurrencyRateModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    cmpidFilter = '';
    curidFilter = '';
    maxAUDTDATEFilter : moment.Moment;
		minAUDTDATEFilter : moment.Moment;
    audtuserFilter = '';
    curnameFilter = '';
    symbolFilter = '';
    maxRATEDATEFilter : moment.Moment;
		minRATEDATEFilter : moment.Moment;
    maxCURRATEFilter : number;
		maxCURRATEFilterEmpty : number;
		minCURRATEFilter : number;
		minCURRATEFilterEmpty : number;
        companyProfileCompanyNameFilter = '';




    constructor(
        injector: Injector,
        private _currencyRatesServiceProxy: CurrencyRatesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getCurrencyRates(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._currencyRatesServiceProxy.getAll(
            this.filterText,
            this.cmpidFilter,
            this.curidFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.curnameFilter,
            this.symbolFilter,
            this.maxRATEDATEFilter,
            this.minRATEDATEFilter,
            this.maxCURRATEFilter == null ? this.maxCURRATEFilterEmpty: this.maxCURRATEFilter,
            this.minCURRATEFilter == null ? this.minCURRATEFilterEmpty: this.minCURRATEFilter,
            this.companyProfileCompanyNameFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createCurrencyRate(): void {
        this.createOrEditCurrencyRateModal.show();
    }

    deleteCurrencyRate(currencyRate: CurrencyRateDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._currencyRatesServiceProxy.delete(currencyRate.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._currencyRatesServiceProxy.getCurrencyRatesToExcel(
        this.filterText,
            this.cmpidFilter,
            this.curidFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.curnameFilter,
            this.symbolFilter,
            this.maxRATEDATEFilter,
            this.minRATEDATEFilter,
            this.maxCURRATEFilter == null ? this.maxCURRATEFilterEmpty: this.maxCURRATEFilter,
            this.minCURRATEFilter == null ? this.minCURRATEFilterEmpty: this.minCURRATEFilter,
            this.companyProfileCompanyNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
