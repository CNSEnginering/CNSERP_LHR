import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/primeng';
import { Table } from 'primeng/table';
import { mftooltyServiceProxy } from '../../shared/service/mftoolty.service';
import { CreateOrEditMftooltyComponent } from './create-or-edit-mftoolty.component';
import { ViewMftooltyComponent } from './view-mftoolty.component';

@Component({
  selector: 'app-mftoolty',
  templateUrl: './mftoolty.component.html',
  styleUrls: ['./mftoolty.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class MftooltyComponent extends AppComponentBase implements OnInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('ToolTypeModal', { static: true }) createOrEditModal: CreateOrEditMftooltyComponent;
  @ViewChild('viewmftoolty', { static: true }) viewAModal: ViewMftooltyComponent;
  advancedFiltersAreShown = false;
  filterText = '';
  dataList: any;
  sorting: any;
  skipCount: any;
  MaxResultCount: any;

  constructor(
    injector: Injector,
    private _fileDownloadService: FileDownloadService,
    private _MftooltyService: mftooltyServiceProxy,
  ) {

    super(injector);
  }

  ngOnInit() {
    // this.getAll(null);
  }
  reloadPage(): void {
    debugger
    
    this.getAll();
    
  }
  getAll(event?: LazyLoadEvent) {
    this.sorting = this.primengTableHelper.getSorting(this.dataTable);
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event);
    this.MaxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event);

    this.primengTableHelper.showLoadingIndicator();
    this._MftooltyService.getAll(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe(data => {
      console.log(data.items);
      debugger
      this.primengTableHelper.totalRecordsCount = data.totalCount;
      this.primengTableHelper.records = data.items;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }
  delete(id: number) {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._MftooltyService.delete(id).subscribe(() => {
            this.reloadPage();
            this.notify.success(this.l('SuccessfullyDeleted'));
          });
        }
      }
    );
  }
  createOrEdit(id: number) {
    debugger
    
    this.createOrEditModal.show(id);
  }
  exportToExcel() {
    this._MftooltyService.GetDataToExcel(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe((result: any) => {
      this._fileDownloadService.downloadTempFile(result["result"]);
    });
  }
  view(data: any) {
    debugger
    this.viewAModal.show(data);
  }
}
