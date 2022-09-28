import { Component, OnInit, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { Paginator } from 'primeng/components/paginator/paginator';
import { Table } from 'primeng/components/table/table';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { GetDataService } from '@app/main/supplyChain/inventory/shared/services/get-data.service';
import { CreateOrEditInvoiceKnockOffComponent } from './create-or-edit-invoice-knock-off.component';
import { invoiceKnockOffService } from '../shared/services/invoice-knock-off.service';


@Component({
  selector: 'app-invoice-knock-off',
  templateUrl: './invoice-knock-off.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class InvoiceKnockOffComponent extends AppComponentBase implements OnInit {

  filterText = '';
  priceList: any;
  sorting: any;
  skipCount: any;
  MaxResultCount: any;
  listData: any;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('CreateOrEditInvoiceKnockOffComponent', { static: true }) GetInvoiceKnockOffModal: CreateOrEditInvoiceKnockOffComponent;
  //@ViewChild('viewAssemblyModal', { static: true }) viewAssemblyModal: ViewAssemblyComponent;
  constructor(injector: Injector,
    private _assemblyService: invoiceKnockOffService,
    private _fileDownloadService: FileDownloadService,
    private _getDataService: GetDataService
  ) {
    super(injector);
  }

  ngOnInit() {
  }
  getAll(event?: LazyLoadEvent) {
    this.sorting = this.primengTableHelper.getSorting(this.dataTable);
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event);
    this.MaxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event);

    this.primengTableHelper.showLoadingIndicator();
    this._assemblyService.getAll(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe(data => {
      debugger
      console.log(data["result"]["items"]);
      this.listData = data["result"]["items"];
      this.primengTableHelper.totalRecordsCount = data["result"]["totalCount"];
      this.primengTableHelper.records = this.listData;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }
  delete(id: number) {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._assemblyService.delete(id).subscribe(() => {
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
  createOrEdit(id: number) {
    this.GetInvoiceKnockOffModal.show(id);
  }
  view(data: any) {
     //this.viewAssemblyModal.show(data);
  }

}
