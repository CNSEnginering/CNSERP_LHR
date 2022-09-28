import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreditDebitNoteDto } from '../shared/dtos/creditDebitNote-dto';



@Component({
    selector: 'viewCreditDebitNoteModal',
    templateUrl: './view-creditDebitNote-modal.component.html'
})
export class ViewCreditDebitNoteComponent extends AppComponentBase {

    @ViewChild('viewCreditDebitNoteModal', { static: true }) modal: ModalDirective;
   // @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: any;


    constructor(
        injector: Injector
    ) {
        super(injector);
       // this.item = new a();
    }

    show(item: any): void {
        debugger
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
