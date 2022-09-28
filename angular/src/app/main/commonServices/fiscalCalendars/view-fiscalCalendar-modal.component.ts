import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetFiscalCalendarForViewDto, FiscalCalendarDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewFiscalCalendarModal',
    templateUrl: './view-fiscalCalendar-modal.component.html'
})
export class ViewFiscalCalendarModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetFiscalCalendarForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetFiscalCalendarForViewDto();
        this.item.fiscalCalendar = new FiscalCalendarDto();
    }

    show(item: GetFiscalCalendarForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
