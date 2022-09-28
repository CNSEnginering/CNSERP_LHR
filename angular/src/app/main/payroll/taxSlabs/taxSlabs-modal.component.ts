import { Component, ViewEncapsulation, ViewChild, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditTaxSlabsModalComponent } from './create-or-edit-taxSlabs-modal.component';
import { ViewTaxSlabsModalComponent } from './view-taxSlabs-modal.component';
import { SlabSetupService } from '../shared/services/slabSetup.service';

@Component({
  templateUrl: './taxSlabs-modal.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class TaxSlabsComponent extends AppComponentBase {
  @ViewChild('createOrEditTaxSlabsModal', { static: true }) createOrEditTaxSlabsModal: CreateOrEditTaxSlabsModalComponent;
  @ViewChild('viewTaxSlabsModal', { static: true }) viewTaxSlabsModal: ViewTaxSlabsModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxReligionIDFilter: number;
  maxReligionIDFilterEmpty: number;
  minReligionIDFilter: number;
  minReligionIDFilterEmpty: number;
  religionFilter = '';
  activeFilter = -1;
  createdByFilter = '';
  maxCreateDateFilter: moment.Moment;
  minCreateDateFilter: moment.Moment;
  audtUserFilter = '';
  maxAudtDateFilter: moment.Moment;
  minAudtDateFilter: moment.Moment;

  constructor(
    injector: Injector,
    private _slabSetupService: SlabSetupService,
    private _fileDownloadService: FileDownloadService
  ) {
    super(injector);
  }

  getSlabSetup(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }
    debugger;
    this.primengTableHelper.showLoadingIndicator();

    this._slabSetupService.getAll(
      this.filterText,
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

  create(): void {
    this.createOrEditTaxSlabsModal.show();
  }

  // deleteReligion(Religion: ReligionDto): void {
  //   this.message.confirm(
  //       '',
  //       (isConfirmed) => {
  //           if (isConfirmed) {
  //               this._slabSetupService.getall(
  //                 .delete(Religion.id)
  //                   .subscribe(() => {
  //                       this.reloadPage();
  //                       this.notify.success(this.l('SuccessfullyDeleted'));
  //                   });
  //           }
  //       }
  //   );
  // }

  exportToExcel(): void {
    this._slabSetupService.GetDataToExcel(
      this.filterText,
      undefined,
      undefined,
      undefined
    ).subscribe((result: any) => {
      this._fileDownloadService.downloadTempFile(result["result"]);
    });
  }

}
