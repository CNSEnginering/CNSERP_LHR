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
import { CreateOrEditPurchaseOrderModalComponent } from './create-or-edit-purchaseOrder-modal.component';
import { ViewPurchaseOrderModalComponent } from './view-purchaseOrder-modal.component';
import { PurchaseOrderServiceProxy } from '../shared/services/purchaseOrder.service';
import { POPOHeadersService } from '../shared/services/popoHeader.service';
import { POPOHeaderDto } from '../shared/dtos/popoHeader-dto';
import { GetDataService } from '@app/main/supplyChain/inventory/shared/services/get-data.service';
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
@Component({
    templateUrl: './purchaseOrders.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class PurchaseOrdersComponent extends AppComponentBase {

    @ViewChild('createOrEditPurchaseOrderModal', { static: true }) createOrEditPurchaseOrderModal: CreateOrEditPurchaseOrderModalComponent;
    @ViewChild('viewPurchaseOrderModalComponent', { static: true }) viewPurchaseOrderModal: ViewPurchaseOrderModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild("reportviewrModalComponent", { static: false }) reportView: ReportviewrModalComponent;

    advancedFiltersAreShown = false;
    filterText = '';
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
    postedFilter;
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
    fromDoc:number;
    toDoc:number;
    exfromDate:any;
    extoDate:any;
    locations:any;
    users:any;




    constructor(
        injector: Injector,
        private _purchaseOrdersServiceProxy: PurchaseOrderServiceProxy,
        private _popoHeadersServiceProxy: POPOHeadersService,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,private _getDataService: GetDataService
    ) {
        super(injector);
        this._getDataService.getList("ICLocations").subscribe(result => {
          this.locations = result;
        });
        this._getDataService.getList("Users").subscribe(result => {
          this.users = result;
        });
    }

    getPurchaseOrders(event?: LazyLoadEvent) {
      debugger;
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this.maxLocIDFilter=this.minLocIDFilter;
        this._popoHeadersServiceProxy.getAll(
            this.filterText,
            this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty: this.maxDocNoFilter,
            this.minDocNoFilter == null ? this.minDocNoFilterEmpty: this.minDocNoFilter,
            this.maxDocDateFilter,
            this.minDocDateFilter,
            this.maxArrivalDateFilter,
            this.minArrivalDateFilter,
            //this.narrationFilter,
            this.maxLocIDFilter == null ? this.maxLocIDFilterEmpty: this.maxLocIDFilter,
            this.minLocIDFilter == null ? this.minLocIDFilterEmpty: this.minLocIDFilter,
            // this.maxTotalQtyFilter == null ? this.maxTotalQtyFilterEmpty: this.maxTotalQtyFilter,
            // this.minTotalQtyFilter == null ? this.minTotalQtyFilterEmpty: this.minTotalQtyFilter,
            // this.maxTotalAmtFilter == null ? this.maxTotalAmtFilterEmpty: this.maxTotalAmtFilter,
            // this.minTotalAmtFilter == null ? this.minTotalAmtFilterEmpty: this.minTotalAmtFilter,
            this.activeFilter,
            this.postedFilter,
            this.maxLinkDetIDFilter == null ? this.maxLinkDetIDFilterEmpty: this.maxLinkDetIDFilter,
            this.minLinkDetIDFilter == null ? this.minLinkDetIDFilterEmpty: this.minLinkDetIDFilter,
            this.ordNoFilter, 
            
            this.createdByFilter,
            this.maxCreateDateFilter,
            this.minCreateDateFilter,
            this.audtUserFilter,
            this.maxAudtDateFilter,
            this.minAudtDateFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {debugger;
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createPurchaseOrder(): void {
      this.GetSetUpDetail();
      this._purchaseOrdersServiceProxy.getMaxDocId().subscribe(result => {
        debugger; 
        if(result!=0){
            this.maxID=result;
        }
          this.createOrEditPurchaseOrderModal.show(null,this.maxID);
      });
    }
    GetSetUpDetail(): void {
      this._getDataService.GetSetUpDetail().subscribe(result => {
        
          this.createOrEditPurchaseOrderModal.SetDefaultRecord(result);
      });
    }
    deletePurchaseOrder(purchaseOrder: POPOHeaderDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._purchaseOrdersServiceProxy.delete(purchaseOrder.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }
  getReport(po:any) {
    debugger
    let rptParams = "";
    this.fromDoc=po.docNo;
    this.toDoc=po.docNo;
    this.exfromDate=po.arrivalDate;
    this.extoDate=po.arrivalDate;
    rptParams += encodeURIComponent("" + this.fromDoc) + "$";
    rptParams += encodeURIComponent("" + this.toDoc) + "$";
    rptParams += moment(this.exfromDate).format("YYYY/MM/DD") + "$";
    rptParams += moment(this.extoDate).format("YYYY/MM/DD") + "$";
    rptParams = rptParams.replace(/[?$]&/, "");
    this.reportView.show("PurchaseOrder", rptParams);
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
