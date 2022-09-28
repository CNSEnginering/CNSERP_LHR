import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GLLocationsServiceProxy, GLLocationDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditGLLocationModalComponent } from './create-or-edit-glLocation-modal.component';
import { ViewGLLocationModalComponent } from './view-glLocation-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './glLocations.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class GLLocationsComponent extends AppComponentBase {

    @ViewChild('createOrEditGLLocationModal', { static: true }) createOrEditGLLocationModal: CreateOrEditGLLocationModalComponent;
    @ViewChild('viewGLLocationModalComponent', { static: true }) viewGLLocationModal: ViewGLLocationModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    locDescFilter = '';
    auditUserFilter = '';
    maxAuditDateFilter : moment.Moment;
		minAuditDateFilter : moment.Moment;
    maxLocIdFilter : number;
		maxLocIdFilterEmpty : number;
		minLocIdFilter : number;
        minLocIdFilterEmpty : number;
        
        maxID:number;




    constructor(
        injector: Injector,
        private _glLocationsServiceProxy: GLLocationsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getGLLocations(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._glLocationsServiceProxy.getAll(
            this.filterText,
            this.locDescFilter,
            this.auditUserFilter,
            this.maxAuditDateFilter,
            this.minAuditDateFilter,
            this.maxLocIdFilter == null ? this.maxLocIdFilterEmpty: this.maxLocIdFilter,
            this.minLocIdFilter == null ? this.minLocIdFilterEmpty: this.minLocIdFilter,
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

    createGLLocation(): void {
        this._glLocationsServiceProxy.getMaxLocId().subscribe(result => {
            debugger; 
            if(result!=0){
                this.maxID=result;
            }
            this.createOrEditGLLocationModal.show(null,this.maxID);
        });
    }

    deleteGLLocation(glLocation: GLLocationDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._glLocationsServiceProxy.delete(glLocation.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._glLocationsServiceProxy.getGLLocationsToExcel(
        this.filterText,
            this.locDescFilter,
            this.auditUserFilter,
            this.maxAuditDateFilter,
            this.minAuditDateFilter,
            this.maxLocIdFilter == null ? this.maxLocIdFilterEmpty: this.maxLocIdFilter,
            this.minLocIdFilter == null ? this.minLocIdFilterEmpty: this.minLocIdFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
