import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { OpeningDto } from '../shared/dto/opening-dto';

@Component({
    selector: 'viewOpeningModal',
    templateUrl: './view-opening-modal.component.html'
})
export class ViewOpeningModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: OpeningDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new OpeningDto();
    }

    show(item: OpeningDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
