import { Component, OnInit, ViewEncapsulation, ViewChild, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LCExpensesServiceProxy } from '@app/main/finance/shared/services/lcExpenses.service';
import { LCExpensesDto } from '@app/main/finance/shared/dto/lcExpenses-dto';
import { ViewLCExpenseModalComponent } from './view-lcExpense-modal.component';
import { CreateOrEditLCExpenseModalComponent } from './create-or-edit-lcExpense-modal.component';

@Component({
  templateUrl: './lcExpenses.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class LCExpenseComponent extends AppComponentBase {
  @ViewChild('createOrEditLCExpenseModal', { static: true }) createOrEditLCExpenseModal: CreateOrEditLCExpenseModalComponent;
  @ViewChild('viewLCExpenseModal', { static: true }) viewLCExpenseModal: ViewLCExpenseModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxExpIDFilter : number;
	maxExpIDFilterEmpty : number;
	minExpIDFilter : number;
	minExpIDFilterEmpty : number;
  activeFilter=-1;
  createdByFilter = '';
  maxCreateDateFilter : moment.Moment;
  minCreateDateFilter : moment.Moment;
  auditUserFilter = '';
  maxAuditDateFilter : moment.Moment;
  minAuditDateFilter : moment.Moment;  

  constructor(
    injector: Injector,
    private _lcExpenseService: LCExpensesServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) { 
    super(injector); 
  }

  getLCExpense(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
        this.paginator.changePage(0);
        return;
    }
debugger;
    this.primengTableHelper.showLoadingIndicator();

    this._lcExpenseService.getAll(
      this.filterText,
      this.maxExpIDFilter == null ? this.maxExpIDFilterEmpty : this.maxExpIDFilter,
      this.minExpIDFilter == null ? this.minExpIDFilterEmpty : this.minExpIDFilter,
      this.activeFilter,
      this.auditUserFilter,
      this.maxAuditDateFilter,
      this.minAuditDateFilter,
      this.createdByFilter,
      this.maxCreateDateFilter,
      this.minCreateDateFilter,
      this.primengTableHelper.getSorting(this.dataTable),
      this.primengTableHelper.getSkipCount(this.paginator, event),
      this.primengTableHelper.getMaxResultCount(this.paginator, event)
    ).subscribe(result => {
       debugger;
        this.primengTableHelper.totalRecordsCount = result.totalCount;
        this.primengTableHelper.records = result.items;
        this.primengTableHelper.hideLoadingIndicator();
    });
}

reloadPage(): void {
  this.paginator.changePage(this.paginator.getPage());
}

createLCExpense(): void {
  this.createOrEditLCExpenseModal.show();
}

deleteLCExpense(LCExpense: LCExpensesDto): void {
  debugger;
  this.message.confirm(
      '',
      (isConfirmed) => {
          if (isConfirmed) {
              this._lcExpenseService.delete(LCExpense.id)
                  .subscribe(() => {
                    debugger;
                      this.reloadPage();
                      this.notify.success(this.l('SuccessfullyDeleted'));
                  });
          }
      }
  );
}

exportToExcel(): void {
    this._lcExpenseService.GetLCExpensesToExcel(
      this.filterText,
      this.maxExpIDFilter == null ? this.maxExpIDFilterEmpty : this.maxExpIDFilter,
      this.minExpIDFilter == null ? this.minExpIDFilterEmpty : this.minExpIDFilter,
      this.activeFilter,
      this.auditUserFilter,
      this.maxAuditDateFilter,
      this.minAuditDateFilter,
      this.createdByFilter,
      this.maxCreateDateFilter,
      this.minCreateDateFilter,
    )
  .subscribe(result => {
      this._fileDownloadService.downloadTempFile(result);
   });
}

}
