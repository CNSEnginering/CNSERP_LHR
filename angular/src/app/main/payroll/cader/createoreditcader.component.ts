import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { caderDTo } from '../shared/dto/Cader-dto';
import { CaderServiceProxy } from '../shared/services/Cader.service';

@Component({
  selector: 'app-create-or-edit-cader',
  templateUrl: './createoreditcader.component.html'
})
export class CreateOrEditCaderComponent  extends AppComponentBase {

  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  active = false;
  saving = false;

  cader: caderDTo = new caderDTo();

  createDate: Date;
  audtDate: Date;

  constructor(
    injector: Injector,
    private _caderServiceProxy: CaderServiceProxy
) {
    super(injector);
}

show(Id?: number): void {

  debugger;
  if (!Id) {
      debugger;
      this.cader = new caderDTo();
     
      this.active = true;
      this.modal.show();
  } else {
      debugger;
      this._caderServiceProxy.getDataForEdit(Id).subscribe(result => {
          debugger;
          this.cader = result["cader"];


          this.active = true;
          this.modal.show();
      });
  }
}

save(): void {
  this.saving = true;
  debugger;

  this._caderServiceProxy.create(this.cader)
      .pipe(finalize(() => { this.saving = false; }))
      .subscribe(() => {
          debugger;
          this.notify.info(this.l('SavedSuccessfully'));
          this.close();
          this.modalSave.emit(null);
      });
}

close(): void {
  this.active = false;
  this.modal.hide();
}
}




