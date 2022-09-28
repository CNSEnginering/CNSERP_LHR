import { CreateOrEditRouteInvoicesComponent } from './create-or-edit-route-invoices.component';
import { Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { Paginator } from 'primeng/components/paginator/paginator';
import { Table } from 'primeng/components/table/table';
import { RouteInvoiceService } from '../../shared/services/route-invoice.service';

@Component({
  selector: 'app-route-invoices',
  templateUrl: './route-invoices.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class RouteInvoicesComponent extends AppComponentBase implements OnInit {

  filterText = '';
  sorting: any;
  skipCount: any;
  MaxResultCount: any;
  listData: any;

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('CreateOrEditRouteInvoicesComponent', { static: true }) CreateOrEditRouteInvoicesComponent: CreateOrEditRouteInvoicesComponent

  constructor(injector: Injector,
    private _routeInvoiceService: RouteInvoiceService) {
    super(injector);
  }

  ngOnInit() {
  }

  getAll(event?: LazyLoadEvent) {
    this.sorting = this.primengTableHelper.getSorting(this.dataTable);
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event);
    this.MaxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event);

    this.primengTableHelper.showLoadingIndicator();
    this._routeInvoiceService.getAll(
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

  createOrEdit(id: number) {
    this.CreateOrEditRouteInvoicesComponent.show(id);
  }
  delete(id: number) {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._routeInvoiceService.delete(id).subscribe(() => {
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

}
