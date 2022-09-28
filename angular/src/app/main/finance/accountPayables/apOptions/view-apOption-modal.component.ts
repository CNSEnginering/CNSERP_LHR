import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetAPOptionForViewDto, APOptionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewAPOptionModal',
    templateUrl: './view-apOption-modal.component.html'
})
export class ViewAPOptionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetAPOptionForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetAPOptionForViewDto();
        this.item.apOption = new APOptionDto();
    }

    show(item: GetAPOptionForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
