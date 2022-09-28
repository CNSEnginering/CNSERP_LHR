import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { WorkOrderDto } from '../shared/dto/workOrder-dto';

@Component({
    selector: 'viewWorkOrderModal',
    templateUrl: './view-workOrder-modal.component.html'
})
export class ViewWorkOrderModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: WorkOrderDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new WorkOrderDto();
    } 

    show(item: WorkOrderDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
