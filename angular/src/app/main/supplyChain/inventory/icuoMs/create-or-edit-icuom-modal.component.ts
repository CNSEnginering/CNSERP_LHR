import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ICUOMDto } from '../shared/dto/ic-uoms-dto';
import { ICUOMsService } from '../shared/services/ic-uoms.service';


@Component({
    selector: 'createOrEditICUOMModal',
    templateUrl: './create-or-edit-icuom-modal.component.html'
})
export class CreateOrEditICUOMModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    icuom: ICUOMDto = new ICUOMDto();

    createDate: Date;
    audtDate: Date;


    constructor(
        injector: Injector,
        private _icuoMsService: ICUOMsService
    ) {
        super(injector);
    }

    show(icuomId?: number): void {
        this.createDate = null;
        this.audtDate = null;

        if (!icuomId) {
            this.icuom = new ICUOMDto();
            this.icuom.id = icuomId;

            this.active = true;
            this.modal.show();
        } else {
            this._icuoMsService.getICUOMForEdit(icuomId).subscribe(result => {
                this.icuom = result;

                if (this.icuom.createDate) {
                    this.createDate = this.icuom.createDate.toDate();
                }
                if (this.icuom.audtDate) {
                    this.audtDate = this.icuom.audtDate.toDate();
                }

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        if (this.createDate) {
            if (!this.icuom.createDate) {
                this.icuom.createDate = moment(this.createDate).startOf('day');
            }
            else {
                this.icuom.createDate = moment(this.createDate);
            }
        }
        else {
            this.icuom.createDate = null;
        }
        if (this.audtDate) {
            if (!this.icuom.audtDate) {
                this.icuom.audtDate = moment(this.audtDate).startOf('day');
            }
            else {
                this.icuom.audtDate = moment(this.audtDate);
            }
        }
        else {
            this.icuom.audtDate = null;
        }

        this.icuom.audtDate = moment();
        this.icuom.audtUser = this.appSession.user.userName;

        if (!this.icuom.id) {
            this.icuom.createDate = moment();
            this.icuom.createdBy = this.appSession.user.userName;
        }
        if(this.icuom.conver>0){
        this._icuoMsService.createOrEdit(this.icuom)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.message.confirm("Press 'Yes' for New Icuom",this.l('SavedSuccessfully'),( (isConfirmed) =>  {
                    if (isConfirmed) {
                        let seg1ID = this.icuom.id;
                        this.icuom = new ICUOMDto();
        
                        this.show();
                        // this.flag = false;
                        this.modalSave.emit(null);
                    }
                    else
                    {
                        this.notify.info(this.l('SavedSuccessfully'));
                        this.close();
                        this.modalSave.emit(null);
                    }
                }));
            });
        }else{
            this.notify.info("Conver should be greater Than Zero!");
            this.saving = false;
        }
    }

    checkUnitValue(){
        if(this.icuom.unit.length > 7)
        {
            this.notify.warn("Length must be less than and equal than 7");
            this.icuom.unit = "";
        }      
    }





    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
