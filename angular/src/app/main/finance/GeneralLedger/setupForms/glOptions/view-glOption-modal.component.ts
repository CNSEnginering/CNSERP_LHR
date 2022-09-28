import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetGLOptionForViewDto, GLOptionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewGLOptionModal',
    templateUrl: './view-glOption-modal.component.html'
})
export class ViewGLOptionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetGLOptionForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetGLOptionForViewDto();
        this.item.glOption = new GLOptionDto();
    }

    show(item: GetGLOptionForViewDto): void {debugger;
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
