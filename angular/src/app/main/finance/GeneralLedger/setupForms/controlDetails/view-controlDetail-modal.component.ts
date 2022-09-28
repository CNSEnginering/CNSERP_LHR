import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetControlDetailForViewDto, ControlDetailDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewControlDetailModal',
    templateUrl: './view-controlDetail-modal.component.html'
})
export class ViewControlDetailModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetControlDetailForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetControlDetailForViewDto();
        this.item.controlDetail = new ControlDetailDto();
    }

    show(item: GetControlDetailForViewDto): void {
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
