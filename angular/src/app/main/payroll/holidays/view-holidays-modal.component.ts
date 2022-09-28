import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetHolidaysForViewDto, HolidaysDto } from '../shared/dto/holidays-dto';

@Component({
    selector: 'viewHolidaysModal',
    templateUrl: './view-holidays-modal.component.html'
})
export class ViewHolidaysModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetHolidaysForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetHolidaysForViewDto();
        this.item.holidays = new HolidaysDto();
    }

    show(item: GetHolidaysForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
