import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetLCExpensesForViewDto, LCExpensesDto } from '@app/main/finance/shared/dto/lcExpenses-dto';

@Component({
    selector: 'viewLCExpenseModal',
    templateUrl: './view-lcExpense-modal.component.html'
})
export class ViewLCExpenseModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    item: GetLCExpensesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetLCExpensesForViewDto();
        this.item.lcExpenses = new LCExpensesDto();
    }

    show(item: GetLCExpensesForViewDto): void {
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
