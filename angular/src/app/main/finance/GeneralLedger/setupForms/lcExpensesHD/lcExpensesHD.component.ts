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
import { ModalDirective } from 'ngx-bootstrap';
import { ViewLCExpensesHDModalComponent } from './view-lcExpensesHD-modal.component';
import { CreateOrEditLCExpensesHDModalComponent } from './create-or-edit-lcExpensesHD-modal.component';
import { CreateOrEditLCExpensesHeaderDto, LCExpensesHeaderDto } from '@app/main/finance/shared/dto/lcExpensesHeader-dto';
import { LCExpensesHeaderService } from '@app/main/finance/shared/services/lcExpensesHeader.service';


@Component({
  templateUrl: './lcExpensesHD.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class LCExpensesHDComponent extends AppComponentBase {

  @ViewChild('createOrEditLCExpensesHDModal', { static: true }) createOrEditLCExpensesHDModal: CreateOrEditLCExpensesHDModalComponent;
  @ViewChild('viewLCExpensesHDModal', { static: true }) viewLCExpensesHDModal: ViewLCExpensesHDModalComponent;
  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  advancedFiltersAreShown = false;
  filterText = '';
  maxLocIDFilter : number;
	maxLocIDFilterEmpty : number;
	minLocIDFilter : number;
  minLocIDFilterEmpty : number;
  maxDocNoFilter : number;
	maxDocNoFilterEmpty : number;
	minDocNoFilter : number;
	minDocNoFilterEmpty : number;
  maxDocDateFilter: moment.Moment;
  minDocDateFilter: moment.Moment;
  typeIDFilter = '';
  accountIDFilter = '';
  maxSubAccIDFilter : number;
	maxSubAccIDFilterEmpty : number;
	minSubAccIDFilter : number;
	minSubAccIDFilterEmpty : number;
  payableAccIDFilter = '';
  lcNumberFilter = '';
  activeFilter=-1;
  audtUserFilter = '';
  maxAudtDateFilter: moment.Moment;
  minAudtDateFilter: moment.Moment;
  createdByFilter = '';
  maxCreateDateFilter: moment.Moment;
  minCreateDateFilter: moment.Moment;


  input: CreateOrEditLCExpensesHeaderDto = new CreateOrEditLCExpensesHeaderDto();
  saving = false;
  active = false;




  constructor(
    injector: Injector,
    private _lcExpensesHDHeaderServiceProxy: LCExpensesHeaderService,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) {
    super(injector);
  }

  getLCExpensesHeader(event?: LazyLoadEvent) {
    debugger;
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();

    this._lcExpensesHDHeaderServiceProxy.getAll(
      this.filterText,
      this.maxLocIDFilter == null ? this.maxLocIDFilterEmpty : this.maxLocIDFilter,
	    this.minLocIDFilter == null ? this.minLocIDFilterEmpty : this.minLocIDFilter,
	    this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty : this.maxDocNoFilter,
	    this.minDocNoFilter == null ? this.minDocNoFilterEmpty : this.minDocNoFilter,
      this.maxDocDateFilter,
      this.minDocDateFilter,
      this.typeIDFilter,
      this.accountIDFilter,
	    this.maxSubAccIDFilter == null ? this.maxSubAccIDFilterEmpty : this.maxSubAccIDFilter,
      this.minSubAccIDFilter == null ? this.minSubAccIDFilterEmpty : this.minSubAccIDFilter,
      this.payableAccIDFilter,
      this.lcNumberFilter,
      this.activeFilter,
      this.audtUserFilter,
      this.maxAudtDateFilter,
      this.minAudtDateFilter,
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

  createLCExpensesHDHeader(): void {
    debugger;
    this.createOrEditLCExpensesHDModal.show(false);
  }

  deleteLCExpensesHDHeader(LCExpensesHDHeader: LCExpensesHeaderDto): void {
    debugger;
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._lcExpensesHDHeaderServiceProxy.delete(LCExpensesHDHeader.id)
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
    debugger;
    this._lcExpensesHDHeaderServiceProxy.getLCExpensesHeaderToExcel(
      this.filterText,
      this.maxLocIDFilter == null ? this.maxLocIDFilterEmpty : this.maxLocIDFilter,
	    this.minLocIDFilter == null ? this.minLocIDFilterEmpty : this.minLocIDFilter,
	    this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty : this.maxDocNoFilter,
	    this.minDocNoFilter == null ? this.minDocNoFilterEmpty : this.minDocNoFilter,
      this.maxDocDateFilter,
      this.minDocDateFilter,
      this.typeIDFilter,
      this.accountIDFilter,
	    this.maxSubAccIDFilter == null ? this.maxSubAccIDFilterEmpty : this.maxSubAccIDFilter,
      this.minSubAccIDFilter == null ? this.minSubAccIDFilterEmpty : this.minSubAccIDFilter,
      this.payableAccIDFilter,
      this.lcNumberFilter,
      this.activeFilter,
      this.audtUserFilter,
      this.maxAudtDateFilter,
      this.minAudtDateFilter,
      this.createdByFilter,
      this.maxCreateDateFilter,
      this.minCreateDateFilter
    )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
        });
}

  

  close(): void {
    this.active = false;
    this.modal.hide();
  }

}
