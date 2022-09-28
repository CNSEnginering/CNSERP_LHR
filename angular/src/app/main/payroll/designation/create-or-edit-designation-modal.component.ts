import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditDesignationDto } from '../shared/dto/designation-dto';
import { DesignationServiceProxy } from '../shared/services/designation-service';

@Component({
    selector: 'createOrEditDesignationModal',
    templateUrl: './create-or-edit-designation-modal.component.html'
})
export class CreateOrEditDesignationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    designation: CreateOrEditDesignationDto = new CreateOrEditDesignationDto();

    createDate: Date;
    audtDate: Date;



    constructor(
        injector: Injector,
        private _designationServiceProxy: DesignationServiceProxy
    ) {
        super(injector);
    }

    show(designationId?: number): void {

        debugger;
        if (!designationId) {
            debugger;
            this.designation = new CreateOrEditDesignationDto();
            this.designation.id = designationId;
            this.designation.active = true;
            this._designationServiceProxy.getMaxDesignationId().subscribe(result => {
                this.designation.designationID = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            debugger;
            this._designationServiceProxy.getDesignationForEdit(designationId).subscribe(result => {
                debugger;
                this.designation = result.designation;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        debugger;


        this.designation.audtDate = moment();
        this.designation.audtUser = this.appSession.user.userName;

        if (!this.designation.id) {
            this.designation.createDate = moment();
            this.designation.createdBy = this.appSession.user.userName;
        }

        this._designationServiceProxy.createOrEdit(this.designation)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                debugger;
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
