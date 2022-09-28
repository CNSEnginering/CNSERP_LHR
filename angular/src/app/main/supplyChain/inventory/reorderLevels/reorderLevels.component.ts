import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditReorderLevelModalComponent } from './create-or-edit-reorderLevel-modal.component';
import { ViewReorderLevelModalComponent } from './view-reorderLevel-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { ReorderLevelsService } from '../shared/services/reorder-levels.service';
import { ReorderLevelDto } from '../shared/dto/reorder-levels-dto';

@Component({
    templateUrl: './reorderLevels.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ReorderLevelsComponent extends AppComponentBase {

    @ViewChild('createOrEditReorderLevelModal', { static: true }) createOrEditReorderLevelModal: CreateOrEditReorderLevelModalComponent;
    @ViewChild('viewReorderLevelModalComponent', { static: true }) viewReorderLevelModal: ViewReorderLevelModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxMinLevelFilter : number;
		maxMinLevelFilterEmpty : number;
		minMinLevelFilter : number;
		minMinLevelFilterEmpty : number;
    maxMaxLevelFilter : number;
		maxMaxLevelFilterEmpty : number;
		minMaxLevelFilter : number;
		minMaxLevelFilterEmpty : number;
    maxOrdLevelFilter : number;
		maxOrdLevelFilterEmpty : number;
		minOrdLevelFilter : number;
		minOrdLevelFilterEmpty : number;
    createdByFilter = '';
    maxCreateDateFilter : moment.Moment;
		minCreateDateFilter : moment.Moment;
    audtUserFilter = '';
    maxAudtDateFilter : moment.Moment;
		minAudtDateFilter : moment.Moment;
    maxLocIdFilter : number;
		maxLocIdFilterEmpty : number;
		minLocIdFilter : number;
		minLocIdFilterEmpty : number;
    itemIdFilter = '';




    constructor(
        injector: Injector,
        private _reorderLevelsService: ReorderLevelsService,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getReorderLevels(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._reorderLevelsService.getAll(
            this.filterText,
            this.maxLocIdFilter == null ? this.maxLocIdFilterEmpty: this.maxLocIdFilter,
            this.minLocIdFilter == null ? this.minLocIdFilterEmpty: this.minLocIdFilter,
            this.itemIdFilter,
            this.maxMinLevelFilter == null ? this.maxMinLevelFilterEmpty: this.maxMinLevelFilter,
            this.minMinLevelFilter == null ? this.minMinLevelFilterEmpty: this.minMinLevelFilter,
            this.maxMaxLevelFilter == null ? this.maxMaxLevelFilterEmpty: this.maxMaxLevelFilter,
            this.minMaxLevelFilter == null ? this.minMaxLevelFilterEmpty: this.minMaxLevelFilter,
            this.maxOrdLevelFilter == null ? this.maxOrdLevelFilterEmpty: this.maxOrdLevelFilter,
            this.minOrdLevelFilter == null ? this.minOrdLevelFilterEmpty: this.minOrdLevelFilter,
            this.createdByFilter,
            this.maxCreateDateFilter,
            this.minCreateDateFilter,
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

    createReorderLevel(): void {
        this.createOrEditReorderLevelModal.show();
    }

    deleteReorderLevel(reorderLevel: ReorderLevelDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._reorderLevelsService.delete(reorderLevel.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._reorderLevelsService.getReorderLevelsToExcel(
            this.filterText,
            this.maxLocIdFilter == null ? this.maxLocIdFilterEmpty: this.maxLocIdFilter,
            this.minLocIdFilter == null ? this.minLocIdFilterEmpty: this.minLocIdFilter,
            this.itemIdFilter,
            this.maxMinLevelFilter == null ? this.maxMinLevelFilterEmpty: this.maxMinLevelFilter,
            this.minMinLevelFilter == null ? this.minMinLevelFilterEmpty: this.minMinLevelFilter,
            this.maxMaxLevelFilter == null ? this.maxMaxLevelFilterEmpty: this.maxMaxLevelFilter,
            this.minMaxLevelFilter == null ? this.minMaxLevelFilterEmpty: this.minMaxLevelFilter,
            this.maxOrdLevelFilter == null ? this.maxOrdLevelFilterEmpty: this.maxOrdLevelFilter,
            this.minOrdLevelFilter == null ? this.minOrdLevelFilterEmpty: this.minOrdLevelFilter,
            this.createdByFilter,
            this.maxCreateDateFilter,
            this.minCreateDateFilter,
            this.audtUserFilter,
            this.maxAudtDateFilter,
            this.minAudtDateFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
