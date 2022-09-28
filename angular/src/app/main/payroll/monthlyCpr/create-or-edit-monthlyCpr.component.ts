import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditMonthlyCprDto } from '../shared/dto/monthlyCpr-dto';
import { monthlyCprServiceProxy } from '../shared/services/monthlyCpr.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';


@Component({
  selector: 'app-create-or-edit-monthlyCpr',
  templateUrl: './create-or-edit-monthlyCpr.component.html',
})
export class CreateOrEditMonthlyCprComponent extends AppComponentBase {

  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  // @ViewChild("PayRollLookupTableModal", { static: true })
  // PayRollLookupTableModal: PayRollLookupTableModalComponent;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  active = false;
  saving = false;
  taxAmount : number;
  yearAndMonth = new Date();

  MonthCprDto: CreateOrEditMonthlyCprDto = new CreateOrEditMonthlyCprDto();

  audtDate: Date;
  createDate: Date;
  target: any;
  constructor(
      injector: Injector,
      private _MonthlyCprServiceProxy: monthlyCprServiceProxy
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
      this.MonthCprDto = new CreateOrEditMonthlyCprDto();
      this.active = true;
      this.MonthCprDto.active = true;
      this.yearAndMonth = new Date();
      this.modal.show();
  } else {
      this._MonthlyCprServiceProxy.getDataForEdit(id).subscribe(result => {
        debugger
          this.active=true;
          this.MonthCprDto.active=result.monthlyCPR.active;
          this.MonthCprDto.amount=result.monthlyCPR.amount;
          this.MonthCprDto.cprNumber=result.monthlyCPR.cprNumber;
          this.MonthCprDto.id=result.monthlyCPR.id;
          this.MonthCprDto.cprDate = moment(result.monthlyCPR.cprDate).toDate();
          this.MonthCprDto.salaryMonth=result.monthlyCPR.salaryMonth;
          this.MonthCprDto.salaryYear=result.monthlyCPR.salaryYear;
          this.yearAndMonth = moment(result.monthlyCPR.salaryMonth+"/01/"+result.monthlyCPR.salaryYear).toDate();
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
  if (this.yearAndMonth == null) {
    this.notify.error(this.l('Please Select Year and Month'));
    return;
  }
  this.MonthCprDto.salaryYear = this.yearAndMonth.getFullYear();
  this.MonthCprDto.salaryMonth = this.yearAndMonth.getMonth() + 1;
  this._MonthlyCprServiceProxy.GetTotalTaxAmount(this.MonthCprDto.salaryYear,this.MonthCprDto.salaryMonth).subscribe(
    res => {
      debugger
      this.taxAmount = res["result"];
      if(this.taxAmount == null || this.taxAmount != this.MonthCprDto.amount)
       {
      this.notify.error(this.l('Amount is not equal to Tax Amount.'));
      this.saving = false;
         return;
     }else{
      this._MonthlyCprServiceProxy.create(this.MonthCprDto)
      .pipe(finalize(() => { this.saving = false; }))
      .subscribe(() => {
        debugger
          this.notify.info(this.l('SavedSuccessfully'));
          this.close();
          this.modalSave.emit(null);
      });
     }
    });
  debugger




}

}
