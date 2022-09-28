import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditCPRDto, CPRServiceProxy } from '@shared/service-proxies/service-proxies';
@Component({
    selector: 'createOrEditCprModal',
    templateUrl: './create-or-edit-cpr-modal.component.html'
})
export class CreateOrEditCprModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    cpr: CreateOrEditCPRDto = new CreateOrEditCPRDto();

    createDate: Date;
    audtDate: Date;



    constructor(
        injector: Injector,
        private _cprServiceProxy: CPRServiceProxy
    ) {
        super(injector);
    }

    show(cprId?: number): void {

        debugger;
        if (!cprId) {
            debugger;
            this.cpr = new CreateOrEditCPRDto();
            this.cpr.id = cprId;
            this.cpr.active = true;
            this._cprServiceProxy.getMaxCprId().subscribe(result => {
                this.cpr.cprId = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            debugger;
            this._cprServiceProxy.getCPRForEdit(cprId).subscribe(result => {
                debugger;
                this.cpr = result.cpr;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        debugger;


        this.cpr.audtDate = moment();
        this.cpr.audtUser = this.appSession.user.userName;

        if (!this.cpr.id) {
            this.cpr.createDate = moment();
            this.cpr.createdBy = this.appSession.user.userName;
        }

        this._cprServiceProxy.createOrEdit(this.cpr)
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
