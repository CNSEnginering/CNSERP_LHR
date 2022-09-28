import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GroupCodesServiceProxy, GroupCodeDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditGroupCodeModalComponent } from './create-or-edit-groupCode-modal.component';
import { ViewGroupCodeModalComponent } from './view-groupCode-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './groupCodes.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class GroupCodesComponent extends AppComponentBase {

    @ViewChild('createOrEditGroupCodeModal', { static: true }) createOrEditGroupCodeModal: CreateOrEditGroupCodeModalComponent;
    @ViewChild('viewGroupCodeModalComponent', { static: true }) viewGroupCodeModal: ViewGroupCodeModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxGRPCODEFilter: number;
    maxGRPCODEFilterEmpty: number;
    minGRPCODEFilter: number;
    minGRPCODEFilterEmpty: number;
    grpdescFilter = '';
    grpctcodeFilter: any;
    grpctdescFilter = '';




    constructor(
        injector: Injector,
        private _groupCodesServiceProxy: GroupCodesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getGroupCodes(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        debugger;
        this._groupCodesServiceProxy.getAll(
           
            this.filterText,
            this.maxGRPCODEFilter == null ? this.maxGRPCODEFilterEmpty : this.maxGRPCODEFilter,
            this.minGRPCODEFilter == null ? this.minGRPCODEFilterEmpty : this.minGRPCODEFilter,
            this.grpdescFilter,
            this.grpctdescFilter,

            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            debugger;
            this.primengTableHelper.records = result.items;
            debugger;
            // if (result.items.length == 0) {
            //     this.maxID = 1
            // }
            // else {
            //     this.maxID = result.items.slice(-1)[0].groupCode.id + 1;
            // }
            this.primengTableHelper.hideLoadingIndicator();
        });
    }




    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createGroupCode(): void {
        this.createOrEditGroupCodeModal.show(false);
    }

    deleteGroupCode(groupCode: GroupCodeDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._groupCodesServiceProxy.delete(groupCode.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._groupCodesServiceProxy.getGroupCodesToExcel(
            this.filterText,
            this.maxGRPCODEFilter == null ? this.maxGRPCODEFilterEmpty : this.maxGRPCODEFilter,
            this.minGRPCODEFilter == null ? this.minGRPCODEFilterEmpty : this.minGRPCODEFilter,
            this.grpdescFilter,
            this.grpctcodeFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
