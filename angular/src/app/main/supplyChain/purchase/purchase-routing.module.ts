import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PurchaseComponent } from './purchase.component';
import { PurchaseOrdersComponent } from './purchaseOrder/purchaseOrders.component';
import { RequisitionComponent } from './Requisition/requisition.component';
import { ReceiptEntryComponent } from './receiptEntry/receiptEntry.component';
import { ReceiptReturnComponent } from './receiptReturn/receiptReturn.component';
import { ApinvhComponent } from './apinvh/apinvh.component';

const routes: Routes = [
    {
        path: '',
        component: PurchaseComponent,
        children: [
            { path: 'purchase/purchaseOrder', component: PurchaseOrdersComponent, data: { permission: 'Purchase.PurchaseOrders' }  },
            { path: 'purchase/receiptEntry', component: ReceiptEntryComponent, data: { permission: 'Purchase.ReceiptEntry' }  },
            { path: 'purchase/receiptReturn', component: ReceiptReturnComponent, data: { permission: 'Purchase.ReceiptReturn' }  },
            { path: 'purchase/requisition', component: RequisitionComponent, data: { permission: 'Purchase.Requisitions' }  },
            { path: 'purchase/apinvh', component: ApinvhComponent, data: { permission: 'Purchase.APINVH' }  },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
export class PurchaseRoutingModule { }