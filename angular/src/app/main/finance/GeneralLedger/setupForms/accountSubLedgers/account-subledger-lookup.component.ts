import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import {AccountSubLedgersServiceProxy, AccountSubLedgerChartofControlLookupTableDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';


@Component({
  selector: 'app-account-subledger-lookup',
  templateUrl: './account-subledger-lookup.component.html',
  encapsulation: ViewEncapsulation.None,
  styleUrls: ['./account-subledger-lookup.component.css']
})
export class AccountSubledgerLookupComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    filterText = '';
    id: string;
    displayName: string;
    
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;

    constructor(
        injector: Injector,
        private _accountSubLedgersServiceProxy: AccountSubLedgersServiceProxy
    ) {
        super(injector);
    }

    show( AccountID: string): void {
      debugger;
      this.active = true;
      this.paginator.rows = 5;
      this.filterText = AccountID;
      this.getAll();
      this.modal.show();
  }

  getAll( event?: LazyLoadEvent) {
    debugger;
      if (!this.active) {
          return;
      }

      if (this.primengTableHelper.shouldResetPaging(event)) {
          this.paginator.changePage(0);
          return;
      }

      this.primengTableHelper.showLoadingIndicator();

      this._accountSubLedgersServiceProxy.getAllAccountSubledger_lookup(
          this.filterText,
          this.primengTableHelper.getSorting(this.dataTable),
          this.primengTableHelper.getSkipCount(this.paginator, event),
          this.primengTableHelper.getMaxResultCount(this.paginator, event)
      ).subscribe(result => {
          this.primengTableHelper.totalRecordsCount = result.totalCount;
          this.primengTableHelper.records = result.items;
          this.primengTableHelper.hideLoadingIndicator();
      });
  }

  reloadPage(): void {
      this.paginator.changePage(this.paginator.getPage());
  }

  setAndSave(LinkedAccount: AccountSubLedgerChartofControlLookupTableDto) {
      this.id = LinkedAccount.id;
      this.displayName = LinkedAccount.displayName;
      this.active = false;
      this.modal.hide();
      this.modalSave.emit(null);
  }

  close(): void {
      this.active = false;
      this.modal.hide();
      this.modalSave.emit(null);
  }

}
