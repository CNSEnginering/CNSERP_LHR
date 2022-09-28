import { Component, OnInit, ViewEncapsulation, ViewChild, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { GradeServiceProxy } from '../shared/services/grade-service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GradeDto } from '../shared/dto/grade-dto';
import { CreateOrEditGradeModalComponent } from './create-or-edit-grade-modal.component'; 
import { ViewGradeModalComponent } from './view-grade-modal.component';

@Component({
  templateUrl: './grade.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class GradeComponent extends AppComponentBase {
  @ViewChild('createOrEditGradeModal', { static: true }) createOrEditGradeModal: CreateOrEditGradeModalComponent;
  @ViewChild('viewGradeModal', { static: true }) viewGradeModal: ViewGradeModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxGradeIDFilter : number;
	maxGradeIDFilterEmpty : number;
	minGradeIDFilter : number;
	minGradeIDFilterEmpty : number;
  gradeNameFilter = '';
  maxTypeFilter : number;
	maxTypeFilterEmpty : number;
	minTypeFilter : number;
	minTypeFilterEmpty : number;
  activeFilter = -1;
  createdByFilter = '';
  maxCreateDateFilter : moment.Moment;
  minCreateDateFilter : moment.Moment;
  audtUserFilter = '';
  maxAudtDateFilter : moment.Moment;
  minAudtDateFilter : moment.Moment;  

  constructor(
    injector: Injector,
    private _gradeService: GradeServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService
  ) { 
    super(injector); 
  }

  getGrade(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
        this.paginator.changePage(0);
        return;
    }
debugger;
    this.primengTableHelper.showLoadingIndicator();

    this._gradeService.getAll(
        this.filterText,
        this.maxGradeIDFilter == null ? this.maxGradeIDFilterEmpty: this.maxGradeIDFilter,
        this.minGradeIDFilter == null ? this.minGradeIDFilterEmpty: this.minGradeIDFilter,
        this.gradeNameFilter,
        this.maxTypeFilter == null ? this.maxTypeFilterEmpty: this.maxTypeFilter,
        this.minTypeFilter == null ? this.minTypeFilterEmpty: this.minTypeFilter,
        this.activeFilter,
        this.createdByFilter,
        this.maxCreateDateFilter,
        this.minCreateDateFilter,
        this.audtUserFilter,
        this.maxAudtDateFilter,
        this.minAudtDateFilter,
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

createGrade(): void {
  this.createOrEditGradeModal.show();
}

deleteGrade(grade: GradeDto): void {
  this.message.confirm(
      '',
      (isConfirmed) => {
          if (isConfirmed) {
              this._gradeService.delete(grade.id)
                  .subscribe(() => {
                      this.reloadPage();
                      this.notify.success(this.l('SuccessfullyDeleted'));
                  });
          }
      }
  );
}

exportToExcel(): void {
    this._gradeService.GetGradeToExcel(
      this.filterText,
        this.maxGradeIDFilter == null ? this.maxGradeIDFilterEmpty: this.maxGradeIDFilter,
        this.minGradeIDFilter == null ? this.minGradeIDFilterEmpty: this.minGradeIDFilter,
        this.gradeNameFilter,
        this.maxTypeFilter == null ? this.maxTypeFilterEmpty: this.maxTypeFilter,
        this.minTypeFilter == null ? this.minTypeFilterEmpty: this.minTypeFilter,
       this.activeFilter,
        this.createdByFilter,
        this.maxCreateDateFilter,
        this.minCreateDateFilter,
        this.audtUserFilter,
        this.maxAudtDateFilter,
        this.minAudtDateFilter,
    )
  .subscribe(result => {
      this._fileDownloadService.downloadTempFile(result);
   });
}

}
