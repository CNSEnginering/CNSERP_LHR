import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetEducationForViewDto, EducationDto } from '../shared/dto/education-dto';

@Component({
    selector: 'viewEducationModal',
    templateUrl: './view-education-modal.component.html'
})
export class ViewEducationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEducationForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEducationForViewDto();
        this.item.education = new EducationDto();
    }

    show(item: GetEducationForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
