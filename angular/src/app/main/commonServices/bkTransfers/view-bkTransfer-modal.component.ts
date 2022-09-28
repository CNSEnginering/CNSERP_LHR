import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetBkTransferForViewDto, BkTransferDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBkTransferModal',
    templateUrl: './view-bkTransfer-modal.component.html'
})
export class ViewBkTransferModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBkTransferForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetBkTransferForViewDto();
        this.item.bkTransfer = new BkTransferDto();
    }

    show(item: GetBkTransferForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
