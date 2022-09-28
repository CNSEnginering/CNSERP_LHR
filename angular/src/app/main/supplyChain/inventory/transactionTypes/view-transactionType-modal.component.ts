import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TransactionTypeDto } from '../shared/dto/transaction-types-dto';

@Component({
    selector: 'viewTransactionTypeModal',
    templateUrl: './view-transactionType-modal.component.html'
})
export class ViewTransactionTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: TransactionTypeDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new TransactionTypeDto();
    }

    show(item: TransactionTypeDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
