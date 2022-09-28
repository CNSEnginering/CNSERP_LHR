import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetEmployeesForViewDto, EmployeesDto } from '../shared/dto/employee-dto';

@Component({
    selector: 'viewEmployeeModal',
    templateUrl: './view-employee-modal.component.html' 
})
export class ViewEmployeeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmployeesForViewDto();
        this.item.employees = new EmployeesDto();
    }

    show(item: GetEmployeesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
