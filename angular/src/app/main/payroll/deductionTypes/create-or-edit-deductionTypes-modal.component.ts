import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditDeductionTypesDto } from '../shared/dto/deductionTypes-dto';
import { DeductionTypesServiceProxy } from '../shared/services/deductionTypes.service';

@Component({
    selector: 'createOrEditDeductionTypesModal',
    templateUrl: './create-or-edit-deductionTypes-modal.component.html'
})
export class CreateOrEditDeductionTypesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    deductionTypes: CreateOrEditDeductionTypesDto = new CreateOrEditDeductionTypesDto();

    constructor(
        injector: Injector,
        private _deductionTypesServiceProxy: DeductionTypesServiceProxy
    ) {
        super(injector);
    }

    show(deductionTypesId?: number): void {

        if (!deductionTypesId) {
            this.deductionTypes = new CreateOrEditDeductionTypesDto();
            this.deductionTypes.id = deductionTypesId;

            this.deductionTypes.active = true;
            this._deductionTypesServiceProxy.getMaxDeductionTypeId().subscribe(result => {
                this.deductionTypes.typeID = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            this._deductionTypesServiceProxy.getDeductionTypesForEdit(deductionTypesId).subscribe(result => {
                this.deductionTypes = result.deductionTypes;

                this.active = true;
                this.modal.show();
            });
        }

    }

    save(): void {
        this.saving = true;

        this.deductionTypes.audtDate = moment();
        this.deductionTypes.audtUser = this.appSession.user.userName;

        if (!this.deductionTypes.id) {
            this.deductionTypes.createDate = moment();
            this.deductionTypes.createdBy = this.appSession.user.userName;
        }

        this._deductionTypesServiceProxy.createOrEdit(this.deductionTypes)
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
