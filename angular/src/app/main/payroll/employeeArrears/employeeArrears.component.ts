import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEmployeeArrearsModalComponent } from './create-or-edit-employeeArrears-modal.component';
import { ViewEmployeeArrearsModalComponent } from './view-employeeArrears-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { EmployeeArrearsServiceProxy } from '../shared/services/employeeArrears.service';
import { EmployeeArrearsDto } from '../shared/dto/employeeArrears-dto';

@Component({
    templateUrl: './employeeArrears.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class EmployeeArrearsComponent extends AppComponentBase {

    @ViewChild('createOrEditEmployeeArrearsModal', { static: true }) createOrEditEmployeeArrearsModal: CreateOrEditEmployeeArrearsModalComponent;
    @ViewChild('viewEmployeeArrearsModalComponent', { static: true }) viewEmployeeArrearsModal: ViewEmployeeArrearsModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxArrearIDFilter: number;
    maxArrearIDFilterEmpty: number;
    minArrearIDFilter: number;
    minArrearIDFilterEmpty: number;
    maxEmployeeIDFilter: number;
    maxEmployeeIDFilterEmpty: number;
    minEmployeeIDFilter: number;
    minEmployeeIDFilterEmpty: number;
    employeeNameFilter = '';
    maxSalaryYearFilter: number;
    maxSalaryYearFilterEmpty: number;
    minSalaryYearFilter: number;
    minSalaryYearFilterEmpty: number;
    maxSalaryMonthFilter: number;
    maxSalaryMonthFilterEmpty: number;
    minSalaryMonthFilter: number;
    minSalaryMonthFilterEmpty: number;
    maxArrearDateFilter: moment.Moment;
    minArrearDateFilter: moment.Moment;
    maxAmountFilter: number;
    maxAmountFilterEmpty: number;
    minAmountFilter: number;
    minAmountFilterEmpty: number;
    activeFilter = -1;
    audtUserFilter = '';
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    createdByFilter = '';
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;




    constructor(
        injector: Injector,
        private _employeeArrearsServiceProxy: EmployeeArrearsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getEmployeeArrears(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._employeeArrearsServiceProxy.getAll(
            this.filterText,
            this.maxArrearIDFilter == null ? this.maxArrearIDFilterEmpty : this.maxArrearIDFilter,
            this.minArrearIDFilter == null ? this.minArrearIDFilterEmpty : this.minArrearIDFilter,
            this.maxEmployeeIDFilter == null ? this.maxEmployeeIDFilterEmpty : this.maxEmployeeIDFilter,
            this.minEmployeeIDFilter == null ? this.minEmployeeIDFilterEmpty : this.minEmployeeIDFilter,
            this.employeeNameFilter,
            this.maxSalaryYearFilter == null ? this.maxSalaryYearFilterEmpty : this.maxSalaryYearFilter,
            this.minSalaryYearFilter == null ? this.minSalaryYearFilterEmpty : this.minSalaryYearFilter,
            this.maxSalaryMonthFilter == null ? this.maxSalaryMonthFilterEmpty : this.maxSalaryMonthFilter,
            this.minSalaryMonthFilter == null ? this.minSalaryMonthFilterEmpty : this.minSalaryMonthFilter,
            this.maxArrearDateFilter,
            this.minArrearDateFilter,
            this.maxAmountFilter == null ? this.maxAmountFilterEmpty : this.maxAmountFilter,
            this.minAmountFilter == null ? this.minAmountFilterEmpty : this.minAmountFilter,
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
            debugger;
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createEmployeeArrears(): void {
        this.createOrEditEmployeeArrearsModal.show();
    }

    deleteEmployeeArrears(employeeArrears: EmployeeArrearsDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._employeeArrearsServiceProxy.delete(employeeArrears.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._employeeArrearsServiceProxy.getEmployeeArrearsToExcel(
            this.filterText,
            this.maxArrearIDFilter == null ? this.maxArrearIDFilterEmpty : this.maxArrearIDFilter,
            this.minArrearIDFilter == null ? this.minArrearIDFilterEmpty : this.minArrearIDFilter,
            this.maxEmployeeIDFilter == null ? this.maxEmployeeIDFilterEmpty : this.maxEmployeeIDFilter,
            this.minEmployeeIDFilter == null ? this.minEmployeeIDFilterEmpty : this.minEmployeeIDFilter,
            this.employeeNameFilter,
            this.maxSalaryYearFilter == null ? this.maxSalaryYearFilterEmpty : this.maxSalaryYearFilter,
            this.minSalaryYearFilter == null ? this.minSalaryYearFilterEmpty : this.minSalaryYearFilter,
            this.maxSalaryMonthFilter == null ? this.maxSalaryMonthFilterEmpty : this.maxSalaryMonthFilter,
            this.minSalaryMonthFilter == null ? this.minSalaryMonthFilterEmpty : this.minSalaryMonthFilter,
            this.maxArrearDateFilter,
            this.minArrearDateFilter,
            this.maxAmountFilter == null ? this.maxAmountFilterEmpty : this.maxAmountFilter,
            this.minAmountFilter == null ? this.minAmountFilterEmpty : this.minAmountFilter,
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
