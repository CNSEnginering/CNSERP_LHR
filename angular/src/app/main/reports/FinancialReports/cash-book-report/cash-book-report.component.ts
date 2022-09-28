import {
    Component,
    Injector,
    ViewEncapsulation,
    ViewChild,
    OnInit,
    ChangeDetectorRef
} from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { NotifyService } from "@abp/notify/notify.service";
import { AppComponentBase } from "@shared/common/app-component-base";
import {
    TokenAuthServiceProxy,
    CashBookReportServiceProxy,
    VoucherEntryServiceProxy
} from "@shared/service-proxies/service-proxies";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import * as _ from "lodash";
import * as moment from "moment";
import { ChartofcontrolLookupFinderComponent } from "../../chartofcontrol-lookup-finder/chartofcontrol-lookup-finder.component";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";
import { FiscalDateService } from "@app/shared/services/fiscalDate.service";
import { CommonServiceLookupTableModalComponent } from "@app/finders/commonService/commonService-lookup-table-modal.component";
import { ReportFilterServiceProxy, BankChartofControlLookupTableDto, BanksServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: "app-cash-book-report",
    templateUrl: "./cash-book-report.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class CashBookReportComponent extends AppComponentBase
    implements OnInit {
    @ViewChild("fromAccountFinder", { static: true })
    fromAccountFinder: ChartofcontrolLookupFinderComponent;
    @ViewChild("toAccountFinder", { static: true })
    toAccountFinder: ChartofcontrolLookupFinderComponent;
    //@ViewChild('reportviewrModalComponent', { static: false }) reportviewrModalComponent: ReportviewrModalComponent;
    @ViewChild("reportView", { static: true })
    reportView: ReportviewrModalComponent;
    @ViewChild("CommonServiceLookupTableModal", { static: true })
    CommonServiceLookupTableModal: CommonServiceLookupTableModalComponent;

    fromDate;
    toDate;
    reportServer: string;
    target: string;
    // reportUrl: string;
    showParameters: string;
    parameters: {};
    language: string;
    width: number;
    height: number;
    toolbar: string;
    fromAccount = "";
    toAccount = "";
    rptObj: any;

    checkAccount: boolean = false;

    rptCashBook: boolean;
    visible: boolean;
    str: string;
    viewOption: any;
    fromAccountName: string;
    toAccountName: string;
    status: "Approved";
    location: 0;
    locationList;
    curid: string;
    currRate: number;
    constructor(
        injector: Injector,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private route: Router,
        private _reportService: FiscalDateService,
        private _voucherEntryServiceProxy: VoucherEntryServiceProxy, private _reportFilterService: ReportFilterServiceProxy
    ) {
        super(injector);
    }

    ngOnInit() {
        debugger;

        this._voucherEntryServiceProxy.getBaseCurrency().subscribe(result => {
            if (result) {
                this.curid = result.id;
                this.currRate = result.currRate;
            }
        });
        this.status = "Approved";
        this.location = 0;
        this.getLocationList();
        this._reportService.getDate().subscribe(data => {
            this.fromDate = new Date(data["result"]);
            this.toDate = new Date();
        });
        this.viewOption = this._activatedRoute.snapshot.data.viewOption;

        this._reportFilterService.getChartofControlForLookupTable('',this.viewOption,'',0,1).subscribe(data => {
           
         
            this.fromAccount=data.items[0].id;
            this.fromAccountName=data.items[0].displayName;
            this.toAccount=data.items[0].id;
            this.toAccountName=data.items[0].displayName;
        });
        //   this._reportService.getDate().subscribe(
        //     data => {
        //       var fromDate = moment(data["result"]).format("MM/DD/YYYY");
        //       console.log(data["result"])
        //       console.log(fromDate)
        //       this.fromDate = moment(fromDate).toDate();
        //     })

        // debugger;
        this.viewOption = this._activatedRoute.snapshot.data.viewOption;
        if (this.viewOption === "CashBook") {
            this.rptCashBook = true;
            this.target = "CashBook"
        } else {
            this.rptCashBook = false;
            this.target = "BankBook"
        }
        this.fromAccount = "00-000-00-0000";
        this.toAccount = "99-999-99-9999";
    }

    getNewCommonServiceModal() {
        debugger;
        this.curid = this.CommonServiceLookupTableModal.id;
        this.currRate = this.CommonServiceLookupTableModal.currRate;
    }
    setToAllAccount(){
        this.fromAccountName="";
        this.fromAccount="";
    }
    setToAllAccount1(){
        this.toAccount="";
        this.toAccountName="";
    }

    getLocationList(): void {
        this._voucherEntryServiceProxy.getGLLocData().subscribe(resultL => {
            this.locationList = resultL;
        });
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

    getReport() {
        debugger;

        let repParams = "";
        if (this.fromDate !== undefined)
            repParams += "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
        if (this.toDate !== undefined)
            repParams += "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
        if (this.fromAccount !== undefined)
            repParams += encodeURIComponent("" + this.fromAccount) + "$";
        if (this.toAccount !== undefined)
            repParams += encodeURIComponent("" + this.toAccount) + "$";
        if (this.status !== undefined)
            repParams += encodeURIComponent("" + this.status) + "$";
        if (this.location !== undefined)
            repParams +=
                encodeURIComponent("" + this.location) + "$";
        repParams += encodeURIComponent("" + this.currRate) + "$";
        if (this.rptCashBook != undefined)
            repParams += encodeURIComponent("" + this.rptCashBook);



        repParams = repParams.replace(/[?$]&/, "");

        this.reportView.show("CASHBOOK", repParams);
    }

    selectFromAccount() {
        this.fromAccountFinder.accid = this.fromAccount;
        this.fromAccountFinder.displayName = this.fromAccountName;
        this.fromAccountFinder.show(this.target);
    }

    setFromAccount() {
        this.fromAccount = "000-000-00-0000";
        this.fromAccountName = "99-999-99-9999";
        this.setToAccount();
    }

    getToAccount() {
        this.toAccount = this.toAccountFinder.accid;
        this.toAccountName = this.toAccountFinder.displayName;
    }

    getFromAccount() {
        this.fromAccount = this.fromAccountFinder.accid;
        this.fromAccountName = this.fromAccountFinder.displayName;
        this.toAccount = this.fromAccountFinder.accid;
        this.toAccountName = this.fromAccountFinder.displayName;
    }

    selectToAccount() {
        this.toAccountFinder.accid = this.toAccount;
        this.toAccountFinder.displayName = this.toAccountName;
        this.toAccountFinder.show(this.target);
    }

    setToAccount() {
        this.toAccount = "000-000-00-0000";
        this.toAccountName = "99-999-99-9999";

    }
}
