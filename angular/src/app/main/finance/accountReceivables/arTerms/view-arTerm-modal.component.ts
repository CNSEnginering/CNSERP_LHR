import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetAPTermForViewDto, APTermDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewARTermModal',
    templateUrl: './view-arTerm-modal.component.html'
})
export class ViewARTermModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetAPTermForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetAPTermForViewDto();
        this.item.apTerm = new APTermDto();
    }

    show(item: GetAPTermForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
