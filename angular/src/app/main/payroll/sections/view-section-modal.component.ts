import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetSectionForViewDto, SectionDto } from '../shared/dto/section-dto';

@Component({
    selector: 'viewSectionModal',
    templateUrl: './view-section-modal.component.html'
})
export class ViewSectionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetSectionForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetSectionForViewDto();
        this.item.section = new SectionDto();
    }

    show(item: GetSectionForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
