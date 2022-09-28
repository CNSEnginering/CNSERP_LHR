import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetAssetRegistrationForViewDto, AssetRegistrationDto } from '../shared/services/assetRegistration.service';

@Component({
    selector: 'viewAssetRegistrationModal',
    templateUrl: './view-assetRegistration-modal.component.html'
})
export class ViewAssetRegistrationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetAssetRegistrationForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetAssetRegistrationForViewDto();
        this.item.assetRegistration = new AssetRegistrationDto();
    }

    show(item: GetAssetRegistrationForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
