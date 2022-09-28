import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';

import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditPLCategoryModalComponent } from './create-or-edit-plCategory-modal.component';

import { ViewPLCategoryModalComponent } from './view-plCategory-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { PLCategoriesServiceProxy, PLCategoryDto } from '@app/main/finance/shared/services/plCategories.service';

@Component({
    templateUrl: './plCategories.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class PLCategoriesComponent extends AppComponentBase {
    
    @ViewChild('createOrEditPLCategoryModal', { static: true }) createOrEditPLCategoryModal: CreateOrEditPLCategoryModalComponent;
    @ViewChild('viewPLCategoryModalComponent', { static: true }) viewPLCategoryModal: ViewPLCategoryModalComponent;    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxTenantIDFilter : number;
		maxTenantIDFilterEmpty : number;
		minTenantIDFilter : number;
		minTenantIDFilterEmpty : number;
    typeIDFilter = '';
    headingTextFilter = '';
    maxSortOrderFilter : number;
		maxSortOrderFilterEmpty : number;
		minSortOrderFilter : number;
		minSortOrderFilterEmpty : number;




    constructor(
        injector: Injector,
        private _plCategoriesServiceProxy: PLCategoriesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getPLCategories(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._plCategoriesServiceProxy.getAll(
            this.filterText,
            this.maxTenantIDFilter == null ? this.maxTenantIDFilterEmpty: this.maxTenantIDFilter,
            this.minTenantIDFilter == null ? this.minTenantIDFilterEmpty: this.minTenantIDFilter,
            this.typeIDFilter,
            this.headingTextFilter,
            this.maxSortOrderFilter == null ? this.maxSortOrderFilterEmpty: this.maxSortOrderFilter,
            this.minSortOrderFilter == null ? this.minSortOrderFilterEmpty: this.minSortOrderFilter,
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

    createPLCategory(): void {
        this.createOrEditPLCategoryModal.show();        
    }


    deletePLCategory(plCategory: PLCategoryDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._plCategoriesServiceProxy.delete(plCategory.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._plCategoriesServiceProxy.getPLCategoriesToExcel(
        this.filterText,
            this.maxTenantIDFilter == null ? this.maxTenantIDFilterEmpty: this.maxTenantIDFilter,
            this.minTenantIDFilter == null ? this.minTenantIDFilterEmpty: this.minTenantIDFilter,
            this.typeIDFilter,
            this.headingTextFilter,
            this.maxSortOrderFilter == null ? this.maxSortOrderFilterEmpty: this.maxSortOrderFilter,
            this.minSortOrderFilter == null ? this.minSortOrderFilterEmpty: this.minSortOrderFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
