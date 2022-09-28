import { Component, ViewContainerRef, ViewChild, AfterViewInit } from '@angular/core';
import * as moment from 'moment';
import { ICellEditorAngularComp } from 'ag-grid-angular/main';

@Component({
    selector: 'time-editor',
    template: `
    <timepicker #container [(ngModel)]="myTime"
     id="timeIn" name="timeIn"></timepicker>
    `,
    styles: []
})
export class TimeEditorComponent implements ICellEditorAngularComp, AfterViewInit {
    private params: any;
    myTime: Date;

    @ViewChild('container', { static: true, read: ViewContainerRef }) public container;

    ngAfterViewInit() {
        this.container.element.nativeElement.focus();
    }

    agInit(params: any): void {
        debugger;
        this.params = params;
        this.myTime = params.value;
    }

    getValue(): any {
        debugger;
        
        return this.myTime;
    }

    isPopup(): boolean {
        debugger;
        return true;
    }

    onClick(params: any) {
        debugger;
        this.myTime = params.value;
        this.params.api.stopEditing();
    }

}