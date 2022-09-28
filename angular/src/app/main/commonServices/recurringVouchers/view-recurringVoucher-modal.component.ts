import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetRecurringVoucherForViewDto, RecurringVoucherDto } from '../shared/dto/recurringVouchers-dto';

@Component({
    selector: 'viewRecurringVoucherModal',
    templateUrl: './view-recurringVoucher-modal.component.html'
})
export class ViewRecurringVoucherModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetRecurringVoucherForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetRecurringVoucherForViewDto();
        this.item.recurringVoucher = new RecurringVoucherDto();
    }

    show(item: GetRecurringVoucherForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
