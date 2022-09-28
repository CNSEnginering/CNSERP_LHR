import { AppComponentBase } from "@shared/common/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import * as _ from "lodash";
import * as moment from "moment";
import { FiscalDateService } from "@app/shared/services/fiscalDate.service";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";
import { Component, ViewEncapsulation, Injector, ViewChild, Output, EventEmitter } from "@angular/core";
import { FinanceLookupTableModalComponent } from "@app/finders/finance/finance-lookup-table-modal.component";
import { TreeModule } from "primeng/tree";

@Component({
    templateUrl: "./bankReconcile-rpt.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class BankReconcileReportComponent extends AppComponentBase {

    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild("reportView", { static: true }) reportView: ReportviewrModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    bankID: any;
    bankName: any;

    fromDocID: any = "";
    toDocID: any = "";
    fromDate: any;
    toDate: any;
    target: string;
    rptParams: string = "";
    bankRecon: string;
    showBankID: boolean = true;
    showFromDocID: boolean = true;
    showToDocID: boolean = true;
    showFromDate: boolean = true;
    showToDate: boolean = true;

    constructor(
        injector: Injector,
        private _reportService: FiscalDateService
    ) {
        super(injector);
    }



    ngOnInit() {
        this._reportService.getDate().subscribe(data => {
            this.fromDate = new Date(data["result"]);
        });

        this.toDate = new Date();
        this.bankRecon = "reconcileListing";
        this.hideFields();
    }


    hideFields() {
        switch (this.bankRecon) {
            case "reconcileListing":
                this.showBankID = true;
                this.showFromDocID = true;
                this.showToDocID = true;
                this.showFromDate = false;
                this.showToDate = false;
                break;

            case "reconcilation":
                this.showBankID = true;
                this.showFromDocID = false;
                this.showToDocID = false;
                this.showFromDate = true;
                this.showToDate = true;
                break;
        }
    }

    getReport() {
        switch (this.bankRecon) {
            case "reconcileListing":
                this.processReport("BankReconcileReport");
                break;
            case "reconcilation":
                this.processReport("BankReconcilationReport");
                break;
        }
    }

    processReport(report: string) {
        debugger;
        this.rptParams = "";
        switch (report) {
            case "BankReconcileReport":
                if (this.bankID !== undefined)
                    this.rptParams += encodeURIComponent("" + this.bankID) + "$";
                if (this.fromDocID !== undefined)
                    this.rptParams += encodeURIComponent("" + this.fromDocID.split("-")[1]) + "$";
                if (this.toDocID !== undefined)
                    this.rptParams += encodeURIComponent("" + this.toDocID.split("-")[1]) + "$";
                this.rptParams = this.rptParams.replace(/[?$]&/, "");
                break;
            case "BankReconcilationReport":
                if (this.bankID !== undefined)
                    this.rptParams += encodeURIComponent("" + this.bankID) + "$";
                if (this.fromDate !== undefined)
                    this.rptParams += "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
                if (this.toDate !== undefined)
                    this.rptParams += "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
                if (this.bankName !== undefined)
                    this.rptParams += encodeURIComponent("" + this.bankName) + "$";
                this.rptParams = this.rptParams.replace(/[?$]&/, "");
                break;
        }
        this.reportView.show(report, this.rptParams);
    }



    getNewFinanceModal() {
        debugger;
        switch (this.target) {
            case "fromDoc":
                this.getNewDocID();
                break;
            case "toDoc":
                this.getNewDocID();
                break;
            case "bankID":
                this.getNewBankID();
                break;
            default:
                break;
        }
    }
    /////////////////////////////////////DOC ID///////////////////////////////////////////
    openDocIDModal(type) {
        debugger;
        this.target = type;
        if (this.target == "fromDoc") {
            this.FinanceLookupTableModal.id = this.fromDocID;
            this.FinanceLookupTableModal.id = this.toDocID;
            this.FinanceLookupTableModal.show("ReconcilationDocument", this.bankID);
        }
        else if (this.target == "toDoc") {
            this.FinanceLookupTableModal.id = this.toDocID;
            this.FinanceLookupTableModal.show("ReconcilationDocument", this.bankID);
        }
    }

    setDocIDNull(type) {
        if (type == "fromDoc") {
            this.fromDocID = null;
        }
        else if (type == "toDoc") {
            this.toDocID = null;
        }
    }

    getNewDocID() {
        debugger;
        if (this.target == "fromDoc") {
            this.fromDocID = this.FinanceLookupTableModal.id;
            this.toDocID = this.FinanceLookupTableModal.id;
        }
        else if (this.target == "toDoc") {
            this.toDocID = this.FinanceLookupTableModal.id;
        }
    }
    /////////////////////////////////////DOC ID///////////////////////////////////////////
    ////////////////////////////////////Bank ID///////////////////////////////////////////
    openBankIDModal() {
        debugger;
        this.target = "bankID";
        this.FinanceLookupTableModal.id = this.bankID;
        this.FinanceLookupTableModal.displayName = this.bankName;
        this.FinanceLookupTableModal.show("BankReconcileBankID");
    }

    setBankIDNull() {

        this.bankID = null;
        this.bankName = null;
    }

    getNewBankID() {

        debugger;
        this.bankID = this.FinanceLookupTableModal.id;
        this.bankName = this.FinanceLookupTableModal.displayName;

    }


}
