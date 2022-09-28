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
import { CreateOrEditReceiptReturnModalComponent } from './create-or-edit-receiptReturn-modal.component';
import { ViewReceiptReturnModalComponent } from './view-receiptReturn-modal.component';
import { ReceiptReturnServiceProxy } from '../shared/services/receiptReturn.service';
import { PORETHeadersService } from '../shared/services/poretHeader.service';
import { PORETHeaderDto } from '../shared/dtos/poretHeader-dto';
import { GetDataService } from '@app/main/supplyChain/inventory/shared/services/get-data.service';
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
@Component({
    templateUrl: './receiptReturn.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ReceiptReturnComponent extends AppComponentBase {

    @ViewChild('createOrEditReceiptReturnModal', { static: true }) createOrEditReceiptReturnModal: CreateOrEditReceiptReturnModalComponent;
    @ViewChild('viewReceiptReturnModelComponent', { static: true }) viewReceiptReturnModel: ViewReceiptReturnModalComponent;
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
    fromDoc:number;
    toDoc:number;
    locations:any;
    users:any;



    constructor(
        injector: Injector,
        private _receiptReturnServiceProxy: ReceiptReturnServiceProxy,
        private _poretHeadersServiceProxy: PORETHeadersService,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService, private _getDataService: GetDataService
    ) {
        super(injector);
        this._getDataService.getList("ICLocations").subscribe(result => {
          this.locations = result;
        });
        this._getDataService.getList("Users").subscribe(result => {
          this.users = result;
        });
    }

    getReceiptReturn(event?: LazyLoadEvent) {
      debugger;
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._poretHeadersServiceProxy.getAll(
            this.filterText,
            this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty: this.maxDocNoFilter,
            this.minDocNoFilter == null ? this.minDocNoFilterEmpty: this.minDocNoFilter,
            // this.maxDocDateFilter,
            // this.minDocDateFilter,
            // this.maxArrivalDateFilter,
            // this.minArrivalDateFilter,
            //this.narrationFilter,
            this.minLocIDFilter == null ? this.maxLocIDFilterEmpty: this.minLocIDFilter,
            this.minLocIDFilter == null ? this.minLocIDFilterEmpty: this.minLocIDFilter,
            // this.maxTotalQtyFilter == null ? this.maxTotalQtyFilterEmpty: this.maxTotalQtyFilter,
            // this.minTotalQtyFilter == null ? this.minTotalQtyFilterEmpty: this.minTotalQtyFilter,
            // this.maxTotalAmtFilter == null ? this.maxTotalAmtFilterEmpty: this.maxTotalAmtFilter,
            // this.minTotalAmtFilter == null ? this.minTotalAmtFilterEmpty: this.minTotalAmtFilter,
            this.activeFilter,
            this.postedFilter,
            // this.maxLinkDetIDFilter == null ? this.maxLinkDetIDFilterEmpty: this.maxLinkDetIDFilter,
            // this.minLinkDetIDFilter == null ? this.minLinkDetIDFilterEmpty: this.minLinkDetIDFilter,
            // this.ordNoFilter, 
            //this.activeFilter,
            this.createdByFilter,
            this.audtUserFilter,
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

    createReceiptReturn(): void {
      this.GetSetUpDetail();
      this._receiptReturnServiceProxy.getMaxDocId().subscribe(result => {
        debugger; 
        if(result!=0){
            this.maxID=result;
        }
          this.createOrEditReceiptReturnModal.show(null,this.maxID);
      });
    }
    GetSetUpDetail(): void {
      this._getDataService.GetSetUpDetail().subscribe(result => {
      
          this.createOrEditReceiptReturnModal.SetDefaultRecord(result);
      });
    }

    deleteReceiptReturn(receiptReturn: PORETHeaderDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._receiptReturnServiceProxy.delete(receiptReturn.docNo)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }
    getReport(rereturn:any) {
      debugger
      let rptParams = "";
      this.fromDoc=rereturn;
      this.toDoc=rereturn;
      rptParams += encodeURIComponent("" + this.fromDoc) + "$";
      rptParams += encodeURIComponent("" + this.toDoc) + "$";
      rptParams += encodeURIComponent("" + this.permission.isGranted('Purchase.ReceiptEntry.ShowAmounts')) + "$";
      rptParams = rptParams.replace(/[?$]&/, "");
      this.reportView.show("ReceiptReturn", rptParams);
    }
    // exportToExcel(): void {
    //     this._poretHeadersServiceProxy.getPOPOHeaderToExcel(
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
