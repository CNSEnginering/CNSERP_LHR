import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetDepartmentForViewDto, DepartmentDto } from '../shared/dto/department-dto';

@Component({
    selector: 'viewDepartmentModal',
    templateUrl: './view-departments-modal.component.html'
})
export class ViewDepartmentModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetDepartmentForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetDepartmentForViewDto();
        this.item.department = new DepartmentDto();
    }

    show(item: GetDepartmentForViewDto): void {
        debugger;
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        debugger;
        this.active = false;
        this.modal.hide();
    }
}
