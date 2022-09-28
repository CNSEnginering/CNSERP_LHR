import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { PriceListService } from '../shared/services/priceList.service';
import { ChartofControlsServiceProxy, AccountSubLedgersServiceProxy } from '@shared/service-proxies/service-proxies';
import { CostCenterService } from '../shared/services/costCenter.service';
@Component({
    selector: 'CostCenterLookupTableModal',
    styleUrls: ['./costCenter-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './costCenter-lookup-table-modal.component.html'
})
export class CostCenterLookupTableModalComponent extends AppComponentBase {

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
        private _priceListService: PriceListService,
        private _accountServiceProxy: ChartofControlsServiceProxy,
        private _accountSubLedgersServiceProxy: AccountSubLedgersServiceProxy,
        private _costCenterServiceProxy: CostCenterService
    ) {
        super(injector);
    }

    show(type: string): void {
        this.active = true;
        this.filterText = '';
        this.type = type;
        this.getAll();
        this.modal.show();
    }

    getAll(event?: LazyLoadEvent) {
        debugger
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
        switch (this.type) {
            case "Account":
            case "GatePass":
                this._accountServiceProxy.getAll(
                    this.filterText,
                    '',
                    '',
                    this.type == "GatePass" ? 1 : -1,
                    undefined,
                    undefined,
                    undefined,
                    undefined,
                    -1,
                    undefined,
                    undefined,
                    '',
                    undefined,
                    undefined,
                    '',
                    '',
                    '',
                    '',
                    this.sorting,
                    this.skipCount,
                    this.MaxResultCount
                ).subscribe(result => {
                    this.primengTableHelper.totalRecordsCount = result.totalCount;
                    this.primengTableHelper.records = result.items;
                    this.primengTableHelper.hideLoadingIndicator();
                });
                break;
            case "SubAccount":
                this._accountSubLedgersServiceProxy.getAll(
                    this.filterText,
                    this.accId,
                    '',
                    '',
                    '',
                    '',
                    '',
                    '',
                    '',
                    '',
                    '',
                    '',0,0,
                    this.primengTableHelper.getSorting(this.dataTable),
                    this.primengTableHelper.getSkipCount(this.paginator, event),
                    this.primengTableHelper.getMaxResultCount(this.paginator, event)
                ).subscribe(result => {
                    this.primengTableHelper.totalRecordsCount = result.totalCount;
                    this.primengTableHelper.records = result.items;
                    this.primengTableHelper.hideLoadingIndicator();
                });
                break;
            case "CostCenter":
                this._costCenterServiceProxy.getAll(
                    this.filterText,
                    this.sorting,
                    this.skipCount,
                    this.MaxResultCount,
                    "1"
                ).subscribe(result => {
                    this.primengTableHelper.totalRecordsCount = result["result"]["totalCount"];
                    this.primengTableHelper.records = result["result"]["items"];;
                    this.primengTableHelper.hideLoadingIndicator();
                });
                break;
        }

    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    setAndSave(record: any) {
        if (this.type == "Account" || this.type == "GatePass")
            this.data = record.chartofControl;
        else if (this.type == "CostCenter")
            this.data = record.costCenter;
        else
            this.data = record.accountSubLedger;

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
