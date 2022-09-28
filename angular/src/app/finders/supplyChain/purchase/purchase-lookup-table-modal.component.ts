import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PurchaseFindersService } from '@app/finders/shared/services/purchaseFinders.service';
import { PurchaseFindersDto } from '@app/finders/shared/dtos/purchaseFinders-dto';

@Component({
    selector: 'purchaseLookupTableModal',
    styleUrls: ['./purchase-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './purchase-lookup-table-modal.component.html'
})
export class PurchaseLookupTableModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    filterText = '';
    id: string;
    displayName: string;
    target: string;
    private paramFilter: string;
    pickName: string;
    locationShow = false;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;
    location: string;
    IdName='Id';

    constructor(
        injector: Injector,
        private _purchaseFindersService: PurchaseFindersService
    ) {
        super(injector);
    }

    show(target: string, parmValue?: string): void {
        debugger;
        this.active = true;
        this.locationShow = false;
        this.filterText='';
        this.paginator.rows = 5;
        this.target = target;
        switch (target) {
            case "PONo":
                this.paramFilter=parmValue;
                this.IdName=this.l('PONo');
                this.locationShow=true;
                break;
            case "ReceiptNo":
                this.paramFilter=parmValue;
                this.IdName=this.l('ReceiptNo');
                this.locationShow=true;
                break;
            default:
                break;
        }
        this.pickName = this.l('Pick' + target);
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

        this.primengTableHelper.showLoadingIndicator();

        this._purchaseFindersService.getAllPurchaseFindersForLookupTable(
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

    setAndSave(obj: PurchaseFindersDto) {
        this.id = obj.id;
        this.displayName = obj.displayName;
        this.location=obj.location;
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
