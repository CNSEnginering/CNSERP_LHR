import { Component, OnInit, ViewChild, Output, EventEmitter, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/table';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import { ReportFilterServiceProxy, GLTRHeadersServiceProxy, GLTRHeaderGLSubledgerLookupTableDto } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'appsubledgerlookupfinder',
  templateUrl: './subledger-lookup-finder.component.html',
  styleUrls: ['./subledger-lookup-finder.component.less']
})
export class SubledgerLookupFinderComponent extends AppComponentBase {

  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  filterText = '';
  targetFilter='';
  id: string;
  displayName: string;
  
  @Output() modalSave1: EventEmitter<any> = new EventEmitter<any>();
  active = false;
  saving = false;

  constructor(
      injector: Injector,
      private _reportFilterService: ReportFilterServiceProxy,
      private _gltrHeadersServiceProxy: GLTRHeadersServiceProxy,
     
  ) {
      super(injector);
  }

  ngOnInit() {
  }

  show(accountId:string){
    this.active = true;
    this.targetFilter=accountId;
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

    this._gltrHeadersServiceProxy.getAllGLSubledgerForLookupTable(
        this.filterText,
        this.targetFilter,
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

setAndSave(glSubledger: GLTRHeaderGLSubledgerLookupTableDto) {
    this.id = glSubledger.id.toString();
    this.displayName = glSubledger.displayName;
    this.active = false;
    this.modal.hide();
    this.modalSave1.emit(null);
}

close(): void {
    this.active = false;
    this.modal.hide();
    this.modalSave1.emit(null);
}

}
