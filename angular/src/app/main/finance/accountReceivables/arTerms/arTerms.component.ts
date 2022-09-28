import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditARTermModalComponent } from './create-or-edit-arTerm-modal.component';
import { ViewARTermModalComponent } from './view-arTerm-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { ARTermsServiceProxy } from '../../shared/services/arTerms.service';
import { ARTermDto } from '../../shared/dto/arTerm-dto';

@Component({
    templateUrl: './arTerms.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ARTermsComponent extends AppComponentBase {

    @ViewChild('createOrEditARTermModal', { static: true }) createOrEditARTermModal: CreateOrEditARTermModalComponent;
    @ViewChild('viewARTermModalComponent', { static: true }) viewARTermModal: ViewARTermModalComponent;
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
    activeFilter = -1;

    TermTypeFilter: number;

    maxID:any;




    constructor(
        injector: Injector,
        private _arTermsServiceProxy: ARTermsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getARTerms(event?: LazyLoadEvent) {
        debugger
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._arTermsServiceProxy.getAll(
            this.filterText,
            this.termdescFilter,
            this.maxTERMRATEFilter == null ? this.maxTERMRATEFilterEmpty: this.maxTERMRATEFilter,
            this.minTERMRATEFilter == null ? this.minTERMRATEFilterEmpty: this.minTERMRATEFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.activeFilter,
            this.TermTypeFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
            
        ).subscribe(result => {debugger;
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            console.log(result.items);
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

    createARTerm(): void {
        this.createOrEditARTermModal.show(null,this.maxID);
    }

    deleteARTerm(apTerm: ARTermDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._arTermsServiceProxy.delete(apTerm.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._arTermsServiceProxy.getARTermsToExcel(
        this.filterText,
            this.termdescFilter,
            this.maxTERMRATEFilter == null ? this.maxTERMRATEFilterEmpty: this.maxTERMRATEFilter,
            this.minTERMRATEFilter == null ? this.minTERMRATEFilterEmpty: this.minTERMRATEFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.activeFilter,
            this.TermTypeFilter
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
