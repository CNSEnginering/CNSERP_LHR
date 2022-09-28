import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEmployeeLeavesModalComponent } from './create-or-edit-employeeLeaves-modal.component';
import { ViewEmployeeLeavesModalComponent } from './view-employeeLeaves-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { EmployeeLeavesServiceProxy } from '../shared/services/employeeLeaves-service';
import { EmployeeLeavesDto } from '../shared/dto/employeeLeaves-dto';

@Component({
  templateUrl: './employeeLeaves.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class EmployeeLeavesComponent extends AppComponentBase {

  @ViewChild('createOrEditEmployeeLeavesModal', { static: true }) createOrEditEmployeeLeavesModal: CreateOrEditEmployeeLeavesModalComponent;
  @ViewChild('viewEmployeeLeavesModalComponent', { static: true }) viewEmployeeLeavesModal: ViewEmployeeLeavesModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxEmployeeIDFilter: number;
  maxEmployeeIDFilterEmpty: number;
  minEmployeeIDFilter: number;
  minEmployeeIDFilterEmpty: number;
  maxLeaveIDFilter: number;
  maxLeaveIDFilterEmpty: number;
  minLeaveIDFilter: number;
  minLeaveIDFilterEmpty: number;
  maxSalaryYearFilter: number;
  maxSalaryYearFilterEmpty: number;
  minSalaryYearFilter: number;
  minSalaryYearFilterEmpty: number;
  maxSalaryMonthFilter: number;
  maxSalaryMonthFilterEmpty: number;
  minSalaryMonthFilter: number;
  minSalaryMonthFilterEmpty: number;
  maxStartDateFilter: moment.Moment;
  minStartDateFilter: moment.Moment;
  maxLeaveTypeFilter: number;
  maxLeaveTypeFilterEmpty: number;
  minLeaveTypeFilter: number;
  minLeaveTypeFilterEmpty: number;
  maxCasualFilter: number;
  maxCasualFilterEmpty: number;
  minCasualFilter: number;
  minCasualFilterEmpty: number;
  maxSickFilter: number;
  maxSickFilterEmpty: number;
  minSickFilter: number;
  minSickFilterEmpty: number;
  maxAnnualFilter: number;
  maxAnnualFilterEmpty: number;
  minAnnualFilter: number;
  minAnnualFilterEmpty: number;
  payTypeFilter = '';
  remarksFilter = '';
  audtUserFilter = '';
  maxAudtDateFilter: moment.Moment;
  minAudtDateFilter: moment.Moment;
  createdByFilter = '';
  maxCreateDateFilter: moment.Moment;
  minCreateDateFilter: moment.Moment;




  constructor(
    injector: Injector,
    private _employeeLeavesServiceProxy: EmployeeLeavesServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) {
    super(injector);
  }

  getEmployeeLeaves(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();

    this._employeeLeavesServiceProxy.getAll(
      this.filterText,
      this.maxEmployeeIDFilter == null ? this.maxEmployeeIDFilterEmpty : this.maxEmployeeIDFilter,
      this.minEmployeeIDFilter == null ? this.minEmployeeIDFilterEmpty : this.minEmployeeIDFilter,
      this.maxLeaveIDFilter == null ? this.maxLeaveIDFilterEmpty : this.maxLeaveIDFilter,
      this.minLeaveIDFilter == null ? this.minLeaveIDFilterEmpty : this.minLeaveIDFilter,
      this.maxSalaryYearFilter == null ? this.maxSalaryYearFilterEmpty : this.maxSalaryYearFilter,
      this.minSalaryYearFilter == null ? this.minSalaryYearFilterEmpty : this.minSalaryYearFilter,
      this.maxSalaryMonthFilter == null ? this.maxSalaryMonthFilterEmpty : this.maxSalaryMonthFilter,
      this.minSalaryMonthFilter == null ? this.minSalaryMonthFilterEmpty : this.minSalaryMonthFilter,
      this.maxStartDateFilter,
      this.minStartDateFilter,
      this.maxLeaveTypeFilter == null ? this.maxLeaveTypeFilterEmpty : this.maxLeaveTypeFilter,
      this.minLeaveTypeFilter == null ? this.minLeaveTypeFilterEmpty : this.minLeaveTypeFilter,
      this.maxCasualFilter == null ? this.maxCasualFilterEmpty : this.maxCasualFilter,
      this.minCasualFilter == null ? this.minCasualFilterEmpty : this.minCasualFilter,
      this.maxSickFilter == null ? this.maxSickFilterEmpty : this.maxSickFilter,
      this.minSickFilter == null ? this.minSickFilterEmpty : this.minSickFilter,
      this.maxAnnualFilter == null ? this.maxAnnualFilterEmpty : this.maxAnnualFilter,
      this.minAnnualFilter == null ? this.minAnnualFilterEmpty : this.minAnnualFilter,
      this.payTypeFilter,
      this.remarksFilter,
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

  createEmployeeLeaves(): void {
    this.createOrEditEmployeeLeavesModal.show();
  }

  deleteEmployeeLeaves(employeeLeaves: EmployeeLeavesDto): void {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._employeeLeavesServiceProxy.delete(employeeLeaves.id)
            .subscribe(() => {
              this.reloadPage();
              this.notify.success(this.l('SuccessfullyDeleted'));
            });
        }
      }
    );
  }

  exportToExcel(): void {
    this._employeeLeavesServiceProxy.getEmployeeLeavesToExcel(
      this.filterText,
      this.maxEmployeeIDFilter == null ? this.maxEmployeeIDFilterEmpty : this.maxEmployeeIDFilter,
      this.minEmployeeIDFilter == null ? this.minEmployeeIDFilterEmpty : this.minEmployeeIDFilter,
      this.maxLeaveIDFilter == null ? this.maxLeaveIDFilterEmpty : this.maxLeaveIDFilter,
      this.minLeaveIDFilter == null ? this.minLeaveIDFilterEmpty : this.minLeaveIDFilter,
      this.maxSalaryYearFilter == null ? this.maxSalaryYearFilterEmpty : this.maxSalaryYearFilter,
      this.minSalaryYearFilter == null ? this.minSalaryYearFilterEmpty : this.minSalaryYearFilter,
      this.maxSalaryMonthFilter == null ? this.maxSalaryMonthFilterEmpty : this.maxSalaryMonthFilter,
      this.minSalaryMonthFilter == null ? this.minSalaryMonthFilterEmpty : this.minSalaryMonthFilter,
      this.maxStartDateFilter,
      this.minStartDateFilter,
      this.maxLeaveTypeFilter == null ? this.maxLeaveTypeFilterEmpty : this.maxLeaveTypeFilter,
      this.minLeaveTypeFilter == null ? this.minLeaveTypeFilterEmpty : this.minLeaveTypeFilter,
      this.maxCasualFilter == null ? this.maxCasualFilterEmpty : this.maxCasualFilter,
      this.minCasualFilter == null ? this.minCasualFilterEmpty : this.minCasualFilter,
      this.maxSickFilter == null ? this.maxSickFilterEmpty : this.maxSickFilter,
      this.minSickFilter == null ? this.minSickFilterEmpty : this.minSickFilter,
      this.maxAnnualFilter == null ? this.maxAnnualFilterEmpty : this.maxAnnualFilter,
      this.minAnnualFilter == null ? this.minAnnualFilterEmpty : this.minAnnualFilter,
      this.payTypeFilter,
      this.remarksFilter,
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
