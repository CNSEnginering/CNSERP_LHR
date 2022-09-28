import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PurchaseRoutingModule } from './purchase-routing.module';
import { RequisitionComponent } from './Requisition/requisition.component';
import { PurchaseComponent } from './purchase.component';
import { PurchaseOrdersComponent } from './purchaseOrder/purchaseOrders.component';
import { ViewPurchaseOrderModalComponent } from './purchaseOrder/view-purchaseOrder-modal.component';
import { CreateOrEditPurchaseOrderModalComponent } from './purchaseOrder/create-or-edit-purchaseOrder-modal.component';
import { BsDatepickerModule, BsDropdownModule, PopoverModule, ModalModule, TabsModule, TooltipModule, BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap';
import { AgGridModule } from 'ag-grid-angular';
//import { FileUploadModule } from 'ng2-file-upload';
import { FileUploadModule } from 'primeng/primeng';
import { AutoCompleteModule, PaginatorModule, EditorModule, InputMaskModule, InputTextModule } from 'primeng/primeng';
import { TableModule } from 'primeng/table';
import { FormsModule } from '@angular/forms';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import CountoModule from 'angular2-counto';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { FindersModule } from '@app/finders/finders.module';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from 'abp-ng2-module/dist/src/abpHttpInterceptor';
import { CreateOrEditRequisitionModalComponent } from './Requisition/create-or-edit-requisition-modal.component';
import { ViewRequisitionComponent } from './Requisition/view-requisition-modal.component';
import { ReceiptEntryComponent } from './receiptEntry/receiptEntry.component';
import { ViewReceiptEntryModalComponent } from './receiptEntry/view-receiptEntry-modal.component';
import { CreateOrEditReceiptEntryModalComponent } from './receiptEntry/create-or-edit-receiptEntry-modal.component';
import { ReceiptReturnComponent } from './receiptReturn/receiptReturn.component';
import { ViewReceiptReturnModalComponent } from './receiptReturn/view-receiptReturn-modal.component';
import { CreateOrEditReceiptReturnModalComponent } from './receiptReturn/create-or-edit-receiptReturn-modal.component';

import { LightboxModule } from 'ngx-lightbox';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { ApinvhComponent } from './apinvh/apinvh.component';
import { CreateOrEditApinvhComponent } from './apinvh/create-or-edit-apinvh.component';
import { ViewApinvhComponent } from './apinvh/view-apinvh.component';

@NgModule({
  declarations: [
    PurchaseOrdersComponent,
    ViewPurchaseOrderModalComponent, CreateOrEditPurchaseOrderModalComponent,
    ReceiptEntryComponent,
    ViewReceiptEntryModalComponent, CreateOrEditReceiptEntryModalComponent,
    ReceiptReturnComponent,
    ViewReceiptReturnModalComponent, CreateOrEditReceiptReturnModalComponent,
    RequisitionComponent,
    CreateOrEditRequisitionModalComponent,
    ViewRequisitionComponent,
    PurchaseComponent,
    ApinvhComponent,
    CreateOrEditApinvhComponent,
    ViewApinvhComponent,
   
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
    PurchaseRoutingModule,
    LightboxModule,
    CurrencyMaskModule
  ],
  providers: [
		{ provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
		{ provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
		{ provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale },
		{ provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true },
  ],
})
export class PurchaseModule { }
