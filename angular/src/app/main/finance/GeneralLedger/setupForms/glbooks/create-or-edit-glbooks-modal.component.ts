import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { GLBOOKSServiceProxy, CreateOrEditGLBOOKSDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { FormControlName } from '@angular/forms';


@Component({
    selector: 'createOrEditGLBOOKSModal',
    templateUrl: './create-or-edit-glbooks-modal.component.html'
})
export class CreateOrEditGLBOOKSModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    isExists = false;
    active = false;
    saving = false;


    glbooks: CreateOrEditGLBOOKSDto = new CreateOrEditGLBOOKSDto();

    audtdate: Date;


    constructor(
        injector: Injector,
        private _glbooksServiceProxy: GLBOOKSServiceProxy
    ) {
        super(injector);
    }

    onSearchChange(searchValue: string): void {
        this._glbooksServiceProxy.bookIdExists(searchValue).subscribe(
            res => {
                if(res == true)
                {
                    this.isExists = true;
                    this.notify.warn(this.l('Book ID Already Exists'));
                    this.glbooks.bookID = null;
                }
                else
                {
                    this.isExists = false;
                }
            }
        );
    };
    show(glbooksId?: number): void {
        this.audtdate = null;
        if (!glbooksId) {
            this.glbooks = new CreateOrEditGLBOOKSDto();
            this.glbooks.id = glbooksId;
            this.active = true;
            this.modal.show();
        } else {
            this._glbooksServiceProxy.getGLBOOKSForEdit(glbooksId).subscribe(result => {
                this.glbooks = result.glbooks;

                if (this.glbooks.audtdate) {
                    
                    this.audtdate = this.glbooks.audtdate.toDate();
                }

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        debugger
        this.saving = true;
        if (this.audtdate) {
            if (!this.glbooks.audtdate) {
                this.glbooks.audtdate = moment(this.audtdate).startOf('day');
            }
            else {
                this.glbooks.audtdate = moment(this.audtdate);
            }
        }
        else {
            this.glbooks.audtdate = null;
        }
        this._glbooksServiceProxy.createOrEdit(this.glbooks)
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
