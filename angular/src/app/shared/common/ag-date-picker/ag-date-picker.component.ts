import { Component, OnInit } from '@angular/core';
import { IAfterGuiAttachedParams } from 'ag-grid-community';
import { ICellRendererAngularComp } from 'ag-grid-angular/dist/interfaces';
import * as moment from 'moment';

@Component({
  selector: 'app-ag-date-picker',
  templateUrl: './ag-date-picker.component.html',
  styleUrls: ['./ag-date-picker.component.css']
})
export class AgDatePickerComponent implements
  ICellRendererAngularComp {


  private params: any;
  createdDate: moment.Moment
  agInit(params: any): void {
    debugger
    this.params = params;
  }

  afterGuiAttached(params?: IAfterGuiAttachedParams): void {
    debugger
  }

  refresh(params: any): boolean {
    debugger
    params.data.amount++;
    params.data.cbox = params.value
    console.log(params.value);
    params.api.refreshCells(params);
    return false;
  }
  getDatePicker() {
    function Datepicker() { }
    Datepicker.prototype.init = function (params) {
      this.eInput = document.createElement("input");
      this.eInput.value = params.value;
      this.
        $(this.eInput).datepicker({ dateFormat: "dd/mm/yy" });
    };
    Datepicker.prototype.getGui = function () {
      return this.eInput;
    };
    Datepicker.prototype.afterGuiAttached = function () {
      this.eInput.focus();
      this.eInput.select();
    };
    Datepicker.prototype.getValue = function () {
      return this.eInput.value;
    };
    Datepicker.prototype.destroy = function () { };
    Datepicker.prototype.isPopup = function () {
      return false;
    };
    return Datepicker;
  }

}


