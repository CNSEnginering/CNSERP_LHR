import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetEmployeeArrearsForViewDto, EmployeeArrearsDto } from '../shared/dto/employeeArrears-dto';

@Component({
    selector: 'viewEmployeeArrearsModal',
    templateUrl: './view-employeeArrears-modal.component.html'
})
export class ViewEmployeeArrearsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeeArrearsForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmployeeArrearsForViewDto();
        this.item.employeeArrears = new EmployeeArrearsDto();
    }

    show(item: GetEmployeeArrearsForViewDto): void {
        debugger;
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
