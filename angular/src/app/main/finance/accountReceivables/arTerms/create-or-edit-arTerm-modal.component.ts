import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditARTermDto } from '../../shared/dto/arTerm-dto';
import { ARTermsServiceProxy } from '../../shared/services/arTerms.service';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';


@Component({
    selector: 'createOrEditARTermModal',
    templateUrl: './create-or-edit-arTerm-modal.component.html'
})
export class CreateOrEditARTermModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    accountName: string;
    active = false;
    saving = false;

    arTermMaxId=0;

    //test

    arTerm: CreateOrEditARTermDto = new CreateOrEditARTermDto();
    audtdate: Date;


    constructor(
        injector: Injector,
        private _arTermsServiceProxy: ARTermsServiceProxy
    ) {
        super(injector);
    }

    show(arTermId?: number,maxId?: number): void {
        this.audtdate = null;
        if (!arTermId) {
            this.arTerm = new CreateOrEditARTermDto();
            this.arTerm.id = arTermId;
            this.arTerm.termId=maxId;
            this.arTerm.active = true;
            // this.arTerm.termType = 1;
            // this.arTerm.taxStatus=1;
            this.active = true;
            this.modal.show();
        } else {
            this._arTermsServiceProxy.getARTermForEdit(arTermId).subscribe(result => {
                this.arTerm = result.arTerm;
                this.accountName = result.arTerm.accountName;
                debugger
                if (this.arTerm.audtDate) {
					this.audtdate = this.arTerm.audtDate.toDate();
                }

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        debugger
            this.saving = true;
            this.arTerm.audtDate =moment();
            this.arTerm.audtUser = this.appSession.user.userName;
            this._arTermsServiceProxy.createOrEdit(this.arTerm)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openModal(type: string) {
        this.FinanceLookupTableModal.id = "";
        this.FinanceLookupTableModal.displayName = "";
        if (type == "ChartOfAccount"){
            this.FinanceLookupTableModal.show("ChartOfAccount");
        }
    }

    getAccLookUpData() {
        if (this.FinanceLookupTableModal.target == "ChartOfAccount") {
            this.arTerm.termAccId = this.FinanceLookupTableModal.id;
            this.accountName = this.FinanceLookupTableModal.displayName;
        }
    }


    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
