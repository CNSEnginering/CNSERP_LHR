import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetDesignationForViewDto, DesignationDto } from '../shared/dto/designation-dto';

@Component({
    selector: 'viewDesignationModal',
    templateUrl: './view-designation-modal.component.html'
})
export class ViewDesignationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    item: GetDesignationForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetDesignationForViewDto();
        this.item.designation = new DesignationDto();
    }

    show(item: GetDesignationForViewDto): void {
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
