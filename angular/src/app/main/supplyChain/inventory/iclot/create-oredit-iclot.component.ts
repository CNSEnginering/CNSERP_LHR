import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditLotNoDto } from '../shared/dto/icLot-dto';
import { iclotServiceProxy } from '../shared/services/iclot.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';


@Component({
  selector: 'app-create-oredit-iclot',
  templateUrl: './create-oredit-iclot.component.html'
})
export class CreateOreditIclotComponent extends AppComponentBase {

  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  // @ViewChild("PayRollLookupTableModal", { static: true })
  // PayRollLookupTableModal: PayRollLookupTableModalComponent;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  active = false;
  saving = false;

  LotDto: CreateOrEditLotNoDto = new CreateOrEditLotNoDto();

  audtDate: Date;
  createDate: Date;
  target: any;
  constructor(
      injector: Injector,
      private _iclotServiceProxy: iclotServiceProxy
  ) {
      super(injector);
  }

  close(): void {
    this.active = false;
    this.modal.hide();
}

show(id?: any): void {
  debugger
  this.audtDate = null;
  this.createDate = null;
  
  if (!id) {
      this.LotDto = new CreateOrEditLotNoDto();
       this.LotDto.manfDate= new Date();
       this.LotDto.expiryDate= new Date();
     // this.LotDto.lock = false;

      // this._sectionsServiceProxy.getMaxSectionId().subscribe(result => {
      //     this.section.secID = result;
      // });

      this.active = true;
      this.modal.show();
  } else {
    debugger
      this._iclotServiceProxy.getDataForEdit(id).subscribe(result => {
        debugger
          this.LotDto=result.iclot;
          this.LotDto.manfDate=new Date(result.iclot.manfDate);
          this.LotDto.expiryDate=new Date(result.iclot.expiryDate);
          this.active=true;
          this.modal.show();
      });
  }
}
onOpenCalendar(container) {
  container.monthSelectHandler = (event: any): void => {
      container._store.dispatch(container._actions.select(event.date));
  };
  container.setViewMode("month");
}

save(): void {
  this.saving = true;
debugger
// this.LotDto.manfDate=new Date(this.LotDto.manfDate)
// this.LotDto.expiryDate=new Date(this.LotDto.expiryDate);
this.LotDto.manfDate=this.LotDto.manfDate;
 this.LotDto.expiryDate=this.LotDto.expiryDate;
  this._iclotServiceProxy.create(this.LotDto)
      .pipe(finalize(() => { this.saving = false; }))
      .subscribe(() => {
        debugger
          this.notify.info(this.l('SavedSuccessfully'));
          this.close();
          this.modalSave.emit(null);
      });
}

}
