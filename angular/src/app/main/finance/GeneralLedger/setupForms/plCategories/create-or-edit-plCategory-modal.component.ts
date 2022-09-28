import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';

import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditPLCategoryDto, PLCategoriesServiceProxy } from '@app/main/finance/shared/services/plCategories.service';

@Component({
    selector: 'createOrEditPLCategoryModal',
    templateUrl: './create-or-edit-plCategory-modal.component.html'
})
export class CreateOrEditPLCategoryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    plCategory: CreateOrEditPLCategoryDto = new CreateOrEditPLCategoryDto();



    constructor(
        injector: Injector,
        private _plCategoriesServiceProxy: PLCategoriesServiceProxy
    ) {
        super(injector);
    }

    show(plCategoryId?: number): void {

        if (!plCategoryId) {
            this.plCategory = new CreateOrEditPLCategoryDto();
            this.plCategory.id = plCategoryId;

            this.active = true;
            this.modal.show();
        } else {
            this._plCategoriesServiceProxy.getPLCategoryForEdit(plCategoryId).subscribe(result => {
                this.plCategory = result.plCategory;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._plCategoriesServiceProxy.createOrEdit(this.plCategory)
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
