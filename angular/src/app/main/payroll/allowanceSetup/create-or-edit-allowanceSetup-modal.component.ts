import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditAllowanceSetupDto } from '../shared/dto/allowanceSetup-dto';
import { AllowanceSetupServiceProxy } from '../shared/services/allowanceSetup.service';
import { result } from 'lodash';

@Component({
    selector: 'createOrEditAllowanceSetupModal',
    templateUrl: './create-or-edit-allowanceSetup-modal.component.html'
})
export class CreateOrEditAllowanceSetupModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    allowanceSetup: CreateOrEditAllowanceSetupDto = new CreateOrEditAllowanceSetupDto();

    constructor(
        injector: Injector,
        private _allowanceSetupServiceProxy: AllowanceSetupServiceProxy
    ) {
        super(injector);
    }

    show(allowanceSetupId?: number): void {

        if (!allowanceSetupId) {
            this.allowanceSetup = new CreateOrEditAllowanceSetupDto();
            this.allowanceSetup.id = allowanceSetupId;
            this.allowanceSetup.active = true;
            this.allowanceSetup.repairRate = 0.70;
            this.allowanceSetup.milageRate = 40;
            this.allowanceSetup.fuelDate= new Date();

            this._allowanceSetupServiceProxy.getMaxtID().subscribe(result => {
                this.allowanceSetup.docID = result;
            });
            debugger
           
            this.active = true;
            this.modal.show();
        } else {
            this._allowanceSetupServiceProxy.getAllowanceSetupForEdit(allowanceSetupId).subscribe(result => {
                debugger
                this.allowanceSetup = result.allowanceSetup;
                if(result.allowanceSetup.fuelDate != undefined)
                {
                    this.allowanceSetup.fuelDate=new Date(result.allowanceSetup.fuelDate);
                }
                this.PerlitrFul();
                this.active = true;
                this.modal.show();
            });
        }

    }
    PerlitrFul(){
        if(this.allowanceSetup.milageRate!=undefined && this.allowanceSetup.fuelRate){
         this.allowanceSetup.perLiterMilage=this.allowanceSetup.fuelRate/this.allowanceSetup.milageRate;
        }
    }
    save(): void {
        this.saving = true;
debugger
        this.allowanceSetup.audtDate = moment();
        this.allowanceSetup.audtUser = this.appSession.user.userName;

        if (!this.allowanceSetup.id) {
            this.allowanceSetup.createDate = moment();
            this.allowanceSetup.createdBy = this.appSession.user.userName;
        }

        this._allowanceSetupServiceProxy.createOrEdit(this.allowanceSetup)
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
