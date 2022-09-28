import { Component, OnInit, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { Paginator } from 'primeng/components/paginator/paginator';
import { Table } from 'primeng/components/table/table';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CreateOrEditCostSheetComponent } from './create-or-edit-costSheet.component';
import { FileDownloadService } from '@shared/utils/file-download.service';
//import { ViewAssemblyComponent } from './view-assembly-modal.component';
import { saleQutationService } from '../shared/services/saleQutation.service';
import { GetDataService } from '@app/main/supplyChain/inventory/shared/services/get-data.service';
import { costSheetService } from '../shared/services/costSheet.service';

@Component({

  templateUrl: './costSheet.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class CostSheetComponent extends AppComponentBase implements OnInit {

  filterText = '';
  priceList: any;
  sorting: any;
  skipCount: any;
  MaxResultCount: any;
  listData: any;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('CreateOrEditSaleQutationModal', { static: true }) GetSaleQutationModal: CreateOrEditCostSheetComponent;
  //@ViewChild('viewAssemblyModal', { static: true }) viewAssemblyModal: ViewAssemblyComponent;
  constructor(injector: Injector,
    private _assemblyService: costSheetService,
    private _fileDownloadService: FileDownloadService,
    private _getDataService: GetDataService
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
    this._assemblyService.getAll(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe(data => {
      debugger
      console.log(data["result"]["items"]);
      this.listData = data["result"]["items"];
      this.primengTableHelper.totalRecordsCount = data["result"]["totalCount"];
      this.primengTableHelper.records = this.listData;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }
  delete(id: number) {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._assemblyService.delete(id).subscribe(() => {
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
    this.GetSetUpDetail();
    this.GetSaleQutationModal.show(id);
  }
  GetSetUpDetail(): void {
    this._getDataService.GetSetUpDetail().subscribe(result => {
     
        this.GetSaleQutationModal.SetDefaultRecord(result);
    });
  }
  view(data: any) {
     //this.viewAssemblyModal.show(data);
  }
  exportToExcel() {
    this._assemblyService.GetDataToExcel(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe((result: any) => {
      result["result"]["docDate"] =  new Date(result["result"]["docDate"]);
      this._fileDownloadService.downloadTempFile(result["result"]);
    });
  }

}
