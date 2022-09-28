import { Component, Injector, ViewEncapsulation, ViewChild, EventEmitter, Output, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import * as _ from 'lodash';
import * as moment from 'moment';
import { FiscalDateService } from '@app/shared/services/fiscalDate.service';
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
import { PayRollLookupTableModalComponent } from '@app/finders/payRoll/payRoll-lookup-table-modal.component';
@Component({
    templateUrl: './attendanceReports.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class AttendanceReportsComponent extends AppComponentBase {

    @ViewChild('reportView', { static: true }) reportView: ReportviewrModalComponent;
    @ViewChild('PayRollLookupTableModal', { static: true }) PayRollLookupTableModal: PayRollLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();


    target: any;
    type: string = "";
    fromSalary = "0";
    toSalary = "999999";
    fromEmpID = "0";
    fromEmpName = "";
    toEmpID = "99999";
    toEmpName = "";
    fromDeptID = "0";
    fromDeptName = "";
    toDeptID = "99999";
    toDeptName = "";
    fromSecID = "0";
    fromSecName = "";
    toSecID = "99999";
    toSecName = "";
    reportCheck: string;

    yearAndMonth = new Date();
    attendanceYear: number;
    attendanceMonth: number;

    constructor(
        injector: Injector,
        private route: Router

    ) {
        super(injector);
    }

    onOpenCalendar(container) {
        container.monthSelectHandler = (event: any): void => {
            container._store.dispatch(container._actions.select(event.date));
        };
        container.setViewMode('month');
    }

    getReport() {
        debugger;
        switch (this.reportCheck) {
            case 'attendance':
                this.processReport("Attendance");
                break;
            case 'attendanceSummary':
                this.processReport("AttendanceSummary");
                break;
            default:
                break;
        }
    }

    processReport(report: string) {
        debugger;
        if (this.yearAndMonth == null) {
            this.notify.error(this.l('Please Select Year and Month'));
            return;
        }
        let repParams = '';
        this.attendanceYear = this.yearAndMonth.getFullYear();
        this.attendanceMonth = this.yearAndMonth.getMonth() + 1;
        switch (report) {

            case 'Attendance':
                case 'AttendanceSummary':
                if (this.fromEmpID !== undefined)
                    repParams += encodeURIComponent("" + this.fromEmpID) + "$";
                if (this.toEmpID !== undefined)
                    repParams += encodeURIComponent("" + this.toEmpID) + "$";
                if (this.fromDeptID !== undefined)
                    repParams += encodeURIComponent("" + this.fromDeptID) + "$";
                if (this.toDeptID !== undefined)
                    repParams += encodeURIComponent("" + this.toDeptID) + "$";
                if (this.fromSecID !== undefined)
                    repParams += encodeURIComponent("" + this.fromSecID) + "$";
                if (this.toSecID !== undefined)
                    repParams += encodeURIComponent("" + this.toSecID) + "$";
                if (this.fromSalary !== undefined)
                    repParams += encodeURIComponent("" + this.fromSalary) + "$";
                if (this.toSalary !== undefined)
                    repParams += encodeURIComponent("" + this.toSalary) + "$";
                if (this.attendanceYear !== undefined)
                    repParams += encodeURIComponent("" + this.attendanceYear) + "$";
                if (this.attendanceMonth !== undefined)
                    repParams += encodeURIComponent("" + this.attendanceMonth) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            default:
                break;
        }
        this.reportView.show(report, repParams)

    }


    getNewPayRollModal() {
        debugger;
        switch (this.target) {
            case "Employee":
                this.getNewEmployee();
                break;
            case "Department":
                this.getNewDepartment();
                break;
            case "Section":
                this.getNewSection();
                break;
            default:
                break;
        }
    }

    //////////////////////////////////////////////////////Employee//////////////////////////////////////////////////////////////////////////
    openEmployeeModal(type) {
        debugger;
        this.target = "Employee";
        this.type = type;
        if (this.type == "fromEmp") {
            this.PayRollLookupTableModal.id = this.fromEmpID;
            this.PayRollLookupTableModal.displayName = this.fromEmpName;
        } else if (this.type == "toEmp") {
            this.PayRollLookupTableModal.id = this.toEmpID;
            this.PayRollLookupTableModal.displayName = this.toEmpName;
        }
        this.PayRollLookupTableModal.show(this.target);
    }

    getNewEmployee() {
        if (this.type == "fromEmp") {
            this.fromEmpID = this.PayRollLookupTableModal.id;
            this.fromEmpName = this.PayRollLookupTableModal.displayName;
        } else if (this.type == "toEmp") {
            this.toEmpID = this.PayRollLookupTableModal.id;
            this.toEmpName = this.PayRollLookupTableModal.displayName;
        }
    }

    setEmployeeNull(type) {
        if (type == "fromEmp") {
            this.fromEmpID = "0";
            this.fromEmpName = "";
        } else if (type == "toEmp") {
            this.toEmpID = "99999";
            this.toEmpName = "";
        }
    }
    //////////////////////////////////////////////////////Employee////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Department//////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////Department//////////////////////////////////////////////////////////////////////////
    openDepartmentModal(type) {
        debugger;
        this.type = type;
        this.target = "Department";

        if (this.type == "fromDept") {
            this.PayRollLookupTableModal.id = this.fromDeptID;
            this.PayRollLookupTableModal.displayName = this.fromDeptName;
        } else if (this.type == "toDept") {
            this.PayRollLookupTableModal.id = this.toDeptID;
            this.PayRollLookupTableModal.displayName = this.toDeptName;
        }

        this.PayRollLookupTableModal.show(this.target);
    }

    getNewDepartment() {
        if (this.type == "fromDept") {
            this.fromDeptID = this.PayRollLookupTableModal.id;
            this.fromDeptName = this.PayRollLookupTableModal.displayName;
        } else if (this.type == "toDept") {
            this.toDeptID = this.PayRollLookupTableModal.id;
            this.toDeptName = this.PayRollLookupTableModal.displayName;
        }
    }

    setDepartmentNull(type) {
        if (type == "fromDept") {
            this.fromDeptID = "0";
            this.fromDeptName = "";
        } else if (type == "toDept") {
            this.toDeptID = "99999";
            this.toDeptName = "";
        }
    }
    //////////////////////////////////////////////////////Department////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Department//////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////Section////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Department//////////////////////////////////////////////////////////////////////////
    openSectionModal(type) {
        debugger;
        this.target = "Section";
        this.type = type;
        if (this.type == "fromSec") {
            this.PayRollLookupTableModal.id = this.fromSecID;
            this.PayRollLookupTableModal.displayName = this.fromSecName;
        } else if (this.type == "toSec") {
            this.PayRollLookupTableModal.id = this.toSecID;
            this.PayRollLookupTableModal.displayName = this.toSecName;
        }
        this.PayRollLookupTableModal.show(this.target);
    }

    getNewSection() {
        if (this.type == "fromSec") {
            this.fromSecID = this.PayRollLookupTableModal.id;
            this.fromSecName = this.PayRollLookupTableModal.displayName;
        } else if (this.type == "toSec") {
            this.toSecID = this.PayRollLookupTableModal.id;
            this.toSecName = this.PayRollLookupTableModal.displayName;
        }
    }

    setSectionNull(type) {
        if (type == "fromSec") {
            this.fromSecID = "0";
            this.fromSecName = "";
        } else if (type == "toSec") {
            this.toSecID = "99999";
            this.toSecName = "";
        }
    }
    //////////////////////////////////////////////////////Section//////////////////////////////////////////////////////////////////////////



}

