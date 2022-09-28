import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetGLAccountsPermissionForViewDto, GLAccountsPermissionDto } from '@app/main/finance/shared/services/accountsPermission.service';

@Component({
    selector: 'viewGLAccountsPermissionModal',
    templateUrl: './view-glAccountsPermission-modal.component.html'
})
export class ViewGLAccountsPermissionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetGLAccountsPermissionForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetGLAccountsPermissionForViewDto();
        this.item.glAccountsPermission = new GLAccountsPermissionDto();
    }

    show(item: GetGLAccountsPermissionForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
