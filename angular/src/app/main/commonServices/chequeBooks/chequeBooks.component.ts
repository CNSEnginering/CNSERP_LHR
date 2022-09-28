import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditChequeBookModalComponent } from './create-or-edit-chequeBook-modal.component';

import { ViewChequeBookModalComponent } from './view-chequeBook-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { ChequeBooksServiceProxy } from '../shared/services/chequeBooks.service';
import { ChequeBookDto } from '../shared/dto/chequeBooks-dto';

@Component({
    templateUrl: './chequeBooks.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ChequeBooksComponent extends AppComponentBase {

    @ViewChild('createOrEditChequeBookModal', { static: true }) createOrEditChequeBookModal: CreateOrEditChequeBookModalComponent;
    @ViewChild('viewChequeBookModalComponent', { static: true }) viewChequeBookModal: ViewChequeBookModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDocNoFilter: number;
    maxDocNoFilterEmpty: number;
    minDocNoFilter: number;
    minDocNoFilterEmpty: number;
    maxDocDateFilter: moment.Moment;
    minDocDateFilter: moment.Moment;
    bankidFilter = '';
    bankAccNoFilter = '';
    fromChNoFilter = '';
    toChNoFilter = '';
    maxNoofChFilter: number;
    maxNoofChFilterEmpty: number;
    minNoofChFilter: number;
    minNoofChFilterEmpty: number;
    activeFilter = -1;
    audtUserFilter = '';
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    createdByFilter = '';
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;




    constructor(
        injector: Injector,
        private _chequeBooksServiceProxy: ChequeBooksServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getChequeBooks(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._chequeBooksServiceProxy.getAll(
            this.filterText,
            this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty : this.maxDocNoFilter,
            this.minDocNoFilter == null ? this.minDocNoFilterEmpty : this.minDocNoFilter,
            this.maxDocDateFilter,
            this.minDocDateFilter,
            this.bankidFilter,
            this.bankAccNoFilter,
            this.fromChNoFilter,
            this.toChNoFilter,
            this.maxNoofChFilter == null ? this.maxNoofChFilterEmpty : this.maxNoofChFilter,
            this.minNoofChFilter == null ? this.minNoofChFilterEmpty : this.minNoofChFilter,
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

    createChequeBook(): void {
        this.createOrEditChequeBookModal.show(false);
    }


    deleteChequeBook(chequeBook: ChequeBookDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._chequeBooksServiceProxy.delete(chequeBook.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._chequeBooksServiceProxy.getChequeBooksToExcel(
            this.filterText,
            this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty : this.maxDocNoFilter,
            this.minDocNoFilter == null ? this.minDocNoFilterEmpty : this.minDocNoFilter,
            this.maxDocDateFilter,
            this.minDocDateFilter,
            this.bankidFilter,
            this.bankAccNoFilter,
            this.fromChNoFilter,
            this.toChNoFilter,
            this.maxNoofChFilter == null ? this.maxNoofChFilterEmpty : this.maxNoofChFilter,
            this.minNoofChFilter == null ? this.minNoofChFilterEmpty : this.minNoofChFilter,
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
