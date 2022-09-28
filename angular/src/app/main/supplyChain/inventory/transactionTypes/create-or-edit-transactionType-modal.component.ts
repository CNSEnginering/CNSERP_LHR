import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { TransactionTypeDto } from '../shared/dto/transaction-types-dto';
import { TransactionTypesService } from '../shared/services/transaction-types.service';

@Component({
    selector: 'createOrEditTransactionTypeModal',
    templateUrl: './create-or-edit-transactionType-modal.component.html'
})
export class CreateOrEditTransactionTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    transactionType: TransactionTypeDto = new TransactionTypeDto();

    createDate: Date;
    audtDate: Date;


    constructor(
        injector: Injector,
        private _transactionTypesService: TransactionTypesService
    ) {
        super(injector);
    }

    show(transactionTypeId?: number): void {
    this.createDate = null;
    this.audtDate = null;

        if (!transactionTypeId) {
            this.transactionType = new TransactionTypeDto();
            this.transactionType.id = transactionTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._transactionTypesService.getTransactionTypeForEdit(transactionTypeId).subscribe(result => {
                this.transactionType = result;

                if (this.transactionType.createDate) {
					this.createDate = this.transactionType.createDate.toDate();
                }
                if (this.transactionType.audtDate) {
					this.audtDate = this.transactionType.audtDate.toDate();
                }

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
        if (this.createDate) {
            if (!this.transactionType.createDate) {
                this.transactionType.createDate = moment(this.createDate).startOf('day');
            }
            else {
                this.transactionType.createDate = moment(this.createDate);
            }
        }
        else {
            this.transactionType.createDate = null;
        }
        if (this.audtDate) {
            if (!this.transactionType.audtDate) {
                this.transactionType.audtDate = moment(this.audtDate).startOf('day');
            }
            else {
                this.transactionType.audtDate = moment(this.audtDate);
            }
        }
        else {
            this.transactionType.audtDate = null;
        }

        this.transactionType.audtDate=moment();
        this.transactionType.audtUser = this.appSession.user.userName;

        if(!this.transactionType.id){
            this.transactionType.createDate=moment();
            this.transactionType.createdBy = this.appSession.user.userName;
        }
            this._transactionTypesService.createOrEdit(this.transactionType)
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
