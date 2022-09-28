import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { APTermsServiceProxy, APTermDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditAPTermModalComponent } from './create-or-edit-apTerm-modal.component';
import { ViewAPTermModalComponent } from './view-apTerm-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './apTerms.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class APTermsComponent extends AppComponentBase {

    @ViewChild('createOrEditAPTermModal', { static: true }) createOrEditAPTermModal: CreateOrEditAPTermModalComponent;
    @ViewChild('viewAPTermModalComponent', { static: true }) viewAPTermModal: ViewAPTermModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    termdescFilter = '';
    maxTERMRATEFilter : number;
		maxTERMRATEFilterEmpty : number;
		minTERMRATEFilter : number;
		minTERMRATEFilterEmpty : number;
    maxAUDTDATEFilter : moment.Moment;
		minAUDTDATEFilter : moment.Moment;
    audtuserFilter = '';
    inactiveFilter = -1;

    TermTypeFilter: number;

    maxID:any;




    constructor(
        injector: Injector,
        private _apTermsServiceProxy: APTermsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getAPTerms(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._apTermsServiceProxy.getAll(
            this.filterText,
            this.termdescFilter,
            this.maxTERMRATEFilter == null ? this.maxTERMRATEFilterEmpty: this.maxTERMRATEFilter,
            this.minTERMRATEFilter == null ? this.minTERMRATEFilterEmpty: this.minTERMRATEFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.inactiveFilter,
            this.TermTypeFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
            
        ).subscribe(result => {debugger;
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            if (result.totalCount == 0) {
                this.maxID = 1
            }
            else {
                this.maxID = result.totalCount + 1;
            }
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createAPTerm(): void {
        this.createOrEditAPTermModal.show(null,this.maxID);
    }

    deleteAPTerm(apTerm: APTermDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._apTermsServiceProxy.delete(apTerm.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._apTermsServiceProxy.getAPTermsToExcel(
        this.filterText,
            this.termdescFilter,
            this.maxTERMRATEFilter == null ? this.maxTERMRATEFilterEmpty: this.maxTERMRATEFilter,
            this.minTERMRATEFilter == null ? this.minTERMRATEFilterEmpty: this.minTERMRATEFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.inactiveFilter,
            this.TermTypeFilter
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
