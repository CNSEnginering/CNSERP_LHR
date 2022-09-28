import {
    Component,
    Injector,
    ViewEncapsulation,
    ViewChild,
    EventEmitter,
    Output,
    OnInit,
} from "@angular/core";
import { Router } from "@angular/router";
import { AppComponentBase } from "@shared/common/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import * as _ from "lodash";
import * as moment from "moment";
import { FiscalDateService } from "@app/shared/services/fiscalDate.service";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";
import { PayRollLookupTableModalComponent } from "@app/finders/payRoll/payRoll-lookup-table-modal.component";
import { ReportFilterServiceProxy } from "@shared/service-proxies/service-proxies";
@Component({
    templateUrl: "./salaryReports.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class SalaryReportsComponent extends AppComponentBase implements OnInit {
    @ViewChild("reportView", { static: true })
    reportView: ReportviewrModalComponent;
    @ViewChild("PayRollLookupTableModal", { static: true })
    PayRollLookupTableModal: PayRollLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    target: any;
    type: string = "";
    typeID: number = 0;
    empTypeName: string = "";
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
    locationName: string;
    TlocationName: string;
    empTypeId: number;
    locID: number = 0;
    TlocID: number = 99999;
    yearAndMonth = new Date();
    salaryYear: number;
    salaryMonth: number;
    empType;
    constructor(injector: Injector, private route: Router,
        private reportFilterServiceProxy: ReportFilterServiceProxy) {
        super(injector);
    }
    ngOnInit(): void {
        this.reportFilterServiceProxy.getLoanTypes().subscribe(
            data => {
                console.log(data);
                this.empType = data["result"];
                this.empTypeId = 1;
            }
        );
    }

    onOpenCalendar(container) {
        container.monthSelectHandler = (event: any): void => {
            container._store.dispatch(container._actions.select(event.date));
        };
        container.setViewMode("month");
    }
    openLocationModal(type) {
        ;
        this.target = "Location";
        this.type = type;
        this.PayRollLookupTableModal.id = String(this.locID);
        this.PayRollLookupTableModal.displayName = this.locationName;
        this.PayRollLookupTableModal.show(this.target);
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
    getReport() {
        debugger;

        
           
        
        switch (this.reportCheck) {
            case "salarySheet":
                this.processReport("SalarySheet");
                break;

            case "deductionRegister":
                    this.processReport("DeductionRegister");
                    break;
            case "allowanceRegister":
                    this.processReport("AllowanceRegister");
                    break;
    
            case "salarySheetSummary":
                this.processReport("SalarySheetSummary");
                break;
            case "disbursmentSummary":
                this.processReport("disbursmentSummary");
                break;
            case "salarySlips":
                this.processReport("SalarySlips");
                break;
            case "bankAdvicePermanent":
                this.processReport("BankAdvicePermanent");
                break;
            case "bankAdviceContract":
                this.processReport("BankAdviceContract");
                break;
            case "loanLedger":
                this.processReport("LoanLedger");
                break;
            default:
                break;
        }
    }

    processReport(report: string) {
        debugger;
        if (this.yearAndMonth == null) {
            this.notify.error(this.l("Please Select Year and Month"));
            return;
        }
        let repParams = "";
        this.salaryYear = this.yearAndMonth.getFullYear();
        this.salaryMonth = this.yearAndMonth.getMonth() + 1;
        switch (report) {
            case "SalarySheet":
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
                if (this.salaryYear !== undefined)
                    repParams += encodeURIComponent("" + this.salaryYear) + "$";
                if (this.salaryMonth !== undefined)
                    repParams += encodeURIComponent("" + this.salaryMonth) + "$";
                if (this.locID !== undefined)
                    repParams += encodeURIComponent("" + this.locID) + "$";
                if (this.TlocID !== undefined)
                    repParams += encodeURIComponent("" + this.TlocID) + "$";
                if (this.typeID !== undefined)
                    repParams += encodeURIComponent("" + this.typeID) + "$";
                repParams += encodeURIComponent("false") + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;

            case "DeductionRegister":
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
                if (this.salaryYear !== undefined)
                    repParams += encodeURIComponent("" + this.salaryYear) + "$";
                if (this.salaryMonth !== undefined)
                    repParams += encodeURIComponent("" + this.salaryMonth) + "$";
                if (this.locID !== undefined)
                    repParams += encodeURIComponent("" + this.locID) + "$";
                if (this.TlocID !== undefined)
                    repParams += encodeURIComponent("" + this.TlocID) + "$";
                if (this.typeID !== undefined)
                    repParams += encodeURIComponent("" + this.typeID) + "$";
                repParams += encodeURIComponent("false") + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;

                case "AllowanceRegister":
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
                if (this.salaryYear !== undefined)
                    repParams += encodeURIComponent("" + this.salaryYear) + "$";
                if (this.salaryMonth !== undefined)
                    repParams += encodeURIComponent("" + this.salaryMonth) + "$";
                if (this.locID !== undefined)
                    repParams += encodeURIComponent("" + this.locID) + "$";
                if (this.TlocID !== undefined)
                    repParams += encodeURIComponent("" + this.TlocID) + "$";
                if (this.typeID !== undefined)
                    repParams += encodeURIComponent("" + this.typeID) + "$";
                repParams += encodeURIComponent("false") + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;

            case "SalarySheetSummary":
                // if (this.salaryYear !== undefined)
                //     repParams += encodeURIComponent("" + this.salaryYear) + "$";
                // if (this.salaryMonth !== undefined)
                //     repParams += encodeURIComponent("" + this.salaryMonth) + "$";
                // repParams = repParams.replace(/[?$]&/, "");

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
                if (this.salaryYear !== undefined)
                    repParams += encodeURIComponent("" + this.salaryYear) + "$";
                if (this.salaryMonth !== undefined)
                    repParams +=
                        encodeURIComponent("" + this.salaryMonth) + "$";
                if (this.typeID !== undefined)
                    repParams += encodeURIComponent("" + this.typeID) + "$";
                repParams += encodeURIComponent("true") + "$";
                repParams = repParams.replace(/[?$]&/, "");

                break;
            case "disbursmentSummary":
                if (this.salaryYear !== undefined)
                    repParams += encodeURIComponent("" + this.salaryYear) + "$";
                if (this.salaryMonth !== undefined)
                    repParams +=
                        encodeURIComponent("" + this.salaryMonth) + "$";
                repParams = repParams.replace(/[?$]&/, "");

                break;

            case "SalarySlips":
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
                if (this.salaryYear !== undefined)
                    repParams += encodeURIComponent("" + this.salaryYear) + "$";
                if (this.salaryMonth !== undefined)
                    repParams +=
                        encodeURIComponent("" + this.salaryMonth) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;

            case "LoanLedger":
                if (this.fromEmpID !== undefined)
                    repParams += encodeURIComponent("" + this.fromEmpID) + "$";
                if (this.toEmpID !== undefined)
                    repParams += encodeURIComponent("" + this.toEmpID) + "$";
                if (this.toEmpID !== undefined)
                    repParams += encodeURIComponent("" + this.empTypeId) + "$";

                repParams = repParams.replace(/[?$]&/, "");
                break;

            case "BankAdvicePermanent":
                if (this.salaryMonth !== undefined)
                    repParams +=
                        encodeURIComponent("" + this.salaryMonth) + "$";
                if (this.salaryYear !== undefined)
                    repParams += encodeURIComponent("" + this.salaryYear) + "$";
                //repParams += encodeURIComponent("" + 1) + "$";

                repParams = repParams.replace(/[?$]&/, "");
                break;

            case "BankAdviceContract":
                if (this.salaryMonth !== undefined)
                    repParams +=
                        encodeURIComponent("" + this.salaryMonth) + "$";
                if (this.salaryYear !== undefined)
                    repParams += encodeURIComponent("" + this.salaryYear) + "$";
                //repParams += encodeURIComponent("" + 2) + "$";

                repParams = repParams.replace(/[?$]&/, "");
                break;

            default:
                break;
        }
        debugger
        this.reportView.show(report, repParams);
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
            default:
                break;
        }
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

    getNewEmpType() {
        debugger
        if(this.type=='EmpType'){
            this.typeID = Number(this.PayRollLookupTableModal.id);
            this.empTypeName = this.PayRollLookupTableModal.displayName;
        }
        
    }

    setEmpTypeNull() {
        this.typeID = 0;
        this.empTypeName = "";
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
