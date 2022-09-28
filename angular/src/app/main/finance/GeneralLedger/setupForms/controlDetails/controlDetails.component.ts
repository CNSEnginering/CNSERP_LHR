import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ControlDetailsServiceProxy, ControlDetailDto, GLOptionsServiceProxy } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditControlDetailModalComponent } from './create-or-edit-controlDetail-modal.component';
import { ViewControlDetailModalComponent } from './view-controlDetail-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient } from '@angular/common/http';
import { finalize } from 'rxjs/operators';
import { FileUpload } from 'primeng/primeng';

@Component({
    templateUrl: './controlDetails.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ControlDetailsComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditControlDetailModal', { static: true }) createOrEditControlDetailModal: CreateOrEditControlDetailModalComponent;
    @ViewChild('viewControlDetailModalComponent', { static: true }) viewControlDetailModal: ViewControlDetailModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('ExcelFileUpload', { static: true }) excelFileUpload: FileUpload;

    advancedFiltersAreShown = false;
    filterText = '';
    seg1IDFilter :'';
    segmentNameFilter = '';
    FamilyFilter: string;
    FamilyFilterEmpty = '';
    maxID = "";
    Isupdate = false;
    oldCodeFilter = '';

    uploadUrl: string;

    constructor(
        injector: Injector,
        private _controlDetailsServiceProxy: ControlDetailsServiceProxy,
        private _gLOptionsServiceProxy: GLOptionsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _httpClient: HttpClient
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/ControlDetail/ImportFromExcel';
        debugger;
    }

    
    defaultclaccFilter = '';
    stockctrlaccFilter = '';
    seg1NameFilter = '';
    seg2NameFilter = '';
    seg3NameFilter = '';
    directPostFilter = -1;
    autoSeg3Filter = -1;
    maxAUDTDATEFilter : moment.Moment;
		minAUDTDATEFilter : moment.Moment;
    audtuserFilter = '';
        chartofControlIdFilter = '';


    ngOnInit(): void {
        debugger
        this.GetGloptionList();
    }

    GloptionSetup : string;

    segment1des = '';

    GetGloptionList() {
        this._gLOptionsServiceProxy.getAll(
            "",
            this.defaultclaccFilter,
            this.stockctrlaccFilter,
            this.seg1NameFilter,
            this.seg2NameFilter,
            this.seg3NameFilter,
            this.directPostFilter,
            this.autoSeg3Filter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.chartofControlIdFilter,
            null,
            0,
            2147483646
            
            ).subscribe( res => {
            debugger;
            this.GloptionSetup = res.items[0].glOption.seg1Name;
        })
    }


    getControlDetails(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._controlDetailsServiceProxy.getAll(
            this.filterText,
            this.seg1IDFilter,
            this.segmentNameFilter,
            this.FamilyFilter ,
            this.oldCodeFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createControlDetail(): void {
        this.createOrEditControlDetailModal.show(this.Isupdate); 
    }

    deleteControlDetail(controlDetail: ControlDetailDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._controlDetailsServiceProxy.delete(controlDetail.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._controlDetailsServiceProxy.getControlDetailsToExcel(
            this.filterText,
            this.seg1IDFilter,
            this.segmentNameFilter,
            this.FamilyFilter ,
          
            this.oldCodeFilter,
            
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
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
                    this.notify.success(this.l('ImportControlDetailProcessStart'));
                } else if (response.error != null) {
                    this.notify.error(this.l('ImportControlDetailUploadFailed'));
                }
            });
    }

    onUploadExcelError(): void {
        this.notify.error(this.l('ImportControlDetailUploadFailed'));
    }
}
