import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AROptionsServiceProxy, AROptionDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditAROptionModalComponent } from './create-or-edit-arOption-modal.component';
import { ViewAROptionModalComponent } from './view-arOption-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './arOptions.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class AROptionsComponent extends AppComponentBase {

    @ViewChild('createOrEditAROptionModal', { static: true }) createOrEditAROptionModal: CreateOrEditAROptionModalComponent;
    @ViewChild('viewAROptionModalComponent', { static: true }) viewAROptionModal: ViewAROptionModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    defbankidFilter = '';
    maxDEFPAYCODEFilter : number;
		maxDEFPAYCODEFilterEmpty : number;
		minDEFPAYCODEFilter : number;
		minDEFPAYCODEFilterEmpty : number;
    defcusctrlaccFilter = '';
    defcurrcodeFilter = '';
    paytermsFilter = '';
    maxAUDTDATEFilter : moment.Moment;
		minAUDTDATEFilter : moment.Moment;
    audtuserFilter = '';
        bankBANKIDFilter = '';
        currencyRateIdFilter = '';
        chartofControlIdFilter = ''; 


        disableCreateButton = false; 




    constructor(
        injector: Injector,
        private _arOptionsServiceProxy: AROptionsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    } 

    getAROptions(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._arOptionsServiceProxy.getAll(
            this.filterText,
            this.defbankidFilter,
            this.maxDEFPAYCODEFilter == null ? this.maxDEFPAYCODEFilterEmpty: this.maxDEFPAYCODEFilter,
            this.minDEFPAYCODEFilter == null ? this.minDEFPAYCODEFilterEmpty: this.minDEFPAYCODEFilter,
            this.defcusctrlaccFilter,
            this.defcurrcodeFilter,
            this.paytermsFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.bankBANKIDFilter,
            this.currencyRateIdFilter,
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

    createAROption(): void {
        this.createOrEditAROptionModal.show();
    }

    deleteAROption(arOption: AROptionDto): void { 
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._arOptionsServiceProxy.delete(arOption.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._arOptionsServiceProxy.getAROptionsToExcel(
        this.filterText,
            this.defbankidFilter,
            this.maxDEFPAYCODEFilter == null ? this.maxDEFPAYCODEFilterEmpty: this.maxDEFPAYCODEFilter,
            this.minDEFPAYCODEFilter == null ? this.minDEFPAYCODEFilterEmpty: this.minDEFPAYCODEFilter,
            this.defcusctrlaccFilter,
            this.defcurrcodeFilter,
            this.paytermsFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.bankBANKIDFilter,
            this.currencyRateIdFilter,
            this.chartofControlIdFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
