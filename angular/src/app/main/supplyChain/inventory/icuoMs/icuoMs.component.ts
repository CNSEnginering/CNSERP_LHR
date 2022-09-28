import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditICUOMModalComponent } from './create-or-edit-icuom-modal.component';
import { ViewICUOMModalComponent } from './view-icuom-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { ICUOMsService } from '../shared/services/ic-uoms.service';
import { ICUOMDto } from '../shared/dto/ic-uoms-dto';

@Component({
    templateUrl: './icuoMs.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ICUOMsComponent extends AppComponentBase {

    @ViewChild('createOrEditICUOMModal', { static: true }) createOrEditICUOMModal: CreateOrEditICUOMModalComponent;
    @ViewChild('viewICUOMModalComponent', { static: true }) viewICUOMModal: ViewICUOMModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    unitFilter = '';
    unitdescFilter = '';
    maxConverFilter : number;
		maxConverFilterEmpty : number;
		minConverFilter : number;
		minConverFilterEmpty : number;
    activeFilter = -1;
    createdByFilter = '';
    maxCreateDateFilter : moment.Moment;
		minCreateDateFilter : moment.Moment;
    audtUserFilter = '';
    maxAudtDateFilter : moment.Moment;
		minAudtDateFilter : moment.Moment;




    constructor(
        injector: Injector,
        private _icuoMsService: ICUOMsService,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getICUOMs(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }
debugger;
        this.primengTableHelper.showLoadingIndicator();

        this._icuoMsService.getAll(
            this.filterText,
            this.unitFilter,
            this.unitdescFilter,
            this.maxConverFilter == null ? this.maxConverFilterEmpty: this.maxConverFilter,
            this.minConverFilter == null ? this.minConverFilterEmpty: this.minConverFilter,
            this.activeFilter,
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

    createICUOM(): void {
        this.createOrEditICUOMModal.show();
    }

    deleteICUOM(icuom: ICUOMDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._icuoMsService.delete(icuom.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._icuoMsService.getICUOMsToExcel(
        this.filterText,
            this.unitFilter,
            this.unitdescFilter,
            this.maxConverFilter == null ? this.maxConverFilterEmpty: this.maxConverFilter,
            this.minConverFilter == null ? this.minConverFilterEmpty: this.minConverFilter,
            this.activeFilter,
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
