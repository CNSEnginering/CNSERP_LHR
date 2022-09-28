import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditSubDesignationsModalComponent } from './create-or-edit-subDesignations-modal.component';

import { ViewSubDesignationsModalComponent } from './view-subDesignations-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { SubDesignationsServiceProxy } from '../shared/services/subDesignations.service';
import { SubDesignationsDto } from '../shared/dto/subDesignations-dto';

@Component({
    templateUrl: './subDesignations.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class SubDesignationsComponent extends AppComponentBase {


    @ViewChild('createOrEditSubDesignationsModal', { static: true }) createOrEditSubDesignationsModal: CreateOrEditSubDesignationsModalComponent;
    @ViewChild('viewSubDesignationsModalComponent', { static: true }) viewSubDesignationsModal: ViewSubDesignationsModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxSubDesignationIDFilter: number;
    maxSubDesignationIDFilterEmpty: number;
    minSubDesignationIDFilter: number;
    minSubDesignationIDFilterEmpty: number;
    subDesignationFilter = '';
    activeFilter = -1;
    audtUserFilter = '';
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    createdByFilter = '';
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;




    constructor(
        injector: Injector,
        private _subDesignationsServiceProxy: SubDesignationsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getSubDesignations(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._subDesignationsServiceProxy.getAll(
            this.filterText,
            this.maxSubDesignationIDFilter == null ? this.maxSubDesignationIDFilterEmpty : this.maxSubDesignationIDFilter,
            this.minSubDesignationIDFilter == null ? this.minSubDesignationIDFilterEmpty : this.minSubDesignationIDFilter,
            this.subDesignationFilter,
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

    createSubDesignations(): void {
        this.createOrEditSubDesignationsModal.show();
    }


    deleteSubDesignations(subDesignations: SubDesignationsDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._subDesignationsServiceProxy.delete(subDesignations.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._subDesignationsServiceProxy.getSubDesignationsToExcel(
            this.filterText,
            this.maxSubDesignationIDFilter == null ? this.maxSubDesignationIDFilterEmpty : this.maxSubDesignationIDFilter,
            this.minSubDesignationIDFilter == null ? this.minSubDesignationIDFilterEmpty : this.minSubDesignationIDFilter,
            this.subDesignationFilter,
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
