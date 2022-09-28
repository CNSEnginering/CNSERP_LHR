import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';

import { AppComponentBase } from '@shared/common/app-component-base';
import { GetLedgerTypeForViewDto, LedgerTypeDto } from '@app/main/finance/shared/services/GLLedgerType.service';

@Component({
    selector: 'viewLedgerTypeModal',
    templateUrl: './view-ledgerType-modal.component.html'
})
export class ViewLedgerTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetLedgerTypeForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetLedgerTypeForViewDto();
        this.item.ledgerType = new LedgerTypeDto();
    }

    show(item: GetLedgerTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
