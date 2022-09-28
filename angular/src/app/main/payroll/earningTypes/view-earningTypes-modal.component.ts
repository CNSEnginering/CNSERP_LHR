import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetEarningTypesForViewDto, EarningTypesDto } from '../shared/dto/earningTypes-dto';

@Component({
    selector: 'viewEarningTypesModal',
    templateUrl: './view-earningTypes-modal.component.html'
})
export class ViewEarningTypesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEarningTypesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEarningTypesForViewDto();
        this.item.earningTypes = new EarningTypesDto();
    }

    show(item: GetEarningTypesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
