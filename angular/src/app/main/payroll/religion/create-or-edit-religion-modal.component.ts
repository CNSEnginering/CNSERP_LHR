import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditReligionDto } from '../shared/dto/religion-dto';
import { ReligionServiceProxy } from '../shared/services/Religion-service';

@Component({
    selector: 'createOrEditReligionModal',
    templateUrl: './create-or-edit-religion-modal.component.html'
})
export class CreateOrEditReligionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    religion: CreateOrEditReligionDto = new CreateOrEditReligionDto();

    createDate: Date;
    audtDate: Date;



    constructor(
        injector: Injector,
        private _religionServiceProxy: ReligionServiceProxy
    ) {
        super(injector);
    }

    show(religionId?: number): void {

        debugger;
        if (!religionId) {
            this.religion = new CreateOrEditReligionDto();
            this.religion.id = religionId;
            this.religion.active = true;
            this._religionServiceProxy.getMaxReligionId().subscribe(result => {
                this.religion.religionID = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            debugger;
            this._religionServiceProxy.getReligionForEdit(religionId).subscribe(result => {
                debugger;
                this.religion = result.religion;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        debugger;


        this.religion.audtDate = moment();
        this.religion.audtUser = this.appSession.user.userName;

        if (!this.religion.id) {
            this.religion.createDate = moment();
            this.religion.createdBy = this.appSession.user.userName;
        }

        this._religionServiceProxy.createOrEdit(this.religion)
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
