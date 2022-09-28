import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AssetsComponent } from './assets.component';

const routes: Routes = [
  {
    path: '',
    component: AssetsComponent,
    children: [
      //{ path: '', component: , data: { permission: '' }  },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AssetsRoutingModule { }
