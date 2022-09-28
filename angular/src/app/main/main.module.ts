import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { TaxClassesComponent } from './commonServices/taxClasses/taxClasses.component';
import { ViewTaxClassModalComponent } from './commonServices/taxClasses/view-taxClass-modal.component';
import { CreateOrEditTaxClassModalComponent } from './commonServices/taxClasses/create-or-edit-taxClass-modal.component';
import { FiscalCalendersComponent } from './finance/GeneralLedger/setupForms/fiscalCalenders/fiscalCalenders.component';
import { ViewFiscalCalenderModalComponent } from './finance/GeneralLedger/setupForms/fiscalCalenders/view-fiscalCalender-modal.component';
import { CreateOrEditFiscalCalenderModalComponent } from './finance/GeneralLedger/setupForms/fiscalCalenders/create-or-edit-fiscalCalender-modal.component';
import { BanksComponent } from './commonServices/banks/banks.component';
import { ViewBankModalComponent } from './commonServices/banks/view-bank-modal.component';
import { CreateOrEditBankModalComponent } from './commonServices/banks/create-or-edit-bank-modal.component';
import { CompanyProfilesComponent } from './commonServices/companyProfiles/companyProfiles.component';
import { ViewCompanyProfileModalComponent } from './commonServices/companyProfiles/view-companyProfile-modal.component';
import { CreateOrEditCompanyProfileModalComponent } from './commonServices/companyProfiles/create-or-edit-companyProfile-modal.component';
import { AutoCompleteModule } from 'primeng/primeng';
import { PaginatorModule } from 'primeng/primeng';
import { EditorModule } from 'primeng/primeng';
import { InputMaskModule } from 'primeng/primeng'; import { FileUploadModule } from 'primeng/primeng';
import { TableModule } from 'primeng/table';
import { UtilsModule } from '@shared/utils/utils.module';
import { CountoModule } from 'angular2-counto';
import { ModalModule, TabsModule, TooltipModule, BsDropdownModule, PopoverModule } from 'ngx-bootstrap';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MainRoutingModule } from './main-routing.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { BsDatepickerModule, BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { AgGridModule } from 'ag-grid-angular';
import { MainComponent } from './main.component';
import { BkTransferBankLookupTableModalComponent1 } from './commonServices/bkTransfers/bkTransfer-bank-lookup-table-modal.component1';
import { BkTransfersComponent } from './commonServices/bkTransfers/bkTransfers.component';
import { CreateOrEditBkTransferModalComponent } from './commonServices/bkTransfers/create-or-edit-bkTransfer-modal.component';
import { ViewBkTransferModalComponent } from './commonServices/bkTransfers/view-bkTransfer-modal.component';
import { BkTransferBankLookupTableModalComponent } from './commonServices/bkTransfers/bkTransfer-bank-lookup-table-modal.component';
import { TaxAuthoritiesComponent } from './commonServices/taxAuthorities/taxAuthorities.component';
import { ViewTaxAuthorityModalComponent } from './commonServices/taxAuthorities/view-taxAuthority-modal.component';
import { CreateOrEditTaxAuthorityModalComponent } from './commonServices/taxAuthorities/create-or-edit-taxAuthority-modal.component';
import { TaxAuthorityCompanyProfileLookupTableModalComponent } from "./commonServices/taxAuthorities/taxAuthority-companyProfile-lookup-table-modal.component";
import { CurrencyRatesComponent } from './commonServices/currencyRates/currencyRates.component';
import { ViewCurrencyRateModalComponent } from './commonServices/currencyRates/view-currencyRate-modal.component';
import { CreateOrEditCurrencyRateModalComponent } from './commonServices/currencyRates/create-or-edit-currencyRate-modal.component';
import { CurrencyRateCompanyProfileLookupTableModalComponent } from './commonServices/currencyRates/currencyRate-companyProfile-lookup-table-modal.component';
import { VoucherPostingComponent } from './supplyChain/periodics/voucherPosting/voucherPosting.component'
import { FindersModule } from '@app/finders/finders.module';
import { ApprovalComponent } from './supplyChain/periodics/Approval/Approval.component';
import { CprComponent } from './commonServices/cprNumbers/cpr.component';
import { CreateOrEditCprModalComponent } from './commonServices/cprNumbers/create-or-edit-cpr-modal.component';
import { ViewCprModalComponent } from './commonServices/cprNumbers/view-cpr-modal.component';
import { ChequeBooksComponent } from './commonServices/chequeBooks/chequeBooks.component';
import { CreateOrEditChequeBookModalComponent } from './commonServices/chequeBooks/create-or-edit-chequeBook-modal.component';
import { ViewChequeBookModalComponent } from './commonServices/chequeBooks/view-chequeBook-modal.component';
import { CurrencyMaskModule } from "ng2-currency-mask";
import { RecurringVouchersComponent } from './commonServices/recurringVouchers/recurringVouchers.component';
import { CreateOrEditRecurringVoucherModalComponent } from './commonServices/recurringVouchers/create-or-edit-recurringVoucher-modal.component';
import { ViewRecurringVoucherModalComponent } from './commonServices/recurringVouchers/view-recurringVoucher-modal.component';
import { OpenBiComponent } from './OpenBI/openbi.component';
import { UserlocComponent } from './commonServices/userloc/userloc.component';;
import { CreateOrEditUserlocComponent } from './commonServices/userloc/create-or-edit-userloc.component';
import { CheckboxCellComponent } from '@app/shared/common/checkbox-cell/checkbox-cell.component';
import { AgDatePickerComponent } from '@app/shared/common/ag-date-picker/ag-date-picker.component';
// import { DateEditorComponent } from './GeneralLedger/transaction/bankReconcile/date-editor.component';
// import { DateRendererComponent } from './GeneralLedger/transaction/bankReconcile/date-renderer.component';
 import { CheckBoxCellComponent } from '../main/commonServices/userloc/checkBoxCell/checkbox-cell.component';
import { InventoryModule } from './supplyChain/inventory/inventory.module';
//import {CaderComponent } from '../main/payroll/cader/cader.component';
NgxBootstrapDatePickerConfigService.registerNgxBootstrapDatePickerLocales();
@NgModule({
	imports: [
		FileUploadModule,
		AutoCompleteModule,
		PaginatorModule,
		EditorModule,
		InputMaskModule,
		TableModule,
		CurrencyMaskModule,
		FormsModule,
		ModalModule,
		TabsModule,
		TooltipModule,
		AppCommonModule,
		UtilsModule,
		MainRoutingModule,
		AgGridModule.withComponents([
			CheckboxCellComponent,AgDatePickerComponent,
			CheckBoxCellComponent
		]),
		CountoModule,
		NgxChartsModule,
		BsDatepickerModule.forRoot(),
		BsDropdownModule.forRoot(),
		PopoverModule.forRoot(),
		//AgGridModule.withComponents([]),
		FindersModule,
		InventoryModule
	],
	declarations: [
		MainComponent,
		TaxClassesComponent,
		ViewTaxClassModalComponent, CreateOrEditTaxClassModalComponent,
		FiscalCalendersComponent,
		ViewFiscalCalenderModalComponent, CreateOrEditFiscalCalenderModalComponent,
		BkTransferBankLookupTableModalComponent1,
		BkTransferBankLookupTableModalComponent,
		BkTransfersComponent,
		CreateOrEditBkTransferModalComponent,
		ViewBkTransferModalComponent,
		BanksComponent,
		CheckBoxCellComponent,
		ViewBankModalComponent, CreateOrEditBankModalComponent,
		CprComponent,
		CreateOrEditCprModalComponent,
		ViewCprModalComponent,
		//CaderComponent,
		CompanyProfilesComponent,
		ViewCompanyProfileModalComponent, CreateOrEditCompanyProfileModalComponent,
		TaxAuthoritiesComponent,
		ViewTaxAuthorityModalComponent, CreateOrEditTaxAuthorityModalComponent,
		TaxAuthorityCompanyProfileLookupTableModalComponent,
		CurrencyRatesComponent,
		ViewCurrencyRateModalComponent, CreateOrEditCurrencyRateModalComponent,
		CurrencyRateCompanyProfileLookupTableModalComponent,
		DashboardComponent,
		VoucherPostingComponent,
		ApprovalComponent,
		ChequeBooksComponent,
		CreateOrEditChequeBookModalComponent,
		ViewChequeBookModalComponent,
		RecurringVouchersComponent,
		CreateOrEditRecurringVoucherModalComponent,
		ViewRecurringVoucherModalComponent,
		OpenBiComponent
,
		UserlocComponent
,
		CreateOrEditUserlocComponent
	],
	providers: [
		{ provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
		{ provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
		{ provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale },
	]
})
export class MainModule { }
