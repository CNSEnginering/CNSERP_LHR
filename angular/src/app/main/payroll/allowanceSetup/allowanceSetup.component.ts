import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditAllowanceSetupModalComponent } from './create-or-edit-allowanceSetup-modal.component';
import { ViewAllowanceSetupModalComponent } from './view-allowanceSetup-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { AllowanceSetupServiceProxy } from '../shared/services/allowanceSetup.service';
import { AllowanceSetupDto } from '../shared/dto/allowanceSetup-dto';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './allowanceSetup.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class AllowanceSetupComponent extends AppComponentBase {


    @ViewChild('createOrEditAllowanceSetupModal', { static: true }) createOrEditAllowanceSetupModal: CreateOrEditAllowanceSetupModalComponent;
    @ViewChild('viewAllowanceSetupModalComponent', { static: true }) viewAllowanceSetupModal: ViewAllowanceSetupModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDocIDFilter: number;
    maxDocIDFilterEmpty: number;
    minDocIDFilter: number;
    minDocIDFilterEmpty: number;
    maxFuelRateFilter: number;
    maxFuelRateFilterEmpty: number;
    minFuelRateFilter: number;
    minFuelRateFilterEmpty: number;
    maxMilageRateFilter: number;
    maxMilageRateFilterEmpty: number;
    minMilageRateFilter: number;
    minMilageRateFilterEmpty: number;
    maxRepairRateFilter: number;
    maxRepairRateFilterEmpty: number;
    minRepairRateFilter: number;
    minRepairRateFilterEmpty: number;
    activeFilter = -1;
    audtUserFilter = '';
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    createdByFilter = '';
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;




    constructor(
        injector: Injector,
        private _allowanceSetupServiceProxy: AllowanceSetupServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getAllowanceSetup(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._allowanceSetupServiceProxy.getAll(
            this.filterText,
            this.maxDocIDFilter == null ? this.maxDocIDFilterEmpty : this.maxDocIDFilter,
            this.minDocIDFilter == null ? this.minDocIDFilterEmpty : this.minDocIDFilter,
            this.maxFuelRateFilter == null ? this.maxFuelRateFilterEmpty : this.maxFuelRateFilter,
            this.minFuelRateFilter == null ? this.minFuelRateFilterEmpty : this.minFuelRateFilter,
            this.maxMilageRateFilter == null ? this.maxMilageRateFilterEmpty : this.maxMilageRateFilter,
            this.minMilageRateFilter == null ? this.minMilageRateFilterEmpty : this.minMilageRateFilter,
            this.maxRepairRateFilter == null ? this.maxRepairRateFilterEmpty : this.maxRepairRateFilter,
            this.minRepairRateFilter == null ? this.minRepairRateFilterEmpty : this.minRepairRateFilter,
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

    createAllowanceSetup(): void {
        this.createOrEditAllowanceSetupModal.show();
    }


    deleteAllowanceSetup(allowanceSetup: AllowanceSetupDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._allowanceSetupServiceProxy.delete(allowanceSetup.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._allowanceSetupServiceProxy.getAllowanceSetupToExcel(
            this.filterText,
            this.maxDocIDFilter == null ? this.maxDocIDFilterEmpty : this.maxDocIDFilter,
            this.minDocIDFilter == null ? this.minDocIDFilterEmpty : this.minDocIDFilter,
            this.maxFuelRateFilter == null ? this.maxFuelRateFilterEmpty : this.maxFuelRateFilter,
            this.minFuelRateFilter == null ? this.minFuelRateFilterEmpty : this.minFuelRateFilter,
            this.maxMilageRateFilter == null ? this.maxMilageRateFilterEmpty : this.maxMilageRateFilter,
            this.minMilageRateFilter == null ? this.minMilageRateFilterEmpty : this.minMilageRateFilter,
            this.maxRepairRateFilter == null ? this.maxRepairRateFilterEmpty : this.maxRepairRateFilter,
            this.minRepairRateFilter == null ? this.minRepairRateFilterEmpty : this.minRepairRateFilter,
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
