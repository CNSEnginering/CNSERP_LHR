import { PermissionCheckerService } from '@abp/auth/permission-checker.service';
import { AppSessionService } from '@shared/common/session/app-session.service';

import { Injectable } from '@angular/core';
import { AppMenu } from './app-menu';
import { AppMenuItem } from './app-menu-item';
import { DTRadioButton } from 'primeng/primeng';

@Injectable()
export class AppNavigationService {

    constructor(
        private _permissionCheckerService: PermissionCheckerService,
        private _appSessionService: AppSessionService
    ) {

    }

    getMenu(): AppMenu {
        return new AppMenu('MainMenu', 'MainMenu', [
            new AppMenuItem('Dashboard', 'Pages.Administration.Host.Dashboard', 'account_balance', '/app/admin/hostDashboard'),
            new AppMenuItem('Dashboard', 'Pages.Tenant.Dashboard', 'dashboard', '/app/main/dashboard'),
            new AppMenuItem('Tenants', 'Pages.Tenants', 'roles', '/app/admin/tenants'),
            new AppMenuItem('Editions', 'Pages.Editions', 'shift', '/app/admin/editions'),
            new AppMenuItem('Business Intelligence', 'Pages.Tenant.Dashboard', 'flaticon-app', '/app/main/openbi'),


            new AppMenuItem('Company Setups', null, 'company_profile', '/app/main/commonServices', [
                new AppMenuItem('CompanyProfile', 'SetupForms.CompanyProfiles', 'company_profile', '/app/main/commonServices/companyProfiles'),
                new AppMenuItem('FiscalCalender', 'Pages.FiscalCalenders', 'fiscal_calendar', '/app/main/GeneralLedger/setupForms/fiscalCalenders'),
                new AppMenuItem('TaxAuthorities', 'Pages.TaxAuthorities', 'tax_authorities', '/app/main/commonServices/taxAuthorities'),
                new AppMenuItem('TaxClasses', 'Pages.TaxClasses', 'tax_classes', '/app/main/commonServices/taxClasses'),
                new AppMenuItem('Banks', 'Pages.Banks', 'banks', '/app/main/commonServices/banks'),
                new AppMenuItem('ChequeBooks', 'Pages.ChequeBooks', 'Cheque_Book', '/app/main/commonServices/chequeBooks'),
                new AppMenuItem('CPRNumbers', 'Pages.CPR', 'cpr', '/app/main/commonServices/cprNumbers'),
                new AppMenuItem('Currency Code', 'SetupForms.CurrencyRates', 'currency_code', '/app/main/commonServices/currencyRates'),
                new AppMenuItem('RecurringVouchers', 'Pages.RecurringVouchers', 'recurring', '/app/main/commonServices/recurringVouchers'),
                new AppMenuItem('User Location', 'Pages.CSUserLocH', 'recurring', '/app/main/commonServices/userloc')
            ]),

            new AppMenuItem('Accounts Payable', null, 'accounts_payable', '/app/main/finance/accountPayables', [

                new AppMenuItem('APOptions', 'Pages.APOptions', 'AP_options', '/app/main/finance/accountPayables/apOptions'),
                new AppMenuItem('CRDRNote', 'Pages.CRDRNote', 'credit_notes', '/app/main/finance/accountPayables/crdrNote'),
                new AppMenuItem('DirectInvoice', 'Pages.DirectInvoice', 'direct_invoice', '/app/main/finance/accountPayables/directInvoices'),
                new AppMenuItem('Vendor Master', 'SetupForms.VendorMaster', 'vendor_master', '/app/main/finance/GeneralLedger/setupForms/vendorMaster'),
                new AppMenuItem('Post Dated Cheque', 'Pages.GlCheques', 'post_date_cheque', '/app/main/finance/GeneralLedger/transaction/glCheques'),
                new AppMenuItem('Receipt Invoices', 'Purchase.APINVH', 'debit_notes', '/app/main/supplyChain/purchase/apinvh'),
            ]),

            new AppMenuItem('Accounts Receivable', null, 'accounts_recivable', '/app/main/finance/accountReceivables', [
                new AppMenuItem('AR Terms', 'Pages.APTerms', 'AR_terms', '/app/main/finance/accountReceivables/arTerms'),
                new AppMenuItem('AROptions', 'Pages.AROptions', 'Ar_option', '/app/main/finance/accountPayables/arOptions'),
                new AppMenuItem('CRDRNote', 'Pages.CRDRNote', 'credit_notes', '/app/main/finance/accountReceivables/arCRDRNote'),
                new AppMenuItem('DirectInvoice', 'Pages.DirectInvoice', 'direct_invoice', '/app/main/finance/accountReceivables/arDirectInvoices'),
                new AppMenuItem('Direct Invoice(ST)', 'Pages.DirectInvoice', 'direct_invoice', '/app/main/finance/accountReceivables/arDirectInvoiceST'),
                new AppMenuItem('Customer Master', 'SetupForms.CustomerMaster', 'customer_master', '/app/main/finance/GeneralLedger/setupForms/customerMaster'),
                new AppMenuItem('Post Dated Cheque', 'Pages.GlCheques', 'post_date_cheque', '/app/main/finance/GeneralLedger/transaction/PostDatedCheque'),
                new AppMenuItem('Invocie KnockOff', 'Pages.OEINVKNOCKH', 'sale_entry', '/app/main/supplyChain/sales/invoice-knock-off'),
                new AppMenuItem('Route Invoices', 'Pages.ARINVH', 'sale_entry', '/app/main/finance/accountReceivables/route-invoices')
            ]),

            new AppMenuItem('GeneralLedger', null, 'general_ledger', '/app/main/finance/GeneralLedger', [
                new AppMenuItem('SetupForms', '', 'setup_forms', '/app/main/finance/GeneralLedger/setupForms', [

                    new AppMenuItem('GLOptions', 'Pages.GLOptions', 'gl_option', '/app/main/finance/GeneralLedger/setupForms/glOptions'),
                    new AppMenuItem('GL Classes', 'SetupForms.GroupCategories', 'gl_classes', '/app/main/finance/GeneralLedger/setupForms/groupCategories'),
                    new AppMenuItem('GL Group', 'SetupForms.GroupCodes', 'gl_groups', '/app/main/finance/GeneralLedger/setupForms/groupCodes'),
                    new AppMenuItem('GLLocations', 'SetupForms.GLLocations', 'location', '/app/main/finance/GeneralLedger/setupForms/glLocations'),
                    new AppMenuItem('Ledger Type', 'Pages.LedgerTypes', 'ledger_types', '/app/main/finance/GeneralLedger/setupForms/ledgerTypes'),
                    new AppMenuItem('ChartofControls', 'SetupForms.ChartofControls', 'charts_of_account', '/app/main/finance/GeneralLedger/setupForms/chartofControls', [
                        new AppMenuItem('Level 1', 'SetupForms.ControlDetails', 'level_1', '/app/main/finance/GeneralLedger/setupForms/controlDetails'),
                        new AppMenuItem('Level 2', 'SetupForms.SubControlDetails', 'level_2', '/app/main/finance/GeneralLedger/setupForms/subControlDetails'),
                        new AppMenuItem('Level 3', 'SetupForms.Segmentlevel3s', 'level_3', '/app/main/finance/GeneralLedger/setupForms/segmentlevel3s'),
                        new AppMenuItem('ChartofControls', 'SetupForms.ChartofControls', 'charts_of_account', '/app/main/finance/GeneralLedger/setupForms/chartofControls'),
                    ]),
                    new AppMenuItem('CompanySetups', 'SetupForms.CompanySetups', 'company_profile', '/app/main/finance/GeneralLedger/setupForms/companySetups'),
                    new AppMenuItem('AccountSubLedgers', 'SetupForms.AccountSubLedgers', 'sub_ledger', '/app/main/finance/GeneralLedger/setupForms/accountSubLedgers'),
                    new AppMenuItem('VoucherType', 'Pages.GLBOOKS', 'voucher_type', '/app/main/finance/GeneralLedger/setupForms/glbooks'),
                    new AppMenuItem('CashAndBankConfig', 'Pages.GLCONFIG', 'cash_and_bank_configration', '/app/main/finance/GeneralLedger/setupForms/glconfig'),

                    new AppMenuItem('GL Security', 'Pages.GLSecurityHeader', 'gl_security', '/app/main/finance/GeneralLedger/setupForms/glSecurity'),
                    new AppMenuItem('LCExpenses', 'Pages.LCExpenses', 'lc_expences', '/app/main/finance/GeneralLedger/setupForms/lcExpenses'),
                    //new AppMenuItem('Sl Group', 'Pages.GLSLGroups', 'lc_expences', '/app/main/finance/GeneralLedger/setupForms/glslgroup')
                    
                ]),
                new AppMenuItem('Transaction', '', 'transaction', '/app/main/finance/GeneralLedger/transaction', [
                    new AppMenuItem('Bank Transfers', 'Pages.BkTransfers', 'bank_transfer', '/app/main/commonServices/bkTransfers'),
                    new AppMenuItem('Bank Reconciliation', 'Transaction.BankReconcile', 'bank_Reconcilliation', '/app/main/finance/GeneralLedger/transaction/bankReconcile'),
                    new AppMenuItem('Transaction Voucher', 'Transaction.VoucherEntry', 'transaction_vouchers', '/app/main/finance/GeneralLedger/transaction/transactionVoucher'),
                    new AppMenuItem('Intergrated Voucher', 'Transaction.VoucherEntry', 'invoice', '/app/main/finance/GeneralLedger/transaction/integratedVoucher'),
                    new AppMenuItem('JournalVoucher', 'Transaction.JVEntry', 'voucher_type', '/app/main/finance/GeneralLedger/transaction/jvEntry'),
                    new AppMenuItem('Voucher Reversal', 'SetupForms.VoucherReversal', 'with_holding_terms', '/app/main/finance/GeneralLedger/setupForms/voucher-reversal'),
                    new AppMenuItem('VoucherPosting', 'Pages.AccountsPostings', 'voucher_posting', '/app/main/finance/GeneralLedger/transaction/accountsPostings', [
                        new AppMenuItem('Voucher Posting - Approved', 'Pages.AccountsPostings', 'voucher_posting_approved', '/app/main/finance/GeneralLedger/transaction/accountsPostings/approved'),
                        new AppMenuItem('Voucher Posting - UnApproved', 'Pages.AccountsPostings', 'voucher_unapproved', '/app/main/finance/GeneralLedger/transaction/accountsPostings/UnApproved'),
                        new AppMenuItem('Voucher Posting - Posted', 'Pages.AccountsPostings', 'voucher_posted', '/app/main/finance/GeneralLedger/transaction/accountsPostings/Posted')
                    ]),

                    new AppMenuItem('RecurringVouchers', 'Pages.RecurringVouchers', 'recurring_vouchers', '/app/main/finance/GeneralLedger/transaction/recurringVouchersPosting'),
                    new AppMenuItem('Batch List Preview', 'Pages.BatchListPreviews', 'batch_list', '/app/main/finance/GeneralLedger/transaction/batchListPreviews'),
                    new AppMenuItem('LCExpenses', 'Pages.LCExpensesHeader', 'lc_expences', '/app/main/finance/GeneralLedger/setupForms/lcExpensesHD'),
                    new AppMenuItem('GLTransfer', 'Pages.GLTransfer', 'transfer', '/app/main/finance/GeneralLedger/transaction/glTransfer'),
                    //    new AppMenuItem('Gl Cheques', 'Pages.GlCheques', 'flaticon-more', '/app/main/finance/GeneralLedger/transaction/glCheques'),
                    // new AppMenuItem('Bank Transfers', 'Pages.BkTransfers', 'flaticon-more', '/app/main/commonServices/bkTransfers'),
                ]),
            ]),

            new AppMenuItem('SupplyChain', 'Pages.SupplyChain', 'supply_chain', '/app/main/supplyChain', [
                new AppMenuItem('Inventory', 'SupplyChain.Inventory', 'inventory', '/app/main/supplyChain/inventory', [
                    new AppMenuItem('Setups', '', 'periodics', '', [
                        new AppMenuItem('ICSetup', 'Inventory.ICSetups', 'inventory', '/app/main/supplyChain/inventory/ic-setup'),
                        new AppMenuItem('InventoryGlLink', 'Inventory.InventoryGlLinks', 'inventory_gl_links', '/app/main/supplyChain/inventory/inventoryGlLink'),
                        new AppMenuItem('APTerms', 'Pages.APTerms', 'item_pricing', '/app/main/finance/accountPayables/apTerms'),
                        new AppMenuItem('ICLocations', 'Inventory.ICLocations', 'location', '/app/main/supplyChain/inventory/icLocations'),
                        new AppMenuItem('ICUOMs', 'Inventory.ICUOMs', 'unit_of_measure', '/app/main/supplyChain/inventory/icuoMs'),
                        new AppMenuItem('PriceList', 'Inventory.PriceLists', 'item_pricing', '/app/main/supplyChain/inventory/priceList'),
                        new AppMenuItem('ItemPricing', 'Inventory.ItemPricings', 'item_pricing', '/app/main/supplyChain/inventory/itemPricing'),
                        
                        new AppMenuItem('CostCenter', 'Inventory.CostCenters', 'cost_center', '/app/main/supplyChain/inventory/costCenter'),
                        new AppMenuItem('SubCostCenter', 'Inventory.SubCostCenters', 'sub_cost_center', '/app/main/supplyChain/inventory/subCostCenter'),
                        // new AppMenuItem('ReorderLevels', 'Inventory.ReorderLevels', 'reorder_level', '/app/main/supplyChain/inventory/reorderLevels'),
                        new AppMenuItem('ICSegment1', 'Inventory.ICSegment1', 'segment_1', '/app/main/supplyChain/inventory/setupForms/ic-segment1'),
                        new AppMenuItem('ICSegment2', 'Inventory.ICSegment2', 'segment_2', '/app/main/supplyChain/inventory/setupForms/ic-segment2'),
                        new AppMenuItem('ICSegment3', 'Inventory.ICSegment3', 'segment_3', '/app/main/supplyChain/inventory/setupForms/ic-segment3'),
                        new AppMenuItem('ICItem', '', 'item_ledger', '/app/main/supplyChain/inventory/setupForms/ic-items'),
                        new AppMenuItem('Item Status', 'Inventory.ICOPT4', 'option_4', '/app/main/supplyChain/inventory/setupForms/ic-OPT4'),
                        new AppMenuItem('ICOPT5', 'Inventory.ICOPT5', 'option_5', '/app/main/supplyChain/inventory/setupForms/ic-OPT5'),
                        // new AppMenuItem('Lot No', 'Pages.ICLOT', 'religion', '/app/main/supplyChain/inventory/iclot'),
                        // new AppMenuItem('Regions', 'Inventory.ICELocation', 'religion', '/app/main/supplyChain/inventory/regions'),
                        // new AppMenuItem('AssetRsrc', 'Pages.AssetRegistration', 'asset_registration', '/app/main/supplyChain/inventory/ReferenceInventory'),

                    ]),
                    new AppMenuItem('Transactions', '', 'transaction', '', [
                        new AppMenuItem('Openings', 'Inventory.Openings', 'opening', '/app/main/supplyChain/inventory/opening'),
                        new AppMenuItem('GatePass', 'Inventory.GatePasses', 'gate_pass', '/app/main/supplyChain/inventory/gatePass'),
                        new AppMenuItem('Transfers', 'Inventory.Transfers', 'transfer', '/app/main/supplyChain/inventory/transfers'),
                        new AppMenuItem('Adjustments', 'Inventory.Adjustments', 'adjustments', '/app/main/supplyChain/inventory/adjustments'),
                        new AppMenuItem('WorkOrder', 'Inventory.WorkOrder', 'work_order', '/app/main/supplyChain/inventory/workOrder'),
                        new AppMenuItem('Consumptions', 'Inventory.Consumptions', 'consumption', '/app/main/supplyChain/inventory/consumption'),
                        new AppMenuItem('Assembly', 'Inventory.Assemblies', 'assembly', '/app/main/supplyChain/inventory/assembly'),
                        new AppMenuItem('AssetRegistration', 'Pages.AssetRegistration', 'asset_registration', '/app/main/supplyChain/inventory/assetRegistration')
                    ])
                ]),
                new AppMenuItem('Periodics', 'SupplyChain.VoucherPosting', 'purchase', '/app/main/supplyChain/periodics', [
                    new AppMenuItem('VoucherPosting', 'SupplyChain.VoucherPosting', 'voucher_posting', '/app/main/supplyChain/periodics/voucherPosting'),
                    new AppMenuItem('Approval', 'SupplyChain.Approval', 'approval', '/app/main/supplyChain/periodics/Approval')
                ]),
                new AppMenuItem('Purchase', 'SupplyChain.Purchase', 'purchase', '/app/main/supplyChain/purchase', [
                    new AppMenuItem('Requisition', 'Purchase.Requisitions', 'Requisition', '/app/main/supplyChain/purchase/requisition'),
                    new AppMenuItem('PurchaseOrders', 'Purchase.PurchaseOrders', 'purchase_order', '/app/main/supplyChain/purchase/purchaseOrder'),
                    new AppMenuItem('Good Received Note', 'Purchase.ReceiptEntry', 'receipt_entry', '/app/main/supplyChain/purchase/receiptEntry'),
                    new AppMenuItem('ReceiptReturn', 'Purchase.ReceiptReturn', 'receipt_return', '/app/main/supplyChain/purchase/receiptReturn'),
                    // new AppMenuItem('Debit Note', 'Sales.DebitNotes', 'flaticon-more', '/app/main/supplyChain/sales/debitNote')
                ]),
                new AppMenuItem('Sales', 'SupplyChain.Sales', 'sale_account', '/app/main/supplyChain/sales', [
                    new AppMenuItem('Sale Types', 'Inventory.TransactionTypes', 'transaction_type', '/app/main/supplyChain/inventory/transactionTypes'),
                    new AppMenuItem('Drivers', 'Pages.OEDrivers', 'common_service', '/app/main/supplyChain/sales/oedrivers'),
                    new AppMenuItem('SaleAccounts', 'Sales.SaleAccounts', 'sale_account', '/app/main/supplyChain/sales/saleAccounts'),
                    new AppMenuItem('Sales Reference', 'Sales.SalesReferences', 'sales_refercence', '/app/main/supplyChain/sales/salesReference'),
                    new AppMenuItem('SaleEntry', 'Sales.SaleEntry', 'sale_entry', '/app/main/supplyChain/sales/saleEntry'),
                    new AppMenuItem('SaleReturn', 'Sales.SaleReturn', 'sale_return', '/app/main/supplyChain/sales/saleReturn'),
                    new AppMenuItem('Sale Quotation', 'Pages.OEQH', 'sale_entry', '/app/main/supplyChain/sales/sale-qutation'),
                    new AppMenuItem('Routes', 'Sales.OERoutes', 'destination', '/app/main/supplyChain/sales/oeRoutes'),
                    // new AppMenuItem('Cost Sheet', 'Pages.OECSH', 'sale_entry', '/app/main/supplyChain/sales/costSheet'),
                    //new AppMenuItem('Credit/Debit Note', 'Sales.CreditDebitNotes', 'flaticon-more', '/app/main/supplyChain/sales/creditDebitNote')
                    // new AppMenuItem('Credit Note', 'Sales.CreditNotes', 'flaticon-more', '/app/main/supplyChain/sales/creditNote'),
                    //new AppMenuItem('Debit Note', 'Sales.CreditDebitNotes', 'flaticon-more', '/app/main/supplyChain/sales/debitNote')

                ])
            ]),

            // new AppMenuItem('Manufacturing', 'Pages.Manufacturing', 'roles', '', [
            //     new AppMenuItem('Setup', 'Pages.Manufacturing.Setups', 'settings', '/app/main/manufacturing/setup', [
            //         new AppMenuItem('AccountsSetup', 'Pages.MFACSET', 'settings', '/app/main/manufacturing/setup/accountSetup'),
            //         new AppMenuItem('MFAREA', 'Pages.MFAREA', 'production', '/app/main/manufacturing/setup/productionArea'),
            //         new AppMenuItem('MFRESMAS', 'Pages.MFRESMAS', 'purchase_order', '/app/main/manufacturing/setup/resource-master'),
            //         new AppMenuItem('MFTOOLTY', 'Pages.MFTOOLTY', 'purchase_order', '/app/main/manufacturing/setup/mftoolty'),
            //         new AppMenuItem('MFTOOL', 'Pages.MFTOOL', 'purchase_order', '/app/main/manufacturing/setup/mftool'),
            //         new AppMenuItem('MFWorkingCenter', 'Pages.MFWCM', 'purchase_order', '/app/main/manufacturing/setup/mf-working-center')
            //     ]),
            //     new AppMenuItem('Transaction', 'Pages.Manufacturing.Transaction', 'transaction', '/app/main/manufacturing/transaction', [])
            // ]),

            new AppMenuItem('PayRoll', 'Pages.PayRoll', 'payroll', '/app/main/payroll', [
                new AppMenuItem('SetupForms', 'Pages.PayRoll.Setup', 'setup_forms', '/app/main/payroll', [
                    new AppMenuItem('HrmSetup', 'PayRoll.HrmSetup', 'salary', '/app/main/payroll/hrmsetup'),
                    new AppMenuItem('AllowanceSetup', 'PayRoll.AllowanceSetup.Setup', 'allowance_setup', '/app/main/payroll/allowanceSetup'),
                    new AppMenuItem('DeductionTypes', 'PayRoll.DeductionTypes.Setup', 'deduction_types', '/app/main/payroll/deductionTypes'),
                    new AppMenuItem('Departments', 'PayRoll.Departments.Setup', 'departments', '/app/main/payroll/departments'),
                    new AppMenuItem('Designation', 'PayRoll.Designations.Setup', 'designation', '/app/main/payroll/designation'),
                    new AppMenuItem('EarningTypes', 'PayRoll.EarningTypes.Setup', 'earning_types', '/app/main/payroll/earningTypes'),
                    new AppMenuItem('Education', 'PayRoll.Education.Setup', 'education', '/app/main/payroll/education'),
                    new AppMenuItem('Employees', 'PayRoll.Employees.Setup', 'employees', '/app/main/payroll/employees'),
                    new AppMenuItem('EmployeeSalary', 'PayRoll.EmployeeSalary.Setup', 'employee_salery', '/app/main/payroll/employeeSalary'),
                    new AppMenuItem('EmployeeType', 'PayRoll.EmployeeType.Setup', 'employee_types', '/app/main/payroll/employeeType'),
                    new AppMenuItem('Grade', 'PayRoll.Grades.Setup', 'grade', '/app/main/payroll/grade'),
                    new AppMenuItem('Holidays', 'PayRoll.Holidays.Setup', 'holidays', '/app/main/payroll/holidays'),
                    new AppMenuItem('Location', 'PayRoll.Locations.Setup', 'location', '/app/main/payroll/location'),
                    new AppMenuItem('Religion', 'PayRoll.Religions.Setup', 'religion', '/app/main/payroll/religion'),
                    new AppMenuItem('Sections', 'PayRoll.Sections.Setup', 'sections', '/app/main/payroll/sections'),
                    new AppMenuItem('SubDesignations', 'PayRoll.SubDesignations.Setup', 'sub_designation', '/app/main/payroll/subDesignations'),
                    new AppMenuItem('Shift', 'PayRoll.Shifts.Setup', 'shift', '/app/main/payroll/shift'),
                    new AppMenuItem('Loans Type', 'PayRoll.EmployeeLoansType.Setup', 'purchase', '/app/main/payroll/employeeLoansType'),
                    new AppMenuItem('Employee Leave Balance', 'PayRoll.EmployeeLoansType.Setup', 'employee_leave', '/app/main/payroll/employeeLeaveBalance'),
                    new AppMenuItem('Tax Slabs Setup', 'PayRoll.SlabSetup.Setup', 'tax_authorities', '/app/main/payroll/taxSlabs'),
                    new AppMenuItem('SalaryLock', 'Pages.SalaryLock', 'salary', '/app/main/payroll/salarylock'),
                    new AppMenuItem('Cader', 'PayRoll.Cader', 'salary', '/app/main/payroll/cader'),
                    new AppMenuItem('GL Link', 'Pages.Cader_link_H', 'salary', '/app/main/payroll/cader-hd')

                ]),
                new AppMenuItem('Transaction', 'Pages.PayRoll.Transactions', 'transaction', '/app/main/payroll', [
                    new AppMenuItem('Allowance', 'PayRoll.Allowances.Transactions', 'allowance', '/app/main/payroll/allowance'),
                    new AppMenuItem('Attendance', 'PayRoll.Attendance.Transactions', 'attendance', '/app/main/payroll/attendance'),
                    new AppMenuItem('BulkAttendance', 'PayRoll.AttendanceHeader.Transactions', 'bulk_attendance', '/app/main/payroll/bulkAttendance'),
                    new AppMenuItem('EmployeeArrears', 'PayRoll.EmployeeArrears.Transactions', 'employee_arrears', '/app/main/payroll/employeeArrears'),
                    new AppMenuItem('EmployeeDeductions', 'PayRoll.EmployeeDeductions.Transactions', 'employee_deduction', '/app/main/payroll/employeeDeductions'),
                    new AppMenuItem('EmployeeAdvances', 'PayRoll.EmployeeAdvances.Transactions', 'employee_types', '/app/main/payroll/employeeAdvances'),
                    new AppMenuItem('EmployeeEarnings', 'PayRoll.EmployeeEarnings.Transactions', 'employee_income', '/app/main/payroll/employeeEarnings'),
                    new AppMenuItem('EmployeeLeaves', 'PayRoll.EmployeeLeaves.Transactions', 'employee_leave', '/app/main/payroll/employeeLeaves'),
                    new AppMenuItem('SalarySheet', 'PayRoll.SalarySheet.Transactions', 'ledger_types', '/app/main/payroll/salarySheet'),
                    new AppMenuItem('Employee Loans', 'PayRoll.EmployeeLoans.Transactions', 'employe_loan', '/app/main/payroll/employeeLoans'),
                    new AppMenuItem('Salary/Loan Stop', 'PayRoll.EmployeeLoans.Transactions', 'salary', '/app/main/payroll/salaryLoanStop'),
                    new AppMenuItem('Monthly CPR', 'Pages.MonthlyCPR', 'salary', '/app/main/payroll/monthlyCpr'),
                    new AppMenuItem('Bonus', 'PayRoll.BonusH.Transactions', 'salary', '/app/main/payroll/bonus'),
                ])
            ]),

            new AppMenuItem('Reports', 'Pages.Reports', 'reports', 'app/main/reports', [
                
                  new AppMenuItem('FinanceReports', 'Reports.FinanceReports', 'financial', '/app/main/reports/FinancialReports', [
                    new AppMenuItem('Setup', 'FinanceReports.Setup', 'setup', '/app/main/reports/FinancialReports/setup'),
                    new AppMenuItem('Post Dated Cheques Reports', 'FinanceReports.Setup', 'post_date_cheque', '/app/main/reports/FinancialReports/postDatedChequesReports'),
                    new AppMenuItem('LedgerReport', 'FinanceReports.Ledger', 'ledger', '/app/main/reports/FinancialReports/ledger-rpt'),
                    new AppMenuItem('TransactionListing', 'FinanceReports.TransactionListing', 'transaction_listing', '/app/main/reports/FinancialReports/aptransactionlist'),
                    new AppMenuItem('Bank Reconcilation', 'FinanceReports.BankReconcilation', 'bank_Reconcilliation', '/app/main/reports/FinancialReports/bankReconcile-rpt'),
                    new AppMenuItem('AccountBalances', 'FinanceReports.PartyBalances', 'account_balance', '/app/main/reports/FinancialReports/PartyBalances'),
                    new AppMenuItem('TrialBalance', 'FinanceReports.TrialBalances', 'trial_balance', '/app/main/reports/FinancialReports/TrialBalance'),
                    new AppMenuItem('ChartOfACListing', 'FinanceReports.ChartOfACListing', 'Chart_of_AC_Listing', '/app/main/reports/FinancialReports/chartofacclisting'),
                    new AppMenuItem('VoucherPrinting', 'FinanceReports.VoucherPrinting', 'voucher printing', '/app/main/reports/FinancialReports/voucherpring-report'),
                    new AppMenuItem('CashBook', 'FinanceReports.CashBook', 'cash_book', '/app/main/reports/FinancialReports/cash-book-report'),
                    new AppMenuItem('BankBook', 'FinanceReports.BankBook', 'bank_book', '/app/main/reports/FinancialReports/Bank-book-report'),
                    new AppMenuItem('SubledgerTrail', 'FinanceReports.SubledgerTrail', 'sub_ledger_trial', '/app/main/reports/FinancialReports/subledger-trail'),
                    new AppMenuItem('Customer Aging', 'FinanceReports.SubledgerTrail', 'customer_aging', '/app/main/reports/FinancialReports/customer-aging-report'),
                    new AppMenuItem('TaxCollection', 'FinanceReports.TaxCollection', 'tax_collection', '/app/main/reports/FinancialReports/taxCollection'),
                    new AppMenuItem('PL Statement', 'FinanceReports.ProfitAndLossStatement', 'pl_statment', '/app/main/reports/FinancialReports/PLSTATEMENT'),
                    new AppMenuItem('Balance Sheet', 'FinanceReports.ProfitAndLossStatement', 'batch_list', '/app/main/reports/FinancialReports/BalanceSheet'),
                    // new AppMenuItem('PL Statement Category Detail', 'FinanceReports.ProfitAndLossStatement', 'flaticon-more', '/app/main/reports/FinancialReports/plStatementCategoryDetail'),
                    // new AppMenuItem('PL Statement Vocuher Detail', 'FinanceReports.ProfitAndLossStatement', 'flaticon-more', '/app/main/reports/FinancialReports/plStatementVoucherDetail'),
                    new AppMenuItem('LCExpenses', 'FinanceReports.LCExpenses', 'lc_expenses', '/app/main/reports/FinancialReports/lcExpenses')
               
               
                 ]),
                new AppMenuItem('SupplyChain', 'Reports.SupplyChainReports', 'supply_chain', '/app/main/reports/supplyChain', [
                    new AppMenuItem('InventoryReports', 'SupplyChainReports.InventoryReport', 'inventory_reports', '/app/main/reports/supplyChain/inventory', [
                        new AppMenuItem('ItemLedgerReport', 'InventoryReport.ItemLedger', 'item_ledger', '/app/main/reports/supplyChain/inventory/itemLedger'),
                        new AppMenuItem('Consumption Reports', 'InventoryReport.Consumption', 'consumption_reports', '/app/main/reports/supplyChain/inventory/consumptionReports'),
                        new AppMenuItem('DocumentPrintingReport', 'InventoryReport.DocumentPrinting', 'document_printing', '/app/main/reports/supplyChain/inventory/documentPrinting'),
                        new AppMenuItem('ActivityReports', 'InventoryReport.Activity', 'reports', '/app/main/reports/supplyChain/inventory/activityReports'),
                        
                        new AppMenuItem('Asset Listing', 'InventoryReport.AssetRegReports', 'reports', '/app/main/reports/supplychain/inventory/asste-register-reports'),
                    ]),
                    new AppMenuItem('SaleReports', 'SupplyChainReports.SaleReports', 'reports', '/app/main/reports/supplyChain/sale', [
                        new AppMenuItem('DocumentPrintingReport', 'SaleReports.DocumentPrinting', 'document_printing', '/app/main/reports/supplyChain/sale/documentPrinting')
                    ])
                ]),
                new AppMenuItem('Pay Roll', 'Reports.PayRollReports', 'payroll', '/app/main/reports/payRoll', [
                    new AppMenuItem('Attendance', 'PayRollReports.AttendanceReports', 'attendance', '/app/main/reports/payRoll/attendanceReports'),
                    new AppMenuItem('Employee', 'PayRollReports.EmployeeReports', 'employee', '/app/main/reports/payRoll/employeeReports'),
                    new AppMenuItem('Salary', 'PayRollReports.SalaryReports', 'salary', '/app/main/reports/payRoll/salaryReports'),
                    new AppMenuItem('Setup', 'PayRollReports.SetupReports', 'setup_forms', '/app/main/reports/payRoll/setupReports'),
                    new AppMenuItem('Allowance', 'PayRollReports.AllowanceReports', 'allowance', '/app/main/reports/payroll/allowances-reports'),
                ]),

                new AppMenuItem('Alert Log', 'FinanceReports.CashBook', 'alert_log', '/app/main/reports/alert-log')

            ]),



            new AppMenuItem('Administration', '', 'administration', '', [
                // new AppMenuItem('OrganizationUnits', 'Pages.Administration.OrganizationUnits', 'organization_unit', '/app/admin/organization-units'),
                new AppMenuItem('Roles', 'Pages.Administration.Roles', 'roles', '/app/admin/roles'),
                new AppMenuItem('Users', 'Pages.Administration.Users', 'users', '/app/admin/users'),
                // new AppMenuItem('Languages', 'Pages.Administration.Languages', 'language', '/app/admin/languages'),
                // new AppMenuItem('AuditLogs', 'Pages.Administration.AuditLogs', 'audit_logs', '/app/admin/auditLogs'),
                new AppMenuItem('Maintenance', 'Pages.Administration.Host.Maintenance', 'audit_logs', '/app/admin/maintenance'),
                // new AppMenuItem('Subscription', 'Pages.Administration.Tenant.SubscriptionManagement', 'subscription', '/app/admin/subscription-management'),
                // new AppMenuItem('VisualSettings', 'Pages.Administration.UiCustomization', 'visual_setting', '/app/admin/ui-customization'),
                new AppMenuItem('Settings', 'Pages.Administration.Host.Settings', 'settings', '/app/admin/hostSettings'),
                new AppMenuItem('Settings', 'Pages.Administration.Tenant.Settings', 'settings', '/app/admin/tenantSettings'),
                new AppMenuItem('CurrencyRate', 'Pages.CurrencyRate', 'currency_code', '/app/main/cscurrate/cscurratEses'),
            ]),
            // new AppMenuItem('DemoUiComponents', 'Pages.DemoUiComponents', 'transaction_listing', '/app/admin/demo-ui-components'),
        ]);

    }

    checkChildMenuItemPermission(menuItem): boolean {
        for (let i = 0; i < menuItem.items.length; i++) {
            let subMenuItem = menuItem.items[i];

            if (subMenuItem.permissionName && this._permissionCheckerService.isGranted(subMenuItem.permissionName)) {
                return true;
            } else if (subMenuItem.items && subMenuItem.items.length) {
                return this.checkChildMenuItemPermission(subMenuItem);
            }
        }

        return false;
    }

    showMenuItem(menuItem: AppMenuItem): boolean {
        
        if (menuItem.permissionName === 'Pages.Administration.Tenant.SubscriptionManagement' && this._appSessionService.tenant && !this._appSessionService.tenant.edition) {
            return false;
        }

        let hideMenuItem = false;

        if (menuItem.requiresAuthentication && !this._appSessionService.user) {
            hideMenuItem = true;
        }

        if (menuItem.permissionName && !this._permissionCheckerService.isGranted(menuItem.permissionName)) {
            hideMenuItem = true;
        }

        if (this._appSessionService.tenant || !abp.multiTenancy.ignoreFeatureCheckForHostUsers) {
            if (menuItem.hasFeatureDependency() && !menuItem.featureDependencySatisfied()) {
                hideMenuItem = true;
            }
        }

        if (!hideMenuItem && menuItem.items && menuItem.items.length) {
            return this.checkChildMenuItemPermission(menuItem);
        }

        return !hideMenuItem;
    }
}
