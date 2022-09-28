import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PriceListDto } from '../shared/dto/priceList-dto';



@Component({
    selector: 'viewPriceListModal',
    templateUrl: './view-priceList-modal.component.html'
})
export class ViewPriceListComponent extends AppComponentBase {

    @ViewChild('viewPriceListModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: PriceListDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new PriceListDto();
    }

    show(item: PriceListDto): void {
        this.item = <any>item["priceList"];
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
