import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';

import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditLedgerTypeDto, LedgerTypesServiceProxy } from '@app/main/finance/shared/services/GLLedgerType.service';

@Component({
    selector: 'createOrEditLedgerTypeModal',
    templateUrl: './create-or-edit-ledgerType-modal.component.html'
})
export class CreateOrEditLedgerTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    ledgerType: CreateOrEditLedgerTypeDto = new CreateOrEditLedgerTypeDto();



    constructor(
        injector: Injector,
        private _ledgerTypesServiceProxy: LedgerTypesServiceProxy
    ) {
        super(injector);
    }

    show(ledgerTypeId?: number): void {

        if (!ledgerTypeId) {
            this.ledgerType = new CreateOrEditLedgerTypeDto();
            this.ledgerType.id = ledgerTypeId;

            this.ledgerType.active = true;
            this._ledgerTypesServiceProxy.getMaxLedgerTypeID().subscribe(result => {
                this.ledgerType.ledgerID = result;
            });
            this.active = true;
            this.modal.show();
        } else {
            this._ledgerTypesServiceProxy.getLedgerTypeForEdit(ledgerTypeId).subscribe(result => {
                this.ledgerType = result.ledgerType;

                this.active = true;
                this.modal.show();
            });
        }

    }

    save(): void {
        this.saving = true;

        this._ledgerTypesServiceProxy.createOrEdit(this.ledgerType)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.message.confirm("Press 'Yes' for create new ledger type", this.l('SavedSuccessfully'), (isConfirmed) => {
                    if (isConfirmed) {
                        this.show();
                        this.modalSave.emit(null);
                    }
                    else {
                        //  this.notify.info(this.l('SavedSuccessfully'));
                        this.close();
                        this.modalSave.emit(null);
                    }
                });

            });
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
