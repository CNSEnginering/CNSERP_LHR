import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TaxClassesComponent } from './commonServices/taxClasses/taxClasses.component';
import { FiscalCalendersComponent } from './finance/GeneralLedger/setupForms/fiscalCalenders/fiscalCalenders.component';
import { BanksComponent } from './commonServices/banks/banks.component';
import { BkTransfersComponent } from './commonServices/bkTransfers/bkTransfers.component';
import { CompanyProfilesComponent } from './commonServices/companyProfiles/companyProfiles.component';
import { TaxAuthoritiesComponent } from './commonServices/taxAuthorities/taxAuthorities.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MainComponent } from './main.component';
import { CurrencyRatesComponent } from './commonServices/currencyRates/currencyRates.component';
import { VoucherPostingComponent } from './supplyChain/periodics/voucherPosting/voucherPosting.component';
import { ApprovalComponent } from './supplyChain/periodics/Approval/Approval.component';
import { CprComponent } from './commonServices/cprNumbers/cpr.component';
import { ChequeBooksComponent } from './commonServices/chequeBooks/chequeBooks.component';
import { RecurringVouchersComponent } from './commonServices/recurringVouchers/recurringVouchers.component';
import { OpenBiComponent } from './OpenBI/openbi.component';
import { UserlocComponent } from './commonServices/userloc/userloc.component';
@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: MainComponent,
                //canActivate: [AppRouteGuard],
                //canActivateChild: [AppRouteGuard],
                children: [
                    { path: 'commonServices/taxClasses', component: TaxClassesComponent, data: { permission: 'Pages.TaxClasses' } },
                    { path: 'supplyChain/periodics/voucherPosting', component: VoucherPostingComponent, data: { permission: 'SupplyChain.VoucherPosting' } },
                    { path: 'supplyChain/periodics/Approval', component: ApprovalComponent, data: { permission: 'SupplyChain.Approval' } },
                    { path: 'GeneralLedger/setupForms/fiscalCalenders', component: FiscalCalendersComponent, data: { permission: 'Pages.FiscalCalenders' } },
                    { path: 'commonServices/banks', component: BanksComponent, data: { permission: 'Pages.Banks' } },
                    { path: 'commonServices/chequeBooks', component: ChequeBooksComponent, data: { permission: 'Pages.ChequeBooks' } },
                    { path: 'commonServices/userloc', component: UserlocComponent, data: { permission: 'Pages.CSUserLocH' } },

                    { path: 'commonServices/cprNumbers', component: CprComponent, data: { permission: 'Pages.CPR' } },
                    { path: 'commonServices/bkTransfers', component: BkTransfersComponent, data: { permission: 'Pages.BkTransfers' } },
                    { path: 'commonServices/companyProfiles', component: CompanyProfilesComponent, data: { permission: 'SetupForms.CompanyProfiles' } },
                    { path: 'commonServices/taxAuthorities', component: TaxAuthoritiesComponent, data: { permission: 'Pages.TaxAuthorities' } },
                    { path: 'commonServices/currencyRates', component: CurrencyRatesComponent, data: { permission: 'SetupForms.CurrencyRates' } },
                    { path: 'commonServices/recurringVouchers', component: RecurringVouchersComponent, data: { permission: 'Pages.RecurringVouchers' } },
                    { path: 'dashboard', component: DashboardComponent, data: { permission: 'Pages.Tenant.Dashboard' } },
                    { path: 'openbi', component: OpenBiComponent, data: { permission: 'Pages.Tenant.Dashboard' } },
                    {
                        path: 'reports',
                        loadChildren: () => import('app/main/reports/reports.module').then(m => m.ReportsModule),
                        data: { preload: true }, //Lazy load Reports module
                    },
                    {
                        path: 'finance',
                        loadChildren: () => import('app/main/finance/finance.module').then(m => m.FinanceModule),
                        data: { preload: true }, //Lazy load Finance module
                    },
                    {
                        path: 'supplyChain',
                        loadChildren: () => import('app/main/supplyChain/inventory/inventory.module').then(m => m.InventoryModule),
                        data: { preload: true }, //Lazy load Inventory module
                    },
                    {
                        path: 'supplyChain',
                        loadChildren: () => import('app/main/supplyChain/purchase/purchase.module').then(m => m.PurchaseModule),
                        data: { preload: true }, //Lazy load Purchase module
                    },
                    {
                        path: 'payroll',
                        loadChildren: () => import('app/main/payroll/payroll.module').then(m => m.PayrollModule),
                        data: { preload: true }, //Lazy load payroll module
                    },
                    {
                        path: 'supplyChain',
                        loadChildren: () => import('app/main/supplyChain/sales/sales.module').then(m => m.SalesModule),
                        data: { preload: true }, //Lazy load sales module
                    },
                    {
                        path: 'manufacturing',
                        loadChildren: () => import('app/main/manufacturing/manufacturing.module').then(m => m.ManufacturingModule),
                        data: { preload: true }, //Lazy load manufacturing module
                    }
                ]
            }
        ])],
    exports: [
        RouterModule
    ]
})
export class MainRoutingModule { }
