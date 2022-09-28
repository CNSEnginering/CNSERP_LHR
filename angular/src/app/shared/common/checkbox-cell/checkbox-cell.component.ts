import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams } from 'ag-grid-community';
import { IAfterGuiAttachedParams } from '@ag-grid-community/all-modules';

@Component({
  selector: 'app-checkbox-cell',
  templateUrl: './checkbox-cell.component.html',
  styleUrls: ['./checkbox-cell.component.css']
})
export class CheckboxCellComponent implements 
ICellRendererAngularComp  {
  
  
  private params: any;

  agInit(params: any): void {
    this.params = params;
  }

  afterGuiAttached(params?: IAfterGuiAttachedParams): void {
  }

  refresh(params: any): boolean {
    debugger;
    params.data.include = params.value
    console.log(params.value);
    params.api.refreshCells(params);
    return false;
  }
}
