import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetLocationForViewDto, LocationDto } from '../shared/dto/location-dto';

@Component({
    selector: 'viewLocationModal',
    templateUrl: './view-location-modal.component.html'
})
export class ViewLocationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetLocationForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetLocationForViewDto();
        this.item.location = new LocationDto();
    }

    show(item: GetLocationForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
