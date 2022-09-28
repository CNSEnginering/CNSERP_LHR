import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ManufacturingComponent } from "./manufacturing.component";
import { ManufacAccComponent } from "./setup/accountSetup/manufacAcc.component";
import { MftoolComponent } from "./setup/mftool/mftool.component";
import { MftooltyComponent } from "./setup/mftoolty/mftoolty.component";
import { ProductionAreaComponent } from "./setup/productionArea/ProductionArea.component";
import { ResourceMasterComponent } from "./setup/resource-master/resource.component";
import { MfWorkingCenterComponent } from './setup/mf-working-center/mf-working-center.component';
const routes: Routes = [
    {
        path: '',
        component: ManufacturingComponent,
        children: [
            { path: 'setup/accountSetup', component: ManufacAccComponent, data: { permission: 'Pages.MFACSET' } },
            { path: 'setup/productionArea', component: ProductionAreaComponent, data: { permission: 'Pages.MFAREA' } },
            { path: 'setup/resource-master', component: ResourceMasterComponent, data: { permission: 'Pages.MFRESMAS' } },
            { path: 'setup/mftoolty', component: MftooltyComponent, data: { permission: 'Pages.MFTOOLTY' } },
            { path: 'setup/mftool', component: MftoolComponent, data: { permission: 'Pages.MFTOOL' } },
            { path: 'setup/mf-working-center', component: MfWorkingCenterComponent, data: { permission: 'Pages.MFWCM' } },
        ]
    }
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ManufacturingRoutingModule { }