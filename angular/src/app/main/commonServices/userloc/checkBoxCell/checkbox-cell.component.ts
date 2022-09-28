import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams } from 'ag-grid-community';
import { IAfterGuiAttachedParams } from '@ag-grid-community/all-modules';

@Component({
  selector: 'app-checkbox-cell',
  template:'<input type="checkbox" [(ngModel)]="params.value" (change)="this.refresh(this.params)">'
})
export class CheckBoxCellComponent implements 
ICellRendererAngularComp  {
  
  
  private params: any;

  agInit(params: any): void {
    this.params = params;
  }

  afterGuiAttached(params?: IAfterGuiAttachedParams): void {
  }

  refresh(params: any): boolean {
    debugger;
    if (params.colDef.field == "status") {
      params.data.status = params.value;
    }
    
    params.api.refreshCells(params);
    return false;
  }
}