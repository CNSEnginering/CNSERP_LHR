import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetCRDRNoteForViewDto, CRDRNoteDto } from '../../shared/dto/crdrNote-dto';

@Component({
    selector: 'viewCRDRNoteModal',
    templateUrl: './view-crdrNote-modal.component.html'
})
export class ViewCRDRNoteModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    item: GetCRDRNoteForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetCRDRNoteForViewDto();
        this.item.crdrNote = new CRDRNoteDto();
    }

    show(item: GetCRDRNoteForViewDto): void {
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
