import { Component, OnInit, Injector, ViewChild, EventEmitter, Output } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { GetICSegment1ForViewDto, ICSegment1Dto } from '../../shared/services/ic-segment1-service';

@Component({
  selector: 'viewicsegment1modal',
  templateUrl: './view-icsegment1-modal.component.html'
})
export class ViewIcsegment1ModalComponent extends AppComponentBase {

  
  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetICSegment1ForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetICSegment1ForViewDto();
        this.item.icSegment = new ICSegment1Dto();
    }

    show(item: GetICSegment1ForViewDto): void {
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
