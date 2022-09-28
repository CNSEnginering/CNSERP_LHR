import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GLINVHeaderDto } from '../../shared/dto/glinvHeader-dto';

@Component({
    selector: 'viewDirectInvoiceModal',
    templateUrl: './view-directInvoice-modal.component.html'
})
export class ViewDirectInvoiceModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    

    item: GLINVHeaderDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item= new GLINVHeaderDto();
    }

    show(item: GLINVHeaderDto): void {debugger
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
