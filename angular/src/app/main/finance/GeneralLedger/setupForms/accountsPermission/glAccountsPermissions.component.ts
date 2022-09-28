import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditGLAccountsPermissionModalComponent } from './create-or-edit-glAccountsPermission-modal.component';
import { ViewGLAccountsPermissionModalComponent } from './view-glAccountsPermission-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { GLAccountsPermissionsServiceProxy, GLAccountsPermissionDto } from '@app/main/finance/shared/services/accountsPermission.service';

@Component({
    templateUrl: './glAccountsPermissions.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class GLAccountsPermissionsComponent extends AppComponentBase {

    @ViewChild('createOrEditGLAccountsPermissionModal', { static: true }) createOrEditGLAccountsPermissionModal: CreateOrEditGLAccountsPermissionModalComponent;
    @ViewChild('viewGLAccountsPermissionModalComponent', { static: true }) viewGLAccountsPermissionModal: ViewGLAccountsPermissionModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    userIDFilter = '';
    maxCanSeeFilter : number;
		maxCanSeeFilterEmpty : number;
		minCanSeeFilter : number;
		minCanSeeFilterEmpty : number;
    beginAccFilter = '';
    endAccFilter = '';
    audtUserFilter = '';
    maxAudtDateFilter : moment.Moment;
		minAudtDateFilter : moment.Moment;




    constructor(
        injector: Injector,
        private _glAccountsPermissionsServiceProxy: GLAccountsPermissionsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getGLAccountsPermissions(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._glAccountsPermissionsServiceProxy.getAll(
            this.filterText,
            this.userIDFilter,
            this.maxCanSeeFilter == null ? this.maxCanSeeFilterEmpty: this.maxCanSeeFilter,
            this.minCanSeeFilter == null ? this.minCanSeeFilterEmpty: this.minCanSeeFilter,
            this.beginAccFilter,
            this.endAccFilter,
            this.audtUserFilter,
            this.maxAudtDateFilter,
            this.minAudtDateFilter,
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

    createGLAccountsPermission(): void {
        this.createOrEditGLAccountsPermissionModal.show();
    }

    deleteGLAccountsPermission(glAccountsPermission: GLAccountsPermissionDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._glAccountsPermissionsServiceProxy.delete(glAccountsPermission.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._glAccountsPermissionsServiceProxy.getGLAccountsPermissionsToExcel(
        this.filterText,
            this.userIDFilter,
            this.maxCanSeeFilter == null ? this.maxCanSeeFilterEmpty: this.maxCanSeeFilter,
            this.minCanSeeFilter == null ? this.minCanSeeFilterEmpty: this.minCanSeeFilter,
            this.beginAccFilter,
            this.endAccFilter,
            this.audtUserFilter,
            this.maxAudtDateFilter,
            this.minAudtDateFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
