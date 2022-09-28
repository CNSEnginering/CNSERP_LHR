import { Component, OnInit, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator, FileUpload } from 'primeng/primeng';
import { Table } from 'primeng/table';
import { IcItemServiceProxy, ICItemDto } from '../../shared/services/ic-Item.service';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { CreateOrEditIcItemModalComponent } from './create-or-edit-ic-item-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ViewItemModalComponent } from './view-ic-items-modal.component';
import { finalize } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { AppConsts } from '@shared/AppConsts';

@Component({
  selector: 'icitems',
  templateUrl: './ic-items.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class IcItemsComponent extends AppComponentBase {
  
  @ViewChild("viewItemModal", { static: true }) viewItemModal: ViewItemModalComponent;
  @ViewChild("CreateOrEditIcItemModal", { static: true }) createOrEditIcItemModal: CreateOrEditIcItemModalComponent;
  @ViewChild("paginator", { static: true }) paginator: Paginator;
  @ViewChild("dataTable", { static: true }) dataTable: Table;
  @ViewChild('ExcelFileUpload', { static: true }) excelFileUpload: FileUpload;

  itemIdFilter = '';
  descpFilter  = '';
  seg1IdFilter = '';
  seg2IdFilter = '';
  seg3IdFilter = '';
  creationDateFilter = '';
  itemCtgFilter: number;
  itemTypeFilter: number;
  itemStatusFilter: number;
  stockUnitFilter = '';
  packingFilter: number;
  weightFilter: number;
  taxableFilter: boolean;
  saleableFilter: boolean;
  activeFilter: boolean;
  barcodeFilter = '';
  filterText: string;
  uploadUrl: string;

  constructor(
    injector: Injector,
    private _IcItemServiceProxy: IcItemServiceProxy,
    private _fileDownloadService: FileDownloadService,
    private _httpClient: HttpClient
  ) { super(injector)
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/ICItem/ImportFromExcel';
    console.log(this.uploadUrl);
   }


  getIcItems(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();

    debugger;
    this._IcItemServiceProxy.getAll(
      this.filterText,
      this.itemIdFilter,
      this.descpFilter,
      this.seg1IdFilter,
      this.seg2IdFilter,
      this.seg3IdFilter,
      this.itemCtgFilter,
      this.itemTypeFilter,
      this.itemStatusFilter,
      this.stockUnitFilter,
      this.packingFilter,
      this.weightFilter,
      this.taxableFilter,
      this.saleableFilter,
      this.activeFilter,
      this.barcodeFilter,
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

  createIcItems(): void {
    this.createOrEditIcItemModal.show(false);
  }

  deleteIcSegment(icSegment3: ICItemDto): void {
    this.message.confirm(
      '',
      (isConfirmed) => {
        if (isConfirmed) {
          this._IcItemServiceProxy.delete(icSegment3.id)
            .subscribe(() => {
              this.reloadPage();
              this.notify.success(this.l('SuccessfullyDeleted'));
            });
        }
      }
    );
  }

  exportToExcel(): void {
    debugger
    this._IcItemServiceProxy.GetIcItemToExcel(
      this.filterText,
      this.itemIdFilter,
      this.descpFilter,
      this.seg1IdFilter,
      this.seg2IdFilter,
      this.seg3IdFilter,
      this.itemCtgFilter,
      this.itemTypeFilter,
      this.itemStatusFilter,
      this.stockUnitFilter,
      this.packingFilter,
      this.weightFilter,
      this.taxableFilter,
      this.saleableFilter,
      this.activeFilter,
      this.barcodeFilter
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
  debugger
    const formData: FormData = new FormData();
    const file = data.files[0];
    formData.append('file', file, file.name);

    this._httpClient
        .post<any>(this.uploadUrl, formData)
        .pipe(finalize(() => this.excelFileUpload.clear()))
        .subscribe(response => {
            if (response.success) {
                this.notify.success(this.l('ImportICItemProcessStart'));
            } else if (response.error != null) {
                this.notify.error(this.l('ImportICItemUploadFailed'));
            }
        });
}

onUploadExcelError(): void {
    this.notify.error(this.l('ImportICItemUploadFailed'));
}

}
