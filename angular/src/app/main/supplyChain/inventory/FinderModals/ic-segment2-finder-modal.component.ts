import { Component, OnInit, ViewChild, Output, EventEmitter, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import { Table } from 'primeng/table';
import { IcSegment2ServiceProxy } from '../shared/services/ic-segment2-service';
import { NameValueDto } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'icsegment2findermodal',
  templateUrl: './ic-segment2-finder-modal.component.html',
  styles: []
})
export class IcSegment2FinderModalComponent extends AppComponentBase {

  @ViewChild('ICSegment2FinderModal', { static: true }) modal: ModalDirective;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
  active: boolean;
  filterText: '';
  id: string;
  displayName: string;
  seg1ID: string;
  constructor(
    injector: Injector,
    private _ICSegment2ServiceProxy: IcSegment2ServiceProxy
  ) { super(injector) }

  show(seg1ID: string ): void {
    this.active = true;
    this.seg1ID = seg1ID;
    this.paginator.rows = 5;
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

    this._ICSegment2ServiceProxy.GetICSegment2ForFinder(
      this.filterText,
      this.seg1ID,
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
