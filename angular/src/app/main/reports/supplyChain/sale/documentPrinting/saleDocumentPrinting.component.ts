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
import { ItemPricingLookupTableModalComponent } from "@app/main/supplyChain/inventory/FinderModals/itemPricing-lookup-table-modal.component";
import { ItemLookupTableModalComponent } from "@app/main/reports/item-lookup-finder/item-lookup-table-modal.component";
import { LocLookupTableModalComponent } from "@app/main/reports/loc-lookup-finder/loc-lookup-table-modal.component";
import { ReportFilterServiceProxy } from "@shared/service-proxies/service-proxies";
import { FiscalDateService } from "@app/shared/services/fiscalDate.service";
import * as _ from "lodash";
import * as moment from "moment";
import { GetDataService } from '../../../../supplyChain/inventory/shared/services/get-data.service';
import { IDropdownSettings, } from 'ng-multiselect-dropdown';
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";
@Component({
    templateUrl: "./saledocumentPrinting.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class SaleDocumentPrintingComponent extends AppComponentBase {
    @ViewChild("ItemLookupTableModal", { static: true })
    ItemLookupTableModal: ItemLookupTableModalComponent;
    @ViewChild("LocLookupTableModal", { static: true })
    LocLookupTableModal: LocLookupTableModalComponent;
    @ViewChild("reportView", { static: true })
    reportView: ReportviewrModalComponent;
    rptObj: any;
    status: string = "";
    showReport: boolean = false;
    opening: boolean = false;
    transfer: boolean = false;
    consumption: boolean = false;
    locationList:any;
    dropdownSettings:IDropdownSettings={};
    locIdlist:any;
    adjustment: boolean = false;
    igp: boolean = false;
    ogp: boolean = false;
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
    type: string = "";
    filterText = "";
    id: number;
    priceList: any;
    sorting: any;
    skipCount: any;
    MaxResultCount: any;
    data: any;

    constructor(
        injector: Injector,
        private route: Router,
        private _reportService: FiscalDateService,private _getDataService: GetDataService
    ) {
        super(injector);
    }

    ngOnInit() {
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
        this._reportService.getDate().subscribe(data => {
            this.fromDate = new Date(data["result"]);
            this.toDate = new Date();
        });
        this.fromDoc = 0;
        this.toDoc = 99999;
        this.fromLoc = "0";
        this.toLoc = "99999";
        this.fromItem = "0";
        this.toItem = "99-999-99-9999";
       this.itemLedger="salesRegister";
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
            case "invoice":
            case "invoiceTax":
                this.showFromDate = true;
                this.showToDate = true;
                this.showFromDoc = true;
                this.showToDoc = true;
                this.showFromLoc = false;
                this.showToLoc = false;
                this.showFromItem = false;
                this.showToItem = false;
                break;
            case "salesRegister":
                this.showFromDate = true;
                this.showToDate = true;
                this.showFromDoc = true;
                this.showToDoc = true;
                this.showFromLoc = true;
                this.showToLoc = true;
                this.showFromItem = true;
                this.showToItem = true;
                break;
            case "SaleQuotation":
            case "costsheet":
                this.showFromDate = false;
                this.showToDate = false;
                this.showFromDoc = true;
                this.showToDoc = false;
                this.showFromLoc = false;
                //this.actv='disabled';
                this.showToLoc = false;
                this.showFromItem = false;
                this.showToItem = false;

        }
    }
    getReport() {
        switch (this.itemLedger) {
            case "invoice":
                this.processReport("Invoice");
                break;
            case "invoiceTax":
                this.processReport("InvoiceTax");
                break;
            case "salesRegister":
                this.processReport("SalesRegister");
                break;
            case "SaleQuotation":
                this.processReport("SaleQuotation");
                 break;
            case "costsheet":
                this.processReport("costsheet");
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
       
        let repParams = "";
        switch (report) {
            case "Invoice":
            case "InvoiceTax":
            case "SalesRegister":
            case "SaleQuotation":
            case "costsheet":
                // this.rptObj = {
                //     FromDate: new Date(this.fromDate).toLocaleDateString(),
                //     ToDate: new Date(this.toDate).toLocaleDateString(),
                //     FromDoc: this.fromDoc,
                //     ToDoc: this.toDoc,
                //     TenantId: this.appSession.tenant.id
                //     // "address":"Model Town Lahore",
                //     // "phoneNo":"123457894"
                // };
                if (this.fromDate !== undefined)
                    repParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    repParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                if (this.fromDoc !== undefined)
                    repParams += encodeURIComponent("" + this.fromDoc) + "$";
                if (this.toDoc !== undefined)
                    repParams += encodeURIComponent("" + this.toDoc) + "$";

                console.log(repParams);
                repParams = repParams.replace(/[?$]&/, "");
                break;
        }
        this.reportView.show(report, repParams);
        // localStorage.setItem("rptObj", JSON.stringify(this.rptObj));
        // localStorage.setItem("rptName", report);

        // this.route.navigateByUrl("/app/main/reports/ReportView");
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
