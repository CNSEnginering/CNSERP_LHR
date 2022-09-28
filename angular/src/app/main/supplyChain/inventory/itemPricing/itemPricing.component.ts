import { Component, OnInit, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { Paginator } from 'primeng/components/paginator/paginator';
import { Table } from 'primeng/components/table/table';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CreateOrEditItemPricingModalComponent } from './create-or-edit-itemPricing-modal.component';
import { ItemPricingService } from '../shared/services/itemPricing.service';
import { ViewItemPricingComponent } from './view-itemPricing-modal.component';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { FileUpload } from 'primeng/primeng';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient } from '@angular/common/http';
import { finalize } from 'rxjs/operators';
@Component({
  templateUrl: './itemPricing.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class ItemPricingComponent extends AppComponentBase implements OnInit {
  filterText = '';
  priceList: any;
  sorting: any;
  skipCount: any;
  MaxResultCount: any;
  uploadUrl: string;

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('itemPricingModal', { static: true }) itemPricingModal: CreateOrEditItemPricingModalComponent;
  @ViewChild('viewItemPricingModal', { static: true }) viewItemPricingModal: ViewItemPricingComponent;
  @ViewChild('ExcelFileUpload', { static: true }) excelFileUpload: FileUpload;
  constructor(injector: Injector,
    private _itemPricingService: ItemPricingService,
    private _fileDownloadService: FileDownloadService,
    private _httpClient: HttpClient
  ) {
    super(injector);
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/ICItemPricing/ImportFromExcel';
  }

  ngOnInit() {
  }
  getAll(event?: LazyLoadEvent) {
    this.sorting = this.primengTableHelper.getSorting(this.dataTable);
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event);
    this.MaxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event);

     this.primengTableHelper.showLoadingIndicator();
     this._itemPricingService.getAll(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe(data => {
      this.priceList = data["result"]["items"]
      this.primengTableHelper.totalRecordsCount = data["result"]["totalCount"];
      this.primengTableHelper.records = this.priceList;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }
  delete(id: string) {
    debugger
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._itemPricingService.delete(id).subscribe(() => {
            this.reloadPage()
            this.notify.success(this.l('SuccessfullyDeleted'))
          });
        }
      }
    );
  }
  reloadPage(): void {
    this.paginator.changePage(this.paginator.getPage());
  }
  createOrEdit(id: string) {
    this.itemPricingModal.show(id);
  }
  view(data: any) {
    this.viewItemPricingModal.show(data);
  }
  exportToExcel() {
    this._itemPricingService.GetItemPricingToExcel(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe((result: any) => {
      this._fileDownloadService.downloadTempFile(result["result"]);
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
                this.notify.success(this.l('ImportICItemPriceProcessStart'));
            } else if (response.error != null) {
                this.notify.error(this.l('ImportICItemPriceUploadFailed'));
            }
        });
}

onUploadExcelError(): void {
    this.notify.error(this.l('ImportICItemPriceUploadFailed'));
}
}
