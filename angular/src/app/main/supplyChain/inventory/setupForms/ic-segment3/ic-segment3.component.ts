import { Component, Injector, ViewEncapsulation, ViewChild, OnInit,  } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator, FileUpload } from 'primeng/primeng';
import { Table } from 'primeng/table';
import { CreateOrEditIcSegment3ModalComponent } from './create-or-edit-icsegment3-Modal.component';
import { ViewIcsegment3ModalComponent } from './view-icsegment3-modal.component';
import { IcSegment3ServiceProxy, ICSegment3Dto } from '../../shared/services/ic-segment3-service';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment  from 'moment';
import { ICSetupsService } from '../../shared/services/ic-setup.service';
import { AppConsts } from '@shared/AppConsts';
import { finalize } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'icSegment3Selector',
  templateUrl: './ic-segment3.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class IcSegment3Component extends AppComponentBase implements OnInit {
  @ViewChild("createOrEditIcSegment3Modal", { static: true }) createOrEditIcSegment3Modal: CreateOrEditIcSegment3ModalComponent;
  @ViewChild("viewIcSegment3Modal", { static: true }) viewIcSegment3Modal: ViewIcsegment3ModalComponent;
  @ViewChild("paginator", { static: true }) paginator: Paginator;
  @ViewChild("dataTable", { static: true }) dataTable: Table;
  @ViewChild('ExcelFileUpload', { static: true }) excelFileUpload: FileUpload;
  filterText: any;
  seg3IdFilter: any;
  seg3NameFilter: any;
  seg2NameFilter: any;
  seg1NameFilter: any;


  seg1Setup:string;
  seg2Setup: string;
  seg3Setup: string;

  seg1IdFilter: '';
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

        uploadUrl: string;
  constructor(
    injector:Injector,
    private _IcSegment3ServiceProxy: IcSegment3ServiceProxy,
    private _fileDownloadService: FileDownloadService,
    private _icSetupsService: ICSetupsService,
    private _httpClient: HttpClient
  ) 
  {
     super(injector)
     this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/ICSegment3/ImportFromExcel';
  }


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
     // this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.seg1Setup = result.items[0].segment1;
      this.seg2Setup = result.items[0].segment2;
      this.seg3Setup = result.items[0].segment3;
  });
  }

  getIcSegments(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();

    debugger;
    this._IcSegment3ServiceProxy.getAll(
      this.filterText,
      this.seg3IdFilter,
      this.seg3NameFilter,
      this.seg2NameFilter,
      this.seg1NameFilter,
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

  createIcSegment(): void {
     this.createOrEditIcSegment3Modal.show(false); 
  }

  deleteIcSegment(icSegment3: ICSegment3Dto): void {
    this.message.confirm(
      '',
      (isConfirmed) => {
        if (isConfirmed) {
          this._IcSegment3ServiceProxy.delete(icSegment3.id)
            .subscribe(() => {
              this.reloadPage();
              this.notify.success(this.l('SuccessfullyDeleted'));
            });
        }
      }
    );
  }

  exportToExcel(): void {
    this._IcSegment3ServiceProxy.GetICSegment3ToExcel(
      this.filterText,
      this.seg3IdFilter,
      this.seg3NameFilter,
      this.seg2NameFilter,
      this.seg1NameFilter,
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
          debugger
          if (response["error"]["message"] === null || response["error"]["message"]==="")
          this.notify.success(
              this.l("AllChartofAccountSuccessfullyImportedFromExcel")
          );
      else this.message.error(response["error"]["message"]);
            // if (response.success) {
            //     this.notify.success(this.l('ImportICSegment3ProcessStart'));
            // } else if (response.error != null) {
            //     this.notify.error(this.l('ImportICSegment3UploadFailed'));
            // }
        });
}

onUploadExcelError(): void {
    this.notify.error(this.l('ImportICSegment3UploadFailed'));
}

}
