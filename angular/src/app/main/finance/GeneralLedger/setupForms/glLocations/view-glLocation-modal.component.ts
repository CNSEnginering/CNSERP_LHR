import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetGLLocationForViewDto, GLLocationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewGLLocationModal',
    templateUrl: './view-glLocation-modal.component.html'
})
export class ViewGLLocationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetGLLocationForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetGLLocationForViewDto();
        this.item.glLocation = new GLLocationDto();
    }

    show(item: GetGLLocationForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
