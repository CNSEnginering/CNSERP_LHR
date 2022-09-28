import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetBatchListPreviewForViewDto, BatchListPreviewDto, BatchListPreviewsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import * as moment from 'moment';
//import moment = require('moment');


@Component({
    selector: 'viewBatchListPreviewModal',
    templateUrl: './view-batchListPreview-modal.component.html'
})
export class ViewBatchListPreviewModalComponent extends AppComponentBase {


    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable1: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    filterText: Date;
    DocDate: string;
    maxDocDateFilter: moment.Moment;
    minDocDateFilter: moment.Moment;

    item: GetBatchListPreviewForViewDto;


    constructor(
        injector: Injector,
        private _batchListPreviewsServiceProxy: BatchListPreviewsServiceProxy,
    ) {
        super(injector);
        this.item = new GetBatchListPreviewForViewDto();
        this.item.batchListPreview = new BatchListPreviewDto();
    }

    getBatchListPreviews1(id:number, BookID:string, docDate: moment.Moment) {

        // if (this.primengTableHelper.shouldResetPaging(event)) {
        //     this.paginator.changePage(0);
        //     return;
        // }
        if (moment(new Date()).format("A") === "AM") {
            docDate = moment(docDate);
        } else {
            docDate = moment(docDate).endOf('day');
        }

        this.primengTableHelper.showLoadingIndicator();
        debugger;
        this._batchListPreviewsServiceProxy.getBatchListPreviewForView(
            id,
            BookID,
            docDate
        ).subscribe(result => {
            debugger;
            this.primengTableHelper.records = result;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    show(item: GetBatchListPreviewForViewDto): void {
        debugger;
        this.item = item;

        this.DocDate = this.item.batchListPreview.docDate.format('DD-MMM-YYYY');

        this.getBatchListPreviews1(this.item.batchListPreview.id,this.item.batchListPreview.bookID
            ,this.item.batchListPreview.docDate);
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
