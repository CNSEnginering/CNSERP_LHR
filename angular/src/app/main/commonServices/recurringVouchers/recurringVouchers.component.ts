import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditRecurringVoucherModalComponent } from './create-or-edit-recurringVoucher-modal.component';

import { ViewRecurringVoucherModalComponent } from './view-recurringVoucher-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { RecurringVouchersServiceProxy } from '../shared/services/recurringVouchers.service';
import { RecurringVoucherDto } from '../shared/dto/recurringVouchers-dto';

@Component({
    templateUrl: './recurringVouchers.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class RecurringVouchersComponent extends AppComponentBase {

    @ViewChild('createOrEditRecurringVoucherModal', { static: true }) createOrEditRecurringVoucherModal: CreateOrEditRecurringVoucherModalComponent;
    @ViewChild('viewRecurringVoucherModalComponent', { static: true }) viewRecurringVoucherModal: ViewRecurringVoucherModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDocNoFilter: number;
    maxDocNoFilterEmpty: number;
    minDocNoFilter: number;
    minDocNoFilterEmpty: number;
    bookIDFilter = '';
    maxVoucherNoFilter: number;
    maxVoucherNoFilterEmpty: number;
    minVoucherNoFilter: number;
    minVoucherNoFilterEmpty: number;
    fmtVoucherNoFilter = '';
    maxVoucherDateFilter: moment.Moment;
    minVoucherDateFilter: moment.Moment;
    maxVoucherMonthFilter: number;
    maxVoucherMonthFilterEmpty: number;
    minVoucherMonthFilter: number;
    minVoucherMonthFilterEmpty: number;
    maxConfigIDFilter: number;
    maxConfigIDFilterEmpty: number;
    minConfigIDFilter: number;
    minConfigIDFilterEmpty: number;
    referenceFilter = '';
    activeFilter = -1;
    audtUserFilter = '';
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    createdByFilter = '';
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;




    constructor(
        injector: Injector,
        private _recurringVouchersServiceProxy: RecurringVouchersServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getRecurringVouchers(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._recurringVouchersServiceProxy.getAll(
            this.filterText,
            this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty : this.maxDocNoFilter,
            this.minDocNoFilter == null ? this.minDocNoFilterEmpty : this.minDocNoFilter,
            this.bookIDFilter,
            this.maxVoucherNoFilter == null ? this.maxVoucherNoFilterEmpty : this.maxVoucherNoFilter,
            this.minVoucherNoFilter == null ? this.minVoucherNoFilterEmpty : this.minVoucherNoFilter,
            this.fmtVoucherNoFilter,
            this.maxVoucherDateFilter,
            this.minVoucherDateFilter,
            this.maxVoucherMonthFilter == null ? this.maxVoucherMonthFilterEmpty : this.maxVoucherMonthFilter,
            this.minVoucherMonthFilter == null ? this.minVoucherMonthFilterEmpty : this.minVoucherMonthFilter,
            this.maxConfigIDFilter == null ? this.maxConfigIDFilterEmpty : this.maxConfigIDFilter,
            this.minConfigIDFilter == null ? this.minConfigIDFilterEmpty : this.minConfigIDFilter,
            this.referenceFilter,
            this.activeFilter,
            this.audtUserFilter,
            this.maxAudtDateFilter,
            this.minAudtDateFilter,
            this.createdByFilter,
            this.maxCreateDateFilter,
            this.minCreateDateFilter,
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

    createRecurringVoucher(): void {
        this.createOrEditRecurringVoucherModal.show();
    }


    deleteRecurringVoucher(recurringVoucher: RecurringVoucherDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._recurringVouchersServiceProxy.delete(recurringVoucher.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._recurringVouchersServiceProxy.getRecurringVouchersToExcel(
            this.filterText,
            this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty : this.maxDocNoFilter,
            this.minDocNoFilter == null ? this.minDocNoFilterEmpty : this.minDocNoFilter,
            this.bookIDFilter,
            this.maxVoucherNoFilter == null ? this.maxVoucherNoFilterEmpty : this.maxVoucherNoFilter,
            this.minVoucherNoFilter == null ? this.minVoucherNoFilterEmpty : this.minVoucherNoFilter,
            this.fmtVoucherNoFilter,
            this.maxVoucherDateFilter,
            this.minVoucherDateFilter,
            this.maxVoucherMonthFilter == null ? this.maxVoucherMonthFilterEmpty : this.maxVoucherMonthFilter,
            this.minVoucherMonthFilter == null ? this.minVoucherMonthFilterEmpty : this.minVoucherMonthFilter,
            this.maxConfigIDFilter == null ? this.maxConfigIDFilterEmpty : this.maxConfigIDFilter,
            this.minConfigIDFilter == null ? this.minConfigIDFilterEmpty : this.minConfigIDFilter,
            this.referenceFilter,
            this.activeFilter,
            this.audtUserFilter,
            this.maxAudtDateFilter,
            this.minAudtDateFilter,
            this.createdByFilter,
            this.maxCreateDateFilter,
            this.minCreateDateFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
