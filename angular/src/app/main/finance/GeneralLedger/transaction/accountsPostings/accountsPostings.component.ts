import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountsPostingsServiceProxy, AccountsPostingDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditAccountsPostingModalComponent } from './create-or-edit-accountsPosting-modal.component';
import { ViewAccountsPostingModalComponent } from './view-accountsPosting-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
  templateUrl: './accountsPostings.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class AccountsPostingsComponent extends AppComponentBase {

  @ViewChild('createOrEditAccountsPostingModal', { static: true }) createOrEditAccountsPostingModal: CreateOrEditAccountsPostingModalComponent;
  @ViewChild('viewAccountsPostingModalComponent', { static: true }) viewAccountsPostingModal: ViewAccountsPostingModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxDetIDFilter: number;
  maxDetIDFilterEmpty: number;
  minDetIDFilter: number;
  minDetIDFilterEmpty: number;
  bookIDFilter = '';
  maxConfigIDFilter: number;
  maxConfigIDFilterEmpty: number;
  minConfigIDFilter: number;
  minConfigIDFilterEmpty: number;
  maxDocNoFilter: number;
  maxDocNoFilterEmpty: number;
  minDocNoFilter: number;
  minDocNoFilterEmpty: number;
  maxDocMonthFilter: number;
  maxDocMonthFilterEmpty: number;
  minDocMonthFilter: number;
  minDocMonthFilterEmpty: number;
  maxDocDateFilter: moment.Moment;
  minDocDateFilter: moment.Moment;
  auditUserFilter = '';
  maxAuditTimeFilter: moment.Moment;
  minAuditTimeFilter: moment.Moment;
  postedFilter = -1;
  bookNameFilter = '';
  accountIDFilter = '';
  maxSubAccIDFilter: number;
  maxSubAccIDFilterEmpty: number;
  minSubAccIDFilter: number;
  minSubAccIDFilterEmpty: number;
  narrationFilter = '';
  maxAmountFilter: number;
  maxAmountFilterEmpty: number;
  minAmountFilter: number;
  minAmountFilterEmpty: number;
  accountNameFilter = '';
  subAccNameFilter = '';
  maxDetailIDFilter: number;
  maxDetailIDFilterEmpty: number;
  minDetailIDFilter: number;
  minDetailIDFilterEmpty: number;
  chequeNoFilter = '';
  regNoFilter = '';
  referenceFilter = '';




  constructor(
    injector: Injector,
    private _accountsPostingsServiceProxy: AccountsPostingsServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) {
    super(injector);
  }

  getAccountsPostings(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();

    this._accountsPostingsServiceProxy.getAll(
      this.filterText,
      this.maxDetIDFilter == null ? this.maxDetIDFilterEmpty : this.maxDetIDFilter,
      this.minDetIDFilter == null ? this.minDetIDFilterEmpty : this.minDetIDFilter,
      this.bookIDFilter,
      this.maxConfigIDFilter == null ? this.maxConfigIDFilterEmpty : this.maxConfigIDFilter,
      this.minConfigIDFilter == null ? this.minConfigIDFilterEmpty : this.minConfigIDFilter,
      this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty : this.maxDocNoFilter,
      this.minDocNoFilter == null ? this.minDocNoFilterEmpty : this.minDocNoFilter,
      this.maxDocMonthFilter == null ? this.maxDocMonthFilterEmpty : this.maxDocMonthFilter,
      this.minDocMonthFilter == null ? this.minDocMonthFilterEmpty : this.minDocMonthFilter,
      this.maxDocDateFilter,
      this.minDocDateFilter,
      this.auditUserFilter,
      this.maxAuditTimeFilter,
      this.minAuditTimeFilter,
      this.postedFilter,
      this.bookNameFilter,
      this.accountIDFilter,
      this.maxSubAccIDFilter == null ? this.maxSubAccIDFilterEmpty : this.maxSubAccIDFilter,
      this.minSubAccIDFilter == null ? this.minSubAccIDFilterEmpty : this.minSubAccIDFilter,
      this.narrationFilter,
      this.maxAmountFilter == null ? this.maxAmountFilterEmpty : this.maxAmountFilter,
      this.minAmountFilter == null ? this.minAmountFilterEmpty : this.minAmountFilter,
      this.accountNameFilter,
      this.subAccNameFilter,
      this.maxDetailIDFilter == null ? this.maxDetailIDFilterEmpty : this.maxDetailIDFilter,
      this.minDetailIDFilter == null ? this.minDetailIDFilterEmpty : this.minDetailIDFilter,
      this.chequeNoFilter,
      this.regNoFilter,
      this.referenceFilter,
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

  createAccountsPosting(): void {
    this.createOrEditAccountsPostingModal.show("AccountsPosting");
  }

  createAccountsApproval(): void  {
    this.createOrEditAccountsPostingModal.show("AccountsApproval");
  }

  createAccountsUnApproval(): void  {
    this.createOrEditAccountsPostingModal.show("AccountsUnApproval");
  }

  deleteAccountsPosting(accountsPosting: AccountsPostingDto): void {
    this.message.confirm(
      '',
      (isConfirmed) => {
        if (isConfirmed) {
          this._accountsPostingsServiceProxy.delete(accountsPosting.id)
            .subscribe(() => {
              this.reloadPage();
              this.notify.success(this.l('SuccessfullyDeleted'));
            });
        }
      }
    );
  }
}
