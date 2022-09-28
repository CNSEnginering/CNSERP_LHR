import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { PayRollLookupTableModalComponent } from "@app/finders/payRoll/payRoll-lookup-table-modal.component";
import { CreateOrEditSectionDto } from '../shared/dto/section-dto';
import { SectionsServiceProxy } from '../shared/services/section.service';

@Component({
    selector: 'createOrEditSectionModal',
    templateUrl: './create-or-edit-section-modal.component.html'
})
export class CreateOrEditSectionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild("PayRollLookupTableModal", { static: true })
    PayRollLookupTableModal: PayRollLookupTableModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    section: CreateOrEditSectionDto = new CreateOrEditSectionDto();

    audtDate: Date;
    createDate: Date;
    target: any;
    deptName: string;
    constructor(
        injector: Injector,
        private _sectionsServiceProxy: SectionsServiceProxy
    ) {
        super(injector);
    }

    openDepartmentModal() {
        ;
        this.target = "Department";
        debugger
        this.PayRollLookupTableModal.id = String(this.section.deptID);
        this.PayRollLookupTableModal.displayName = this.deptName;
        this.PayRollLookupTableModal.show(this.target);
    }
    setDepartmentNull() {
        this.section.deptID = null;
        this.deptName = "";
    }
    show(sectionId?: number): void {
        this.audtDate = null;
        this.createDate = null;

        if (!sectionId) {
            this.section = new CreateOrEditSectionDto();
            this.section.id = sectionId;

            this.section.active = true;

            this._sectionsServiceProxy.getMaxSectionId().subscribe(result => {
                this.section.secID = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            this._sectionsServiceProxy.getSectionForEdit(sectionId).subscribe(result => {
                this.section = result.section;

                this.active = true;
                this.modal.show();
            });
        }
    }

    getNewPayRollModal() {
       
     this.getNewDepartment();
         
    }
    getNewDepartment() {
        debugger
        this.section.deptID = Number(this.PayRollLookupTableModal.id);
        this.section.deptName = this.PayRollLookupTableModal.displayName;
    }
    save(): void {
        this.saving = true;

        this.section.audtDate = moment();
        this.section.audtUser = this.appSession.user.userName;

        if (!this.section.id) {
            this.section.createDate = moment();
            this.section.createdBy = this.appSession.user.userName;
        }

        this._sectionsServiceProxy.createOrEdit(this.section)
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
