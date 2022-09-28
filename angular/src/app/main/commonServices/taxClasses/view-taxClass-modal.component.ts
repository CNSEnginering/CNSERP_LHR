import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetTaxClassForViewDto, TaxClassDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTaxClassModal',
    templateUrl: './view-taxClass-modal.component.html'
})
export class ViewTaxClassModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTaxClassForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetTaxClassForViewDto();
        this.item.taxClass = new TaxClassDto();
    }

    show(item: GetTaxClassForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
