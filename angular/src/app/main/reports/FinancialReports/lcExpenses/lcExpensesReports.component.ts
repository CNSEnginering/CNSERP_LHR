import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import * as _ from 'lodash';
import * as moment from "moment";
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
import { FiscalDateService } from '@app/shared/services/fiscalDate.service';
@Component({
    templateUrl: './lcExpensesReports.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class LCExpensesReportsComponent extends AppComponentBase implements OnInit {
    @ViewChild('reportView', { static: true }) reportView: ReportviewrModalComponent;

    fromCode = 0;
    toCode = 999999;
    typeID = "";
    fromDate: Date;
    toDate: Date;

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
            case 'LCExpenses':
                this.processReport("LCExpenses");
                break;
        }
    }

    processReport(report: string) {
        debugger;
        let repParams = '';

        switch (report) {

            case 'LCExpenses':
                if (this.fromDate !== undefined)
                    repParams += 
                    "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    repParams +=
                    "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            default:
                break;
        }
        this.reportView.show(report, repParams);
    }

}

