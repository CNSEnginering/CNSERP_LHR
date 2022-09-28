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
  templateUrl: './allowances-reports.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
 
})
export class AllowancesReportsComponent extends AppComponentBase {

 
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
    empTypeId: number;
    locID: number = 0;
    TlocID: number = 99999;
    TlocationName:string;
    locationName:string;
    empTypeName:string;
    typeID:number=0;
    valchk:boolean=true;
    transaction: string;
    reportServer: string;
    reportUrl: string;
    showParameters: string;
    parameters: any;
    language: string;
    width: number;
    height: number;
    toolbar: string;
    location: number;
    AllowancetypeID:number=1;
    yearAndMonth = new Date();
    AllowanceYear: number;
    AllowanceMonth: number;
    AllowancetypeName:string;
    filterText = '';
    id: number;
    priceList: any;
    sorting: any;
    skipCount: any;
    MaxResultCount: any;
    data: any;
    alltypeid:number=1;
    alltypeName:string;
    constructor(
        injector: Injector,
        private route: Router,
        private _reportService: FiscalDateService

    ) {
        super(injector);
    }




    onOpenCalendar(container) {
        container.monthSelectHandler = (event: any): void => {
            container._store.dispatch(container._actions.select(event.date));
        };
        container.setViewMode('month');
    }
    openLocationModal(type) {
        ;
        this.target ='Location' ;
        this.type = type;
        this.PayRollLookupTableModal.id = String(this.locID);
        this.PayRollLookupTableModal.displayName = this.locationName;
        this.PayRollLookupTableModal.show(this.target);
    }
    setLocationNull(type) {
        this.type = type;
        if (this.type == "fromLoc") {
            this.locID = null;
            this.locationName = "";
        } else if (this.type == "ToLoc") {
            this.TlocID = null;
            this.TlocationName = "";
        }

    }
    openAllTypeModal(type) {
       
        debugger
        this.target ='Allowancetype' ;
        this.type = type;
        this.PayRollLookupTableModal.id = String(this.AllowancetypeID);
        this.PayRollLookupTableModal.displayName = this.AllowancetypeName;
        this.PayRollLookupTableModal.show(this.target);
    }
    setAllTypeNull(type) {
        this.type = type;
        if (this.type == "Allowancetype") {
            this.AllowancetypeID = null;
            this.AllowancetypeName = "";
        } 

    }
    setEmpTypeNull() {
        this.typeID = 0;
        this.empTypeName = "";
    }
    openEmpTypeModal(type) {
        ;
        this.target = "EmploymentType";
        this.type=type;
        debugger
        if(this.type=='EmploymentType'){
            this.PayRollLookupTableModal.id = String(this.typeID);
            this.PayRollLookupTableModal.displayName = this.empTypeName;
          
        }
        this.PayRollLookupTableModal.show(this.target);
    }
    getReport() {
        this.processReport("AllowanceType");
    }

    processReport(report: string) {
        debugger;

        if (this.yearAndMonth == null) {
            this.notify.error(this.l('Please Select Year and Month'));
            return;
        }
        if (this.reportCheck == undefined) {
            this.notify.error(this.l('Please Select Allowance Type'));
            return;
        }
        let repParams = '';
        this.AllowanceYear = this.yearAndMonth.getFullYear();
        this.AllowanceMonth = this.yearAndMonth.getMonth() + 1;
        switch (report) {

            case 'AllowanceType':
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
                if (this.AllowanceYear !== undefined)
                    repParams += encodeURIComponent("" + this.AllowanceYear) + "$";
                if (this.AllowanceMonth !== undefined)
                    repParams += encodeURIComponent("" + this.AllowanceMonth) + "$";

                if (this.locID !== undefined)
                    repParams += encodeURIComponent("" + this.locID) + "$";
                if (this.TlocID !== undefined)
                    repParams += encodeURIComponent("" + this.TlocID) + "$";
               
                if (this.AllowancetypeID !== undefined)
                    repParams += encodeURIComponent("" + this.AllowancetypeID) + "$";
                if (this.typeID !== undefined)
                    repParams += encodeURIComponent("" + this.typeID) + "$";
                if (this.reportCheck !== undefined)
                    repParams += encodeURIComponent("" + this.reportCheck) + "$";
                
                repParams += encodeURIComponent("" + true) + "$";
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
            case "Location":
                    this.getNewLocation();
            case "EmploymentType":
                    this.getNewEmpType();
            case "Allowancetype":
                this.getNewAllowanceType();
                break;
            // case "Shift":
            //     this.getNewShift();
            //     break;
            // case "EmploymentType":
            //     this.getNewEmpType();
            //     break;
            default:
                break;
        }
    }
    getNewEmpType() {
        debugger
        if(this.type=='EmpType'){
            this.typeID = Number(this.PayRollLookupTableModal.id);
            this.empTypeName = this.PayRollLookupTableModal.displayName;
        }
        
    }
    getNewAllowanceType() {
        debugger
        if(this.type=='Allowancetype'){
            this.AllowancetypeID = Number(this.PayRollLookupTableModal.id);
            this.AllowancetypeName = this.PayRollLookupTableModal.displayName;
        }
        
    }
    getNewLocation() {
        if (this.type == "fromLoc") {
            this.locID = Number(this.PayRollLookupTableModal.id);
            this.locationName = this.PayRollLookupTableModal.displayName;
        } else if (this.type == "ToLoc") {
            this.TlocID = Number(this.PayRollLookupTableModal.id);
            this.TlocationName = this.PayRollLookupTableModal.displayName;
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
    RadioChange(val) {
        if (val == 'AllowanceSummary') {
          this.typeID = 1;
          this.AllowancetypeID = 1;
        } else {
            this.typeID = 0;
            this.empTypeName = '';
        }
      }


}
