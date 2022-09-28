import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetICOPT5ForViewDto, ICOPT5Dto } from '../../shared/dto/ic-opt5-dto';

@Component({
    selector: 'viewICOPT5Modal',
    templateUrl: './view-icopT5-modal.component.html'
})
export class ViewICOPT5ModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetICOPT5ForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetICOPT5ForViewDto();
        this.item.iCOPT5 = new ICOPT5Dto();
    }

    show(item: GetICOPT5ForViewDto): void {
        
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
