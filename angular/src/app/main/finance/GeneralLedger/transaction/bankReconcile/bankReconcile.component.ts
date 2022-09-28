import { Component, Injector, ViewEncapsulation, ViewChild, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { BankReconcileServiceProxy } from '@app/main/finance/shared/services/bkReconcile.service';
import { GLReconHeadersService } from '@app/main/finance/shared/services/glReconHeader.service';
import { ICCNSHeaderDto } from '@app/main/supplyChain/inventory/shared/dto/iccnsHeader-dto';
import { CreateOrEditBankReconcileModalComponent } from './create-or-edit-bankReconcile-modal.component';
import { finalize } from 'rxjs/operators';
import { ModalDirective } from 'ngx-bootstrap';
import { ViewBankReconcileModalComponent } from './view-bankReconcile-modal.component';
import { CreateOrEditGLReconHeaderDto, GLReconHeaderDto } from '@app/main/finance/shared/dto/glReconHeader-dto';


@Component({
  templateUrl: './bankReconcile.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class BankReconcileComponent extends AppComponentBase {

  @ViewChild('createOrEditBankReconcileModal', { static: true }) createOrEditBankReconcileModal: CreateOrEditBankReconcileModalComponent;
  @ViewChild('viewBankReconcileModal', { static: true }) viewBankReconcileModal: ViewBankReconcileModalComponent;
  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  advancedFiltersAreShown = false;
  filterText = '';
  docNoFilter: string;
  maxDocDateFilter: moment.Moment;
  minDocDateFilter: moment.Moment;
  bankIDFilter: string;
  bankNameFilter: string;
  maxID: number;

  input: CreateOrEditGLReconHeaderDto = new CreateOrEditGLReconHeaderDto();
  saving = false;
  active = false;





  constructor(
    injector: Injector,
    private _bankReconcileServiceProxy: BankReconcileServiceProxy,
    private _glReconHeadersServiceProxy: GLReconHeadersService,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) {
    super(injector);
  }

  getBankReconciles(event?: LazyLoadEvent) {
    debugger;
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();

    this._glReconHeadersServiceProxy.getAll(
      this.filterText,
      this.docNoFilter,
      this.maxDocDateFilter,
      this.minDocDateFilter,
      this.bankIDFilter,
      this.bankNameFilter,
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

  createBankReconcile(): void {
    debugger;
    this.createOrEditBankReconcileModal.show(false);
  }

  deleteBankReconcile(gLReconHeader: GLReconHeaderDto): void {
    debugger;
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._bankReconcileServiceProxy.delete(gLReconHeader.id)
            .subscribe(() => {
              debugger;
              this.reloadPage();
              this.notify.success(this.l('SuccessfullyDeleted'));
            });
        }
      }
    );
  }


  close(): void {
    this.active = false;
    this.modal.hide();
  }

}
