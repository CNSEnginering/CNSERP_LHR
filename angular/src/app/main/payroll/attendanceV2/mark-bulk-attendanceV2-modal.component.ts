import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { TimeEditorComponent } from '../attendance/timeComponents/time-editor.component';
import { TimeRendererComponent } from '../attendance/timeComponents/time-renderer.component';
import { AttendanceServiceProxy } from '../shared/services/attendanceV2.service';
import { CreateOrEditAttendanceDetailDto } from '../shared/dto/attendanceDetail-dto';
import { CreateOrEditAttendanceHeaderDto } from '../shared/dto/attendanceHeader-dto';

@Component({
    selector: 'markBulkAttendanceV2Modal',
    templateUrl: './mark-bulk-attendanceV2-modal.component.html'
})
export class MarkBulkAttendanceV2ModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    processing = false;

    input: CreateOrEditAttendanceHeaderDto;
    details: CreateOrEditAttendanceDetailDto[];
    attendanceDate: Date;

    constructor(
        injector: Injector,
        private _attendanceServiceProxy: AttendanceServiceProxy
    ) {
        super(injector);
    }

    show(): void {
        debugger;
        this.attendanceDate = new Date();

        this.rowData = [];
        this.active = true;
        this.modal.show();
    }

    save(): void {
        debugger;
        this.saving = true;

        var count = this.gridApi.getDisplayedRowCount();

        if (count == 0) {
            this.notify.error(this.l('Please Enter Grid Data'));
            return;
        }

        this.gridApi.forEachNode(node => {
            if (moment(node.data.timeIn).format('A') == 'PM' && moment(node.data.timeOut).format('A') == 'AM') {
                node.data.timeOut = moment(moment(node.data.timeOut).add(1, 'days').format('YYYY-MM-DDTHH:mm:ss.SSS'));
            }
        });

        this.gridApi.forEachNode(node => {
            if (moment.duration(moment(node.data.timeOut).diff(moment(node.data.timeIn))).asMinutes() < 1) {
                this.notify.error(this.l('Time Out can not be less than Time In'));
                return;
            }
        });

        var rowData = [];
        this.gridApi.forEachNode(node => {
            rowData.push(node.data);
        });

        //this.input.docDate = moment();



        this.input = new CreateOrEditAttendanceHeaderDto();

        this.input.attendanceDetail = rowData;


        this._attendanceServiceProxy.updateBulkAttendance(this.input)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.message.success(this.l('SavedSuccessfully'));
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }


    close(): void {
        debugger;
        this.active = false;
        this.modal.hide();
    }

    processBulkAttendance(): void {
        debugger;

        this.processing = true;
        this._attendanceServiceProxy.getAttendanceData(moment(this.attendanceDate).startOf('day')).subscribe(result => {
            debugger;
            this.rowData = [];
            this.rowData = result.items;
            this.processing = false;

        });

    }

    //==================================Grid=================================
    public gridApi;
    public gridColumnApi;
    public rowData;
    public rowSelection;
    columnDefs = [

        { headerName: this.l('SrNo'), field: 'srNo', sortable: true, width: 60, valueGetter: 'node.rowIndex+1' },
        { headerName: this.l('EmployeeID'), field: 'employeeID', sortable: true, width: 120, resizable: true },
        { headerName: this.l('EmployeeName'), field: 'employeeName', sortable: true, filter: true, width: 120, resizable: true },
        { headerName: this.l('ShiftID'), field: 'shiftID', sortable: true, filter: true, width: 100, editable: false, resizable: true },
        {
            headerName: this.l('TimeIn'), field: 'timeIn', editable: true, sortable: true, filter: true, width: 100, resizable: true,
            cellEditorFramework: TimeEditorComponent, cellRendererFramework: TimeRendererComponent
        },
        {
            headerName: this.l('TimeOut'), field: 'timeOut', editable: true, sortable: true, filter: true, width: 100, resizable: true,
            cellEditorFramework: TimeEditorComponent, cellRendererFramework: TimeRendererComponent
        }

    ];

    onGridReady(params) {
        debugger;
        this.rowData = [];
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        params.api.sizeColumnsToFit();
        this.rowSelection = "multiple";
    }



}
