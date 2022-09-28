import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BatchListPreviewsServiceProxy, BatchListPreviewDto, VoucherEntryServiceProxy } from "@shared/service-proxies/service-proxies";
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';

import { ViewBatchListPreviewModalComponent } from './view-batchListPreview-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './batchListPreviews.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class BatchListPreviewsComponent extends AppComponentBase {

    @ViewChild('viewBatchListPreviewModalComponent', { static: true }) viewBatchListPreviewModal: ViewBatchListPreviewModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDocDateFilter: moment.Moment;
    minDocDateFilter: moment.Moment;
    maxDocMonthFilter = 0;
    maxDocMonthFilterEmpty: number;
    amountFilterEmpty: number;
    minDocMonthFilter = 0;
    minDocMonthFilterEmpty: number;
    bookIDFilter = '';
    locationFilter = '';
    referenceFilter = '';
    narrationFilter = '';
    booksList: any = '';

    minVoucherNo = 0;
    maxVoucherNo = 99999;
    maxAmountFilter = 999999;
    minAmountFilter = 0;

    approvedFilter = -1;

    postedFilter = -1;

    statusFilter = "All"



    constructor(
        injector: Injector,
        private _batchListPreviewsServiceProxy: BatchListPreviewsServiceProxy,
        private _voucherEntryServiceProxy: VoucherEntryServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getBatchListPreviews(event?: LazyLoadEvent) {
        debugger;
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._voucherEntryServiceProxy.getBooksDetails(false,"BatchListPreviews").subscribe(result => {
            debugger;
            this.booksList = result;
        });

        this._batchListPreviewsServiceProxy.getAll(
            this.filterText,
            this.locationFilter,
            this.referenceFilter,
            this.narrationFilter,
            this.maxAmountFilter,
            this.minAmountFilter,
            this.bookIDFilter,
            this.maxDocMonthFilter == 0 ? this.maxDocMonthFilterEmpty : this.maxDocMonthFilter,
            this.minDocMonthFilter == 0 ? this.minDocMonthFilterEmpty : this.minDocMonthFilter,
            this.maxDocDateFilter,
            this.minDocDateFilter,
            this.minVoucherNo,
            this.maxVoucherNo,
            this.approvedFilter,
            this.postedFilter,
            this.statusFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            debugger;
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    clearFilters(): void {
        debugger;
        this.locationFilter = '';
        this.minVoucherNo = 0;
        this.maxVoucherNo = 99999;
        this.minDocDateFilter = null;
        this.maxDocDateFilter = null;
        this.maxAmountFilter = 999999;
        this.minAmountFilter = 0;
        this.narrationFilter = '';
        this.referenceFilter = '';
        this.statusFilter = "All";
        this.bookIDFilter = "";
        this.getBatchListPreviews();
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    // createBatchListPreview(): void {
    //     this.createOrEditBatchListPreviewModal.show();
    // }

    deleteBatchListPreview(batchListPreview: BatchListPreviewDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._batchListPreviewsServiceProxy.delete(batchListPreview.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    onPostedChange(value): void {

        this.postedFilter = value;

        this.getBatchListPreviews();
        console.log(value);
    }

    onStatusChange(value): void {

        this.statusFilter = value;

        // this.approvedFilter = value;

        this.getBatchListPreviews();
        console.log(value);

    }

    onVoucherTypeChange(value): void {
        this.bookIDFilter = value;

        this.getBatchListPreviews();
        console.log(value);
    }

    exportToExcel(): void {
        this._batchListPreviewsServiceProxy.getBatchListPreviewsToExcel(
            this.filterText,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }


}
