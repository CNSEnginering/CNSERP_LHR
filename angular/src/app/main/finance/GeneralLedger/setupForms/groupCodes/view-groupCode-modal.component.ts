import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetGroupCodeForViewDto, GroupCodeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewGroupCodeModal',
    templateUrl: './view-groupCode-modal.component.html'
})
export class ViewGroupCodeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetGroupCodeForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetGroupCodeForViewDto();
        this.item.groupCode = new GroupCodeDto();
    }

    show(item: GetGroupCodeForViewDto): void {
        debugger;
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
