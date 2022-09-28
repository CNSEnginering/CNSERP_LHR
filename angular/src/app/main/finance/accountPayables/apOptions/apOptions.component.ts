import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { APOptionsServiceProxy, APOptionDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditAPOptionModalComponent } from './create-or-edit-apOption-modal.component';
import { ViewAPOptionModalComponent } from './view-apOption-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './apOptions.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class APOptionsComponent extends AppComponentBase {

    @ViewChild('createOrEditAPOptionModal', { static: true }) createOrEditAPOptionModal: CreateOrEditAPOptionModalComponent;
    @ViewChild('viewAPOptionModalComponent', { static: true }) viewAPOptionModal: ViewAPOptionModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    defbankidFilter = '';
    maxDEFPAYCODEFilter : number;
		maxDEFPAYCODEFilterEmpty : number;
		minDEFPAYCODEFilter : number;
		minDEFPAYCODEFilterEmpty : number;
    defvenctrlaccFilter = '';
    defcurrcodeFilter = '';
    paytermsFilter = '';
    maxAUDTDATEFilter : moment.Moment;
		minAUDTDATEFilter : moment.Moment;
    audtuserFilter = '';
        currencyRateIdFilter = '';
        bankBANKIDFilter = '';
        chartofControlIdFilter = '';

    disableCreateButton = false;



    constructor(
        injector: Injector,
        private _apOptionsServiceProxy: APOptionsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getAPOptions(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._apOptionsServiceProxy.getAll(
            this.filterText,
            this.defbankidFilter,
            this.maxDEFPAYCODEFilter == null ? this.maxDEFPAYCODEFilterEmpty: this.maxDEFPAYCODEFilter,
            this.minDEFPAYCODEFilter == null ? this.minDEFPAYCODEFilterEmpty: this.minDEFPAYCODEFilter,
            this.defvenctrlaccFilter,
            this.defcurrcodeFilter,
            this.paytermsFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.currencyRateIdFilter,
            this.bankBANKIDFilter,
            this.chartofControlIdFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            if (result.totalCount > 0) {
                this.disableCreateButton = true;
            }
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createAPOption(): void {
        this.createOrEditAPOptionModal.show();
    }

    deleteAPOption(apOption: APOptionDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._apOptionsServiceProxy.delete(apOption.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._apOptionsServiceProxy.getAPOptionsToExcel(
        this.filterText,
            this.defbankidFilter,
            this.maxDEFPAYCODEFilter == null ? this.maxDEFPAYCODEFilterEmpty: this.maxDEFPAYCODEFilter,
            this.minDEFPAYCODEFilter == null ? this.minDEFPAYCODEFilterEmpty: this.minDEFPAYCODEFilter,
            this.defvenctrlaccFilter,
            this.defcurrcodeFilter,
            this.paytermsFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.currencyRateIdFilter,
            this.bankBANKIDFilter,
            this.chartofControlIdFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
