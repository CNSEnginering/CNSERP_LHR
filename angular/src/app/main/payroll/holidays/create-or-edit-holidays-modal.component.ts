import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditHolidaysDto } from '../shared/dto/holidays-dto';
import { HolidaysServiceProxy } from '../shared/services/holidays.service';

@Component({
    selector: 'createOrEditHolidaysModal',
    templateUrl: './create-or-edit-holidays-modal.component.html'
})
export class CreateOrEditHolidaysModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    holidayDate: Date;

    holidays: CreateOrEditHolidaysDto = new CreateOrEditHolidaysDto();

    constructor(
        injector: Injector,
        private _holidaysServiceProxy: HolidaysServiceProxy
    ) {
        super(injector);
    }

    show(holidaysId?: number): void {

        if (!holidaysId) {
            this.holidays = new CreateOrEditHolidaysDto();
            this.holidays.id = holidaysId;

            this.holidayDate = new Date();

            this.holidays.active = true;
            this._holidaysServiceProxy.getMaxHolidayId().subscribe(result => {
                this.holidays.holidayID = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            this._holidaysServiceProxy.getHolidaysForEdit(holidaysId).subscribe(result => {
                this.holidays = result.holidays;

                this.holidayDate = moment(this.holidays.holidayDate).toDate();

                this.active = true;
                this.modal.show();
            });
        }

    }

    save(): void {
        this.saving = true;

        this.holidays.audtDate = moment();
        this.holidays.audtUser = this.appSession.user.userName;

        if (!this.holidays.id) {
            this.holidays.createDate = moment();
            this.holidays.createdBy = this.appSession.user.userName;
        }

        this.holidays.holidayDate = moment(this.holidayDate);

        this._holidaysServiceProxy.createOrEdit(this.holidays)
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
