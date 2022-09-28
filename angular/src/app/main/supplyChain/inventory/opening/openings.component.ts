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
import { ICOPNHeaderDto } from '../shared/dto/icopnHeader-dto';
import { CreateOrEditOpeningModalComponent } from './create-or-edit-opening-modal.component';
import { ViewOpeningModalComponent } from './view-opening-modal.component';
import { OpeningServiceProxy } from '../shared/services/opening.service';
import { ICOPNHeadersService } from '../shared/services/icopnHeader.service';
import { HttpClient } from '@angular/common/http';
import { FileUpload } from 'primeng/primeng';
import { finalize } from 'rxjs/internal/operators/finalize';
import { AppConsts } from '@shared/AppConsts';
import { GetDataService } from '@app/main/supplyChain/inventory/shared/services/get-data.service';
//import { DatePipe } from '@angular/common';
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
@Component({
    templateUrl: './openings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class OpeningsComponent extends AppComponentBase {

    @ViewChild('createOrEditOpeningModal', { static: true }) createOrEditOpeningModal: CreateOrEditOpeningModalComponent;
    @ViewChild('viewOpeningModalComponent', { static: true }) viewOpeningModal: ViewOpeningModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild("ExcelFileUpload", { static: true }) excelFileUpload: FileUpload;
    @ViewChild("reportviewrModalComponent", { static: false }) reportView: ReportviewrModalComponent;
    advancedFiltersAreShown = false;
    filterText = '';
    maxDocNoFilter : number;
		maxDocNoFilterEmpty : number;
		minDocNoFilter : number;
		minDocNoFilterEmpty : number;
    maxDocDateFilter : moment.Moment;
		minDocDateFilter : moment.Moment;
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
    activeFilter = -1;
    createdByFilter = '';
    maxCreateDateFilter : moment.Moment;
		minCreateDateFilter : moment.Moment;
    audtUserFilter = '';
    maxAudtDateFilter : moment.Moment;
    minAudtDateFilter : moment.Moment;
    
    maxID:number;
    uploadUrl: string;
    locations:any;
    users:any;


    constructor(
        injector: Injector,
        private _openingsServiceProxy: OpeningServiceProxy,
        private _icopnHeadersServiceProxy: ICOPNHeadersService,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _httpClient:HttpClient,
        private _getDataService: GetDataService,
        //private datePipe: DatePipe
    ) {
        super(injector);
        this.uploadUrl =
        AppConsts.remoteServiceBaseUrl + "/ICOPN/ImportFromExcel";
        this._getDataService.getList("ICLocations").subscribe(result => {
          this.locations = result;
        });
        this._getDataService.getList("Users").subscribe(result => {
          this.users = result;
        });
    }

    getOpenings(event?: LazyLoadEvent) {
      debugger;
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this.maxLocIDFilter=this.minLocIDFilter;
        this._icopnHeadersServiceProxy.getAll(
            this.filterText,
            this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty: this.maxDocNoFilter,
            this.minDocNoFilter == null ? this.minDocNoFilterEmpty: this.minDocNoFilter,
            this.maxDocDateFilter,
            this.minDocDateFilter,
            //this.narrationFilter,
            this.maxLocIDFilter == null ? this.maxLocIDFilterEmpty: this.maxLocIDFilter,
            this.minLocIDFilter == null ? this.minLocIDFilterEmpty: this.minLocIDFilter,
            // this.maxTotalQtyFilter == null ? this.maxTotalQtyFilterEmpty: this.maxTotalQtyFilter,
            // this.minTotalQtyFilter == null ? this.minTotalQtyFilterEmpty: this.minTotalQtyFilter,
            // this.maxTotalAmtFilter == null ? this.maxTotalAmtFilterEmpty: this.maxTotalAmtFilter,
            // this.minTotalAmtFilter == null ? this.minTotalAmtFilterEmpty: this.minTotalAmtFilter,
            this.postedFilter,
            this.maxLinkDetIDFilter == null ? this.maxLinkDetIDFilterEmpty: this.maxLinkDetIDFilter,
            this.minLinkDetIDFilter == null ? this.minLinkDetIDFilterEmpty: this.minLinkDetIDFilter,
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
        ).subscribe(result => {debugger;
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createOpening(): void {
        this.GetSetUpDetail();
      this._openingsServiceProxy.getMaxDocId().subscribe(result => {
        debugger; 
        if(result!=0){
            this.maxID=result;
        }
          this.createOrEditOpeningModal.show(null,this.maxID);
      });
    }
    GetSetUpDetail(): void {
        this._getDataService.GetSetUpDetail().subscribe(result => {
          debugger; 
          if(result!=null){
             // this.=result;
             
          }
            this.createOrEditOpeningModal.SetDefaultRecord(result);
        });
      }
    deleteOpening(opening: ICOPNHeaderDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._openingsServiceProxy.delete(opening.docNo)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    // exportToExcel(): void {
    //     this._icopnHeadersServiceProxy.getICOPNHeaderToExcel(
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
                        this.l("AllICOPNSuccessfullyImportedFromExcel")
                    );}
                else {this.message.error(response["error"]["message"]);}

                abp.ui.clearBusy();
            });
    }

    onUploadExcelError(): void {
        this.notify.error(this.l("ImportICOPNUploadFailed"));
    }
    getReport(opening: ICOPNHeaderDto) {
      debugger
      let rptParams = "";
      //rptParams += "" + this.datePipe.transform(opening.docDate,"yyyy/MM/dd") + "$";
      //rptParams += "" + this.datePipe.transform(opening.docDate,"yyyy/MM/dd") + "$";
      rptParams += "" + moment(opening.docDate).format("YYYY/MM/DD") + "$";
      rptParams += "" + moment(opening.docDate).format("YYYY/MM/DD") + "$";
      rptParams += "" + opening.docNo + "$";
      rptParams += "" + opening.docNo + "$";
      rptParams += "" + opening.locID + "$";
      rptParams += "" + opening.locID + "$";
      rptParams += encodeURIComponent("" + this.permission.isGranted('Purchase.ReceiptEntry.ShowAmounts')) + "$";
      rptParams = rptParams.replace(/[?$]&/, "");
      this.reportView.show("OpeningReport", rptParams);
  }
}
