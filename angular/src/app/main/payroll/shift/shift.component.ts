import { Component, OnInit, ViewEncapsulation, ViewChild, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ShiftServiceProxy } from '../shared/services/shift-service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ShiftDto } from '../shared/dto/shift-dto';
import { CreateOrEditShiftModalComponent } from './create-or-edit-shift-modal.component'; 
import { ViewShiftModalComponent } from './view-shift-modal.component';

@Component({
  templateUrl: './shift.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class ShiftComponent extends AppComponentBase {
  @ViewChild('createOrEditShiftModal', { static: true }) createOrEditShiftModal: CreateOrEditShiftModalComponent;
  @ViewChild('viewShiftModal', { static: true }) viewShiftModal: ViewShiftModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxShiftIDFilter : number;
	maxShiftIDFilterEmpty : number;
	minShiftIDFilter : number;
	minShiftIDFilterEmpty : number;
  shiftNameFilter = '';
  maxStartTimeFilter : moment.Moment;
	minStartTimeFilter : moment.Moment;
  maxEndTimeFilter : moment.Moment;
	minEndTimeFilter : moment.Moment;
  maxBeforeStartFilter : number;
	maxBeforeStartFilterEmpty : number;
	minBeforeStartFilter : number;
  minBeforeStartFilterEmpty : number;
  maxAfterStartFilter : number;
	maxAfterStartFilterEmpty : number;
	minAfterStartFilter : number;
  minAfterStartFilterEmpty : number;
  maxBeforeFinishFilter : number;
	maxBeforeFinishFilterEmpty : number;
	minBeforeFinishFilter : number;
  minBeforeFinishFilterEmpty : number;
  maxAfterFinishFilter : number;
	maxAfterFinishFilterEmpty : number;
	minAfterFinishFilter : number;
  minAfterFinishFilterEmpty : number;
  maxTotalHourFilter : number;
	maxTotalHourFilterEmpty : number;
	minTotalHourFilter : number;
	minTotalHourFilterEmpty : number;
  activeFilter = -1;
  createdByFilter = '';
  maxCreateDateFilter : moment.Moment;
  minCreateDateFilter : moment.Moment;
  audtUserFilter = '';
  maxAudtDateFilter : moment.Moment;
  minAudtDateFilter : moment.Moment;  

  constructor(
    injector: Injector,
    private _shiftService: ShiftServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) { 
    super(injector); 
  }

  getShift(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
        this.paginator.changePage(0);
        return;
    }
debugger;
    this.primengTableHelper.showLoadingIndicator();

    this._shiftService.getAll(
        this.filterText,
        this.maxShiftIDFilter == null ? this.maxShiftIDFilterEmpty: this.maxShiftIDFilter,
        this.minShiftIDFilter == null ? this.minShiftIDFilterEmpty: this.minShiftIDFilter,
        this.shiftNameFilter,
        this.maxStartTimeFilter,
        this.minStartTimeFilter,
        this.maxEndTimeFilter,        
        this.minEndTimeFilter,        
        this.maxBeforeStartFilter == null ? this.maxBeforeStartFilterEmpty: this.maxBeforeStartFilter,   
        this.minBeforeStartFilter == null ? this.minBeforeStartFilterEmpty: this.minBeforeStartFilter,    
        this.maxAfterStartFilter == null ? this.maxAfterStartFilterEmpty: this.maxAfterStartFilter,      
        this.minAfterStartFilter == null ? this.minAfterStartFilterEmpty: this.minAfterStartFilter,
        this.maxBeforeFinishFilter == null ? this.maxBeforeFinishFilterEmpty: this.maxBeforeFinishFilter,       
        this.minBeforeFinishFilter == null ? this.minBeforeFinishFilterEmpty: this.minBeforeFinishFilter,        
        this.maxAfterFinishFilter == null ? this.maxAfterFinishFilterEmpty: this.maxAfterFinishFilter,         
        this.minAfterFinishFilter == null ? this.minAfterFinishFilterEmpty: this.minAfterFinishFilter,       
        this.maxTotalHourFilter == null ? this.maxTotalHourFilterEmpty: this.maxTotalHourFilter,        
        this.minTotalHourFilter == null ? this.minTotalHourFilterEmpty: this.minTotalHourFilter,        
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

createShift(): void {
  this.createOrEditShiftModal.show();
}

deleteShift(shift: ShiftDto): void {
  this.message.confirm(
      '',
      (isConfirmed) => {
          if (isConfirmed) {
              this._shiftService.delete(shift.id)
                  .subscribe(() => {
                      this.reloadPage();
                      this.notify.success(this.l('SuccessfullyDeleted'));
                  });
          }
      }
  );
}

exportToExcel(): void {
    this._shiftService.GetShiftToExcel(
      this.filterText,
        this.maxShiftIDFilter == null ? this.maxShiftIDFilterEmpty: this.maxShiftIDFilter,
        this.minShiftIDFilter == null ? this.minShiftIDFilterEmpty: this.minShiftIDFilter,
        this.shiftNameFilter,
        this.maxStartTimeFilter,
        this.minStartTimeFilter,
        this.maxEndTimeFilter,        
        this.minEndTimeFilter,        
        this.maxBeforeStartFilter == null ? this.maxBeforeStartFilterEmpty: this.maxBeforeStartFilter,   
        this.minBeforeStartFilter == null ? this.minBeforeStartFilterEmpty: this.minBeforeStartFilter,    
        this.maxAfterStartFilter == null ? this.maxAfterStartFilterEmpty: this.maxAfterStartFilter,      
        this.minAfterStartFilter == null ? this.minAfterStartFilterEmpty: this.minAfterStartFilter,
        this.maxBeforeFinishFilter == null ? this.maxBeforeFinishFilterEmpty: this.maxBeforeFinishFilter,       
        this.minBeforeFinishFilter == null ? this.minBeforeFinishFilterEmpty: this.minBeforeFinishFilter,        
        this.maxAfterFinishFilter == null ? this.maxAfterFinishFilterEmpty: this.maxAfterFinishFilter,         
        this.minAfterFinishFilter == null ? this.minAfterFinishFilterEmpty: this.minAfterFinishFilter,       
        this.maxTotalHourFilter == null ? this.maxTotalHourFilterEmpty: this.maxTotalHourFilter,        
        this.minTotalHourFilter == null ? this.minTotalHourFilterEmpty: this.minTotalHourFilter,        
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
