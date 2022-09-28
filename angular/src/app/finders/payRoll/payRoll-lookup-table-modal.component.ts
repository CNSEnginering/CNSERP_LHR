import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PayRollFindersDto } from '../shared/dtos/payRollFinders-dto';
import { PayRollFindersService } from '../shared/services/payRollFinders.service';

@Component({
    selector: 'payRollLookupTableModal',
    styleUrls: ['./payRoll-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './payRoll-lookup-table-modal.component.html'
})
export class PayRollLookupTableModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    filterText = '';
    id: string;
    displayName: string;
    dutyHours: string;
    joiningDate: string;
    target: string;
    paramFilter: string;
    currRate: number;
    accountId: string;
    taxRate: number;
    pickName: string;
    accountIDShow = false;
    currRateShow = false;
    taxRateShow = false;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;

    constructor(
        injector: Injector,
        private _PayRollFindersService: PayRollFindersService
    ) {
        super(injector);
    }

    show(target: string, parmValue?: string): void {
        debugger;
        this.active = true;
        this.paginator.rows = 5;
        this.target = target;
        this.filterText = '';
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

        this._PayRollFindersService.getAllPayRollsFindersForLookupTable(
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

    setAndSave(obj: PayRollFindersDto) {
        debugger;
        this.id = obj.id;
        this.displayName = obj.displayName;
        this.dutyHours = obj.dutyHours;
        this.joiningDate  = obj.joiningDate;
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
