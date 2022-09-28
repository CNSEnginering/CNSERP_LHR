import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { RequisitionDto } from '../shared/dtos/requisition-dto';



@Component({
    selector: 'viewRequisitionModal',
    templateUrl: './view-requisition-modal.component.html'
})
export class ViewRequisitionComponent extends AppComponentBase {

    @ViewChild('viewRequisitionModal', { static: true }) modal: ModalDirective;
   // @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: RequisitionDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new RequisitionDto();
    }

    show(item: RequisitionDto): void {
        debugger
        this.item = item["requisition"];
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
