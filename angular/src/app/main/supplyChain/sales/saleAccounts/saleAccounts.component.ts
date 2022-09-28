import { Component, OnInit, Injector, ViewChild, ViewEncapsulation, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/primeng';
import { LazyLoadEvent } from 'primeng/api';
import { SaleAccountsService } from '../shared/services/saleAccounts.service';
import { CreateOrEditSaleAccountsModalComponent } from './create-or-edit-SaleAccounts-modal.component';
import { ViewSaleAccountsComponent } from './view-saleAccounts-modal.component';
import { GetDataService } from '@app/main/supplyChain/inventory/shared/services/get-data.service';


@Component({
  templateUrl: './saleAccounts.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class SaleAccountscomponent extends AppComponentBase implements OnInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('SaleAccountsModal', { static: true }) saleAccountsModal: CreateOrEditSaleAccountsModalComponent;
  @ViewChild('viewSaleAccountsModal', { static: true }) viewSaleAccountsModal: ViewSaleAccountsComponent;
  advancedFiltersAreShown = false;
  filterText = '';
  inventoryGlLink: any;
  sorting: any;
  skipCount: any;
  MaxResultCount: any;

  constructor(
    injector: Injector,
    private _fileDownloadService: FileDownloadService,
    private _saleAccountsService: SaleAccountsService, private _getDataService: GetDataService
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
    this._saleAccountsService.getAll(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe(data => {
      this.inventoryGlLink = data["result"]["items"]
      console.log(this.inventoryGlLink);
      this.primengTableHelper.totalRecordsCount = this.inventoryGlLink.length;
      this.primengTableHelper.records = data["result"]["items"];
      this.primengTableHelper.hideLoadingIndicator();
    });
  }
  delete(id: number) {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._saleAccountsService.delete(id).subscribe(() => {
            this.reloadPage();
            this.notify.success(this.l('SuccessfullyDeleted'));
          });
        }
      }
    );
  }
  createOrEdit(id: number) {
    this.GetSetUpDetail();
    this.saleAccountsModal.show(id);
  }
  GetSetUpDetail(): void {
    this._getDataService.GetSetUpDetail().subscribe(result => {
   
        this.saleAccountsModal.SetDefaultRecord(result);
    });
  }
  exportToExcel() {
    this._saleAccountsService.GetDataToExcel(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe((result: any) => {
      this._fileDownloadService.downloadTempFile(result["result"]);
    });
  }
  view(data: any) {
    this.viewSaleAccountsModal.show(data);
   }
}

