import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InventoryFindersService } from '@app/finders/shared/services/inventoryFinders.service';
import { InventoryFindersDto } from '@app/finders/shared/dtos/inventoryFinders-dto';

@Component({
    selector: 'inventoryLookupTableModal',
    styleUrls: ['./inventory-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './inventory-lookup-table-modal.component.html'
})
export class InventoryLookupTableModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    filterText = '';
    id: string;
    displayName: string;
    target: string;
    filter:string;
    private paramFilter: string;
    pickName: string;
    unitShow = false;
    converShow = false;
    unit: string;
    conver: number;
    rate: number;
    expiryDate: Date |string;
    manfDate:Date|string;
      
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;
    optionShow=false;
    option5: string;

    constructor(
        injector: Injector,
        private _inventoryFindersService: InventoryFindersService
    ) {
        super(injector);
    }

    show(target: string, parmValue?: string,paramValue2?:string): void {
        debugger;
        this.active = true;
        this.unitShow = false;
        this.converShow = false;
        this.optionShow=false;
        this.filterText=parmValue;
        this.paginator.rows = 5;
        this.target = target;
        this.paramFilter=paramValue2;
        this.pickName = this.l('Pick' + target);
        this.getAll();
        this.modal.show();
    }

    getAll(event?: LazyLoadEvent) {
        if (!this.active) {
            return;
        }
        debugger
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._inventoryFindersService.getAllInventoryFindersForLookupTable(
            this.filterText,
            this.target,
            this.paramFilter,
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

    setAndSave(obj: InventoryFindersDto) {
        this.id = obj.id;
        this.displayName = obj.displayName;
        this.unit = obj.unit;
        this.conver = obj.conver;
        this.option5=obj.option5;
        this.rate=obj.rate;
        this.manfDate=obj.manfDate;
        this.expiryDate=obj.expiryDate;
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        // this.modalSave.emit(null);
    }
}
