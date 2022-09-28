import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import * as _ from 'lodash';
import * as moment from "moment";
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
import { FiscalDateService } from '@app/shared/services/fiscalDate.service';
@Component({
    templateUrl: './setupReports.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class GLSetupReportsComponent extends AppComponentBase implements OnInit {
    @ViewChild('reportView', { static: true }) reportView: ReportviewrModalComponent;

    fromCode = 0;
    toCode = 999999;
    typeID = "";
    fromDate: Date;
    toDate: Date;
    showDatesCheck: boolean = true;
    showCodes: boolean = true;

    documentListing: string;

    constructor(
        injector: Injector,
        private route: Router,
        private _reportService: FiscalDateService
    ) {
        super(injector);
    }

    ngOnInit() {
        this.toDate = new Date();
        this._reportService.getDate().subscribe(data => {
            this.fromDate = new Date(data["result"]);
        });
    }

    getReport() {
        switch (this.documentListing) {
            case 'LedgerTypesListing':
                this.processReport("GLLedgerTypes");
                break;
            case 'GLlocationsListing':
                this.processReport("GLlocations");
                break;
            case 'GLCategoriesListing':
                this.processReport("GLCategories");
                break;
            case 'GLGroupListing':
                this.processReport("GLGroup");
                break;
            case 'Level1Listing':
                this.processReport("GLLevel1");
                break;
            case 'Level2Listing':
                this.processReport("GLLevel2");
                break;
            case 'Level3Listing':
                this.processReport("GLLevel3");
                break;
            case 'GLbooksListing':
                this.processReport("GLbooks");
                break;
            case 'GLConfigurationListing':
                this.processReport("GLConfiguration");
                break;
            case 'PostDatedChequeReceived':
                this.typeID = "1";
                this.processReport("PostDatedCheque");
                break;
            case 'PostDatedChequeIssued':
                this.typeID = "0";
                this.processReport("PostDatedCheque");
                break;
            default:
                break;
        }
    }

    processReport(report: string) {
        debugger;
        let repParams = '';

        switch (report) {
            case 'GLLedgerTypes':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'GLlocations':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'GLCategories':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'GLGroup':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'GLLevel1':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'GLLevel2':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'GLLevel3':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'GLbooks':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'GLConfiguration':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'PostDatedCheque':
                if (!this.toDate) {
                    this.notify.error(this.l('Please select "To Date"'));
                    return;
                }
                if (!this.fromDate) {
                    this.notify.error(this.l('Please select "From Date"'));
                    return;
                }
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                repParams += "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                repParams += "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                // if (this.fromDate !== undefined)
                //     repParams += encodeURIComponent("" + this.fromDate) + "$";
                // if (this.toDate !== undefined)
                //     repParams += encodeURIComponent("" + this.toDate) + "$";
                if (this.typeID !== undefined)
                    repParams += encodeURIComponent("" + this.typeID) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            default:
                break;
        }
        this.reportView.show(report, repParams);
    }

    hideFields() {
        debugger;
        if (this.documentListing === 'PostDatedChequeReceived' || this.documentListing === 'PostDatedChequeIssued' ) {
            this.showDatesCheck = true;
            this.showCodes = true;
        }
        else if(this.documentListing === 'CPRNumbers')
        {
            this.showDatesCheck = true;
            this.showCodes = false;
        }
        else {
            this.showDatesCheck = false;
            this.showCodes = true;
        }
    }
}

