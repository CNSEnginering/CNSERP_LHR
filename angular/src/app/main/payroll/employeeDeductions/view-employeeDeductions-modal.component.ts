import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetEmployeeDeductionsForViewDto, EmployeeDeductionsDto } from '../shared/dto/employeeDeductions-dto';

@Component({
    selector: 'viewEmployeeDeductionsModal',
    templateUrl: './view-employeeDeductions-modal.component.html'
})
export class ViewEmployeeDeductionsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeeDeductionsForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmployeeDeductionsForViewDto();
        this.item.employeeDeductions = new EmployeeDeductionsDto();
    }

    show(item: GetEmployeeDeductionsForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
