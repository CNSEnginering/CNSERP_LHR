import { Component, OnInit, Injector, ViewChild, EventEmitter, Output } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetICSegment2ForViewDto, ICSegment2Dto } from '../../shared/services/ic-segment2-service';
import { ModalDirective } from 'ngx-bootstrap';

@Component({
  selector: 'viewIcSegment2modal',
  templateUrl: './view-IcSegment2-modal.component.html'
})
export class ViewIcSegment2ModalComponent extends AppComponentBase {

  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  active = false;
  saving = false;

  item: GetICSegment2ForViewDto;


  constructor(
      injector: Injector
  ) {
      super(injector);
      this.item = new GetICSegment2ForViewDto();
      this.item.icSegment = new ICSegment2Dto();
  }

  show(item: GetICSegment2ForViewDto): void {
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
