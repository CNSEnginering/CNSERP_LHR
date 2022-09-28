import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetEmployeeSalaryForViewDto, EmployeeSalaryDto } from '../shared/dto/employeeSalary-dto';

@Component({
    selector: 'viewEmployeeSalaryModal',
    templateUrl: './view-employeeSalary-modal.component.html'
})
export class ViewEmployeeSalaryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeeSalaryForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmployeeSalaryForViewDto();
        this.item.employeeSalary = new EmployeeSalaryDto();
    }

    show(item: GetEmployeeSalaryForViewDto): void {
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
