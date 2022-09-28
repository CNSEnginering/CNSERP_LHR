import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BanksServiceProxy, BankDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditBankModalComponent } from './create-or-edit-bank-modal.component';
import { ViewBankModalComponent } from './view-bank-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './banks.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class BanksComponent extends AppComponentBase {

    @ViewChild('createOrEditBankModal', { static: true }) createOrEditBankModal: CreateOrEditBankModalComponent;
    @ViewChild('viewBankModalComponent', { static: true }) viewBankModal: ViewBankModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    cmpidFilter = '';
    bankidFilter = '';
    banknameFilter = '';
    branchNameFilter = '';
    addR1Filter = '';
    addR2Filter = '';
    addR3Filter = '';
    addR4Filter = '';
    cityFilter = '';
    stateFilter = '';
    countryFilter = '';
    postalFilter = '';
    contactFilter = '';
    phoneFilter = '';
    faxFilter = '';
    inactiveFilter = -1;
    maxINACTDATEFilter : moment.Moment;
		minINACTDATEFilter : moment.Moment;
    bkacctnumberFilter = '';
    idacctbankFilter = '';
    idacctwoffFilter = '';
    idacctcrcardFilter = '';
    maxAUDTDATEFilter : moment.Moment;
		minAUDTDATEFilter : moment.Moment;
    audtuserFilter = '';
        chartofControlIdFilter = '';




    constructor(
        injector: Injector,
        private _banksServiceProxy: BanksServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getBanks(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._banksServiceProxy.getAll(
            this.filterText,
            this.cmpidFilter,
            this.bankidFilter,
            this.banknameFilter,
            this.branchNameFilter,
            this.addR1Filter,
            this.addR2Filter,
            this.addR3Filter,
            this.addR4Filter,
            this.cityFilter,
            this.stateFilter,
            this.countryFilter,
            this.postalFilter,
            this.contactFilter,
            this.phoneFilter,
            this.faxFilter,
            this.inactiveFilter,
            this.maxINACTDATEFilter,
            this.minINACTDATEFilter,
            this.bkacctnumberFilter,
            this.idacctbankFilter,
            this.idacctwoffFilter,
            this.idacctcrcardFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.chartofControlIdFilter,
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

    createBank(): void {
        this.createOrEditBankModal.show();
    }

    deleteBank(bank: BankDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._banksServiceProxy.delete(bank.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._banksServiceProxy.getBanksToExcel(
        this.filterText,
            this.cmpidFilter,
            this.bankidFilter,
            this.banknameFilter,
            this.addR1Filter,
            this.addR2Filter,
            this.addR3Filter,
            this.addR4Filter,
            this.cityFilter,
            this.stateFilter,
            this.countryFilter,
            this.postalFilter,
            this.contactFilter,
            this.phoneFilter,
            this.faxFilter,
            this.inactiveFilter,
            this.maxINACTDATEFilter,
            this.minINACTDATEFilter,
            this.bkacctnumberFilter,
            this.idacctbankFilter,
            this.idacctwoffFilter,
            this.idacctcrcardFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.chartofControlIdFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
