import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditEarningTypesDto } from '../shared/dto/earningTypes-dto';
import { EarningTypesServiceProxy } from '../shared/services/earningTypes.service';

@Component({
    selector: 'createOrEditEarningTypesModal',
    templateUrl: './create-or-edit-earningTypes-modal.component.html'
})
export class CreateOrEditEarningTypesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    earningTypes: CreateOrEditEarningTypesDto = new CreateOrEditEarningTypesDto();

    constructor(
        injector: Injector,
        private _earningTypesServiceProxy: EarningTypesServiceProxy
    ) {
        super(injector);
    }

    show(earningTypesId?: number): void {

        if (!earningTypesId) {
            this.earningTypes = new CreateOrEditEarningTypesDto();
            this.earningTypes.id = earningTypesId;

            this.earningTypes.active = true;
            this._earningTypesServiceProxy.getMaxEarningTypeId().subscribe(result => {
                this.earningTypes.typeID = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            this._earningTypesServiceProxy.getEarningTypesForEdit(earningTypesId).subscribe(result => {
                this.earningTypes = result.earningTypes;

                this.active = true;
                this.modal.show();
            });
        }

    }

    save(): void {
        this.saving = true;

        this.earningTypes.audtDate = moment();
        this.earningTypes.audtUser = this.appSession.user.userName;

        if (!this.earningTypes.id) {
            this.earningTypes.createDate = moment();
            this.earningTypes.createdBy = this.appSession.user.userName;
        }

        this._earningTypesServiceProxy.createOrEdit(this.earningTypes)
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
