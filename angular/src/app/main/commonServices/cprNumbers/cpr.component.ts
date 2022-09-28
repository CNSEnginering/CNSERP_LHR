import { Component, OnInit, ViewEncapsulation, ViewChild, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { TokenAuthServiceProxy, CPRServiceProxy, CPRDto } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditCprModalComponent } from './create-or-edit-cpr-modal.component';
import { ViewCprModalComponent } from './view-cpr-modal.component';

@Component({
  templateUrl: './cpr.component.html',   
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class CprComponent extends AppComponentBase {
  @ViewChild('createOrEditCprModal', { static: true }) createOrEditCprModal: CreateOrEditCprModalComponent;
  @ViewChild('viewCprModal', { static: true }) viewCprModal: ViewCprModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxCprIdFilter : number;
	maxCprIdFilterEmpty : number;
	minCprIdFilter : number;
	minCprIdFilterEmpty : number;
  cprNoFilter = '';
  activeFilter=-1;
  createdByFilter = '';
  maxCreateDateFilter : moment.Moment;
  minCreateDateFilter : moment.Moment;
  audtUserFilter = '';
  maxAudtDateFilter : moment.Moment;
  minAudtDateFilter : moment.Moment;  

  constructor(
    injector: Injector,
    private _cprService: CPRServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) { 
    super(injector); 
  }

  getCpr(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
        this.paginator.changePage(0);
        return;
    }
debugger;
    this.primengTableHelper.showLoadingIndicator();

    this._cprService.getAll(
      this.filterText,
      this.maxCprIdFilter == null ? this.maxCprIdFilterEmpty : this.maxCprIdFilter,
      this.minCprIdFilter == null ? this.minCprIdFilterEmpty : this.minCprIdFilter,
      this.cprNoFilter,
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
       debugger;
        this.primengTableHelper.totalRecordsCount = result.totalCount;
        this.primengTableHelper.records = result.items;
        this.primengTableHelper.hideLoadingIndicator();
    });
}

reloadPage(): void {
  this.paginator.changePage(this.paginator.getPage());
}

createCpr(): void {
  this.createOrEditCprModal.show();
}

deleteCpr(cpr: CPRDto): void {
  debugger;
  this.message.confirm(
      '',
      (isConfirmed) => {
          if (isConfirmed) {
              this._cprService.delete(cpr.id)
                  .subscribe(() => {
                    debugger;
                      this.reloadPage();
                      this.notify.success(this.l('SuccessfullyDeleted'));
                  });
          }
      }
  );
}

exportToExcel(): void {
    this._cprService.GetCPRToExcel(
      this.filterText,
        this.maxCprIdFilter == null ? this.maxCprIdFilterEmpty: this.maxCprIdFilter,
        this.minCprIdFilter == null ? this.minCprIdFilterEmpty: this.minCprIdFilter,
        this.cprNoFilter,
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
