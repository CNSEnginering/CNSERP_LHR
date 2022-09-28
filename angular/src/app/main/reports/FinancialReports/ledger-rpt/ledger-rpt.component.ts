// import { Component, OnInit } from '@angular/core';
// import { appModuleAnimation } from '@shared/animations/routerTransition';

import {
    Component,
    Injector,
    ViewEncapsulation,
    ViewChild,
    OnInit,
    Output,
    EventEmitter
} from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import {
    ReportFilterServiceProxy,
    VoucherEntryServiceProxy,
    GetBookViewModeldto,
    UserDto,
    APTransactionListServiceProxy
} from "@shared/service-proxies/service-proxies";
import { NotifyService } from "@abp/notify/notify.service";
import { AppComponentBase } from "@shared/common/app-component-base";
import { TokenAuthServiceProxy } from "@shared/service-proxies/service-proxies";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { FileDownloadService } from "@shared/utils/file-download.service";
import * as _ from "lodash";
import * as moment from "moment";
import { LedgerFiltersDto } from "../../dto/ledger-filters-dto";
import { ChartofcontrolLookupFinderComponent } from "../../chartofcontrol-lookup-finder/chartofcontrol-lookup-finder.component";
import { SubledgerLookupFinderComponent } from "../../subledger-lookup-finder/subledger-lookup-finder.component";
import { LegderTypeComboboxService } from "@app/shared/common/legdertype-combobox/legdertype-combobox.service";
import { FinanceLookupTableModalComponent } from "@app/finders/finance/finance-lookup-table-modal.component";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";
import { FiscalDateService } from "@app/shared/services/fiscalDate.service";
import { CommonServiceLookupTableModalComponent } from "@app/finders/commonService/commonService-lookup-table-modal.component";

@Component({
    selector: "app-ledger-rpt",
    templateUrl: "./ledger-rpt.component.html",
    styleUrls: ["./ledger-rpt.component.css"],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class LedgerRptComponent extends AppComponentBase implements OnInit {
    @ViewChild("FinanceLookupTableModal", { static: true })
    FinanceLookupTableModal: FinanceLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild("reportView", { static: true })
    reportView: ReportviewrModalComponent;
    @ViewChild("CommonServiceLookupTableModal", { static: true })
    CommonServiceLookupTableModal: CommonServiceLookupTableModalComponent;

    dateSection: boolean = true;
    accSection: boolean = true;
    asOnDateSec:boolean = false;
    toAccSec: boolean = true;
    locSection: boolean = true;
    subAccSection: boolean = false;
    slSection: boolean = false;
    locationsSec: boolean = false;
    chkBoxesSection: boolean = false;
    trialBalanceSection: boolean = true;
    booksSection: boolean = true;
    usersSection: boolean = true;
    statusSection: boolean = true;
    curSection: boolean = true;
    curid: any;
    asOnDate;
    currRate: any;
    bookDto: GetBookViewModeldto[] = [];
    userDto: UserDto[] = [];
    status = "";
    reportServer: string;
    reportUrl: string;
    showParameters: string;
    parameters: any;
    language: string;
    width: number;
    height: number;
    toolbar: string;
    trialBalanceArr;
    trialBalance;
    public isCollapsed = false;
    fromDate;
    toDate;
    ledgerFilters: LedgerFiltersDto = new LedgerFiltersDto();
    chartOfAccountList: any;
    subLedgerList: any;
    checkAccount: boolean = false;
    checkSubAccount: boolean = false;
    bookId = "";
    userId = "";
    rptObj: any;
    public locationList;
    ledgerTypes: any[];
    target: any;
    picker: any;
    report: string;
    fromAC: string;
    toAC: string;
    locId: number;
    slType: any;
    fromSubledgerId: any;
    toSubledgerId: any;
    constructor(
        injector: Injector,
        private _reportFilterServiceProxy: ReportFilterServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _apTransactionListServiceProxy: APTransactionListServiceProxy,
        private _voucherEntryServiceProxy: VoucherEntryServiceProxy,
        private _LegderTypeComboboxService: LegderTypeComboboxService,
        private _reportService: FiscalDateService,
        private route: Router
    ) {
        super(injector);
    }
    getBookList(): void {
        this._apTransactionListServiceProxy.getBookList().subscribe(result => {
            this.bookDto = result.items;
        });
    }

    getUserList(): void {
        this._apTransactionListServiceProxy.getUserList().subscribe(result => {
            this.userDto = result.items;
        });
    }
    ngOnInit() {
        this._voucherEntryServiceProxy.getBaseCurrency().subscribe(result => {
            if (result) {
                this.curid = result.id;
                this.currRate = result.currRate;
            }
        });
        // var toDate = moment().format("MM/DD/YYYY");
        this.toDate = new Date();
        this.asOnDate = new Date();
        this._reportService.getDate().subscribe(data => {
            this.fromDate = new Date(data["result"]);
        });
        //this.fromDate = fromDate;
        this.ledgerFilters.fromAccount = "00-000-00-0000";
        this.ledgerFilters.toAccount = "99-999-99-9999";
        this.ledgerFilters.fromSubAccount = "0";
        this.ledgerFilters.toSubAccount = "99999";
        this.ledgerFilters.fromLoc = "0";
        this.ledgerFilters.toLoc = "99999";
        this.ledgerFilters.fromSubAccount = "0";
        this.ledgerFilters.toSubAccount = "99999";

        this.bookId = "All";
        this.userId = "All";
        this.ledgerFilters.status = "Approved";
        this.ledgerFilters.location = 0;
        this.ledgerFilters.slType = 0;
        this.subAccSection = false;
        this.getBookList();
        this.getUserList();
        this.getLocationList();
        this.getLedgerTypes();

        this._reportFilterServiceProxy
            .getAllChartofControlList()
            .subscribe(result => {
                this.chartOfAccountList = result.items;
            });
        this.trialBalanceArr = [];
        this.trialBalanceArr.push({
            id: "TrialBalance1",
            name: "Trial Balance 1"
        });
        this.trialBalanceArr.push({
            id: "TrialBalance2",
            name: "Trial Balance 2"
        });
        this.trialBalanceArr.push({
            id: "TrialBalance3",
            name: "Trial Balance 3"
        });
        this.trialBalanceArr.push({
            id: "TrialBalance",
            name: "Trial Balance 4"
        });
        this.trialBalance = "TrialBalance1";

        this.ledgerFilters.reportType = "PartyLedger";
    }

    showHideControls(reportType): void {
        debugger;
        this.dateSection = false;
        this.accSection = false;
        this.locSection = false;
        this.slSection = false;
        this.subAccSection = false;
        this.locationsSec = false;
        this.chkBoxesSection = false;
        this.asOnDateSec = false;
        this.toAccSec=true;
        switch (reportType) {
            case "CustAnalysisReport":
                this.dateSection = true;
                this.statusSection = true;
                this.slSection = true;
                this.accSection = true;
                this.chkBoxesSection = true;
                this.subAccSection = true;
                this.ledgerFilters.bookSummary=false;
                this.asOnDateSec = false;
                this.locationsSec = true;
                break;
            case "PartyBalance":
                this.accSection = true;
                this.dateSection = true;
                this.ledgerFilters.bookSummary=false;
                this.asOnDateSec = false;
                break;
            case "PartyLedger":
                this.dateSection = true;
                this.accSection = true;
                this.locSection = true; 
                this.ledgerFilters.bookSummary=false;
                this.asOnDateSec = false;
                break;
            case "CustomerLedger":
                this.dateSection = true;
                this.accSection = true;
                this.subAccSection = true;
                this.locationsSec = true;
                this.slSection = true;
                this.chkBoxesSection = true;
                this.asOnDateSec = false;
                break;
            case "TrialBalance":
                this.accSection = true;
                this.dateSection = true;
                this.trialBalanceSection = true;
                this.ledgerFilters.bookSummary=false;
                this.asOnDateSec = false;
                break;
            case "TransactionListing":
                this.ledgerFilters.bookSummary=false;
                this.dateSection = true;
                this.statusSection = true;
                this.booksSection = true;
                this.usersSection = true;
                this.asOnDateSec = false;
                break;

            case "MonthlyConsolidatedLedger":
                this.accSection = true;
                this.dateSection = true;
                this.statusSection = true;
                this.locSection = true; 
                this.ledgerFilters.bookSummary=false;
                this.asOnDateSec = false;
                break;
            case "AgeingInvoiceReport":
                this.accSection = true;
                this.dateSection = false;
                this.chkBoxesSection = false;
                this.subAccSection = true;
                this.trialBalanceSection = false;
                this.statusSection = true;
                this.booksSection = false;
                this.usersSection = false;
                this.locSection = false;
                this.curSection = false;
                this.slSection = false;
                this.asOnDateSec = true;
                this.toAccSec = false;
                break;
        }
    }

    getLocationList(): void {
        this._voucherEntryServiceProxy.getGLLocData().subscribe(resultL => {
            this.locationList = resultL;
        });
    }

    getReport(): void {
        debugger;
        switch (this.ledgerFilters.reportType) {
            case "CustAnalysisReport":
                this.getCustAnalysisReport();
                break;
            case "PartyBalance":
                this.getPartyBalanceReport();
                break;
            case "TransactionListing":
                this.getTransactionListingReport();
                break;
            case "TrialBalance":
                this.getTrialBalanceReport();
                break;
            case "PartyLedger":
                if (this.ledgerFilters.bookSummary) {
                    this.getGLSummaryBookWiseReport();
                } else {
                    this.getPartyLedgerReport();
                }
                break;
            case "CustomerLedger":
                this.getCustomerLedgerReport();
                break;
            case "MonthlyConsolidatedLedger":
                this.getConsolidatedLedger();
                break;
            case "AgeingInvoiceReport":
                this.getAgingInvoiceReport();
                break;
        }
    }

    getAgingInvoiceReport() {
        debugger
        let repParams = "";
        this.report = "AgeingInvoiceReport";
        if (this.asOnDate !== undefined)
            repParams += "" + moment(this.asOnDate).format("YYYY/MM/DD") + "$";
        if (this.toDate !== undefined)
            repParams += "" + moment(this.toDate).format("YYYY/MM/DD") + "$";     
        if (this.ledgerFilters.fromAccount !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.fromAccount) + "$";
        if (this.ledgerFilters.toAccount !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.toAccount) + "$";
        if (this.ledgerFilters.fromLoc !== undefined)
            repParams +=
                 encodeURIComponent("" + this.ledgerFilters.fromLoc) + "$";    
        if (this.ledgerFilters.toLoc !== undefined)
                repParams +=
                    encodeURIComponent("" + this.ledgerFilters.toLoc) + "$";
     
        repParams += encodeURIComponent("" + this.currRate) + "$";


        repParams += encodeURIComponent("" + this.ledgerFilters.location) + "$";
        repParams += encodeURIComponent("" + this.curid) + "$";
        repParams +=
            encodeURIComponent("" + this.ledgerFilters.bookSummary) + "$";

        repParams = repParams.replace(/[?$]&/, "");

        this.reportView.show(this.report, repParams);
    };

    getCustAnalysisReport() {
        debugger
        let repParams = "";
        this.report = "CustAnalysisReport";
        if (this.fromDate !== undefined)
            repParams += "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
        if (this.toDate !== undefined)
            repParams += "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
        if (this.ledgerFilters.status !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.status) + "$";
        if (this.ledgerFilters.slType !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.slType) + "$";
        if (this.ledgerFilters.fromAccount !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.fromAccount) + "$";
        if (this.ledgerFilters.toAccount !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.toAccount) + "$";
        if (this.ledgerFilters.fromSubAccount !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.fromSubAccount) + "$";
        if (this.ledgerFilters.toSubAccount !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.toSubAccount) + "$";        

        
        repParams += encodeURIComponent("" + this.currRate) + "$";

        repParams += encodeURIComponent("" + this.ledgerFilters.location) + "$";
        repParams += encodeURIComponent("" + this.curid) + "$";
        
        

            repParams += encodeURIComponent("" + this.ledgerFilters.bookSummary) + "$";

        repParams = repParams.replace(/[?$]&/, "");

        this.reportView.show(this.report, repParams);
    }
    getTrialBalanceReport() {
        var reportName = this.trialBalance;
        var moment = require("moment");
        this.reportServer = "http://db-sbx-cnserp/reportserver";
        this.reportUrl = "ERP.Reports/" + reportName;
        this.showParameters = "false";
        this.parameters = {
            FromDate: moment(this.fromDate).format("YYYY/MM/DD"),
            ToDate: moment(this.toDate).format("YYYY/MM/DD"),
            FromAcc: this.ledgerFilters.fromAccount,
            ToAcc: this.ledgerFilters.toAccount,
            TenantId: this.appSession.tenantId
        };
        this.language = "en-us";
        this.width = 100;
        this.height = 100;
        this.toolbar = "true";
    }
    getTransactionListingReport() {
        var moment = require("moment");
        this.reportServer = "http://db-sbx-cnserp/reportserver";
        this.reportUrl = "ERP.Reports/Finance_TransList";
        this.showParameters = "false";
        this.parameters = {
            FromDate: moment(this.fromDate).format("YYYY/MM/DD"),
            ToDate: moment(this.toDate).format("YYYY/MM/DD"),
            Book: this.bookId,
            User: this.userId,
            Status: this.status == "true" ? true : false,
            TenantId: this.appSession.tenantId,
            locId: this.ledgerFilters.location
        };
        this.language = "en-us";
        this.width = 100;
        this.height = 100;
        this.toolbar = "true";
    }
    getGLSummaryBookWiseReport() {
        // this.parameters = {
        //   "fromDate": moment(this.fromDate).format("YYYY/MM/DD"),
        //   "toDate": moment(this.toDate).format("YYYY/MM/DD"),
        //   "fromAC": this.ledgerFilters.fromAccount,
        //   "toAC": this.ledgerFilters.toAccount,
        //   "status": this.ledgerFilters.status,
        //   "tenantId": this.appSession.tenantId,
        //   "locId": this.ledgerFilters.location
        // };

        // localStorage.setItem('rptObj', JSON.stringify(this.parameters));
        // localStorage.setItem('rptName', "GeneralLedgerBookWise");
        // this.route.navigateByUrl('/app/main/reports/ReportView');

        let repParams = "";
        this.report = "GeneralLedgerBookWise";
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
        if (this.ledgerFilters.status !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.status) + "$";
        if (this.ledgerFilters.location !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.location) + "$";

        repParams += encodeURIComponent("" + this.currRate) + "$";
        repParams += encodeURIComponent("" + this.curid) + "$";
        if (this.ledgerFilters.location !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.location) + "$";

        repParams = repParams.replace(/[?$]&/, "");

        this.reportView.show(this.report, repParams);
    }
    getCustomerLedgerReport() {
        // this.parameters = {
        //   "fromDate": moment(this.fromDate).format("YYYY/MM/DD"),
        //   "toDate": moment(this.toDate).format("YYYY/MM/DD"),
        //   "fromAC": this.ledgerFilters.fromAccount,
        //   "toAC": this.ledgerFilters.toAccount,
        //   "fromSubledgerId": this.ledgerFilters.fromSubAccount,
        //   "toSubledgerId": this.ledgerFilters.toSubAccount,
        //   "tenantId": this.appSession.tenantId,
        //   "locId": this.ledgerFilters.location,
        //   "slType": this.ledgerFilters.slType
        // };

        // localStorage.setItem('rptObj', JSON.stringify(this.parameters));
        // localStorage.setItem('rptName', "Subledger");
        // this.route.navigateByUrl('/app/main/reports/ReportView');

        let repParams = "";
        this.report = "Subledger";
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
        if (this.ledgerFilters.fromSubAccount !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.fromSubAccount) +
                "$";
        if (this.ledgerFilters.toSubAccount !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.toSubAccount) + "$";
        if (this.ledgerFilters.location !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.location) + "$";
        if (this.ledgerFilters.slType !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.slType) + "$";
        if (this.ledgerFilters.status !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.status) + "$";

        repParams += encodeURIComponent("" + this.currRate) + "$";

        repParams += encodeURIComponent("" + this.curid) + "$";
        repParams +=
            encodeURIComponent("" + this.ledgerFilters.bookSummary) + "$";

        repParams = repParams.replace(/[?$]&/, "");

        this.reportView.show(this.report, repParams);
    }
    getPartyLedgerReport() {
        // this.parameters = {
        //   "fromDate": moment(this.fromDate).format("YYYY/MM/DD"),
        //   "toDate": moment(this.toDate).format("YYYY/MM/DD"),
        //   "fromAC": this.ledgerFilters.fromAccount,
        //   "toAC": this.ledgerFilters.toAccount,
        //   "status": this.ledgerFilters.status,
        //   "tenantId": this.appSession.tenantId,
        //   "locId": this.ledgerFilters.location
        // };
        let repParams = "";
        this.report = "GeneralLedger";
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
        if (this.ledgerFilters.status !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.status) + "$";
        if (this.ledgerFilters.fromLoc !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.fromLoc) + "$";

        repParams += encodeURIComponent("" + this.currRate) + "$";

        repParams += encodeURIComponent("" + this.curid) + "$";

        if (this.ledgerFilters.toLoc !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.toLoc) + "$";
        if (this.ledgerFilters.fromLoc !== undefined)
        repParams +=
            encodeURIComponent("" + this.ledgerFilters.fromLocName) + "$";
        if (this.ledgerFilters.toLoc !== undefined)
        repParams +=
            encodeURIComponent("" + this.ledgerFilters.toLocName) + "$";
        debugger
        repParams = repParams.replace(/[?$]&/, "");

        this.reportView.show(this.report, repParams);
        // localStorage.setItem('rptObj', JSON.stringify(this.parameters));
        // localStorage.setItem('rptName', "GeneralLedger");
        // this.route.navigateByUrl('/app/main/reports/ReportView');
    }
    getPartyBalanceReport() {
        this.parameters = {
            FromDate: moment(this.fromDate).format("YYYY/MM/DD"),
            ToDate: moment(this.toDate).format("YYYY/MM/DD"),
            FromAcc: this.ledgerFilters.fromAccount,
            ToAcc: this.ledgerFilters.toAccount,
            TenantId: this.appSession.tenantId,
            LocId: this.ledgerFilters.location,
            Posted: this.ledgerFilters.status
        };

        localStorage.setItem("rptObj", JSON.stringify(this.parameters));
        localStorage.setItem("rptName", "PartyBalances");
        this.route.navigateByUrl("/app/main/reports/ReportView");
    }

    getConsolidatedLedger() {
        let repParams = "";

        repParams += "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";

        repParams += "" + moment(this.toDate).format("YYYY/MM/DD") + "$";

        repParams +=
            encodeURIComponent("" + this.ledgerFilters.fromAccount) + "$";

        repParams +=
            encodeURIComponent("" + this.ledgerFilters.toAccount) + "$";

        repParams += encodeURIComponent("" + this.ledgerFilters.status) + "$";

        repParams += encodeURIComponent("" + this.currRate) + "$";

        if (this.ledgerFilters.fromLoc !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.fromLoc) + "$";
        if (this.ledgerFilters.toLoc !== undefined)
            repParams +=
                encodeURIComponent("" + this.ledgerFilters.toLoc) + "$";
        if (this.ledgerFilters.fromLoc !== undefined)
        repParams +=
            encodeURIComponent("" + this.ledgerFilters.fromLocName) + "$";
        if (this.ledgerFilters.toLoc !== undefined)
        repParams +=
            encodeURIComponent("" + this.ledgerFilters.toLocName) + "$";

        repParams = repParams.replace(/[?$]&/, "");

        this.reportView.show("MonthlyConsolidatedLedger", repParams);
    }

    getSubLedger(): void {
        if (this.ledgerFilters.fromAccount) {
            this._reportFilterServiceProxy
                .getAllAccountSubLedgerAccountList(
                    this.ledgerFilters.fromAccount
                )
                .subscribe(result => {
                    this.subLedgerList = result.items;
                });
        }
    }

    getNewFinanceModal() {
        debugger;
        switch (this.target) {
            case "ChartOfAccount":
                this.getNewChartOfAC();
                break;
            case "SubLedger":
                this.getNewSubAcc();
                break;
            case "GLLocation":
                this.getNewLoc();
                break;
            default:
                break;
        }
    }

    //=====================Location Model================
    openSelectLocModal(val) {
        this.target = "GLLocation";
        if (val == "fromLoc") {
            this.FinanceLookupTableModal.id = this.ledgerFilters.fromLoc;
            this.FinanceLookupTableModal.displayName = this.ledgerFilters.fromLocName;
        } else if (val == "toLoc") {
            this.FinanceLookupTableModal.id = this.ledgerFilters.toLoc;
            this.FinanceLookupTableModal.displayName = this.ledgerFilters.toLocName;
        }
        this.FinanceLookupTableModal.show(this.target, "", "", " Location");
        this.picker = val;
    }

    setLocIDNull(val) {
        if (val == "fromLoc") {
            this.ledgerFilters.fromLoc = "0";
            this.ledgerFilters.fromLocName = "";
        } else if (val == "toLoc") {
            this.ledgerFilters.toLoc = "99999";
            this.ledgerFilters.toLocName = "";
        }
    }

    getNewLoc() {
        debugger;
        if (this.picker == "fromLoc") {
            this.ledgerFilters.fromLoc = this.FinanceLookupTableModal.id;
            this.ledgerFilters.fromLocName = this.FinanceLookupTableModal.displayName;
            this.ledgerFilters.toLoc = this.FinanceLookupTableModal.id;
            this.ledgerFilters.toLocName = this.FinanceLookupTableModal.displayName;
        } else if (this.picker == "toLoc") {
            this.ledgerFilters.toLoc = this.FinanceLookupTableModal.id;
            this.ledgerFilters.toLocName = this.FinanceLookupTableModal.displayName;
        }
    }
    //=====================Loc Model================

    //=====================Chart of Ac Model================
    openSelectChartofACModal(val) {
        debugger;
        this.target = "ChartOfAccount";
        if (val == "FromAC") {
            this.FinanceLookupTableModal.id = this.ledgerFilters.fromAccount;
            this.FinanceLookupTableModal.displayName = this.ledgerFilters.fromAccountName;
            this.FinanceLookupTableModal.id = this.ledgerFilters.toAccount;
            this.FinanceLookupTableModal.displayName = this.ledgerFilters.toAccountName;
        } else if (val == "ToAC") {
            this.FinanceLookupTableModal.id = this.ledgerFilters.toAccount;
            this.FinanceLookupTableModal.displayName = this.ledgerFilters.toAccountName;
        }
        this.FinanceLookupTableModal.show(this.target);
        this.picker = val;
    }

    setAccountIDNull(val) {
        if (val == "FromAC") {
            this.ledgerFilters.fromAccount = "00-000-00-0000";
            this.ledgerFilters.fromAccountName = "";
            this.setSubAccIDNull("FromSL");
        } else if (val == "ToAC") {
            this.ledgerFilters.toAccount = "99-999-99-9999";
            this.ledgerFilters.toAccountName = "";
            this.setSubAccIDNull("ToSL");
        }
    }

    getNewChartOfAC() {
        debugger;
        if (this.picker == "FromAC") {
            this.ledgerFilters.fromAccount = this.FinanceLookupTableModal.id;
            this.ledgerFilters.fromAccountName = this.FinanceLookupTableModal.displayName;
            this.ledgerFilters.toAccount = this.FinanceLookupTableModal.id;
            this.ledgerFilters.toAccountName = this.FinanceLookupTableModal.displayName;
        } else if (this.picker == "ToAC") {
            this.ledgerFilters.toAccount = this.FinanceLookupTableModal.id;
            this.ledgerFilters.toAccountName = this.FinanceLookupTableModal.displayName;
        }
    }
    //=====================Chart of Ac Model================

    //=====================Sub Account Model================
    openSelectSubAccModal(val) {
        debugger;
        var account = this.ledgerFilters.fromAccount;
        var slType = String(this.ledgerFilters.slType);
        if (account == "" || account == null) {
            this.message.warn(
                this.l("Please select account first"),
                "Account Required"
            );
            return;
        }
        if (account == "00-000-00-0000") {
            account = "";
        }
        if (slType == "0") {
            slType = "";
        }
        this.target = "SubLedger";
        this.FinanceLookupTableModal.id = this.ledgerFilters.fromSubAccount;
        this.FinanceLookupTableModal.displayName = this.ledgerFilters.fromSubAccountName;
        this.FinanceLookupTableModal.id = this.ledgerFilters.toSubAccount;
        this.FinanceLookupTableModal.displayName = this.ledgerFilters.toSubAccountName;
        this.FinanceLookupTableModal.show(this.target, account, slType);
        this.picker = val;
    }

    setSubAccIDNull(val) {
        if (val == "FromSL") {
            this.ledgerFilters.fromSubAccount = "0";
            this.ledgerFilters.fromSubAccountName = "";
        } else if (val == "ToSL") {
            this.ledgerFilters.toSubAccount = "99999";
            this.ledgerFilters.toSubAccountName = "";
        }
    }

    getNewSubAcc() {
        if (this.picker == "FromSL") {
            this.ledgerFilters.fromSubAccount = this.FinanceLookupTableModal.id;
            this.ledgerFilters.fromSubAccountName = this.FinanceLookupTableModal.displayName;
            this.ledgerFilters.toSubAccount = this.FinanceLookupTableModal.id;
            this.ledgerFilters.toSubAccountName = this.FinanceLookupTableModal.displayName;
        } else if (this.picker == "ToSL") {
            this.ledgerFilters.toSubAccount = this.FinanceLookupTableModal.id;
            this.ledgerFilters.toSubAccountName = this.FinanceLookupTableModal.displayName;
        }
    }
    //=====================Sub Account Model================

    setToAllAccount() {
        this.ledgerFilters.toSubAccount = "99999";
        this.ledgerFilters.fromAccountName = "";
        this.ledgerFilters.fromSubAccountName = "";
        this.ledgerFilters.fromSubAccount = "0";
        this.ledgerFilters.fromAccount = "000-000-00-0000";
        this.ledgerFilters.toSubAccountName = "";
        this.ledgerFilters.toAccount = "99-999-99-9999";
        this.ledgerFilters.toAccountName = "";
    }

    getNewCommonServiceModal() {
        debugger;
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

    getLedgerTypes() {
        this._LegderTypeComboboxService
            .getLedgerTypesForCombobox("")
            .subscribe(res => {
                this.ledgerTypes = res.items;
            });
    }
}
