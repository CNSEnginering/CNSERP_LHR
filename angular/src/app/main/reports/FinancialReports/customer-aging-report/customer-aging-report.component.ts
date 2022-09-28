import { Component, OnInit, ViewEncapsulation, ViewChild, EventEmitter, Output, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
import { Router } from '@angular/router';
import { FiscalDateService } from '@app/shared/services/fiscalDate.service';
import * as moment from 'moment';
import { LedgerFiltersDto } from '../../dto/ledger-filters-dto';

@Component({
    selector: 'app-customer-aging-report',
    templateUrl: './customer-aging-report.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class CustomerAgingReportComponent extends AppComponentBase {


    @ViewChild("fromAccountFinder", { static: true }) fromAccountFinder: FinanceLookupTableModalComponent;
    @ViewChild("toAccountFinder", { static: true }) toAccountFinder: FinanceLookupTableModalComponent;

    @ViewChild("fromsubledgerAccountFinder", { static: true }) fromsubledgerAccountFinder: FinanceLookupTableModalComponent;
    @ViewChild("tosubledgerAccountFinder", { static: true }) tosubledgerAccountFinder: FinanceLookupTableModalComponent;

    @ViewChild("reportView", { static: true }) reportView: ReportviewrModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    asonDate = new Date();

    fromDate;
    toDate;
    reportServer: string;
    reportUrl: string;
    showParameters: string;
    parameters: {};
    language: string;
    width: number;
    height: number;
    toolbar: string;
    FromAccountID = "00-000-00-0000";
    ToAccountID = "99-999-99-9999";
    fromSubAccID = 0;
    toSubAccID = 99999;
    agingperiod1 = 30;
    agingperiod2 = 60;
    agingPeriod3 = 90;
    agingPeriod4 = 91;
    agingPeriod5 = 91;
    fromAccountName: string;
    fromSubAccountName: string;
    toAccountName: string;
    toSubAccountName: string;
    checkSubAccount: boolean = false;
    checkAccount: boolean = false;
    ledgerFilters: LedgerFiltersDto = new LedgerFiltersDto();
    rptObj: any;
    type: string;

    constructor(
        injector: Injector,
        private route: Router,
        private _reportService: FiscalDateService

    ) {
        super(injector);
    }

    ngOnInit() {
    }

    selectFromAccount() {
        this.fromAccountFinder.id = this.FromAccountID;
        this.fromAccountFinder.displayName = this.fromAccountName;
        this.fromAccountFinder.show("ChartOfAccount");
    }

    setFromAccount() {
        this.FromAccountID = "00-000-00-0000";
        this.fromAccountName = "";
    }

    getFromAccount() {
        debugger;
        this.FromAccountID = this.fromAccountFinder.id;
        this.fromAccountName = this.fromAccountFinder.displayName;
        if (this.FromAccountID !== "00-000-00-0000") {
            this.ToAccountID = this.FromAccountID;
            this.toAccountName = this.fromAccountName;
        }
    }

    selectToAccount() {
        this.toAccountFinder.id = this.ToAccountID;
        this.toAccountFinder.displayName = this.toAccountName;
        this.toAccountFinder.show("ChartOfAccount");
    }

    setToAccount() {
        this.ToAccountID = "99-999-99-9999";
        this.toAccountName = "";
    }

    getToAccount() {
        this.ToAccountID = this.toAccountFinder.id;
        this.toAccountName = this.toAccountFinder.displayName;
    }

    selectFromSubAccount() {
        debugger;
        this.fromsubledgerAccountFinder.id = this.fromSubAccID.toString();
        this.fromsubledgerAccountFinder.displayName = this.fromSubAccountName;
        this.fromsubledgerAccountFinder.show("SubLedger", this.FromAccountID);
    }

    selectToSubAccount() {
        this.tosubledgerAccountFinder.id = this.toSubAccID.toString();
        this.tosubledgerAccountFinder.displayName = this.toSubAccountName;
        this.tosubledgerAccountFinder.show("SubLedger", this.FromAccountID);
    }

    setFromSubAccount() {
        this.fromSubAccID = 0;
        this.fromSubAccountName = "";
    }

    setToSubAccount() {
        this.toSubAccID = 99999;
        this.toSubAccountName = "";
    }

    getFromSubAccount() {
        debugger;
        this.fromSubAccID = Number(this.fromsubledgerAccountFinder.id);
        this.fromSubAccountName = this.fromsubledgerAccountFinder.displayName;
        if (this.fromSubAccID !== 0) {
            this.toSubAccID = this.fromSubAccID;
            this.toSubAccountName = this.fromSubAccountName;
        }
    }

    getToSubAccount() {
        this.toSubAccID = Number(this.tosubledgerAccountFinder.id);
        this.toSubAccountName = this.tosubledgerAccountFinder.displayName;
    }

    getReport() {
        debugger;
        this.rptObj = JSON.stringify({
            asonDate: moment(this.fromDate).format("YYYY/MM/DD"),
            ToDate: moment(this.toDate).format("YYYY/MM/DD"),
            FromAccountID: this.FromAccountID,
            ToAccountID: this.ToAccountID,
            FromSubAccID: this.fromSubAccID,
            ToSubAccID: this.toSubAccID,
            TenantID: this.appSession.tenantId
        });

        let repParams = "";

        repParams = repParams.replace(/[?$]&/, "");
        //if (this.ledgerFilters.reportType === "AgingReport") {
        if (this.asonDate !== undefined)
            repParams += "" + moment(this.asonDate).format("YYYY/MM/DD") + "$";
        if (this.FromAccountID !== undefined)
            repParams += encodeURIComponent("" + this.FromAccountID) + "$";
        if (this.ToAccountID !== undefined)
            repParams += encodeURIComponent("" + this.ToAccountID) + "$";
        if (this.fromSubAccID !== undefined)
            repParams += encodeURIComponent("" + this.fromSubAccID) + "$";
        if (this.toSubAccID !== undefined)
            repParams += encodeURIComponent("" + this.toSubAccID) + "$";
        if (this.agingperiod1 !== undefined)
            repParams += encodeURIComponent("" + this.agingperiod1) + "$";
        if (this.agingperiod2 !== undefined)
            repParams += encodeURIComponent("" + this.agingperiod2) + "$";
        if (this.agingPeriod3 !== undefined)
            repParams += encodeURIComponent("" + this.agingPeriod3) + "$";
        if (this.agingPeriod4 !== undefined)
            repParams += encodeURIComponent("" + this.agingPeriod4) + "$";
        if (this.agingPeriod5 !== undefined)
            repParams += encodeURIComponent("" + this.agingPeriod5) + "$";
        if (this.ledgerFilters.reportType === "AgingReport") {
            this.reportView.show("CUSTOMERAGING", repParams);
        } else {
            this.reportView.show("BillWiseAgingReport", repParams);
        }
        // }
        // else if (this.ledgerFilters.reportType === "BillWiseAgingReport") {
        // if (this.asonDate !== undefined)
        //     repParams += "" + moment(this.asonDate).format("YYYY/MM/DD") + "$";
        // if (this.FromAccountID !== undefined)
        //     repParams += encodeURIComponent("" + this.FromAccountID) + "$";
        // if (this.ToAccountID !== undefined)
        //     repParams += encodeURIComponent("" + this.ToAccountID) + "$";
        // if (this.fromSubAccID !== undefined)
        //     repParams += encodeURIComponent("" + this.fromSubAccID) + "$";
        // if (this.toSubAccID !== undefined)
        //     repParams += encodeURIComponent("" + this.toSubAccID) + "$";

        // repParams += encodeURIComponent("" + this.type) + "$";

        //}
    }

    reset() {
        this.asonDate = new Date();;
        this.FromAccountID = "00-000-00-0000";
        this.fromAccountName = "";
        this.ToAccountID = "99-999-99-9999";
        this.toAccountName = "";
        this.fromSubAccID = 0;
        this.fromSubAccountName = "";
        this.toSubAccID = 99999;
        this.toSubAccountName = "";
        this.agingperiod1 = 30;
        this.agingperiod2 = 60;
        this.agingPeriod3 = 90;
        this.agingPeriod4 = 91;
        this.agingPeriod5 = 91;

    }

}
