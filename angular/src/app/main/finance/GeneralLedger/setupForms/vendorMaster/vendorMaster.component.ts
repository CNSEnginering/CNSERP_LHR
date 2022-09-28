import { Component, OnInit,ViewEncapsulation, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AccountSubLedgersComponent } from '../accountSubLedgers/accountSubLedgers.component';

@Component({
    templateUrl: './vendorMaster.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class VendorMasterComponent implements OnInit {
  @ViewChild('accountSubLedgersModal', { static: true }) accountSubLedgersModal: AccountSubLedgersComponent;
  mode: string = "vendorMaster";
  constructor() {
   }

  ngOnInit() {
    debugger;
    this.accountSubLedgersModal.getCreate("Create New Vendor Master","Vendor Master","Manage Account Vendor Master");
  }

}
