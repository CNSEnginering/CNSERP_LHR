import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GroupCategoriesServiceProxy, GroupCategoryDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditGroupCategoryModalComponent } from './create-or-edit-groupCategory-modal.component';
import { ViewGroupCategoryModalComponent } from './view-groupCategory-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './groupCategories.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class GroupCategoriesComponent extends AppComponentBase {

    @ViewChild('createOrEditGroupCategoryModal', { static: true }) createOrEditGroupCategoryModal: CreateOrEditGroupCategoryModalComponent;
    @ViewChild('viewGroupCategoryModalComponent', { static: true }) viewGroupCategoryModal: ViewGroupCategoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    grpctdescFilter = '';
    maxID:any;




    constructor(
        injector: Injector,
        private _groupCategoriesServiceProxy: GroupCategoriesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getGroupCategories(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
debugger;
        this._groupCategoriesServiceProxy.getAll(
           
            this.filterText,
            this.grpctdescFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            debugger;
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            // if (result.items.length == 0) {
            //     this.maxID = 1
            // }
            // else {
            //     this.maxID = result.items.slice(-1)[0].groupCategory.id + 1;
            // }


            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createGroupCategory(): void {
        this.createOrEditGroupCategoryModal.show(this.maxID);
    }

    deleteGroupCategory(groupCategory: GroupCategoryDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._groupCategoriesServiceProxy.delete(groupCategory.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._groupCategoriesServiceProxy.getGroupCategoriesToExcel(
        this.filterText,
            this.grpctdescFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
