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
import { FiscalDateService } from "@app/shared/services/fiscalDate.service";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";
import { InventoryLookupTableModalComponent } from "@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component";
import { IDropdownSettings, } from 'ng-multiselect-dropdown';
import { GetDataService } from '../../../../supplyChain/inventory/shared/services/get-data.service';
import {
    TokenAuthServiceProxy,
    VoucherEntryServiceProxy
} from "@shared/service-proxies/service-proxies";
import { ViewCprModalComponent } from "@app/main/commonServices/cprNumbers/view-cpr-modal.component";
@Component({
    templateUrl: "./itemLedger.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ItemLedgerComponent extends AppComponentBase {
    @ViewChild("ItemLookupTableModal", { static: true })
    ItemLookupTableModal: ItemLookupTableModalComponent;
    @ViewChild("LocLookupTableModal", { static: true })
    LocLookupTableModal: LocLookupTableModalComponent;
    @ViewChild("reportView", { static: true })
    reportView: ReportviewrModalComponent;
    @ViewChild("inventoryLookupTableModal", { static: true })
    inventoryLookupTableModal: InventoryLookupTableModalComponent;
    dropdownSettings:IDropdownSettings={};
    rptObj: any;
    status: string = "";
    showReport: boolean = false;
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
    target:any;
    itemlist:any=false;
    standard:any=true;
    //type:any;
    fromDate: any;
    toDate: any;
    locIdlist: any;
    itemTypeid:any;
    fromItem: any;
    fromItemName: any;
    toItem: any;
    Fromseg3s:boolean;
    toItemName: any;
    fromLoc: any;
    toLoc: any;
    fromLocName: any;
    toLocName: any;
    quantitative: any;
    toseg3Name:any;
    toseg3:any=999;
    fromseg3:any=0;
    fromseg3Name:any;
    locationList:any;
    type: string = "";
    filterText = "";
    id: number;
    itemType:any;
    priceList: any;
    sorting: any;
    skipCount: any;
    MaxResultCount: any;
    data: any;
    orderBy: any;
    ftdate:any;
    dtloc:any;

    constructor(
        injector: Injector,
        private route: Router, private _getDataService: GetDataService,
        private _reportService: FiscalDateService
    ) {
        super(injector);
    }

    ngOnInit() {
        this._reportService.getDate().subscribe(data => {
            this.fromDate = new Date(data["result"]);
            this.toDate = new Date();
        });
        //this.toDate = moment().format("MM/DD/YYYY");
        //this.fromDate = moment().format("dd/MM/yyyy");
        this.fromItem = "0";
        this.toItem = "99-999-99-9999";
        this.fromLoc = "0";
        this.toLoc = "999";
        this.orderBy = "docNo";
        this.itemLedger = "itemLedgerDetail";
        this.getLocationList();
        this.dropdownSettings = {
            idField: 'id',
            textField: 'displayName',
            enableCheckAll: true, allowSearchFilter: true,
            selectAllText: "Select All ",
            unSelectAllText: "UnSelect All ",
            itemsShowLimit: 0,singleSelection: false,
          };

          this.itemType = [
            { id: 1, displayName: 'Manufactured' },
            { id: 2, displayName: 'Purchased' },
            { id: 3, displayName: 'Service' },
            { id: 4, displayName: 'Stock' },
            { id: 5, displayName: 'Non Stock' }
          ];
          this.hideFields();
    }

    getReport() {
        debugger;
        switch (this.itemLedger) {
            case "itemLedgerDetail":
                this.processReport("ItemLedgerDetail");
                //console.log(this.itemLedger);
                break;
            // case 'itemLedgerDetailNew':
            //     this.processReport("ItemLedgerDetailNew");
            //     //console.log(this.itemLedger);
            //     break;
            case "itemLedgerQuantitative":
                this.processReport("ItemLedgerQuantitative");
                //console.log(this.itemLedger);
                break;
            case "itemListing":
                this.processReport("ItemListing");
                //console.log(this.itemLedger);
                break;
            case "segment2":
                this.processReport("ItemStockSegment2");
                //console.log(this.itemLedger);
                break;
            case "segment3":
                    this.processReport("ItemStockSegment3");
                    break;
            case "segment2Summary":
                this.processReport("ItemStockSegmentSummary");
                //console.log(this.itemLedger);
                break;
            // case 'itemStock':
            //   this.processReport("ItemStock");
            //   //console.log(this.itemLedger);
            //   break;
            case "quantitativeStock":
                this.processReport("QuantitativeStock");
                //console.log(this.itemLedger);
                break;
            case "quantitativeStock3":
                this.processReport("QuantitativeStock3");
                    //console.log(this.itemLedger);
                 break;
            case "stockReportDetail":
                debugger
                this.processReport("StockReportDetail");
                //console.log(this.itemLedger);
                break;
            case "stockReportQuantitative":
                this.processReport("StockReportQuantitative");
                //console.log(this.itemLedger);
                break;
            case "StocklevelWise":
                this.processReport("StocklevelWise");
                    //console.log(this.itemLedger);
                break;
                
            case "ConsolidatstockReport":
                this.processReport("ConsolidatstockReport");
                    //console.log(this.itemLedger);
                break;
            case "itemStatus":
                    this.processReport("itemStatus");
                        //console.log(this.itemLedger);
                break;
        }
    }
    processReport(report: string) {
        var locstr="";
        var itemstr="";
        console.log(this.locIdlist);
 
        if(this.locIdlist!=undefined){
            this.locIdlist.forEach(element => {
                debugger
               locstr= locstr+element.id+",";
            });
        }
        if(this.itemTypeid!=undefined){
            this.itemTypeid.forEach(element => {
                debugger
                itemstr= itemstr+element.id+",";
            });
        }
        let dxrep = false;
        let repParams = "";
        switch (report) {
            case "ItemLedgerDetail":
                // this.rptObj = {
                //   "FromDate": new Date(this.fromDate).toLocaleDateString(),
                //   "ToDate": new Date(this.toDate).toLocaleDateString(),
                //   "FromLocId": this.fromLoc,
                //   "ToLocId": this.toLoc,
                //   "TenantId": this.appSession.tenant.id,
                //   "FromItem": this.fromItem,
                //   "ToItem": this.toItem
                // };
                // break;
                // case 'ItemLedgerDetailNew':
                report = "ItemLedgerDetail";
                if (this.fromDate !== undefined)
                    repParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    repParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                // if (this.fromLoc !== undefined)
                //     repParams += encodeURIComponent("" + this.fromLoc) + "$";
                // if (this.toLoc !== undefined)
                //     repParams += encodeURIComponent("" + this.toLoc) + "$";
                //if (locstr !== "")
                     repParams += locstr.replace(/^,|,$/g, '') + "$";
                if (this.fromItem !== undefined)
                    repParams += encodeURIComponent("" + this.fromItem) + "$";
                if (this.toItem !== undefined)
                    repParams += encodeURIComponent("" + this.toItem) + "$";
                // if (this.orderBy !== undefined)
                //     repParams += encodeURIComponent("" + this.orderBy) + "$";

                repParams = repParams.replace(/[?$]&/, "");
                dxrep = true;
                break;
            case "itemStatus":
                    // this.rptObj = {
                    //   "FromDate": new Date(this.fromDate).toLocaleDateString(),
                    //   "ToDate": new Date(this.toDate).toLocaleDateString(),
                    //   "FromLocId": this.fromLoc,
                    //   "ToLocId": this.toLoc,
                    //   "TenantId": this.appSession.tenant.id,
                    //   "FromItem": this.fromItem,
                    //   "ToItem": this.toItem
                    // };
                    // break;
                    // case 'ItemLedgerDetailNew':
                    report = "itemStatus";
                    if (this.fromDate !== undefined)
                        repParams +=
                            "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                    if (this.toDate !== undefined)
                        repParams +=
                            "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                    // if (this.fromLoc !== undefined)
                    //     repParams += encodeURIComponent("" + this.fromLoc) + "$";
                    // if (this.toLoc !== undefined)
                    //     repParams += encodeURIComponent("" + this.toLoc) + "$";
                    repParams += locstr.replace(/^,|,$/g, '') + "$";
                    if (this.fromItem !== undefined)
                        repParams += encodeURIComponent("" + this.fromItem) + "$";
                    if (this.toItem !== undefined)
                        repParams += encodeURIComponent("" + this.toItem) + "$";
                    if (this.fromseg3 !== undefined)
                        repParams += encodeURIComponent("" + this.fromseg3) + "$";
                    if (this.toseg3 !== undefined)
                        repParams += encodeURIComponent("" + this.toseg3) + "$";
                    // if (this.orderBy !== undefined)
                    //     repParams += encodeURIComponent("" + this.orderBy) + "$";
    
                    repParams = repParams.replace(/[?$]&/, "");
                    dxrep = true;
                    break;
            case "ItemLedgerQuantitative":
                // this.rptObj = {
                //   "FromDate": new Date(this.fromDate).toLocaleDateString(),
                //   "ToDate": new Date(this.toDate).toLocaleDateString(),
                //   "FromLocId": this.fromLoc,
                //   "ToLocId": this.toLoc,
                //   "TenantId": this.appSession.tenant.id,
                //   "FromItem": this.fromItem,
                //   "ToItem": this.toItem
                // };
                if (this.fromDate !== undefined)
                    repParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    repParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                // if (this.fromLoc !== undefined)
                //     repParams += encodeURIComponent("" + this.fromLoc) + "$";
                // if (this.toLoc !== undefined)
                //     repParams += encodeURIComponent("" + this.toLoc) + "$";
                repParams += locstr.replace(/^,|,$/g, '') + "$";
                repParams += locstr.replace(/^,|,$/g, '') + "$";
                if (this.fromItem !== undefined)
                    repParams += encodeURIComponent("" + this.fromItem) + "$";
                if (this.toItem !== undefined)
                    repParams += encodeURIComponent("" + this.toItem) + "$";
                if (this.orderBy !== undefined)
                    repParams += encodeURIComponent("" + this.orderBy) + "$";

                repParams = repParams.replace(/[?$]&/, "");
                dxrep = true;
                break;
            case "ItemListing":
                
                repParams += itemstr.replace(/^,|,$/g, '') + "$";
                this.rptObj = {
                    TenantId: this.appSession.tenant.id
                };
                repParams = repParams.replace(/[?$]&/, "");
                dxrep = true;

                break;
            // case 'ItemStock':
            //   this.rptObj = {
            //     "FromDate": new Date(this.fromDate).toLocaleDateString(),
            //     "ToDate": new Date(this.toDate).toLocaleDateString(),
            //     "FromLocId": this.fromLoc,
            //     "ToLocId": this.toLoc,
            //     "TenantId": this.appSession.tenant.id,
            //     "FromItem": this.fromItem,
            //     "ToItem": this.toItem
            //   };

            //   break;
            case "QuantitativeStock":
                // this.rptObj = {
                //   "FromLocId": this.fromLoc,
                //   "ToLocId": this.toLoc,
                //   "TenantId": this.appSession.tenant.id,
                //   "FromItem": this.fromItem,
                //   "ToItem": this.toItem
                // };

                // if (this.fromLoc !== undefined)
                //     repParams += encodeURIComponent("" + this.fromLoc) + "$";
                // if (this.toLoc !== undefined)
                //     repParams += encodeURIComponent("" + this.toLoc) + "$";
                repParams += locstr.replace(/^,|,$/g, '') + "$";
                repParams += locstr.replace(/^,|,$/g, '') + "$";
                if (this.fromItem !== undefined)
                    repParams += encodeURIComponent("" + this.fromItem) + "$";
                if (this.toItem !== undefined)
                    repParams += encodeURIComponent("" + this.toItem) + "$";

                repParams = repParams.replace(/[?$]&/, "");
                dxrep = true;
                break;
            case "QuantitativeStock3":
                    // this.rptObj = {
                    //   "FromLocId": this.fromLoc,
                    //   "ToLocId": this.toLoc,
                    //   "TenantId": this.appSession.tenant.id,
                    //   "FromItem": this.fromItem,
                    //   "ToItem": this.toItem
                    // };
    
            //    if (this.fromLoc !== undefined)
            //             repParams += encodeURIComponent("" + this.fromLoc) + "$";
            //      if (this.toLoc !== undefined)
            //             repParams += encodeURIComponent("" + this.toLoc) + "$";
            repParams += locstr.replace(/^,|,$/g, '') + "$";
            repParams += locstr.replace(/^,|,$/g, '') + "$";
                 if (this.fromItem !== undefined)
                        repParams += encodeURIComponent("" + this.fromItem) + "$";
                 if (this.toItem !== undefined)
                        repParams += encodeURIComponent("" + this.toItem) + "$";
    
                 repParams = repParams.replace(/[?$]&/, "");
                 dxrep = true;
                 break;
            case "StockReportDetail":
            case "ConsolidatstockReport":     
            case "StockReportQuantitative":
            case "StocklevelWise":
                if (this.fromDate !== undefined)
                    repParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    repParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                // if (this.fromLoc !== undefined)
                //     repParams += encodeURIComponent("" + this.fromLoc) + "$";
                // if (this.toLoc !== undefined)
                //     repParams += encodeURIComponent("" + this.toLoc) + "$";
                repParams += locstr.replace(/^,|,$/g, '') + "$";
                repParams += locstr.replace(/^,|,$/g, '') + "$";
                if (this.fromItem !== undefined)
                    repParams += encodeURIComponent("" + this.fromItem) + "$";
                if (this.toItem !== undefined)
                    repParams += encodeURIComponent("" + this.toItem) + "$";

                repParams = repParams.replace(/[?$]&/, "");
                dxrep = true;
                break;
            case "ItemStockSegment2":
            case"ItemStockSegment3":
                // this.rptObj = {
                //   "FromDate": new Date(this.fromDate).toLocaleDateString(),
                //   "ToDate": new Date(this.toDate).toLocaleDateString(),
                //   "FromLocId": this.fromLoc,
                //   "ToLocId": this.toLoc,
                //   "TenantId": this.appSession.tenant.id,
                //   "FromItem": this.fromItem,
                //   "ToItem": this.toItem
                // };
                if (this.fromDate !== undefined)
                    repParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    repParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                // if (this.fromLoc !== undefined)
                //     repParams += encodeURIComponent("" + this.fromLoc) + "$";
                // if (this.toLoc !== undefined)
                //     repParams += encodeURIComponent("" + this.toLoc) + "$";
                repParams += locstr.replace(/^,|,$/g, '') + "$";
                repParams += locstr.replace(/^,|,$/g, '') + "$";
                if (this.fromItem !== undefined)
                    repParams += encodeURIComponent("" + this.fromItem) + "$";
                if (this.toItem !== undefined)
                    repParams += encodeURIComponent("" + this.toItem) + "$";

                repParams = repParams.replace(/[?$]&/, "");
                dxrep = true;
                break;
            case "ItemStockSegmentSummary":
                // this.rptObj = {
                //   "FromDate": new Date(this.fromDate).toLocaleDateString(),
                //   "ToDate": new Date(this.toDate).toLocaleDateString(),
                //   "FromLocId": this.fromLoc,
                //   "ToLocId": this.toLoc,
                //   "TenantId": this.appSession.tenant.id,
                //   "FromItem": this.fromItem,
                //   "ToItem": this.toItem
                // };
                if (this.fromDate !== undefined)
                    repParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    repParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                // if (this.fromLoc !== undefined)
                //     repParams += encodeURIComponent("" + this.fromLoc) + "$";
                // if (this.toLoc !== undefined)
                //     repParams += encodeURIComponent("" + this.toLoc) + "$";
                repParams += locstr.replace(/^,|,$/g, '') + "$";
                repParams += locstr.replace(/^,|,$/g, '') + "$";
                if (this.fromItem !== undefined)
                    repParams += encodeURIComponent("" + this.fromItem) + "$";
                if (this.toItem !== undefined)
                    repParams += encodeURIComponent("" + this.toItem) + "$";

                repParams = repParams.replace(/[?$]&/, "");
                dxrep = true;
                break;
        }
        if (dxrep) {
            debugger;
            this.reportView.show(report, repParams);
        } else {
            localStorage.setItem("rptObj", JSON.stringify(this.rptObj));
            localStorage.setItem("rptName", report);

            this.route.navigateByUrl("/app/main/reports/ReportView");
        }
    }

    getLocationList(): void {
        this._getDataService.getList("ICLocations").subscribe(resultL => {
            this.locationList = resultL;
        });
        
    }

    openModal(type) {
        debugger;
        this.type = type;
        if (this.type == "fromItem" || this.type == "toItem")
            this.ItemLookupTableModal.show();
        else if (this.type == "fromLoc" || this.type == "toLoc")
            this.LocLookupTableModal.show();
        else if(this.type=="fromSeg" || this.type=="toseg3")
            this.openSelectICSegment3Modal();
    }
    openSelectICSegment3Modal() {
        this.target = "Segment3";
        this.inventoryLookupTableModal.id = this.fromseg3;
        this.inventoryLookupTableModal.displayName = this.fromseg3Name;
        this.inventoryLookupTableModal.show(this.target);

    }
    getNewICSegment3Id() {
        if(this.type=="fromSeg"){
            this.fromseg3= this.inventoryLookupTableModal.id;
            this.fromseg3Name = this.inventoryLookupTableModal.displayName;
        }
        else if(this.type=="toseg3"){
            this.toseg3= this.inventoryLookupTableModal.id;
            this.toseg3Name = this.inventoryLookupTableModal.displayName;
        }
    }
    getLookUpData() {
       
        if (this.type == "fromItem") {
            this.fromItem = this.ItemLookupTableModal.data.itemId;
            this.fromItemName = this.ItemLookupTableModal.data.descp;
            this.toItem = this.ItemLookupTableModal.data.itemId;
            this.toItemName = this.ItemLookupTableModal.data.descp;
        } else if (this.type == "toItem") {
            this.toItem = this.ItemLookupTableModal.data.itemId;
            this.toItemName = this.ItemLookupTableModal.data.descp;
        } else if (this.type == "fromLoc") {
            this.fromLoc = this.LocLookupTableModal.data.locID;
            this.fromLocName = this.LocLookupTableModal.data.locName;
            this.toLoc = this.LocLookupTableModal.data.locID;
            this.toLocName = this.LocLookupTableModal.data.locName;
        } else if (this.type == "toLoc") {
            this.toLoc = this.LocLookupTableModal.data.locID;
            this.toLocName = this.LocLookupTableModal.data.locName;
        }
    }
    hideFields() {
        debugger
        if(this.itemLedger=="itemStatus"){
          this.Fromseg3s=true;
          this.ftdate=true;
          this.itemlist=false;
          this.dtloc=true;
          this.standard=true;
        }else if(this.itemLedger=="itemListing"){
            this.Fromseg3s=false;
            this.standard=false;
            this.dtloc=false;
            this.ftdate=false;
            this.itemlist=true;
        }else if(this.itemLedger=="itemLedgerDetail"){
            this.Fromseg3s=false;
            this.itemlist=false;
            this.dtloc=true;
            this.ftdate=true;
            this.standard=true;
        }else{
            this.Fromseg3s=false;
            this.itemlist=false;
            this.dtloc=true;
            this.ftdate=true;
            this.standard=true;
        }
       
    }
    setIdNull(type) {
        this.type = type;
        if (this.type == "fromItem") {
            this.fromItem = 0;
            this.toItem = "99-999-99-9999";
            this.fromItemName = "";
            this.toItemName = "";
        } else if(this.type=="toItem"){
            this.fromLoc = 0;
            this.toLoc = "999999";
            this.fromLocName = "";
            this.toLocName = "";
        } 
        if(this.type=="fromSeg"){
            this.fromseg3 = 0;
            this.fromseg3Name = "";
           
        }else if(this.type){
            this.toseg3 ="99-999-99";
            this.toseg3Name = "";
        }
    }
}
