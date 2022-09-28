import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular/main';
import * as moment from 'moment';

@Component({
    selector: 'time-renderer',
    template: `
  <div>
  {{ myTime | momentFormat:'hh:mm A' }}
  </div>
    `
})
export class TimeRendererComponent implements ICellRendererAngularComp {
    private params: any;
    myTime: Date;

    agInit(params: any): void {
        //debugger;
        this.params = params;
        this.myTime = params.value;
        //this.setTime(params);
    }

    refresh(params: any): boolean {
       // debugger;
        if (params.colDef.field == "timeIn") {
            params.data.timeIn = params.value;
        }
        else if (params.colDef.field == "timeOut") {
            params.data.timeOut = params.value;
        }
        else if (params.colDef.field == "breakIn") {
            params.data.breakIn = params.value;
        }
        else if (params.colDef.field == "breakOut") {
            params.data.breakOut = params.value;
        }
        this.myTime = params.value;
        //this.setTime(params);
        params.api.refreshCells(params);
        return false;
    }

    // setTime(params) {
    //   debugger;
    //   this.myTime = params.value;
    // }

}

