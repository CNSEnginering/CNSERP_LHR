import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetGLCONFIGForViewDto, GLCONFIGDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewGLCONFIGModal',
    templateUrl: './view-glconfig-modal.component.html'
})
export class ViewGLCONFIGModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetGLCONFIGForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetGLCONFIGForViewDto();
        this.item.glconfig = new GLCONFIGDto();
    }

    show(item: GetGLCONFIGForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
