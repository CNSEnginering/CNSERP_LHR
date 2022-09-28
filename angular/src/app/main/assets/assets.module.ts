import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService, ModalModule, TabsModule, TooltipModule, BsDatepickerModule, BsDropdownModule, PopoverModule } from 'ngx-bootstrap';
import { FileUploadModule } from 'ng2-file-upload';
import { AutoCompleteModule, PaginatorModule, EditorModule, InputMaskModule } from 'primeng/primeng';
import { TableModule } from 'primeng/table';
import { FormsModule } from '@angular/forms';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import CountoModule from 'angular2-counto';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AgGridModule } from 'ag-grid-angular';
import { FindersModule } from '@app/finders/finders.module';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from 'abp-ng2-module/dist/src/abpHttpInterceptor';
import { AssetsRoutingModule } from './assets-routing.module';


@NgModule({
  declarations: [
    
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
		AssetsRoutingModule,
		FindersModule

  ],
  providers: [
		{ provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
		{ provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
		{ provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale },
		{ provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true }
	],
	schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AssetsModule { }
