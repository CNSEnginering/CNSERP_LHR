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
import { ViewGLTransferModalComponent } from './view-glTransfer-modal.component';
import { GLTransferServiceProxy } from '@app/main/finance/shared/services/glTransfer.service';
import { GLTransferDto } from '@app/main/finance/shared/dto/glTransfer-dto';
import { CreateOrEditGLTransferModalComponent } from './create-or-edit-glTransfer-modal.component';

@Component({
  templateUrl: './glTransfer.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class GLTransferComponent extends AppComponentBase {
  @ViewChild('createOrEditGLTransferModal', { static: true }) createOrEditGLTransferModal: CreateOrEditGLTransferModalComponent;
  @ViewChild('viewGLTransferModal', { static: true }) viewGLTransferModal: ViewGLTransferModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxDocIDFilter : number;
	maxDocIDFilterEmpty : number;
	minDocIDFilter : number;
  minDocIDFilterEmpty : number;
  maxDocDateFilter : moment.Moment;
  minDocDateFilter : moment.Moment;
  createdByFilter = '';
  maxCreateDateFilter : moment.Moment;
  minCreateDateFilter : moment.Moment;
  audtUserFilter = '';
  maxAudtDateFilter : moment.Moment;
  minAudtDateFilter : moment.Moment;  

  constructor(
    injector: Injector,
    private _glTransferService: GLTransferServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) { 
    super(injector); 
  }

  getGLTransfer(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
        this.paginator.changePage(0);
        return;
    }
debugger;
    this.primengTableHelper.showLoadingIndicator();

    this._glTransferService.getAll(
      this.filterText,
      this.maxDocIDFilter == null ? this.maxDocIDFilterEmpty : this.maxDocIDFilter,
      this.minDocIDFilter == null ? this.minDocIDFilterEmpty : this.minDocIDFilter,
      this.maxDocDateFilter,
      this.minDocDateFilter,
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

createGLTransfer(): void {
  this.createOrEditGLTransferModal.show();
}

deleteGLTransfer(glTransfer: GLTransferDto): void {
  debugger;
  this.message.confirm(
      '',
      (isConfirmed) => {
          if (isConfirmed) {
              this._glTransferService.delete(glTransfer.id)
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
    this._glTransferService.GetGLTransferToExcel(
      this.filterText,
      this.maxDocIDFilter == null ? this.maxDocIDFilterEmpty : this.maxDocIDFilter,
      this.minDocIDFilter == null ? this.minDocIDFilterEmpty : this.minDocIDFilter,
      this.maxDocDateFilter,
      this.minDocDateFilter,
      this.audtUserFilter,
      this.maxAudtDateFilter,
      this.minAudtDateFilter,
      this.createdByFilter,
      this.maxCreateDateFilter,
      this.minCreateDateFilter,
    )
  .subscribe(result => {
      this._fileDownloadService.downloadTempFile(result);
   });
}

}
