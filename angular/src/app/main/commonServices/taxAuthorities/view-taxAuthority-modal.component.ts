import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetTaxAuthorityForViewDto, TaxAuthorityDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTaxAuthorityModal',
    templateUrl: './view-taxAuthority-modal.component.html'
})
export class ViewTaxAuthorityModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTaxAuthorityForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetTaxAuthorityForViewDto();
        this.item.taxAuthority = new TaxAuthorityDto();
    }

    show(item: GetTaxAuthorityForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
