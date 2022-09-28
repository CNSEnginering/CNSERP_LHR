import { Component, Injector, ViewEncapsulation, ViewChild, EventEmitter, Output } from '@angular/core';
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
import { ViewDirectInvoiceModalComponent } from './view-directInvoice-modal.component';
import { CreateOrEditDirectInvoiceModalComponent } from './create-or-edit-directInvoice-modal.component';
import { GLINVHeaderDto } from '../../shared/dto/glinvHeader-dto';
import { GLINVHeadersService } from '../../shared/services/glinvHeader.service';
import { DirectInvoiceServiceProxy } from '../../shared/services/directinvoice.service';
import { HttpClient } from '@angular/common/http';
import { AppConsts } from '@shared/AppConsts';
import { FileUpload } from 'primeng/primeng';
import { finalize } from 'rxjs/operators';

@Component({
    selector:'directInvoiceComponent',
    templateUrl: './directInvoice.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class DirectinvoiceComponent extends AppComponentBase {

    @ViewChild('createOrEditDirectInvoiceModal', { static: true }) createOrEditDirectInvoiceModal: CreateOrEditDirectInvoiceModalComponent;
    @ViewChild('viewDirectInvoiceModal', { static: true }) viewDirectInvoiceModal: ViewDirectInvoiceModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('ExcelFileUpload', { static: true }) excelFileUpload: FileUpload;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDocNoFilter : number;
        maxDocNoFilterEmpty : number; 
    minDocNoFilter : number;
        minDocNoFilterEmpty : number;
    typeIDFilter = 'AP';
    bankIDFilter ='';
    maxDocDateFilter : moment.Moment;
	minDocDateFilter : moment.Moment;
    maxPostDateFilter : moment.Moment;
    minPostDateFilter : moment.Moment;
    partyInvNoFilter= '';
    maxPartyInvDateFilter : moment.Moment;
    minPartyInvDateFilter : moment.Moment;
    narrationFilter = '';
    postedFilter = -1;
    audtUserFilter = '';
    maxAudtDateFilter : moment.Moment;
    minAudtDateFilter : moment.Moment;
    createdByFilter = '';
    maxCreateDateFilter : moment.Moment;
    minCreateDateFilter : moment.Moment;
    

    maxID:any;
    directInvoice:string;
    directInvoiceHeaderInfo:string;

    uploadUrl: string;

    constructor(
        injector: Injector,
        private _glinvHeadersServiceProxy: GLINVHeadersService,
        private _directInvoiceServiceProxy: DirectInvoiceServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _httpClient: HttpClient
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/DirectInvoiceCPR/ImportFromExcel';
    }

    getInvType(type?:string){
        debugger;
        this.typeIDFilter=type;
    }

    getGLINVHeaders(event?: LazyLoadEvent) {
        debugger;
        if (this.primengTableHelper.shouldResetPaging(event)) { 
            this.paginator.changePage(0);
            return;
        }

        this.directInvoice=this.typeIDFilter+" "+this.l('DirectInvoice');
        if(this.typeIDFilter == 'ST')
          this.directInvoiceHeaderInfo=this.l('DirectInvoiceWithSalesTax');
        else
          this.directInvoiceHeaderInfo=this.typeIDFilter+" "+this.l('DirectInvoiceHeaderInfo');


        debugger;
        this.primengTableHelper.showLoadingIndicator();

        this._glinvHeadersServiceProxy.getAll(
            this.filterText,
            this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty: this.maxDocNoFilter,
            this.minDocNoFilter == null ? this.minDocNoFilterEmpty: this.minDocNoFilter,
            this.bankIDFilter,
            this.typeIDFilter,
            this.maxDocDateFilter,
            this.minDocDateFilter,
            this.maxPostDateFilter,
            this.minPostDateFilter,
            this.partyInvNoFilter,
            this.minPartyInvDateFilter,
            this.maxPartyInvDateFilter,
            this.postedFilter,
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

    createGLINVHeader(val){
        debugger;
        this._directInvoiceServiceProxy.getMaxDocId(this.typeIDFilter).subscribe(result=>{
            if(result!=0){
                this.maxID=result;
            }
            this.createOrEditDirectInvoiceModal.show(null,this.maxID,this.typeIDFilter);
        });
    }

    deleteGLINVHeader(glinvHeader: GLINVHeaderDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {debugger;
                    this._directInvoiceServiceProxy.delete(glinvHeader.id)
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

    onBeforeSend(event): void {
        event.xhr.setRequestHeader('Authorization', 'Bearer ' + abp.auth.getToken());
    }

    uploadExcel(data: { files: File }): void {
        const formData: FormData = new FormData();
        const file = data.files[0];
        formData.append('file', file, file.name);

        this._httpClient
            .post<any>(this.uploadUrl, formData)
            .pipe(finalize(() => this.excelFileUpload.clear()))
            .subscribe(response => {
                if (response.success) {
                    this.notify.success(this.l('ImportDirectInvoiceProcessStart'));
                } else if (response.error != null) {
                    this.notify.error(this.l('ImportDirectInvoiceUploadFailed'));
                }
            });
    }

    onUploadExcelError(): void {
        this.notify.error(this.l('ImportDirectInvoiceUploadFailed'));
    }
}
