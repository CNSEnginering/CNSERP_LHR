import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditDeductionTypesModalComponent } from './create-or-edit-deductionTypes-modal.component';

import { ViewDeductionTypesModalComponent } from './view-deductionTypes-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { DeductionTypesServiceProxy } from '../shared/services/deductionTypes.service';
import { DeductionTypesDto } from '../shared/dto/deductionTypes-dto';

@Component({
    templateUrl: './deductionTypes.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class DeductionTypesComponent extends AppComponentBase {


    @ViewChild('createOrEditDeductionTypesModal', { static: true }) createOrEditDeductionTypesModal: CreateOrEditDeductionTypesModalComponent;
    @ViewChild('viewDeductionTypesModalComponent', { static: true }) viewDeductionTypesModal: ViewDeductionTypesModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxTypeIDFilter: number;
    maxTypeIDFilterEmpty: number;
    minTypeIDFilter: number;
    minTypeIDFilterEmpty: number;
    typeDescFilter = '';
    activeFilter = -1;
    audtUserFilter = '';
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    createdByFilter = '';
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;




    constructor(
        injector: Injector,
        private _deductionTypesServiceProxy: DeductionTypesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getDeductionTypes(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._deductionTypesServiceProxy.getAll(
            this.filterText,
            this.maxTypeIDFilter == null ? this.maxTypeIDFilterEmpty : this.maxTypeIDFilter,
            this.minTypeIDFilter == null ? this.minTypeIDFilterEmpty : this.minTypeIDFilter,
            this.typeDescFilter,
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

    createDeductionTypes(): void {
        this.createOrEditDeductionTypesModal.show();
    }


    deleteDeductionTypes(deductionTypes: DeductionTypesDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._deductionTypesServiceProxy.delete(deductionTypes.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._deductionTypesServiceProxy.getDeductionTypesToExcel(
            this.filterText,
            this.maxTypeIDFilter == null ? this.maxTypeIDFilterEmpty : this.maxTypeIDFilter,
            this.minTypeIDFilter == null ? this.minTypeIDFilterEmpty : this.minTypeIDFilter,
            this.typeDescFilter,
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
