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
import { ApinvHServiceProxy } from '../shared/services/apinvH.service';
import * as moment from 'moment';
import { CreateOrEditApinvhComponent } from './create-or-edit-apinvh.component';
import { ViewApinvhComponent } from './view-apinvh.component';
import { PurchaseOrderServiceProxy } from '../shared/services/purchaseOrder.service';
import { POPOHeadersService } from '../shared/services/popoHeader.service';
import { POPOHeaderDto } from '../shared/dtos/popoHeader-dto';
import { GetDataService } from '@app/main/supplyChain/inventory/shared/services/get-data.service';
import { apinvDTo } from '../shared/dtos/apiInv-dto';

@Component({
  selector: 'app-apinvh',
  templateUrl: './apinvh.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class ApinvhComponent extends AppComponentBase{

  @ViewChild('createOrEditINVHModal', { static: true }) createOrEditINVHModal: CreateOrEditApinvhComponent;
  @ViewChild('viewApinvHComponent', { static: true }) viewINVHModal: ViewApinvhComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxDocNoFilter : string;
  maxDocNoFilterEmpty : string;
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
  postedFilter = -1;
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




  constructor(
      injector: Injector,
      private _purchaseOrdersServiceProxy: PurchaseOrderServiceProxy,
      private _popoHeadersServiceProxy: POPOHeadersService,
      private _apinvService : ApinvHServiceProxy,
      private _notifyService: NotifyService,
      private _tokenAuth: TokenAuthServiceProxy,
      private _activatedRoute: ActivatedRoute,
      private _fileDownloadService: FileDownloadService,private _getDataService: GetDataService
  ) {
      super(injector);
  }

  getApinvH(event?: LazyLoadEvent) {
    debugger;
      if (this.primengTableHelper.shouldResetPaging(event)) {
          this.paginator.changePage(0);
          return;
      }

      this.primengTableHelper.showLoadingIndicator();

      this._apinvService.getAll(
          this.filterText,
          this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty: this.maxDocNoFilter,
          this.minDocNoFilter == null ? this.minDocNoFilterEmpty: this.minDocNoFilter,
          this.maxID,
      ).subscribe(result => {debugger;
          this.primengTableHelper.totalRecordsCount = result.totalCount;
          this.primengTableHelper.records = result.items;
          
          this.primengTableHelper.hideLoadingIndicator();
      });
  }

  reloadPage(): void {
      this.paginator.changePage(this.paginator.getPage());
  }

  createApinvH(): void {
    this.GetSetUpDetail();
    this._apinvService.getMaxDocId().subscribe(result => {
      debugger; 
      
      this.maxID=result;
        this.createOrEditINVHModal.show(this.maxID,null);
        //this.createOrEditINVHModal.show();
    });
    debugger
    //this.createOrEditINVHModal.show(null,this.maxID,null);
  }
  GetSetUpDetail(): void {
    this._getDataService.GetSetUpDetail().subscribe(result => {
      
        this.createOrEditINVHModal.SetDefaultRecord(result);
    });
  }
  show(data:any){

  }
  deletePurchaseOrder(purchaseOrder: any): void {
      debugger
      this.message.confirm(
          '',
          this.l('AreYouSure'),
          (isConfirmed) => {
              if (isConfirmed) {
                  this._apinvService.delete(purchaseOrder.apinvh.id)
                      .subscribe(() => {
                          this.reloadPage();
                          this.notify.success(this.l('SuccessfullyDeleted'));
                      });
              }
          }
      );
  }

  // exportToExcel(): void {
  //     this._popoHeadersServiceProxy.getPOPOHeaderToExcel(
  //     this.filterText,
  //         this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty: this.maxDocNoFilter,
  //         this.minDocNoFilter == null ? this.minDocNoFilterEmpty: this.minDocNoFilter,
  //         this.maxDocDateFilter,
  //         this.minDocDateFilter,
  //         this.narrationFilter,
  //         this.maxLocIDFilter == null ? this.maxLocIDFilterEmpty: this.maxLocIDFilter,
  //         this.minLocIDFilter == null ? this.minLocIDFilterEmpty: this.minLocIDFilter,
  //         this.maxTotalQtyFilter == null ? this.maxTotalQtyFilterEmpty: this.maxTotalQtyFilter,
  //         this.minTotalQtyFilter == null ? this.minTotalQtyFilterEmpty: this.minTotalQtyFilter,
  //         this.maxTotalAmtFilter == null ? this.maxTotalAmtFilterEmpty: this.maxTotalAmtFilter,
  //         this.minTotalAmtFilter == null ? this.minTotalAmtFilterEmpty: this.minTotalAmtFilter,
  //         this.postedFilter,
  //         this.maxLinkDetIDFilter == null ? this.maxLinkDetIDFilterEmpty: this.maxLinkDetIDFilter,
  //         this.minLinkDetIDFilter == null ? this.minLinkDetIDFilterEmpty: this.minLinkDetIDFilter,
  //         this.maxOrdNoFilter == null ? this.maxOrdNoFilterEmpty: this.maxOrdNoFilter,
  //         this.minOrdNoFilter == null ? this.minOrdNoFilterEmpty: this.minOrdNoFilter,
  //         this.activeFilter,
  //         this.createdByFilter,
  //         this.maxCreateDateFilter,
  //         this.minCreateDateFilter,
  //         this.audtUserFilter,
  //         this.maxAudtDateFilter,
  //         this.minAudtDateFilter,
  //     )
  //     .subscribe(result => {
  //         this._fileDownloadService.downloadTempFile(result);
  //      });
  // }
}
