import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditAllowancesModalComponent } from './create-or-edit-allowances-modal.component';

import { ViewAllowancesModalComponent } from './view-allowances-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { AllowancesServiceProxy } from '../shared/services/allowances.service';
import { AllowancesDto } from '../shared/dto/allowances-dto';

@Component({
    templateUrl: './allowances.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class AllowancesComponent extends AppComponentBase {


    @ViewChild('createOrEditAllowancesModal', { static: true }) createOrEditAllowancesModal: CreateOrEditAllowancesModalComponent;
    @ViewChild('viewAllowancesModalComponent', { static: true }) viewAllowancesModal: ViewAllowancesModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDocIDFilter: number;
    maxDocIDFilterEmpty: number;
    minDocIDFilter: number;
    minDocIDFilterEmpty: number;
    maxDocdateFilter: moment.Moment;
    minDocdateFilter: moment.Moment;
    maxDocMonthFilter: number;
    maxDocMonthFilterEmpty: number;
    minDocMonthFilter: number;
    minDocMonthFilterEmpty: number;
    maxDocYearFilter: number;
    maxDocYearFilterEmpty: number;
    minDocYearFilter: number;
    minDocYearFilterEmpty: number;
    audtUserFilter = '';
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    createdByFilter = '';
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;




    constructor(
        injector: Injector,
        private _allowancesServiceProxy: AllowancesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getAllowances(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._allowancesServiceProxy.getAll(
            this.filterText,
            this.maxDocIDFilter == null ? this.maxDocIDFilterEmpty : this.maxDocIDFilter,
            this.minDocIDFilter == null ? this.minDocIDFilterEmpty : this.minDocIDFilter,
            this.maxDocdateFilter,
            this.minDocdateFilter,
            this.maxDocMonthFilter == null ? this.maxDocMonthFilterEmpty : this.maxDocMonthFilter,
            this.minDocMonthFilter == null ? this.minDocMonthFilterEmpty : this.minDocMonthFilter,
            this.maxDocYearFilter == null ? this.maxDocYearFilterEmpty : this.maxDocYearFilter,
            this.minDocYearFilter == null ? this.minDocYearFilterEmpty : this.minDocYearFilter,
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

    createAllowances(): void {
        this.createOrEditAllowancesModal.show(false);
    }


    deleteAllowances(allowances: AllowancesDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._allowancesServiceProxy.delete(allowances.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._allowancesServiceProxy.getAllowancesToExcel(
            this.filterText,
            this.maxDocIDFilter == null ? this.maxDocIDFilterEmpty : this.maxDocIDFilter,
            this.minDocIDFilter == null ? this.minDocIDFilterEmpty : this.minDocIDFilter,
            this.maxDocdateFilter,
            this.minDocdateFilter,
            this.maxDocMonthFilter == null ? this.maxDocMonthFilterEmpty : this.maxDocMonthFilter,
            this.minDocMonthFilter == null ? this.minDocMonthFilterEmpty : this.minDocMonthFilter,
            this.maxDocYearFilter == null ? this.maxDocYearFilterEmpty : this.maxDocYearFilter,
            this.minDocYearFilter == null ? this.minDocYearFilterEmpty : this.minDocYearFilter,
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
