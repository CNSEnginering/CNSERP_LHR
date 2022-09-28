import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FinanceRoutingModule } from './finance-routing.module';
import { AccountSubledgerLookupComponent } from './GeneralLedger/setupForms/accountSubLedgers/account-subledger-lookup.component';
import { CustomerMasterComponent } from './GeneralLedger/setupForms/customerMaster/customerMaster.component';
import { VendorMasterComponent } from './GeneralLedger/setupForms/vendorMaster/vendorMaster.component';
import { CreateOrEditGroupCategoryModalComponent } from './GeneralLedger/setupForms/groupCategories/create-or-edit-groupCategory-modal.component';
import { ViewGroupCategoryModalComponent } from './GeneralLedger/setupForms/groupCategories/view-groupCategory-modal.component';
import { GroupCategoriesComponent } from './GeneralLedger/setupForms/groupCategories/groupCategories.component';
import { CreateOrEditGroupCodeModalComponent } from './GeneralLedger/setupForms/groupCodes/create-or-edit-groupCode-modal.component';
import { ViewGroupCodeModalComponent } from './GeneralLedger/setupForms/groupCodes/view-groupCode-modal.component';
import { GroupCodesComponent } from './GeneralLedger/setupForms/groupCodes/groupCodes.component';
import { ViewControlDetailModalComponent } from './GeneralLedger/setupForms/controlDetails/view-controlDetail-modal.component';
import { CreateOrEditControlDetailModalComponent } from './GeneralLedger/setupForms/controlDetails/create-or-edit-controlDetail-modal.component';
import { ControlDetailsComponent } from './GeneralLedger/setupForms/controlDetails/controlDetails.component';
import { SubControlDetailControlDetailLookupTableModalComponent } from './GeneralLedger/setupForms/subControlDetails/subControlDetail-controlDetail-lookup-table-modal.component';
import { CreateOrEditSubControlDetailModalComponent } from './GeneralLedger/setupForms/subControlDetails/create-or-edit-subControlDetail-modal.component';
import { ViewSubControlDetailModalComponent } from './GeneralLedger/setupForms/subControlDetails/view-subControlDetail-modal.component';
import { SubControlDetailsComponent } from './GeneralLedger/setupForms/subControlDetails/subControlDetails.component';
import { Segmentlevel3SubControlDetailLookupTableModalComponent } from './GeneralLedger/setupForms/segmentlevel3s/segmentlevel3-subControlDetail-lookup-table-modal.component';
import { Segmentlevel3ControlDetailLookupTableModalComponent } from './GeneralLedger/setupForms/segmentlevel3s/segmentlevel3-controlDetail-lookup-table-modal.component';
import { ViewSegmentlevel3ModalComponent } from './GeneralLedger/setupForms/segmentlevel3s/view-segmentlevel3-modal.component';
import { CreateOrEditSegmentlevel3ModalComponent } from './GeneralLedger/setupForms/segmentlevel3s/create-or-edit-segmentlevel3-modal.component';
import { Segmentlevel3sComponent } from './GeneralLedger/setupForms/segmentlevel3s/segmentlevel3s.component';
import { ChartofControlSegmentlevel3LookupTableModalComponent } from './GeneralLedger/setupForms/chartofControls/chartofControl-segmentlevel3-lookup-table-modal.component';
import { ChartofControlSubControlDetailLookupTableModalComponent } from './GeneralLedger/setupForms/chartofControls/chartofControl-subControlDetail-lookup-table-modal.component';
import { ChartofControlControlDetailLookupTableModalComponent } from './GeneralLedger/setupForms/chartofControls/chartofControl-controlDetail-lookup-table-modal.component';
import { ViewChartofControlModalComponent } from './GeneralLedger/setupForms/chartofControls/view-chartofControl-modal.component';
import { ChartofControlsComponent } from './GeneralLedger/setupForms/chartofControls/chartofControls.component';
import { CreateOrEditChartofControlModalComponent } from './GeneralLedger/setupForms/chartofControls/create-or-edit-chartofControl-modal.component';
import { AccountSubLedgerTaxAuthorityLookupTableModalComponent } from './GeneralLedger/setupForms/accountSubLedgers/accountSubLedger-taxAuthority-lookup-table-modal.component';
import { AccountSubLedgerChartofControlLookupTableModalComponent } from './GeneralLedger/setupForms/accountSubLedgers/accountSubLedger-chartofControl-lookup-table-modal.component';
import { ViewAccountSubLedgerModalComponent } from './GeneralLedger/setupForms/accountSubLedgers/view-accountSubLedger-modal.component';
import { CreateOrEditAccountSubLedgerModalComponent } from './GeneralLedger/setupForms/accountSubLedgers/create-or-edit-accountSubLedger-modal.component';
import { AccountSubLedgersComponent } from './GeneralLedger/setupForms/accountSubLedgers/accountSubLedgers.component';
import { ViewAPTermModalComponent } from './accountPayables/apTerms/view-apTerm-modal.component';
import { APTransactionListComponent } from './accountPayables/aptransactionlist/aptransactionlist.component';
import { ViewGLLocationModalComponent } from './GeneralLedger/setupForms/glLocations/view-glLocation-modal.component';
import { CreateOrEditGLLocationModalComponent } from './GeneralLedger/setupForms/glLocations/create-or-edit-glLocation-modal.component';
import { GLLocationsComponent } from './GeneralLedger/setupForms/glLocations/glLocations.component';
import { GLOptionsComponent } from './GeneralLedger/setupForms/glOptions/glOptions.component';
import { ViewGLOptionModalComponent } from './GeneralLedger/setupForms/glOptions/view-glOption-modal.component';
import { CreateOrEditGLOptionModalComponent } from './GeneralLedger/setupForms/glOptions/create-or-edit-glOption-modal.component';
import { GLOptionChartofControlLookupTableModalComponent } from './GeneralLedger/setupForms/glOptions/glOption-chartofControl-lookup-table-modal.component';
import { AROptionsComponent } from './accountReceivables/arOptions/arOptions.component';
import { CreateOrEditAROptionModalComponent } from './accountReceivables/arOptions/create-or-edit-arOption-modal.component';
import { ViewAROptionModalComponent } from './accountReceivables/arOptions/view-arOption-modal.component';
import { APOptionsComponent } from './accountPayables/apOptions/apOptions.component';
import { ViewAPOptionModalComponent } from './accountPayables/apOptions/view-apOption-modal.component';
import { CreateOrEditAPOptionModalComponent } from './accountPayables/apOptions/create-or-edit-apOption-modal.component';
import { VoucherEntryComponent } from './GeneralLedger/transaction/voucherEntry/voucherEntry.component';
import { ViewVoucherEntryModalComponent } from './GeneralLedger/transaction/voucherEntry/view-voucherEntry-modal.component';
import { CreateOrEditVoucherEntryModalComponent } from './GeneralLedger/transaction/voucherEntry/create-or-edit-voucherEntry-modal.component';
import { JVEntryComponent } from './GeneralLedger/transaction/jvEntry/jvEntry.component';
import { ViewJVEntryModalComponent } from './GeneralLedger/transaction/jvEntry/view-jvEntry-modal.component';
import { CreateOrEditJVEntryModalComponent } from './GeneralLedger/transaction/jvEntry/create-or-edit-jvEntry-modal.component';
import { AccountsPostingsComponent } from './GeneralLedger/transaction/accountsPostings/accountsPostings.component';
import { ViewAccountsPostingModalComponent } from './GeneralLedger/transaction/accountsPostings/view-accountsPosting-modal.component';
import { CreateOrEditAccountsPostingModalComponent } from './GeneralLedger/transaction/accountsPostings/create-or-edit-accountsPosting-modal.component';
import { GLCONFIGComponent } from './GeneralLedger/setupForms/glconfig/glconfig.component';
import { ViewGLCONFIGModalComponent } from './GeneralLedger/setupForms/glconfig/view-glconfig-modal.component';
import { CreateOrEditGLCONFIGModalComponent } from './GeneralLedger/setupForms/glconfig/create-or-edit-glconfig-modal.component';
import { GLCONFIGGLBOOKSLookupTableModalComponent } from './GeneralLedger/setupForms/glconfig/glconfig-glbooks-lookup-table-modal.component';
import { GLCONFIGChartofControlLookupTableModalComponent } from './GeneralLedger/setupForms/glconfig/glconfig-chartofControl-lookup-table-modal.component';
import { GLCONFIGAccountSubLedgerLookupTableModalComponent } from './GeneralLedger/setupForms/glconfig/glconfig-accountSubLedger-lookup-table-modal.component';
import { GLBOOKSComponent } from './GeneralLedger/setupForms/glbooks/glbooks.component';
import { ViewGLBOOKSModalComponent } from './GeneralLedger/setupForms/glbooks/view-glbooks-modal.component';
import { CreateOrEditGLBOOKSModalComponent } from './GeneralLedger/setupForms/glbooks/create-or-edit-glbooks-modal.component';
// import { TaxAuthoritiesComponent } from '../commonServices/taxAuthorities/taxAuthorities.component';
// import { ViewTaxAuthorityModalComponent } from '../commonServices/taxAuthorities/view-taxAuthority-modal.component';
// import { CreateOrEditTaxAuthorityModalComponent } from '../commonServices/taxAuthorities/create-or-edit-taxAuthority-modal.component';
import { APTermsComponent } from './accountPayables/apTerms/apTerms.component';
import { CreateOrEditAPTermModalComponent } from './accountPayables/apTerms/create-or-edit-apTerm-modal.component';
import { FileUploadModule } from 'primeng/primeng';
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
import { BatchListPreviewsComponent } from './GeneralLedger/transaction/batchListPreviews/batchListPreviews.component';
import { ViewBatchListPreviewModalComponent } from './GeneralLedger/transaction/batchListPreviews/view-batchListPreview-modal.component';
import { FinanceComponent } from './finance.component';
import { DirectinvoiceComponent } from './accountPayables/directInvoices/directInvoice.component';
import { ViewDirectInvoiceModalComponent } from './accountPayables/directInvoices/view-directInvoice-modal.component';
import { CreateOrEditDirectInvoiceModalComponent } from './accountPayables/directInvoices/create-or-edit-directInvoice-modal.component';
import { ARDirectinvoiceComponent } from './accountReceivables/arDirectInvoice/arDirectInvoice.component';
import { FindersModule } from '@app/finders/finders.module';
import { BankReconcileComponent } from './GeneralLedger/transaction/bankReconcile/bankReconcile.component';
import { CreateOrEditBankReconcileModalComponent } from './GeneralLedger/transaction/bankReconcile/create-or-edit-bankReconcile-modal.component';
import { ViewBankReconcileModalComponent } from './GeneralLedger/transaction/bankReconcile/view-bankReconcile-modal.component';
import { CheckboxCellComponent } from '@app/shared/common/checkbox-cell/checkbox-cell.component';
import { AgDatePickerComponent } from '@app/shared/common/ag-date-picker/ag-date-picker.component';
import { DateEditorComponent } from './GeneralLedger/transaction/bankReconcile/date-editor.component';
import { DateRendererComponent } from './GeneralLedger/transaction/bankReconcile/date-renderer.component';
import { LedgerTypesServiceProxy } from './shared/services/GLLedgerType.service';
import { LedgerTypesComponent } from './GeneralLedger/setupForms/ledgerTypes/ledgerTypes.component';
import { CreateOrEditLedgerTypeModalComponent } from './GeneralLedger/setupForms/ledgerTypes/create-or-edit-ledgerType-modal.component';
import { ViewLedgerTypeModalComponent } from './GeneralLedger/setupForms/ledgerTypes/view-ledgerType-modal.component';
import { GlChequescomponent } from './GeneralLedger/transaction/GlCheques/glCheques.component';
import { CreateOrEditGlChequesModalComponent } from './GeneralLedger/transaction/GlCheques/create-or-edit-glCheques-modal.component';
import { ViewGlChequesModalComponent } from './GeneralLedger/transaction/GlCheques/view-glCheques-modal.component';
import { GLAccountsPermissionsComponent } from './GeneralLedger/setupForms/accountsPermission/glAccountsPermissions.component';
import { ViewGLAccountsPermissionModalComponent } from './GeneralLedger/setupForms/accountsPermission/view-glAccountsPermission-modal.component';
import { CreateOrEditGLAccountsPermissionModalComponent } from './GeneralLedger/setupForms/accountsPermission/create-or-edit-glAccountsPermission-modal.component';
import { AbpHttpInterceptor } from 'abp-ng2-module/dist/src/abpHttpInterceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { GLAccountsPermissionsServiceProxy } from './shared/services/accountsPermission.service';
//import { ReportViewerModule } from 'ngx-ssrs-reportviewer';
import { CurrencyMaskModule } from "ng2-currency-mask";
import { GLSecurityComponent } from './GeneralLedger/setupForms/glSecurity/glSecurity.component';
import { ViewGLSecurityModalComponent } from './GeneralLedger/setupForms/glSecurity/view-glSecurity-modal.component';
import { CreateOrEditGLSecurityModalComponent } from './GeneralLedger/setupForms/glSecurity/create-or-edit-glSecurity-modal.component';
import { CheckBoxCellComponent } from './GeneralLedger/setupForms/glSecurity/checkBoxCell/checkbox-cell.component';
import { LightboxModule } from 'ngx-lightbox';
import { ApprovedComponent } from './GeneralLedger/transaction/accountsPostings/approved.component';
import { UnapprovedComponent } from './GeneralLedger/transaction/accountsPostings/Unapproved.component';
import { PostedComponent } from './GeneralLedger/transaction/accountsPostings/Posted.component';
import { PLCategoriesComponent } from './GeneralLedger/setupForms/plCategories/plCategories.component';
import { ViewPLCategoryModalComponent } from './GeneralLedger/setupForms/plCategories/view-plCategory-modal.component';
import { CreateOrEditPLCategoryModalComponent } from './GeneralLedger/setupForms/plCategories/create-or-edit-plCategory-modal.component';
import { RecurringVouchersComponent } from '../commonServices/recurringVouchers/recurringVouchers.component';
import { RecurringVouchersPostingComponent } from './GeneralLedger/transaction/recurringVouchers/recurringVouchersPosting.component';
import { PLCategoriesServiceProxy } from './shared/services/plCategories.service';
import { ARTermsComponent } from './accountReceivables/arTerms/arTerms.component';
import { ViewARTermModalComponent } from './accountReceivables/arTerms/view-arTerm-modal.component';
import { CreateOrEditARTermModalComponent } from './accountReceivables/arTerms/create-or-edit-arTerm-modal.component';
import { ARTermsServiceProxy } from './shared/services/arTerms.service';
import { TransferAccountSubLedgerModalComponent } from './GeneralLedger/setupForms/accountSubLedgers/transfer-accountSubLedger-modal.component';
import { ARDirectinvoiceSTComponent } from './accountReceivables/arDirectInvoiceST/arDirectInvoiceST.component';
import { LCExpenseComponent } from './GeneralLedger/setupForms/lcExpenses/lcExpenses.component';
import { CreateOrEditLCExpenseModalComponent } from './GeneralLedger/setupForms/lcExpenses/create-or-edit-lcExpense-modal.component';
import { ViewLCExpenseModalComponent } from './GeneralLedger/setupForms/lcExpenses/view-lcExpense-modal.component';
import { LCExpensesHDComponent } from './GeneralLedger/setupForms/lcExpensesHD/lcExpensesHD.component';
import { CreateOrEditLCExpensesHDModalComponent } from './GeneralLedger/setupForms/lcExpensesHD/create-or-edit-lcExpensesHD-modal.component';
import { ViewLCExpensesHDModalComponent } from './GeneralLedger/setupForms/lcExpensesHD/view-lcExpensesHD-modal.component';
import { CRDRNoteComponent } from './accountPayables/crdrNote/crdrNote.component';
import { CreateOrEditCRDRNoteModalComponent } from './accountPayables/crdrNote/create-or-edit-CRDRNote-modal.component';
import { ViewCRDRNoteModalComponent } from './accountPayables/crdrNote/view-CRDRNote-modal.component';
import { ARCRDRComponent } from './accountReceivables/arCRDRNote/arCRDRNote.component';
import { GLTransferComponent } from './GeneralLedger/transaction/glTransfer/glTransfer.component';
import { ViewGLTransferModalComponent } from './GeneralLedger/transaction/glTransfer/view-glTransfer-modal.component';
import { CreateOrEditGLTransferModalComponent } from './GeneralLedger/transaction/glTransfer/create-or-edit-glTransfer-modal.component';
import { GlslgroupComponent } from './GeneralLedger/setupForms/glslgroup/glslgroup.component';
import { CreateOrEditglslgroupComponent } from './GeneralLedger/setupForms/glslgroup/create-or-editglslgroup.component';
import { VoucherReversalComponent } from './GeneralLedger/setupForms/voucher-reversal/voucher-reversal.component';
import { CreateOrEditvoucherReversalComponent } from './GeneralLedger/setupForms/voucher-reversal/create-or-edit-voucher-reversal.component';
import { InventoryModule } from '../supplyChain/inventory/inventory.module';
import { RouteInvoicesComponent } from './accountReceivables/route-invoices/route-invoices.component';
import { CreateOrEditRouteInvoicesComponent } from './accountReceivables/route-invoices/create-or-edit-route-invoices.component';
@NgModule({
	imports: [
		FormsModule,
		FileUploadModule,
		AutoCompleteModule,
		PaginatorModule,
		EditorModule,
		InputMaskModule,
		TableModule,
		CommonModule,
		ModalModule,
		TabsModule,
		TooltipModule,
		AppCommonModule,
		UtilsModule,
		FinanceRoutingModule,
		CountoModule,
		//ReportViewerModule,
		NgxChartsModule,
		BsDatepickerModule.forRoot(),
		BsDropdownModule.forRoot(),
		PopoverModule.forRoot(),
		AgGridModule.withComponents([
			CheckboxCellComponent,AgDatePickerComponent,
			DateEditorComponent,DateRendererComponent,CheckBoxCellComponent]),
		FindersModule,
		CurrencyMaskModule,
		LightboxModule,InventoryModule
	],
	exports:[
		CreateOrEditJVEntryModalComponent
	],
	declarations: [
		PLCategoriesComponent,
		ViewPLCategoryModalComponent,		CreateOrEditPLCategoryModalComponent,
		GLSecurityComponent,
		ViewGLSecurityModalComponent,
		CreateOrEditGLSecurityModalComponent,
		DirectinvoiceComponent,
		ViewDirectInvoiceModalComponent,CreateOrEditDirectInvoiceModalComponent,
		APTransactionListComponent,
		GLLocationsComponent,
		ViewGLLocationModalComponent, CreateOrEditGLLocationModalComponent,
		GLOptionsComponent,
		ViewGLOptionModalComponent, CreateOrEditGLOptionModalComponent,
		GLOptionChartofControlLookupTableModalComponent,
		AROptionsComponent,
		ViewAROptionModalComponent, CreateOrEditAROptionModalComponent,
		APOptionsComponent,
		ViewAPOptionModalComponent, CreateOrEditAPOptionModalComponent,
		VoucherEntryComponent,
		ViewVoucherEntryModalComponent, CreateOrEditVoucherEntryModalComponent,
		JVEntryComponent,
		ViewJVEntryModalComponent, CreateOrEditJVEntryModalComponent,
		AccountsPostingsComponent,
		ViewAccountsPostingModalComponent, CreateOrEditAccountsPostingModalComponent,
		GLCONFIGComponent,
		ViewGLCONFIGModalComponent,
		CreateOrEditGLCONFIGModalComponent,
		GLCONFIGGLBOOKSLookupTableModalComponent,
		GLCONFIGChartofControlLookupTableModalComponent,
		GLCONFIGAccountSubLedgerLookupTableModalComponent,
		GLBOOKSComponent,
		ViewGLBOOKSModalComponent, CreateOrEditGLBOOKSModalComponent,

		APTermsComponent,
		ViewAPTermModalComponent, CreateOrEditAPTermModalComponent,
        AccountSubLedgersComponent,
        TransferAccountSubLedgerModalComponent,
		ViewAccountSubLedgerModalComponent, CreateOrEditAccountSubLedgerModalComponent,
		AccountSubLedgerChartofControlLookupTableModalComponent, AccountSubLedgerTaxAuthorityLookupTableModalComponent,

		ChartofControlsComponent,
		ViewChartofControlModalComponent, CreateOrEditChartofControlModalComponent,
		ChartofControlControlDetailLookupTableModalComponent,
		ChartofControlSubControlDetailLookupTableModalComponent,
		ChartofControlSegmentlevel3LookupTableModalComponent,
		Segmentlevel3sComponent,
		ViewSegmentlevel3ModalComponent, CreateOrEditSegmentlevel3ModalComponent,
		Segmentlevel3ControlDetailLookupTableModalComponent,
		Segmentlevel3SubControlDetailLookupTableModalComponent,
		SubControlDetailsComponent,
		ViewSubControlDetailModalComponent, CreateOrEditSubControlDetailModalComponent,
		SubControlDetailControlDetailLookupTableModalComponent,
		ControlDetailsComponent,
		ViewControlDetailModalComponent, CreateOrEditControlDetailModalComponent,
		GroupCodesComponent,
		ViewGroupCodeModalComponent, CreateOrEditGroupCodeModalComponent,
		GroupCategoriesComponent,
		ViewGroupCategoryModalComponent, CreateOrEditGroupCategoryModalComponent,
		BatchListPreviewsComponent,
		ViewBatchListPreviewModalComponent,
		VendorMasterComponent,
		CustomerMasterComponent,
		AccountSubledgerLookupComponent,
		FinanceComponent,
		ARDirectinvoiceComponent,
		BankReconcileComponent,
		CreateOrEditBankReconcileModalComponent,
		ViewBankReconcileModalComponent,
		DateEditorComponent,
		DateRendererComponent,
		CheckBoxCellComponent,
		LedgerTypesComponent,
		CreateOrEditLedgerTypeModalComponent,
		ViewLedgerTypeModalComponent,
		GlChequescomponent,CreateOrEditGlChequesModalComponent,ViewGlChequesModalComponent,
		ApprovedComponent,
		UnapprovedComponent,
		PostedComponent,
		RecurringVouchersPostingComponent,
		ARTermsComponent,
		ViewARTermModalComponent,
		ARTermsComponent,
		CreateOrEditARTermModalComponent,
		ARDirectinvoiceSTComponent,
		LCExpenseComponent,
		CreateOrEditLCExpenseModalComponent,
		ViewLCExpenseModalComponent,
		LCExpensesHDComponent,
		CreateOrEditLCExpensesHDModalComponent,
		ViewLCExpensesHDModalComponent,
		CRDRNoteComponent,
		CreateOrEditCRDRNoteModalComponent,
		ViewCRDRNoteModalComponent,
		ARCRDRComponent,
		GLTransferComponent,
		ViewGLTransferModalComponent,
		CreateOrEditGLTransferModalComponent,
		GlslgroupComponent,
		CreateOrEditglslgroupComponent,
		VoucherReversalComponent,
		CreateOrEditvoucherReversalComponent,
		RouteInvoicesComponent,
		CreateOrEditRouteInvoicesComponent
	],

	providers: [
		{ provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
		{ provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
		{ provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale },
		{ provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true },
		LedgerTypesServiceProxy,
		GLAccountsPermissionsServiceProxy,
		PLCategoriesServiceProxy,
		ARTermsServiceProxy
	],
	schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class FinanceModule { }
