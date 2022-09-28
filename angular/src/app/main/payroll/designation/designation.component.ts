import { Component, OnInit, ViewEncapsulation, ViewChild, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { DesignationServiceProxy } from '../shared/services/designation-service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DesignationDto } from '../shared/dto/designation-dto';
import { CreateOrEditDesignationModalComponent } from './create-or-edit-designation-modal.component';
import { ViewDesignationModalComponent } from './view-designation-modal.component';

@Component({
  templateUrl: './designation.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class DesignationComponent extends AppComponentBase {
  @ViewChild('createOrEditDesignationModal', { static: true }) createOrEditDesignationModal: CreateOrEditDesignationModalComponent;
  @ViewChild('viewDesignationModal', { static: true }) viewDesignationModal: ViewDesignationModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxDesignationIDFilter : number;
	maxDesignationIDFilterEmpty : number;
	minDesignationIDFilter : number;
	minDesignationIDFilterEmpty : number;
  designationFilter = '';
  activeFilter=-1;
  createdByFilter = '';
  maxCreateDateFilter : moment.Moment;
  minCreateDateFilter : moment.Moment;
  audtUserFilter = '';
  maxAudtDateFilter : moment.Moment;
  minAudtDateFilter : moment.Moment;  

  constructor(
    injector: Injector,
    private _designationService: DesignationServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) { 
    super(injector); 
  }

  getDesignation(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
        this.paginator.changePage(0);
        return;
    }
debugger;
    this.primengTableHelper.showLoadingIndicator();

    this._designationService.getAll(
      this.filterText,
      this.maxDesignationIDFilter == null ? this.maxDesignationIDFilterEmpty : this.maxDesignationIDFilter,
      this.minDesignationIDFilter == null ? this.minDesignationIDFilterEmpty : this.minDesignationIDFilter,
      this.designationFilter,
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

createDesignation(): void {
  this.createOrEditDesignationModal.show();
}

deleteDesignation(designation: DesignationDto): void {
  debugger;
  this.message.confirm(
      '',
      (isConfirmed) => {
          if (isConfirmed) {
              this._designationService.delete(designation.id)
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
    this._designationService.GetDesignationToExcel(
      this.filterText,
        this.maxDesignationIDFilter == null ? this.maxDesignationIDFilterEmpty: this.maxDesignationIDFilter,
        this.minDesignationIDFilter == null ? this.minDesignationIDFilterEmpty: this.minDesignationIDFilter,
        this.designationFilter,
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
