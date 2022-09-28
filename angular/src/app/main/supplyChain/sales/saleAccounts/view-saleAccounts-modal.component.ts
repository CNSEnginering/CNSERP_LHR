import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { saleAccountsDto } from '../shared/dtos/saleAccounts-dto';

//import { InventoryGlLinkDto } from '../shared/dto/inventory-glLink-dto';

@Component({
    selector: 'viewSaleAccountsModal',
    templateUrl: './view-SaleAccounts-modal.component.html'
})
export class ViewSaleAccountsComponent extends AppComponentBase {

    @ViewChild('viewInventoryGlLinkModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: saleAccountsDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
    this.item = new saleAccountsDto();
    }

    show(item: saleAccountsDto): void {
        this.item = item["oecoll"];
        console.log(item);
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
