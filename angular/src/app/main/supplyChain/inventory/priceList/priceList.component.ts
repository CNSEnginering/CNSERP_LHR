import { Component, OnInit, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PriceListService } from '../shared/services/priceList.service';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { Paginator } from 'primeng/components/paginator/paginator';
import { Table } from 'primeng/components/table/table';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CreateOrEditPriceListNewModalComponent } from './create-or-edit-priceListNew-modal.component';
import { id } from '@swimlane/ngx-charts/release/utils';
import { ViewPriceListComponent } from './view-priceList-modal.component';
import { FileDownloadService } from '@shared/utils/file-download.service';
@Component({
  // selector: 'priceList',
  templateUrl: './priceList.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class PriceListComponent extends AppComponentBase implements OnInit {
  filterText = '';
  priceList: any;
  sorting: any;
  skipCount: any;
  MaxResultCount: any;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('PriceListNewModal', { static: true }) PriceListNewModal: CreateOrEditPriceListNewModalComponent;
  @ViewChild('viewPriceListModal', { static: true }) viewPriceListModal: ViewPriceListComponent;
  constructor(injector: Injector,
    private _priceListervice: PriceListService,
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
    this._priceListervice.getAll(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe(data => {
      this.priceList = data["result"]["items"]
      this.primengTableHelper.totalRecordsCount = data["result"]["totalCount"];
      this.primengTableHelper.records = this.priceList;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }
  delete(id: number) {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._priceListervice.delete(id).subscribe(() => {
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
    this.PriceListNewModal.show(id);
  }
  view(data: any) {
    debugger
    this.viewPriceListModal.show(data);
  }
  exportToExcel() {
    this._priceListervice.GetDataToExcel(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe((result: any) => {
      this._fileDownloadService.downloadTempFile(result["result"]);
    });
  }

}
