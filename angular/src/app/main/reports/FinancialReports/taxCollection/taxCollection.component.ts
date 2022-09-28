import { Component, Injector, ViewEncapsulation, ViewChild, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import * as _ from 'lodash';
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
import { CommonServiceLookupTableModalComponent } from '@app/finders/commonService/commonService-lookup-table-modal.component';
import { FiscalDateService } from '@app/shared/services/fiscalDate.service';
import * as moment from "moment";
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';

@Component({
    templateUrl: './taxCollection.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TaxCollectionComponent extends AppComponentBase {

    @ViewChild("FinanceLookupTableModal", { static: true })
    FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild('commonServiceLookupTableModal', { static: true }) commonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('reportView', { static: true }) reportView: ReportviewrModalComponent;

    taxCollection: string;

    target: string;
    taxAuth: string;
    taxAuthDesc: string;
    fromTaxClass: string;
    fromTaxClassDesc: string;
    toTaxClass: string;
    toTaxClassDesc: string;
    fromAcc: string;
    fromAccDesc: string;
    toAcc: string;
    toAccDesc: string;
    fromSubAcc: string;
    fromSubAccDesc: string;
    toSubAcc: string;
    toSubAccDesc: string;
    showFromAcc: boolean = true;
    showToAcc: boolean = true;
    showFromSubAcc: boolean = true;
    showToSubAcc: boolean = true;
    fromDate: Date;
    toDate: Date;
    picker: string;
    type: string;
    reportType: string;

    constructor(
        injector: Injector,
        private route: Router,
        private _reportService: FiscalDateService
    ) {
        super(injector);
    }

    ngOnInit() {
        this._reportService.getDate().subscribe(data => {
            this.fromDate = new Date(data["result"]);
            this.toDate = new Date();
        });
        this.fromAcc = "00-000-00-0000";
        this.toAcc = "99-999-99-9999";
        this.fromSubAcc = "0";
        this.toSubAcc = "99999";
    }
    hideFields() {
        switch (this.taxCollection) {
            case 'salesTaxWithheld':
                this.showFromAcc = true;
                this.showToAcc = true;
                this.showFromSubAcc = true;
                this.showToSubAcc = true;
                break;
            case 'partyTax':
                this.showFromAcc = true;
                this.showToAcc = true;
                this.showFromSubAcc = true;
                this.showToSubAcc = true;
                break;
            case 'CPRNumbers':
                this.showFromAcc = true;
                this.showToAcc = true;
                this.showFromSubAcc = true;
                this.showToSubAcc = true;
                break;

        }
    }
    getReport() {
        debugger;
        this.type = ""
        this.reportType = ""
        switch (this.taxCollection) {
            case 'salesTaxOutput':
                this.type = "AR";
                this.reportType = "Output";
                this.processReport("salesTaxWithheld");
                break;
            case 'salesTaxInput':
                this.type = "AP";
                this.reportType = "Input";
                this.processReport("salesTaxWithheld");
                break;
            case 'partyTax':
                this.processReport("PartyTax");
                break;
            case 'CPRNumbers':
                this.processReport("CPRNumbers");
                break;
            case 'incomeTax':
                this.type = "AP";
                this.reportType = "Income";
                this.processReport("taxDueReport");
                break;
            // case 'salesTax':
            //     this.type="AR";
            //     //this.reportType = "Sales";
            //     this.processReport("taxDueReport");
            case 'salesTaxDeduction':
                this.processReport("SalesTaxDeduction");
                break;
            default:
                break;
        }
    }

    processReport(report: string) {
        debugger;
        let repParams = '';

        switch (report) {
            case 'salesTaxWithheld':
                if (this.fromDate !== undefined)
                    repParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    repParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                if (this.fromAcc !== undefined)
                    repParams += encodeURIComponent("" + this.fromAcc) + "$";
                if (this.toAcc !== undefined)
                    repParams += encodeURIComponent("" + this.toAcc) + "$";
                if (this.fromSubAcc !== undefined)
                    repParams += encodeURIComponent("" + this.fromSubAcc) + "$";
                if (this.toSubAcc !== undefined)
                    repParams += encodeURIComponent("" + this.toSubAcc) + "$";
                if (this.taxAuth !== undefined)
                    repParams += encodeURIComponent("" + this.taxAuth) + "$";
                if (this.fromTaxClass !== undefined)
                    repParams += encodeURIComponent("" + this.fromTaxClass) + "$";
                if (this.taxAuthDesc !== undefined)
                    repParams += encodeURIComponent("" + this.taxAuthDesc) + "$";
                if (this.fromTaxClassDesc !== undefined)
                    repParams += encodeURIComponent("" + this.fromTaxClassDesc) + "$";
                if (this.type !== undefined)
                    repParams += encodeURIComponent("" + this.type) + "$";
                if (this.reportType !== undefined)
                    repParams += encodeURIComponent("" + this.reportType) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'PartyTax':
                if (this.fromDate !== undefined)
                    repParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    repParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                if (this.fromAcc !== undefined)
                    repParams += encodeURIComponent("" + this.fromAcc) + "$";
                if (this.toAcc !== undefined)
                    repParams += encodeURIComponent("" + this.toAcc) + "$";
                if (this.fromSubAcc !== undefined)
                    repParams += encodeURIComponent("" + this.fromSubAcc) + "$";
                if (this.toSubAcc !== undefined)
                    repParams += encodeURIComponent("" + this.toSubAcc) + "$";
                if (this.taxAuth !== undefined)
                    repParams += encodeURIComponent("" + this.taxAuth) + "$";
                if (this.taxAuthDesc !== undefined)
                    repParams += encodeURIComponent("" + this.taxAuthDesc) + "$";
                if (this.fromTaxClass !== undefined)
                    repParams += encodeURIComponent("" + this.fromTaxClass) + "$";
                if (this.toTaxClass !== undefined)
                    repParams += encodeURIComponent("" + this.toTaxClass) + "$";
                if (this.fromTaxClassDesc !== undefined)
                    repParams += encodeURIComponent("" + this.fromTaxClassDesc) + "$";
                if (this.toTaxClassDesc !== undefined)
                    repParams += encodeURIComponent("" + this.toTaxClassDesc) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'CPRNumbers':
                if (this.fromDate !== undefined)
                    repParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    repParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                if (this.fromAcc !== undefined)
                    repParams += encodeURIComponent("" + this.fromAcc) + "$";
                if (this.toAcc !== undefined)
                    repParams += encodeURIComponent("" + this.toAcc) + "$";
                if (this.fromSubAcc !== undefined)
                    repParams += encodeURIComponent("" + this.fromSubAcc) + "$";
                if (this.toSubAcc !== undefined)
                    repParams += encodeURIComponent("" + this.toSubAcc) + "$";
                if (this.taxAuth !== undefined)
                    repParams += encodeURIComponent("" + this.taxAuth) + "$";
                if (this.fromTaxClass !== undefined)
                    repParams += encodeURIComponent("" + this.fromTaxClass) + "$";
                if (this.taxAuthDesc !== undefined)
                    repParams += encodeURIComponent("" + this.taxAuthDesc) + "$";
                if (this.fromTaxClassDesc !== undefined)
                    repParams += encodeURIComponent("" + this.fromTaxClassDesc) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'taxDueReport':
                if (this.fromDate !== undefined)
                    repParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    repParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                if (this.fromAcc !== undefined)
                    repParams += encodeURIComponent("" + this.fromAcc) + "$";
                if (this.toAcc !== undefined)
                    repParams += encodeURIComponent("" + this.toAcc) + "$";
                if (this.fromSubAcc !== undefined)
                    repParams += encodeURIComponent("" + this.fromSubAcc) + "$";
                if (this.toSubAcc !== undefined)
                    repParams += encodeURIComponent("" + this.toSubAcc) + "$";
                if (this.taxAuth !== undefined)
                    repParams += encodeURIComponent("" + this.taxAuth) + "$";
                if (this.fromTaxClass !== undefined)
                    repParams += encodeURIComponent("" + this.fromTaxClass) + "$";
                if (this.taxAuthDesc !== undefined)
                    repParams += encodeURIComponent("" + this.taxAuthDesc) + "$";
                if (this.fromTaxClassDesc !== undefined)
                    repParams += encodeURIComponent("" + this.fromTaxClassDesc) + "$";
                if (this.type !== undefined)
                    repParams += encodeURIComponent("" + this.type) + "$";
                if (this.reportType !== undefined)
                    repParams += encodeURIComponent("" + this.reportType) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'SalesTaxDeduction':
                if (this.fromDate !== undefined)
                    repParams +=
                        "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    repParams +=
                        "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                if (this.fromAcc !== undefined)
                    repParams += encodeURIComponent("" + this.fromAcc) + "$";
                if (this.toAcc !== undefined)
                    repParams += encodeURIComponent("" + this.toAcc) + "$";
                if (this.fromSubAcc !== undefined)
                    repParams += encodeURIComponent("" + this.fromSubAcc) + "$";
                if (this.toSubAcc !== undefined)
                    repParams += encodeURIComponent("" + this.toSubAcc) + "$";
                if (this.taxAuth !== undefined)
                    repParams += encodeURIComponent("" + this.taxAuth) + "$";
                if (this.fromTaxClass !== undefined)
                    repParams += encodeURIComponent("" + this.fromTaxClass) + "$";
                if (this.taxAuthDesc !== undefined)
                    repParams += encodeURIComponent("" + this.taxAuthDesc) + "$";
                if (this.fromTaxClassDesc !== undefined)
                    repParams += encodeURIComponent("" + this.fromTaxClassDesc) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            default:
                break;
        }
        this.reportView.show(report, repParams);
    }


    getNewCommonServiceModal() {
        switch (this.target) {
            case "TaxAuthority":
                this.getNewTaxAuthority();
                break;
            case "TaxClass":
                this.getNewTaxClass();
                break;
            default:
                break;
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
            default:
                break;
        }
    }

    //=====================Chart of Ac Model================
    openSelectChartofACModal(val) {
        debugger;
        this.target = "ChartOfAccount";
        if (val == "FromAC") {
            this.FinanceLookupTableModal.id = this.fromAcc;
            this.FinanceLookupTableModal.displayName = this.fromAccDesc;
            this.FinanceLookupTableModal.id = this.toAcc;
            this.FinanceLookupTableModal.displayName = this.toAccDesc;
        } else if (val == "ToAC") {
            this.FinanceLookupTableModal.id = this.toAcc;
            this.FinanceLookupTableModal.displayName = this.toAccDesc;
        }
        this.FinanceLookupTableModal.show(this.target);
        this.picker = val;
    }

    setChartOfACNull(val) {
        debugger;
        if (val == "FromAC") {
            this.fromAcc = "00-000-00-0000";
            this.fromAccDesc = "";
            this.setSubAccNull("FromSubAcc");
        } else if (val == "ToAC") {
            this.toAcc = "99-999-99-9999";
            this.toAccDesc = "";
            this.setSubAccNull("ToSubAcc");
        }
    }

    getNewChartOfAC() {
        debugger;
        if (this.picker == "FromAC") {
            this.fromAcc = this.FinanceLookupTableModal.id;
            this.fromAccDesc = this.FinanceLookupTableModal.displayName;
            this.toAcc = this.FinanceLookupTableModal.id;
            this.toAccDesc = this.FinanceLookupTableModal.displayName;
        } else if (this.picker == "ToAC") {
            this.toAcc = this.FinanceLookupTableModal.id;
            this.toAccDesc = this.FinanceLookupTableModal.displayName;
        }
    }
    //=====================Chart of Ac Model================//
    //=====================Sub Account Model================
    openSelectSubAccModal(val) {
        debugger;
        var account = this.fromAcc;
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
        this.target = "SubLedger";

        if (val == "FromSubAC") {
            this.FinanceLookupTableModal.id = this.fromSubAcc;
            this.FinanceLookupTableModal.displayName = this.fromSubAccDesc;
            this.FinanceLookupTableModal.id = this.toSubAcc;
            this.FinanceLookupTableModal.displayName = this.toSubAccDesc;
        }
        else if (val == "ToSubAC") {
            this.FinanceLookupTableModal.id = this.toSubAcc;
            this.FinanceLookupTableModal.displayName = this.toSubAccDesc;
        }
        this.FinanceLookupTableModal.show(this.target, account);
        this.picker = val;
    }

    setSubAccNull(val) {
        if (val == "FromSubAcc") {
            this.fromSubAcc = "0";
            this.fromSubAccDesc = "";
        } else if (val == "ToSubAcc") {
            this.toSubAcc = "99999";
            this.toSubAccDesc = "";
        }
    }

    getNewSubAcc() {
        if (this.picker == "FromSubAcc") {
            this.fromSubAcc = this.FinanceLookupTableModal.id;
            this.fromSubAccDesc = this.FinanceLookupTableModal.displayName;
            this.toSubAcc = this.FinanceLookupTableModal.id;
            this.toSubAccDesc = this.FinanceLookupTableModal.displayName;
        } else if (this.picker == "ToSubAcc") {
            this.toSubAcc = this.FinanceLookupTableModal.id;
            this.toSubAccDesc = this.FinanceLookupTableModal.displayName;
        }
    }
    //=====================Sub Account Model================


    //=====================Tax Authority Model================
    openSelectTaxAuthorityModal() {
        this.target = "TaxAuthority";
        this.commonServiceLookupTableModal.id = this.taxAuth;
        this.commonServiceLookupTableModal.displayName = this.taxAuthDesc;
        this.commonServiceLookupTableModal.show(this.target);
    }

    setTaxAuthorityIdNull() {
        this.taxAuth = '';
        this.taxAuthDesc = '';
        this.setTaxClassIdNull("FromTax");
        this.setTaxClassIdNull("ToTax");
    }

    getNewTaxAuthority() {
        if (this.commonServiceLookupTableModal.id != this.taxAuth) {
            this.setTaxClassIdNull("FromTax");
            this.setTaxClassIdNull("ToTax");
        }
        this.taxAuth = this.commonServiceLookupTableModal.id;
        this.taxAuthDesc = this.commonServiceLookupTableModal.displayName;
    }
    //=====================Tax Authority Model================

    //=====================Tax Class================
    openSelectTaxClassModal(type: string) {
        debugger;
        if (this.taxAuth == "" || this.taxAuth == null) {
            this.message.warn(this.l('Please select Tax authority'), 'Tax Authority Required');
            return;
        }
        this.target = "TaxClass";

        if (type == "FromTax") {
            this.commonServiceLookupTableModal.id = this.fromTaxClass;
            this.commonServiceLookupTableModal.displayName = this.fromTaxClassDesc;
        }

        else if (type == "ToTax") {
            this.commonServiceLookupTableModal.id = this.toTaxClass;
            this.commonServiceLookupTableModal.displayName = this.toTaxClassDesc;
        }
        this.commonServiceLookupTableModal.show(this.target, this.taxAuth);
        this.picker = type;
    }
    getNewTaxClass() {
        if (this.picker == "FromTax") {

            this.fromTaxClass = this.commonServiceLookupTableModal.id;
            this.fromTaxClassDesc = this.commonServiceLookupTableModal.displayName;
        }
        else if (this.picker == "ToTax") {

            this.toTaxClass = this.commonServiceLookupTableModal.id;
            this.toTaxClassDesc = this.commonServiceLookupTableModal.displayName;
        }
    }
    setTaxClassIdNull(type: string) {
        debugger
        if (type == "FromTax") {
            this.fromTaxClass = null;
            this.fromTaxClassDesc = '';
        }

        if (type == "ToTax") {
            this.toTaxClass = null;
            this.toTaxClassDesc = '';
        }
    }
    //=====================Tax Class================

}

