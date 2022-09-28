import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { SaleEntryDto } from '../shared/dtos/saleEntry-dto';

@Component({
    selector: 'viewSaleEntryModal',
    templateUrl: './view-saleEntry-modal.component.html'
})
export class ViewSaleEntryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: SaleEntryDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new SaleEntryDto();
    }

    show(item: SaleEntryDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
