import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetEmployeeLeavesForViewDto, EmployeeLeavesDto } from '../shared/dto/employeeLeaves-dto';

@Component({
    selector: 'viewEmployeeLeavesModal',
    templateUrl: './view-employeeLeaves-modal.component.html'
})
export class ViewEmployeeLeavesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeeLeavesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmployeeLeavesForViewDto();
        this.item.employeeLeaves = new EmployeeLeavesDto();
    }

    show(item: GetEmployeeLeavesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
