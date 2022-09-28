import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PayrollRoutingModule } from './payroll-routing.module';
import { DesignationComponent } from './designation/designation.component';
import { CreateOrEditDesignationModalComponent } from './designation/create-or-edit-designation-modal.component';
import { ViewDesignationModalComponent } from './designation/view-designation-modal.component';
import { FileUploadModule, AutoCompleteModule, PaginatorModule, EditorModule, InputMaskModule, TooltipModule, InputTextModule, DialogModule, ButtonModule } from 'primeng/primeng';
import { TableModule } from 'primeng/table';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule, TabsModule, BsDatepickerModule, BsDropdownModule, PopoverModule, BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService, TimepickerModule, ButtonsModule } from 'ngx-bootstrap';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import CountoModule from 'angular2-counto';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AgGridModule } from 'ag-grid-angular';
import { FindersModule } from '@app/finders/finders.module';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from 'abp-ng2-module/dist/src/abpHttpInterceptor';
import { GradeComponent } from './grade/grade.component';
import { CreateOrEditGradeModalComponent } from './grade/create-or-edit-grade-modal.component';
import { ViewGradeModalComponent } from './grade/view-grade-modal.component';
import { ShiftComponent } from './shift/shift.component';
import { CreateOrEditShiftModalComponent } from './shift/create-or-edit-shift-modal.component';
import { ViewShiftModalComponent } from './shift/view-shift-modal.component';
import { ViewSectionModalComponent } from './sections/view-section-modal.component';
import { CreateOrEditSectionModalComponent } from './sections/create-or-edit-section-modal.component';
import { SectionsComponent } from './sections/sections.component';
import { DepartmentsComponent } from './departments/departments.component';
import { CreateOrEditDepartmentModalComponent } from './departments/create-or-edit-departments-modal.component';
import { ViewDepartmentModalComponent } from './departments/view-departments-modal.component';
import { DepartmentsServiceProxy } from './shared/services/department.service';
import { LocationComponent } from './location/location.component';
import { CreateOrEditLocationModalComponent } from './location/create-or-edit-location-modal.component';
import { ViewLocationModalComponent } from './location/view-location-modal.component';
import { EmployeeTypeComponent } from './employeeType/employeeType.component';
import { CreateOrEditEmployeeTypeModalComponent } from './employeeType/create-or-edit-employeeType-modal.component';
import { ViewEmployeeTypeModalComponent } from './employeeType/view-employeeType-modal.component';
import { EmployeeEarningsComponent } from './employeeEarnings/employeeEarnings.component';
import { CreateOrEditEmployeeEarningsModalComponent } from './employeeEarnings/create-or-edit-employeeEarnings-modal.component';
import { ViewEmployeeEarningsModalComponent } from './employeeEarnings/view-employeeEarnings-modal.component';
import { EmployeeArrearsComponent } from './employeeArrears/employeeArrears.component';
import { CreateOrEditEmployeeArrearsModalComponent } from './employeeArrears/create-or-edit-employeeArrears-modal.component';
import { ViewEmployeeArrearsModalComponent } from './employeeArrears/view-employeeArrears-modal.component';
import { EmployeeDeductionsComponent } from './employeeDeductions/employeeDeductions.component';
import { CreateOrEditEmployeeDeductionsModalComponent } from './employeeDeductions/create-or-edit-employeeDeductions-modal.component';
import { ViewEmployeeDeductionsModalComponent } from './employeeDeductions/view-employeeDeductions-modal.component';
import { EducationComponent } from './education/education.component';
import { ReligionComponent } from './religion/religion.component';
import { CreateOrEditEducationModalComponent } from './education/create-or-edit-education-modal.component';
import { ViewEducationModalComponent } from './education/view-education-modal.component';
import { CreateOrEditReligionModalComponent } from './religion/create-or-edit-religion-modal.component';
import { ViewReligionModalComponent } from './religion/view-religion-modal.component';
import { EmployeeSalaryComponent } from './employeeSalary/employeeSalary.component';
import { CreateOrEditEmployeeSalaryModalComponent } from './employeeSalary/create-or-edit-employeeSalary-modal.component';
import { ViewEmployeeSalaryModalComponent } from './employeeSalary/view-employeeSalary-modal.component';
import { AttendanceComponent } from './attendance/attendance.component';
import { CreateOrEditAttendanceModalComponent } from './attendance/create-or-edit-attendance-modal.component';
import { ViewAttendanceModalComponent } from './attendance/view-attendance-modal.component';
import { TimeEditorComponent } from './attendance/timeComponents/time-editor.component';
import { TimeRendererComponent } from './attendance/timeComponents/time-renderer.component';
import { EmployeeLeavesComponent } from './employeeLeaves/employeeLeaves.component';
import { CreateOrEditEmployeeLeavesModalComponent } from './employeeLeaves/create-or-edit-employeeLeaves-modal.component';
import { ViewEmployeeLeavesModalComponent } from './employeeLeaves/view-employeeLeaves-modal.component';
import { EmployeeComponent } from './employee/employee.component';
import { ViewEmployeeModalComponent } from './employee/view-employee-modal.component';
import { CreateOrEditEmployeesModalComponent } from './employee/create-or-edit-employee-modal.component';
import { SalarySheetComponent } from './salarySheet/salarySheet.component';
import { AttendanceV2Component } from './attendanceV2/attendanceV2.component';
import { LightboxModule } from 'ngx-lightbox';
import { DeductionTypesComponent } from './deductionTypes/deductionTypes.component';
import { CreateOrEditDeductionTypesModalComponent } from './deductionTypes/create-or-edit-deductionTypes-modal.component';
import { ViewDeductionTypesModalComponent } from './deductionTypes/view-deductionTypes-modal.component';
import { CurrencyMaskModule } from "ng2-currency-mask";
import { EarningTypesComponent } from './earningTypes/earningTypes.component';
import { CreateOrEditEarningTypesModalComponent } from './earningTypes/create-or-edit-earningTypes-modal.component';
import { ViewEarningTypesModalComponent } from './earningTypes/view-earningTypes-modal.component';
import { HolidaysComponent } from './holidays/holidays.component';
import { CreateOrEditHolidaysModalComponent } from './holidays/create-or-edit-holidays-modal.component';
import { ViewHolidaysModalComponent } from './holidays/view-holidays-modal.component';
import { MarkBulkAttendanceV2ModalComponent } from './attendanceV2/mark-bulk-attendanceV2-modal.component';
import { AllowanceSetupComponent } from './allowanceSetup/allowanceSetup.component';
import { CreateOrEditAllowanceSetupModalComponent } from './allowanceSetup/create-or-edit-allowanceSetup-modal.component';
import { ViewAllowanceSetupModalComponent } from './allowanceSetup/view-allowanceSetup-modal.component';
import { AllowancesComponent } from './allowances/allowances.component';
import { CreateOrEditAllowancesModalComponent } from './allowances/create-or-edit-allowances-modal.component';
import { ViewAllowancesModalComponent } from './allowances/view-allowances-modal.component';
import { SubDesignationsComponent } from './subDesignations/subDesignations.component';
import { CreateOrEditSubDesignationsModalComponent } from './subDesignations/create-or-edit-subDesignations-modal.component';
import { ViewSubDesignationsModalComponent } from './subDesignations/view-subDesignations-modal.component';
import { EmployeeLoansComponent } from './employeeLoans/employeeLoans.component';
import { CreateOrEditEmployeeLoansModalComponent } from './employeeLoans/create-or-edit-employeeLoans-modal.component';
import { CreateOrEditEmployeeLoansTypeModalComponent } from './employeeLoansType/create-or-edit-employee-loans-type-modal.component';
import { EmployeeLoansTypeComponent } from './employeeLoansType/employeeLoansType.component';
import { EmployeeAdvancesComponent } from './employeeAdvances/employeeAdvances.component';
import { CreateOrEditEmployeeAdvancesModalComponent } from './employeeAdvances/create-or-edit-employee-advances-modal.component';
import { ViewEmployeeAdvancesModalComponent } from './employeeAdvances/view-employeeAdvances-modal.component';
import { SalaryLoanStopComponent } from './salaryLoanStop/salaryLoanStop.component';
import { CreateOrEditSalaryLoanStopModalComponent } from './salaryLoanStop/create-or-edit-salaryLoanStop-modal.component';
import { CheckboxCellComponent } from '@app/shared/common/checkbox-cell/checkbox-cell.component';
import { EmployeeLeaveBalanceComponent } from './employeeLeaveBalance/employeeLeaveBalance.component';
import { CreateOrEditEmployeeLeaveBalanceModalComponent } from './employeeLeaveBalance/create-or-edit-employeeLeaveBalance-modal.component';
import { ViewEmployeeLeaveBalanceModalComponent } from './employeeLeaveBalance/view-employeeLeaveBalance-modal.component';
import { ViewTaxSlabsModalComponent } from './taxSlabs/view-taxSlabs-modal.component';
import { CreateOrEditTaxSlabsModalComponent } from './taxSlabs/create-or-edit-taxSlabs-modal.component';
import { TaxSlabsComponent } from './taxSlabs/taxSlabs-modal.component';
import { SalarylockComponent } from './salarylock/salarylock.component';
import { CreateOrEditSalarylockComponent } from './salarylock/create-or-edit-salarylock.component';
import { MonthlyCprComponent } from './monthlyCpr/monthlyCpr.component';
import { CreateOrEditMonthlyCprComponent } from './monthlyCpr/create-or-edit-monthlyCpr.component';
import { CaderComponent } from './cader/cader.component';
import { CreateOrEditCaderComponent } from './cader/createoreditcader.component';
import { CaderHDComponent } from './cader-hd/cader-hd.component';
import { CreateOrEditCaderHDComponent } from './cader-hd/cader-hd-createor-edit.component';
import { FinanceModule } from '../finance/finance.module';
import { HrmsetupComponent } from './hrmsetup/hrmsetup.component';






@NgModule({
    declarations: [
        DepartmentsComponent,
        CreateOrEditDepartmentModalComponent,
        ViewDepartmentModalComponent,
        DesignationComponent,
        CreateOrEditDesignationModalComponent,
        ViewDesignationModalComponent,
        ReligionComponent,
        CreateOrEditReligionModalComponent,
        ViewReligionModalComponent,
        GradeComponent,
        CreateOrEditGradeModalComponent,
        ViewGradeModalComponent,
        ShiftComponent,
        CreateOrEditShiftModalComponent,
        ViewShiftModalComponent,
        SectionsComponent,
        CreateOrEditSectionModalComponent,
        ViewSectionModalComponent,
        LocationComponent,
        CreateOrEditLocationModalComponent,
        ViewLocationModalComponent,
        EmployeeTypeComponent,
        CreateOrEditEmployeeTypeModalComponent,
        ViewEmployeeTypeModalComponent,
        EmployeeEarningsComponent,
        CreateOrEditEmployeeEarningsModalComponent,
        ViewEmployeeEarningsModalComponent,
        EmployeeArrearsComponent,
        CreateOrEditEmployeeArrearsModalComponent,
        ViewEmployeeArrearsModalComponent,
        EmployeeDeductionsComponent,
        CreateOrEditEmployeeDeductionsModalComponent,
        ViewEmployeeDeductionsModalComponent,
        EmployeeSalaryComponent,
        CreateOrEditEmployeeSalaryModalComponent,
        ViewEmployeeSalaryModalComponent,
        EducationComponent,
        CreateOrEditEducationModalComponent,
        ViewEducationModalComponent,
        AttendanceComponent,
        CreateOrEditAttendanceModalComponent,
        ViewAttendanceModalComponent,
        TimeEditorComponent,
        TimeRendererComponent,
        EmployeeLeavesComponent,
        CreateOrEditEmployeeLeavesModalComponent,
        ViewEmployeeLeavesModalComponent,
        EmployeeComponent,
        CreateOrEditEmployeesModalComponent,
        ViewEmployeeModalComponent,
        SalarySheetComponent,
        AttendanceV2Component,
        DeductionTypesComponent,
        CreateOrEditDeductionTypesModalComponent,
        ViewDeductionTypesModalComponent,
        EarningTypesComponent,
        CreateOrEditEarningTypesModalComponent,
        ViewEarningTypesModalComponent,
        HolidaysComponent,
        CreateOrEditHolidaysModalComponent,
        ViewHolidaysModalComponent,
        MarkBulkAttendanceV2ModalComponent,
        AllowanceSetupComponent,
        CreateOrEditAllowanceSetupModalComponent,
        ViewAllowanceSetupModalComponent,
        AllowancesComponent,
        CreateOrEditAllowancesModalComponent,
        ViewAllowancesModalComponent,
        SubDesignationsComponent,
        CreateOrEditSubDesignationsModalComponent,
        ViewSubDesignationsModalComponent,
        EmployeeLoansComponent,
        CreateOrEditEmployeeLoansModalComponent,
        EmployeeLoansTypeComponent,
        CreateOrEditEmployeeLoansTypeModalComponent,
        EmployeeAdvancesComponent,
        CreateOrEditEmployeeAdvancesModalComponent,
        ViewEmployeeAdvancesModalComponent,
        SalaryLoanStopComponent,
        CreateOrEditSalaryLoanStopModalComponent,
        EmployeeLeaveBalanceComponent,
        CreateOrEditEmployeeLeaveBalanceModalComponent,
        ViewEmployeeLeaveBalanceModalComponent,
        ViewTaxSlabsModalComponent,
        CreateOrEditTaxSlabsModalComponent,
        TaxSlabsComponent,
        SalarylockComponent,
        CreateOrEditSalarylockComponent,
        MonthlyCprComponent,
        CreateOrEditMonthlyCprComponent,
        CaderComponent,
        CreateOrEditCaderComponent,
        CaderHDComponent,
        CreateOrEditCaderHDComponent,
        HrmsetupComponent
       

    ],
    imports: [
        PayrollRoutingModule,
        FileUploadModule,
        AutoCompleteModule,
        PaginatorModule,
        EditorModule,
        InputMaskModule,
        TableModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        ModalModule,
        TabsModule,
        TooltipModule,
        AppCommonModule,
        UtilsModule,
        CountoModule,
        NgxChartsModule,
        BsDatepickerModule.forRoot(),
        BsDropdownModule.forRoot(),
        PopoverModule.forRoot(),
        AgGridModule.withComponents([
            TimeEditorComponent,
            TimeRendererComponent,
            CheckboxCellComponent,
        ]),
        TimepickerModule.forRoot(),
        PopoverModule.forRoot(),
        FindersModule,
        InputTextModule,
        DialogModule,
        ButtonModule,
        LightboxModule,
        CurrencyMaskModule,
        FinanceModule
    ],
    providers: [
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale },
        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true },
        DepartmentsServiceProxy
    ]
})
export class PayrollModule { }
