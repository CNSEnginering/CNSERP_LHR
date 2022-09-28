import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetSalesReferenceForViewDto, SalesReferenceDto } from '../shared/dtos/salesReference-dto';
import { Input } from '@angular/core';

@Component({
    selector: 'viewSalesReferenceModal',
    templateUrl: './view-salesReference-modal.component.html'
})
export class ViewSalesReferenceModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    refType: string;
    active = false;
    saving = false;

    item: GetSalesReferenceForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetSalesReferenceForViewDto();
        this.item.salesReference = new SalesReferenceDto();
    }

    show(item: GetSalesReferenceForViewDto,type:string): void {
        this.refType == type;
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
