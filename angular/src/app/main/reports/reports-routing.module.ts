import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LedgerRptComponent } from './FinancialReports/ledger-rpt/ledger-rpt.component';
import { APTransactionListComponent } from './FinancialReports/aptransactionlist/aptransactionlist.component';
import { VoucherpringReportComponent } from './FinancialReports/voucherpring-report/voucherpring-report.component';
import { CashBookReportComponent } from './FinancialReports/cash-book-report/cash-book-report.component';
import { ChartOfAccountListingReportComponent } from "./FinancialReports/chartofacclisting/chartofacclisting.component";
import { GeneralLedgerReportComponent } from "./FinancialReports/generalLedger/generalLedger.component";
import { PartyBalancesComponent } from './FinancialReports/partyBalances/PartyBalances.component';
import { TrialBalanceComponent } from './FinancialReports/TrialBalance/TrialBalance.component';
import { SubledgerTrailComponent } from './FinancialReports/subledger-trail/subledger-trail.component';
import { ReportViewComponent } from './ReportView/ReportView.Component';
import { ItemLedgerComponent } from './supplyChain/inventory/ItemLedger/ItemLedger.component';
import { DocumentPrintingComponent } from './supplyChain/inventory/documentPrinting/documentPrinting.component';
import { ActivityReportsComponent } from './supplyChain/inventory/activityReports/activityReports.component';
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
import{AssteRegisterReportsComponent} from './supplychain/inventory/asste-register-reports/asste-register-reports.component';
const routes: Routes = [
];

@NgModule({
  imports: [RouterModule.forChild([

    {
      path: '',
      children: [
        { path: 'alert-log', component: AlertLogComponent, data: { permission: 'FinanceReports.CashBook' } },
        { path: 'FinancialReports/customer-aging-report', component: CustomerAgingReportComponent, data: { permission: 'FinanceReports.CashBook' } },
        { path: 'ReportView', component: ReportViewComponent, data: { permission: 'Pages.ReportView' } },
        { path: 'FinancialReports/cash-book-report', component: CashBookReportComponent, data: { permission: 'FinanceReports.CashBook', viewOption: 'CashBook' } },
        { path: 'FinancialReports/Bank-book-report', component: CashBookReportComponent, data: { permission: 'FinanceReports.CashBook', viewOption: 'BankBook' } },
        { path: 'FinancialReports/setup', component: GLSetupReportsComponent, data: { permission: 'FinanceReports.Setup' } },
        { path: 'FinancialReports/postDatedChequesReports', component: PostDatedChequesReportsComponent, data: { permission: 'FinanceReports.Setup' } },
        { path: 'FinancialReports/ledger-rpt', component: LedgerRptComponent, data: { permission: 'FinanceReports.Ledger' } },
        { path: 'FinancialReports/aptransactionlist', component: APTransactionListComponent, data: { permission: 'FinanceReports.TransactionListing' } },
        { path: 'FinancialReports/bankReconcile-rpt', component: BankReconcileReportComponent, data: {permission: 'FinanceReports.BankReconcilation'} },
        { path: 'FinancialReports/voucherpring-report', component: VoucherpringReportComponent, data: { permission: 'FinanceReports.VoucherPrinting' } },
        { path: 'FinancialReports/chartofacclisting', component: ChartOfAccountListingReportComponent, data: { permission: 'FinanceReports.ChartOfACListing' } },
        { path: 'FinancialReports/generalLedger', component: GeneralLedgerReportComponent, data: { permission: 'Financial.GeneralLedger' } },
        //{ path: 'FinancialReports/chartofacclisting', component: ChartOfAccountListingReportComponent, data: { permission: 'Financial.ChartOfAccListing' }  },
        { path: 'FinancialReports/PartyBalances', component: PartyBalancesComponent, data: { permission: 'FinanceReports.PartyBalances' } },
        { path: 'FinancialReports/TrialBalance', component: TrialBalanceComponent, data: { permission: 'FinanceReports.TrialBalances' } },
        { path: 'FinancialReports/subledger-trail', component: SubledgerTrailComponent, data: { permission: '' } },
        { path: 'FinancialReports/taxCollection', component: TaxCollectionComponent, data: { permission: 'FinanceReports.TaxCollection' } },
        { path: 'FinancialReports/lcExpenses', component: LCExpensesReportsComponent, data: { permission: 'FinanceReports.LCExpenses' } },
       
        { path: 'FinancialReports/BalanceSheet', component: BalanceSheetComponent, data: { permission: 'FinanceReports.ProfitAndLossStatement' } },
        { path: 'FinancialReports/PLSTATEMENT', component: PorfitAndLossStatmentComponent, data: { permission: 'FinanceReports.ProfitAndLossStatement' } },
        { path: 'FinancialReports/plStatementCategoryDetail', component: PlStatementCategoryDetailComponent, data: { permission: 'FinanceReports.ProfitAndLossStatement' } },
        { path: 'FinancialReports/plStatementVoucherDetail', component: PlStatementVoucherDetailComponent, data: { permission: 'FinanceReports.ProfitAndLossStatement' } },
       
        // { path: 'FinancialReports/chartofacclisting', component: ChartOfAccountListingReportComponent, data: { permission: 'Financial.ChartOfAccListing' }  },
        // { path: 'FinancialReports/PartyBalances', component: PartyBalancesComponent, data: { permission: 'Pages.PartyBalances' }  }
        { path: 'supplyChain/inventory/itemLedger', component: ItemLedgerComponent, data: { permission: 'InventoryReport.ItemLedger' } },
        { path: 'supplyChain/inventory/documentPrinting', component: DocumentPrintingComponent, data: { permission: 'InventoryReport.DocumentPrinting' } },
        { path: 'supplyChain/inventory/consumptionReports', component: ConsumptionReportsComponent, data: { permission: 'InventoryReport.Consumption' } },
        { path: 'supplyChain/inventory/activityReports', component: ActivityReportsComponent, data: { permission: 'InventoryReport.Activity' } },
        { path: 'supplyChain/sale/documentPrinting', component: SaleDocumentPrintingComponent, data: { permission: 'SaleReports.DocumentPrinting' } },
        { path: 'supplychain/inventory/asste-register-reports', component: AssteRegisterReportsComponent, data: { permission: 'InventoryReport.AssetRegReports' } },

        { path: 'payRoll/attendanceReports', component: AttendanceReportsComponent, data: { permission: 'PayRollReports.AttendanceReports' } },
        { path: 'payRoll/employeeReports', component: EmployeeReportsComponent, data: { permission: 'PayRollReports.EmployeeReports' } },
        { path: 'payRoll/salaryReports', component: SalaryReportsComponent, data: { permission: 'PayRollReports.SalaryReports' } },
        { path: 'payRoll/setupReports', component: SetupReportsComponent, data: { permission: 'PayRollReports.SetupReports' } },
        { path: 'payroll/allowances-reports', component: AllowancesReportsComponent, data: { permission: 'PayRollReports.AllowanceReports' } },
      
      ]
    }
  ])],
  exports: [RouterModule]
})
export class ReportsRoutingModule { }


