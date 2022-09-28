import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    ViewEncapsulation,
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { Table } from "primeng/components/table/table";
import { Paginator } from "primeng/components/paginator/paginator";
import { LazyLoadEvent } from "primeng/components/common/lazyloadevent";
import { AppComponentBase } from "@shared/common/app-component-base";
import { CommonServiceFindersDto } from "../shared/dtos/commonServicesFinders-dto";
import { CommonServiceFindersService } from "../shared/services/commonServicesFinders.service";
import { BankReconcileServiceProxy } from "@app/main/finance/shared/services/bkReconcile.service";

@Component({
    selector: "commonServiceLookupTableModal",
    styleUrls: ["./commonService-lookup-table-modal.component.less"],
    encapsulation: ViewEncapsulation.None,
    templateUrl: "./commonService-lookup-table-modal.component.html",
})
export class CommonServiceLookupTableModalComponent extends AppComponentBase {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild("dataTable", { static: true }) dataTable: Table;
    @ViewChild("paginator", { static: true }) paginator: Paginator;

    filterText = "";
    id: string;
    detId: number;
    narration: string;
    displayName: string;
    target: string;
    paramFilter: string;
    currRate: number;
    accountId: string;
    taxRate: number;
    pickName: string;
    accountIDShow = false;
    currRateShow = false;
    taxRateShow = false;
    docTypeShow = false;
    detIdShow = false;
    narrationShow = false;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;
    docType: number;
    mode: string="";
    transType:number;
    constructor(
        injector: Injector,
        private _commonServiceFindersService: CommonServiceFindersService,
        private _bankReconcileServiceProxy: BankReconcileServiceProxy
    ) {
        super(injector);
    }

     show(target: string, parmValue?: string, title?: string): void {
        debugger;
        this.active = true;
        this.paginator.rows = 5;
        this.filterText = "";
        this.target = target;
        this.accountIDShow = false;
        this.currRateShow = false;
        switch (target) {
            case "Bank":
                this.accountIDShow = true;
                this.docTypeShow = true;
                this.paramFilter = parmValue;
                break;
            case "Cash":
                this.accountIDShow = true;
                this.docTypeShow = true;
                this.paramFilter = parmValue;
                break;
            case "Currency":
                this.currRateShow = true;
                break;
            case "TaxClass":
                this.paramFilter = parmValue;
                this.taxRateShow = true;
                this.accountIDShow = true;
                break;
            case "RecurringVouchers":
                this.narrationShow = true;
                break;
                case "RequsitionNo":
                   
                    this.paramFilter = parmValue;
                    this.accountIDShow =false;
                    this.narrationShow = false;
                    this.taxRateShow = false;
                    this.currRateShow = false;
                    this.docTypeShow = false;
                    break;
            default:
                break;
        }
        this.pickName = this.l(
            "Pick" +
                (title == "" || title == undefined || title == null
                    ? target
                    : title)
        );
        this.getAll();
        this.modal.show();
    }

    getAll(event?: LazyLoadEvent) {
        if (!this.active) {
            return;
        }

        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._commonServiceFindersService
            .getAllCommonServicesFindersForLookupTable(
                this.filterText,
                this.target,
                this.paramFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                debugger;
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    setAndSave(obj: CommonServiceFindersDto) {
        if (this.mode == "reconcilation") {
            this._bankReconcileServiceProxy
                .GetUnApprovedBankReconciles(obj.id)
                .subscribe((res) => {
                    //  console.log(res);
                    debugger;
                    if (res["result"] != null) {
                        this.message.warn(
                            "Document with DocID : " +
                                res["result"]["docID"] +
                                " and Doc No : " +
                                res["result"]["docNo"] +
                                " is pending for approval."
                        );

                        this.modalSave.emit(null);
                    } else {
                        this.id = obj.id;
                        this.displayName = obj.displayName;
                        this.docType = obj.docType;
                        this.currRate = obj.currRate;
                        this.accountId = obj.accountID;
                        this.taxRate = obj.taxRate;
                        this.detId = obj.detId;
                        this.narration = obj.narration;
                        this.active = false;
                        this.modal.hide();
                        this.modalSave.emit(null);
                    }
                });
        } else {
            this.id = obj.id;
            this.displayName = obj.displayName;
            this.docType = obj.docType;
            this.currRate = obj.currRate;
            this.accountId = obj.accountID;
            this.taxRate = obj.taxRate;
            this.detId = obj.detId;
            this.narration = obj.narration;
            this.active = false;
            this.modal.hide();
            this.modalSave.emit(null);
        }
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }
}
