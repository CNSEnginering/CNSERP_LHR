import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { CreateOrEditSaleEntryModalComponent } from './create-or-edit-saleEntry-modal.component';
import { ViewSaleEntryModalComponent } from './view-saleEntry-modal.component';
import { SaleEntryServiceProxy } from '../shared/services/saleEntry.service';
import { OESALEHeadersService } from '../shared/services/oesaleHeader.service';
import { OESALEHeaderDto } from '../shared/dtos/oesaleHeader-dto';
import { GetDataService } from '@app/main/supplyChain/inventory/shared/services/get-data.service';
import { ICSetupsService } from '../../inventory/shared/services/ic-setup.service';
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
import { SaleEntryDto } from '../shared/dtos/saleEntry-dto';
import { FileUpload } from 'primeng/primeng';
import { HttpClient } from '@angular/common/http';
import { AppConsts } from '@shared/AppConsts';
import { finalize } from 'rxjs/internal/operators/finalize';

@Component({
  templateUrl: './saleEntry.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class SaleEntryComponent extends AppComponentBase {

  @ViewChild('createOrEditSaleEntryModal', { static: true }) createOrEditSaleEntryModal: CreateOrEditSaleEntryModalComponent;
  @ViewChild('viewSaleEntryModelComponent', { static: true }) viewSaleEntryModel: ViewSaleEntryModalComponent;
  @ViewChild("reportviewrModalComponent", { static: false }) reportView: ReportviewrModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild("ExcelFileUpload", { static: true }) excelFileUpload: FileUpload;

  advancedFiltersAreShown = false;
  filterText = '';
  maxDocNoFilter: number;
  maxDocNoFilterEmpty: number;
  minDocNoFilter: number;
  minDocNoFilterEmpty: number;
  maxDocDateFilter: moment.Moment;
  minDocDateFilter: moment.Moment;
  maxArrivalDateFilter: moment.Moment;
  minArrivalDateFilter: moment.Moment;
  narrationFilter = '';
  maxLocIDFilter: number;
  maxLocIDFilterEmpty: number;
  minLocIDFilter: number;
  minLocIDFilterEmpty: number;
  maxTotalQtyFilter: number;
  maxTotalQtyFilterEmpty: number;
  minTotalQtyFilter: number;
  minTotalQtyFilterEmpty: number;
  maxTotalAmtFilter: number;
  maxTotalAmtFilterEmpty: number;
  minTotalAmtFilter: number;
  minTotalAmtFilterEmpty: number;
  postedFilter = -1;
  maxLinkDetIDFilter: number;
  maxLinkDetIDFilterEmpty: number;
  minLinkDetIDFilter: number;
  minLinkDetIDFilterEmpty: number;
  ordNoFilter: string;
  minOrdNoFilterEmpty: number;
  activeFilter = -1;
  createdByFilter = '';
  maxCreateDateFilter: moment.Moment;
  minCreateDateFilter: moment.Moment;
  audtUserFilter = '';
  maxAudtDateFilter: moment.Moment;
  minAudtDateFilter: moment.Moment;
  maxID: number;
  fromDoc:number;
  toDoc:number;
  uploadUrl: string;



  constructor(
    injector: Injector,
    private _saleEntryServiceProxy: SaleEntryServiceProxy,
    private _oesaleHeadersServiceProxy: OESALEHeadersService,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService,
    private _getDataService: GetDataService,
    private _icSetupsService: ICSetupsService,
    private _httpClient:HttpClient,
  ) {
    super(injector);
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + "/SaleEntry/ImportFromExcel";
  }

  getSaleEntry(event?: LazyLoadEvent) {
    debugger;
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();

    this._oesaleHeadersServiceProxy.getAll(
      this.filterText,
      this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty : this.maxDocNoFilter,
      this.minDocNoFilter == null ? this.minDocNoFilterEmpty : this.minDocNoFilter,
      this.maxDocDateFilter,
      this.minDocDateFilter,
      this.maxArrivalDateFilter,
      this.minArrivalDateFilter,
      //this.narrationFilter,
      this.maxLocIDFilter == null ? this.maxLocIDFilterEmpty : this.maxLocIDFilter,
      this.minLocIDFilter == null ? this.minLocIDFilterEmpty : this.minLocIDFilter,
      // this.maxTotalQtyFilter == null ? this.maxTotalQtyFilterEmpty: this.maxTotalQtyFilter,
      // this.minTotalQtyFilter == null ? this.minTotalQtyFilterEmpty: this.minTotalQtyFilter,
      // this.maxTotalAmtFilter == null ? this.maxTotalAmtFilterEmpty: this.maxTotalAmtFilter,
      // this.minTotalAmtFilter == null ? this.minTotalAmtFilterEmpty: this.minTotalAmtFilter,
      this.postedFilter,
      this.maxLinkDetIDFilter == null ? this.maxLinkDetIDFilterEmpty : this.maxLinkDetIDFilter,
      this.minLinkDetIDFilter == null ? this.minLinkDetIDFilterEmpty : this.minLinkDetIDFilter,
      this.ordNoFilter,
      //this.activeFilter,
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

  createSaleEntry(): void {
   
    this._saleEntryServiceProxy.getMaxDocId().subscribe(result => {
      debugger;
      if (result != 0) {
        this.maxID = result;
      }
      this.GetSetUpDetail();
      this.createOrEditSaleEntryModal.show(null, this.maxID);
    });
  }

  GetSetUpDetail(): void {
   
    this._getDataService.GetSetUpDetail().subscribe(result => {
      debugger; 
      if(result!=null){
         // this.=result;

      }
        this.createOrEditSaleEntryModal.SetDefaultRecord(result);
    });
  }
  deleteSaleEntry(saleEntry: OESALEHeaderDto): void {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._saleEntryServiceProxy.delete(saleEntry.docNo)
            .subscribe(() => {
              this.reloadPage();
              this.notify.success(this.l('SuccessfullyDeleted'));
            });
        }
      }
    );
  }
  
  uploadedFiles: any[] = [];
  onUpload(event): void {
      for (const file of event.files) {
          this.uploadedFiles.push(file);
      }
  }

  uploadExcel(data: { files: File }): void {
      const formData: FormData = new FormData();
      const file = data.files[0];
      formData.append("file", file, file.name);
      abp.ui.setBusy(undefined, "", 1);
      this._httpClient
          .post<any>(this.uploadUrl, formData)
          .pipe(finalize(() => this.excelFileUpload.clear()))
          .subscribe((response) => {
              debugger;
              if (response["error"]["message"] == "")
                 { this.notify.success(
                      this.l("AllSaleEntrySuccessfullyImportedFromExcel")
                  );}
              else {this.message.error(response["error"]["message"]);}

              abp.ui.clearBusy();
          });
  }

  onUploadExcelError(): void {
      this.notify.error(this.l("ImportSaleEntryUploadFailed"));
  }

  getReport(saledto: OESALEHeaderDto) {
    debugger
    let rptParams = "";
    rptParams += "" + moment(saledto.docDate).format("YYYY/MM/DD") + "$";
    rptParams += "" + moment(saledto.docDate).format("YYYY/MM/DD") + "$";
    rptParams += encodeURIComponent("" + saledto.docNo) + "$";
    rptParams += encodeURIComponent("" + saledto.docNo) + "$";
    
    rptParams = rptParams.replace(/[?$]&/, "");
    this.reportView.show("Invoice", rptParams);
}
}
