import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InventoryGlLinkDto } from '../shared/dto/inventory-glLink-dto';

@Component({
    selector: 'viewInventoryGlLinkModal',
    templateUrl: './view-InventoryGlLink-modal.component.html'
})
export class ViewInventoryGlLinkComponent extends AppComponentBase {

    @ViewChild('viewInventoryGlLinkModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: InventoryGlLinkDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new InventoryGlLinkDto();
    }

    show(item: InventoryGlLinkDto): void {
        this.item = item["inventoryGlLink"];
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
