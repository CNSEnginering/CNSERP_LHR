import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Segmentlevel3sServiceProxy, Segmentlevel3Dto, GLOptionsServiceProxy } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditSegmentlevel3ModalComponent } from './create-or-edit-segmentlevel3-modal.component';
import { ViewSegmentlevel3ModalComponent } from './view-segmentlevel3-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { FileUpload } from 'primeng/primeng';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient } from '@angular/common/http';
import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './segmentlevel3s.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class Segmentlevel3sComponent extends AppComponentBase {

    @ViewChild('ExcelFileUpload', { static: true }) excelFileUpload: FileUpload;
    @ViewChild('createOrEditSegmentlevel3Modal', { static: true }) createOrEditSegmentlevel3Modal: CreateOrEditSegmentlevel3ModalComponent;
    @ViewChild('viewSegmentlevel3ModalComponent', { static: true }) viewSegmentlevel3Modal: ViewSegmentlevel3ModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    seg3IDFilter = '';
    seg1Filter = '';
    seg2Filter = '';
    segmentNameFilter = '';
    oldCodeFilter = '';
    controlDetailIdFilter = '';
    subControlDetailIdFilter = '';


    uploadUrl: string;

    constructor(
        injector: Injector,
        private _segmentlevel3sServiceProxy: Segmentlevel3sServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _httpClient: HttpClient,
        private _gLOptionsServiceProxy: GLOptionsServiceProxy
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/SegmentLevel3/ImportFromExcel';
    }

    ngOnInit(): void {
        debugger
        this.GetGloptionList();
    }

    GloptionSetup: string;

    segment1Des = '';
    segment2Des = '';

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

    Gloptionlevel1: string;
    Gloptionlevel2: string;

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
            this.GloptionSetup = res.items[0].glOption.seg3Name;
            this.Gloptionlevel1 = res.items[0].glOption.seg1Name;
            this.Gloptionlevel2 = res.items[0].glOption.seg2Name
        })
    }

    getSegmentlevel3s(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._segmentlevel3sServiceProxy.getAll(
            this.filterText,
            this.seg1Filter,
            this.seg2Filter,
            this.seg3IDFilter,
            this.segmentNameFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            debugger
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createSegmentlevel3(): void {
        this.createOrEditSegmentlevel3Modal.show(false);
    }

    deleteSegmentlevel3(segmentlevel3: Segmentlevel3Dto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._segmentlevel3sServiceProxy.delete(segmentlevel3.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._segmentlevel3sServiceProxy.getSegmentlevel3sToExcel(
            this.filterText,
            this.seg3IDFilter,
            this.segmentNameFilter,

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
        abp.ui.setBusy(undefined, '', 1);
        this._httpClient
            .post<any>(this.uploadUrl, formData)
            .pipe(finalize(() => { this.excelFileUpload.clear(); abp.ui.clearBusy(); }))
            .subscribe(response => {
                if (response["error"]["message"] === null)
                    this.notify.success(
                        this.l("AllSegmentLevel3SuccessfullyImportedFromExcel")
                    );
                else this.message.error(response["error"]["message"]);
            });
    }

    onUploadExcelError(): void {
        this.notify.error(this.l('ImportSegmentLevel3UploadFailed'));
    }
}
