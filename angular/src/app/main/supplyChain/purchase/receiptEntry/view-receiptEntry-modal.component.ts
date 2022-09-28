import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ReceiptEntryDto } from '../shared/dtos/receiptEntry-dto';

@Component({
    selector: 'viewReceiptEntryModal',
    templateUrl: './view-receiptEntry-modal.component.html'
})
export class ViewReceiptEntryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: ReceiptEntryDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new ReceiptEntryDto();
    }

    show(item: ReceiptEntryDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
