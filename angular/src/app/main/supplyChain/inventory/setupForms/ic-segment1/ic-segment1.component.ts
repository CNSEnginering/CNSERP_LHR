import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { IcSegment1ServiceProxy, ICSegment1Dto } from '../../shared/services/ic-segment1-service';
import { CreateOrEditIcSegment1ModalComponent } from './create-or-edit-IcSegment1-modal.component';
import { ViewIcsegment1ModalComponent } from './view-icsegment1-modal.component';
import { Paginator, FileUpload } from 'primeng/primeng';
import { Table } from 'primeng/table';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from  'moment';
import { ICSetupsService } from '../../shared/services/ic-setup.service';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient } from '@angular/common/http';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'icSegment1Selector',
  templateUrl: './ic-segment1.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class IcSegment1Component extends AppComponentBase implements OnInit {

  ngOnInit(): void {
    this._icSetupsService.getAll(
      this.filterText,
      this.seg1IdFilter,
      this.segment2Filter,
      this.segment3Filter,
      this.maxErrSrNoFilter == null ? this.maxErrSrNoFilterEmpty: this.maxErrSrNoFilter,
      this.minErrSrNoFilter == null ? this.minErrSrNoFilterEmpty: this.minErrSrNoFilter,
      this.maxCostingMethodFilter == null ? this.maxCostingMethodFilterEmpty: this.maxCostingMethodFilter,
      this.minCostingMethodFilter == null ? this.minCostingMethodFilterEmpty: this.minCostingMethodFilter,
      this.prBookIDFilter,
      this.rtBookIDFilter,
      this.cnsBookIDFilter,
      this.slBookIDFilter,
      this.srBookIDFilter,
      this.trBookIDFilter,
      this.prdBookIDFilter,
      this.pyRecBookIDFilter,
      this.adjBookIDFilter,
      this.asmBookIDFilter,
      this.wsBookIDFilter,
      this.dsBookIDFilter,
      this.maxCurrentLocIDFilter == null ? this.maxCurrentLocIDFilterEmpty: this.maxCurrentLocIDFilter,
      this.minCurrentLocIDFilter == null ? this.minCurrentLocIDFilterEmpty: this.minCurrentLocIDFilter,
      this.opt4Filter,
      this.opt5Filter,
      this.createdByFilter,
      this.maxCreateadOnFilter,
      this.minCreateadOnFilter,
      null,
      0,
      2147483646
  ).subscribe(result => {
      debugger;
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.segSetup = result.items[0].segment1;
  });
  }

  @ViewChild("createOrEditIcSegment1Modal", { static: true }) createOrEditIcSegment1Modal: CreateOrEditIcSegment1ModalComponent;
  @ViewChild("viewIcsegment1Modal", { static: true }) viewIcsegment1Modal: ViewIcsegment1ModalComponent;
  @ViewChild("dataTable", { static: true }) dataTable: Table;
  @ViewChild("paginator", { static: true }) paginator: Paginator;
  @ViewChild('ExcelFileUpload', { static: true }) excelFileUpload: FileUpload;


  segSetup: string;

  advancedFiltersAreShown = false;
  filterText: '';
  seg1IdFilter: '';
  seg1NameFilter: '';
    segment2Filter = '';
    segment3Filter = '';
    maxAllowNegativeFilter : number;
		maxAllowNegativeFilterEmpty : number;
		minAllowNegativeFilter : number;
		minAllowNegativeFilterEmpty : number;
    maxErrSrNoFilter : number;
		maxErrSrNoFilterEmpty : number;
		minErrSrNoFilter : number;
		minErrSrNoFilterEmpty : number;
    maxCostingMethodFilter : number;
		maxCostingMethodFilterEmpty : number;
		minCostingMethodFilter : number;
		minCostingMethodFilterEmpty : number;
    prBookIDFilter = '';
    rtBookIDFilter = '';
    cnsBookIDFilter = '';
    slBookIDFilter = '';
    srBookIDFilter = '';
    trBookIDFilter = '';
    prdBookIDFilter = '';
    pyRecBookIDFilter = '';
    adjBookIDFilter = '';
    asmBookIDFilter = '';
    wsBookIDFilter = '';
    dsBookIDFilter = '';
    maxCurrentLocIDFilter : number;
		maxCurrentLocIDFilterEmpty : number;
		minCurrentLocIDFilter : number;
		minCurrentLocIDFilterEmpty : number;
    opt4Filter = '';
    opt5Filter = '';
    createdByFilter = '';
    maxCreateadOnFilter : moment.Moment;
    minCreateadOnFilter : moment.Moment;
    disableCreateButton=false
    uploadUrl: string;

  constructor(
    injector: Injector,
    private _IcSegment1ServiceProxy: IcSegment1ServiceProxy,
    private _fileDownloadService: FileDownloadService,
    private _icSetupsService: ICSetupsService,
    private _httpClient: HttpClient
  ) {
    super(injector);
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/ICSegment1/ImportFromExcel';
  }

  
  getIcSegments(event?: LazyLoadEvent) {
    debugger;
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();
    this._IcSegment1ServiceProxy.getAll(
      this.filterText,
      this.seg1IdFilter,
      this.seg1NameFilter,
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

  createIcSegment(): void {
    this.createOrEditIcSegment1Modal.show(false);
  }

  deleteIcSegment(icSegment1: ICSegment1Dto): void {
    this.message.confirm(
      '',
      (isConfirmed) => {
        if (isConfirmed) {
          this._IcSegment1ServiceProxy.delete(icSegment1.id)
            .subscribe(() => {
              this.reloadPage();
              this.notify.success(this.l('SuccessfullyDeleted'));
            });
        }
      }
    );
  }

  exportToExcel(): void {
    this._IcSegment1ServiceProxy.GetICSegment1ToExcel(
      this.filterText,
      this.seg1IdFilter,
      this.seg1NameFilter
    )
      .subscribe(result => {
        this._fileDownloadService.downloadTempFile(result);
      });
  }

  uploadedFiles: any[] = [];
  onUpload(event): void {
    for (const file of event.files) {
        this.uploadedFiles.push(file);
    }
}

onBeforeSend(event): void {
    event.xhr.setRequestHeader('Authorization', 'Bearer ' + abp.auth.getToken());
}

uploadExcel(data: { files: File }): void {
    const formData: FormData = new FormData();
    const file = data.files[0];
    formData.append('file', file, file.name);

    this._httpClient
        .post<any>(this.uploadUrl, formData)
        .pipe(finalize(() => this.excelFileUpload.clear()))
        .subscribe(response => {
            if (response.success) {
                this.notify.success(this.l('ImportICSegment1ProcessStart'));
            } else if (response.error != null) {
                this.notify.error(this.l('ImportICSegment1UploadFailed'));
            }
        });
}

onUploadExcelError(): void {
    this.notify.error(this.l('ImportICSegment1UploadFailed'));
}
}
