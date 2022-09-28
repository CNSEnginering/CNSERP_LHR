import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GLBOOKSServiceProxy, GLBOOKSDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditGLBOOKSModalComponent } from './create-or-edit-glbooks-modal.component';
import { ViewGLBOOKSModalComponent } from './view-glbooks-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './glbooks.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class GLBOOKSComponent extends AppComponentBase {

    @ViewChild('createOrEditGLBOOKSModal', { static: true }) createOrEditGLBOOKSModal: CreateOrEditGLBOOKSModalComponent;
    @ViewChild('viewGLBOOKSModalComponent', { static: true }) viewGLBOOKSModal: ViewGLBOOKSModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    bookIDFilter = '';
    bookNameFilter = '';
    maxNormalEntryFilter : number;
		maxNormalEntryFilterEmpty : number;
		minNormalEntryFilter : number;
		minNormalEntryFilterEmpty : number;
    integratedFilter = -1;
    inactiveFilter = -1;
    maxAUDTDATEFilter : moment.Moment;
		minAUDTDATEFilter : moment.Moment;
    audtuserFilter = '';
    maxRestrictedFilter : number;
		maxRestrictedFilterEmpty : number;
		minRestrictedFilter : number;
        minRestrictedFilterEmpty : number;
        


        normalEntry = '';




    constructor( 
        injector: Injector,
        private _glbooksServiceProxy: GLBOOKSServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getGLBOOKS(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._glbooksServiceProxy.getAll(
            this.filterText,
            this.bookIDFilter,
            this.bookNameFilter,
            this.maxNormalEntryFilter == null ? this.maxNormalEntryFilterEmpty: this.maxNormalEntryFilter,
            this.minNormalEntryFilter == null ? this.minNormalEntryFilterEmpty: this.minNormalEntryFilter,
            this.integratedFilter,
            this.inactiveFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.maxRestrictedFilter == null ? this.maxRestrictedFilterEmpty: this.maxRestrictedFilter,
            this.minRestrictedFilter == null ? this.minRestrictedFilterEmpty: this.minRestrictedFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            debugger;
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;

            
                
        

            debugger;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createGLBOOKS(): void {
        this.createOrEditGLBOOKSModal.show();
    }

    deleteGLBOOKS(glbooks: GLBOOKSDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._glbooksServiceProxy.delete(glbooks.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._glbooksServiceProxy.getGLBOOKSToExcel(
        this.filterText,
            this.bookIDFilter,
            this.bookNameFilter,
            this.maxNormalEntryFilter == null ? this.maxNormalEntryFilterEmpty: this.maxNormalEntryFilter,
            this.minNormalEntryFilter == null ? this.minNormalEntryFilterEmpty: this.minNormalEntryFilter,
            this.integratedFilter,
            this.inactiveFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.maxRestrictedFilter == null ? this.maxRestrictedFilterEmpty: this.maxRestrictedFilter,
            this.minRestrictedFilter == null ? this.minRestrictedFilterEmpty: this.minRestrictedFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
