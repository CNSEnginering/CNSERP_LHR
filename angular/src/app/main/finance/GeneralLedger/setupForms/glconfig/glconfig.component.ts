import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GLCONFIGServiceProxy, GLCONFIGDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditGLCONFIGModalComponent } from './create-or-edit-glconfig-modal.component';
import { ViewGLCONFIGModalComponent } from './view-glconfig-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './glconfig.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class GLCONFIGComponent extends AppComponentBase {

    @ViewChild('createOrEditGLCONFIGModal', { static: true }) createOrEditGLCONFIGModal: CreateOrEditGLCONFIGModalComponent;
    @ViewChild('viewGLCONFIGModalComponent', { static: true }) viewGLCONFIGModal: ViewGLCONFIGModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    accountIDFilter = '';
    maxSubAccIDFilter : number;
		maxSubAccIDFilterEmpty : number;
		minSubAccIDFilter : number;
		minSubAccIDFilterEmpty : number;
    maxConfigIDFilter : number;
		maxConfigIDFilterEmpty : number;
		minConfigIDFilter : number;
		minConfigIDFilterEmpty : number;
    bookIDFilter = '';
    postingOnFilter = -1;
    maxAUDTDATEFilter : moment.Moment;
		minAUDTDATEFilter : moment.Moment;
    audtuserFilter = '';
        glbooksBookNameFilter = '';
        chartofControlAccountNameFilter = '';
        accountSubLedgerSubAccNameFilter = '';




    constructor(
        injector: Injector,
        private _glconfigServiceProxy: GLCONFIGServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getGLCONFIG(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._glconfigServiceProxy.getAll(
            this.filterText,
            this.accountIDFilter,
            this.maxSubAccIDFilter == null ? this.maxSubAccIDFilterEmpty: this.maxSubAccIDFilter,
            this.minSubAccIDFilter == null ? this.minSubAccIDFilterEmpty: this.minSubAccIDFilter,
            this.maxConfigIDFilter == null ? this.maxConfigIDFilterEmpty: this.maxConfigIDFilter,
            this.minConfigIDFilter == null ? this.minConfigIDFilterEmpty: this.minConfigIDFilter,
            this.bookIDFilter,
            this.postingOnFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.glbooksBookNameFilter,
            this.chartofControlAccountNameFilter,
            this.accountSubLedgerSubAccNameFilter,
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

    createGLCONFIG(): void {
        this.createOrEditGLCONFIGModal.show();
    }

    deleteGLCONFIG(glconfig: GLCONFIGDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._glconfigServiceProxy.delete(glconfig.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._glconfigServiceProxy.getGLCONFIGToExcel(
        this.filterText,
            this.accountIDFilter,
            this.maxSubAccIDFilter == null ? this.maxSubAccIDFilterEmpty: this.maxSubAccIDFilter,
            this.minSubAccIDFilter == null ? this.minSubAccIDFilterEmpty: this.minSubAccIDFilter,
            this.maxConfigIDFilter == null ? this.maxConfigIDFilterEmpty: this.maxConfigIDFilter,
            this.minConfigIDFilter == null ? this.minConfigIDFilterEmpty: this.minConfigIDFilter,
            this.bookIDFilter,
            this.postingOnFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.glbooksBookNameFilter,
            this.chartofControlAccountNameFilter,
            this.accountSubLedgerSubAccNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
