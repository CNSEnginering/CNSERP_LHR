import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';

import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditGLAccountsPermissionDto, GLAccountsPermissionsServiceProxy } from '@app/main/finance/shared/services/accountsPermission.service';

@Component({
    selector: 'createOrEditGLAccountsPermissionModal',
    templateUrl: './create-or-edit-glAccountsPermission-modal.component.html'
})
export class CreateOrEditGLAccountsPermissionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    glAccountsPermission: CreateOrEditGLAccountsPermissionDto = new CreateOrEditGLAccountsPermissionDto();

    audtDate: Date;


    constructor(
        injector: Injector,
        private _glAccountsPermissionsServiceProxy: GLAccountsPermissionsServiceProxy
    ) {
        super(injector);
    }

    show(glAccountsPermissionId?: number): void {
    this.audtDate = null;

        if (!glAccountsPermissionId) {
            this.glAccountsPermission = new CreateOrEditGLAccountsPermissionDto();
            this.glAccountsPermission.id = glAccountsPermissionId;

            this.active = true;
            this.modal.show();
        } else {
            this._glAccountsPermissionsServiceProxy.getGLAccountsPermissionForEdit(glAccountsPermissionId).subscribe(result => {
                this.glAccountsPermission = result.glAccountsPermission;

                if (this.glAccountsPermission.audtDate) {
					this.audtDate = this.glAccountsPermission.audtDate.toDate();
                }

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
        if (this.audtDate) {
            if (!this.glAccountsPermission.audtDate) {
                this.glAccountsPermission.audtDate = moment(this.audtDate).startOf('day');
            }
            else {
                this.glAccountsPermission.audtDate = moment(this.audtDate);
            }
        }
        else {
            this.glAccountsPermission.audtDate = null;
        }
            this._glAccountsPermissionsServiceProxy.createOrEdit(this.glAccountsPermission)
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
