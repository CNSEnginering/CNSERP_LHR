import { LogService } from "../shared/services/Log.service";
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

@Component({
  selector: 'app-log',
  templateUrl: './log.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LogComponent extends AppComponentBase{

  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @ViewChild("dataTable", { static: true }) dataTable: Table;
  @ViewChild("paginator", { static: true }) paginator: Paginator;

  formName: string;
  docNo:number;

  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  active = false;
  saving = false;

  constructor(
      injector: Injector,
      private _LogService: LogService
  ) {
      super(injector);
  }

  show(docNo :any, formName:any): void {
     debugger
     this.active=true;
     this.docNo=docNo;
     this.formName=formName;    
      this.getAll();
      this.modal.show();
  }

  getAll(event?: LazyLoadEvent) {
      debugger;
      if (!this.active) {
          return;
      }

      if (this.primengTableHelper.shouldResetPaging(event)) {
          this.paginator.changePage(0);
          return;
      }

      this.primengTableHelper.showLoadingIndicator();

      this._LogService
          .getAll(
            this.formName,
              this.docNo
          )
          .subscribe(result => {
            debugger
            console.log(result);
              this.primengTableHelper.totalRecordsCount = result["result"].length;
              this.primengTableHelper.records = result["result"];
              this.primengTableHelper.hideLoadingIndicator();
          });
  }

  reloadPage(): void {
      this.paginator.changePage(this.paginator.getPage());
  }

  
  close(): void {
      this.active = false;
      this.modal.hide();
      this.modalSave.emit(null);
  }

}
