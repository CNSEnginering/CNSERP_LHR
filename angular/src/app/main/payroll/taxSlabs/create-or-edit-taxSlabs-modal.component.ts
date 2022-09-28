import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditSlabSetupDto } from '../shared/dto/slabSetup-dto';
import { SlabSetupService } from '../shared/services/slabSetup.service';
import { NgForm } from '@angular/forms';

@Component({
    selector: 'createOrEditTaxSlabsModal',
    templateUrl: './create-or-edit-taxSlabs-modal.component.html'
})
export class CreateOrEditTaxSlabsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('Form', { static: true }) form: NgForm;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    dto: CreateOrEditSlabSetupDto = new CreateOrEditSlabSetupDto();

    createDate: Date;
    audtDate: Date;



    constructor(
        injector: Injector,
        private _slabSetupService: SlabSetupService
    ) {
        super(injector);
    }

    show(id?: number): void {

        debugger;
        if (!id) {
            this.dto = new CreateOrEditSlabSetupDto();
            this.dto.id = id;
            this.dto.active = true;
            this.active = true;
            this.modal.show();
        } else {
            debugger;
            this._slabSetupService.getDataForEdit(id).subscribe(result => {
                debugger;
                this.dto = result.slabSetup;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        debugger;




        if (!this.dto.id) {
            this.dto.createDate = new Date();
            this.dto.createdBy = this.appSession.user.userName;
        }
        else {
            this.dto.audtDate = new Date();
            this.dto.audtUser = this.appSession.user.userName;
        }

        this._slabSetupService.createOrEdit(this.dto)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                debugger;
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.dto = new CreateOrEditSlabSetupDto();
        this.active = false;
        this.modal.hide();
    }
}
