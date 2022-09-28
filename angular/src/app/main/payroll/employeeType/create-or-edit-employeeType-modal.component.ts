import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditEmployeeTypeDto } from '../shared/dto/employeeType-dto';
import { EmployeeTypeServiceProxy } from '../shared/services/employeeType.service';

@Component({
    selector: 'createOrEditEmployeeTypeModal',
    templateUrl: './create-or-edit-employeeType-modal.component.html'
})
export class CreateOrEditEmployeeTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    employeeType: CreateOrEditEmployeeTypeDto = new CreateOrEditEmployeeTypeDto();

    audtDate: Date;
    createDate: Date;


    constructor(
        injector: Injector,
        private _employeeTypeServiceProxy: EmployeeTypeServiceProxy
    ) {
        super(injector);
    }

    show(employeeTypeId?: number): void {
        this.audtDate = null;
        this.createDate = null;

        if (!employeeTypeId) {
            this.employeeType = new CreateOrEditEmployeeTypeDto();
            this.employeeType.id = employeeTypeId;

            this.employeeType.active = true;

            this._employeeTypeServiceProxy.getMaxTypeId().subscribe(result => {
                this.employeeType.typeID = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            this._employeeTypeServiceProxy.getEmployeeTypeForEdit(employeeTypeId).subscribe(result => {
                this.employeeType = result.employeeType;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
debugger;
        this.employeeType.audtDate = moment();
        this.employeeType.audtUser = this.appSession.user.userName;

        if (!this.employeeType.id) {
            this.employeeType.createDate = moment();
            this.employeeType.createdBy = this.appSession.user.userName;
        }

        this._employeeTypeServiceProxy.createOrEdit(this.employeeType)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

}
