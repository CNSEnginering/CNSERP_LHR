import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ConsumptionDto } from '../shared/dto/consumption-dto';

@Component({
    selector: 'viewConsumptionModal',
    templateUrl: './view-consumption-modal.component.html'
})
export class ViewConsumptionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: ConsumptionDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new ConsumptionDto();
    } 

    show(item: ConsumptionDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
