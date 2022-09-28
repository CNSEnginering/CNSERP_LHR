import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ReorderLevelDto } from '../shared/dto/reorder-levels-dto';

@Component({
    selector: 'viewReorderLevelModal',
    templateUrl: './view-reorderLevel-modal.component.html'
})
export class ViewReorderLevelModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: ReorderLevelDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new ReorderLevelDto();
    }

    show(item: ReorderLevelDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
