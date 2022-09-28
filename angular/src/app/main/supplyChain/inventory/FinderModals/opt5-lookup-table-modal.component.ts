import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { ICOPT5ServiceProxy } from '../shared/services/ic-opt5-service';
@Component({
    selector: 'opt5LookupTableModal',
    styleUrls: ['./opt5-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './opt5-lookup-table-modal.component.html'
})
export class Opt5LookupTableModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    type: string = '';
    filterText = '';
    id: number;
    data: any;
    sorting: any;
    skipCount: any;
    MaxResultCount: any;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    accId: string = '';
    constructor(
        injector: Injector,
        private _icopT5ServiceProxy: ICOPT5ServiceProxy,
    ) {
        super(injector);
    }

    show(): void {
        this.active = true;
        this.filterText = '';
        this.getAll();
        this.modal.show();
    }

    getAll(event?: LazyLoadEvent) {
        if (!this.active) {
            return;
        }

        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.sorting = this.primengTableHelper.getSorting(this.dataTable);
        this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event);
        this.MaxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event);

        this.primengTableHelper.showLoadingIndicator();
        this._icopT5ServiceProxy.getAll(
            this.filterText,
            undefined,
            undefined,
            undefined,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            console.log(result);
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    setAndSave(record: any) {
        this.data = record.iCOPT5;
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }
}
