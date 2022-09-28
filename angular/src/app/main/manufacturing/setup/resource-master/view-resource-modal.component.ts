import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { MFRESMASDto } from '../../shared/dto/resource.dto';

@Component({
    selector: 'viewresourceModal',
    templateUrl: './view-resource-modal.component.html'
})
export class ViewResourceMasterModalComponent extends AppComponentBase {

    @ViewChild('viewModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: MFRESMASDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new MFRESMASDto();
    }

    show(item: MFRESMASDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
