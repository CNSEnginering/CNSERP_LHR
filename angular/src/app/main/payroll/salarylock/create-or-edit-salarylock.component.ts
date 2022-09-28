import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditsalarylockDto } from '../shared/dto/salarylock-dto';
import { SalarylockServiceProxy } from '../shared/services/salarylock.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';

@Component({
  selector: 'app-create-or-edit-salarylock',
  templateUrl: './create-or-edit-salarylock.component.html',
})
export class CreateOrEditSalarylockComponent extends AppComponentBase {

  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  // @ViewChild("PayRollLookupTableModal", { static: true })
  // PayRollLookupTableModal: PayRollLookupTableModalComponent;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  active = false;
  saving = false;

  salarylock: CreateOrEditsalarylockDto = new CreateOrEditsalarylockDto();

  audtDate: Date;
  createDate: Date;
  target: any;
  constructor(
      injector: Injector,
      private _SalarylockServiceProxy: SalarylockServiceProxy
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
      this.salarylock = new CreateOrEditsalarylockDto();
    this.salarylock.lockDate= new Date();
      this.salarylock.locked = false;

      // this._sectionsServiceProxy.getMaxSectionId().subscribe(result => {
      //     this.section.secID = result;
      // });

      this.active = true;
      this.modal.show();
  } else {
    debugger
      this._SalarylockServiceProxy.getDataForEdit(id).subscribe(result => {
        debugger
          this.salarylock.id=result.salaryLock.id; 
          this.salarylock.lockDate=new Date(result.salaryLock.lockDate);
          
          this.salarylock.locked = result.salaryLock.locked;
          this.salarylock.jvLocked=result.salaryLock.jvLocked;
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
  if (this.salarylock.lockDate == null) {
    this.notify.error(this.l("Please Select Year and Month"));
    return;
}

  var dates=this.salarylock.lockDate;
  this.salarylock.salaryMonth=dates.getMonth()+1;
  this.salarylock.salaryYear=dates.getFullYear();
  this.salarylock.lockDate=new Date();
  this._SalarylockServiceProxy.create(this.salarylock)
      .pipe(finalize(() => { this.saving = false; }))
      .subscribe(() => {
        debugger
          this.notify.info(this.l('SavedSuccessfully'));
          this.close();
          this.modalSave.emit(null);
      });
}
}
