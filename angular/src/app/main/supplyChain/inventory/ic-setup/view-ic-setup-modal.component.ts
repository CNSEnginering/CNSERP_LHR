import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ICSetupDto } from '../shared/dto/ic-setup-dto';

@Component({
    selector: 'viewICSetupModal',
    templateUrl: './view-ic-setup-modal.component.html'
})
export class ViewICSetupModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: ICSetupDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new ICSetupDto();
    }

    show(item: ICSetupDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
