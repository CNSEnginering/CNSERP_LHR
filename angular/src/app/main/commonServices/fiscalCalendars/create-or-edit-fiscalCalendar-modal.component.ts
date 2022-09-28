import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { FiscalCalendarsServiceProxy, CreateOrEditFiscalCalendarDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';




@Component({
    selector: 'createOrEditFiscalCalendarModal',
    templateUrl: './create-or-edit-fiscalCalendar-modal.component.html'
})
export class CreateOrEditFiscalCalendarModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    // CurrentYear = moment(Date()).format('YYYY');
    // PreviousYear = moment(Date()).format('YYYY') - 1 ;


    // fiscalMonth: {startDate : string, EndDate: string};

    // creatFiscalMonth(): void {

    //     for (let index = 6; index <= 12; index++) {
    //         if (index ) {

    //         }

    //         this.fiscalMonth.startDate = this.FiscalYear + '-' + 1 + index + '-' + 1 ;

    //     }
    // }


    fiscalCalendar: CreateOrEditFiscalCalendarDto = new CreateOrEditFiscalCalendarDto();

    columnDefs = [
        { headerName: 'Period 1', field: 'Period1' },
        { headerName: 'Period 2', field: 'Period2' },
        { headerName: 'Period 3', field: 'Period3' },
        { headerName: 'Period 4', field: 'Period4' },
        { headerName: 'Period 5', field: 'Period5' },
        { headerName: 'Period 6', field: 'Period6' },
        { headerName: 'Period 7', field: 'Period7' },
        { headerName: 'Period 8', field: 'Period8' },
        { headerName: 'Period 9', field: 'Period9' },
        { headerName: 'Period 10', field: 'Period10' },
        { headerName: 'Period 11', field: 'Period11' },
        { headerName: 'Period 12', field: 'Period12' }
    ];

    rowData = [
        { make: 'Toyota', model: 'Celica', price: 35000 },
        { make: 'Ford', model: 'Mondeo', price: 32000 },
        { make: 'Porsche', model: 'Boxter', price: 72000 },
        { make: 'Toyota', model: 'Celica', price: 35000 },
        { make: 'Ford', model: 'Mondeo', price: 32000 },
        { make: 'Porsche', model: 'Boxter', price: 72000 },
        { make: 'Toyota', model: 'Celica', price: 35000 },
        { make: 'Ford', model: 'Mondeo', price: 32000 },
        { make: 'Porsche', model: 'Boxter', price: 72000 },
        { make: 'Toyota', model: 'Celica', price: 35000 },
        { make: 'Ford', model: 'Mondeo', price: 32000 },
        { make: 'Porsche', model: 'Boxter', price: 72000 }
    ];


    constructor(
        injector: Injector,
        private _fiscalCalendarsServiceProxy: FiscalCalendarsServiceProxy
    ) {
        super(injector);
    }

    show(fiscalCalendarId?: number): void {
        debugger;
        if (!fiscalCalendarId) {
            this.fiscalCalendar = new CreateOrEditFiscalCalendarDto();
            this.fiscalCalendar.id = fiscalCalendarId;
            this.fiscalCalendar.audtdate = moment().startOf('day');
            this.fiscalCalendar.bgndatE1 = moment().startOf('day');
            this.fiscalCalendar.bgndatE2 = moment().startOf('day');
            this.fiscalCalendar.bgndatE3 = moment().startOf('day');
            this.fiscalCalendar.bgndatE4 = moment().startOf('day');
            this.fiscalCalendar.bgndatE5 = moment().startOf('day');
            this.fiscalCalendar.bgndatE6 = moment().startOf('day');
            this.fiscalCalendar.bgndatE7 = moment().startOf('day');
            this.fiscalCalendar.bgndatE8 = moment().startOf('day');
            this.fiscalCalendar.bgndatE9 = moment().startOf('day');
            this.fiscalCalendar.bgndatE10 = moment().startOf('day');
            this.fiscalCalendar.bgndatE11 = moment().startOf('day');
            this.fiscalCalendar.bgndatE12 = moment().startOf('day');
            this.fiscalCalendar.bgndatE13 = moment().startOf('day');
            this.fiscalCalendar.enddatE1 = moment().startOf('day');
            this.fiscalCalendar.enddatE2 = moment().startOf('day');
            this.fiscalCalendar.enddatE3 = moment().startOf('day');
            this.fiscalCalendar.enddatE4 = moment().startOf('day');
            this.fiscalCalendar.enddatE5 = moment().startOf('day');
            this.fiscalCalendar.enddatE6 = moment().startOf('day');
            this.fiscalCalendar.enddatE7 = moment().startOf('day');
            this.fiscalCalendar.enddatE8 = moment().startOf('day');
            this.fiscalCalendar.enddatE9 = moment().startOf('day');
            this.fiscalCalendar.enddatE10 = moment().startOf('day');
            this.fiscalCalendar.enddatE11 = moment().startOf('day');
            this.fiscalCalendar.enddatE12 = moment().startOf('day');
            this.fiscalCalendar.enddatE13 = moment().startOf('day');

            this.active = true;
            this.modal.show();
        } else {
            this._fiscalCalendarsServiceProxy.getFiscalCalendarForEdit(fiscalCalendarId).subscribe(result => {
                this.fiscalCalendar = result.fiscalCalendar;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;


        this._fiscalCalendarsServiceProxy.createOrEdit(this.fiscalCalendar)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
