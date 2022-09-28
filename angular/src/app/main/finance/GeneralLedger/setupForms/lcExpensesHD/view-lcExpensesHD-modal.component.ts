import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetLCExpensesHeaderForViewDto } from '@app/main/finance/shared/dto/lcExpensesHeader-dto';

@Component({
    selector: 'viewLCExpensesHDModal',
    templateUrl: './view-lcExpensesHD-modal.component.html'
})
export class ViewLCExpensesHDModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetLCExpensesHeaderForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetLCExpensesHeaderForViewDto();
    }

    show(item: GetLCExpensesHeaderForViewDto): void {
        debugger;
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
