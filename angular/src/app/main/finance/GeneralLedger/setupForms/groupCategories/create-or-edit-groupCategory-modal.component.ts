import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { GroupCategoriesServiceProxy, CreateOrEditGroupCategoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';


@Component({
    selector: 'createOrEditGroupCategoryModal',
    templateUrl: './create-or-edit-groupCategory-modal.component.html'
})
export class CreateOrEditGroupCategoryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    isUpdate = false;

    groupCategory: CreateOrEditGroupCategoryDto = new CreateOrEditGroupCategoryDto();



    constructor(
        injector: Injector,
        private _groupCategoriesServiceProxy: GroupCategoriesServiceProxy
    ) {
        super(injector);
    }

    show(groupCategoryId?: number, Update:boolean = false): void {
debugger;
        if (!Update) {
            this.groupCategory = new CreateOrEditGroupCategoryDto();
            this.groupCategory.id = groupCategoryId;
            this._groupCategoriesServiceProxy.maxid().subscribe(result => {
                this.groupCategory.grpctcode = result;
            })
            this.isUpdate = Update;
            this.active = true;
            this.modal.show();
        } else {
            this._groupCategoriesServiceProxy.getGroupCategoryForEdit(groupCategoryId).subscribe(result => {
                this.groupCategory = result.groupCategory;
                this.isUpdate = Update;
                this.active = true;
                this.modal.show();
            });
        }
    }

   


    save(): void {
            this.saving = true;

			
            this._groupCategoriesServiceProxy.createOrEdit(this.groupCategory)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.message.confirm("Press 'Yes' for create new class", this.l('SavedSuccessfully'), (isConfirmed) => {
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

        this.active = false;
        this.modal.hide();
    }
}
