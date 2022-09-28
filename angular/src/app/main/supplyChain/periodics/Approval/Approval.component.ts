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
import { appModuleAnimation } from "@shared/animations/routerTransition";
import * as moment from "moment";
import { VoucherPostingDto } from "../shared/dto/voucher-posting-dto";
import { VoucherPostingServiceProxy } from "../shared/services/voucher-posting-service";
import { ApprovalDto } from "../shared/dto/approval-dto";
import { ApprovalService } from "../shared/services/approval-service.";
import { LazyLoadEvent } from "primeng/api";
import { Table } from "primeng/table";
import { Paginator } from "primeng/primeng";

@Component({
    selector: "approval",
    templateUrl: "./Approval.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ApprovalComponent extends AppComponentBase implements OnInit {
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    posting: number[] = [];
    @ViewChild("dataTable", { static: true }) dataTable: Table;
    @ViewChild("paginator", { static: true }) paginator: Paginator;
    @ViewChildren("checkboxes") checkboxes: QueryList<ElementRef>;
    listText: string = "Un Check All";
    constructor(injector: Injector, private _approvalService: ApprovalService) {
        super(injector);
    }
    filterText = "";
    subCostCenter: any;
    sorting: any;
    skipCount: any;
    maxResultCount: any;
    mode: string;
    checkList: boolean = false;

    ngOnInit() {
        this.mode = "";
        this.approval.toDate = moment().endOf("day");
        this.approval.fromDate = moment()
            .subtract(1, "years")
            .endOf("day");
        //moment()
        //     .format("DD/MM/YYYY")
        //     .toString();
        this.approval.toDoc = 99999;
        this.approval.fromDoc = 0;
        // this.approval.fromDate = moment()
        //     .subtract(1, "years")
        //     .format("DD/MM/YYYY")
        //     .toString();
        this.approval.options = "opening";
    }

    approval: ApprovalDto = new ApprovalDto();
    checkAll: boolean = false;
    processing = false;
    saving = false;

    onCheckAllChange(event: any) {
        this.checkAll = !this.checkAll;

        //this.approval.opening = this.checkAll ? true : false;
    }

    save(): void {
        this.saving = true;
        this._approvalService
            .ApprovalData(
                this.approval.options,
                this.posting.slice(),
                this.mode,
                true
            )
            .subscribe(data => {
                this.posting.splice(0);
                this.saving = false;
                this.notify.info(this.l("SavedSuccessfully"));
                this.primengTableHelper.totalRecordsCount = 0;
                this.primengTableHelper.records.splice(0);
            });
    }

    getData(event?: LazyLoadEvent, mode?: string) {
        this.mode = mode == "" || mode == undefined ? this.mode : mode;
        this.skipCount = this.primengTableHelper.getSkipCount(
            this.paginator,
            event
        );
        this.maxResultCount = this.primengTableHelper.getMaxResultCount(
            this.paginator,
            event
        );

        this._approvalService
            .getData(
                this.approval.options,
                moment(this.approval.fromDate)
                    .format("MM/DD/YYYY")
                    .toString(),
                moment(this.approval.toDate)
                    .format("MM/DD/YYYY")
                    .toString(),
                this.mode,
                this.approval.fromDoc,
                this.approval.toDoc,
                this.skipCount,
                this.maxResultCount
            )
            .subscribe((data: any) => {
                this.primengTableHelper.showLoadingIndicator();
                this.primengTableHelper.totalRecordsCount =
                    data["result"]["totalCount"];
                this.primengTableHelper.records = data["result"]["items"];
                this.primengTableHelper.hideLoadingIndicator();
            });
        this.listText = "Check All";
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

    CheckAllList() {
        debugger;
        if (this.checkList == true) {
            this.listText = "Check All";
            this.checkboxes.forEach(element => {
                element.nativeElement.checked = false;
                this.posting.push(element.nativeElement.value);
                this.checkList = false;
            });
        } else {
            this.listText = "Un Check All";
            this.checkboxes.forEach(element => {
                element.nativeElement.checked = true;
                this.posting.push(element.nativeElement.value);
                this.checkList = true;
            });
        }
    }

    handleChange(evt: any) {
        this.primengTableHelper.totalRecordsCount = 0;
        this.primengTableHelper.records.splice(0);
    }
}
