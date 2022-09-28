import { Component, OnInit, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { Paginator } from 'primeng/components/paginator/paginator';
import { Table } from 'primeng/components/table/table';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CreateoreditworkingcenterComponent } from './createoreditworkingcenter.component';
import { FileDownloadService } from '@shared/utils/file-download.service';

// import { ViewTransfersComponent } from './view-transfers-modal.component';
import { GetDataService } from '@app/main/supplyChain/inventory/shared/services/get-data.service';
import { mfworkingCenterService } from '../../shared/service/mfworkingCenter.service';
@Component({
  selector: 'app-mf-working-center',
  templateUrl: './mf-working-center.component.html'

})
export class MfWorkingCenterComponent  extends AppComponentBase  implements OnInit {

  filterText = '';
  priceList: any;
  sorting: any;
  skipCount: any;
  MaxResultCount: any;
  listData: any;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('mfWorkingCenterModal', { static: true }) mfWorkingCenterModal: CreateoreditworkingcenterComponent;
  // @ViewChild('TransfersModal', { static: true }) TransfersModal: CreateOrEditTransfersModalComponent;
  // @ViewChild('viewTransfersModal', { static: true }) viewTransfersModal: ViewTransfersComponent;
  constructor(injector: Injector,
     private _mfwcmService: mfworkingCenterService,
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
    this._mfwcmService.getAll(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe(data => {
      this.listData = data["result"]["items"];
      
      this.primengTableHelper.totalRecordsCount = data["result"]["totalCount"];
      this.primengTableHelper.records = this.listData;
      this.primengTableHelper.hideLoadingIndicator();
     debugger
    });
  }
  delete(id: number) {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._mfwcmService.delete(id).subscribe(() => {
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
    this.mfWorkingCenterModal.show(id, "");
  }
  GetSetUpDetail(): void {
    this._getDataService.GetSetUpDetail().subscribe(result => {
     
        this.mfWorkingCenterModal.SetDefaultRecord(result);
    });
  }
  view(data: any) {
     this.mfWorkingCenterModal.show(data);
  }
  exportToExcel() {
    this._mfwcmService.GetDataToExcel(
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

