import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TaxAuthoritiesServiceProxy, TaxAuthorityDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTaxAuthorityModalComponent } from './create-or-edit-taxAuthority-modal.component';
import { ViewTaxAuthorityModalComponent } from './view-taxAuthority-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './taxAuthorities.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TaxAuthoritiesComponent extends AppComponentBase {

    @ViewChild('createOrEditTaxAuthorityModal', { static: true }) createOrEditTaxAuthorityModal: CreateOrEditTaxAuthorityModalComponent;
    @ViewChild('viewTaxAuthorityModalComponent', { static: true }) viewTaxAuthorityModal: ViewTaxAuthorityModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    cmpidFilter = '';
    taxauthFilter = '';
    taxauthdescFilter = '';
    accliabilityFilter = '';
    accrecoverableFilter = '';
    maxRECOVERRATEFilter : number;
		maxRECOVERRATEFilterEmpty : number;
		minRECOVERRATEFilter : number;
		minRECOVERRATEFilterEmpty : number;
    accexpenseFilter = '';
    maxAUDTDATEFilter : moment.Moment;
		minAUDTDATEFilter : moment.Moment;
    audtuserFilter = '';
        companyProfileIdFilter = '';




    constructor(
        injector: Injector,
        private _taxAuthoritiesServiceProxy: TaxAuthoritiesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getTaxAuthorities(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._taxAuthoritiesServiceProxy.getAll(
            this.filterText,
          
            this.taxauthFilter,
            this.taxauthdescFilter,
            this.accliabilityFilter,
            this.accrecoverableFilter,
            this.maxRECOVERRATEFilter == null ? this.maxRECOVERRATEFilterEmpty: this.maxRECOVERRATEFilter,
            this.minRECOVERRATEFilter == null ? this.minRECOVERRATEFilterEmpty: this.minRECOVERRATEFilter,
            this.accexpenseFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.companyProfileIdFilter,
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

    createTaxAuthority(): void {
        this.createOrEditTaxAuthorityModal.show(true);
    }

    deleteTaxAuthority(taxAuthority: TaxAuthorityDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._taxAuthoritiesServiceProxy.delete(taxAuthority.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._taxAuthoritiesServiceProxy.getTaxAuthoritiesToExcel(
        this.filterText,
            this.cmpidFilter,
            this.taxauthFilter,
            this.taxauthdescFilter,
            this.accliabilityFilter,
            this.accrecoverableFilter,
            this.maxRECOVERRATEFilter == null ? this.maxRECOVERRATEFilterEmpty: this.maxRECOVERRATEFilter,
            this.minRECOVERRATEFilter == null ? this.minRECOVERRATEFilterEmpty: this.minRECOVERRATEFilter,
            this.accexpenseFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.companyProfileIdFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
