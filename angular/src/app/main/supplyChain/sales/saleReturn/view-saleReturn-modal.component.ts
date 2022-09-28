import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { SaleReturnDto } from '../shared/dtos/saleReturn-dto';

@Component({
    selector: 'viewSaleReturnModal',
    templateUrl: './view-saleReturn-modal.component.html'
})
export class ViewSaleReturnModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: SaleReturnDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new SaleReturnDto();
    }

    show(item: SaleReturnDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
