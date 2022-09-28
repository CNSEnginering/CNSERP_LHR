import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { MFtoolSETDto } from '../../shared/dto/mftool.dto';
@Component({
  selector: 'app-view-mftool',
  templateUrl: './view-mftool.component.html',
  styleUrls: ['./view-mftool.component.css']
})
export class ViewMftoolComponent extends AppComponentBase{

  
  @ViewChild('viewModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  active = false;
  saving = false;

  dto: MFtoolSETDto;


  constructor(
      injector: Injector
  ) {
      super(injector);
      this.dto = new MFtoolSETDto();
  }

  show(item: MFtoolSETDto): void {
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
