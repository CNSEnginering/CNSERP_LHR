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
import { CreateOrEditAttendanceHeaderDto, AttendanceHeaderDto } from '../shared/dto/attendanceHeader-dto';
import { AttendanceHeaderService } from '../shared/services/attendanceHeader.service';
import { CreateOrEditAttendanceModalComponent } from './create-or-edit-attendance-modal.component';
import { ViewAttendanceModalComponent } from './view-attendance-modal.component';



@Component({
  templateUrl: './attendance.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class AttendanceComponent extends AppComponentBase {

  @ViewChild('createOrEditAttendanceModal', { static: true }) createOrEditAttendanceModal: CreateOrEditAttendanceModalComponent;
  @ViewChild('viewAttendanceModal', { static: true }) viewAttendanceModal: ViewAttendanceModalComponent;
  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  advancedFiltersAreShown = false;
  filterText = '';
  maxDocDateFilter: moment.Moment;
  minDocDateFilter: moment.Moment;
  createdByFilter = '';
  maxCreateDateFilter: moment.Moment;
  minCreateDateFilter: moment.Moment;


  input: CreateOrEditAttendanceHeaderDto = new CreateOrEditAttendanceHeaderDto();
  saving = false;
  active = false;




  constructor(
    injector: Injector,
    private _attendanceHeaderServiceProxy: AttendanceHeaderService,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) {
    super(injector);
  }

  getAttendanceHeader(event?: LazyLoadEvent) {
    debugger;
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();

    this._attendanceHeaderServiceProxy.getAll(
      this.filterText,
      this.maxDocDateFilter,
      this.minDocDateFilter,
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

  createAttendanceHeader(): void {
    debugger;
    this.createOrEditAttendanceModal.show(false);
  }

  deleteAttendanceHeader(attendanceHeader: AttendanceHeaderDto): void {
    debugger;
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._attendanceHeaderServiceProxy.delete(attendanceHeader.id)
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
