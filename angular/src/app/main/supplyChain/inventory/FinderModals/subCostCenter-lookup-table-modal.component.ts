import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { CostCenterService } from '../shared/services/costCenter.service';
import { SubCostCenterService } from '../shared/services/subCostCenter.service';
import { SubCostCenterDto } from '../shared/dto/subCostCenter-dto';
@Component({
    selector: 'SubCostCenterLookupTableModal',
    styleUrls: ['./subCostCenter-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './subCostCenter-lookup-table-modal.component.html'
})
export class SubCostCenterLookupTableModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    filterText = '';
    ccidFilter :string;
    subccid: number;
    subCCName:string;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    constructor(
        injector: Injector,
        private _subCostCenterServiceProxy: SubCostCenterService
    ) {
        super(injector);
    }

    show(ccid:string): void {
        this.active = true;
        this.ccidFilter=ccid;
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

        this._subCostCenterServiceProxy.getAllSubCostCenterForLookupTable(
            this.filterText,
            this.ccidFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {debugger;
            this.primengTableHelper.totalRecordsCount = result['result']['totalCount'];
            this.primengTableHelper.records =result['result']['items'];
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void { 
        this.paginator.changePage(this.paginator.getPage());
    }

    setAndSave(record: SubCostCenterDto) {debugger;
        this.subccid = record.subccid;
        this.subCCName = record.subCCName;
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
