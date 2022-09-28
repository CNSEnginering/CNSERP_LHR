import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular/main';

@Component({
  selector: 'date-renderer',
  template: `
  <div>
  {{ myDate | momentFormat:'DD-MM-YYYY' }}
  </div>
   `,
  styles: []
})

export class DateRendererComponent implements ICellRendererAngularComp {
  private params: any;
  myDate: Date;

  agInit(params: any): void {
    debugger;
    this.params = params;
    this.myDate = params.value;
  }

  refresh(params: any): boolean {
    debugger;
    params.data.clearingDate = params.value;
    this.myDate = params.value;
    params.api.refreshCells(params);
    return false;
  }

}

