import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import {BkTransfersServiceProxy, BkTransferBankLookupTableDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';

@Component({
    selector: 'bkTransferBankLookupTableModal1',
    styleUrls: ['./bkTransfer-bank-lookup-table-modal.component1.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './bkTransfer-bank-lookup-table-modal.component1.html'
})
export class BkTransferBankLookupTableModalComponent1 extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    filterText = '';
    id: number;
    displayName: string;
    address: string;
    bankAccount: string;
    
    @Output() modalSave1: EventEmitter<any> = new EventEmitter<any>();
    
    active = false;
    saving = false;

    constructor(
        injector: Injector,
        private _bkTransfersServiceProxy: BkTransfersServiceProxy
    ) {
        super(injector);
    }

    show(): void {
        this.active = true;
        this.paginator.rows = 5;
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

        this._bkTransfersServiceProxy.getAllBankForLookupTable(
            this.filterText,
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

    setAndSave(bank: BkTransferBankLookupTableDto) {
        this.id = bank.id;
        this.displayName = bank.displayName;
        this.address = bank.address;       
        this.bankAccount = bank.bankAccount;  
        this.active = false;
        this.modal.hide();
        this.modalSave1.emit(null);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this.modalSave1.emit(null);
    }
}
