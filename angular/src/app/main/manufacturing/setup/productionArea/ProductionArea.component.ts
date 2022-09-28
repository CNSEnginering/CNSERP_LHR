import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/primeng';
import { Table } from 'primeng/table';
import { ManufacAccServiceProxy } from '../../shared/service/manufacAcc.service';
import { ProductionAreaServiceProxy } from '../../shared/service/productarea.service';
import { CreateOrEditManufacAccModalComponent } from '../accountSetup/create-or-edit-manufacAcc-modal.component';
import { ViewManufacAccModalComponent } from '../accountSetup/view-manufacAcc-modal.component';
import { CreateOrEditProductionAreaComponent } from './create-or-edit-productionarea.component';
import { ViewProductionAreaComponent } from './view-productionarea.component';

@Component({
  selector: 'app-ProductionArea',
  templateUrl: './ProductionArea.component.html',
  styleUrls: ['./ProductionArea.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})


export class ProductionAreaComponent extends AppComponentBase implements OnInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('prodAreaModal', { static: true }) createOrEditModal: CreateOrEditProductionAreaComponent;
  @ViewChild('viewProductionarea', { static: true }) viewAModal: ViewProductionAreaComponent;
  advancedFiltersAreShown = false;
  filterText = '';
  dataList: any;
  sorting: any;
  skipCount: any;
  MaxResultCount: any;

  constructor(
    injector: Injector,
    private _fileDownloadService: FileDownloadService,
    private _prodAreaService: ProductionAreaServiceProxy,
  ) {

    super(injector);
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
    this._prodAreaService.getAll(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe(data => {
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
          this._prodAreaService.delete(id).subscribe(() => {
            this.reloadPage();
            this.notify.success(this.l('SuccessfullyDeleted'));
          });
        }
      }
    );
  }
  createOrEdit(id: number) {
    this.createOrEditModal.show(id);
  }
  exportToExcel() {
    this._prodAreaService.GetDataToExcel(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe((result: any) => {
      this._fileDownloadService.downloadTempFile(result["result"]);
    });
  }
  view(data: any) {
    this.viewAModal.show(data);
  }
}


