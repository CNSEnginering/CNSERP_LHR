import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CostCenterDto } from '../shared/dto/costCenter-dto';
import { GatePassHeaderDto } from '../shared/dto/gatePassHeader-dto';



@Component({
    selector: 'viewGatePassModal',
    templateUrl: './view-gatePass-modal.component.html'
})
export class ViewGatePassComponent extends AppComponentBase {

    @ViewChild('viewGatePassModal', { static: true }) modal: ModalDirective;
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

    show(item: GatePassHeaderDto): void {
        debugger
        this.item = item["gatePass"];
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
