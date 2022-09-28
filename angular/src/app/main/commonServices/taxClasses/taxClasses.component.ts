import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TaxClassesServiceProxy, TaxClassDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTaxClassModalComponent } from './create-or-edit-taxClass-modal.component';
import { ViewTaxClassModalComponent } from './view-taxClass-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './taxClasses.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TaxClassesComponent extends AppComponentBase {

    @ViewChild('createOrEditTaxClassModal', { static: true }) createOrEditTaxClassModal: CreateOrEditTaxClassModalComponent;
    @ViewChild('viewTaxClassModalComponent', { static: true }) viewTaxClassModal: ViewTaxClassModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    taxauthFilter = '';
    classdescFilter = '';
    maxCLASSRATEFilter : number;
		maxCLASSRATEFilterEmpty : number;
		minCLASSRATEFilter : number;
		minCLASSRATEFilterEmpty : number;
    maxTRANSTYPEFilter : number;
		maxTRANSTYPEFilterEmpty : number;
		minTRANSTYPEFilter : number;
		minTRANSTYPEFilterEmpty : number;
    classtypeFilter = '';
    maxAUDTDATEFilter : moment.Moment;
		minAUDTDATEFilter : moment.Moment;
    audtuserFilter = '';
        taxAuthorityTAXAUTHFilter = '';




    constructor(
        injector: Injector,
        private _taxClassesServiceProxy: TaxClassesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getTaxClasses(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._taxClassesServiceProxy.getAll(
            this.filterText,
            this.taxauthFilter,
            this.classdescFilter,
            this.maxCLASSRATEFilter == null ? this.maxCLASSRATEFilterEmpty: this.maxCLASSRATEFilter,
            this.minCLASSRATEFilter == null ? this.minCLASSRATEFilterEmpty: this.minCLASSRATEFilter,
            this.maxTRANSTYPEFilter == null ? this.maxTRANSTYPEFilterEmpty: this.maxTRANSTYPEFilter,
            this.minTRANSTYPEFilter == null ? this.minTRANSTYPEFilterEmpty: this.minTRANSTYPEFilter,
            this.classtypeFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.taxAuthorityTAXAUTHFilter,
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

    createTaxClass(): void {
        this.createOrEditTaxClassModal.show();
    }

    deleteTaxClass(taxClass: TaxClassDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._taxClassesServiceProxy.delete(taxClass.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._taxClassesServiceProxy.getTaxClassesToExcel(
        this.filterText,
            this.taxauthFilter,
            this.classdescFilter,
            this.maxCLASSRATEFilter == null ? this.maxCLASSRATEFilterEmpty: this.maxCLASSRATEFilter,
            this.minCLASSRATEFilter == null ? this.minCLASSRATEFilterEmpty: this.minCLASSRATEFilter,
            this.maxTRANSTYPEFilter == null ? this.maxTRANSTYPEFilterEmpty: this.maxTRANSTYPEFilter,
            this.minTRANSTYPEFilter == null ? this.minTRANSTYPEFilterEmpty: this.minTRANSTYPEFilter,
            this.classtypeFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.taxAuthorityTAXAUTHFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
