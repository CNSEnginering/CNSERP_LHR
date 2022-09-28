import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { GatePassService } from '../shared/services/gatePass.service';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
@Component({
    selector: 'GatePassLookupTableModal',
    styleUrls: ['./gatePass-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './gatePass-lookup-table-modal.component.html'
})
export class GatePassLookupTableModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    gatePass: any;
    filterText = '';
    id: number;
    data: any;
    sorting: any;
    skipCount: any;
    MaxResultCount: any;
    type:string;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    accId: string = '';
    constructor(
        injector: Injector,
        private _gatePassService: GatePassService
    ) {
        super(injector);
    }

    show(type: string): void {
        debugger
        this.type = type;
        this.data = undefined;
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

        if(this.type == "Inward")
        {
            this._gatePassService.GetOGPNo(
                this.filterText,
                this.sorting,
                this.skipCount,
                this.MaxResultCount
            ).subscribe(data => {
                this.gatePass = data["result"]["items"];
                this.primengTableHelper.totalRecordsCount = data["result"]["totalCount"];
                this.primengTableHelper.records = this.gatePass;
                this.primengTableHelper.hideLoadingIndicator();
            });
        }
        else if (this.type == "Outward")
        {
            this._gatePassService.GetIGPNo(
                this.filterText,
                this.sorting,
                this.skipCount,
                this.MaxResultCount
            ).subscribe(data => {
                this.gatePass = data["result"]["items"];
                this.primengTableHelper.totalRecordsCount = data["result"]["totalCount"];
                this.primengTableHelper.records = this.gatePass;
                this.primengTableHelper.hideLoadingIndicator();
            });
        }
       
    }


    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    setAndSave(record: any) {
        this.data = record.gatePass.id;
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
