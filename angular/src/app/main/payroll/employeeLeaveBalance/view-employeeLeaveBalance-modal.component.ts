import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetDesignationForViewDto, DesignationDto } from '../shared/dto/designation-dto';
import { employeeLeaveBalanceDto, GetemployeeLeaveBalanceForViewDto } from '../shared/dto/employeeLeaveBalance-dto';

@Component({
    selector: 'viewEmployeeLeaveBalanceModal',
    templateUrl: './view-employeeLeaveBalance-modal.component.html'
})
export class ViewEmployeeLeaveBalanceModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    item: GetemployeeLeaveBalanceForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetemployeeLeaveBalanceForViewDto();
        this.item.employeeLeavesTotal = new employeeLeaveBalanceDto();
    }

    show(item: GetemployeeLeaveBalanceForViewDto): void {
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
