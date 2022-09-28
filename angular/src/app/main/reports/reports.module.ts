import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportsRoutingModule } from './reports-routing.module';
import { LedgerRptComponent } from './FinancialReports/ledger-rpt/ledger-rpt.component';
import { FileUploadModule } from 'ng2-file-upload';
import { AutoCompleteModule, PaginatorModule, EditorModule, InputMaskModule } from 'primeng/primeng';
import { TableModule } from 'primeng/table';
import { FormsModule } from '@angular/forms';
import { ModalModule, TabsModule, TooltipModule, BsDatepickerModule, BsDropdownModule, PopoverModule, BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import CountoModule from 'angular2-counto';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AgGridModule } from 'ag-grid-angular';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { ReportsComponent } from './reports.component';
import { ReportFilterServiceProxy, BanksServiceProxy, VoucherPrintingReportsServiceProxy } from '@shared/service-proxies/service-proxies';
import { ChartofcontrolLookupFinderComponent } from './chartofcontrol-lookup-finder/chartofcontrol-lookup-finder.component';
import { APTransactionListComponent } from './FinancialReports/aptransactionlist/aptransactionlist.component';
import { VoucherpringReportComponent } from './FinancialReports/voucherpring-report/voucherpring-report.component';
import { CashBookReportComponent } from './FinancialReports/cash-book-report/cash-book-report.component';
import { SubledgerLookupFinderComponent } from './subledger-lookup-finder/subledger-lookup-finder.component';
import {ChartOfAccountListingReportComponent} from "./FinancialReports/chartofacclisting/chartofacclisting.component";
import {GeneralLedgerReportComponent} from "./FinancialReports/generalLedger/generalLedger.component";
import { PartyBalancesComponent } from './FinancialReports/partyBalances/PartyBalances.component';
import { TrialBalanceComponent } from './FinancialReports/TrialBalance/TrialBalance.component';
import { SubledgerTrailComponent } from './FinancialReports/subledger-trail/subledger-trail.component';
import { ReportViewComponent } from './ReportView/ReportView.Component';
import { ItemLedgerComponent } from './supplyChain/inventory/ItemLedger/ItemLedger.component';
import { ItemLookupTableModalComponent } from './item-lookup-finder/item-lookup-table-modal.component';
import { LocLookupTableModalComponent } from './loc-lookup-finder/loc-lookup-table-modal.component';
import { DocumentPrintingComponent } from './supplyChain/inventory/documentPrinting/documentPrinting.component';
import { ActivityReportsComponent } from './supplyChain/inventory/activityReports/activityReports.component';
import { ReportViewerModule } from 'ngx-ssrs-reportviewer';
import { ReportViewService } from './ReportView/reportView.service';
import { FindersModule } from '@app/finders/finders.module';
import { SaleDocumentPrintingComponent } from './supplyChain/sale/documentPrinting/saleDocumentPrinting.component';
import { EmployeeReportsComponent } from './payRoll/employeeReports/employeeReports.component';
import { SalaryReportsComponent } from './payRoll/salaryReports/salaryReports.component';
import { SetupReportsComponent } from './payRoll/setupReports/setupReports.component';
import { AttendanceReportsComponent } from './payRoll/attendanceReports/attendanceReports.component';
import { BankReconcileReportComponent } from './FinancialReports/bankReconcile/bankReconcile-rpt.component';
import { CustomerAgingReportComponent } from './FinancialReports/customer-aging-report/customer-aging-report.component';
import { ConsumptionReportsComponent } from './supplyChain/inventory/consumptionReports/consumptionReports.component';
import { GLSetupReportsComponent } from './generalLedger/setupReports/setupReports.component';
import { TaxCollectionComponent } from './FinancialReports/taxCollection/taxCollection.component';
import { AlertLogComponent } from './alert-log/alert-log.component';
import { PorfitAndLossStatmentComponent } from './FinancialReports/porfit-and-loss-statment/porfit-and-loss-statment.component';
import { PostDatedChequesReportsComponent } from './FinancialReports/PostDatedChequesReports/PostDatedChequesReports.component';
import { LCExpensesReportsComponent } from './FinancialReports/lcExpenses/lcExpensesReports.component';
import { PlStatementCategoryDetailComponent } from './FinancialReports/plStatementCategoryDetail/plStatementCategoryDetail.component';
import { PlStatementVoucherDetailComponent } from './FinancialReports/PlStatementVoucherDetail/PlStatementVoucherDetail.component';
import { BalanceSheetComponent } from './FinancialReports/balance-sheet/balance-sheet';
import { AllowancesReportsComponent } from './payroll/allowances-reports/allowances-reports.component';

import { AssteRegisterReportsComponent } from './supplychain/inventory/asste-register-reports/asste-register-reports.component';
// import { MultiSelectDropdownComponent } from './multi-select-dropdown/multi-select-dropdown.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';


@NgModule({
	declarations: [
		ChartOfAccountListingReportComponent,
		GeneralLedgerReportComponent,
		LedgerRptComponent,
		ReportsComponent,
		ChartofcontrolLookupFinderComponent,
		APTransactionListComponent,
		BankReconcileReportComponent,
		VoucherpringReportComponent,
		CashBookReportComponent,
		SubledgerLookupFinderComponent,
		PartyBalancesComponent,
		TrialBalanceComponent,
		ReportViewComponent,
		SubledgerTrailComponent,
		ItemLedgerComponent,
		ItemLookupTableModalComponent,
		LocLookupTableModalComponent,
		DocumentPrintingComponent,
		ConsumptionReportsComponent,
		ActivityReportsComponent,
		SaleDocumentPrintingComponent,
        EmployeeReportsComponent,
        SalaryReportsComponent,
        SetupReportsComponent,
        AttendanceReportsComponent,
        CustomerAgingReportComponent,
        GLSetupReportsComponent,
        TaxCollectionComponent,
        AlertLogComponent,
		PorfitAndLossStatmentComponent,
		PostDatedChequesReportsComponent,
		LCExpensesReportsComponent,
		PlStatementCategoryDetailComponent,
		PlStatementVoucherDetailComponent,
		BalanceSheetComponent,
		AllowancesReportsComponent,
		
		AssteRegisterReportsComponent,
		
		// MultiSelectDropdownComponent
	
	],
	imports: [
		FileUploadModule,
		AutoCompleteModule,
		PaginatorModule,
		EditorModule,
		InputMaskModule,
		TableModule,
		CommonModule,
		FormsModule,
		ModalModule,
		TabsModule,
		TooltipModule,
		AppCommonModule,
		ReportViewerModule,
		UtilsModule,
		ReportsRoutingModule,
		CountoModule,
		NgxChartsModule,
		BsDatepickerModule.forRoot(),
		BsDropdownModule.forRoot(),
		PopoverModule.forRoot(),
		AgGridModule.withComponents(null),
		NgMultiSelectDropDownModule.forRoot(),
		FindersModule
	],

	providers: [
		{ provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
		{ provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
		{ provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale },
		ReportFilterServiceProxy,
		BanksServiceProxy,VoucherPrintingReportsServiceProxy,
		ReportViewService

	],
	schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ReportsModule { }
