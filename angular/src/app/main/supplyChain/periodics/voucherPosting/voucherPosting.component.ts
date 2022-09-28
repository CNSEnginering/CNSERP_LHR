import {
    Component,
    OnInit,
    Output,
    Injector,
    ViewEncapsulation,
    EventEmitter,
    ViewChild,
    ViewChildren,
    QueryList,
    ElementRef
} from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { NotifyService } from "abp-ng2-module/dist/src/notify/notify.service";
import { TokenAuthServiceProxy } from "@shared/service-proxies/service-proxies";
import { ActivatedRoute, Router } from "@angular/router";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import * as moment from "moment";
import { VoucherPostingDto } from "../shared/dto/voucher-posting-dto";
import { VoucherPostingServiceProxy } from "../shared/services/voucher-posting-service";
import { LazyLoadEvent } from "primeng/api";
import { Table } from "primeng/table";
import { Paginator } from "primeng/primeng";

@Component({
    selector: "app-voucher-posting",
    templateUrl: "./voucherPosting.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class VoucherPostingComponent extends AppComponentBase
    implements OnInit {
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    posting: number[] = [];
    constructor(
        injector: Injector,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _voucherPostingServiceProxy: VoucherPostingServiceProxy
    ) {
        super(injector);
    }
    toDate: moment.Moment | undefined;
    fromDate: moment.Moment | undefined;
    lastPostedDate: moment.Moment | undefined;
    currentPostingDate: moment.Moment | undefined;
    selectedType: string;
    listText: string = "Un Check All";
    ngOnInit() {
        debugger;
        this.toDate = moment().endOf("day");
        this.voucherPosting.toDoc = 99999;
        this.voucherPosting.fromDoc = 0;
        this.fromDate = moment()
            .subtract(1, "years")
            .endOf("day");
        this.currentPostingDate = moment().endOf("day");
        // this.lastPostedDate = moment("DD/MM/YYYY");
        this.options = "receipt";
        this.selectedType = "Receipt";
        this.getLastPostedDate();
        this.posting.length = 0;
    }

    voucherPosting: VoucherPostingDto = new VoucherPostingDto();
    @ViewChild("dataTable", { static: true }) dataTable: Table;
    @ViewChild("paginator", { static: true }) paginator: Paginator;
    @ViewChildren("checkboxes") checkboxes: QueryList<ElementRef>;
    checkAll: boolean = false;
    processing = false;
    saving = false;
    options: string | undefined;
    sorting: any;
    skipCount: any;
    maxResultCount: any;
    mode: string;
    checkList: boolean = false;

    onCheckAllChange(event: any) {
        this.checkAll = !this.checkAll;

        this.voucherPosting.receipt = this.checkAll ? true : false;
        this.voucherPosting.sales = this.checkAll ? true : false;
        this.voucherPosting.receiptReturn = this.checkAll ? true : false;
        this.voucherPosting.salesReturn = this.checkAll ? true : false;
        this.voucherPosting.transfer = this.checkAll ? true : false;
        this.voucherPosting.consumption = this.checkAll ? true : false;
        this.voucherPosting.bankTransfer = this.checkAll ? true : false;
        this.voucherPosting.assemblies = this.checkAll ? true : false;
        this.voucherPosting.creditNote = this.checkAll ? true : false;
        this.voucherPosting.debitNote = this.checkAll ? true : false;
    }

    save(): void {
        this.message.confirm("Process Voucher Posting", isConfirmed => {
            if (isConfirmed) {
                if (this.checkDate(this.currentPostingDate)) {
                    return;
                }
                this.saving = true;
                debugger;
                var curDate = moment(this.currentPostingDate).format(
                    "DD/MM/YYYY"
                );
                this.processing = true;
                this.voucherPosting.toDate = moment(this.toDate);
                this.voucherPosting.fromDate = moment(this.fromDate);
                this._voucherPostingServiceProxy
                    .processData(this.options, curDate, this.posting.slice())
                    .subscribe(result => {
                        debugger;
                        if (result["statusText"] == "OK") {
                            this.posting.splice(0);
                            this.saving = false;
                            this.processing = false;
                            this.notify.info(this.l("ProcessSuccessfully"));
                            this.primengTableHelper.totalRecordsCount = 0;
                            this.primengTableHelper.records.splice(0);
                            this.modalSave.emit(null);
                            this.getLastPostedDate();
                        } else {
                            this.posting.splice(0);
                            this.saving = false;
                            this.processing = false;
                            this.notify.error(this.l("ProcessFailed"));
                        }
                    });
            }
        });
    }

    checkCheckBoxes(): boolean {
        debugger;
        if (
            this.voucherPosting.receipt ||
            this.voucherPosting.sales ||
            this.voucherPosting.receiptReturn ||
            this.voucherPosting.salesReturn ||
            this.voucherPosting.transfer ||
            this.voucherPosting.consumption ||
            this.voucherPosting.assemblies ||
            this.voucherPosting.bankTransfer ||
            this.voucherPosting.creditNote ||
            this.voucherPosting.debitNote
        ) {
            return false;
        }
        return true;
    }

    getLastPostedDate() {
        this._voucherPostingServiceProxy
            .getLastPostedDate(this.selectedType)
            .subscribe(data => {
                this.lastPostedDate = moment()
                    .subtract(1, "years")
                    .endOf("day");
                if (data["result"] !== "")
                    this.lastPostedDate = moment(data["result"]).endOf("day");
            });
    }

    checkDate(date: any) {
        debugger;
        var curDate = moment(date).format("YYYY-MM-DD");
        var lastPosDate = moment(this.lastPostedDate).format("YYYY-MM-DD");
        if (curDate != lastPosDate) {
            if (curDate < lastPosDate) {
                this.message.warn(
                    "Current posting date must be greater than last posting date",
                    "Last Posting Date Greater"
                );
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    handleChange(data: any) {
        this.primengTableHelper.records = data["result"];
        this.voucherPosting.creditNote = false;
        this.voucherPosting.debitNote = false;
        this.voucherPosting.assemblies = false;
        this.voucherPosting.bankTransfer = false;
        this.voucherPosting.consumption = false;
        this.voucherPosting.receipt = false;
        this.voucherPosting.sales = false;
        this.voucherPosting.receiptReturn = false;
        this.voucherPosting.salesReturn = false;
        this.voucherPosting.transfer = false;
        switch (this.options) {
            case "creditNote":
                this.voucherPosting.creditNote = true;
                this.selectedType = "CreditNote";
                break;
            case "debitNote":
                this.voucherPosting.debitNote = true;
                this.selectedType = "DebitNote";
                break;
            case "assemblies":
                this.voucherPosting.assemblies = true;
                break;
            case "bankTransfer":
                this.voucherPosting.bankTransfer = true;
                this.selectedType = "BankTransfer";
                break;
            case "consumption":
                this.voucherPosting.consumption = true;
                this.selectedType = "Consumption";
                break;
            case "receipt":
                this.voucherPosting.receipt = true;
                this.selectedType = "Receipt";
                break;
            case "sales":
                this.voucherPosting.sales = true;
                this.selectedType = "Sales";
                break;
            case "receiptReturn":
                this.voucherPosting.receiptReturn = true;
                this.selectedType = "ReceiptReturn";
                break;
            case "salesReturn":
                this.voucherPosting.salesReturn = true;
                this.selectedType = "SalesReturn";
                break;
            case "transfer":
                this.voucherPosting.transfer = true;
                this.selectedType = "Transfer";
                break;
        }
        this.getLastPostedDate();
        // this.primengTableHelper.totalRecordsCount = 0;
        // this.primengTableHelper.records.splice(0);
    }

    getData(event?: LazyLoadEvent) {
        debugger;
        this.skipCount = this.primengTableHelper.getSkipCount(
            this.paginator,
            event
        );
        this.maxResultCount = this.primengTableHelper.getMaxResultCount(
            this.paginator,
            event
        );
        this._voucherPostingServiceProxy
            .getVouchersData(
                this.voucherPosting.fromDoc,
                this.voucherPosting.toDoc,
                this.options,
                moment(this.fromDate)
                    .format("MM/DD/YYYY")
                    .toString(),
                moment(this.toDate)
                    .format("MM/DD/YYYY")
                    .toString()
            )
            .subscribe((data: any) => {
                debugger;
                this.primengTableHelper.records = data["result"];
                if (data.result) {
                    this.primengTableHelper.showLoadingIndicator();
                    this.primengTableHelper.totalRecordsCount =
                        data["result"].length;
                    this.primengTableHelper.records = data["result"];
                    this.primengTableHelper.hideLoadingIndicator();
                }
            });
        this.listText = "Check All";
    }

    CheckAllList() {
        if (this.checkList == true) {
            this.listText = "Check All";
            this.checkboxes.forEach(element => {
                element.nativeElement.checked = false;
                this.posting.forEach((el, index) => {
                    if (el == parseInt(element.nativeElement.value)) {
                        this.posting.splice(index, 1);
                    }
                });
            });
            this.checkList = false;
        } else {
            this.listText = "Un Check All";
            var check = false;
            this.checkboxes.forEach(element => {
                element.nativeElement.checked = true;
                //this.posting.push(element.nativeElement.value);

                this.posting.forEach((el, index) => {
                    if (el == parseInt(element.nativeElement.value)) {
                        check = true;
                    }
                });
                if (check == false) {
                    this.posting.push(element.nativeElement.value);
                }
            });
            this.checkList = true;
        }
    }

    getDataForApproval(event: HTMLInputElement) {
        if (event.checked == true) {
            this.posting.push(parseInt(event.value));
        } else {
            this.posting.forEach((el, index) => {
                if (el == parseInt(event.value)) {
                    this.posting.splice(index, 1);
                }
            });
        }
    }
}
