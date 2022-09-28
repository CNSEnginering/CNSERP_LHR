import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetAROptionForViewDto, AROptionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewAROptionModal',
    templateUrl: './view-arOption-modal.component.html'
})
export class ViewAROptionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetAROptionForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetAROptionForViewDto();
        this.item.arOption = new AROptionDto();
    }

    show(item: GetAROptionForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
