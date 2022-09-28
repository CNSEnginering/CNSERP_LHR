import { Component, OnInit, Injector, ViewChild, ViewEncapsulation, Output, EventEmitter, Input, AfterViewInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { SaleAccountsService } from '../shared/services/saleAccounts.service';
import { CreateOrEditCreditDebitNoteModalComponent } from './create-or-edit-creditDebitNote-modal.component';
import { Table } from 'primeng/table';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import { CreditDebitNoteService } from '../shared/services/creditDebitNote.service';
import { ViewCreditDebitNoteComponent } from './view-creditDebitNote-modal.component';



@Component({
  templateUrl: './debitNote.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class DebitNoteComponent extends AppComponentBase implements OnInit,AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('CreditDebitNoteModal', { static: true }) creditDebitNoteModal: CreateOrEditCreditDebitNoteModalComponent;
  @ViewChild('viewCreditDebitNoteModal', { static: true }) viewCreditDebitNoteModal: ViewCreditDebitNoteComponent;
  advancedFiltersAreShown = false;
  filterText = '';
  creditDebitNote: any;
  sorting: any;
  skipCount: any;
  MaxResultCount: any;

  constructor(
    injector: Injector,
    private _fileDownloadService: FileDownloadService,
    private _creditDebitNoteService: CreditDebitNoteService
  ) {
    super(injector);
  }

  ngOnInit() {
    // this.getAll(null);
  }
  reloadPage(): void {
    this.getAll();
  }
  ngAfterViewInit(): void {

  }
  getAll(event?: LazyLoadEvent) {
    this.sorting = this.primengTableHelper.getSorting(this.dataTable);
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event);
    this.MaxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event);

    this.primengTableHelper.showLoadingIndicator();
    this._creditDebitNoteService.getAll(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount,"2"
    ).subscribe(data => {
      this.creditDebitNote = data["result"]["items"]
      this.primengTableHelper.totalRecordsCount = this.creditDebitNote.length;
      this.primengTableHelper.records = data["result"]["items"];
      this.primengTableHelper.hideLoadingIndicator();
    });
  }
  delete(id: number) {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._creditDebitNoteService.delete(id).subscribe(() => {
            this.reloadPage();
            this.notify.success(this.l('SuccessfullyDeleted'));
          });
        }
      }
    );
  }
  createOrEdit(id: number) {
    this.creditDebitNoteModal.show(id,"DebitNote");
  }
  exportToExcel() {
    this._creditDebitNoteService.GetDataToExcel(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe((result: any) => {
      this._fileDownloadService.downloadTempFile(result["result"]);
    });
  }
  view(data: any) {
     this.viewCreditDebitNoteModal.show(data);
   }
}

