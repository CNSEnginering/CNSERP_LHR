import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetEmployeeTypeForViewDto, EmployeeTypeDto } from '../shared/dto/employeeType-dto';

@Component({
    selector: 'viewEmployeeTypeModal',
    templateUrl: './view-employeeType-modal.component.html'
})
export class ViewEmployeeTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeeTypeForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmployeeTypeForViewDto();
        this.item.employeeType = new EmployeeTypeDto();
    }

    show(item: GetEmployeeTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
