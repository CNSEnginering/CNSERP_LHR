import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetEmployeeEarningsForViewDto, EmployeeEarningsDto } from '../shared/dto/employeeEarnings-dto';

@Component({
    selector: 'viewEmployeeEarningsModal',
    templateUrl: './view-employeeEarnings-modal.component.html'
})
export class ViewEmployeeEarningsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeeEarningsForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmployeeEarningsForViewDto();
        this.item.employeeEarnings = new EmployeeEarningsDto();
    }

    show(item: GetEmployeeEarningsForViewDto): void {
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
