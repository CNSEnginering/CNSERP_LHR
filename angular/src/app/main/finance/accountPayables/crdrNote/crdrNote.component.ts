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
import { ViewCRDRNoteModalComponent } from './view-CRDRNote-modal.component';
import { CreateOrEditCRDRNoteModalComponent } from './create-or-edit-CRDRNote-modal.component';
import { CRDRNoteServiceProxy } from '../../shared/services/crdrNote.service';
import { CRDRNoteDto } from '../../shared/dto/crdrNote-dto';

@Component({
  selector:"CRDRNoteComponent",
  templateUrl: './crdrNote.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class CRDRNoteComponent extends AppComponentBase {
  @ViewChild('createOrEditCRDRNoteModal', { static: true }) createOrEditCRDRNoteModal: CreateOrEditCRDRNoteModalComponent;
  @ViewChild('viewCRDRNoteModal', { static: true }) viewCRDRNoteModal: ViewCRDRNoteModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

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
  maxDocDateFilter : moment.Moment;
  minDocDateFilter : moment.Moment;
  typeIDFilter:number =0;
  maxID:number;
  expDescFilter = '';
  activeFilter=-1;
  createdByFilter = '';
  maxCreateDateFilter : moment.Moment;
  minCreateDateFilter : moment.Moment;
  audtUserFilter = '';
  maxAudtDateFilter : moment.Moment;
  minAudtDateFilter : moment.Moment;  
  typeName: string='AP';

  constructor(
    injector: Injector,
    private _crdrNoteService: CRDRNoteServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) { 
    super(injector); 
  }

  setTypeID(typeID?:number)
  {
    debugger;
    this.typeIDFilter=typeID;
    this.typeName=this.typeIDFilter==0 ? 'AP':'AR';
  }

  getCRDRNote(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
        this.paginator.changePage(0);
        return;
    }
debugger;
    this.primengTableHelper.showLoadingIndicator();

    this._crdrNoteService.getAll(
      this.filterText,
      this.maxLocIDFilter == null ? this.maxLocIDFilterEmpty : this.maxLocIDFilter,
      this.minLocIDFilter == null ? this.minLocIDFilterEmpty : this.minLocIDFilter,
      this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty : this.maxDocNoFilter,
      this.minDocNoFilter == null ? this.minDocNoFilterEmpty : this.minDocNoFilter,
      this.maxDocDateFilter,
      this.minDocDateFilter,
      this.typeIDFilter,
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

createCRDRNote(): void {
  this._crdrNoteService.getMaxCRDRNoteId(this.typeIDFilter).subscribe(result => {
      this.maxID = result;
      this.createOrEditCRDRNoteModal.show(null, this.maxID,this.typeIDFilter, this.typeName);
  });
}

deleteCRDRNote(CRDRNote: CRDRNoteDto): void {
  debugger;
  this.message.confirm(
      '',
      (isConfirmed) => {
          if (isConfirmed) {
              this._crdrNoteService.delete(CRDRNote.id)
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
    this._crdrNoteService.GetCRDRNoteToExcel(
      this.filterText,
      this.maxLocIDFilter == null ? this.maxLocIDFilterEmpty : this.maxLocIDFilter,
      this.minLocIDFilter == null ? this.minLocIDFilterEmpty : this.minLocIDFilter,
      this.maxDocNoFilter == null ? this.maxDocNoFilterEmpty : this.maxDocNoFilter,
      this.minDocNoFilter == null ? this.minDocNoFilterEmpty : this.minDocNoFilter,
      this.maxDocDateFilter,
      this.minDocDateFilter,
      this.typeIDFilter,
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
