import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetICOPT4ForViewDto, ICOPT4Dto } from '../../shared/dto/ic-opt4-dto';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewICOPT4Modal',
    templateUrl: './view-icopT4-modal.component.html'
})
export class ViewICOPT4ModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetICOPT4ForViewDto;

 
    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetICOPT4ForViewDto();
        this.item.iCOPT4 = new ICOPT4Dto();
    }

    show(item: GetICOPT4ForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
