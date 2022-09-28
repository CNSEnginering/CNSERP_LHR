import { Component, OnInit, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { Paginator } from 'primeng/components/paginator/paginator';
import { Table } from 'primeng/components/table/table';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CreateOrEditCostCenterModalComponent } from './create-or-edit-costCenter-modal.component';
import { ItemPricingService } from '../shared/services/itemPricing.service';
import { CostCenterService } from '../shared/services/costCenter.service';
import { ViewCostCenterComponent } from './view-costCenter-modal.component';
import { FileDownloadService } from '@shared/utils/file-download.service';
@Component({
  templateUrl: './costCenter.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
 export class CostCenterComponent extends AppComponentBase implements OnInit {
  filterText = '';
  costCenter: any;
  sorting: any;
  skipCount: any;
  MaxResultCount: any;

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('costCenterModal', { static: true }) costCenterModal: CreateOrEditCostCenterModalComponent;
  @ViewChild('viewCostCenterModal', { static: true }) viewCostCenterModal: ViewCostCenterComponent;
  constructor(injector: Injector,
    private _costCenterService: CostCenterService,
    private _fileDownloadService: FileDownloadService
  ) {
    super(injector);
  }

  ngOnInit() {
  }
  getAll(event?: LazyLoadEvent) {
    this.sorting = this.primengTableHelper.getSorting(this.dataTable);
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event);
    this.MaxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event);

    this.primengTableHelper.showLoadingIndicator();
    this._costCenterService.getAll(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount,
      undefined
    ).subscribe(data => {
      this.costCenter = data["result"]["items"];
      this.primengTableHelper.totalRecordsCount = data["result"]["totalCount"];
      this.primengTableHelper.records = this.costCenter;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }
  delete(id: number) {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._costCenterService.delete(id).subscribe(() => {
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
    this.costCenterModal.show(id);
  }
  view(data: any) {
    this.viewCostCenterModal.show(data);
  }
  exportToExcel() {
    this._costCenterService.GetDataToExcel(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe((result: any) => {
      this._fileDownloadService.downloadTempFile(result["result"]);
    });
  }
}
