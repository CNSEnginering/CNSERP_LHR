import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ReceiptReturnDto } from '../shared/dtos/receiptReturn-dto';

@Component({
    selector: 'viewReceiptReturnModal',
    templateUrl: './view-receiptReturn-modal.component.html'
})
export class ViewReceiptReturnModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: ReceiptReturnDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new ReceiptReturnDto();
    }

    show(item: ReceiptReturnDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
