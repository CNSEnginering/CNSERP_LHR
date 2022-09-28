import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { APTermsServiceProxy, CreateOrEditAPTermDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';


@Component({
    selector: 'createOrEditAPTermModal',
    templateUrl: './create-or-edit-apTerm-modal.component.html'
})
export class CreateOrEditAPTermModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    apTermMaxId=0;



    //test
    

    apTerm: CreateOrEditAPTermDto = new CreateOrEditAPTermDto();

            audtdate: Date;


    constructor(
        injector: Injector,
        private _apTermsServiceProxy: APTermsServiceProxy
    ) {
        super(injector);
    }

    show(apTermId?: number,maxId?: number): void {
        debugger;
        this.audtdate = null;
        if (!apTermId) {
            this.apTerm = new CreateOrEditAPTermDto();
            this.apTerm.id = apTermId;
            this.apTermMaxId=maxId;
            this.apTerm.termType = 1;
            this.apTerm.taxStatus=1;
            this.active = true;
            this.modal.show();
        } else {
            this._apTermsServiceProxy.getAPTermForEdit(apTermId).subscribe(result => {
                this.apTerm = result.apTerm;

                if (this.apTerm.audtdate) {
					this.audtdate = this.apTerm.audtdate.toDate();
                }

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;
debugger;
			
            this.apTerm.audtdate =moment();
            this.apTerm.audtuser = this.appSession.user.userName;
            this._apTermsServiceProxy.createOrEdit(this.apTerm)
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
