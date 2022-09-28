import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { MFtooltySETDto } from '../../shared/dto/mftoolty.dto';

@Component({
  selector: 'viewmftoolty',
  templateUrl: './view-mftoolty.component.html',
  styleUrls: ['./view-mftoolty.component.css']
})
export class ViewMftooltyComponent extends AppComponentBase {

  @ViewChild('viewModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  active = false;
  saving = false;

  dto: MFtooltySETDto;


  constructor(
      injector: Injector
  ) {
      super(injector);
      this.dto = new MFtooltySETDto();
  }

  show(item: MFtooltySETDto): void {
    debugger
    this.dto = item;
      
      this.active = true;
      this.modal.show();
  }

  close(): void {
      this.active = false;
      this.modal.hide();
  }

}
