import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditShiftDto } from '../shared/dto/shift-dto';
import { ShiftServiceProxy } from '../shared/services/shift-service';
import { templateEngine } from 'knockout';

@Component({
    selector: 'createOrEditShiftModal',
    templateUrl: './create-or-edit-shift-modal.component.html'
})
export class CreateOrEditShiftModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    shift: CreateOrEditShiftDto = new CreateOrEditShiftDto();

    createDate: Date;
    audtDate: Date;

    myTime: string;
    myTime1: string;
    startTime: any;
    endTime: any;


    constructor(
        injector: Injector,
        private _shiftServiceProxy: ShiftServiceProxy
    ) {
        super(injector);
    }

    show(shiftId?: number): void {

        this.myTime = null;
        this.myTime1 = null;
        debugger;
        if (!shiftId) {
            this.shift = new CreateOrEditShiftDto();
            this.shift.id = shiftId;
            this.shift.active = true;
            this._shiftServiceProxy.getMaxShiftId().subscribe(result => {
                this.shift.shiftID = result;
            });
            this.active = true;
            this.modal.show();
        } else {
            debugger;
            this._shiftServiceProxy.getShiftForEdit(shiftId).subscribe(result => {
                debugger;
                this.shift = result.shift;

                this.shift.startTime == null || undefined ? this.myTime = null : this.myTime = moment(this.shift.startTime).format("hh:mm A");
                this.shift.endTime == null || undefined ? this.myTime1 = null : this.myTime1 = moment(this.shift.endTime).format("hh:mm A");

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        debugger;


        this.shift.audtDate = moment();
        this.shift.audtUser = this.appSession.user.userName;

        this.shift.startTime = this.startTime;
        //this.shift.endTime = this.endTime;

        if (!this.shift.id) {
            this.shift.createDate = moment();
            this.shift.createdBy = this.appSession.user.userName;
        }

        this._shiftServiceProxy.createOrEdit(this.shift)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                debugger;
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }
    onChange(newvalue) {
        debugger;
        if (newvalue == null)
            return;
        this.startTime = newvalue;
        this.myTime = newvalue.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
    }
    onChange1(newvalue) {
        if (newvalue == null)
            return;
        this.endTime = newvalue;
        this.myTime1 = newvalue.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
        this.totalHours();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    totalHours() {
        debugger
        // this.shift.endTime = moment(this.shift.startTime).add(this.shift.totalHour, 'hours');
        // this.myTime1 = moment(this.shift.endTime).format("hh:mm A");

        this.myTime = moment(this.shift.startTime).format("hh:mm A");
         this.myTime1 = moment(this.shift.endTime).format("hh:mm A");

        this._shiftServiceProxy.getTotalHrs(  this.myTime,   this.myTime1).
            subscribe(result => this.shift.totalHour = result["result"]);


    }
}
// totalHours() {
//     debugger
//     this.shift.totalHour = moment(this.shift.startTime)


// }

