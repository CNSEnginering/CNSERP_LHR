import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
import * as moment from 'moment';
import { FiscalDateService } from '@app/shared/services/fiscalDate.service';

@Component({
  selector: 'app-porfit-and-loss-statment',
  templateUrl: './porfit-and-loss-statment.component.html'
})
export class PorfitAndLossStatmentComponent extends AppComponentBase implements OnInit {

  @ViewChild("reportView", { static: true })
  reportView: ReportviewrModalComponent;
  documentListing: string='PLSTATMENT';
  fromDate;
    toDate;
  constructor( 
    injector: Injector,
    private _reportService: FiscalDateService,
  ) {
    super(injector)
   }
  ngOnInit(): void {
    this._reportService.getDate().subscribe(data => {
      this.fromDate = new Date(data["result"]);
      this.toDate = new Date();
  });
  }

   getReport() {
    debugger;

    let repParams = "";
    if (this.fromDate !== undefined)
        repParams += "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
    if (this.toDate !== undefined)
        repParams += "" + moment(this.toDate).format("YYYY/MM/DD") ;

    repParams = repParams.replace(/[?$]&/, "");

    this.reportView.show(this.documentListing, repParams);
}


}
