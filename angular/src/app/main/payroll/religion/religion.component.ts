import { Component, OnInit, ViewEncapsulation, ViewChild, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ReligionServiceProxy } from '../shared/services/Religion-service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ReligionDto } from '../shared/dto/religion-dto';
import { CreateOrEditReligionModalComponent } from './create-or-edit-religion-modal.component';
import { ViewReligionModalComponent } from './view-religion-modal.component';

@Component({
  templateUrl: './religion.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class ReligionComponent extends AppComponentBase {
  @ViewChild('createOrEditReligionModal', { static: true }) createOrEditReligionModal: CreateOrEditReligionModalComponent;
  @ViewChild('viewReligionModal', { static: true }) viewReligionModal: ViewReligionModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxReligionIDFilter : number;
	maxReligionIDFilterEmpty : number;
	minReligionIDFilter : number;
	minReligionIDFilterEmpty : number;
  religionFilter = '';
  activeFilter=-1;
  createdByFilter = '';
  maxCreateDateFilter : moment.Moment;
  minCreateDateFilter : moment.Moment;
  audtUserFilter = '';
  maxAudtDateFilter : moment.Moment;
  minAudtDateFilter : moment.Moment;

  constructor(
    injector: Injector,
    private _religionService: ReligionServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) {
    super(injector);
  }

  getReligion(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
        this.paginator.changePage(0);
        return;
    }
debugger;
    this.primengTableHelper.showLoadingIndicator();

    this._religionService.getAll(
      this.filterText,
      this.maxReligionIDFilter == null ? this.maxReligionIDFilterEmpty : this.maxReligionIDFilter,
      this.minReligionIDFilter == null ? this.minReligionIDFilterEmpty : this.minReligionIDFilter,
      this.religionFilter,
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

createReligion(): void {
  this.createOrEditReligionModal.show();
}

deleteReligion(Religion: ReligionDto): void {
  this.message.confirm(
      '',
      (isConfirmed) => {
          if (isConfirmed) {
              this._religionService.delete(Religion.id)
                  .subscribe(() => {
                      this.reloadPage();
                      this.notify.success(this.l('SuccessfullyDeleted'));
                  });
          }
      }
  );
}

exportToExcel(): void {
    this._religionService.GetReligionToExcel(
      this.filterText,
        this.maxReligionIDFilter == null ? this.maxReligionIDFilterEmpty: this.maxReligionIDFilter,
        this.minReligionIDFilter == null ? this.minReligionIDFilterEmpty: this.minReligionIDFilter,
        this.religionFilter,
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
