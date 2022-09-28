import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetDeductionTypesForViewDto, DeductionTypesDto } from '../shared/dto/deductionTypes-dto';

@Component({
    selector: 'viewDeductionTypesModal',
    templateUrl: './view-deductionTypes-modal.component.html'
})
export class ViewDeductionTypesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetDeductionTypesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetDeductionTypesForViewDto();
        this.item.deductionTypes = new DeductionTypesDto();
    }

    show(item: GetDeductionTypesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
