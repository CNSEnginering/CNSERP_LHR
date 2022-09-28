import {
    Component,
    Injector,
    ViewEncapsulation,
    ViewChild,
    EventEmitter,
    Output
} from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { NotifyService } from "@abp/notify/notify.service";
import { AppComponentBase } from "@shared/common/app-component-base";
import {
    TokenAuthServiceProxy,
    VoucherEntryServiceProxy
} from "@shared/service-proxies/service-proxies";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { SessionServiceProxy } from "@shared/service-proxies/service-proxies";
import { LazyLoadEvent } from "primeng/components/common/lazyloadevent";
import * as _ from "lodash";
import * as moment from "moment";
import { ReportFilterServiceProxy } from "@shared/service-proxies/service-proxies";
import { LedgerFiltersDto } from "../../dto/ledger-filters-dto";
import { ChartofcontrolLookupFinderComponent } from "../../chartofcontrol-lookup-finder/chartofcontrol-lookup-finder.component";
import { threadId } from "worker_threads";
import { FiscalDateService } from "@app/shared/services/fiscalDate.service";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";
import { CommonServiceLookupTableModalComponent } from "@app/finders/commonService/commonService-lookup-table-modal.component";
import { IDropdownSettings, } from 'ng-multiselect-dropdown';
import { FormBuilder, FormGroup } from '@angular/forms';
@Component({
    templateUrl: "./trialBalance.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TrialBalanceComponent extends AppComponentBase {

    dropdownSettings:IDropdownSettings={};
    // dropdownSettings = {
    //     idField: 'item_id',
    //     textField: 'item_text',
    //   };


    @ViewChild("appchartofcontrollookupfinder", { static: true })
    appchartofcontrollookupfinder: ChartofcontrolLookupFinderComponent;

    @ViewChild("CommonServiceLookupTableModal", { static: true })
    CommonServiceLookupTableModal: CommonServiceLookupTableModalComponent;

    @ViewChild("reportView", { static: true })
    reportView: ReportviewrModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    rptObj: any;
    chartOfAccountList: any;
    fromDate;
    toDate;
    curid: any;
    list : any[];
    currRate: any;
    glloc:string;
    bookId = "";
    userId = "";
    directPost = false;
    status: number = 1;
    showReport: boolean = false;
    ledgerFilters: LedgerFiltersDto = new LedgerFiltersDto();
    checkAccount: boolean = false;
    reportServer: string;
    reportUrl: string;
    showParameters: string;
    parameters: any;
    language: string;
    width: number;
    height: number;
    toolbar: string;
    //adding new variable
    newValue: string="";
    valuef:string;
    
     
    //----------//
    trialBalance;
    trialBalanceArr;
     locationList=[];
     selectedItems=[];
    location: number;
    constructor(
        injector: Injector,
        private session: SessionServiceProxy,
        private _reportFilterServiceProxy: ReportFilterServiceProxy,
        private route: Router,
        private _voucherEntryServiceProxy: VoucherEntryServiceProxy,
        private _reportService: FiscalDateService,private fb: FormBuilder
    ) {
        super(injector);
    }

    ngOnInit() {
        this._voucherEntryServiceProxy.getBaseCurrency().subscribe(result => {
            if (result) {
                this.curid = result.id;
                this.currRate = result.currRate;
            }
        });
        this.getLocationList();
        this.toDate = new Date();
        this._reportService.getDate().subscribe(data => {
            this.fromDate = new Date(data["result"]);
        });
        // var toDate = moment().format("MM/DD/YYYY");
        // this.toDate = toDate;
        // var fromDate = moment("2019-07-01").format("MM/DD/YYYY");
        // this.fromDate = fromDate;
        this._reportFilterServiceProxy
            .getAllChartofControlList()
            .subscribe(result => {
                this.chartOfAccountList = result.items;
            });
        this.ledgerFilters.fromAccount = "00-000-00-0000";
        this.ledgerFilters.toAccount = "99-999-99-9999";
        this.trialBalanceArr = [];
        this.trialBalanceArr.push({
            id: "TrialBalance1",
            name: "Level 1"
        });
        this.trialBalanceArr.push({
            id: "TrialBalance2",
            name: "Level 2"
        });
        this.trialBalanceArr.push({
            id: "TrialBalance3",
            name: "Level 3"
        });
        this.trialBalanceArr.push({
            id: "TrialBalance",
            name: "Level 4"
        });
        this.trialBalanceArr.push({
            id: "TrialBalanceOpening1",
            name: "Level 1 with Opening"
        });
        this.trialBalanceArr.push({
            id: "TrialBalanceOpening2",
            name: "Level 2 with Opening"
        });
        this.trialBalanceArr.push({
            id: "TrialBalanceOpening3",
            name: "Level 3 with Opening"
        });
        this.trialBalanceArr.push({
            id: "TrialBalanceOpening",
            name: "Level 4 with Opening"
        });
        this.trialBalance = "TrialBalance1";
        this.location = 0;
        this.status = 1;
        this.ledgerFilters.includeLevel3 = false;
        this.ledgerFilters.includeZeroBalance = false;

        // this._reportService.getDate().subscribe(data => {
        //     var fromDate = moment(data["result"]).format("MM/DD/YYYY");
        //     console.log(data["result"]);
        //     console.log(fromDate);
        //     this.fromDate = fromDate;
        // });
        
        this.dropdownSettings = {
            idField: 'locId',
            textField: 'locDesc',
            enableCheckAll: true, allowSearchFilter: true,
            selectAllText: "Select All Location",
            unSelectAllText: "UnSelect All Location",
            itemsShowLimit: 0,singleSelection: false,
          };
    }
    getLocationList(): void {
        this._voucherEntryServiceProxy.getGLLocData().subscribe(resultL => {
            this.locationList = resultL;
            console.log(this.locationList);
        });
        
    }
    onItemSelect(event: any, checked: boolean) {
         debugger
        // if (this.selectedQuarterList.length > 1) { //almost two elements selected
    
        //   //we order the elements acording the list
        //   const value=this.quarterList.filter(x=>this.selectedQuarterList.indexOf(x)>=0)
    
        //   //get the index of the first and the last element
        //   let first = this.quarterList.findIndex((x) => x == value[0]);
        //   let last = this.quarterList.findIndex(
        //     (x) => x == value[value.length - 1]
        //   );
    
        //   //and give the value between this indexs
        //   this.selectedQuarterList = this.quarterList.filter(
        //     (_, index) => index >= first && (last < 0 || index <= last)
        //   );
        // }
      }
    selectFromAccount() {
        this.appchartofcontrollookupfinder.accid = this.ledgerFilters.fromAccount;
        this.appchartofcontrollookupfinder.displayName = this.ledgerFilters.fromAccountName;
        this.appchartofcontrollookupfinder.show();
    }

    setFromAccount() {
        this.ledgerFilters.fromAccount = "00-000-00-0000";
        this.ledgerFilters.toAccount = "99-999-99-9999";
        this.ledgerFilters.fromAccountName = "";
    }

    getFromAndToAccount() {
        if (this.checkAccount) {
            this.ledgerFilters.toAccount = this.appchartofcontrollookupfinder.accid;
            this.ledgerFilters.toAccountName = this.appchartofcontrollookupfinder.displayName;
        } else {
            this.ledgerFilters.fromAccount = this.appchartofcontrollookupfinder.accid;
            this.ledgerFilters.fromAccountName = this.appchartofcontrollookupfinder.displayName;
            this.ledgerFilters.toAccount = this.appchartofcontrollookupfinder.accid;
            this.ledgerFilters.toAccountName = this.appchartofcontrollookupfinder.displayName;
        }
        this.checkAccount = false;
    }

    selectToAccount() {
        this.checkAccount = true;
        this.appchartofcontrollookupfinder.accid = this.ledgerFilters.toAccount;
        this.appchartofcontrollookupfinder.displayName = this.ledgerFilters.toAccountName;
        this.appchartofcontrollookupfinder.show();
    }

    setToAccount() {
        this.ledgerFilters.fromAccount = "00-000-00-0000";
        this.ledgerFilters.toAccount = "99-999-99-9999";
        this.ledgerFilters.toAccountName = "";
    }

    getNewCommonServiceModal() {
        this.curid = this.CommonServiceLookupTableModal.id;
        this.currRate = this.CommonServiceLookupTableModal.currRate;
    }

    getReport() {
        
        debugger;
        console.log(this.glloc);
        var reportName = this.trialBalance;
        // this.rptObj = {
        //   "FromDate": moment(this.fromDate).format("YYYY/MM/DD"),
        //   "ToDate": moment(this.toDate).format("YYYY/MM/DD"),
        //   "FromAcc": this.ledgerFilters.fromAccount,
        //   "ToAcc": this.ledgerFilters.toAccount,
        //   "TenantId": this.appSession.tenant.id,
        //   "LocId":this.location,
        //   "Status":this.status
        // };
        let repParams = "";
        if (this.fromDate !== undefined)
            repParams += "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
        if (this.toDate !== undefined)
            repParams += "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
        if (this.ledgerFilters.fromAccount !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.fromAccount) + "$";
        if (this.ledgerFilters.toAccount !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.toAccount) + "$";
        if (this.location !== undefined)
            repParams += encodeURIComponent("" + this.location) + "$";
        if (this.status !== undefined)
            repParams += encodeURIComponent("" + this.status) + "$";

        repParams +=
            encodeURIComponent("" + this.ledgerFilters.includeLevel3) + "$";

        repParams +=
            encodeURIComponent("" + this.ledgerFilters.includeZeroBalance) +
            "$";
       
        repParams += this.curid + "$";

        repParams +=
        encodeURIComponent("" + this.curid) +
        "$";

        repParams = repParams.replace(/[?$]&/, "");
        this.reportView.show(reportName, repParams);
        // localStorage.setItem('rptObj', JSON.stringify(this.rptObj));
        // localStorage.setItem('rptName', reportName);
        // this.route.navigateByUrl('/app/main/reports/ReportView');
    }

    openSelectCurrencyRateModal() {
        this.curid = "";
        this.currRate = 0;
        this.CommonServiceLookupTableModal.show("Currency");
    }

    setCurrencyRateIdNull() {
        this.curid = "";
        this.currRate = null;
    }
  //value change Func
  onChange(){
    debugger
    var value 
    value=this.ledgerFilters.fromAccount;
    this.newValue= value.substring(0, 2);

  }
  onClick(){
     debugger
     var valueToaccNew
    valueToaccNew=this.ledgerFilters.toAccount.substring(2,14);
    this.ledgerFilters.toAccount=this.newValue + valueToaccNew; 
  }

}
