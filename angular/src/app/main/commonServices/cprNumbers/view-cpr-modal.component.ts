import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetCPRForViewDto, CPRDto } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'viewCprModal',
    templateUrl: './view-cpr-modal.component.html'
})
export class ViewCprModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    item: GetCPRForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetCPRForViewDto();
        this.item.cpr = new CPRDto();
    }

    show(item: GetCPRForViewDto): void {
        debugger;
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
