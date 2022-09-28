import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SalesRoutingModule } from './sales-routing.module';
import { SalesComponent } from './sales.component';
import { SaleAccountscomponent } from './saleAccounts/SaleAccounts.component';
import { SaleEntryComponent } from './saleEntry/saleEntry.component';
import { ViewSaleEntryModalComponent } from './saleEntry/view-saleEntry-modal.component';
import { CreateOrEditSaleEntryModalComponent } from './saleEntry/create-or-edit-saleEntry-modal.component';
import { FileUploadModule } from 'ng2-file-upload';
import {  FileUploadModule as PrimeNgFileUploadModule} from "primeng/primeng";
import { AutoCompleteModule, PaginatorModule, EditorModule, InputMaskModule, InputTextModule } from 'primeng/primeng';
import { TableModule } from 'primeng/table';
import { FormsModule } from '@angular/forms';
import { ModalModule, TabsModule, TooltipModule, BsDatepickerModule, BsDropdownModule, PopoverModule, BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import CountoModule from 'angular2-counto';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AgGridModule } from 'ag-grid-angular';
import { FindersModule } from '@app/finders/finders.module';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from 'abp-ng2-module/dist/src/abpHttpInterceptor';
import { CreateOrEditSaleAccountsModalComponent } from './saleAccounts/create-or-edit-SaleAccounts-modal.component';
import { ViewSaleAccountsComponent } from './saleAccounts/view-saleAccounts-modal.component';
import { SaleReturnComponent } from './saleReturn/saleReturn.component';
import { ViewSaleReturnModalComponent } from './saleReturn/view-saleReturn-modal.component';
import { CreateOrEditSaleReturnModalComponent } from './saleReturn/create-or-edit-saleReturn-modal.component';
import { CreateOrEditCreditDebitNoteModalComponent } from './creditDebitNote/create-or-edit-creditDebitNote-modal.component';
import { ViewCreditDebitNoteComponent } from './creditDebitNote/view-creditDebitNote-modal.component';
import { creditNoteComponent } from './creditDebitNote/creditNote.component';
import { DebitNoteComponent } from './creditDebitNote/debitNote.component';
import { SalesReferencesComponent } from './salesReference/salesReferences.component';
import { CreateOrEditSalesReferenceModalComponent } from './salesReference/create-or-edit-salesReference-modal.component';
import { ViewSalesReferenceModalComponent } from './salesReference/view-salesReference-modal.component';
import { SaleQutationComponent }from './sale-qutation/sale-qutation.component';
import {CreateOrEditSaleQutationComponent} from './sale-qutation/create-or-edit-sale-qutation.component';
import { CostSheetComponent } from './costSheet/costSheet.component';
import { CreateOrEditCostSheetComponent } from './costSheet/create-or-edit-costSheet.component';
import { InvoiceKnockOffComponent } from './invoice-knock-off/invoice-knock-off.component';
import { CreateOrEditInvoiceKnockOffComponent } from './invoice-knock-off/create-or-edit-invoice-knock-off.component';
import { CurrencyMaskModule } from "ng2-currency-mask";
import { OedriversComponent } from './oedrivers/oedrivers.component';
import { CreatOrEditOedriversComponent } from './oedrivers/creat-or-edit-oedrivers.component';
import { OeroutesComponent } from './oeRoutes/oe-routes.component';
import { CreateOrEditOeRoutesComponent } from './oeRoutes/create-or-edit-oe-routes.component';

@NgModule({
  declarations: [
    SaleReturnComponent,
    ViewSaleReturnModalComponent, CreateOrEditSaleReturnModalComponent,
    SaleEntryComponent,
    ViewSaleEntryModalComponent, CreateOrEditSaleEntryModalComponent,
    SalesComponent, SaleAccountscomponent,
    CreateOrEditSaleAccountsModalComponent,
    ViewSaleAccountsComponent,
    // CreditDebitNoteComponent,
    CreateOrEditCreditDebitNoteModalComponent,
    ViewCreditDebitNoteComponent,
    creditNoteComponent,
    DebitNoteComponent,
    SalesReferencesComponent,
    CreateOrEditSalesReferenceModalComponent,
    ViewSalesReferenceModalComponent,
    SaleQutationComponent,
    CreateOrEditSaleQutationComponent,
    CostSheetComponent,
    CreateOrEditCostSheetComponent,
    InvoiceKnockOffComponent,
    CreateOrEditInvoiceKnockOffComponent,
    OedriversComponent,
    CreatOrEditOedriversComponent,
    OeroutesComponent,
    CreateOrEditOeRoutesComponent
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
    PrimeNgFileUploadModule,
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
    AgGridModule.withComponents(null),
    FindersModule,
    InputTextModule,
    DialogModule,
    ButtonModule,
    CommonModule,
    SalesRoutingModule,
    CurrencyMaskModule
  ],
  providers: [
    { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
    { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
    { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale },
    { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true },
  ],
  exports: [SalesReferencesComponent]
})
export class SalesModule { }
