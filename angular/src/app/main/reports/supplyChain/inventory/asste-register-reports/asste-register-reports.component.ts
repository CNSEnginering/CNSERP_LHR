import { Component, Injector, ViewEncapsulation, ViewChild, EventEmitter, Output, OnInit } from '@angular/core';
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
import { LocLookupTableModalComponent } from "@app/main/reports/loc-lookup-finder/loc-lookup-table-modal.component";
import { Router } from "@angular/router";
import { AppComponentBase } from "@shared/common/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import * as _ from "lodash";
import { FiscalDateService } from "@app/shared/services/fiscalDate.service";
import * as moment from "moment";
import { GetDataService } from '../../../../supplyChain/inventory/shared/services/get-data.service';
import { IDropdownSettings, } from 'ng-multiselect-dropdown';
@Component({
 
  templateUrl: './asste-register-reports.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class AssteRegisterReportsComponent extends AppComponentBase {

  @ViewChild('reportView', { static: true }) reportView: ReportviewrModalComponent;
  @ViewChild("LocLookupTableModal", { static: true }) LocLookupTableModal: LocLookupTableModalComponent;
 
  
  reportServer: string;
  reportUrl: string;
  showParameters: string;
  parameters: any;
  language: string;
  width: number;
  height: number;
  toolbar: string;
  locationList:any;
  dropdownSettings:IDropdownSettings={};
  locIdlist:any;
  rptObj: any;
  location: number;
  fromDate: any;
  toDate: any;
  fromLoc: any;
  toLoc: any;
  fromLocName: any;
  toLocName: any;
  type:any;
  
  constructor(
    injector: Injector,
    private route: Router,
    private _reportService: FiscalDateService,private _getDataService: GetDataService
) {
    super(injector);
}
ngOnInit() {
  this._reportService.getDate().subscribe(data => {
    this.fromDate = new Date(data["result"]);
    this.toDate = new Date();
    this.getLocationList();
        this.dropdownSettings = {
            idField: 'id',
            textField: 'displayName',
            enableCheckAll: true, allowSearchFilter: true,
            selectAllText: "Select All Location",
            unSelectAllText: "UnSelect All Location",
            itemsShowLimit: 0,singleSelection: false,
          };
});

  this.fromLoc = "0";
  this.toLoc = "999";

  }

  getLocationList(): void {
    this._getDataService.getList("ICLocations").subscribe(resultL => {
        this.locationList = resultL;
    });
    
}
  getLookUpData() {
    debugger;
    if (this.type == "fromLoc") {
        this.fromLoc = this.LocLookupTableModal.data.locID;
        this.fromLocName = this.LocLookupTableModal.data.locName;
     
    } else if (this.type == "toLoc") {
        this.toLoc = this.LocLookupTableModal.data.locID;
        this.toLocName = this.LocLookupTableModal.data.locName;
    }
}

  openModal(type) {
    debugger;
    this.type = type;
    this.LocLookupTableModal.show();
    
}
setIdNull(type) {
  this.type = type;
  if (this.type == "fromLoc") {
      this.fromLoc = 0;
      this.fromLocName = "";
     
  } else {
     
      this.toLoc = "";
     
      this.toLocName = "";
  }
}
getReport() {
  debugger;
  this.processReport("AssetResourceList");
 
}
processReport(report: string) {
  var locstr="";
  console.log(this.locIdlist);
  if(this.locIdlist!=undefined){
      this.locIdlist.forEach(element => {
          debugger
         locstr= locstr+element.id+",";
      });
  }
  let dxrep = false;
  let repParams = "";
  if (this.fromDate !== undefined)
  repParams +=
      "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
if (this.toDate !== undefined)
  repParams +=
      "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
// if (this.fromLoc !== undefined)
//   repParams += encodeURIComponent("" + this.fromLoc) + "$";
// if (this.toLoc !== undefined)
//   repParams += encodeURIComponent("" + this.toLoc) + "$";
repParams += locstr.replace(/^,|,$/g, '') + "$";
repParams += locstr.replace(/^,|,$/g, '') + "$";
// if (this.orderBy !== undefined)
//     repParams += encodeURIComponent("" + this.orderBy) + "$";

repParams = repParams.replace(/[?$]&/, "");
dxrep = true;

  if (dxrep) {
      debugger;
      this.reportView.show(report, repParams);
  } else {
      localStorage.setItem("rptObj", JSON.stringify(this.rptObj));
      localStorage.setItem("rptName", report);

      this.route.navigateByUrl("/app/main/reports/ReportView");
  }
}


}
