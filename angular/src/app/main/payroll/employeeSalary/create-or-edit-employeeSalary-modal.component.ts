import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditEmployeeSalaryDto } from '../shared/dto/employeeSalary-dto';
import { EmployeeSalaryServiceProxy } from '../shared/services/employeeSalary.service';
import { PayRollLookupTableModalComponent } from '@app/finders/payRoll/payRoll-lookup-table-modal.component';


@Component({
    selector: 'createOrEditEmployeeSalaryModal',
    templateUrl: './create-or-edit-employeeSalary-modal.component.html'
})
export class CreateOrEditEmployeeSalaryModalComponent extends AppComponentBase {


    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('PayRollLookupTableModal', { static: true }) PayRollLookupTableModal: PayRollLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    employeeSalary: CreateOrEditEmployeeSalaryDto = new CreateOrEditEmployeeSalaryDto();

    target: string;
    startDate: Date;

    constructor(
        injector: Injector,
        private _employeeSalaryServiceProxy: EmployeeSalaryServiceProxy
    ) {
        super(injector);
    }
    onSearchChange(searchValue: number): void {  
        debugger
        if(searchValue!=undefined){
            searchValue=parseFloat(searchValue.toString().replace(/,/g, ""));

            var TaxAbleSal=(searchValue/110)*100;

            var YearlyTaxAbleSal=Math.round(TaxAbleSal*12);


       
        this._employeeSalaryServiceProxy.getEmpSlab(YearlyTaxAbleSal).subscribe(result => {
          debugger
          var ExceedAmt=(YearlyTaxAbleSal - parseInt(result["slabFrom"]));
           if(ExceedAmt!=undefined && ExceedAmt>0){

            ExceedAmt=ExceedAmt*(result["rate"]/100)

            ExceedAmt=ExceedAmt+parseInt(result["amount"]);
            ExceedAmt= Math.ceil(ExceedAmt/12);
            this.employeeSalary.tax=(Math.ceil((ExceedAmt/100))*100);
           // this.employeeSalary.tax=Math.round((ExceedAmt*100)/100);
           }
        });
    }else{
        this.employeeSalary.tax=0;
    }
      }
    show(employeeSalaryId?: number): void {
        if (!employeeSalaryId) {
            this.employeeSalary = new CreateOrEditEmployeeSalaryDto();
            this.employeeSalary.id = employeeSalaryId;
            this.startDate=new Date();
            this.active = true;
            this.modal.show();
        } else {
            this._employeeSalaryServiceProxy.getEmployeeSalaryForEdit(employeeSalaryId).subscribe(result => {
                this.employeeSalary = result.employeeSalary;
                this.startDate=moment(this.employeeSalary.startDate).toDate();
                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        debugger;
        if(this.employeeSalary.gross_Salary==null)
        {
            this.notify.error(this.l('Please Enter Gross Salary'));
            return;
        }
        if(this.employeeSalary.net_Salary>this.employeeSalary.gross_Salary )
        {
            this.notify.error(this.l('Net Salary must be Less than or Equal to Gross Salary'));
            return;
        }

        this.saving = true;
        this.employeeSalary.audtDate=moment();
        this.employeeSalary.audtUser=this.appSession.user.userName;
        this.employeeSalary.startDate = moment(this.startDate).add(1,'day');



        //this.employeeSalary.net_Salary=this.employeeSalary.tax + this.employeeSalary.house_Rent + this.employeeSalary.basic_Salary;

        if (!this.employeeSalary.id) {
            this.employeeSalary.createDate = moment();
            this.employeeSalary.createdBy = this.appSession.user.userName;

        }

        this.employeeSalary.net_Salary = this.employeeSalary.basic_Salary;

        this._employeeSalaryServiceProxy.createOrEdit(this.employeeSalary)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
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
/////////////////////////////////////////////////////////Employee/////////////////////////////////////////////////////
    openEmployeeModal() {
        this.target="Employee";
        this.PayRollLookupTableModal.id= String(this.employeeSalary.employeeID);
        this.PayRollLookupTableModal.displayName= String(this.employeeSalary.employeeName);
        this.PayRollLookupTableModal.show(this.target);
    }

    setEmployeeNull() {
        this.employeeSalary.employeeID = null;
        this.employeeSalary.employeeName = "";
    }


    getNewEmployee()
    {
        this.employeeSalary.employeeID=Number(this.PayRollLookupTableModal.id);
        this.employeeSalary.employeeName=this.PayRollLookupTableModal.displayName;
        this.startDate = new Date(this.PayRollLookupTableModal.joiningDate);
    }

/////////////////////////////////////////////////////////Employee/////////////////////////////////////////////////////
    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
