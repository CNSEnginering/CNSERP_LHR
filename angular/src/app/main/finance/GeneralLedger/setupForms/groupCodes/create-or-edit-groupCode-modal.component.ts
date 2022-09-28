import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { GroupCodesServiceProxy, CreateOrEditGroupCodeDto, GroupCategoryForComboboxDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';



@Component({
    selector: 'createOrEditGroupCodeModal',
    templateUrl: './create-or-edit-groupCode-modal.component.html'
})
export class CreateOrEditGroupCodeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    IsUpdate = false;

    groupCode: CreateOrEditGroupCodeDto = new CreateOrEditGroupCodeDto();

    grgcat: GroupCategoryForComboboxDto[] =  [];


    constructor(
        injector: Injector,
        private _groupCodesServiceProxy: GroupCodesServiceProxy
    ) {
        super(injector);
    }

    show(Update: boolean = false, groupCodeId?: number ): void {
        debugger;
        this.init();
        if (!Update) {
            this.groupCode = new CreateOrEditGroupCodeDto();
            this.groupCode.id = groupCodeId;
            this._groupCodesServiceProxy.maxid().subscribe(result => {
                this.groupCode.grpcode = result.maxID;
            })
            debugger;
            this.IsUpdate = false;
            
            this.active = true;
            this.modal.show();
        } else {
            this._groupCodesServiceProxy.getGroupCodeForEdit(groupCodeId).subscribe(result => {
                this.groupCode = result.groupCode;
                this.IsUpdate = true;
                this.active = true;
                this.modal.show();
            });
        }
    }

    init(): void {

        this._groupCodesServiceProxy.getGroupCategoryForCombobox()
            .subscribe((result) => {
                this.grgcat = result.items;
            });
    }

    save(): void {
        this.saving = true;
        debugger;
        this._groupCodesServiceProxy.createOrEdit(this.groupCode)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.message.confirm("Press 'Yes' for create new group code", this.l('SavedSuccessfully'), (isConfirmed) => {
                    if (isConfirmed) {
                        this.show();
                        this.modalSave.emit(null);
                    }
                    else {
                        this.close();
                        this.modalSave.emit(null);
                    }
                });
            });
    }

    close(): void {
        this.grgcat = [];
        this.active = false;
        this.modal.hide();
    }

    // ifExists = false
    // checkGroupID(event): void {

    //     debugger;
    //     this._groupCodesServiceProxy.maxid().subscribe(result => {
    //         debugger;
    //         if (!result.maxID) {
    //             this.ifExists = true
    //         }
    //         this.groupCode.id = result.maxID;
    //     })


    // }
}
