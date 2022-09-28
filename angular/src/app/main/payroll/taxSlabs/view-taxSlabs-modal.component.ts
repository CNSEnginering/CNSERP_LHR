import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetReligionForViewDto, ReligionDto } from '../shared/dto/religion-dto';

@Component({
    selector: 'viewTaxSlabsModal',
    templateUrl: './view-taxSlabs-modal.component.html'
})
export class ViewTaxSlabsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetReligionForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetReligionForViewDto();
        this.item.religion = new ReligionDto();
    }

    show(item: GetReligionForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
