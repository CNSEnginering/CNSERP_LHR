import { AbpModule } from '@abp/abp.module';
import * as ngCommon from '@angular/common';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppLocalizationService } from '@app/shared/common/localization/app-localization.service';
import { AppNavigationService } from '@app/shared/layout/nav/app-navigation.service';
import { CommonModule } from '@shared/common/common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { ModalModule } from 'ngx-bootstrap';
import { PaginatorModule, FileUploadModule } from 'primeng/primeng';
import { TableModule } from 'primeng/table';
import { AppAuthService } from './auth/app-auth.service';
import { AppRouteGuard } from './auth/auth-route-guard';
import { CommonLookupModalComponent } from './lookup/common-lookup-modal.component';
import { EntityTypeHistoryModalComponent } from './entityHistory/entity-type-history-modal.component';
import { EntityChangeDetailModalComponent } from './entityHistory/entity-change-detail-modal.component';
import { DateRangePickerInitialValueSetterDirective } from './timing/date-range-picker-initial-value.directive';
import { DatePickerInitialValueSetterDirective } from './timing/date-picker-initial-value.directive';
import { DateTimeService } from './timing/date-time.service';
import { TimeZoneComboComponent } from './timing/timezone-combo.component';
import { FileUploaderComponent } from './file-uploader/file-uploader.component';
import { CheckboxCellComponent } from './checkbox-cell/checkbox-cell.component';
import { AgDatePickerComponent } from './ag-date-picker/ag-date-picker.component';
import { LegderTypeComboboxService } from './legdertype-combobox/legdertype-combobox.service';
import { DxReportViewerModule } from 'devexpress-reporting-angular';
import { ReportviewrModalComponent } from './reportviewr-modal/reportviewr-modal.component';
import { ReportViewService } from './reportviewr-modal/reportView.service';



@NgModule({
    imports: [
        ngCommon.CommonModule,
        FormsModule,
        FileUploadModule,
        ReactiveFormsModule,
        ModalModule.forRoot(),
        UtilsModule,
        AbpModule,
        CommonModule,
        TableModule,
        PaginatorModule,
        DxReportViewerModule
    ],
    declarations: [
        TimeZoneComboComponent,
        CommonLookupModalComponent,
        EntityTypeHistoryModalComponent,
        EntityChangeDetailModalComponent,
        DateRangePickerInitialValueSetterDirective,
        DatePickerInitialValueSetterDirective,
        FileUploaderComponent,
        CheckboxCellComponent,
        AgDatePickerComponent,
        ReportviewrModalComponent
    ],
    exports: [
        TimeZoneComboComponent,
        CommonLookupModalComponent,
        EntityTypeHistoryModalComponent,
        EntityChangeDetailModalComponent,
        DateRangePickerInitialValueSetterDirective,
        DatePickerInitialValueSetterDirective,
        CheckboxCellComponent,
        AgDatePickerComponent, 
        ReportviewrModalComponent
    ],
    providers: [
        DateTimeService,
        AppLocalizationService,
        AppNavigationService,
        LegderTypeComboboxService,
        ReportViewService
        
    ]
})
export class AppCommonModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: AppCommonModule,
            providers: [
                AppAuthService,
                AppRouteGuard
            ]
        };
    }
}
