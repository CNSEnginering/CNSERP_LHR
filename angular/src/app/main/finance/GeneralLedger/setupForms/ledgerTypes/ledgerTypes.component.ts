import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditLedgerTypeModalComponent } from './create-or-edit-ledgerType-modal.component';
import { ViewLedgerTypeModalComponent } from './view-ledgerType-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { LedgerTypesServiceProxy, LedgerTypeDto } from '@app/main/finance/shared/services/GLLedgerType.service';

@Component({
    templateUrl: './ledgerTypes.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class LedgerTypesComponent extends AppComponentBase {

    @ViewChild('createOrEditLedgerTypeModal', { static: true }) createOrEditLedgerTypeModal: CreateOrEditLedgerTypeModalComponent;
    @ViewChild('viewLedgerTypeModalComponent', { static: true }) viewLedgerTypeModal: ViewLedgerTypeModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    ledgerIDFilter: number;
    ledgerDescFilter = '';
    activeFilter = -1;




    constructor(
        injector: Injector,
        private _ledgerTypesServiceProxy: LedgerTypesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getLedgerTypes(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        debugger;

        this.primengTableHelper.showLoadingIndicator();

        this._ledgerTypesServiceProxy.getAll(
            this.filterText,
            this.ledgerIDFilter == null ? undefined: this.ledgerIDFilter,
            this.ledgerDescFilter,
            this.activeFilter,
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

    createLedgerType(): void {
        this.createOrEditLedgerTypeModal.show();
    }

    deleteLedgerType(ledgerType: LedgerTypeDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._ledgerTypesServiceProxy.delete(ledgerType.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._ledgerTypesServiceProxy.getLedgerTypesToExcel(
            this.filterText,
            this.ledgerIDFilter,
            this.ledgerDescFilter,
            this.activeFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
