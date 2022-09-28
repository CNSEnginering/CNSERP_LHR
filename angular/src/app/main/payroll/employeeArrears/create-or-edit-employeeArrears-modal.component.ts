import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditEmployeeArrearsDto } from '../shared/dto/employeeArrears-dto';
import { EmployeeArrearsServiceProxy } from '../shared/services/employeeArrears.service';
import { PayRollLookupTableModalComponent } from '@app/finders/payRoll/payRoll-lookup-table-modal.component';
import { analyzeAndValidateNgModules } from '@angular/compiler';

@Component({
    selector: 'createOrEditEmployeeArrearsModal',
    templateUrl: './create-or-edit-employeeArrears-modal.component.html'
})
export class CreateOrEditEmployeeArrearsModalComponent extends AppComponentBase {


    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('PayRollLookupTableModal', { static: true }) PayRollLookupTableModal: PayRollLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;




    employeeArrears: CreateOrEditEmployeeArrearsDto = new CreateOrEditEmployeeArrearsDto();

    target: string;

    years: any;
    arrearDate: Date;
    salaryYear: string;
    salaryMonth: string;
    tempDate: Date;

    constructor(
        injector: Injector,
        private _employeeArrearsServiceProxy: EmployeeArrearsServiceProxy
    ) {
        super(injector);
    }


    show(employeeArrearsId?: number): void {
        debugger;

        if (!employeeArrearsId) {
            this.employeeArrears = new CreateOrEditEmployeeArrearsDto();
            this.employeeArrears.id = employeeArrearsId;
            this.arrearDate=new Date();
            this.salaryYear=String(this.arrearDate.getFullYear());
            this.salaryMonth=String(this.arrearDate.getMonth() + 1);
            //this.arrearDate=moment().format("DD/MM/YYYY");
            // this.salaryYear=this.arrearDate.substr(6, 4);
            // this.salaryMonth=this.arrearDate.substr(3,2);

            this.employeeArrears.active=true;

            this._employeeArrearsServiceProxy.getMaxArrearId().subscribe(result => {
                this.employeeArrears.arrearID = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            this._employeeArrearsServiceProxy.getEmployeeArrearsForEdit(employeeArrearsId).subscribe(result => {
                debugger;
                this.employeeArrears = result.employeeArrears;
                this.salaryMonth= ('0' + String(this.employeeArrears.salaryMonth)).slice(-2);
                this.salaryYear=String(this.employeeArrears.salaryYear);
                this.arrearDate=moment(this.employeeArrears.arrearDate).toDate();

                //this.employeeArrears.arrearDate==null || undefined ? this.arrearDate="" : this.arrearDate=moment(this.employeeArrears.arrearDate).format("DD/MM/YYYY");

                this.active = true;
                this.modal.show();
            });
        }
    }


    save(): void {
        debugger;
        if (this.salaryYear == 'Select Year') {
            this.notify.error(this.l('Please select Salary Year'));
            return;
        }

        if (this.salaryMonth == 'Select Month') {
            this.notify.error(this.l('Please select Salary Month'));
            return;
        }

        this.saving = true;

        this.employeeArrears.audtDate=moment();
        this.employeeArrears.audtUser=this.appSession.user.userName;

        this.employeeArrears.arrearDate = moment(this.arrearDate);

        if (!this.employeeArrears.id) {
            this.employeeArrears.createDate = moment();
            this.employeeArrears.createdBy = this.appSession.user.userName;
        }

        this.employeeArrears.salaryMonth=Number(this.salaryMonth);
        this.employeeArrears.salaryYear=Number(this.salaryYear);

        this._employeeArrearsServiceProxy.createOrEdit(this.employeeArrears)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    onChange(newvalue)
    {
        this.salaryYear=String(newvalue.getFullYear());
        this.salaryMonth=('0' + String(newvalue.getMonth()+1)).slice(-2);
    }

    openEmployeeModal() {
        debugger;
        this.target="Employee";
        this.PayRollLookupTableModal.id= String(this.employeeArrears.employeeID);
        this.PayRollLookupTableModal.displayName= String(this.employeeArrears.employeeName);
        this.PayRollLookupTableModal.show(this.target);
    }

    setEmployeeNull() {
        this.employeeArrears.employeeID = null;
        this.employeeArrears.employeeName = "";
    }
    getNewPayRollModal()
    {
        switch (this.target) {
            case "Employee":
                this.getNewEmployee();
                break;
            default:
                break;
        }
    }

    getNewEmployee()
    {
        this.employeeArrears.employeeID=Number(this.PayRollLookupTableModal.id);
        this.employeeArrears.employeeName=this.PayRollLookupTableModal.displayName;
    }





    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
