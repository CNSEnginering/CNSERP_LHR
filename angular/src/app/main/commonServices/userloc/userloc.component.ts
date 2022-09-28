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
import { CreateOrEditGLSecurityHeaderDto, GLSecurityHeaderDto } from '@app/main/finance/shared/dto/glSecurityHeader-dto';
import { userLocService } from '@app/main/commonServices/shared/services/userLoc.service';
 import { CreateOrEditUserlocComponent } from './create-or-edit-userloc.component';

@Component({
  templateUrl: './userloc.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class UserlocComponent extends AppComponentBase  {

 
   @ViewChild('createOrEdituserlocModal', { static: true }) createOrEdituserlocModal: CreateOrEditUserlocComponent;
  // @ViewChild('viewGLSecurityModal', { static: true }) viewGLSecurityModal: ViewGLSecurityModalComponent;
   @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  advancedFiltersAreShown = false;
  filterText = '';
  userIDFilter = '';
  userNameFilter = '';
  audtUserFilter = '';
  maxAudtDateFilter: moment.Moment;
  minAudtDateFilter: moment.Moment;
  createdByFilter = '';
  maxCreateDateFilter: moment.Moment;
  minCreateDateFilter: moment.Moment;


  input: CreateOrEditGLSecurityHeaderDto = new CreateOrEditGLSecurityHeaderDto();
  saving = false;
  active = false;




  constructor(
    injector: Injector,
    private _userLocServiceProxy: userLocService,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) {
    super(injector);
  }

  getGLSecurityHeader(event?: LazyLoadEvent) {
    debugger;
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();

    this._userLocServiceProxy.getAll(
      this.filterText,
      this.primengTableHelper.getSorting(this.dataTable),
      this.primengTableHelper.getSkipCount(this.paginator, event),
      this.primengTableHelper.getMaxResultCount(this.paginator, event)
    ).subscribe(res => {
      debugger;
      console.log(res);
      this.primengTableHelper.totalRecordsCount = res["result"]["totalCount"];
      this.primengTableHelper.records = res["result"]["items"];
      this.primengTableHelper.hideLoadingIndicator();
    });
  }

  reloadPage(): void {
    this.paginator.changePage(this.paginator.getPage());
  }

  createGLSecurityHeader(): void {
    debugger;
    this.createOrEdituserlocModal.show(false);
  }

  deleteGLSecurityHeader(gLSecurityHeader: GLSecurityHeaderDto): void {
    debugger;
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._userLocServiceProxy.delete(gLSecurityHeader.id)
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
