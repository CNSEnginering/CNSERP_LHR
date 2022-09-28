import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { InventoryGlLinkService } from '../shared/services/inventory-gl-link.service';
import { InventoryGlLinkDto } from '../shared/dto/inventory-glLink-dto';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { ChartofControlsServiceProxy } from '@shared/service-proxies/service-proxies';
import { ICLocationsService } from '../shared/services/ic-locations.service';
import { IcSegment1ServiceProxy } from '../shared/services/ic-segment1-service';
@Component({
    selector: 'InventoryGlLinkLookupTableModal',
    styleUrls: ['./InventoryGlLink-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './InventoryGlLink-lookup-table-modal.component.html'
})
export class InventoryGlLinkLookupTableModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    data: any;
    type: string = '';
    filterText = '';
    id: number;

    inventoryGlLink: any;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    constructor(
        injector: Injector,
        private _inventoryGlLinkservice: InventoryGlLinkService,
        private _chartofControlsServiceProxy: ChartofControlsServiceProxy,
        private _icLocationsService: ICLocationsService,
        private _IcSegment1ServiceProxy: IcSegment1ServiceProxy,
    ) {
        super(injector);
    }

    show(type: string): void {
        this.active = true;
        this.type = type;
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
        if (this.type == "accRec" || this.type == "accRet" || this.type == "accAdj"
            || this.type == "accCgs" || this.type == "accWip"
        ) {
            this._chartofControlsServiceProxy.getAll(
                this.filterText,
                '',
                '',
                -1,
                undefined,
                undefined,
                undefined,
                undefined,
                -1,
                null,
                null,
                '',
                null,
                null,
                '',
                '',
                '',
                '',
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            ).subscribe(result => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
        }
        else if (this.type == 'locId') {
            this._icLocationsService.getAll(this.filterText,
                undefined,
                undefined,
                '',
                '',
                '',
                '',
                -1,
                -1,
                -1,
                '',
                null,
                null,
                '',
                null,
                null,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)).subscribe(result => {
                    this.primengTableHelper.totalRecordsCount = result.totalCount;
                    this.primengTableHelper.records = result.items;
                    this.primengTableHelper.hideLoadingIndicator();
                });
        }
        else if (this.type == 'segId') {
            this._IcSegment1ServiceProxy.getAll(
                this.filterText,
                '',
                '',
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
              ).subscribe(result => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
              });
        }
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    setAndSave(record: any) {
        if (this.type == "accRec" || this.type == "accRet" || this.type == "accAdj"
            || this.type == "accCgs" || this.type == "accWip"
        ) {
            this.data = record.chartofControl;
        }
        else if(this.type == "segId")
        {
            this.data = record.icSegment;
        }
        else {
            this.data = record;
        }

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
