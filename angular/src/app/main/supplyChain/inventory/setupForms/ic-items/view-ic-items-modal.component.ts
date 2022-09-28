import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetICOPT4ForViewDto, ICOPT4Dto } from '../../shared/dto/ic-opt4-dto';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetICItemForViewDto, ICItemDto } from '../../shared/services/ic-Item.service';

@Component({
    selector: 'viewItemModal',
    templateUrl: './view-ic-items-modal.component.html'
})
export class ViewItemModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item:any;

 
    constructor(
        injector: Injector
    ) {
        super(injector);
       // this.item.icItem = new ICItemDto();
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
