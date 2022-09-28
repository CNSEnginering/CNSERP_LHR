import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManufacturingComponent } from './manufacturing.component';
import { FormsModule } from '@angular/forms';
import { FindersModule } from '@app/finders/finders.module';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AgGridModule } from 'ag-grid-angular';
import CountoModule from 'angular2-counto';
import { FileUploadModule } from 'ng2-file-upload';
import { ModalModule, TabsModule, TooltipModule, BsDatepickerModule, BsDropdownModule, PopoverModule, BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { AutoCompleteModule, PaginatorModule, EditorModule, InputMaskModule, InputTextModule } from 'primeng/primeng';
import { TableModule } from 'primeng/table';
import { CreateOrEditManufacAccModalComponent } from './setup/accountSetup/create-or-edit-manufacAcc-modal.component';
import { ViewManufacAccModalComponent } from './setup/accountSetup/view-manufacAcc-modal.component';
import { ManufacAccComponent } from './setup/accountSetup/manufacAcc.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from 'abp-ng2-module/dist/src/abpHttpInterceptor';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { ManufacturingRoutingModule } from './manufacturing-routing.module';
import { ProductionAreaComponent } from './setup/productionArea/ProductionArea.component';
import { CreateOrEditProductionAreaComponent } from './setup/productionArea/create-or-edit-productionarea.component';
import { ViewProductionAreaComponent } from './setup/productionArea/view-productionarea.component';
import { ResourceMasterComponent } from './setup/resource-master/resource.component';
import { ViewResourceMasterModalComponent } from './setup/resource-master/view-resource-modal.component';
import { CreateOrEditResourceMasterModalComponent } from './setup/resource-master/create-or-edit-resource-modal.component';
import { CreateOrEditMftoolComponent } from './setup/mftool/create-or-edit-mftool.component';
import { MftoolComponent } from './setup/mftool/mftool.component';
import { ViewMftoolComponent } from './setup/mftool/view-mftool.component';
import { CreateOrEditMftooltyComponent } from './setup/mftoolty/create-or-edit-mftoolty.component';
import { MftooltyComponent } from './setup/mftoolty/mftoolty.component';
import { ViewMftooltyComponent } from './setup/mftoolty/view-mftoolty.component';
import { MfWorkingCenterComponent } from './setup/mf-working-center/mf-working-center.component';
import { CreateoreditworkingcenterComponent } from './setup/mf-working-center/createoreditworkingcenter.component';
import { ViewMfWorkingComponent } from './setup/mf-working-center/view-mf-working.component';

@NgModule({
  declarations: [
    ManufacturingComponent, CreateOrEditManufacAccModalComponent,
    ViewManufacAccModalComponent, ManufacAccComponent,
    ProductionAreaComponent, CreateOrEditProductionAreaComponent, ViewProductionAreaComponent,
    ResourceMasterComponent, CreateOrEditResourceMasterModalComponent, ViewResourceMasterModalComponent,
    CreateOrEditMftooltyComponent, MftooltyComponent, ViewMftooltyComponent, CreateOrEditMftoolComponent,
     MftoolComponent, ViewMftoolComponent, MfWorkingCenterComponent, CreateoreditworkingcenterComponent, 
     ViewMfWorkingComponent
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
    CommonModule,
    ManufacturingRoutingModule
  ],
  providers: [
    { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
    { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
    { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale },
    { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true },
  ],
})
export class ManufacturingModule { }
