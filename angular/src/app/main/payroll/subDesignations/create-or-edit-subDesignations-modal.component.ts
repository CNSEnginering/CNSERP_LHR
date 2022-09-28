import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditSubDesignationsDto } from '../shared/dto/subDesignations-dto';
import { SubDesignationsServiceProxy } from '../shared/services/subDesignations.service';

@Component({
    selector: 'createOrEditSubDesignationsModal',
    templateUrl: './create-or-edit-subDesignations-modal.component.html'
})
export class CreateOrEditSubDesignationsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    subDesignations: CreateOrEditSubDesignationsDto = new CreateOrEditSubDesignationsDto();

    constructor(
        injector: Injector,
        private _subDesignationsServiceProxy: SubDesignationsServiceProxy
    ) {
        super(injector);
    }

    show(subDesignationsId?: number): void {

        if (!subDesignationsId) {
            this.subDesignations = new CreateOrEditSubDesignationsDto();
            this.subDesignations.id = subDesignationsId;
            this.subDesignations.active = true;

            this._subDesignationsServiceProxy.getMaxtID().subscribe(result => {
                this.subDesignations.subDesignationID = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            this._subDesignationsServiceProxy.getSubDesignationsForEdit(subDesignationsId).subscribe(result => {
                this.subDesignations = result.subDesignations;

                this.active = true;
                this.modal.show();
            });
        }

    }

    save(): void {
        this.saving = true;

        this.subDesignations.audtDate = moment();
        this.subDesignations.audtUser = this.appSession.user.userName;

        if (!this.subDesignations.id) {
            this.subDesignations.createDate = moment();
            this.subDesignations.createdBy = this.appSession.user.userName;
        }

        this._subDesignationsServiceProxy.createOrEdit(this.subDesignations)
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
