import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CostCenterDto } from '../shared/dto/costCenter-dto';



@Component({
    selector: 'viewCostCenterModal',
    templateUrl: './view-costCenter-modal.component.html'
})
export class ViewCostCenterComponent extends AppComponentBase {

    @ViewChild('viewCostCenterModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: CostCenterDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new CostCenterDto();
    }

    show(item: CostCenterDto): void {
        this.item = item["costCenter"];
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
