import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditHolidaysModalComponent } from './create-or-edit-holidays-modal.component';
import { ViewHolidaysModalComponent } from './view-holidays-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { HolidaysServiceProxy } from '../shared/services/holidays.service';
import { HolidaysDto } from '../shared/dto/holidays-dto';

@Component({
    templateUrl: './holidays.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class HolidaysComponent extends AppComponentBase {


    @ViewChild('createOrEditHolidaysModal', { static: true }) createOrEditHolidaysModal: CreateOrEditHolidaysModalComponent;
    @ViewChild('viewHolidaysModalComponent', { static: true }) viewHolidaysModal: ViewHolidaysModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxHolidayIDFilter: number;
    maxHolidayIDFilterEmpty: number;
    minHolidayIDFilter: number;
    minHolidayIDFilterEmpty: number;
    maxHolidayDateFilter: moment.Moment;
    minHolidayDateFilter: moment.Moment;
    holidayNameFilter = '';
    activeFilter = -1;
    audtUserFilter = '';
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    createdByFilter = '';
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;




    constructor(
        injector: Injector,
        private _holidaysServiceProxy: HolidaysServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getHolidays(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._holidaysServiceProxy.getAll(
            this.filterText,
            this.maxHolidayIDFilter == null ? this.maxHolidayIDFilterEmpty : this.maxHolidayIDFilter,
            this.minHolidayIDFilter == null ? this.minHolidayIDFilterEmpty : this.minHolidayIDFilter,
            this.maxHolidayDateFilter,
            this.minHolidayDateFilter,
            this.holidayNameFilter,
            this.activeFilter,
            this.audtUserFilter,
            this.maxAudtDateFilter,
            this.minAudtDateFilter,
            this.createdByFilter,
            this.maxCreateDateFilter,
            this.minCreateDateFilter,
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

    createHolidays(): void {
        this.createOrEditHolidaysModal.show();
    }


    deleteHolidays(holidays: HolidaysDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._holidaysServiceProxy.delete(holidays.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._holidaysServiceProxy.getHolidaysToExcel(
            this.filterText,
            this.maxHolidayIDFilter == null ? this.maxHolidayIDFilterEmpty : this.maxHolidayIDFilter,
            this.minHolidayIDFilter == null ? this.minHolidayIDFilterEmpty : this.minHolidayIDFilter,
            this.maxHolidayDateFilter,
            this.minHolidayDateFilter,
            this.holidayNameFilter,
            this.activeFilter,
            this.audtUserFilter,
            this.maxAudtDateFilter,
            this.minAudtDateFilter,
            this.createdByFilter,
            this.maxCreateDateFilter,
            this.minCreateDateFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
