import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetGLBOOKSForViewDto, GLBOOKSDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { denodeify } from 'q';

@Component({
    selector: 'viewGLBOOKSModal',
    templateUrl: './view-glbooks-modal.component.html'
})
export class ViewGLBOOKSModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetGLBOOKSForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        debugger
        this.item = new GetGLBOOKSForViewDto();
        this.item.glbooks = new GLBOOKSDto();
    }

    show(item: GetGLBOOKSForViewDto): void {
        debugger
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
