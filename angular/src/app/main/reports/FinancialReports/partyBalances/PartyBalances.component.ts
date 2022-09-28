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
import { FiscalDateService } from "@app/shared/services/fiscalDate.service";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";
import { CommonServiceLookupTableModalComponent } from "@app/finders/commonService/commonService-lookup-table-modal.component";
@Component({
    templateUrl: "./partyBalances.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class PartyBalancesComponent extends AppComponentBase {
    @ViewChild("appchartofcontrollookupfinder", { static: true })
    appchartofcontrollookupfinder: ChartofcontrolLookupFinderComponent;

    @ViewChild("reportView", { static: true })
    reportView: ReportviewrModalComponent;

    @ViewChild("CommonServiceLookupTableModal", { static: true })
    CommonServiceLookupTableModal: CommonServiceLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    chartOfAccountList: any;
    rptObj: any;
    fromDate;
    toDate;
    bookId = "";
    userId = "";
    directPost = false;
    status: string = "";
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
    private locationList;
    location: number;
    curid: any;
    currRate: any;

    constructor(
        injector: Injector,
        private session: SessionServiceProxy,
        private _reportFilterServiceProxy: ReportFilterServiceProxy,
        private route: Router,
        private _voucherEntryServiceProxy: VoucherEntryServiceProxy,
        private _reportService: FiscalDateService
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

        this._reportFilterServiceProxy
            .getAllChartofControlList()
            .subscribe(result => {
                this.chartOfAccountList = result.items;
            });
        // var toDate = moment().format("MM/DD/YYYY");
        // this.toDate = toDate;
        this.toDate = new Date();
        this._reportService.getDate().subscribe(data => {
            this.fromDate = new Date(data["result"]);
        });
        // this._reportService.getDate().subscribe(data => {
        //     var fromDate = moment(data["result"]).format("MM/DD/YYYY");
        //     console.log(data["result"]);
        //     console.log(fromDate);
        //     this.fromDate = fromDate;
        // });

        this.ledgerFilters.fromAccount = "0";
        this.ledgerFilters.toAccount = "9-999-99-9999";
        this.getLocationList();
        this.location = 0;
        this.status = "Both";
        this.ledgerFilters.includeLevel3 = false;
        this.ledgerFilters.includeZeroBalance = false;
    }
    getLocationList(): void {
        this._voucherEntryServiceProxy.getGLLocData().subscribe(resultL => {
            this.locationList = resultL;
        });
    }

    getNewCommonServiceModal() {
        this.curid = this.CommonServiceLookupTableModal.id;
        this.currRate = this.CommonServiceLookupTableModal.currRate;
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

    selectFromAccount() {
        this.appchartofcontrollookupfinder.accid = this.ledgerFilters.fromAccount;
        this.appchartofcontrollookupfinder.displayName = this.ledgerFilters.fromAccountName;
        this.appchartofcontrollookupfinder.show();
    }

    setFromAccount() {
        this.ledgerFilters.fromAccount = "0";
        this.ledgerFilters.toAccount = "9-999-99-9999";
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
        this.ledgerFilters.fromAccount = "0";
        this.ledgerFilters.toAccount = "9-999-99-9999";
        this.ledgerFilters.toAccountName = "";
    }

    getReport() {
        // this.rptObj = {
        //   "FromDate": new Date(this.fromDate).toLocaleDateString(),
        //   "ToDate": new Date(this.toDate).toLocaleDateString(),
        //   "FromAcc": this.ledgerFilters.fromAccount,
        //   "ToAcc": this.ledgerFilters.toAccount,
        //   "TenantId": this.appSession.tenant.id,
        //   "LocId": this.location,
        //   "Status": this.status
        // };
        // localStorage.setItem('rptObj', JSON.stringify(this.rptObj));
        // localStorage.setItem('rptName', 'PartyBalances');
        // this.route.navigateByUrl('/app/main/reports/ReportView');
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

        repParams += this.currRate + "$";

        repParams += this.curid + "$";

        this.reportView.show("PartyBalances", repParams);
    }
}
