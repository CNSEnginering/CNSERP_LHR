import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditEducationDto } from '../shared/dto/education-dto';
import { EducationServiceProxy } from '../shared/services/education.service';

@Component({
    selector: 'createOrEditEducationModal',
    templateUrl: './create-or-edit-education-modal.component.html'
})
export class CreateOrEditEducationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    education: CreateOrEditEducationDto = new CreateOrEditEducationDto();

    audtDate: Date;
    createDate: Date;


    constructor(
        injector: Injector,
        private _educationServiceProxy: EducationServiceProxy
    ) {
        super(injector);
    }

    show(educationId?: number): void {
    this.audtDate = null;
    this.createDate = null;

        if (!educationId) {
            this.education = new CreateOrEditEducationDto();
            this.education.id = educationId;

            this.education.active=true;
            this._educationServiceProxy.getMaxEducationId().subscribe(result => {
                this.education.edID = result;
            });
            this.active = true;
            this.modal.show();
        } else {
            this._educationServiceProxy.getEducationForEdit(educationId).subscribe(result => {
                this.education = result.education;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

            this.education.audtDate=moment();
            this.education.audtUser=this.appSession.user.userName;
    
            if (!this.education.id) {
                this.education.createDate = moment();
                this.education.createdBy = this.appSession.user.userName;
            }
        
            this._educationServiceProxy.createOrEdit(this.education)
             .pipe(finalize(() => { this.saving = false;}))
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
