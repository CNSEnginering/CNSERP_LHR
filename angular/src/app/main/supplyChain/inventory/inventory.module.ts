import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
import { CommonModule } from "@angular/common";
import { InventoryRoutingModule } from "./inventory-routing.module";
import {
    BsDatepickerConfig,
    BsDaterangepickerConfig,
    BsLocaleService,
    ModalModule,
    TabsModule,
    TooltipModule,
    BsDatepickerModule,
    BsDropdownModule,
    PopoverModule
} from "ngx-bootstrap";
import { NgxBootstrapDatePickerConfigService } from "assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service";
import { InventoryComponent } from "./inventory.component";
import { ICSetupsComponent } from "./ic-setup/ic-setup.component";
import { CreateOrEditICSetupModalComponent } from "./ic-setup/create-or-edit-ic-setup-modal.component";
import { ViewICSetupModalComponent } from "./ic-setup/view-ic-setup-modal.component";
import { InventoryGlLinkcomponent } from "./InventoryGlLink/InventoryGlLink.component";
import { FileUploadModule } from "ng2-file-upload";
import {
    AutoCompleteModule,
    PaginatorModule,
    FileUploadModule as PrimeNgFileUploadModule,
    EditorModule,
    InputMaskModule
} from "primeng/primeng";
import { TableModule } from "primeng/table";
import { InputTextModule } from "primeng/inputtext";
import { ButtonModule } from "primeng/button";
import { DialogModule } from "primeng/dialog";
import { FormsModule } from "@angular/forms";
import { AppCommonModule } from "@app/shared/common/app-common.module";
import { UtilsModule } from "@shared/utils/utils.module";
import CountoModule from "angular2-counto";
import { NgxChartsModule } from "@swimlane/ngx-charts";
import { AgGridModule } from "ag-grid-angular";
import { InventoryGlLinkLookupTableModalComponent } from "./FinderModals/InventoryGlLink-lookup-table-modal.component";
import { CreateOrEditInventoryGlLinkModalComponent } from "./InventoryGlLink/create-or-edit-InventoryGlLink-modal.component";
import { PriceListComponent } from "./priceList/priceList.component";
import { ICLocationsComponent } from "./icLocations/icLocations.component";
import { ViewICLocationModalComponent } from "./icLocations/view-icLocation-modal.component";
import { CreateOrEditICLocationModalComponent } from "./icLocations/create-or-edit-icLocation-modal.component";
import { ViewICUOMModalComponent } from "./icuoMs/view-icuom-modal.component";
import { CreateOrEditICUOMModalComponent } from "./icuoMs/create-or-edit-icuom-modal.component";
import { ICUOMsComponent } from "./icuoMs/icuoMs.component";
import { ReorderLevelsComponent } from "./reorderLevels/reorderLevels.component";
import { ViewReorderLevelModalComponent } from "./reorderLevels/view-reorderLevel-modal.component";
import { CreateOrEditReorderLevelModalComponent } from "./reorderLevels/create-or-edit-reorderLevel-modal.component";
import { TransactionTypesComponent } from "./transactionTypes/transactionTypes.component";
import { ViewTransactionTypeModalComponent } from "./transactionTypes/view-transactionType-modal.component";
import { CreateOrEditTransactionTypeModalComponent } from "./transactionTypes/create-or-edit-transactionType-modal.component";
import { CreateOrEditPriceListNewModalComponent } from "./priceList/create-or-edit-priceListNew-modal.component";
import { ItemPricingComponent } from "./itemPricing/itemPricing.component";
import { CreateOrEditItemPricingModalComponent } from "./itemPricing/create-or-edit-itemPricing-modal.component";
import { ItemPricingLookupTableModalComponent } from "./FinderModals/itemPricing-lookup-table-modal.component";
import { CostCenterComponent } from "./costCenter/costCenter.component";
import { CostCenterLookupTableModalComponent } from "./FinderModals/costCenter-lookup-table-modal.component";
import { CreateOrEditCostCenterModalComponent } from "./costCenter/create-or-edit-costCenter-modal.component";
import { SubCostCenterComponent } from "./subCostCenter/subCostCenter.component";
import { SubCostCenterLookupTableModalComponent } from "./FinderModals/subCostCenter-lookup-table-modal.component";
import { CreateOrEditSubCostCenterModalComponent } from "./subCostCenter/create-or-edit-subCostCenter-modal.component";
import { IcSegment1Component } from "./setupForms/ic-segment1/ic-segment1.component";
import { IcSegment2Component } from "./setupForms/ic-segment2/ic-segment2.component";
import { IcSegment3Component } from "./setupForms/ic-segment3/ic-segment3.component";
import { CreateOrEditIcSegment1ModalComponent } from "./setupForms/ic-segment1/create-or-edit-IcSegment1-modal.component";
import { CreateOrEditIcSegment2ModalComponent } from "./setupForms/ic-segment2/create-or-edit-Icsegment2-Modal.component";
import { CreateOrEditIcSegment3ModalComponent } from "./setupForms/ic-segment3/create-or-edit-icsegment3-Modal.component";
import { ViewIcsegment3ModalComponent } from "./setupForms/ic-segment3/view-icsegment3-modal.component";
import { ViewIcSegment2ModalComponent } from "./setupForms/ic-segment2/view-IcSegment2-modal.component";
import { ViewIcsegment1ModalComponent } from "./setupForms/ic-segment1/view-icsegment1-modal.component";
import * as ApiSegment1ServiceProxy from "./shared/services/ic-segment1-service";
import * as ApiSegment2ServiceProxy from "./shared/services/ic-segment2-service";
import * as ApiSegment3ServiceProxy from "./shared/services/ic-segment3-service";
import { AbpHttpInterceptor } from "@abp/abpHttpInterceptor";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { ViewInventoryGlLinkComponent } from "./InventoryGlLink/view-InventoryGlLink-modal.component";
import { ViewItemPricingComponent } from "./itemPricing/view-itemPricing-modal.component";
import { IcSegmentFinderModalComponent } from "./FinderModals/ic-segment-finder-modal.component";
import { IcSegment2FinderModalComponent } from "./FinderModals/ic-segment2-finder-modal.component";
import { IcItemsComponent } from "./setupForms/ic-items/ic-items.component";
import { CreateOrEditIcItemModalComponent } from "./setupForms/ic-items/create-or-edit-ic-item-modal.component";
import { IcItemServiceProxy } from "./shared/services/ic-Item.service";
import { IcSegment3FinderModalComponent } from "./FinderModals/ic-segment3-finder-modal.component";
import { ViewPriceListComponent } from "./priceList/view-priceList-modal.component";
import { ViewCostCenterComponent } from "./costCenter/view-costCenter-modal.component";
import { ViewSubCostCenterComponent } from "./subCostCenter/view-subCostCenter-modal.component";
import { GatePassComponent } from "./gatePass/gatePass.component";
import { CreateOrEditGetPassModalComponent } from "./gatePass/create-or-edit-gatePass-modal.component";
import { GatePassLookupTableModalComponent } from "./FinderModals/gatePass-lookup-table-modal.component";
import { AdjustmentsComponent } from "./adjustments/adjustments.component";
import { ViewAdjustmentModalComponent } from "./adjustments/view-adjustment-modal.component";
import { CreateOrEditAdjustmentModalComponent } from "./adjustments/create-or-edit-adjustment-modal.component";
import { FindersModule } from "@app/finders/finders.module";
import { TransfersComponent } from "./transfers/transfers.component";
import { CreateOrEditTransfersModalComponent } from "./transfers/create-or-edit-transfers-modal.component";
import { ICUOMLookupTableModalComponent } from "./FinderModals/icuom-lookup-table-modal.component";
import { CreateOrEditICOPT4ModalComponent } from "./setupForms/ic-OPT4/create-or-edit-icopT4-modal.component";
import { ICOPT4Component } from "./setupForms/ic-OPT4/icopT4.component";
import { ViewICOPT4ModalComponent } from "./setupForms/ic-OPT4/view-icopT4-modal.component";
import { CreateOrEditICOPT5ModalComponent } from "./setupForms/ic-OPT5/create-or-edit-icopT5-modal.component";
import { ICOPT5Component } from "./setupForms/ic-OPT5/icopT5.component";
import { ViewICOPT5ModalComponent } from "./setupForms/ic-OPT5/view-icopT5-modal.component";

import { ICOPT4ServiceProxy } from "./shared/services/ic-opt4-service";
import { ICOPT5ServiceProxy } from "./shared/services/ic-opt5-service";
import { ConsumptionsComponent } from "./consumption/consumptions.component";
import { ViewConsumptionModalComponent } from "./consumption/view-consumption-modal.component";
import { CreateOrEditConsumptionModalComponent } from "./consumption/create-or-edit-consumption-modal.component";
import { ViewTransfersComponent } from "./transfers/view-transfers-modal.component";
import { ViewGatePassComponent } from "./gatePass/view-gatePass-modal.component";
import { OpeningsComponent } from "./opening/openings.component";
import { ViewOpeningModalComponent } from "./opening/view-opening-modal.component";
import { CreateOrEditOpeningModalComponent } from "./opening/create-or-edit-opening-modal.component";
import { CreateOrEditAssemblyModalComponent } from "./assembly/create-or-edit-assembly-modal.component";
import { ViewAssemblyComponent } from "./assembly/view-assembly-modal.component";
import { AssemblyComponent } from "./assembly/assembly.component";
import { Opt4LookupTableModalComponent } from "./FinderModals/opt4-lookup-table-modal.component";
import { Opt5LookupTableModalComponent } from "./FinderModals/opt5-lookup-table-modal.component";
import { ViewItemModalComponent } from "./setupForms/ic-items/view-ic-items-modal.component";
import { IcUnitServiceProxy } from "./shared/services/ic-units-service";
import { IcItemPrictureModalComponent } from "./setupForms/ic-items/ic-item-pricture-modal.component";
import { AssetRegistrationComponent } from "./assetRegistration/assetRegistration.component";
import { CreateOrEditAssetRegistrationModalComponent } from "./assetRegistration/create-or-edit-assetRegistration-modal.component";
import { ViewAssetRegistrationModalComponent } from "./assetRegistration/view-assetRegistration-modal.component";
import { AssetRegistrationsServiceProxy } from "./shared/services/assetRegistration.service";
import { ImageCropperModule } from "ngx-image-cropper";
import { AssetRegistrationDetailServiceProxy } from "./shared/services/assetRegistrationDetail.service";
import { WorkOrderComponent } from "./workOrder/workOrder.component";
import { ViewWorkOrderModalComponent } from "./workOrder/view-workOrder-modal.component";
import { CreateOrEditWorkOrderModalComponent } from "./workOrder/create-or-edit-workOrder-modal.component";
import { RegionsComponent } from "./regions/regions.component";
import { CreateOrEditRegionsModalComponent } from "./regions/create-or-edit-regions-modal.component";
import { LightboxModule } from 'ngx-lightbox';
import { CurrencyMaskModule } from "ng2-currency-mask";
import { SalesModule } from "../sales/sales.module";
import { InventoryReferencesComponent } from "./ReferenceInventory/InventoryReferences.component";
import { CreateOreditIclotComponent } from './iclot/create-oredit-iclot.component';
import { IclotComponent } from './iclot/iclot.component';

@NgModule({
    declarations: [
        OpeningsComponent,
        ViewOpeningModalComponent,
        CreateOrEditOpeningModalComponent,
        WorkOrderComponent,
        ViewWorkOrderModalComponent,
        CreateOrEditWorkOrderModalComponent,
        ConsumptionsComponent,
        ViewConsumptionModalComponent,
        CreateOrEditConsumptionModalComponent,
        AdjustmentsComponent,
        ViewAdjustmentModalComponent,
        CreateOrEditAdjustmentModalComponent,
        ICOPT4Component,
        ICOPT5Component,
        CreateOrEditICOPT4ModalComponent,
        ViewICOPT4ModalComponent,
        CreateOrEditICOPT5ModalComponent,
        ViewICOPT5ModalComponent,
        ReorderLevelsComponent,
        ViewReorderLevelModalComponent,
        CreateOrEditReorderLevelModalComponent,
        TransactionTypesComponent,
        ViewTransactionTypeModalComponent,
        CreateOrEditTransactionTypeModalComponent,
        ICUOMsComponent,
        ViewICUOMModalComponent,
        CreateOrEditICUOMModalComponent,
        ICLocationsComponent,
        ViewICLocationModalComponent,
        CreateOrEditICLocationModalComponent,
        ICSetupsComponent,
        ViewICSetupModalComponent,
        CreateOrEditICSetupModalComponent,
        InventoryGlLinkcomponent,
        InventoryComponent,
        InventoryGlLinkLookupTableModalComponent,
        CreateOrEditInventoryGlLinkModalComponent,
        PriceListComponent,
        CreateOrEditPriceListNewModalComponent,
        ItemPricingComponent,
        CreateOrEditItemPricingModalComponent,
        ItemPricingLookupTableModalComponent,
        ICUOMLookupTableModalComponent,
        CostCenterComponent,
        CostCenterLookupTableModalComponent,
        CreateOrEditCostCenterModalComponent,
        SubCostCenterComponent,
        SubCostCenterLookupTableModalComponent,
        CreateOrEditSubCostCenterModalComponent,
        CreateOrEditCostCenterModalComponent,
        CreateOrEditItemPricingModalComponent,
        IcSegment1Component,
        IcSegment2Component,
        IcSegment3Component,
        CreateOrEditIcSegment1ModalComponent,
        CreateOrEditIcSegment2ModalComponent,
        CreateOrEditIcSegment3ModalComponent,
        ViewIcsegment3ModalComponent,
        ViewIcSegment2ModalComponent,
        ViewIcsegment1ModalComponent,
        ViewInventoryGlLinkComponent,
        ViewItemPricingComponent,
        ViewIcsegment1ModalComponent,
        IcSegmentFinderModalComponent,
        IcSegment2FinderModalComponent,
        IcItemsComponent,
        CreateOrEditIcItemModalComponent,
        IcSegment3FinderModalComponent,
        ViewPriceListComponent,
        ViewCostCenterComponent,
        ViewSubCostCenterComponent,
        GatePassComponent,
        CreateOrEditGetPassModalComponent,
        GatePassLookupTableModalComponent,
        CreateOrEditTransfersModalComponent,
        TransfersComponent,
        ViewTransfersComponent,
        ViewGatePassComponent,
        CreateOrEditAssemblyModalComponent,
        AssemblyComponent,
        ViewAssemblyComponent,
        Opt4LookupTableModalComponent,
        Opt5LookupTableModalComponent,
        ViewItemModalComponent,
        IcItemPrictureModalComponent,
        AssetRegistrationComponent,
        CreateOrEditAssetRegistrationModalComponent,
        ViewAssetRegistrationModalComponent,
        RegionsComponent,
        CreateOrEditRegionsModalComponent,
        InventoryReferencesComponent,
        CreateOreditIclotComponent,
        IclotComponent
    ],
    exports:[
        ItemPricingLookupTableModalComponent,
    ],
    imports: [
        AutoCompleteModule,
        PrimeNgFileUploadModule,
        PaginatorModule,
        EditorModule,
        InputMaskModule,
        TableModule,
        FormsModule,
        CommonModule,
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
        FileUploadModule,
        InventoryRoutingModule,
        FindersModule,
        InputTextModule,
        DialogModule,
        ButtonModule,
        ImageCropperModule,
        LightboxModule,
        CurrencyMaskModule,
        SalesModule
    ],
    providers: [
        {
            provide: BsDatepickerConfig,
            useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig
        },
        {
            provide: BsDaterangepickerConfig,
            useFactory:
                NgxBootstrapDatePickerConfigService.getDaterangepickerConfig
        },
        {
            provide: BsLocaleService,
            useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AbpHttpInterceptor,
            multi: true
        },
        ApiSegment1ServiceProxy.IcSegment1ServiceProxy,
        ApiSegment2ServiceProxy.IcSegment2ServiceProxy,
        ApiSegment3ServiceProxy.IcSegment3ServiceProxy,
        IcItemServiceProxy,
        ICOPT4ServiceProxy,
        ICOPT5ServiceProxy,
        IcUnitServiceProxy,
        AssetRegistrationsServiceProxy,
        AssetRegistrationDetailServiceProxy
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class InventoryModule {}
