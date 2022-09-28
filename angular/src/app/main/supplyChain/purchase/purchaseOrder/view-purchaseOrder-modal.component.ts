import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PurchaseOrderDto } from '../shared/dtos/purchaseOrder-dto';

@Component({
    selector: 'viewPurchaseOrderModal',
    templateUrl: './view-purchaseOrder-modal.component.html'
})
export class ViewPurchaseOrderModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: PurchaseOrderDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new PurchaseOrderDto();
    }

    show(item: PurchaseOrderDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
