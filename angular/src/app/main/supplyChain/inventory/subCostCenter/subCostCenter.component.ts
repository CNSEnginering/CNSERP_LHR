import { Component, OnInit, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { Paginator } from 'primeng/components/paginator/paginator';
import { Table } from 'primeng/components/table/table';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CreateOrEditSubCostCenterModalComponent } from './create-or-edit-subCostCenter-modal.component';
import { SubCostCenterService } from '../shared/services/subCostCenter.service';
import { ViewSubCostCenterComponent } from './view-subCostCenter-modal.component';
import { FileDownloadService } from '@shared/utils/file-download.service';
@Component({
  templateUrl: './subCostCenter.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class SubCostCenterComponent extends AppComponentBase implements OnInit {
  filterText = '';
  subCostCenter: any;
  sorting: any;
  skipCount: any;
  MaxResultCount: any;

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('subCostCenterModal', { static: true }) subCostCenterModal: CreateOrEditSubCostCenterModalComponent;
  @ViewChild('viewSubCostCenterModal', { static: true }) viewSubCostCenterModal: ViewSubCostCenterComponent;

  constructor(injector: Injector,
    private _subCostCenterService: SubCostCenterService,
    private _fileDownloadService: FileDownloadService
  ) {
    super(injector);
  }

  ngOnInit() {
  }
  getAll(event?: LazyLoadEvent) {
    debugger
    this.sorting = this.primengTableHelper.getSorting(this.dataTable);
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event);
    this.MaxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event);

    this.primengTableHelper.showLoadingIndicator();
    this._subCostCenterService.getAll(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe(data => {
      console.log(data["result"]["items"]);
      this.subCostCenter = data["result"]["items"];
      this.primengTableHelper.totalRecordsCount = data["result"]["totalCount"];
      this.primengTableHelper.records = this.subCostCenter;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }
  delete(id: number) {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._subCostCenterService.delete(id).subscribe(() => {
            this.reloadPage()
            this.notify.success(this.l('SuccessfullyDeleted'))
          });
        }
      }
    );

  }
  reloadPage(): void {
    this.paginator.changePage(this.paginator.getPage());
  }
  createOrEdit(id: number) {
    this.subCostCenterModal.show(id);
  }
  view(data: any) {
    this.viewSubCostCenterModal.show(data);
  }
  exportToExcel() {
    this._subCostCenterService.GetDataToExcel(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe((result: any) => {
      this._fileDownloadService.downloadTempFile(result["result"]);
    });
  }
}
