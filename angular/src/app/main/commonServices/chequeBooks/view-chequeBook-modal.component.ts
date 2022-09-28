import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetChequeBookForViewDto, ChequeBookDto } from '../shared/dto/chequeBooks-dto';

@Component({
    selector: 'viewChequeBookModal',
    templateUrl: './view-chequeBook-modal.component.html'
})
export class ViewChequeBookModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetChequeBookForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetChequeBookForViewDto();
        this.item.chequeBook = new ChequeBookDto();
    }

    show(item: GetChequeBookForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
