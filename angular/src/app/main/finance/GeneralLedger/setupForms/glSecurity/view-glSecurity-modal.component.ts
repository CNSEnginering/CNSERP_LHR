import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetGLSecurityHeaderForViewDto } from '@app/main/finance/shared/dto/glSecurityHeader-dto';

@Component({
    selector: 'viewGLSecurityModal',
    templateUrl: './view-glSecurity-modal.component.html'
})
export class ViewGLSecurityModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetGLSecurityHeaderForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetGLSecurityHeaderForViewDto();
    }

    show(item: GetGLSecurityHeaderForViewDto): void {
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
