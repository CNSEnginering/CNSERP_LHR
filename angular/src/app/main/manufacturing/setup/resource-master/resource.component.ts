import { Component, OnInit, Injector, ViewChild, ViewEncapsulation, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/primeng';
import { LazyLoadEvent } from 'primeng/api';
import { ViewResourceMasterModalComponent } from './view-resource-modal.component';
import { CreateOrEditResourceMasterModalComponent } from './create-or-edit-resource-modal.component';
import { ResourceServiceProxy } from '../../shared/service/resource.service';


@Component({
  templateUrl: './resource.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class ResourceMasterComponent extends AppComponentBase implements OnInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('resourceModal', { static: true }) resourceModal: CreateOrEditResourceMasterModalComponent;
  @ViewChild('viewresourceModal', { static: true }) viewresourceModal: ViewResourceMasterModalComponent;
  advancedFiltersAreShown = false;
  filterText = '';
  dataList: any;
  sorting: any;
  skipCount: any;
  MaxResultCount: any;

  constructor(
    injector: Injector,
    private _fileDownloadService: FileDownloadService,
    private _resourceService: ResourceServiceProxy,
  ) {

    super(injector);
  }

  ngOnInit() {
    // this.getAll(null);
  }
  reloadPage(): void {
    this.getAll();
  }
  getAll(event?: LazyLoadEvent) {
    this.sorting = this.primengTableHelper.getSorting(this.dataTable);
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event);
    this.MaxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event);

    this.primengTableHelper.showLoadingIndicator();
    this._resourceService.getAll(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe(data => {
      this.primengTableHelper.totalRecordsCount = data.totalCount;
      this.primengTableHelper.records = data.items;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }
  delete(id: number) {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._resourceService.delete(id).subscribe(() => {
            this.reloadPage();
            this.notify.success(this.l('SuccessfullyDeleted'));
          });
        }
      }
    );
  }
  createOrEdit(id: number) {
    this.resourceModal.show(id);
  }
  exportToExcel() {
    this._resourceService.GetDataToExcel(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe((result: any) => {
      this._fileDownloadService.downloadTempFile(result["result"]);
    });
  }
  view(data: any) {
    this.viewresourceModal.show(data);
  }
}

