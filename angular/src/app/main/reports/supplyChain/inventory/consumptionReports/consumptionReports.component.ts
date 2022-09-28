import {
    Component,
    Injector,
    ViewEncapsulation,
    ViewChild,
    EventEmitter,
    Output
} from "@angular/core";
import { Router } from "@angular/router";
import { AppComponentBase } from "@shared/common/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import * as _ from "lodash";
import * as moment from "moment";
import { ItemLookupTableModalComponent } from "@app/main/reports/item-lookup-finder/item-lookup-table-modal.component";
import { LocLookupTableModalComponent } from "@app/main/reports/loc-lookup-finder/loc-lookup-table-modal.component";
import { FiscalDateService } from "@app/shared/services/fiscalDate.service";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";
import { GetDataService } from '../../../../supplyChain/inventory/shared/services/get-data.service';
import { IDropdownSettings, } from 'ng-multiselect-dropdown';
@Component({
    templateUrl: "./consumptionReports.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ConsumptionReportsComponent extends AppComponentBase {
    @ViewChild("ItemLookupTableModal", { static: true })
    ItemLookupTableModal: ItemLookupTableModalComponent;
    @ViewChild("LocLookupTableModal", { static: true })
    LocLookupTableModal: LocLookupTableModalComponent;
    @ViewChild("reportView", { static: true })
    reportView: ReportviewrModalComponent;
    rptParams: string = "";
    itemLedger: string="";
    locationList:any;
    dropdownSettings:IDropdownSettings={};
    locIdlist:any;
    fromItem: any;
    toItem: any;
    fromDate: any;
    toDate: any;
    fromDoc: any;
    toDoc: any;
    fromItemName: any;
    toItemName: any;
    fromLoc: any;
    toLoc: any;
    fromLocName: any;
    toLocName: any;

    showToDate: boolean = true;
    showFromDate: boolean = true;
    showToDoc: boolean = true;
    showFromDoc: boolean = true;
    showToLoc: boolean = true;
    showFromLoc: boolean = true;
    showToItem: boolean = true;
    showFromItem: boolean = true;
    type: string = "";

    constructor(
        injector: Injector,
        private route: Router,
        private _reportService: FiscalDateService,private _getDataService: GetDataService
    ) {
        super(injector);
    }

    ngOnInit() {
        this.toDate = new Date();
        this._reportService.getDate().subscribe(data => {
            this.fromDate = new Date(data["result"]);
            
        });
        this.fromDoc = 0;
        this.toDoc = 99999;
        this.fromLoc = "0";
        this.toLoc = "99999";
        this.fromItem = "0";
        this.toItem = "99-999-99-9999";
        this.itemLedger = "consumptionReportItemWise";
        this.hideFields();
        this.getLocationList();
        this.dropdownSettings = {
            idField: 'id',
            textField: 'displayName',
            enableCheckAll: true, allowSearchFilter: true,
            selectAllText: "Select All Location",
            unSelectAllText: "UnSelect All Location",
            itemsShowLimit: 0,singleSelection: false,
          };

    }

    getLocationList(): void {
        this._getDataService.getList("ICLocations").subscribe(resultL => {
            this.locationList = resultL;
        });
        
    }
    hideFields() {
        switch (this.itemLedger) {
            case "actualAndBudget":
                this.showFromDate = false;
                this.showToDate = false;
                this.showFromLoc = false;
                this.showToLoc = false;
                this.showFromItem = false;
                this.showToItem = false;
                this.showFromDoc = true;
                this.showToDoc = true;
                break;
            case "consumptionReportItemWise":
                this.showFromDate = true;
                this.showToDate = true;
                this.showFromLoc = false;
                this.showToLoc = false;
                this.showFromItem = true;
                this.showToItem = true;
                this.showFromDoc = false;
                this.showToDoc = false;
                break;
            case "consumptionReportAccWise":
            case "consumptionReportOrderWise":
                this.showFromDate = true;
                this.showToDate = true;
                this.showFromLoc = false;
                this.showToLoc = false;
                this.showFromItem = false;
                this.showToItem = false;
                this.showFromDoc = false;
                this.showToDoc = false;
                break;
            case "consumptionSummaryDepartmentWise":
            case "consumptionDepartmentWise":
                this.showFromDate = false;
                this.showToDate = false;
                this.showFromLoc = true;
                this.showToLoc = true;
                this.showFromItem = false;
                this.showToItem = false;
                this.showFromDoc = false;
                this.showToDoc = false;
                break;
        }
    }
    getReport() {
        debugger;
        switch (this.itemLedger) {
            case "actualAndBudget":
                this.processReport("ActualAndBudget");
                break;
            case "consumptionReportOrderWise":
                this.processReport("ConsumptionReportOrderWise");
                break;
            case "consumptionReportAccWise":
                this.processReport("ConsumptionReportAccWise");
                break;
            case "consumptionReportItemWise":
                this.processReport("ConsumptionReportItemWise");
                break;
            case "consumptionSummaryDepartmentWise":
                this.processReport("ConsumptionSummaryDepartmentWise");
                break;
            case "consumptionDepartmentWise":
                this.processReport("ConsumptionDepartmentWise");
                break;
        }
    }
    processReport(report: string) {
        debugger;
        var locstr="";
        console.log(this.locIdlist);
        if(this.locIdlist!=undefined){
            this.locIdlist.forEach(element => {
                debugger
               locstr= locstr+element.id+",";
            });
        }
       
        this.rptParams = "";
        switch (report) {
            case "ActualAndBudget":
                if (this.fromDoc !== undefined)
                    this.rptParams += "" + this.fromDoc + "$";
                if (this.toDoc !== undefined)
                    this.rptParams += "" + this.toDoc + "$";

                this.rptParams = this.rptParams.replace(/[?$]&/, "");
                break;
            case "ConsumptionSummaryDepartmentWise":
            case "ConsumptionDepartmentWise":
            case "ConsumptionReportItemWise":
            case "ConsumptionReportAccWise":
            case "ConsumptionReportOrderWise":
                // if (this.fromLoc !== undefined)
                //     this.rptParams +=
                //         encodeURIComponent("" + this.fromLoc) + "$";
                // if (this.toLoc !== undefined)
                //     this.rptParams += encodeURIComponent("" + this.toLoc) + "$";
                this.rptParams += locstr.replace(/^,|,$/g, '') + "$";
                this.rptParams += locstr.replace(/^,|,$/g, '') + "$";
                if (this.fromDate !== undefined)
                    this.rptParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    this.rptParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                if (this.fromItem !== undefined)
                    this.rptParams +=
                        encodeURIComponent("" + this.fromItem) + "$";
                if (this.toItem !== undefined)
                    this.rptParams +=
                        encodeURIComponent("" + this.toItem) + "$";

                this.rptParams = this.rptParams.replace(/[?$]&/, "");
                break;
        }
        this.reportView.show(report, this.rptParams);
    }

    openModal(type) {
        this.type = type;
        if (this.type == "fromLoc" || this.type == "toLoc")
            this.LocLookupTableModal.show();
        else if (this.type == "fromItem" || this.type == "toItem")
            this.ItemLookupTableModal.show();
    }
    getLookUpData() {
        if (this.type == "fromLoc") {
            this.fromLoc = this.LocLookupTableModal.data.locID;
            this.fromLocName = this.LocLookupTableModal.data.locName;
        } else if (this.type == "toLoc") {
            this.toLoc = this.LocLookupTableModal.data.locID;
            this.toLocName = this.LocLookupTableModal.data.locName;
        } else if (this.type == "fromItem") {
            this.fromItem = this.ItemLookupTableModal.data.itemId;
            this.fromItemName = this.ItemLookupTableModal.data.descp;
        } else if (this.type == "toItem") {
            this.toItem = this.ItemLookupTableModal.data.itemId;
            this.toItemName = this.ItemLookupTableModal.data.descp;
        }
    }

    setIdNull(type) {
        this.type = type;
        if (this.type == "fromItem") {
            this.fromItem = 0;
            this.fromItemName = "";
        } else if (this.type == "toItem") {
            this.toItem = "99-999-99-9999";
            this.toItemName = "";
        }
        this.fromLoc = 0;
        this.toLoc = "999999";
        this.fromLocName = "";
        this.toLocName = "";
    }
}
