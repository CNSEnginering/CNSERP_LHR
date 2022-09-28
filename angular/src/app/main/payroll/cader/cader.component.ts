import { Component, OnInit, ViewEncapsulation, ViewChild, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { CaderServiceProxy } from '../shared/services/Cader.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { caderDTo } from '../shared/dto/Cader-dto';
import { CreateOrEditCaderComponent } from './createoreditcader.component';

@Component({
  templateUrl: './cader.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class CaderComponent  extends AppComponentBase {

  @ViewChild('createOrEditCaderModal', { static: true }) createOrEditCaderModal: CreateOrEditCaderComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxDesignationIDFilter : number;
	maxDesignationIDFilterEmpty : number;
	minDesignationIDFilter : number;
	minDesignationIDFilterEmpty : number;
  designationFilter = '';
  activeFilter=-1;
  createdByFilter = '';
  maxCreateDateFilter : moment.Moment;
  minCreateDateFilter : moment.Moment;
  audtUserFilter = '';
  maxAudtDateFilter : moment.Moment;
  minAudtDateFilter : moment.Moment; 
  constructor(
    injector: Injector,
    private _caderService: CaderServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) { 
    super(injector);
  }
  getAllCader(event?: LazyLoadEvent) {
    debugger;
    if (this.primengTableHelper.shouldResetPaging(event)) {
        this.paginator.changePage(0);
        return;
    }
    this.primengTableHelper.showLoadingIndicator();

    this._caderService.getAll(
      this.filterText,
      this.maxDesignationIDFilter == null ? this.maxDesignationIDFilterEmpty : this.maxDesignationIDFilter,
      this.minDesignationIDFilter == null ? this.minDesignationIDFilterEmpty : this.minDesignationIDFilter,

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
  
  createCader(): void {
    this.createOrEditCaderModal.show(null);
  }
  
  deleteDesignation(designation: caderDTo): void {
    debugger;
    this.message.confirm(
        '',
        (isConfirmed) => {
            if (isConfirmed) {
                this._caderService.delete(designation.id)
                    .subscribe(() => {
                      debugger;
                        this.reloadPage();
                        this.notify.success(this.l('SuccessfullyDeleted'));
                    });
            }
        }
    );
   }
  
  
  
  
  }