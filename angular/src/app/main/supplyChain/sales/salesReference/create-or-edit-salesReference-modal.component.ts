import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditSalesReferenceDto } from '../shared/dtos/salesReference-dto';
import { SalesReferencesServiceProxy } from '../shared/services/salesReference.service';

@Component({
    selector: 'createOrEditSalesReferenceModal',
    templateUrl: './create-or-edit-salesReference-modal.component.html'
})
export class CreateOrEditSalesReferenceModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    refType: string;
    active = false;
    saving = false;

    salesReference: CreateOrEditSalesReferenceDto = new CreateOrEditSalesReferenceDto();

    constructor(
        injector: Injector,
        private _salesReferencesServiceProxy: SalesReferencesServiceProxy
    ) {
        super(injector);
    }

    show(type: string, salesReferenceId?: number): void {
        this.refType = type;
        if (!salesReferenceId) {
            this.salesReference = new CreateOrEditSalesReferenceDto();
            this.salesReference.id = salesReferenceId;

            this.salesReference.active = true;
            this._salesReferencesServiceProxy.getMaxReferenceId().subscribe(result => {
                this.salesReference.refID = result;
            });
            this.active = true;
            this.modal.show();
        } else {
            this._salesReferencesServiceProxy.getSalesReferenceForEdit(salesReferenceId).subscribe(result => {
                this.salesReference = result.salesReference;

                this.active = true;
                this.modal.show();
            });
        }

    }

    save(): void {
        this.saving = true;

        this.salesReference.audtdate = moment();
        this.salesReference.audtuser = this.appSession.user.userName;

        if (!this.salesReference.id) {
            this.salesReference.createdDATE = moment();
            this.salesReference.createdUSER = this.appSession.user.userName;
        }
        this.salesReference.refType = this.refType == undefined ? 'OE' : this.refType;
        this._salesReferencesServiceProxy.createOrEdit(this.salesReference)
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
