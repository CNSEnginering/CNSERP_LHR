import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { MFACSETDto } from '../../shared/dto/manufacacc.dto';

@Component({
    selector: 'viewManufacAccModal',
    templateUrl: './view-manufacAcc-modal.component.html'
})
export class ViewManufacAccModalComponent extends AppComponentBase {

    @ViewChild('viewModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: MFACSETDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new MFACSETDto();
    }

    show(item: MFACSETDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
