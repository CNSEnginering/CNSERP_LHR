import { FinanceFindersDto } from "../shared/dtos/financeFinders-dto";
import { FinanceFindersService } from "../shared/services/financeFinders.service";
import * as moment from 'moment';
import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    ViewEncapsulation
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { AppComponentBase } from "@shared/common/app-component-base";
import { Table } from "primeng/components/table/table";
import { Paginator } from "primeng/components/paginator/paginator";
import { LazyLoadEvent } from "primeng/components/common/lazyloadevent";

@Component({
    selector: "financeLookupTableModal",
    styleUrls: ["./finance-lookup-table-modal.component.less"],
    encapsulation: ViewEncapsulation.None,
    templateUrl: "./finance-lookup-table-modal.component.html"
})
export class FinanceLookupTableModalComponent extends AppComponentBase {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild("dataTable", { static: true }) dataTable: Table;
    @ViewChild("paginator", { static: true }) paginator: Paginator;

    filterText = "";
    id: string;
    displayName: string;
    taxRate:number;
    itemPriceID: string;
    driverCtrlAcc : string;
    driverSubAcc : string;
    

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;

    target: string;
    pickName: string;
    accountIDShow = false;
    termRateShow = false;
    docDate: Date;
    docNo: number;
    docMonth: number;
    configID: number;
    fmtDocNo : number;
    bookId : string;

    private paramFilter: string;
    private param2Filter: string;
    private param3Filter: number;
    subledger: boolean;
    accountID: string;
    locName: string;
    

    sltype: number;
    displayNameShow: boolean;
    narrationShow: boolean;
    docNoShow: boolean;
    docMonthShow: boolean;
    bookIdShow: boolean;
    voucherNoShow: boolean;
    idShow: boolean;
    voucherDateShow: boolean;
    amount: number;
    amountShow: boolean;
    partyInvNoShow: boolean;
    driverAccShow: boolean;
    driverSubAccShow:boolean;
    constructor(
        injector: Injector,
        private _financeFindersService: FinanceFindersService
    ) {
        super(injector);
    }

    show(target: string, parmValue?: string, parm2Value?: string, title?: string,parm3Value?:number): void {
       debugger
        this.active = true;
        this.paginator.rows = 5;
        this.filterText = "";
        this.target = target;
        this.idShow = true;
        this.bookId=""
        this.bookIdShow=false;
        this.docNoShow=false;
        this.displayNameShow = true;
        this.termRateShow = false;
        this.accountIDShow = false;
        this.docMonthShow = false;
        this.narrationShow = false;
        this.voucherNoShow = false;
        this.voucherDateShow = false;
        this.partyInvNoShow = false;
        this.amountShow=false;
        this.driverAccShow=false;
        this.driverSubAccShow=false;
        
        switch (target) {
            case "GLConfig":
                this.accountIDShow = true;
                this.paramFilter = parmValue;
                break;
            case "ArTerm":
                 this.accountIDShow = true;
                 this.termRateShow = true;
                 break;
            case "WHTerm":
                this.termRateShow = true;
                break;
            case "SubLedger":
                this.paramFilter = parmValue;
                this.param2Filter = parm2Value;
                break;
            case "Customer":
                this.paramFilter = parmValue;
                break;
            case "Level2":
                this.paramFilter = parmValue;
                break;
            case "Level3":
                this.paramFilter = parmValue;
                break;
            case "ChartOfAccount":
                this.paramFilter = parmValue;
                this.param2Filter = parm2Value;
                break;
            case "GLBooks":
                this.paramFilter = parmValue;
                break;
            case "ReconcilationDocument":
                this.displayNameShow = false;
                this.paramFilter = parmValue;
                break;
            case "ChequeBookDetail":
                this.displayNameShow = true;
                this.paramFilter = parmValue;
                break;
            case "VoucherNo":
                this.idShow = false;
                this.displayNameShow = false;
                this.voucherNoShow = true;
                this.voucherDateShow=true;
                this.narrationShow = true;
                this.paramFilter = parmValue;
                break;
            case "Voucher":
                    this.idShow = false;
                    this.displayNameShow = false;
                    this.bookIdShow = true;
                    this.narrationShow = true;
                    this.docMonthShow   =true;
                    this.voucherDateShow=true;
                    this.docNoShow=true;
                    this.paramFilter = parmValue;
                    break;
            case "InvoiceNoPV":
                debugger
                this.displayNameShow=false;
                this.narrationShow = true;
                this.param2Filter = parm2Value;
                this.param3Filter = parm3Value;
                break;        
            case "InvoiceNo":
                this.idShow = false;
                this.displayNameShow = false;
                this.voucherNoShow = true;
                this.voucherDateShow=true;
                this.partyInvNoShow=true;
                this.amountShow=true;
                this.paramFilter = parmValue;
                this.param2Filter = parm2Value;
                this.param3Filter = parm3Value;
                break;
            case "Cader":
                this.paramFilter = parmValue;
                this.param2Filter = parm2Value;
                break;
            case "SLGrp":
                this.paramFilter = parmValue;
                this.param2Filter = parm2Value;
                break;
                default:
                break;
            case "Debtors":
                this.paramFilter = parmValue;
                break;
            case "CustomerByDebtor":
                this.paramFilter = parmValue;
                break;
            case "Routes":
                this.paramFilter = parmValue;
                break;
            case "Drivers":
                this.paramFilter = parmValue;
                this.driverAccShow=true;
                this.driverSubAccShow=true;
                break;    
        }
        this.pickName = this.l("Pick" + ((title == "" ||
            title == undefined || title == null)
            ? target : title));

        this.getAll();
        this.modal.show();
    }

    getAll(event?: LazyLoadEvent) {
        debugger;
        if (!this.active) {
            return;
        }

        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._financeFindersService
            .getAllFinanceFindersForLookupTable(
                this.filterText,
                this.target,
                this.paramFilter,
                this.param2Filter,
                this.param3Filter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe(result => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    setAndSave(obj: FinanceFindersDto) {
        this.id = obj.id;
        this.displayName = obj.displayName;
        this.subledger = obj.subledger;
        this.accountID = obj.accountID;
        this.sltype = obj.slType;
        this.docDate = moment(obj.docDate).toDate();
        this.docMonth = obj.docMonth;
        this.configID = obj.configID;
        this.taxRate = obj.termRate;
        this.amount = obj.amount;
        this.accountID = obj.accountID;
        this.bookId = obj.bookId;
        this.docNo = obj.docNo;
        this.fmtDocNo = obj.fmtDocNo;
        this.itemPriceID = obj.itemPriceID;
        this.locName = obj.locName;
        this.driverCtrlAcc=obj.driverCtrlAcc;
        this.driverSubAcc=obj.driverSubAcc;
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }
}
