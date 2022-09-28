import { Component, OnInit, ViewEncapsulation, ViewChild, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AppComponentBase } from '@shared/common/app-component-base';
// import { CreateOrEditEmployeeLeavesTotalModalComponent } from '../EmployeeLeavesTotal/create-or-edit-EmployeeLeavesTotal-modal.component';
import { EmployeeLeaveBalanceServiceProxy } from '../shared/services/employeeLeaveBalance.service';
import { employeeLeaveBalanceDto } from '../shared/dto/employeeLeaveBalance-dto';
import { CreateOrEditEmployeeLeaveBalanceModalComponent } from './create-or-edit-employeeLeaveBalance-modal.component';
import { ViewEmployeeLeaveBalanceModalComponent } from './view-employeeLeaveBalance-modal.component';



@Component({
  templateUrl: './employeeLeaveBalance.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class EmployeeLeaveBalanceComponent extends AppComponentBase {
  @ViewChild('createOrEditEmployeeLeaveBalanceModal', { static: true }) createOrEditEmployeeLeavesTotalModal: CreateOrEditEmployeeLeaveBalanceModalComponent;
  @ViewChild('viewEmployeeLeaveBalanceModal', { static: true }) viewEmployeeLeaveBalanceModal: ViewEmployeeLeaveBalanceModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxEmployeeLeavesTotalIDFilter: number;
  maxEmployeeLeavesTotalIDFilterEmpty: number;
  minEmployeeLeavesTotalIDFilter: number;
  minEmployeeLeavesTotalIDFilterEmpty: number;
  EmployeeLeavesTotalFilter = '';
  activeFilter = -1;
  createdByFilter = '';
  maxCreateDateFilter: moment.Moment;
  minCreateDateFilter: moment.Moment;
  audtUserFilter = '';
  maxAudtDateFilter: moment.Moment;
  minAudtDateFilter: moment.Moment;

  constructor(
    injector: Injector,
    private _employeeLeavesTotalService: EmployeeLeaveBalanceServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) {
    super(injector);
  }

  getEmployeeLeavesTotal(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }
    this.primengTableHelper.showLoadingIndicator();

    this._employeeLeavesTotalService.getAll(
      this.filterText,
      this.maxEmployeeLeavesTotalIDFilter == null ? this.maxEmployeeLeavesTotalIDFilterEmpty : this.maxEmployeeLeavesTotalIDFilter,
      this.minEmployeeLeavesTotalIDFilter == null ? this.minEmployeeLeavesTotalIDFilterEmpty : this.minEmployeeLeavesTotalIDFilter,
      this.EmployeeLeavesTotalFilter,
      this.activeFilter,
      this.createdByFilter,
      this.maxCreateDateFilter,
      this.minCreateDateFilter,
      this.audtUserFilter,
      this.maxAudtDateFilter,
      this.minAudtDateFilter,
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

  createEmployeeLeavesTotal(): void {
    this.createOrEditEmployeeLeavesTotalModal.show();
  }

  deleteEmployeeLeavesTotal(employeeLeavesTotal: employeeLeaveBalanceDto): void {
    this.message.confirm(
      '',
      (isConfirmed) => {
        if (isConfirmed) {
          this._employeeLeavesTotalService.delete(employeeLeavesTotal.id)
            .subscribe(() => {
              this.reloadPage();
              this.notify.success(this.l('SuccessfullyDeleted'));
            });
        }
      }
    );
  }

  exportToExcel(): void {
    this._employeeLeavesTotalService.GetEmployeeLeavesTotalToExcel(
      this.filterText,
      this.maxEmployeeLeavesTotalIDFilter == null ? this.maxEmployeeLeavesTotalIDFilterEmpty : this.maxEmployeeLeavesTotalIDFilter,
      this.minEmployeeLeavesTotalIDFilter == null ? this.minEmployeeLeavesTotalIDFilterEmpty : this.minEmployeeLeavesTotalIDFilter,
      this.EmployeeLeavesTotalFilter,
      this.activeFilter,
      this.createdByFilter,
      this.maxCreateDateFilter,
      this.minCreateDateFilter,
      this.audtUserFilter,
      this.maxAudtDateFilter,
      this.minAudtDateFilter,
    )
      .subscribe(result => {
        this._fileDownloadService.downloadTempFile(result);
      });
  }

}
