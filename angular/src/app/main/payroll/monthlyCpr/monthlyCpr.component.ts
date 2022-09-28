import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditMonthlyCprComponent } from './create-or-edit-monthlyCpr.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { MonthlyCPRDto } from '../shared/dto/monthlyCpr-dto';
import { monthlyCprServiceProxy } from '../shared/services/monthlyCpr.service';

@Component({
  selector: 'app-monthlyCpr',
  templateUrl: './monthlyCpr.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class MonthlyCprComponent extends AppComponentBase {

  @ViewChild('createOrEditmonthlyCprModel', { static: true }) createOrEditmonthlyCprModel: CreateOrEditMonthlyCprComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  constructor(
    injector: Injector,
    private _MonthlyCprServiceProxy: monthlyCprServiceProxy,
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

getAllMonthlyCpr(event?: LazyLoadEvent) {
  if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
  }

  this.primengTableHelper.showLoadingIndicator();

  this._MonthlyCprServiceProxy.getAll(
      this.filterText,
      this.monthfilter == null ? this.monthfilter : this.monthfilter,
      this.yearfilter == null ? this.yearfilter : this.yearfilter,
      //this.primengTableHelper.getSorting(this.dataTable),
      this.primengTableHelper.getSkipCount(this.paginator, event),
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

createMonthlyCpr(): void {
 this.createOrEditmonthlyCprModel.show();
}

deleteSection(section: MonthlyCPRDto): void {
  this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
          if (isConfirmed) {
              this._MonthlyCprServiceProxy.delete(section.id)
                  .subscribe(() => {
                      this.reloadPage();
                      this.notify.success(this.l('SuccessfullyDeleted'));
                  });
          }
      }
  );
}

exportToExcel(): void {
  this._MonthlyCprServiceProxy.GetDataToExcel(
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
