import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { CreateOrEditICSetupModalComponent } from './create-or-edit-ic-setup-modal.component';
import { ViewICSetupModalComponent } from './view-ic-setup-modal.component';
import { ICSetupsService } from '../shared/services/ic-setup.service';
import { ICSetupDto } from '../shared/dto/ic-setup-dto';

@Component({
    templateUrl: './ic-setup.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ICSetupsComponent extends AppComponentBase {

    @ViewChild('createOrEditICSetupModal', { static: true }) createOrEditICSetupModal: CreateOrEditICSetupModalComponent;
    @ViewChild('viewICSetupModalComponent', { static: true }) viewICSetupModal: ViewICSetupModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    segment1Filter = '';
    segment2Filter = '';
    segment3Filter = '';
    maxAllowNegativeFilter : number;
		maxAllowNegativeFilterEmpty : number;
		minAllowNegativeFilter : number;
		minAllowNegativeFilterEmpty : number;
    maxErrSrNoFilter : number;
		maxErrSrNoFilterEmpty : number;
		minErrSrNoFilter : number;
		minErrSrNoFilterEmpty : number;
    maxCostingMethodFilter : number;
		maxCostingMethodFilterEmpty : number;
		minCostingMethodFilter : number;
		minCostingMethodFilterEmpty : number;
    prBookIDFilter = '';
    rtBookIDFilter = '';
    cnsBookIDFilter = '';
    slBookIDFilter = '';
    srBookIDFilter = '';
    trBookIDFilter = '';
    prdBookIDFilter = '';
    pyRecBookIDFilter = '';
    adjBookIDFilter = '';
    asmBookIDFilter = '';
    wsBookIDFilter = '';
    dsBookIDFilter = '';
    maxCurrentLocIDFilter : number;
		maxCurrentLocIDFilterEmpty : number;
		minCurrentLocIDFilter : number;
		minCurrentLocIDFilterEmpty : number;
    opt4Filter = '';
    opt5Filter = '';
    createdByFilter = '';
    maxCreateadOnFilter : moment.Moment;
        minCreateadOnFilter : moment.Moment;
        
        disableCreateButton=false;




    constructor(
        injector: Injector,
        private _icSetupsService: ICSetupsService,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getICSetups(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._icSetupsService.getAll(
            this.filterText,
            this.segment1Filter,
            this.segment2Filter,
            this.segment3Filter,
            this.maxErrSrNoFilter == null ? this.maxErrSrNoFilterEmpty: this.maxErrSrNoFilter,
            this.minErrSrNoFilter == null ? this.minErrSrNoFilterEmpty: this.minErrSrNoFilter,
            this.maxCostingMethodFilter == null ? this.maxCostingMethodFilterEmpty: this.maxCostingMethodFilter,
            this.minCostingMethodFilter == null ? this.minCostingMethodFilterEmpty: this.minCostingMethodFilter,
            this.prBookIDFilter,
            this.rtBookIDFilter,
            this.cnsBookIDFilter,
            this.slBookIDFilter,
            this.srBookIDFilter,
            this.trBookIDFilter,
            this.prdBookIDFilter,
            this.pyRecBookIDFilter,
            this.adjBookIDFilter,
            this.asmBookIDFilter,
            this.wsBookIDFilter,
            this.dsBookIDFilter,
            this.maxCurrentLocIDFilter == null ? this.maxCurrentLocIDFilterEmpty: this.maxCurrentLocIDFilter,
            this.minCurrentLocIDFilter == null ? this.minCurrentLocIDFilterEmpty: this.minCurrentLocIDFilter,
            this.opt4Filter,
            this.opt5Filter,
            this.createdByFilter,
            this.maxCreateadOnFilter,
            this.minCreateadOnFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            if (result.totalCount > 0) {
                this.disableCreateButton = true;
            }
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createICSetup(): void {
        this.createOrEditICSetupModal.show();
    }

    deleteICSetup(icSetup: ICSetupDto): void { 
        debugger;
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._icSetupsService.delete(icSetup.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._icSetupsService.getICSetupsToExcel(
        this.filterText,
            this.segment1Filter,
            this.segment2Filter,
            this.segment3Filter,
            this.maxErrSrNoFilter == null ? this.maxErrSrNoFilterEmpty: this.maxErrSrNoFilter,
            this.minErrSrNoFilter == null ? this.minErrSrNoFilterEmpty: this.minErrSrNoFilter,
            this.maxCostingMethodFilter == null ? this.maxCostingMethodFilterEmpty: this.maxCostingMethodFilter,
            this.minCostingMethodFilter == null ? this.minCostingMethodFilterEmpty: this.minCostingMethodFilter,
            this.prBookIDFilter,
            this.rtBookIDFilter,
            this.cnsBookIDFilter,
            this.slBookIDFilter,
            this.srBookIDFilter,
            this.trBookIDFilter,
            this.prdBookIDFilter,
            this.pyRecBookIDFilter,
            this.adjBookIDFilter,
            this.asmBookIDFilter,
            this.wsBookIDFilter,
            this.dsBookIDFilter,
            this.maxCurrentLocIDFilter == null ? this.maxCurrentLocIDFilterEmpty: this.maxCurrentLocIDFilter,
            this.minCurrentLocIDFilter == null ? this.minCurrentLocIDFilterEmpty: this.minCurrentLocIDFilter,
            this.opt4Filter,
            this.opt5Filter,
            this.createdByFilter,
            this.maxCreateadOnFilter,
            this.minCreateadOnFilter
        ) 
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}

