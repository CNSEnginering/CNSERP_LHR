import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEmployeeTypeModalComponent } from './create-or-edit-employeeType-modal.component';
import { ViewEmployeeTypeModalComponent } from './view-employeeType-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { EmployeeTypeServiceProxy } from '../shared/services/employeeType.service';
import { EmployeeTypeDto } from '../shared/dto/employeeType-dto';

@Component({
    templateUrl: './employeeType.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class EmployeeTypeComponent extends AppComponentBase {

    @ViewChild('createOrEditEmployeeTypeModal', { static: true }) createOrEditEmployeeTypeModal: CreateOrEditEmployeeTypeModalComponent;
    @ViewChild('viewEmployeeTypeModalComponent', { static: true }) viewEmployeeTypeModal: ViewEmployeeTypeModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxTypeIDFilter: number;
    maxTypeIDFilterEmpty: number;
    minTypeIDFilter: number;
    minTypeIDFilterEmpty: number;
    empTypeFilter = '';
    activeFilter = -1;
    audtUserFilter = '';
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    createdByFilter = '';
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;




    constructor(
        injector: Injector,
        private _employeeTypeServiceProxy: EmployeeTypeServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getEmployeeType(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._employeeTypeServiceProxy.getAll(
            this.filterText,
            this.maxTypeIDFilter == null ? this.maxTypeIDFilterEmpty : this.maxTypeIDFilter,
            this.minTypeIDFilter == null ? this.minTypeIDFilterEmpty : this.minTypeIDFilter,
            this.empTypeFilter,
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

    createEmployeeType(): void {
        this.createOrEditEmployeeTypeModal.show();
    }

    deleteEmployeeType(employeeType: EmployeeTypeDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._employeeTypeServiceProxy.delete(employeeType.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._employeeTypeServiceProxy.getEmployeeTypeToExcel(
            this.filterText,
            this.maxTypeIDFilter == null ? this.maxTypeIDFilterEmpty : this.maxTypeIDFilter,
            this.minTypeIDFilter == null ? this.minTypeIDFilterEmpty : this.minTypeIDFilter,
            this.empTypeFilter,
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
