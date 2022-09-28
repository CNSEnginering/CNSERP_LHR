import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ICLocationDto } from '../shared/dto/ic-locations-dto';

@Component({
    selector: 'viewICLocationModal',
    templateUrl: './view-icLocation-modal.component.html'
})
export class ViewICLocationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: ICLocationDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new ICLocationDto();
    }

    show(item: ICLocationDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
