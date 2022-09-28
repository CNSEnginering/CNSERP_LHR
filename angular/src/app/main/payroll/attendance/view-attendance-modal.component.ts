import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetAttendanceHeaderForViewDto, AttendanceHeaderDto } from '../shared/dto/attendanceHeader-dto';

@Component({
    selector: 'viewAttendanceModal',
    templateUrl: './view-attendance-modal.component.html'
})
export class ViewAttendanceModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetAttendanceHeaderForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetAttendanceHeaderForViewDto();
    }

    show(item: GetAttendanceHeaderForViewDto): void {
        debugger;
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
