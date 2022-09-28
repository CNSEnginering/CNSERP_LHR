import { Component, Injector, ViewEncapsulation, EventEmitter, ViewChild, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import * as _ from 'lodash';
import * as moment from 'moment';
import { CreateOrEditAttendanceDto } from '../shared/dto/attendanceV2-dto';
import { AttendanceServiceProxy } from '../shared/services/attendanceV2.service';
import { PayRollLookupTableModalComponent } from '@app/finders/payRoll/payRoll-lookup-table-modal.component';
import { stringify } from 'querystring';
import { MarkBulkAttendanceV2ModalComponent } from './mark-bulk-attendanceV2-modal.component';
import { CheckboxCellComponent } from '@app/shared/common/checkbox-cell/checkbox-cell.component';
import { AttendanceDetailDto } from '../shared/dto/attendanceDetail-dto';
import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './attendanceV2.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class AttendanceV2Component extends AppComponentBase {
    @ViewChild('PayRollLookupTableModal', { static: true }) PayRollLookupTableModal: PayRollLookupTableModalComponent;
    @ViewChild('markBulkAttendanceV2ModalComponent', { static: true }) markBulkAttendanceV2Modal: MarkBulkAttendanceV2ModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();


    input: CreateOrEditAttendanceDto = new CreateOrEditAttendanceDto();

    processing = false;
    target: string;
    attendanceDate: Date = new Date();
    attedanceBulk: Date = new Date();
    todayDate: Date = new Date();

    shiftID: number;
    shiftName: string;
    designationID: number;
    designationName: string;
    absent: boolean;
    protected gridApi;
    protected gridColumnApi;
    protected rowData = [];
    protected rowSelection;
    columnDefs = [
        {
            headerName: "SrNo",
            field: "srNo",
            width: 55,
            sortable: true,
            valueGetter: "node.rowIndex+1",
        },
        {
            headerName: "Attendance Date",
            field: "attendanceDate",
            resizable: true,
            sortable: true,
            filter: true,
            minWidth: "45%",
        },
        {
            headerName: this.l("Absent"),
            field: "include",
            sortable: true,
            width: 120,
            resizable: true,
            minWidth: "45%",
            cellRendererFramework: CheckboxCellComponent,
        },
    ];


    constructor(
        injector: Injector,
        private _attendanceServiceProxy: AttendanceServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute
    ) {
        super(injector);
    }

    openEmployeeModal() {
        debugger;
        this.target = "Employee";
        this.PayRollLookupTableModal.id = String(this.input.employeeID);
        this.PayRollLookupTableModal.displayName = String(this.input.employeeName);
        this.PayRollLookupTableModal.show(this.target);
    }

    getNewPayRollModal() {
        debugger;
        switch (this.target) {
            case "Employee":
                this.getNewEmployee();
                break;
            default:
                break;
        }
    }

    getNewEmployee() {
        debugger;

        this.input.employeeID = Number(this.PayRollLookupTableModal.id);
        this.input.employeeName = this.PayRollLookupTableModal.displayName;

        if (this.input.employeeID) {
            this.employeeDataForAttendance();
        }

    }


    onGridReady(params) {
        debugger;
        this.rowData = [];

        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        params.api.sizeColumnsToFit();
        this.rowSelection = "multiple";

    }

    fetchAttendance() {

        this._attendanceServiceProxy.getEmployeeAttendanceData(this.attedanceBulk, this.input.employeeID)
            .subscribe(res => {

                this.rowData = res["items"];
                this.gridApi.refreshCells();
            })



    }

    setDataNull() {
        debugger;
        this.input.employeeID = null;
        this.input.employeeName = "";
        this.shiftID = null;
        this.shiftName = "";
        this.designationID = null;
        this.designationName = "";
        this.input.timeIn = null;
        this.input.timeOut = null;
        // this.input.breakOut = null;
        // this.input.breakIn = null;
        this.input.reason = "";
        // this.timeIn = "";
    }

    employeeDataForAttendance() {
        this._attendanceServiceProxy.employeeDataForAttendance(this.input.employeeID, moment(this.attendanceDate)).subscribe(result => {
            this.shiftID = result.shiftID;
            this.shiftName = result.shiftName;
            this.designationID = result.designationID;
            this.designationName = result.designation;
            this.input.timeIn = result.timeIn;
            this.input.timeOut = result.timeOut;
            this.input.reason = result.reason;
            // this.onChangeTimeIn(moment(result.timeIn).toDate());
        });
    }

    updateAttendance() {
        debugger;

        if (moment(this.input.timeIn).format('A') == 'PM' && moment(this.input.timeOut).format('A') == 'AM') {
            this.input.timeOut = moment(moment(this.input.timeOut).add(1, 'days').format('YYYY-MM-DDTHH:mm:ss.SSS'));
        }

        if (moment.duration(moment(this.input.timeOut).diff(moment(this.input.timeIn))).asMinutes() < 1) {
            this.notify.error(this.l('Time Out can not be less than Time In'));
            return;
        }

        if (this.absent) {
            this.input.timeIn = null;
            this.input.timeOut = null;
        }

        this.processing = true;
        this.input.attendanceDate = moment(this.attendanceDate).startOf('day');
        this._attendanceServiceProxy.updateAttendance(this.input).subscribe(result => {
            if (result == "done") {
                this.processing = false;
                this.setDataNull();
                this.notify.info(this.l('Updated Successfully'));
            }
            else if (result == "failed") {
                this.processing = false;
                this.notify.error(this.l('Please first schedule attendance.'));
            }
        });
    }

    // timeIn: string;

    // onChangeTimeIn(newvalue) {
    //     if (newvalue != "Invalid Date") {
    //         this.timeIn = newvalue.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
    //     }
    //     else {
    //         this.timeIn = "";
    //     }
    // }

    onOpenCalendar(container) {
        container.monthSelectHandler = (event: any): void => {
            container._store.dispatch(container._actions.select(event.date));
        };
        container.setViewMode('month');
    }

    updateAbsences() {
        debugger;

        if (this.rowData.length == 0) {
            return this.notify.warn("Please fetch attendance data");
        }

        let data: AttendanceDetailDto[];

        data = this.rowData;

        this._attendanceServiceProxy.updateBulkAttendanceByEmp(data)
            .pipe(finalize(() => { this.processing = false; }))
            .subscribe(() => {
                this.processing = true;
                this.message.success(this.l('SavedSuccessfully'));
                this.rowData = [];
                this.gridApi.refreshCells();
            });
    }

}
