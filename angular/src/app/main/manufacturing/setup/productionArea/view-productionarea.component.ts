import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { MFAREADto } from '../../shared/dto/productionarea.dto';

@Component({
    selector: 'viewProductionarea',
    templateUrl: './view-productionarea.component.html'
})
export class ViewProductionAreaComponent extends AppComponentBase {

    @ViewChild('viewModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    dto: MFAREADto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.dto = new MFAREADto();
    }

    show(item: MFAREADto): void {
        this.dto = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
