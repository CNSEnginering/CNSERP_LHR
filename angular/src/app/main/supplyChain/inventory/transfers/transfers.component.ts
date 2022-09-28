import { Component, OnInit, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { Paginator } from 'primeng/components/paginator/paginator';
import { Table } from 'primeng/components/table/table';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import * as moment from 'moment';
import { CreateOrEditTransfersModalComponent } from './create-or-edit-transfers-modal.component';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { TransfersService } from '../shared/services/transfers.service';
import { ViewTransfersComponent } from './view-transfers-modal.component';
import { GetDataService } from '@app/main/supplyChain/inventory/shared/services/get-data.service';
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
import { TransferDto } from '../shared/dto/transfer-dto';
@Component({
  templateUrl: './transfers.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class TransfersComponent extends AppComponentBase implements OnInit {
  filterText = '';
  priceList: any;
  sorting: any;
  skipCount: any;
  MaxResultCount: any;
  listData: any;
  fromDoc:number;
  toDoc:number;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('TransfersModal', { static: true }) TransfersModal: CreateOrEditTransfersModalComponent;
  @ViewChild('viewTransfersModal', { static: true }) viewTransfersModal: ViewTransfersComponent;
  @ViewChild("reportviewrModalComponent", { static: false }) reportView: ReportviewrModalComponent;
  transdto : TransferDto = new TransferDto()

  advancedFiltersAreShown = false;
  maxDocNoFilter : number;
  maxDocNoFilterEmpty : number;
  minDocNoFilter : number;
  minDocNoFilterEmpty : number;
  maxDocDateFilter : moment.Moment;
  minDocDateFilter : moment.Moment;
  maxArrivalDateFilter : moment.Moment;
  minArrivalDateFilter : moment.Moment;
  narrationFilter = '';
  maxLocIDFilter : number;
  maxLocIDFilterEmpty : number;
  minLocIDFilter : number;
  minLocIDFilterEmpty : number;
  maxTotalQtyFilter : number;
  maxTotalQtyFilterEmpty : number;
  minTotalQtyFilter : number;
  minTotalQtyFilterEmpty : number;
  maxTotalAmtFilter : number;
  maxTotalAmtFilterEmpty : number;
  minTotalAmtFilter : number;
  minTotalAmtFilterEmpty : number;
  postedFilter = 0;
  maxLinkDetIDFilter : number;
  maxLinkDetIDFilterEmpty : number;
  minLinkDetIDFilter : number;
  minLinkDetIDFilterEmpty : number;
  ordNoFilter : string;
  minOrdNoFilterEmpty : number;
  activeFilter = -1;
  createdByFilter = '';
  maxCreateDateFilter : moment.Moment;
  minCreateDateFilter : moment.Moment;
  audtUserFilter = '';
  maxAudtDateFilter : moment.Moment;
  minAudtDateFilter : moment.Moment;
  maxID:number;
  locations:any;
  users:any;
  
  constructor(injector: Injector,
    private _transfersService: TransfersService,
    private _fileDownloadService: FileDownloadService,
    private _getDataService: GetDataService
  ) {
    super(injector);
    this._getDataService.getList("Users").subscribe(result => {
      this.users = result;
    });
  }

  ngOnInit() {
  }
  getAll(event?: LazyLoadEvent) {
    this.sorting = this.primengTableHelper.getSorting(this.dataTable);
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event);
    this.MaxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event);

    this.primengTableHelper.showLoadingIndicator();
    this._transfersService.getAll(
      this.filterText,
      this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty: this.maxDocNoFilter,
      this.minDocNoFilter == null ? this.minDocNoFilterEmpty: this.minDocNoFilter,
      this.activeFilter,
      this.postedFilter,
      this.createdByFilter,
      this.audtUserFilter,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe(data => {
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
          this._transfersService.delete(id).subscribe(() => {
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
    this.TransfersModal.show(id, "");
  }
  GetSetUpDetail(): void {
    this._getDataService.GetSetUpDetail().subscribe(result => {
     
        this.TransfersModal.SetDefaultRecord(result);
    });
  }
  view(data: any) {
     this.viewTransfersModal.show(data);
  }
  exportToExcel() {
    this._transfersService.GetDataToExcel(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe((result: any) => {
      result["result"]["docDate"] =  new Date(result["result"]["docDate"]);
      this._fileDownloadService.downloadTempFile(result["result"]);
    });
  }
  getReport(trans:any) {
    debugger
    let rptParams = "";
    this.fromDoc=trans;
    this.toDoc=trans;
    rptParams += encodeURIComponent("" + this.fromDoc) + "$";
    rptParams += encodeURIComponent("" + this.toDoc) + "$";
    rptParams = rptParams.replace(/[?$]&/, "");
    this.reportView.show("StockTransfer", rptParams);
}  
}