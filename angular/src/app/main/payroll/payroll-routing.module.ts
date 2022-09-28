import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SectionsComponent } from './sections/sections.component';
import { DesignationComponent } from './designation/designation.component';
import { GradeComponent } from './grade/grade.component';
import { ShiftComponent } from './shift/shift.component';
import { DepartmentsComponent } from './departments/departments.component';
import { LocationComponent } from './location/location.component';
import { EmployeeTypeComponent } from './employeeType/employeeType.component';
import { EmployeeEarningsComponent } from './employeeEarnings/employeeEarnings.component';
import { EmployeeArrearsComponent } from './employeeArrears/employeeArrears.component';
import { EmployeeDeductionsComponent } from './employeeDeductions/employeeDeductions.component';
import { EducationComponent } from './education/education.component';
import { ReligionComponent } from './religion/religion.component';
import { EmployeeSalaryComponent } from './employeeSalary/employeeSalary.component';
import { AttendanceComponent } from './attendance/attendance.component';
import { EmployeeLeavesComponent } from './employeeLeaves/employeeLeaves.component';
import { EmployeeComponent } from './employee/employee.component';
import { SalarySheetComponent } from './salarySheet/salarySheet.component';
import { AttendanceV2Component } from './attendanceV2/attendanceV2.component';
import { DeductionTypesComponent } from './deductionTypes/deductionTypes.component';
import { EarningTypesComponent } from './earningTypes/earningTypes.component';
import { HolidaysComponent } from './holidays/holidays.component';
import { AllowanceSetupComponent } from './allowanceSetup/allowanceSetup.component';
import { AllowancesComponent } from './allowances/allowances.component';
import { SubDesignationsComponent } from './subDesignations/subDesignations.component';
import { EmployeeLoansComponent } from './employeeLoans/employeeLoans.component';
import { EmployeeLoansTypeComponent } from './employeeLoansType/employeeLoansType.component';
import { EmployeeAdvancesComponent } from './employeeAdvances/employeeAdvances.component';
import { SalaryLoanStopComponent } from './salaryLoanStop/salaryLoanStop.component';
import { EmployeeLeaveBalanceComponent } from './employeeLeaveBalance/employeeLeaveBalance.component';
import { TaxSlabsComponent } from './taxSlabs/taxSlabs-modal.component';
import { SalarylockComponent } from './salarylock/salarylock.component';
import {MonthlyCprComponent} from './monthlyCpr/monthlyCpr.component';
import { CaderComponent } from './cader/cader.component';
import { CaderHDComponent } from './cader-hd/cader-hd.component';
import { HrmsetupComponent } from './hrmsetup/hrmsetup.component';


const routes: Routes = [
    {
        path: '',
        children: [
            { path: 'hrmsetup', component: HrmsetupComponent, data: { permission: 'PayRoll.HrmSetup' } },
            { path: 'subDesignations', component: SubDesignationsComponent, data: { permission: 'PayRoll.SubDesignations.Setup' } },
            { path: 'allowance', component: AllowancesComponent, data: { permission: 'PayRoll.Allowances.Transactions' } },
            { path: 'allowanceSetup', component: AllowanceSetupComponent, data: { permission: 'PayRoll.AllowanceSetup.Setup' } },
            { path: 'holidays', component: HolidaysComponent, data: { permission: 'PayRoll.Holidays.Setup' } },
            { path: 'earningTypes', component: EarningTypesComponent, data: { permission: 'PayRoll.EarningTypes.Setup' } },
            { path: 'deductionTypes', component: DeductionTypesComponent, data: { permission: 'PayRoll.DeductionTypes.Setup' } },
            { path: 'departments', component: DepartmentsComponent, data: { permission: 'PayRoll.Departments.Setup' } },
            { path: 'designation', component: DesignationComponent, data: { permission: 'PayRoll.Designations.Setup' } },
            { path: 'grade', component: GradeComponent, data: { permission: 'PayRoll.Grades.Setup' } },
            { path: 'shift', component: ShiftComponent, data: { permission: 'PayRoll.Shifts.Setup' } },
            { path: 'sections', component: SectionsComponent, data: { permission: 'PayRoll.Sections.Setup' } },
            { path: 'location', component: LocationComponent, data: { permission: 'PayRoll.Locations.Setup' } },
            { path: 'employees', component: EmployeeComponent, data: { permission: 'PayRoll.Employees.Setup' } },
            { path: 'employeeType', component: EmployeeTypeComponent, data: { permission: 'PayRoll.EmployeeType.Setup' } },
            { path: 'employeeEarnings', component: EmployeeEarningsComponent, data: { permission: 'PayRoll.EmployeeEarnings.Transactions' } },
            { path: 'employeeArrears', component: EmployeeArrearsComponent, data: { permission: 'PayRoll.EmployeeArrears.Transactions' } },
            { path: 'employeeDeductions', component: EmployeeDeductionsComponent, data: { permission: 'PayRoll.EmployeeDeductions.Transactions' } },
            { path: 'employeeSalary', component: EmployeeSalaryComponent, data: { permission: 'PayRoll.EmployeeSalary.Setup' } },
            { path: 'education', component: EducationComponent, data: { permission: 'PayRoll.Education.Setup' } },
            { path: 'religion', component: ReligionComponent, data: { permission: 'PayRoll.Religions.Setup' } },
            { path: 'attendance', component: AttendanceV2Component, data: { permission: 'PayRoll.Attendance.Transactions' } },
            { path: 'bulkAttendance', component: AttendanceComponent, data: { permission: 'PayRoll.AttendanceHeader.Transactions' } },
            { path: 'employeeLeaves', component: EmployeeLeavesComponent, data: { permission: 'PayRoll.EmployeeLeaves.Transactions' } },
            { path: 'salarySheet', component: SalarySheetComponent, data: { permission: 'PayRoll.SalarySheet.Transactions' } },
            { path: 'employeeLoans', component: EmployeeLoansComponent, data: { permission: 'PayRoll.EmployeeLoans.Transactions' } },
            { path: 'employeeLoansType', component: EmployeeLoansTypeComponent, data: { permission: 'PayRoll.EmployeeLoansType.Setup' } },
            { path: 'employeeAdvances', component: EmployeeAdvancesComponent, data: { permission: 'PayRoll.EmployeeAdvances.Transactions' } },
            { path: 'salaryLoanStop', component: SalaryLoanStopComponent, data: { permission: 'PayRoll.StopSalary.Transactions' } },
            { path: 'employeeLeaveBalance', component: EmployeeLeaveBalanceComponent, data: { permission: 'PayRoll.EmployeeLeavesTotal.Setup' } },   
            { path: 'cader', component: CaderComponent, data: { permission: 'PayRoll.Cader' } },   
            { path: 'taxSlabs', component: TaxSlabsComponent, data: { permission: 'PayRoll.SlabSetup.Setup' } },
            { path: 'salarylock', component: SalarylockComponent, data: { permission: 'Pages.SalaryLock' } },
            { path: 'monthlyCpr', component: MonthlyCprComponent, data: { permission: 'Pages.MonthlyCPR' } },
            { path: 'cader-hd', component: CaderHDComponent, data: { permission: 'Pages.Cader_link_H' } },
            
        ]
        

    }];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class PayrollRoutingModule { }
