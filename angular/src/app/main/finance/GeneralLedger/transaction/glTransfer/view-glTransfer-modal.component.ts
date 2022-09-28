import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetGLTransferForViewDto, GLTransferDto } from '@app/main/finance/shared/dto/glTransfer-dto';

@Component({
    selector: 'viewGLTransferModal',
    templateUrl: './view-glTransfer-modal.component.html'
})
export class ViewGLTransferModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    item: GetGLTransferForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetGLTransferForViewDto();
        this.item.glTransfer = new GLTransferDto();
    }

    show(item: GetGLTransferForViewDto): void {
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
