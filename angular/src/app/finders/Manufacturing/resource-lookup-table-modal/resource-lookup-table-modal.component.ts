
import * as moment from 'moment';
import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    ViewEncapsulation
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { AppComponentBase } from "@shared/common/app-component-base";
import { Table } from "primeng/components/table/table";
import { Paginator } from "primeng/components/paginator/paginator";
import { LazyLoadEvent } from "primeng/components/common/lazyloadevent";
import { ToolTypeFindersDto } from "@app/finders/shared/dtos/ToolTypefinders-dto";
import { ToolTypeFindersService } from '@app/finders/shared/services/ToolTypeServiceFinders.service';


@Component({
  selector: 'ResourceLookupTablemodal',
  templateUrl: './resource-lookup-table-modal.component.html',
  styleUrls: ['./resource-lookup-table-modal.component.css']
})
export class ResourceLookupTableModalComponent extends AppComponentBase {

  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @ViewChild("dataTable", { static: true }) dataTable: Table;
  @ViewChild("paginator", { static: true }) paginator: Paginator;

  filterText;
  id: string;
  displayName: string;
  unitCost:string;
  uom:string;
  

  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  active = false;
  saving = false;

  target: string;
  pickName: string;
  accountIDShow = false;
  termRateShow = false;
  docDate: Date;
  docMonth: number;
  configID: number;

  private paramFilter: string;
  private param2Filter: string;
  private param3Filter: number;
  subledger: boolean;
  accountID: string;

  sltype: number;
  displayNameShow: boolean;
  narrationShow: boolean;
  voucherNoShow: boolean;
  idShow: boolean;
  voucherDateShow: boolean;
  amount: number;
  amountShow: boolean;
  partyInvNoShow: boolean;

  constructor(
      injector: Injector,
      private _tooltyFindersService: ToolTypeFindersService
  ) {
      super(injector);
  }

  show(target:string): void {
      debugger
      this.active = true;
      this.paginator.rows = 5;
      this.target = target;
     // this.filterText = '';
      this.pickName = this.l('Pick');
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
      //this.filterText="Filters";
      this._tooltyFindersService
          .getAllToolTyFindersForLookupTable(
              this.filterText,
              this.target,
              this.paramFilter,
              this.param2Filter,
              this.param3Filter,
              this.primengTableHelper.getSorting(this.dataTable),
              this.primengTableHelper.getSkipCount(this.paginator, event),
              this.primengTableHelper.getMaxResultCount(this.paginator, event)
          )
          .subscribe(result => {
              this.primengTableHelper.totalRecordsCount = result.totalCount;
              this.primengTableHelper.records = result.items;
              this.primengTableHelper.hideLoadingIndicator();
          });
  }

  reloadPage(): void {
      this.paginator.changePage(this.paginator.getPage());
  }

  setAndSave(obj: ToolTypeFindersDto) {
      debugger
      this.id = obj.id;
      this.displayName = obj.displayName;
      this.unitCost=obj.unitCost;
      this.uom=obj.uom;
      this.active = false;
      this.modal.hide();
      this.modalSave.emit(null);
  }

  close(): void {
      this.active = false;
      this.modal.hide();
      this.modalSave.emit(null);
  }

}
