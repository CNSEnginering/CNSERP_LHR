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
import { ReportFilterServiceProxy } from "@shared/service-proxies/service-proxies";
import { FiscalDateService } from "@app/shared/services/fiscalDate.service";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";
import { InventoryLookupTableModalComponent } from "@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component";
import { GetDataService } from '../../../../supplyChain/inventory/shared/services/get-data.service';
import { IDropdownSettings, } from 'ng-multiselect-dropdown';
@Component({
    templateUrl: "./activityReports.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ActivityReportsComponent extends AppComponentBase {
    @ViewChild("ItemLookupTableModal", { static: true })
    ItemLookupTableModal: ItemLookupTableModalComponent;
    @ViewChild("LocLookupTableModal", { static: true })
    LocLookupTableModal: LocLookupTableModalComponent;
    @ViewChild("reportView", { static: true })
    reportView: ReportviewrModalComponent;
    @ViewChild("inventoryLookupTableModal", { static: true })
    inventoryLookupTableModal: InventoryLookupTableModalComponent;
    rptObj: any;
    status: string = "";
    showReport: boolean = false;
    opening: boolean = false;
    transfer: boolean = false;
    consumption: boolean = false;
    adjustment: boolean = false;
    igp: boolean = false;
    ogp: boolean = false;
    locIdlist:any;    locationList:any;
    dropdownSettings:IDropdownSettings={};
    assembly: boolean = false;
    itemLedger: string;
    reportServer: string;
    reportUrl: string;
    showParameters: string;
    parameters: any;
    language: string;
    width: number;
    height: number;
    toolbar: string;
    location: number;
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
    quantitative: any;

    showToDate: boolean = true;
    showFromDate: boolean = true;
    showToDoc: boolean = true;
    showFromDoc: boolean = true;
    showToLoc: boolean = true;
    showFromLoc: boolean = true;
    showToItem: boolean = true;
    showFromItem: boolean = true;
    showCostCenter: boolean;
    type: string = "";
    filterText = "";
    id: number;
    priceList: any;
    sorting: any;
    skipCount: any;
    MaxResultCount: any;
    data: any;
    dxrep = false;
    rptParams: string;
    costCenter: string;
    costCenterName: string;

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
        // var toDate = moment().format("MM/DD/YYYY");
        // this.toDate = toDate;
        // this._reportService.getDate().subscribe(
        //     data => {
        //         var fromDate = moment(data["result"]).format("MM/DD/YYYY");
        //         console.log(data["result"])
        //         console.log(fromDate)
        //         this.fromDate = fromDate;
        //     }
        // )
        // this.toDate = moment().format("MM/DD/YYYY");
        // this.fromDate = moment().format("MM/DD/YYYY");
        this.fromDoc = 0;
        this.toDoc = 99999;
        this.fromLoc = "0";
        this.toLoc = "99999";
        this.fromItem = "0";
        this.toItem = "99-999-99-9999";
        this.costCenter = "0";
        this.itemLedger="transfer";
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
            case "assembly":
            case "gatePass":
                this.showFromDate = true;
                this.showToDate = true;
                this.showFromDoc = true;
                this.showToDoc = true;
                this.showFromLoc = false;
                this.showToLoc = false;
                this.showCostCenter = false;
                break;
            case "transfer":
                this.showFromDate = true;
                this.showToDate = true;
                this.showFromDoc = true;
                this.showToDoc = true;
                this.showFromLoc = true;
                this.showToLoc = true;
                this.showCostCenter = false;
                break;
            case "consumptionCostWise":
                this.showCostCenter = true;
                this.showFromDate = true;
                this.showToDate = true;
                this.showFromDoc = true;
                this.showToDoc = true;
                this.showFromLoc = true;
                this.showToLoc = true;
                break;
        }
    }
    getReport() {
        switch (this.itemLedger) {
            case "transfer":
                this.processReport("TransferRegister");
                break;
            case "assembly":
                this.processReport("AssemblyRegister");
                break;
            case "InwardGatePassRegister":
                this.processReport("InwardGatePassRegister");
                break;
            case "OutwardGatePassRegister":
                this.processReport("OutwardGatePassRegister");
                break;
            case "opening":
                this.processReport("OpeningActivityReport");
                break;
            case "consumption":
                this.processReport("ConsumptionActivityReport");
                break;
            case "consumptionCostWise":
                this.processReport("ConsumptionActivityReportCostWise");
                break;
            case "adjustment":
                this.processReport("AdjustmentActivityReport");
                break;
        }
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
        this.dxrep = false;
        this.rptParams = "";
        switch (report) {
            case "AssemblyRegister":
            case "InwardGatePassRegister":
            case "OutwardGatePassRegister":
                // this.rptObj = {
                //     "FromDate": new Date(this.fromDate).toLocaleDateString(),
                //     "ToDate": new Date(this.toDate).toLocaleDateString(),
                //     "FromDoc": this.fromDoc,
                //     "ToDoc": this.toDoc,
                //     "TenantId": this.appSession.tenant.id,
                // };
                if (this.fromDate !== undefined)
                    this.rptParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    this.rptParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                if (this.fromDoc !== undefined)
                    this.rptParams +=
                        encodeURIComponent("" + this.fromDoc) + "$";
                if (this.toDoc !== undefined)
                    this.rptParams += encodeURIComponent("" + this.toDoc) + "$";

                this.rptParams = this.rptParams.replace(/[?$]&/, "");
                this.dxrep = true;

                break;
            case "TransferRegister":
                if (this.fromDate !== undefined)
                    this.rptParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    this.rptParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                if (this.fromDoc !== undefined)
                    this.rptParams +=
                        encodeURIComponent("" + this.fromDoc) + "$";
                if (this.toDoc !== undefined)
                    this.rptParams += encodeURIComponent("" + this.toDoc) + "$";
                // if (this.fromLoc !== undefined)
                //     this.rptParams +=
                //         encodeURIComponent("" + this.fromLoc) + "$";
                // if (this.toLoc !== undefined)
                //     this.rptParams += encodeURIComponent("" + this.toLoc) + "$";
                this.rptParams += locstr.replace(/^,|,$/g, '') + "$";
                this.rptParams += locstr.replace(/^,|,$/g, '') + "$";

                this.rptParams = this.rptParams.replace(/[?$]&/, "");
                this.dxrep = true;

                break;
            case "ConsumptionActivityReportCostWise":
                debugger;
                if (this.fromDate !== undefined)
                    this.rptParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    this.rptParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                if (this.fromDoc !== undefined)
                    this.rptParams +=
                        encodeURIComponent("" + this.fromDoc) + "$";
                if (this.toDoc !== undefined)
                    this.rptParams += encodeURIComponent("" + this.toDoc) + "$";
                // if (this.fromLoc !== undefined)
                //     this.rptParams += encodeURIComponent("" + 0) + "$";
                // if (this.toLoc !== undefined)
                //     this.rptParams += encodeURIComponent("" + 99999) + "$";
                this.rptParams += locstr.replace(/^,|,$/g, '') + "$";
                this.rptParams += locstr.replace(/^,|,$/g, '') + "$";

                if (this.costCenter !== undefined)
                    this.rptParams += "" + this.costCenter;

                this.rptParams = this.rptParams.replace(/[?$]&/, "");
                this.dxrep = true;

                break;
            case "OpeningActivityReport":
            case "ConsumptionActivityReport":
            case "AdjustmentActivityReport":
                // this.rptObj = {
                //     "fromDate": new Date(this.fromDate).toLocaleDateString(),
                //     "toDate": new Date(this.toDate).toLocaleDateString(),
                //     "fromDoc": this.fromDoc,
                //     "toDoc": this.toDoc,
                //     "fromLoc": "0",
                //     "toLoc": "999999",
                //     "tenantId": this.appSession.tenant.id,
                // };

                //for devexpress
                if (this.fromDate !== undefined)
                    this.rptParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    this.rptParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                if (this.fromDoc !== undefined)
                    this.rptParams +=
                        encodeURIComponent("" + this.fromDoc) + "$";
                if (this.toDoc !== undefined)
                    this.rptParams += encodeURIComponent("" + this.toDoc) + "$";
                // if (this.fromLoc !== undefined)
                //     this.rptParams += encodeURIComponent("" + 0) + "$";
                // if (this.toLoc !== undefined)
                //     this.rptParams += encodeURIComponent("" + 99999) + "$";
                this.rptParams += locstr.replace(/^,|,$/g, '') + "$";
                this.rptParams += locstr.replace(/^,|,$/g, '') + "$";

                    if (this.fromItem !== undefined)
                    this.rptParams += encodeURIComponent("" + this.fromItem) + "$";
                    if (this.toItem !== undefined)
                    this.rptParams += encodeURIComponent("" + this.toItem) + "$";

                this.rptParams = this.rptParams.replace(/[?$]&/, "");
                this.dxrep = true;

                break;
        }
        if (this.dxrep) {
            debugger;
            this.reportView.show(report, this.rptParams);
        } else {
            localStorage.setItem("rptObj", JSON.stringify(this.rptObj));
            localStorage.setItem("rptName", report);

            this.route.navigateByUrl("/app/main/reports/ReportView");
        }
    }
    setIdNull(type) {
        this.type = type;
        this.fromLoc = 0;
        this.toLoc = "999999";
        this.fromLocName = "";
        this.toLocName = "";
    }

    openModal(type) {
        debugger;
        this.type = type;
        // if (this.type == "fromItem" || this.type == "toItem")
        //     this.ItemLookupTableModal.show();
        // else
        this.inventoryLookupTableModal.id = "";
        this.inventoryLookupTableModal.displayName = "";
        if (this.type == "fromLoc" || this.type == "toLoc")
            this.inventoryLookupTableModal.show("Location");

        if (this.type == "costCenter")
            this.inventoryLookupTableModal.show("CostCenter");
            
        if (this.type == "fromItem" || this.type == "toItem")
            this.ItemLookupTableModal.show();
    }

    getNewInventoryModal() {
        if (this.type == "fromLoc") {
            this.fromLoc = this.inventoryLookupTableModal.id;
            this.fromLocName = this.inventoryLookupTableModal.displayName;
        } else if (this.type == "toLoc") {
            this.toLoc = this.inventoryLookupTableModal.id;
            this.toLocName = this.inventoryLookupTableModal.displayName;
        } else if (this.type == "costCenter") {
            this.costCenter = this.inventoryLookupTableModal.id;
            this.costCenterName = this.inventoryLookupTableModal.displayName;
        }
    }

    getLookUpData()
    {
        if (this.type == "fromItem") { 
            this.fromItem = this.ItemLookupTableModal.data.itemId;
            this.fromItemName = this.ItemLookupTableModal.data.descp;
        } else if (this.type == "toItem") {
            this.toItem = this.ItemLookupTableModal.data.itemId;
            this.toItemName = this.ItemLookupTableModal.data.descp;
        }

    }
}
