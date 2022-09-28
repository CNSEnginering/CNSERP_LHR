import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetFiscalCalenderForViewDto, FiscalCalenderDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewFiscalCalenderModal',
    templateUrl: './view-fiscalCalender-modal.component.html'
})
export class ViewFiscalCalenderModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetFiscalCalenderForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetFiscalCalenderForViewDto();
        this.item.fiscalCalender = new FiscalCalenderDto();
    }

    show(item: GetFiscalCalenderForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
