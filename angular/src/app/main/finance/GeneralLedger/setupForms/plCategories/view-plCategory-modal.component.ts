import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetPLCategoryForViewDto, PLCategoryDto } from '@app/main/finance/shared/services/plCategories.service';

@Component({
    selector: 'viewPLCategoryModal',
    templateUrl: './view-plCategory-modal.component.html'
})
export class ViewPLCategoryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetPLCategoryForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetPLCategoryForViewDto();
        this.item.plCategory = new PLCategoryDto();
    }

    show(item: GetPLCategoryForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
