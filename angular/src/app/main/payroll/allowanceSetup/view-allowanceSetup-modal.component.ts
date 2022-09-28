import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetAllowanceSetupForViewDto, AllowanceSetupDto } from '../shared/dto/allowanceSetup-dto';

@Component({
    selector: 'viewAllowanceSetupModal',
    templateUrl: './view-allowanceSetup-modal.component.html'
})
export class ViewAllowanceSetupModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetAllowanceSetupForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetAllowanceSetupForViewDto();
        this.item.allowanceSetup = new AllowanceSetupDto();
    }

    show(item: GetAllowanceSetupForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
