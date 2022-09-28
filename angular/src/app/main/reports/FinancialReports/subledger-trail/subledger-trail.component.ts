import {
    Component,
    ViewEncapsulation,
    Injector,
    ViewChild,
    Output,
    EventEmitter,
    OnInit
} from "@angular/core";
import * as _ from "lodash";
import * as moment from "moment";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AppComponentBase } from "@shared/common/app-component-base";
import { Router } from "@angular/router";
import { FiscalDateService } from "@app/shared/services/fiscalDate.service";
import { LegderTypeComboboxService } from "@app/shared/common/legdertype-combobox/legdertype-combobox.service";
import { FinanceLookupTableModalComponent } from "@app/finders/finance/finance-lookup-table-modal.component";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";

@Component({
    selector: "app-subledger-trail",
    templateUrl: "./subledger-trail.component.html",
    styleUrls: ["./subledger-trail.component.css"],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class SubledgerTrailComponent extends AppComponentBase
    implements OnInit {
    @ViewChild("fromAccountFinder", { static: true })
    fromAccountFinder: FinanceLookupTableModalComponent;
    @ViewChild("toAccountFinder", { static: true })
    toAccountFinder: FinanceLookupTableModalComponent;

    @ViewChild("fromsubledgerAccountFinder", { static: true })
    fromsubledgerAccountFinder: FinanceLookupTableModalComponent;
    @ViewChild("tosubledgerAccountFinder", { static: true })
    tosubledgerAccountFinder: FinanceLookupTableModalComponent;

    @ViewChild("reportView", { static: true })
    reportView: ReportviewrModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    // fromDate: Date = moment()
    //     .startOf("day")
    //     .toDate();
    // toDate: Date = moment()
    //     .endOf("day")
    //     .toDate();
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
    fromAccountName: string;
    fromSubAccountName: string;
    toAccountName: string;
    toSubAccountName: string;
    checkSubAccount: boolean = false;
    checkAccount: boolean = false;

    rptObj: any;

    slType: number;
    ledgerTypes: any[];

    constructor(
        injector: Injector,

        private _LegderTypeComboboxService: LegderTypeComboboxService,
        private route: Router,
        private _reportService: FiscalDateService
    ) {
        super(injector);
    }

    ngOnInit() {
        this.slType = 0;
        // this._reportService.getDate().subscribe(
        //   data => {
        //     var fromDate = moment(data["result"]).format("MM/DD/YYYY");
        //     console.log(data["result"])
        //     console.log(fromDate)
        //     this.fromDate = moment(fromDate).toDate();
        //   })

        this._reportService.getDate().subscribe(data => {
            this.fromDate = new Date(data["result"]);
            this.toDate = new Date();
        });
        this.getLedgerTypes();
    }

    selectFromAccount() {
        this.fromAccountFinder.id = this.FromAccountID;
        this.fromAccountFinder.displayName = this.fromAccountName;
        this.fromAccountFinder.show("ChartOfAccount");
    }

    setFromAccount() {
        this.FromAccountID = "00-000-00-0000";
        this.fromAccountName = "";
        //this.setToAccount();
    }

    getFromAccount() {
        debugger;
        this.FromAccountID = this.fromAccountFinder.id;
        this.fromAccountName = this.fromAccountFinder.displayName;
        // this.ToAccountID  = this.toAccountFinder.id;
        // this.toAccountName = this.toAccountFinder.displayName;
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
        this.fromSubAccID = Number(this.fromsubledgerAccountFinder.id);
        this.fromSubAccountName = this.fromsubledgerAccountFinder.displayName;
    }

    getToSubAccount() {
        this.toSubAccID = Number(this.tosubledgerAccountFinder.id);
        this.toSubAccountName = this.tosubledgerAccountFinder.displayName;
    }

    getReport() {
        debugger;
        this.rptObj = JSON.stringify({
            FromDate: moment(this.fromDate).format("YYYY/MM/DD"),
            ToDate: moment(this.toDate).format("YYYY/MM/DD"),
            FromAccountID: this.FromAccountID,
            ToAccountID: this.ToAccountID,
            FromSubAccID: this.fromSubAccID,
            ToSubAccID: this.toSubAccID,
            SLType: this.slType != 0 ? this.slType : null,
            TenantID: this.appSession.tenantId
        });

        let repParams = "";
        if (this.fromDate !== undefined)
            repParams += "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
        if (this.toDate !== undefined)
            repParams += "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
        if (this.FromAccountID !== undefined)
            repParams += encodeURIComponent("" + this.FromAccountID) + "$";
        if (this.ToAccountID !== undefined)
            repParams += encodeURIComponent("" + this.ToAccountID) + "$";
        if (this.fromSubAccID !== undefined)
            repParams += encodeURIComponent("" + this.fromSubAccID) + "$";
        if (this.toSubAccID !== undefined)
            repParams += encodeURIComponent("" + this.toSubAccID) + "$";
        if (this.slType !== undefined)
            repParams += encodeURIComponent("" + this.slType) ;
        repParams = repParams.replace(/[?$]&/, "");

        // localStorage.setItem('rptObj', JSON.stringify(this.rptObj));
        // localStorage.setItem('rptName', 'SubLederTrail');
        // this.route.navigateByUrl('/app/main/reports/ReportView');

        //''

        this.reportView.show("SUBLEDGER", repParams);
    }

    getLedgerTypes() {
        this._LegderTypeComboboxService
            .getLedgerTypesForCombobox("")
            .subscribe(res => {
                debugger;
                this.ledgerTypes = res.items;
            });
    }
}
