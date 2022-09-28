import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AdjustmentDto } from '../shared/dto/adjustment-dto';

@Component({
    selector: 'viewAdjustmentModal',
    templateUrl: './view-adjustment-modal.component.html'
})
export class ViewAdjustmentModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: AdjustmentDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new AdjustmentDto();
    }

    show(item: AdjustmentDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
