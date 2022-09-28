import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetEmployeeAdvancesForViewDto, EmployeeAdvancesDto } from '../shared/dto/advances-dto';

@Component({
    selector: 'viewEmployeeAdvancesModal',
    templateUrl: './view-employeeAdvances-modal.component.html'
})
export class ViewEmployeeAdvancesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeeAdvancesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmployeeAdvancesForViewDto();
        this.item.employeeAdvances = new EmployeeAdvancesDto();
    }

    show(item: GetEmployeeAdvancesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
