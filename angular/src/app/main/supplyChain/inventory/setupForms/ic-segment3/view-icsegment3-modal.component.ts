import { Component, OnInit, Injector, EventEmitter, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetICSegment3ForViewDto, ICSegment3Dto } from '../../shared/services/ic-segment3-service';
import { ModalDirective } from 'ngx-bootstrap';

@Component({
  selector: 'viewIcSegment3Modal',
  templateUrl: './view-IcSegment3-modal.component.html'
})
export class ViewIcsegment3ModalComponent extends AppComponentBase {

  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  active = false;
  saving = false;

  item: GetICSegment3ForViewDto;


  constructor(
      injector: Injector
  ) {
      super(injector);
      this.item = new GetICSegment3ForViewDto();
      this.item.icSegment = new ICSegment3Dto();
  }

  show(item: GetICSegment3ForViewDto): void {
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
