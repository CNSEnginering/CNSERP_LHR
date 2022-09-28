import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditEmployeeLeavesDto } from '../shared/dto/employeeLeaves-dto';
import { EmployeeLeavesServiceProxy } from '../shared/services/employeeLeaves-service';
import { PayRollLookupTableModalComponent } from '@app/finders/payRoll/payRoll-lookup-table-modal.component';

@Component({
    selector: 'createOrEditEmployeeLeavesModal',
    templateUrl: './create-or-edit-employeeLeaves-modal.component.html'
})
export class CreateOrEditEmployeeLeavesModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('PayRollLookupTableModal', { static: true }) PayRollLookupTableModal: PayRollLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    salaryYear = 'Select Year';
    salaryMonth = 'Select Month';
    leaveType = 'Select Leave Type';

    type = 'fullLeave';


    employeeLeaves: CreateOrEditEmployeeLeavesDto = new CreateOrEditEmployeeLeavesDto();

    target: string;
    employeeName: string;
    startDate: Date;
    years: any;

    constructor(
        injector: Injector,
        private _employeeLeavesServiceProxy: EmployeeLeavesServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.getYears();
    }

    show(employeeLeavesId?: number): void {

        if (!employeeLeavesId) {
            this.salaryYear = 'Select Year';
            this.salaryMonth = 'Select Month';
            this.leaveType = 'Select Leave Type';

            this.employeeLeaves = new CreateOrEditEmployeeLeavesDto();
            this.employeeLeaves.id = employeeLeavesId;
            this.startDate=new Date();
            this.salaryYear=String(this.startDate.getFullYear());
            this.salaryMonth=String(this.startDate.getMonth() + 1);
            this.employeeName="";

            this._employeeLeavesServiceProxy.getMaxLeaveId().subscribe(result => {
                this.employeeLeaves.leaveID = result;
            });

            this.active = true;
            this.modal.show();
        } else {

            this._employeeLeavesServiceProxy.getEmployeeLeavesForEdit(employeeLeavesId).subscribe(result => {
                this.employeeLeaves = result.employeeLeaves;

                this._employeeLeavesServiceProxy.getEmployeeName(this.employeeLeaves.employeeID).subscribe(result => {
                    this.employeeName = result;
                });

                this.salaryMonth = String(this.employeeLeaves.salaryMonth);
                this.salaryYear = String(this.employeeLeaves.salaryYear);

                if (this.employeeLeaves.casual) {
                    this.leaveType = '1';
                }
                else if (this.employeeLeaves.sick) {
                    this.leaveType = '2';
                }
                else if (this.employeeLeaves.annual) {
                    this.leaveType = '3';
                }

                if (this.employeeLeaves.leaveType == 1) {
                    this.type = 'fullLeave';
                }
                else if (this.employeeLeaves.leaveType == 0.5) {
                    this.type = 'halfLeave';
                }


                this.startDate = moment(this.employeeLeaves.startDate).toDate();

                this.active = true;
                this.modal.show();
            });
        }
    }
    GetLeaveBalance() {
        this._employeeLeavesServiceProxy.getEmployeeLeavesForBalance(this.employeeLeaves.employeeID).subscribe(result => {
         //   this.shiftID = result.shiftID;
           // this.shiftName = result.shiftName;
            //this.designationID = result.designationID;
           // this.designationName = result.designation;
            //this.input.timeIn = result.timeIn;
            //this.input.timeOut = result.timeOut;
            //this.input.reason = result.reason;
            // this.onChangeTimeIn(moment(result.timeIn).toDate());
        });
    }

    save(): void {
        debugger;
        this.setLeaveType(this.type);

        if (this.leaveType == 'Select Leave Type') {
            this.notify.error(this.l('Please select leave type'));
            return;
        }

        // if (this.salaryYear == 'Select Year') {
        //     this.notify.error(this.l('Please select Salary Year'));
        //     return;
        // }
        // if (this.salaryMonth == 'Select Month') {
        //     this.notify.error(this.l('Please select Salary Month'));
        //     return;
        // }

        if (this.leaveType == '1') {
            this.employeeLeaves.casual = 1;
            this.employeeLeaves.sick = 0;
            this.employeeLeaves.annual = 0;
        }
        else if (this.leaveType == '2') {
            this.employeeLeaves.casual = 0;
            this.employeeLeaves.sick = 1;
            this.employeeLeaves.annual = 0;
        }
        else if (this.leaveType == '3') {
            this.employeeLeaves.casual = 0;
            this.employeeLeaves.sick = 0;
            this.employeeLeaves.annual = 1;
        }

        this.saving = true;

        this.employeeLeaves.audtDate = moment();
        this.employeeLeaves.audtUser = this.appSession.user.userName;

        this.employeeLeaves.startDate = moment(this.startDate);

        if (!this.employeeLeaves.id) {
            this.employeeLeaves.createDate = moment();
            this.employeeLeaves.createdBy = this.appSession.user.userName;
        }

        this.employeeLeaves.salaryMonth = Number(this.salaryMonth);
        this.employeeLeaves.salaryYear = Number(this.salaryYear);

        this._employeeLeavesServiceProxy.createOrEdit(this.employeeLeaves)
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

    setLeaveType(leaveType): void {
        debugger;
        switch (leaveType) {
            case 'fullLeave':
                this.employeeLeaves.leaveType = 1;
                break;
            case 'halfLeave':
                this.employeeLeaves.leaveType = 0.5;
                break;
        }
    }

    getYears() {
        this.years = new Date().getFullYear();
        var range = [];
        for (var i = -40; i <= 40; i++) {
            range.push(this.years + i);
        }
        this.years = range;
    }

    openEmployeeModal() {
        debugger;
        this.target = "Employee";
        this.PayRollLookupTableModal.id = String(this.employeeLeaves.employeeID);
        this.PayRollLookupTableModal.displayName = this.employeeName;
        this.PayRollLookupTableModal.show(this.target);
    }

    setEmployeeNull() {
        this.employeeLeaves.employeeID = null;
        this.employeeName = "";
    }

    getNewPayRollModal() {
        switch (this.target) {
            case "Employee":
                this.getNewEmployee();
                break;
            default:
                break;
        }
    }

    getNewEmployee() {
        this.employeeLeaves.employeeID = Number(this.PayRollLookupTableModal.id);
        this.employeeName = this.PayRollLookupTableModal.displayName;

    }

    onChange(newvalue)
    {
        this.salaryYear=String(newvalue.getFullYear());
        this.salaryMonth=('0' + String(newvalue.getMonth()+1)).slice(-2);
    }

}
