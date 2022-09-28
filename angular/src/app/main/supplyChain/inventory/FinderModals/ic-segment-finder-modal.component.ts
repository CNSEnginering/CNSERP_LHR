import { Component, OnInit, Injector, ViewChild, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import { Table } from 'primeng/table';
import { IcSegment1ServiceProxy } from '../shared/services/ic-segment1-service';
import { NameValueDto } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'icsegmentfindermodal',
  templateUrl: './ic-segment-finder-modal.component.html',
  styles: []
})
export class IcSegmentFinderModalComponent extends AppComponentBase {

  @ViewChild('ICSegment1FinderModal', { static: true }) modal: ModalDirective;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
  active: boolean;
  filterText: '';
  id: string;
  displayName: string;
  constructor(
    injector: Injector,
    private _ICSegment1ServiceProxy: IcSegment1ServiceProxy
  ) { super(injector) }

  show(): void {
    this.active = true;
    this.paginator.rows = 5;
    debugger;
    this.getAll();
    this.modal.show();
  }

  getAll(event?: LazyLoadEvent) {
    if (!this.active) {
      return;
    }

    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();

    this._ICSegment1ServiceProxy.GetICSegment1ForFinder(
      this.filterText,
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

  setAndSave(segment1: NameValueDto) {
    debugger;
    this.id = segment1.value;
    this.displayName = segment1.name;
    this.active = false;
    this.modal.hide();
    this.modalSelect.emit(null);
  }

  close(): void {
    this.active = false;
    this.modal.hide();
    this.modalSelect.emit(null);
  }
}
