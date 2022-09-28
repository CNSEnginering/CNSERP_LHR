import { Component, ViewContainerRef, ViewChild, AfterViewInit } from '@angular/core';
import { ICellEditorAngularComp } from 'ag-grid-angular/main';

@Component({
    selector: 'date-editor',
    template: `
    <input #container type="date" [(ngModel)]="myDate" id="myDate" name="myDate"/>
    `,
    styles: []
})

export class DateEditorComponent implements ICellEditorAngularComp, AfterViewInit {
    private params: any;
    myDate: Date;

    @ViewChild('container', { static: true, read: ViewContainerRef }) public container;

    ngAfterViewInit() {
        debugger;
        this.container.element.nativeElement.focus();
    }

    agInit(params: any): void {
        debugger;
        this.params = params;
        this.myDate=params.value;
    }

    getValue(): any {
        debugger;
        return this.myDate;
    }

    isPopup(): boolean {
        debugger;
        return true;
    }

    onClick(params: any) {
        debugger;
        this.myDate = params.value;
        this.params.api.stopEditing();
    }

}