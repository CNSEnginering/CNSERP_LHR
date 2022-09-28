import {
    Component,
    Injector,
    ViewEncapsulation,
    ViewChild,
    Input,
} from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { NotifyService } from "@abp/notify/notify.service";
import { AppComponentBase } from "@shared/common/app-component-base";
import { TokenAuthServiceProxy } from "@shared/service-proxies/service-proxies";
import { CreateOrEditEmployeeDeductionsModalComponent } from "./create-or-edit-employeeDeductions-modal.component";
import { ViewEmployeeDeductionsModalComponent } from "./view-employeeDeductions-modal.component";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { Table } from "primeng/components/table/table";
import { Paginator } from "primeng/components/paginator/paginator";
import { LazyLoadEvent } from "primeng/components/common/lazyloadevent";
import { FileDownloadService } from "@shared/utils/file-download.service";
import * as _ from "lodash";
import * as moment from "moment";
import { EmployeeDeductionsServiceProxy } from "../shared/services/employeeDeductions.service";
import { EmployeeDeductionsDto } from "../shared/dto/employeeDeductions-dto";
import { AdjustmentHServiceProxy } from "../shared/services/adjustmentH.service";
import { AdjHDto } from "../shared/dto/AdjH-dto";

@Component({
    selector: "employeeAdjustments",
    templateUrl: "./employeeDeductions.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class EmployeeDeductionsComponent extends AppComponentBase {
    @ViewChild("createOrEditEmployeeDeductionsModal", { static: true })
    createOrEditEmployeeDeductionsModal: CreateOrEditEmployeeDeductionsModalComponent;
    @ViewChild("createOrEditEmployeeDeductionsModal", { static: true })
    viewEmployeeDeductionsModal: CreateOrEditEmployeeDeductionsModalComponent;
    @ViewChild("dataTable", { static: true }) dataTable: Table;
    @ViewChild("paginator", { static: true }) paginator: Paginator;

    @Input() adjustmentType: string = "deduction";

    advancedFiltersAreShown = false;
    filterText = "";
    maxDocIDFilter: number;
    maxDocIDFilterEmpty: number;
    minDocIDFilter: number;
    minDocIDFilterEmpty: number;
    maxEmployeeIDFilter: number;
    maxEmployeeIDFilterEmpty: number;
    minEmployeeIDFilter: number;
    minEmployeeIDFilterEmpty: number;
    employeeNameFilter = "";
    maxSalaryYearFilter: number;
    maxSalaryYearFilterEmpty: number;
    minSalaryYearFilter: number;
    minSalaryYearFilterEmpty: number;
    maxSalaryMonthFilter: number;
    maxSalaryMonthFilterEmpty: number;
    minSalaryMonthFilter: number;
    minSalaryMonthFilterEmpty: number;
    maxDocDateFilter: moment.Moment;
    minDocDateFilter: moment.Moment;
    maxAmountFilter: number;
    maxAmountFilterEmpty: number;
    minAmountFilter: number;
    minAmountFilterEmpty: number;
    activeFilter = -1;
    audtUserFilter = "";
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    createdByFilter = "";
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;

    constructor(
        injector: Injector,
        private _employeeDeductionsServiceProxy: EmployeeDeductionsServiceProxy,
        private _adjustmentHServiceProxy: AdjustmentHServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getEmployeeAdjustment(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._adjustmentHServiceProxy
            .getAll(
                this.filterText,
                this.maxDocIDFilter == null
                    ? this.maxDocIDFilterEmpty
                    : this.maxDocIDFilter,
                this.minDocIDFilter == null
                    ? this.minDocIDFilterEmpty
                    : this.minDocIDFilter,
                this.maxEmployeeIDFilter == null
                    ? this.maxEmployeeIDFilterEmpty
                    : this.maxEmployeeIDFilter,
                this.minEmployeeIDFilter == null
                    ? this.minEmployeeIDFilterEmpty
                    : this.minEmployeeIDFilter,
                this.employeeNameFilter,
                this.maxSalaryYearFilter == null
                    ? this.maxSalaryYearFilterEmpty
                    : this.maxSalaryYearFilter,
                this.minSalaryYearFilter == null
                    ? this.minSalaryYearFilterEmpty
                    : this.minSalaryYearFilter,
                this.maxSalaryMonthFilter == null
                    ? this.maxSalaryMonthFilterEmpty
                    : this.maxSalaryMonthFilter,
                this.minSalaryMonthFilter == null
                    ? this.minSalaryMonthFilterEmpty
                    : this.minSalaryMonthFilter,
                this.maxDocDateFilter,
                this.minDocDateFilter,
                this.maxAmountFilter == null
                    ? this.maxAmountFilterEmpty
                    : this.maxAmountFilter,
                this.minAmountFilter == null
                    ? this.minAmountFilterEmpty
                    : this.minAmountFilter,
                this.activeFilter,
                this.audtUserFilter,
                this.maxAudtDateFilter,
                this.minAudtDateFilter,
                this.createdByFilter,
                this.maxCreateDateFilter,
                this.minCreateDateFilter,
                this.adjustmentType == "earnings" ? 1 : 2,
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

        // this._employeeDeductionsServiceProxy.getAll(
        //     this.filterText,
        //     this.maxDeductionIDFilter == null ? this.maxDeductionIDFilterEmpty : this.maxDeductionIDFilter,
        //     this.minDeductionIDFilter == null ? this.minDeductionIDFilterEmpty : this.minDeductionIDFilter,
        //     this.maxEmployeeIDFilter == null ? this.maxEmployeeIDFilterEmpty : this.maxEmployeeIDFilter,
        //     this.minEmployeeIDFilter == null ? this.minEmployeeIDFilterEmpty : this.minEmployeeIDFilter,
        //     this.employeeNameFilter,
        //     this.maxSalaryYearFilter == null ? this.maxSalaryYearFilterEmpty : this.maxSalaryYearFilter,
        //     this.minSalaryYearFilter == null ? this.minSalaryYearFilterEmpty : this.minSalaryYearFilter,
        //     this.maxSalaryMonthFilter == null ? this.maxSalaryMonthFilterEmpty : this.maxSalaryMonthFilter,
        //     this.minSalaryMonthFilter == null ? this.minSalaryMonthFilterEmpty : this.minSalaryMonthFilter,
        //     this.maxDocDateFilter,
        //     this.minDocDateFilter,
        //     this.maxAmountFilter == null ? this.maxAmountFilterEmpty : this.maxAmountFilter,
        //     this.minAmountFilter == null ? this.minAmountFilterEmpty : this.minAmountFilter,
        //     this.activeFilter,
        //     this.audtUserFilter,
        //     this.maxAudtDateFilter,
        //     this.minAudtDateFilter,
        //     this.createdByFilter,
        //     this.maxCreateDateFilter,
        //     this.minCreateDateFilter,
        //     this.primengTableHelper.getSorting(this.dataTable),
        //     this.primengTableHelper.getSkipCount(this.paginator, event),
        //     this.primengTableHelper.getMaxResultCount(this.paginator, event)
        // ).subscribe(result => {
        //     this.primengTableHelper.totalRecordsCount = result.totalCount;
        //     this.primengTableHelper.records = result.items;
        //     this.primengTableHelper.hideLoadingIndicator();
        // });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createEmployeeAdjustment(): void {
        this.createOrEditEmployeeDeductionsModal.show(
            null,
            this.adjustmentType
        );
    }

    deleteEmployeeAdjustment(adjh: AdjHDto): void {
        debugger

        this.message.confirm("", this.l("AreYouSure"), (isConfirmed) => {
            if (isConfirmed) {
                // this._employeeDeductionsServiceProxy.delete(employeeDeductions.id)
                this._adjustmentHServiceProxy.deleteAdjH(adjh).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l("SuccessfullyDeleted"));
                });
            }
        });
    }

    exportToExcel(): void {
        this._employeeDeductionsServiceProxy
            .getEmployeeDeductionsToExcel(
                this.filterText,
                this.maxDocIDFilter == null
                    ? this.maxDocIDFilterEmpty
                    : this.maxDocIDFilter,
                this.minDocIDFilter == null
                    ? this.minDocIDFilterEmpty
                    : this.minDocIDFilter,
                this.maxEmployeeIDFilter == null
                    ? this.maxEmployeeIDFilterEmpty
                    : this.maxEmployeeIDFilter,
                this.minEmployeeIDFilter == null
                    ? this.minEmployeeIDFilterEmpty
                    : this.minEmployeeIDFilter,
                this.employeeNameFilter,
                this.maxSalaryYearFilter == null
                    ? this.maxSalaryYearFilterEmpty
                    : this.maxSalaryYearFilter,
                this.minSalaryYearFilter == null
                    ? this.minSalaryYearFilterEmpty
                    : this.minSalaryYearFilter,
                this.maxSalaryMonthFilter == null
                    ? this.maxSalaryMonthFilterEmpty
                    : this.maxSalaryMonthFilter,
                this.minSalaryMonthFilter == null
                    ? this.minSalaryMonthFilterEmpty
                    : this.minSalaryMonthFilter,
                this.maxDocDateFilter,
                this.minDocDateFilter,
                this.maxAmountFilter == null
                    ? this.maxAmountFilterEmpty
                    : this.maxAmountFilter,
                this.minAmountFilter == null
                    ? this.minAmountFilterEmpty
                    : this.minAmountFilter,
                this.activeFilter,
                this.audtUserFilter,
                this.maxAudtDateFilter,
                this.minAudtDateFilter,
                this.createdByFilter,
                this.maxCreateDateFilter,
                this.minCreateDateFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
