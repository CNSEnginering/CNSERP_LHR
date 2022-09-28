import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEmployeeEarningsModalComponent } from './create-or-edit-employeeEarnings-modal.component';
import { ViewEmployeeEarningsModalComponent } from './view-employeeEarnings-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { EmployeeEarningsServiceProxy } from '../shared/services/employeeEarnings.service';
import { EmployeeEarningsDto } from '../shared/dto/employeeEarnings-dto';
import { CreateOrEditEmployeeDeductionsModalComponent } from '../employeeDeductions/create-or-edit-employeeDeductions-modal.component';

@Component({
    templateUrl: './employeeEarnings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class EmployeeEarningsComponent extends AppComponentBase {

    @ViewChild('createOrEditEmployeeEarningsModal', { static: true }) createOrEditEmployeeEarningsModal: CreateOrEditEmployeeEarningsModalComponent;

    @ViewChild('createOrEditEmployeeDeductionsModal', { static: true }) createOrEditEmployeeDeductionsModal: CreateOrEditEmployeeDeductionsModalComponent;

    @ViewChild('viewEmployeeEarningsModalComponent', { static: true }) viewEmployeeEarningsModal: ViewEmployeeEarningsModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxEarningIDFilter: number;
    maxEarningIDFilterEmpty: number;
    minEarningIDFilter: number;
    minEarningIDFilterEmpty: number;
    employeeNameFilter= '';
    maxEmployeeIDFilter: number;
    maxEmployeeIDFilterEmpty: number;
    minEmployeeIDFilter: number;
    minEmployeeIDFilterEmpty: number;
    maxSalaryYearFilter: number;
    maxSalaryYearFilterEmpty: number;
    minSalaryYearFilter: number;
    minSalaryYearFilterEmpty: number;
    maxSalaryMonthFilter: number;
    maxSalaryMonthFilterEmpty: number;
    minSalaryMonthFilter: number;
    minSalaryMonthFilterEmpty: number;
    maxEarningDateFilter: moment.Moment;
    minEarningDateFilter: moment.Moment;
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
        private _employeeEarningsServiceProxy: EmployeeEarningsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getEmployeeEarnings(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._employeeEarningsServiceProxy.getAll(
            this.filterText,
            this.maxEarningIDFilter == null ? this.maxEarningIDFilterEmpty : this.maxEarningIDFilter,
            this.minEarningIDFilter == null ? this.minEarningIDFilterEmpty : this.minEarningIDFilter,
            this.maxEmployeeIDFilter == null ? this.maxEmployeeIDFilterEmpty : this.maxEmployeeIDFilter,
            this.minEmployeeIDFilter == null ? this.minEmployeeIDFilterEmpty : this.minEmployeeIDFilter,
            this.employeeNameFilter,
            this.maxSalaryYearFilter == null ? this.maxSalaryYearFilterEmpty : this.maxSalaryYearFilter,
            this.minSalaryYearFilter == null ? this.minSalaryYearFilterEmpty : this.minSalaryYearFilter,
            this.maxSalaryMonthFilter == null ? this.maxSalaryMonthFilterEmpty : this.maxSalaryMonthFilter,
            this.minSalaryMonthFilter == null ? this.minSalaryMonthFilterEmpty : this.minSalaryMonthFilter,
            this.maxEarningDateFilter,
            this.minEarningDateFilter,
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
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createEmployeeEarnings(): void {
        this.createOrEditEmployeeDeductionsModal.show(undefined,"earnings");
    }

    deleteEmployeeEarnings(employeeEarnings: EmployeeEarningsDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._employeeEarningsServiceProxy.delete(employeeEarnings.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._employeeEarningsServiceProxy.getEmployeeEarningsToExcel(
            this.filterText,
            this.maxEarningIDFilter == null ? this.maxEarningIDFilterEmpty : this.maxEarningIDFilter,
            this.minEarningIDFilter == null ? this.minEarningIDFilterEmpty : this.minEarningIDFilter,
            this.maxEmployeeIDFilter == null ? this.maxEmployeeIDFilterEmpty : this.maxEmployeeIDFilter,
            this.minEmployeeIDFilter == null ? this.minEmployeeIDFilterEmpty : this.minEmployeeIDFilter,
            this.employeeNameFilter,
            this.maxSalaryYearFilter == null ? this.maxSalaryYearFilterEmpty : this.maxSalaryYearFilter,
            this.minSalaryYearFilter == null ? this.minSalaryYearFilterEmpty : this.minSalaryYearFilter,
            this.maxSalaryMonthFilter == null ? this.maxSalaryMonthFilterEmpty : this.maxSalaryMonthFilter,
            this.minSalaryMonthFilter == null ? this.minSalaryMonthFilterEmpty : this.minSalaryMonthFilter,
            this.maxEarningDateFilter,
            this.minEarningDateFilter,
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
