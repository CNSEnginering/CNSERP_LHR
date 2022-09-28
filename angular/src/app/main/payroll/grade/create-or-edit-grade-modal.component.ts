import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditGradeDto } from '../shared/dto/grade-dto';
import { GradeServiceProxy } from '../shared/services/grade-service';

@Component({
    selector: 'createOrEditGradeModal',
    templateUrl: './create-or-edit-grade-modal.component.html'
})
export class CreateOrEditGradeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    grade: CreateOrEditGradeDto = new CreateOrEditGradeDto();

    createDate: Date;
    audtDate: Date;



    constructor(
        injector: Injector,
        private _gradeServiceProxy: GradeServiceProxy
    ) {
        super(injector);
    }

    show(gradeId?: number): void {

        if (!gradeId) {
            this.grade = new CreateOrEditGradeDto();
            this.grade.id = gradeId;
            this.grade.active = true;

            this._gradeServiceProxy.getMaxGradeId().subscribe(result => {
                this.grade.gradeID = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            this._gradeServiceProxy.getGradeForEdit(gradeId).subscribe(result => {
                this.grade = result.grade;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        debugger;


        this.grade.audtDate = moment();
        this.grade.audtUser = this.appSession.user.userName;

        if (!this.grade.id) {
            this.grade.createDate = moment();
            this.grade.createdBy = this.appSession.user.userName;
        }

        this._gradeServiceProxy.createOrEdit(this.grade)
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
