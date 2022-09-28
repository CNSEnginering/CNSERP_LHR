import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditSalarylockComponent } from './create-or-edit-salarylock.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { salarylockDTo } from '../shared/dto/salarylock-dto';
import { SalarylockServiceProxy } from '../shared/services/salarylock.service';
@Component({
  
  templateUrl: './salarylock.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class SalarylockComponent extends AppComponentBase {

  @ViewChild('createOrEditsalarylockModal', { static: true }) createOrEditsalarylockModal: CreateOrEditSalarylockComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  constructor(
    injector: Injector,
    private _SalaryLockServiceProxy: SalarylockServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
) {
    super(injector);
}
advancedFiltersAreShown = false;
filterText = '';
monthfilter: number;
yearfilter: number;
minSecIDFilter: string;
minSecIDFilterEmpty: string;
secNameFilter = '';
activeFilter = -1;
audtUserFilter = '';
maxAudtDateFilter: moment.Moment;
minAudtDateFilter: moment.Moment;
createdByFilter = '';
maxCreateDateFilter: moment.Moment;
minCreateDateFilter: moment.Moment;

getSalarylock(event?: LazyLoadEvent) {
  if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
  }

  this.primengTableHelper.showLoadingIndicator();

  this._SalaryLockServiceProxy.getAll(
      this.filterText,
      this.monthfilter == null ? this.monthfilter : this.monthfilter,
      this.yearfilter == null ? this.yearfilter : this.yearfilter,
      // this.primengTableHelper.getSorting(this.dataTable),
      // this.primengTableHelper.getSkipCount(this.paginator, event),
      // this.primengTableHelper.getMaxResultCount(this.paginator, event)
  ).subscribe(result => {
    debugger
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      
  });
}

reloadPage(): void {
  this.paginator.changePage(this.paginator.getPage());
}

createSalaryLock(): void {
 this.createOrEditsalarylockModal.show();
}

deleteSection(section: salarylockDTo): void {
  this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
          if (isConfirmed) {
              this._SalaryLockServiceProxy.delete(section.id)
                  .subscribe(() => {
                      this.reloadPage();
                      this.notify.success(this.l('SuccessfullyDeleted'));
                  });
          }
      }
  );
}

exportToExcel(): void {
  this._SalaryLockServiceProxy.GetDataToExcel(
      this.filterText,
      this.minSecIDFilter == null ? this.minSecIDFilterEmpty : this.minSecIDFilter,
      this.secNameFilter,
      this.activeFilter,
      
  ).subscribe(result => {
    debugger
          this._fileDownloadService.downloadTempFile(result["result"]);
      });
 }

}
