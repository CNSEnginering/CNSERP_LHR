import { Component, OnInit, ViewEncapsulation, ViewChild, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { LocationServiceProxy } from '../shared/services/location-service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LocationDto } from '../shared/dto/location-dto';
import { CreateOrEditLocationModalComponent } from './create-or-edit-location-modal.component';
import { ViewLocationModalComponent } from './view-location-modal.component';

@Component({
  templateUrl: './location.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class LocationComponent extends AppComponentBase {
  @ViewChild('createOrEditLocationModal', { static: true }) createOrEditLocationModal: CreateOrEditLocationModalComponent;
  @ViewChild('viewLocationModal', { static: true }) viewLocationModal: ViewLocationModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxLocIDFilter : number;
	maxLocIDFilterEmpty : number;
	minLocIDFilter : number;
	minLocIDFilterEmpty : number;
  locationFilter = '';
  activeFilter=-1;
  createdByFilter = '';
  maxCreateDateFilter : moment.Moment;
  minCreateDateFilter : moment.Moment;
  audtUserFilter = '';
  maxAudtDateFilter : moment.Moment;
  minAudtDateFilter : moment.Moment;  

  constructor(
    injector: Injector,
    private _locationService: LocationServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) { 
    super(injector); 
  }

  getLocation(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
        this.paginator.changePage(0);
        return;
    }
debugger;
    this.primengTableHelper.showLoadingIndicator();

    this._locationService.getAll(
      this.filterText,
      this.maxLocIDFilter == null ? this.maxLocIDFilterEmpty : this.maxLocIDFilter,
      this.minLocIDFilter == null ? this.minLocIDFilterEmpty : this.minLocIDFilter,
      this.locationFilter,
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

createLocation(): void {
  this.createOrEditLocationModal.show();
}

deleteLocation(location: LocationDto): void {
  this.message.confirm(
      '',
      (isConfirmed) => {
          if (isConfirmed) {
              this._locationService.delete(location.id)
                  .subscribe(() => {
                      this.reloadPage();
                      this.notify.success(this.l('SuccessfullyDeleted'));
                  });
          }
      }
  );
}

exportToExcel(): void {
    this._locationService.GetLocationToExcel(
      this.filterText,
        this.maxLocIDFilter == null ? this.maxLocIDFilterEmpty: this.maxLocIDFilter,
        this.minLocIDFilter == null ? this.minLocIDFilterEmpty: this.minLocIDFilter,
        this.locationFilter,
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
