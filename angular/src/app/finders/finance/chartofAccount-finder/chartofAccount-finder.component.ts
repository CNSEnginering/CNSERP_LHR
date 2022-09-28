import { Component, ViewChild, Output, EventEmitter, Injector, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/table';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import { ReportFilterServiceProxy, BankChartofControlLookupTableDto } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'ChartofAccountFinder',
    templateUrl: './chartofAccount-finder.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class ChartofAccountFinderComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    filterText = '';
    accid: string;
    displayName: string;

    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;

    constructor(
        injector: Injector,
        private _reportFilterService: ReportFilterServiceProxy
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

        this._reportFilterService.getChartofControlForLookupTable(
            this.filterText,
            null,
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

    setAndSave(accountID: BankChartofControlLookupTableDto) {
        debugger;
        this.accid = accountID.id;
        this.displayName = accountID.displayName;
        this.active = false;
        this.modal.hide();
        this.modalSelect.emit(null);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this.modalSelect.emit(null);
    }

}
