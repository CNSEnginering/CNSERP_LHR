import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { FiscalCalendersServiceProxy, CreateOrEditFiscalCalenderDto, FiscalCalendarDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';


@Component({
    selector: 'createOrEditFiscalCalenderModal',
    templateUrl: './create-or-edit-fiscalCalender-modal.component.html'
})
export class CreateOrEditFiscalCalenderModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    range = [];
    isCalendarEdit = false;
    allChecked = false;

    fiscalCalender: CreateOrEditFiscalCalenderDto = new CreateOrEditFiscalCalenderDto();
    latFiscalCalender: CreateOrEditFiscalCalenderDto = new CreateOrEditFiscalCalenderDto();
    fiscalCalenderEdit: CreateOrEditFiscalCalenderDto = new CreateOrEditFiscalCalenderDto();

    createdDate: Date;
    editDate: Date;
    period: string;
    lockedCalendar: boolean = false;
    activeCalendar: boolean = false;
    editPeriod: boolean = false;
    selectedPeriod: number;
    trueAll: boolean = false;


    constructor(
        injector: Injector,
        private _fiscalCalendersServiceProxy: FiscalCalendersServiceProxy
    ) {
        super(injector);
    }

    show(fiscalCalenderId?: number, fiscalCalendarEdit?: boolean, calendarEdit?: FiscalCalendarDto): void {
        debugger;
        this.isCalendarEdit = false;
        this.lockedCalendar = false;
        this.activeCalendar = false;

        this.editPeriod = false;
        if (!fiscalCalenderId && !fiscalCalendarEdit) {
            this.fiscalCalender = new CreateOrEditFiscalCalenderDto();
            this.latFiscalCalender = new CreateOrEditFiscalCalenderDto();
            this.fiscalCalender.id = fiscalCalenderId;
            this.fiscalCalender.startDate = moment().startOf('day');
            this.fiscalCalender.endDate = moment().startOf('day');
            this.active = true;
            var year = new Date().getFullYear() - 1;
            console.log("Year", year)
            this.fiscalCalender.period = 0;
            // this._fiscalCalendersServiceProxy.getLastYear().subscribe(result => {
            //     debugger;
            //     this.latFiscalCalender = result.fiscalCalender;

            // if(this.latFiscalCalender != undefined)
            // {
            //     console.log("Last Fiscal Calendar Period",this.latFiscalCalender.period)
            //     year = this.latFiscalCalender.period-1;
            // }
            // else{
            //     
            // }
            //});
            this.range = [];
            for (var i = 0; i < 3; i++) {
                debugger;
                this.range.push({
                    label: String(year + i) + ' - ' + String(year + i + 1),
                    value: parseInt(String(year + i))
                });
            }

            this.modal.show();
        } else if (fiscalCalendarEdit) {
            debugger;
            this.isCalendarEdit = true;

            this._fiscalCalendersServiceProxy.getFiscalCalenderForEdit(calendarEdit.id).subscribe(result => {
                debugger;
                this.fiscalCalenderEdit = result.fiscalCalender;
                this.period = this.fiscalCalenderEdit.period.toString() + "-" + (this.fiscalCalenderEdit.period + 1).toString();
                //this.fiscalCalenderEdit.isActive = true;
                //this.fiscalCalenderEdit.isLocked = true;
                this.allChecked = false;
                if (this.fiscalCalenderEdit.isLocked)
                    this.lockedCalendar = this.fiscalCalenderEdit.isLocked;
                if (this.lockedCalendar) {
                    this.trueAll = true;
                }

                if (this.fiscalCalenderEdit.isActive)
                    this.activeCalendar = this.fiscalCalenderEdit.isActive;

                if (this.fiscalCalenderEdit.createdDate) {
                    this.createdDate = this.fiscalCalenderEdit.createdDate.toDate();
                }
                if (this.fiscalCalenderEdit.editDate) {
                    this.editDate = this.fiscalCalenderEdit.editDate.toDate();
                }


                this.active = true;
                this.modal.show();
            });

            this.active = true;
            this.modal.show();

        }
        else {
            this.editPeriod = true;
            this._fiscalCalendersServiceProxy.getFiscalCalenderForEdit(fiscalCalenderId).subscribe(result => {
                this.fiscalCalender = result.fiscalCalender;
                this.period = this.fiscalCalender.period.toString() + "-" + (this.fiscalCalender.period + 1).toString();
                this.onCheckedChange();
                if (this.fiscalCalender.createdDate) {
                    this.createdDate = this.fiscalCalender.createdDate.toDate();
                }
                if (this.fiscalCalender.editDate) {
                    this.editDate = this.fiscalCalender.editDate.toDate();
                }

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        debugger;
        this.saving = true;
        if (!this.isCalendarEdit) {
            this.selectedPeriod = this.fiscalCalender.period;
            if (!this.editPeriod) {
                if (this.fiscalCalender.isActive) {

                    this._fiscalCalendersServiceProxy.calendarStatus(0)
                        .subscribe(result => {
                            if (result) {
                                this.message.info(this.l('MsgCalendarStatus'));
                                this.saving = false;
                            }
                            else {
                                this.createOrEdit(this.fiscalCalender);
                            }
                        });

                }
                else {
                    this.createOrEdit(this.fiscalCalender);
                }
            }
            else {
                this.createOrEdit(this.fiscalCalender);
            }
        }


        else {
            debugger;
            this._fiscalCalendersServiceProxy.checkAll(this.fiscalCalenderEdit.period, this.trueAll)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modalSave.emit(null);
                });
            if (this.fiscalCalenderEdit.isLocked) {
                debugger;
                this._fiscalCalendersServiceProxy.checkCalendarStatus(this.fiscalCalenderEdit.period)
                    .subscribe(result => {
                        if (result > 0) {
                            this.message.info(this.l('MsgLockedPeriods'));
                            this.saving = false;
                        }
                        else {
                            this.updateCalendar(this.fiscalCalenderEdit);
                        }
                    });
            }
            else {
                if (this.fiscalCalenderEdit.isActive) {
                    this._fiscalCalendersServiceProxy.calendarStatus(this.fiscalCalenderEdit.period)
                        .subscribe(result => {
                            if (result) {
                                this.message.info(this.l('MsgCalendarStatus'));
                                this.saving = false;
                            }
                            else {
                                this.updateCalendar(this.fiscalCalenderEdit);
                            }
                        });
                }

                else {

                    this.updateCalendar(this.fiscalCalenderEdit);
                }

            }
            ////////////////////////////////////Check All////////////////////////////////



            /////////////////////////////////////////////////////////////////////////////


        }

    }

    close(): void {

        this.active = false;
        this.modal.hide();
    }

    delete(): void {

        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._fiscalCalendersServiceProxy.deleteCalendar(this.fiscalCalenderEdit.period)
                        .subscribe(() => {
                            //this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                            this.close();
                            this.modalSave.emit(null);
                        });
                }
            }
        );
    }

    clearLock(): void {

        if (this.fiscalCalenderEdit.isActive) {
            this.fiscalCalenderEdit.isLocked = false;
            //this.activeCalendar = true;
        }
        else this.activeCalendar = false;
    }

    clearActive(): void {
        if (this.fiscalCalenderEdit.isLocked)
            this.fiscalCalenderEdit.isActive = false;
        else this.lockedCalendar = false;


    }


    updateCalendar(calendarDto: CreateOrEditFiscalCalenderDto): void {

        this._fiscalCalendersServiceProxy.updateCalender(calendarDto)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });

    }

    createOrEdit(calendarDto: CreateOrEditFiscalCalenderDto): void {

        this._fiscalCalendersServiceProxy.createOrEdit(calendarDto)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });

    }

    allModuleChecked(): void {

        if (this.allChecked) {
            this.fiscalCalender.hr = true;
            this.fiscalCalender.in = true;
            this.fiscalCalender.oe = true;
            this.fiscalCalender.po = true;
            this.fiscalCalender.pr = true;
            this.fiscalCalender.gl = true;
            this.fiscalCalender.ap = true;
            this.fiscalCalender.ar = true;
            this.fiscalCalender.bk = true;
        }
        else {
            this.fiscalCalender.hr = false;
            this.fiscalCalender.in = false;
            this.fiscalCalender.oe = false;
            this.fiscalCalender.po = false;
            this.fiscalCalender.pr = false;
            this.fiscalCalender.gl = false;
            this.fiscalCalender.ap = false;
            this.fiscalCalender.ar = false;
            this.fiscalCalender.bk = false;
        }
    }

    onCheckedChange(): void {
        if (
            this.fiscalCalender.hr &&
            this.fiscalCalender.in &&
            this.fiscalCalender.oe &&
            this.fiscalCalender.po &&
            this.fiscalCalender.pr &&
            this.fiscalCalender.gl &&
            this.fiscalCalender.ap &&
            this.fiscalCalender.ar &&
            this.fiscalCalender.bk
        ) {
            this.allChecked = true;
        }

        else {
            this.allChecked = false;
        }
    }




}
