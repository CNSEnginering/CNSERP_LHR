import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { SubCostCenterDto } from '../shared/dto/subCostCenter-dto';
import { TransferDto } from '../shared/dto/transfer-dto';



@Component({
    selector: 'viewTransfersModal',
    templateUrl: './view-transfers-modal.component.html'
})
export class ViewTransfersComponent extends AppComponentBase {

    @ViewChild('viewTransfersModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;

    item: SubCostCenterDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new SubCostCenterDto();
    }

    show(item: TransferDto): void {
        this.item = item["transfer"];
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
