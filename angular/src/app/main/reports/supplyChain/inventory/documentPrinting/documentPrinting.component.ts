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
import { ItemPricingLookupTableModalComponent } from "@app/main/supplyChain/inventory/FinderModals/itemPricing-lookup-table-modal.component";
import { ItemLookupTableModalComponent } from "@app/main/reports/item-lookup-finder/item-lookup-table-modal.component";
import { LocLookupTableModalComponent } from "@app/main/reports/loc-lookup-finder/loc-lookup-table-modal.component";
import { ReportFilterServiceProxy } from "@shared/service-proxies/service-proxies";
import { FiscalDateService } from "@app/shared/services/fiscalDate.service";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";
import { InventoryLookupTableModalComponent } from "@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component";
import { GetDataService } from '../../../../supplyChain/inventory/shared/services/get-data.service';
import { IDropdownSettings, } from 'ng-multiselect-dropdown';
@Component({
    templateUrl: "./documentPrinting.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class DocumentPrintingComponent extends AppComponentBase {
    @ViewChild("ItemLookupTableModal", { static: true })
    ItemLookupTableModal: ItemLookupTableModalComponent;
    @ViewChild("LocLookupTableModal", { static: true })
    LocLookupTableModal: LocLookupTableModalComponent;
    @ViewChild("inventoryLookupTableModal", { static: true })
    inventoryLookupTableModal: InventoryLookupTableModalComponent;
    @ViewChild("reportView", { static: true })
    reportView: ReportviewrModalComponent;
    locIdlist:any;   locationList:any;
    dropdownSettings:IDropdownSettings={};
    rptObj: any;
    rptParams: string = "";
    status: string = "";
    
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
    showOrderCost:any;
    OrderCost:string;
    priceList: string;
    priceListName: any;
    fromDate: any;
    toDate: any;
    exfromDate: any;
    extoDate: any;
    fromDoc: any;
    toDoc: any;
    fromItemName: any;
    toItemName: any;
    fromLoc: any;
    toLoc: any;
    fromLocName: any;
    toLocName: any;
    quantitative: any;
    showExFromDate:any;

    showToDate: boolean = true;
    showFromDate: boolean = true;
    showToDoc: boolean = true;
    showFromDoc: boolean = true;
    showToLoc: boolean;
    showFromLoc: boolean = true;
    showToItem: boolean;
    showFromItem: boolean;
    showPriceList: boolean;

    showReport: boolean;
    opening: boolean;
    transfer: boolean;
    consumption: boolean;
    adjustment: boolean;
    igp: boolean;
    ogp: boolean;
    assembly: boolean;

    type: string = "";
    filterText = "";
    id: number;
    sorting: any;
    skipCount: any;
    MaxResultCount: any;
    data: any;
    dxrep = false;
    target: any;

    constructor(
        injector: Injector,
        private route: Router,
        private _reportService: FiscalDateService,private _getDataService: GetDataService
    ) {
        super(injector);
    }

    ngOnInit() {
        this.toDate = new Date();
        this.extoDate = new Date();
        this._reportService.getDate().subscribe(data => {
            this.fromDate = new Date(data["result"]);
            this.exfromDate = new Date(data["result"]);
        });
        this.fromDoc = 0;
        this.toDoc = 99999;
        this.fromLoc = "0";
        this.toLoc = "99999";
        this.fromItem = "0";
        this.toItem = "99-999-99-9999";
        this.itemLedger ="opening" ;
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
                this.showFromDoc = true;
                this.showToDoc = true;
                this.showFromLoc = false;
                this.showToLoc = false;
                this.showExFromDate=false;
                this.showFromItem = false;
                this.showToItem = false;
                this.showFromLoc = false;
                this.showToLoc = false;
                this.showPriceList = false;
                this.showOrderCost=false;
                break;
            case "consumptionReportItemWise":
                this.showFromDate = true;
                this.showToDate = true;
                this.showFromDoc = false;
                this.showToDoc = false;
                this.showFromLoc = false;
                this.showExFromDate=false;
                this.showToLoc = false;
                this.showFromItem = true;
                this.showToItem = true;
                this.showPriceList = false;
                this.showOrderCost=false;
                break;
            case "consumptionReportAccWise":
            case "consumptionReportOrderWise":
                this.showFromDate = true;
                this.showToDate = true;
                this.showExFromDate=false;
                this.showFromDoc = false;
                this.showToDoc = false;
                this.showFromLoc = false;
                this.showToLoc = false;
                this.showFromItem = false;
                this.showToItem = false;
                this.showOrderCost=false;
                this.showPriceList = false;
                break;
            case "consumptionSummaryDepartmentWise":
            case "consumptionDepartmentWise":
                this.showFromDate = false;
                this.showToDate = false;
                this.showFromDoc = false;
                this.showExFromDate=false;
                this.showToDoc = false;
                this.showFromLoc = true;
                this.showToLoc = true;
                this.showOrderCost=false;
                this.showFromItem = false;
                this.showToItem = false;
                this.showPriceList = false;
                break;
            case "opening":
            case "adjustment":
            case "consumption":
                this.showFromDate = true;
                this.showToDate = true;
                this.showFromDoc = true;
                this.showToDoc = true;
                this.showExFromDate=false;
                this.showFromLoc = true;
                this.showOrderCost=false;
                this.showToLoc = true;
                this.showFromItem = false;
                this.showToItem = false;
                this.showPriceList = false;
                break;
            case "transfer":
            case "Grouprequisition":
            case "inwardGatePass":
                this.showFromDate = false;
                this.showToDate = false;
                this.showFromDoc = true;
                this.showToDoc = true;
                this.showFromLoc = false;
                this.showOrderCost=false;
                this.showToLoc = false;
                this.showFromItem = false;
                this.showExFromDate=false;
                this.showToItem = false;
                this.showPriceList = false;
                break;
            case "outwardGatePass":
                this.showFromDate = false;
                this.showToDate = false;
                this.showFromDoc = true;
                this.showToDoc = true;
                this.showOrderCost=false;
                this.showFromLoc = false;
                this.showToLoc = false;
                this.showFromItem = false;
                this.showToItem = false;
                this.showPriceList = false;
                this.showExFromDate=false;
                break;
            case "assembly":
                this.showFromDate = true;
                this.showToDate = true;
                this.showFromDoc = false;
                this.showToDoc = false;
                this.showFromLoc = true;
                this.showExFromDate=false;
                this.showOrderCost=false;
                this.showToLoc = true;
                this.showFromItem = true;
                this.showToItem = true;
                this.showPriceList = false;
                break;
            case "assemblyCost":
                this.showFromDate = false;
                this.showToDate = false;
                this.showFromDoc = false;
                this.showToDoc = false;
                this.showFromLoc = false;
                this.showToLoc = false;
                this.showFromItem = false;
                this.showExFromDate=false;
                this.showToItem = false;
                this.showPriceList = false;
                this.showOrderCost=true;
                break;

            case "requisition":
                this.showFromDate = false;
                this.showExFromDate=false;
                this.showToDate = false;
                this.showFromDoc = true;
                this.showToDoc = true;
                this.showFromLoc = false;
                this.showToLoc = false;
                this.showFromItem = false;
                this.showOrderCost=false;
                this.showToItem = false;
                this.showPriceList = false;
                break;
            case "purchaseOrder":
            case "reqOrderStatus":
            case "receipt":
            case "receiptReturn":
                this.showFromDate = false;
                this.showToDate = false;
                this.showOrderCost=false;
                this.showFromDoc = true;
                this.showToDoc = true;
                this.showFromLoc = false;
                this.showExFromDate=false;
                this.showToLoc = false;
                this.showFromItem = false;
                this.showToItem = false;
                this.showPriceList = false;
                break;
            case "purchaseOrderStatus":
                this.showFromDate = true;
                this.showToDate = true;
                this.showFromDoc = true;
                this.showOrderCost=false;
                this.showExFromDate=true;
                this.showToDoc = true;
                this.showFromLoc = false;
                this.showToLoc = false;
                this.showFromItem = false;
                this.showToItem = false;
                this.showPriceList = false;
                break;
            case "itemsPriceList":
                this.showFromDate = false;
                this.showExFromDate=false;
                this.showToDate = false;
                this.showFromDoc = false;
                this.showToDoc = false;
                this.showOrderCost=false;
                this.showFromLoc = false;
                this.showToLoc = false;
                this.showFromItem = true;
                this.showToItem = true;
                this.showPriceList = true;
                break;
            case "assetRegList":
                this.showFromDate = false;
                this.showToDate = false;
                this.showExFromDate=false;
                this.showFromDoc = false;
                this.showToDoc = false;
                this.showFromLoc = false;
                this.showToLoc = false;
                this.showOrderCost=false;
                this.showFromItem = false;
                this.showToItem = false;
                this.showPriceList = false;
                break
            case "assetRegReport":
                this.showFromDate = false;
                this.showToDate = false;
                this.showFromDoc = false;
                this.showToDoc = false;
                this.showOrderCost=false;
                this.showFromLoc = false;
                this.showToLoc = false;
                this.showExFromDate=false;
                this.showFromItem = false;
                this.showToItem = false;
                this.showPriceList = false;
                break
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
            case "opening":
                this.processReport("OpeningReport");
                break;
            case "transfer":
                this.processReport("StockTransfer");
                break;
            case "consumption":
                this.processReport("ConsumptionReport");
                break;
            case "adjustment":
                this.processReport("AdjustmentReport");
                break;
            case "assembly":
                this.processReport("AssemblyStock");
                break;
            case "assemblyCost":
                this.processReport("assemblyCost");
                break;
            case "inwardGatePass":
                this.processReport("InwardGatePass");
                break;
            case "outwardGatePass":
                this.processReport("OutwardGatePass");
                break;
            case "requisition":
                this.processReport("Requisition");
                break;
            case "Grouprequisition":
                this.processReport("Grouprequisition");
                break;
            case "purchaseOrder":
                this.processReport("PurchaseOrder");
                break;
            case "purchaseOrderStatus":
                this.processReport("PurchaseOrderStatus");
                break;
            case "reqOrderStatus":
                this.processReport("ReqOrderStatus");
                break;
            case "receipt":
                this.processReport("Receipt");
                break;
            case "receiptReturn":
                this.processReport("ReceiptReturn");
                break;
            case "itemsPriceList":
                this.processReport("ItemsPriceList");
                break;
            case "assetRegList":
                this.processReport("AssetRegListing");
                break;
            case "assetRegReport":
                this.processReport("AssetRegistrationReport");
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
       
        this.dxrep = false;
        this.rptParams = "";
        switch (report) {
            case "ConsumptionReport":
            case "OpeningReport":
            case "AdjustmentReport":
                // this.rptObj = {
                //     "fromDate": new Date(this.fromDate).toLocaleDateString(),
                //     "toDate": new Date(this.toDate).toLocaleDateString(),
                //     "fromDoc": this.fromDoc,
                //     "toDoc": this.toDoc,
                //     "fromLoc": this.fromLoc,
                //     "toLoc": this.toLoc,
                //     "tenantId": this.appSession.tenant.id
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
                //     this.rptParams +=
                //         encodeURIComponent("" + this.fromLoc) + "$";
                // if (this.toLoc !== undefined)
                //     this.rptParams += encodeURIComponent("" + this.toLoc) + "$";

                this.rptParams += locstr.replace(/^,|,$/g, '') + "$";
                this.rptParams += locstr.replace(/^,|,$/g, '') + "$";

                this.rptParams += encodeURIComponent("" + this.permission.isGranted('Purchase.ReceiptEntry.ShowAmounts')) + "$";
                this.rptParams = this.rptParams.replace(/[?$]&/, "");
                this.dxrep = true;
                break;
            case "AssemblyStock":

              
                if (this.fromDate !== undefined)
                    this.rptParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    this.rptParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";

                 this.rptParams += locstr.replace(/^,|,$/g, '') + "$";
                 this.rptParams += locstr.replace(/^,|,$/g, '') + "$";
                // if (this.fromLoc !== undefined)
                //     this.rptParams +=
                //         encodeURIComponent("" + this.fromLoc) + "$";
                // if (this.toLoc !== undefined)
                //     this.rptParams += encodeURIComponent("" + this.toLoc) + "$";
                if (this.fromItem !== undefined)
                    this.rptParams +=
                        encodeURIComponent("" + this.fromItem) + "$";
                if (this.toItem !== undefined)
                    this.rptParams +=
                        encodeURIComponent("" + this.toItem) + "$";

                this.rptParams = this.rptParams.replace(/[?$]&/, "");
                this.dxrep = true;
                break;
            case "assemblyCost":
        
                if (this.OrderCost !== undefined)
                debugger
               // this.OrderCost=this.OrderCost.replace("#","%");
                    this.rptParams +=
                        encodeURIComponent("" + this.OrderCost) + "$";
                // if (this.toLoc !== undefined)
                //     this.rptParams += encodeURIComponent("" + this.toLoc) + "$";
                // if (this.fromItem !== undefined)
                //     this.rptParams +=
                //         encodeURIComponent("" + this.fromItem) + "$";
                // if (this.toItem !== undefined)
                //     this.rptParams +=
                //         encodeURIComponent("" + this.toItem) + "$"; 
                this.rptParams = this.rptParams.replace(/[?$]&/, "");
                this.dxrep = true;
                break;
                
           case "StockTransfer":
            case "InwardGatePass":
            case "Requisition":
            case "Grouprequisition":
          
            case "ReqOrderStatus":
            case "Receipt":
            case "ReceiptReturn":
                // this.rptObj = {
                //     "FromDoc": this.fromDoc,
                //     "ToDoc": this.toDoc,
                //     "TenantId": this.appSession.tenant.id,
                // };
                if (this.fromDoc !== undefined)
                    this.rptParams += "" + this.fromDoc + "$";
                if (this.toDoc !== undefined)
                    this.rptParams += "" + this.toDoc + "$";
                this.rptParams += encodeURIComponent("" + this.permission.isGranted('Purchase.ReceiptEntry.ShowAmounts')) + "$";
                this.rptParams = this.rptParams.replace(/[?$]&/, "");
                this.dxrep = true;
                break;
            case "PurchaseOrder":

                if (this.fromDoc !== undefined)
                this.rptParams += "" + this.fromDoc + "$";
            if (this.toDoc !== undefined)
                this.rptParams += "" + this.toDoc + "$";
            if (this.exfromDate !== undefined)
                this.rptParams +=
                    "" + moment(this.exfromDate).format("YYYY/MM/DD") + "$";
            if (this.extoDate !== undefined)
                this.rptParams +=
                    "" + moment(this.extoDate).format("YYYY/MM/DD") + "$";

            this.rptParams += encodeURIComponent("" + this.permission.isGranted('Purchase.ReceiptEntry.ShowAmounts')) + "$";
            this.rptParams = this.rptParams.replace(/[?$]&/, "");
            this.dxrep = true;
            break;

            case "PurchaseOrderStatus":
                if (this.fromDoc !== undefined)
                    this.rptParams += "" + this.fromDoc + "$";
                if (this.toDoc !== undefined)
                    this.rptParams += "" + this.toDoc + "$";
                if (this.fromDate !== undefined)
                    this.rptParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    this.rptParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                if (this.exfromDate !== undefined)
                        this.rptParams +=
                            "" + moment(this.exfromDate).format("YYYY/MM/DD") + "$";
                if (this.extoDate !== undefined)
                        this.rptParams +=
                            "" + moment(this.extoDate).format("YYYY/MM/DD") + "$";

                this.rptParams = this.rptParams.replace(/[?$]&/, "");
                this.dxrep = true;
                break;
            case "OutwardGatePass":
                // this.rptObj = {
                //     "FromDoc": this.fromDoc,
                //     "ToDoc": this.toDoc,
                //     "TenantId": this.appSession.tenant.id,
                // };
                if (this.fromDoc !== undefined)
                    this.rptParams += "" + this.fromDoc + "$";
                if (this.toDoc !== undefined)
                    this.rptParams += "" + this.toDoc + "$";

                this.rptParams = this.rptParams.replace(/[?$]&/, "");
                this.dxrep = true;
                break;
            case "ItemsPriceList":
                if (this.priceList !== undefined)
                    this.rptParams +=
                        encodeURIComponent("" + this.priceList) + "$";
                if (this.fromItem !== undefined)
                    this.rptParams +=
                        encodeURIComponent("" + this.fromItem) + "$";
                if (this.toItem !== undefined)
                    this.rptParams +=
                        encodeURIComponent("" + this.toItem) + "$";
                this.rptParams = this.rptParams.replace(/[?$]&/, "");
                this.dxrep = true;
                break;
            case "AssetRegListing":
                this.dxrep=true;
                break;
            case "AssetRegistrationReport":
                this.dxrep=true;
                break;
        }

        if (this.dxrep) {
            this.reportView.show(report, this.rptParams);
        } else {
            localStorage.setItem("rptObj", JSON.stringify(this.rptObj));
            localStorage.setItem("rptName", report);

            this.route.navigateByUrl("/app/main/reports/ReportView");
        }
    }
    openPriceListModal() {
        this.target = "PriceList";
        this.inventoryLookupTableModal.id = this.priceList;
        this.inventoryLookupTableModal.displayName = this.priceListName;
        this.inventoryLookupTableModal.show(this.target);
    }
    getInventoryModal() {
        switch (this.target) {
            case "PriceList":
                this.getPriceList();
                break;
            default:
                break;
        }
    }
    getPriceList() {
        debugger;
        this.priceList = this.inventoryLookupTableModal.id;
        this.priceList = this.priceList.trim();
        this.priceListName = this.inventoryLookupTableModal.displayName;
    }
    setPriceListNull() {
        this.priceList = "";
        this.priceListName = "";
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
        this.fromLoc = 0;
        this.toLoc = "999999";
        this.fromLocName = "";
        this.toLocName = "";
    }
}
