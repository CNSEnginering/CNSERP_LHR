import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { InventoryComponent } from "./inventory.component";
import { ICSetupsComponent } from "./ic-setup/ic-setup.component";
import { InventoryGlLinkcomponent } from "./InventoryGlLink/InventoryGlLink.component";
import { PriceListComponent } from "./priceList/priceList.component";
import { ICUOMsComponent } from "./icuoMs/icuoMs.component";
import { ICLocationsComponent } from "./icLocations/icLocations.component";
import { ReorderLevelsComponent } from "./reorderLevels/reorderLevels.component";
import { TransactionTypesComponent } from "./transactionTypes/transactionTypes.component";
import { ItemPricingComponent } from "./itemPricing/itemPricing.component";
import { CostCenterComponent } from "./costCenter/costCenter.component";
import { SubCostCenterComponent } from "./subCostCenter/subCostCenter.component";
import { IcSegment3Component } from "./setupForms/ic-segment3/ic-segment3.component";
import { IcSegment1Component } from "./setupForms/ic-segment1/ic-segment1.component";
import { IcSegment2Component } from "./setupForms/ic-segment2/ic-segment2.component";
import { IcItemsComponent } from "./setupForms/ic-items/ic-items.component";
import { GatePassComponent } from "./gatePass/gatePass.component";
import { TransfersComponent } from "./transfers/transfers.component";
import { AdjustmentsComponent } from "./adjustments/adjustments.component";
import { ICOPT4Component } from "./setupForms/ic-OPT4/icopT4.component";
import { ICOPT5Component } from "./setupForms/ic-OPT5/icopT5.component";
import { ConsumptionsComponent } from "./consumption/consumptions.component";
import { OpeningsComponent } from "./opening/openings.component";
import { AssemblyComponent } from "./assembly/assembly.component";
import { AssetRegistrationComponent } from "./assetRegistration/assetRegistration.component";
import { WorkOrderComponent } from "./workOrder/workOrder.component";
import { RegionsComponent } from "./regions/regions.component";
import { InventoryReferencesComponent } from "./ReferenceInventory/InventoryReferences.component";
import { IclotComponent } from './iclot/iclot.component';
const routes: Routes = [
    {
        path: "",
        component: InventoryComponent,
        children: [
            {
                path: "inventory/setupForms/ic-OPT4",
                component: ICOPT4Component,
                data: { permission: "Inventory.ICOPT4" }
            },
            {
                path: "inventory/setupForms/ic-OPT5",
                component: ICOPT5Component,
                data: { permission: "Inventory.ICOPT5" }
            },
            {
                path: "inventory/reorderLevels",
                component: ReorderLevelsComponent,
                data: { permission: "Inventory.ReorderLevels" }
            },
            {
                path: "inventory/transactionTypes",
                component: TransactionTypesComponent,
                data: { permission: "Inventory.TransactionTypes" }
            },
            {
                path: "inventory/setupForms/ic-items",
                component: IcItemsComponent,
                data: { permission: "" }
            },
            {
                path: "inventory/setupForms/ic-segment1",
                component: IcSegment1Component,
                data: { permission: "Inventory.ICSegment1" }
            },
            {
                path: "inventory/setupForms/ic-segment2",
                component: IcSegment2Component,
                data: { permission: "Inventory.ICSegment2" }
            },
            {
                path: "inventory/setupForms/ic-segment3",
                component: IcSegment3Component,
                data: { permission: "Inventory.ICSegment3" }
            },
            {
                path: "inventory/icuoMs",
                component: ICUOMsComponent,
                data: { permission: "Inventory.ICUOMs" }
            },
            {
                path: "inventory/icLocations",
                component: ICLocationsComponent,
                data: { permission: "Inventory.ICLocations" }
            },
            {
                path: "inventory/inventoryGlLink",
                component: InventoryGlLinkcomponent,
                data: { permission: "Inventory.InventoryGlLinks" }
            },
            {
                path: "inventory/ic-setup",
                component: ICSetupsComponent,
                data: { permission: "Inventory.ICSetups" }
            },
            {
                path: "inventory/iclot",
                component: IclotComponent,
                data: { permission: "Pages.ICLOT" }
            },
            
            {
                path: "inventory/priceList",
                component: PriceListComponent,
                data: { permission: "Inventory.PriceLists" }
            },
            {
                path: "inventory/itemPricing",
                component: ItemPricingComponent,
                data: { permission: "Inventory.ItemPricings" }
            },
            {
                path: "inventory/costCenter",
                component: CostCenterComponent,
                data: { permission: "Inventory.CostCenters" }
            },
            {
                path: "inventory/subCostCenter",
                component: SubCostCenterComponent,
                data: { permission: "Inventory.SubCostCenters" }
            },
            {
                path: "inventory/gatePass",
                component: GatePassComponent,
                data: { permission: "Inventory.GatePasses" }
            },
            {
                path: "inventory/adjustments",
                component: AdjustmentsComponent,
                data: { permission: "Inventory.Adjustments" }
            },
            {
                path: "inventory/workOrder",
                component: WorkOrderComponent,
                data: { permission: "Inventory.WorkOrder" }
            },
            {
                path: "inventory/consumption",
                component: ConsumptionsComponent,
                data: { permission: "Inventory.Consumptions" }
            },
            {
                path: "inventory/opening",
                component: OpeningsComponent,
                data: { permission: "Inventory.Openings" }
            },
            {
                path: "inventory/transfers",
                component: TransfersComponent,
                data: { permission: "Inventory.Transfers" }
            },
            {
                path: "inventory/assembly",
                component: AssemblyComponent,
                data: { permission: "Inventory.Assemblies" }
            },
            {
                path: "inventory/assetRegistration",
                component: AssetRegistrationComponent,
                data: { permission: "Pages.AssetRegistration" }
            },
            {
                path: "inventory/regions",
                component: RegionsComponent,
                data: { permission: "Inventory.ICELocation" }
            },
            {
                path: "inventory/ReferenceInventory",
                component: InventoryReferencesComponent,
                data: { permission: "Pages.AssetRegistration" }
            },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class InventoryRoutingModule { }
