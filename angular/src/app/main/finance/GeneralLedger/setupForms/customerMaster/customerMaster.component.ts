import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AccountSubLedgersComponent } from '../accountSubLedgers/accountSubLedgers.component';
import { CreateOrEditAccountSubLedgerModalComponent } from '../accountSubLedgers/create-or-edit-accountSubLedger-modal.component';

@Component({
  templateUrl: './customerMaster.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class CustomerMasterComponent implements OnInit {
  @ViewChild('accountSubLedgersModal', { static: true }) accountSubLedgersModal: AccountSubLedgersComponent;
  mode: string = "customerMaster";
  constructor() { }

  ngOnInit() {
    debugger;
    this.accountSubLedgersModal.getCreate("Create New Customer Master","Customer Master","Manage Account Customer Master");
    console.log("")
  }

}
