import { Component, OnInit, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { InventoryGlLinkService } from '../shared/services/inventory-gl-link.service';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { CreateOrEditInventoryGlLinkModalComponent } from './create-or-edit-InventoryGlLink-modal.component';
import { ViewInventoryGlLinkComponent } from './view-InventoryGlLink-modal.component';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient } from '@angular/common/http';
import { FileUpload } from 'primeng/primeng';
import { finalize } from 'rxjs/operators';

@Component({
  templateUrl: './inventoryGlLink.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class InventoryGlLinkcomponent extends AppComponentBase implements OnInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('InventoryGlLinkModal', { static: true }) InventoryGlLinkModal: CreateOrEditInventoryGlLinkModalComponent;
  @ViewChild('ViewInventoryGlLinkModal', { static: true }) ViewInventoryGlLinkModal: ViewInventoryGlLinkComponent;
  @ViewChild('ExcelFileUpload', { static: true }) excelFileUpload: FileUpload;

  advancedFiltersAreShown = false;
  filterText = '';
  inventoryGlLink: any;
  sorting: any;
  skipCount: any;
  MaxResultCount: any;
  uploadUrl: string;

  constructor(
    injector: Injector,
    private _fileDownloadService: FileDownloadService,
    private _inventoryGlLinkservice: InventoryGlLinkService,
    private _httpClient: HttpClient
  ) {

    super(injector);
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/InventoryGLLinks/ImportFromExcel';
  }

  ngOnInit() {
    // this.getAll(null);
  }
  reloadPage(): void {
    this.getAll();
  }
  getAll(event?: LazyLoadEvent) {
    this.sorting = this.primengTableHelper.getSorting(this.dataTable);
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event);
    this.MaxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event);

    this.primengTableHelper.showLoadingIndicator();
    this._inventoryGlLinkservice.getAll(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe(data => {
      this.inventoryGlLink = data["result"]["items"]
      this.primengTableHelper.totalRecordsCount = data["result"]["totalCount"];
      this.primengTableHelper.records = this.inventoryGlLink;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }
  delete(id: number) {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._inventoryGlLinkservice.delete(id).subscribe(() => {
            this.reloadPage();
            this.notify.success(this.l('SuccessfullyDeleted'));
          });
        }
      }
    );
  }
  create(id: number) {
    this.InventoryGlLinkModal.show(id);
  }
  exportToExcel() {
    this._inventoryGlLinkservice.GetInventoryGlLinksToExcel(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe((result: any) => {
      this._fileDownloadService.downloadTempFile(result["result"]);
    });
  }
  view(data: any) {
    this.ViewInventoryGlLinkModal.show(data);
  }

  uploadedFiles: any[] = [];
  onUpload(event): void {
    for (const file of event.files) {
      this.uploadedFiles.push(file);
    }
  }

  uploadExcel(data: { files: File }): void {
    const formData: FormData = new FormData();
    const file = data.files[0];
    formData.append('file', file, file.name);
    this.notify.success(this.l('ImportInventoryGLLinkProcessStart'));
    this._httpClient
      .post<any>(this.uploadUrl, formData)
      .pipe(finalize(() => this.excelFileUpload.clear()))
      .subscribe(response => {
        if (response["error"]["message"] === null)
          this.notify.success(this.l('AllInventoryGLLinkSuccessfullyImportedFromExcel'));
        else
          this.notify.error(response["error"]["message"]);
        //   if (response.success) {
        //       this.notify.success(this.l('AllInventoryGLLinkSuccessfullyImportedFromExcel'));
        //   } else if (response.error != null) {
        //       this.notify.error(this.l('ImportInventoryGLLinkUploadFailed'));
        //   }
      });
  }

  onUploadExcelError(): void {
    this.notify.error(this.l('ImportInventoryGLLinkUploadFailed'));
  }


}
