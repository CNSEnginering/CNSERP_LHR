import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditSalesReferenceModalComponent } from './create-or-edit-salesReference-modal.component';
import { ViewSalesReferenceModalComponent } from './view-salesReference-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { SalesReferencesServiceProxy } from '../shared/services/salesReference.service';
import { SalesReferenceDto } from '../shared/dtos/salesReference-dto';
import { Input } from '@angular/core';

@Component({
    templateUrl: './salesReferences.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
    selector: 'salesReference'
})
export class SalesReferencesComponent extends AppComponentBase {

    @ViewChild('createOrEditSalesReferenceModal', { static: true }) createOrEditSalesReferenceModal: CreateOrEditSalesReferenceModalComponent;
    @ViewChild('viewSalesReferenceModalComponent', { static: true }) viewSalesReferenceModal: ViewSalesReferenceModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @Input() refType: string;
    advancedFiltersAreShown = false;
    filterText = '';
    maxRefIDFilter: number;
    maxRefIDFilterEmpty: number;
    minRefIDFilter: number;
    minRefIDFilterEmpty: number;
    refNameFilter = '';
    activeFilter = -1;
    maxAUDTDATEFilter: moment.Moment;
    minAUDTDATEFilter: moment.Moment;
    audtuserFilter = '';
    maxCreatedDATEFilter: moment.Moment;
    minCreatedDATEFilter: moment.Moment;
    createdUSERFilter = '';




    constructor(
        injector: Injector,
        private _salesReferencesServiceProxy: SalesReferencesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getSalesReferences(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }
        debugger;
        this.primengTableHelper.showLoadingIndicator();
        this.refType = this.refType == undefined ? 'OE' : this.refType;

        this._salesReferencesServiceProxy.getAll(
            this.filterText,
            this.maxRefIDFilter == null ? this.maxRefIDFilterEmpty : this.maxRefIDFilter,
            this.minRefIDFilter == null ? this.minRefIDFilterEmpty : this.minRefIDFilter,
            this.refNameFilter,
            this.activeFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.maxCreatedDATEFilter,
            this.minCreatedDATEFilter,
            this.createdUSERFilter,
            this.refType,
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

    createSalesReference(): void {
        this.createOrEditSalesReferenceModal.show(this.refType);
    }

    deleteSalesReference(salesReference: SalesReferenceDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._salesReferencesServiceProxy.delete(salesReference.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._salesReferencesServiceProxy.getSalesReferencesToExcel(
            this.filterText,
            this.maxRefIDFilter == null ? this.maxRefIDFilterEmpty : this.maxRefIDFilter,
            this.minRefIDFilter == null ? this.minRefIDFilterEmpty : this.minRefIDFilter,
            this.refNameFilter,
            this.activeFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.maxCreatedDATEFilter,
            this.minCreatedDATEFilter,
            this.createdUSERFilter,
            this.refType
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
