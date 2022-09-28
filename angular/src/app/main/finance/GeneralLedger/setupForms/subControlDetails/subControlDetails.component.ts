import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SubControlDetailsServiceProxy, SubControlDetailDto, GLOptionsServiceProxy } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditSubControlDetailModalComponent } from './create-or-edit-subControlDetail-modal.component';
import { ViewSubControlDetailModalComponent } from './view-subControlDetail-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { FileUpload } from 'primeng/primeng';
import { HttpClient } from '@angular/common/http';
import { AppConsts } from '@shared/AppConsts';
import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './subControlDetails.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class SubControlDetailsComponent extends AppComponentBase implements OnInit {

    @ViewChild('ExcelFileUpload', { static: true }) excelFileUpload: FileUpload;
    @ViewChild('createOrEditSubControlDetailModal', { static: true }) createOrEditSubControlDetailModal: CreateOrEditSubControlDetailModalComponent;
    @ViewChild('viewSubControlDetailModalComponent', { static: true }) viewSubControlDetailModal: ViewSubControlDetailModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    seg2IDFilter = '';
    segmentNameFilter = '';
    oldCodeFilter = '';
    controlDetailIdFilter = '';

    uploadUrl: string;


    constructor(
        injector: Injector,
        private _subControlDetailsServiceProxy: SubControlDetailsServiceProxy,

        private _fileDownloadService: FileDownloadService,
        private _httpClient: HttpClient,
        private _gLOptionsServiceProxy: GLOptionsServiceProxy
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/SubControlDetail/ImportFromExcel';
    }
    ngOnInit(): void {
        this.GetGloptionList();
    }

    defaultclaccFilter = '';
    stockctrlaccFilter = '';
    seg1NameFilter = '';
    seg2NameFilter = '';
    seg3NameFilter = '';
    directPostFilter = -1;
    autoSeg3Filter = -1;
    maxAUDTDATEFilter: moment.Moment;
    minAUDTDATEFilter: moment.Moment;
    audtuserFilter = '';
    chartofControlIdFilter = '';

    GloptionSetup: string;

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

        ).subscribe(res => {
            debugger;
            this.GloptionSetup = res.items[0].glOption.seg2Name;
        })
    }

    getSubControlDetails(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._subControlDetailsServiceProxy.getAll(
            this.filterText,
            this.seg2IDFilter,
            this.segmentNameFilter,
            this.oldCodeFilter,
            this.controlDetailIdFilter,
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

    createSubControlDetail(): void {
        this.createOrEditSubControlDetailModal.show(false);
    }

    deleteSubControlDetail(subControlDetail: SubControlDetailDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._subControlDetailsServiceProxy.delete(subControlDetail.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._subControlDetailsServiceProxy.getSubControlDetailsToExcel(
            this.filterText,
            this.seg2IDFilter,
            this.segmentNameFilter,
            this.oldCodeFilter,
            this.controlDetailIdFilter,
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
                debugger;
                if (response["error"]["message"] == null) {
                    this.getSubControlDetails(null)
                    this.notify.success(this.l('AllSubControlDetailSuccessfullyImportedFromExcel'));
                }
                else
                    this.notify.error(response["error"]["message"]);
            });
    }

    onUploadExcelError(): void {
        this.notify.error(this.l('ImportSubControlDetailUploadFailed'));
    }
}
