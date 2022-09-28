import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GLOptionsComponent } from './GeneralLedger/setupForms/glOptions/glOptions.component';
import { AROptionsComponent } from './accountReceivables/arOptions/arOptions.component';
import { APOptionsComponent } from './accountPayables/apOptions/apOptions.component';
import { APTransactionListComponent } from './accountPayables/aptransactionlist/aptransactionlist.component';
import { GLCONFIGComponent } from './GeneralLedger/setupForms/glconfig/glconfig.component';
import { AccountsPostingsComponent } from './GeneralLedger/transaction/accountsPostings/accountsPostings.component';
import { GLBOOKSComponent } from './GeneralLedger/setupForms/glbooks/glbooks.component';
import { AccountSubLedgersComponent } from './GeneralLedger/setupForms/accountSubLedgers/accountSubLedgers.component';
import { GLLocationsComponent } from './GeneralLedger/setupForms/glLocations/glLocations.component';
import { Segmentlevel3sComponent } from './GeneralLedger/setupForms/segmentlevel3s/segmentlevel3s.component';
import { SubControlDetailsComponent } from './GeneralLedger/setupForms/subControlDetails/subControlDetails.component';
import { GroupCodesComponent } from './GeneralLedger/setupForms/groupCodes/groupCodes.component';
import { ControlDetailsComponent } from './GeneralLedger/setupForms/controlDetails/controlDetails.component';
import { GroupCategoriesComponent } from './GeneralLedger/setupForms/groupCategories/groupCategories.component';
import { ChartofControlsComponent } from './GeneralLedger/setupForms/chartofControls/chartofControls.component';
import { VendorMasterComponent } from './GeneralLedger/setupForms/vendorMaster/vendorMaster.component';
import { CustomerMasterComponent } from './GeneralLedger/setupForms/customerMaster/customerMaster.component';
import { VoucherEntryComponent } from './GeneralLedger/transaction/voucherEntry/voucherEntry.component';
import { JVEntryComponent } from './GeneralLedger/transaction/jvEntry/jvEntry.component';
import { APTermsComponent } from './accountPayables/apTerms/apTerms.component';
import { BatchListPreviewsComponent } from './GeneralLedger/transaction/batchListPreviews/batchListPreviews.component';
import { FinanceComponent} from './finance.component'
import { DirectinvoiceComponent } from './accountPayables/directInvoices/directInvoice.component';
import { ARDirectinvoiceComponent } from './accountReceivables/arDirectInvoice/arDirectInvoice.component';
import { BankReconcileComponent } from './GeneralLedger/transaction/bankReconcile/bankReconcile.component';
import { LedgerTypesComponent } from './GeneralLedger/setupForms/ledgerTypes/ledgerTypes.component';
import { GlChequescomponent } from './GeneralLedger/transaction/GlCheques/glCheques.component';
import { GLAccountsPermissionsComponent } from './GeneralLedger/setupForms/accountsPermission/glAccountsPermissions.component';
import { GLSecurityComponent } from './GeneralLedger/setupForms/glSecurity/glSecurity.component';
import { ApprovedComponent } from './GeneralLedger/transaction/accountsPostings/approved.component';
import { UnapprovedComponent } from './GeneralLedger/transaction/accountsPostings/Unapproved.component';
import { PostedComponent } from './GeneralLedger/transaction/accountsPostings/Posted.component';
import { PLCategoriesComponent } from './GeneralLedger/setupForms/plCategories/plCategories.component';
import { RecurringVouchersComponent } from '../commonServices/recurringVouchers/recurringVouchers.component';
import { RecurringVouchersPostingComponent } from './GeneralLedger/transaction/recurringVouchers/recurringVouchersPosting.component';
import { ARTermsComponent } from './accountReceivables/arTerms/arTerms.component';
import { ARDirectinvoiceSTComponent } from './accountReceivables/arDirectInvoiceST/arDirectInvoiceST.component';
import { LCExpenseComponent } from './GeneralLedger/setupForms/lcExpenses/lcExpenses.component';
import { LCExpensesHDComponent } from './GeneralLedger/setupForms/lcExpensesHD/lcExpensesHD.component';
import { CRDRNoteComponent } from './accountPayables/crdrNote/crdrNote.component';
import { ARCRDRComponent } from './accountReceivables/arCRDRNote/arCRDRNote.component';
import { GLTransferComponent } from './GeneralLedger/transaction/glTransfer/glTransfer.component';
import { GlslgroupComponent } from './GeneralLedger/setupForms/glslgroup/glslgroup.component';
import { VoucherReversalComponent } from './GeneralLedger/setupForms/voucher-reversal/voucher-reversal.component';
import { RouteInvoicesComponent } from './accountReceivables/route-invoices/route-invoices.component';
const routes: Routes = [
  {
    path: '',
    component:FinanceComponent,
    children: [ 
      { path: 'GeneralLedger/setupForms/plCategories', component: PLCategoriesComponent, data: { permission: 'Pages.PLCategories' }  },
      { path: 'GeneralLedger/setupForms/glSecurity', component: GLSecurityComponent, data: { permission: 'Pages.GLSecurityHeader' }  },
      { path: 'accountPayables/directInvoices', component: DirectinvoiceComponent, data: { permission: 'Pages.DirectInvoice' } },
      { path: 'accountReceivables/arDirectInvoices', component: ARDirectinvoiceComponent, data: { permission: 'Pages.DirectInvoice' } },
      { path: 'accountReceivables/arDirectInvoiceST', component: ARDirectinvoiceSTComponent, data: { permission: 'Pages.DirectInvoice' } },
      { path: 'GeneralLedger/setupForms/glOptions', component: GLOptionsComponent, data: { permission: 'Pages.GLOptions' } },
      { path: 'accountPayables/arOptions', component: AROptionsComponent, data: { permission: 'Pages.AROptions' } },
      { path: 'accountPayables/apOptions', component: APOptionsComponent, data: { permission: 'Pages.APOptions' } },
      { path: 'accountPayables/aptransactionlist', component: APTransactionListComponent, data: { permission: 'Pages.APTransactionList' } },
      { path: 'GeneralLedger/transaction/accountsPostings', component: AccountsPostingsComponent, data: { permission: 'Pages.AccountsPostings' } },
      { path: 'GeneralLedger/transaction/accountsPostings/approved', component: ApprovedComponent, data: { permission: 'Pages.AccountsPostings' } },
      { path: 'GeneralLedger/transaction/accountsPostings/UnApproved', component: UnapprovedComponent, data: { permission: 'Pages.AccountsPostings' } },
      { path: 'GeneralLedger/transaction/accountsPostings/Posted', component: PostedComponent, data: { permission: 'Pages.AccountsPostings' } },
      { path: 'GeneralLedger/setupForms/glconfig', component: GLCONFIGComponent, data: { permission: 'Pages.GLCONFIG' } },
      { path: 'GeneralLedger/setupForms/glbooks', component: GLBOOKSComponent, data: { permission: 'Pages.GLBOOKS' } },
      { path: 'GeneralLedger/setupForms/ledgerTypes', component: LedgerTypesComponent, data: { permission: 'Pages.LedgerTypes' } },
      { path: 'GeneralLedger/setupForms/voucher-reversal', component: VoucherReversalComponent, data: { permission: 'SetupForms.VoucherReversal' } },
      
      { path: 'GeneralLedger/setupForms/accountSubLedgers', component: AccountSubLedgersComponent, data: { permission: 'SetupForms.AccountSubLedgers' } },
      { path: 'GeneralLedger/setupForms/glLocations', component: GLLocationsComponent, data: { permission: 'SetupForms.GLLocations' } },
      { path: 'GeneralLedger/setupForms/chartofControls', component: ChartofControlsComponent, data: { permission: 'SetupForms.ChartofControls' } },
      { path: 'GeneralLedger/setupForms/segmentlevel3s', component: Segmentlevel3sComponent, data: { permission: 'SetupForms.Segmentlevel3s' } },
      { path: 'GeneralLedger/setupForms/subControlDetails', component: SubControlDetailsComponent, data: { permission: 'SetupForms.SubControlDetails' } },
      { path: 'GeneralLedger/setupForms/controlDetails', component: ControlDetailsComponent, data: { permission: 'SetupForms.ControlDetails' } },
      { path: 'GeneralLedger/setupForms/lcExpenses', component: LCExpenseComponent, data: { permission: 'Pages.LCExpenses' } },
      { path: 'GeneralLedger/setupForms/lcExpensesHD', component: LCExpensesHDComponent, data: { permission: 'Pages.LCExpensesHeader' } },
      { path: 'GeneralLedger/setupForms/groupCodes', component: GroupCodesComponent, data: { permission: 'SetupForms.GroupCodes' } },
      { path: 'GeneralLedger/setupForms/groupCategories', component: GroupCategoriesComponent, data: { permission: 'SetupForms.GroupCategories' } },
      { path: 'GeneralLedger/transaction/batchListPreviews', component: BatchListPreviewsComponent, data: { permission: 'Pages.BatchListPreviews' } },
      { path: 'GeneralLedger/setupForms/vendorMaster', component: VendorMasterComponent, data: { permission: 'SetupForms.VendorMaster' } },
      { path: 'GeneralLedger/setupForms/customerMaster', component: CustomerMasterComponent, data: { permission: 'SetupForms.CustomerMaster' } },
      { path: 'GeneralLedger/setupForms/glslgroup', component: GlslgroupComponent, data: { permission: 'Pages.GLSLGroups' } },
      { path: 'GeneralLedger/transaction/transactionVoucher', component: VoucherEntryComponent, data: { permission: 'Transaction.VoucherEntry', viewOption: 'transactionVoucher' } },
      { path: 'GeneralLedger/transaction/integratedVoucher', component: VoucherEntryComponent, data: { permission: 'Transaction.VoucherEntry' , viewOption: 'integratedVoucher' }  },
      { path: 'GeneralLedger/transaction/jvEntry', component: JVEntryComponent, data: { permission: 'Transaction.JVEntry' } },
      { path: 'GeneralLedger/transaction/bankReconcile', component: BankReconcileComponent, data: { permission: 'Transaction.BankReconcile' } },
      { path: 'GeneralLedger/transaction/glCheques', component: GlChequescomponent, data: { permission: 'Pages.GlCheques' } },
      { path: 'GeneralLedger/transaction/glTransfer', component: GLTransferComponent, data: { permission: 'Pages.GLTransfer' } },
      { path: 'GeneralLedger/transaction/PostDatedCheque', component: GlChequescomponent, data: { permission: 'Pages.GlCheques', viewOption: 'PostDatedCheque' } },
      { path: 'accountPayables/apTerms', component: APTermsComponent, data: { permission: 'Pages.APTerms' }  }, 
      { path: 'accountPayables/crdrNote', component: CRDRNoteComponent, data: { permission: 'Pages.CRDRNote' }  },
      { path: 'accountReceivables/arCRDRNote', component: ARCRDRComponent, data: { permission: 'Pages.CRDRNote' }  },
      { path: 'GeneralLedger/transaction/recurringVouchersPosting', component: RecurringVouchersPostingComponent, data: { permission: 'Pages.RecurringVouchers', viewOption: 'RecurringVouchers' } },
      { path: 'accountReceivables/arTerms', component: ARTermsComponent, data: { permission: 'Pages.APTerms' }  },
      { path: 'accountReceivables/route-invoices', component: RouteInvoicesComponent, data: {permission: 'Pages.ARINVH'}}
    ]

  }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FinanceRoutingModule { }
