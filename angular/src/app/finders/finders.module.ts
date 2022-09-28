import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AutoCompleteModule } from 'primeng/components/autocomplete/autocomplete';
import { PaginatorModule, EditorModule, InputMaskModule, FileUploadModule, TreeModule, DragDropModule, ContextMenuModule } from 'primeng/primeng';
import { TableModule } from 'primeng/table';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule, TooltipModule, BsLocaleService, BsDatepickerConfig, BsDaterangepickerConfig, TabsModule, PopoverModule, BsDropdownModule, BsDatepickerModule } from 'ngx-bootstrap';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { ChartofAccountFinderComponent } from './finance/chartofAccount-finder/chartofAccount-finder.component';
import { taxAuthorityFinderModalComponent } from './finance/taxAuthority-Finder/taxAuthority-Finder.component';
import { CommonServiceLookupTableModalComponent } from './commonService/commonService-lookup-table-modal.component';
import { FinanceLookupTableModalComponent } from './finance/finance-lookup-table-modal.component';
import { InventoryLookupTableModalComponent } from './supplyChain/inventory/inventory-lookup-table-modal.component';
import { PayRollLookupTableModalComponent } from './payRoll/payRoll-lookup-table-modal.component';
import { PurchaseLookupTableModalComponent } from './supplyChain/purchase/purchase-lookup-table-modal.component';
import { SalesLookupTableModalComponent } from './supplyChain/sales/sales-lookup-table-modal.component';
import { ToolTypeLookupTableModalComponent } from './Manufacturing/Tooltype-Modal/ToolType-table-modal.component';
import { ResourceLookupTableModalComponent } from './manufacturing/resource-lookup-table-modal/resource-lookup-table-modal.component';
import { LogComponent } from './log/log.component';


@NgModule({
  declarations: [
	ChartofAccountFinderComponent,
	taxAuthorityFinderModalComponent,
	CommonServiceLookupTableModalComponent,
	FinanceLookupTableModalComponent,
	InventoryLookupTableModalComponent,
	PurchaseLookupTableModalComponent,
	SalesLookupTableModalComponent,
	PayRollLookupTableModalComponent,
	ToolTypeLookupTableModalComponent,
	ResourceLookupTableModalComponent,
	LogComponent
  ],
  imports: [
	FormsModule,
	ReactiveFormsModule,
	CommonModule,
	ModalModule.forRoot(),
	TabsModule.forRoot(),
	TooltipModule.forRoot(),
	PopoverModule.forRoot(),
	BsDropdownModule.forRoot(),
	BsDatepickerModule.forRoot(),
	UtilsModule,
	AppCommonModule,
	TableModule,
	PaginatorModule,
	AutoCompleteModule
  ],
  exports: [
	ChartofAccountFinderComponent,
	taxAuthorityFinderModalComponent,
	CommonServiceLookupTableModalComponent,
	FinanceLookupTableModalComponent,
	InventoryLookupTableModalComponent,
	PurchaseLookupTableModalComponent,
	SalesLookupTableModalComponent,
	PayRollLookupTableModalComponent,
	ToolTypeLookupTableModalComponent,
	ResourceLookupTableModalComponent,
	LogComponent
  ]
})
export class FindersModule { }
