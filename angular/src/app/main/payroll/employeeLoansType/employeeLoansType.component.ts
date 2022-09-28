import { Component, OnInit, ViewEncapsulation, ViewChild, Injector } from "@angular/core";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { Table } from "primeng/table";
import { Paginator, LazyLoadEvent } from "primeng/primeng";
import * as moment from "moment";
import { LoansTypeService } from "../shared/services/loansType.service";
import { NotifyService } from "abp-ng2-module/dist/src/notify/notify.service";
import { TokenAuthServiceProxy } from "@shared/service-proxies/service-proxies";
import { ActivatedRoute } from "@angular/router";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { AppComponentBase } from "@shared/common/app-component-base";
import { loansTypeDto } from "../shared/dto/loanTypes-dto";
import { CreateOrEditEmployeeLoansTypeModalComponent } from "./create-or-edit-employee-loans-type-modal.component";

@Component({
    templateUrl: "./employeeLoansType.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class EmployeeLoansTypeComponent extends AppComponentBase {
  @ViewChild('createOrEditEmployeeLoansTypeModal', { static: true }) createOrEditEmployeeLoansTypeModal: CreateOrEditEmployeeLoansTypeModalComponent;
    @ViewChild("dataTable", { static: true }) dataTable: Table;
    @ViewChild("paginator", { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = "";
    maxDeptIDFilter: number;
    maxDeptIDFilterEmpty: number;
    minDeptIDFilter: number;
    minDeptIDFilterEmpty: number;
    deptNameFilter = "";
    maxActiveFilter: number;
    maxActiveFilterEmpty: number;
    minActiveFilter: number;
    minActiveFilterEmpty: number;
    audtUserFilter = "";
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    createdByFilter = "";
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;

    constructor( injector: Injector,
      private _loansTypeService: LoansTypeService,
      private _notifyService: NotifyService,
      private _tokenAuth: TokenAuthServiceProxy,
      private _activatedRoute: ActivatedRoute,
      private _fileDownloadService: FileDownloadService) {
        super(injector);
      }

      getLoansType(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._loansTypeService.getAll(
            this.filterText,
            this.maxDeptIDFilter == null ? this.maxDeptIDFilterEmpty : this.maxDeptIDFilter,
            this.minDeptIDFilter == null ? this.minDeptIDFilterEmpty : this.minDeptIDFilter,
            this.deptNameFilter,
            this.maxActiveFilter == null ? this.maxActiveFilterEmpty : this.maxActiveFilter,
            this.minActiveFilter == null ? this.minActiveFilterEmpty : this.minActiveFilter,
            this.audtUserFilter,
            this.maxAudtDateFilter,
            this.minAudtDateFilter,
            this.createdByFilter,
            this.maxCreateDateFilter,
            this.minCreateDateFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            console.log(result);
            debugger;
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createLoansType(id?:number): void {
        this.createOrEditEmployeeLoansTypeModal.show(id);
    }

    deleteLoanType(loansType:loansTypeDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._loansTypeService.delete(loansType.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

}
