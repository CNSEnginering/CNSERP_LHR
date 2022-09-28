import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { IcItemServiceProxy } from '@app/main/supplyChain/inventory/shared/services/ic-Item.service';
@Component({
    selector: 'ItemLookupTableModal',
    styleUrls: ['./item-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './item-lookup-table-modal.component.html'
})
export class ItemLookupTableModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    type: string = '';
    filterText = '';
    id: number;
    priceList: any;
    sorting: any;
    skipCount: any;
    MaxResultCount: any;
    data: any;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    constructor(
        injector: Injector,
        private _IcItemServiceProxy: IcItemServiceProxy
    ) {
        super(injector);
    }

    show(): void {
        debugger
        this.active = true;
        //this.paginator.rows = 5;
        //this.type = type;
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
                this._IcItemServiceProxy.getAll(
                    this.filterText,
                    '',
                    '',
                    '',
                    '',
                    '',
                    undefined,
                    undefined,
                    undefined,
                    '',
                    undefined,
                    undefined,
                    undefined,
                    undefined,
                    undefined,
                    '',
                    this.sorting,
                    this.skipCount,
                    this.MaxResultCount
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
        if (this.type == "PriceList")
            this.data = record.priceList;
        else if (this.type == "GatePassItem")
            this.data = record.icItem
        else
            this.data = record.icItem;

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
