import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetGLTRHeaderForViewDto, GLTRHeaderDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJVEntryModal',
    templateUrl: './view-jvEntry-modal.component.html'
})
export class ViewJVEntryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    

    item: GetGLTRHeaderForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetGLTRHeaderForViewDto();
        this.item.gltrHeader = new GLTRHeaderDto();
    }

    show(item: GetGLTRHeaderForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
