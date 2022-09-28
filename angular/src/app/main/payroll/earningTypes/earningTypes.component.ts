import {
    Component,
    Injector,
    ViewEncapsulation,
    ViewChild,
} from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { NotifyService } from "@abp/notify/notify.service";
import { AppComponentBase } from "@shared/common/app-component-base";
import { TokenAuthServiceProxy } from "@shared/service-proxies/service-proxies";
import { CreateOrEditEarningTypesModalComponent } from "./create-or-edit-earningTypes-modal.component";

import { ViewEarningTypesModalComponent } from "./view-earningTypes-modal.component";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { Table } from "primeng/components/table/table";
import { Paginator } from "primeng/components/paginator/paginator";
import { LazyLoadEvent } from "primeng/components/common/lazyloadevent";
import { FileDownloadService } from "@shared/utils/file-download.service";
import * as _ from "lodash";
import * as moment from "moment";
import { EarningTypesServiceProxy } from "../shared/services/earningTypes.service";
import { EarningTypesDto } from "../shared/dto/earningTypes-dto";

@Component({
    templateUrl: "./earningTypes.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class EarningTypesComponent extends AppComponentBase {
    @ViewChild("createOrEditEarningTypesModal", { static: true })
    createOrEditEarningTypesModal: CreateOrEditEarningTypesModalComponent;
    @ViewChild("viewEarningTypesModalComponent", { static: true })
    viewEarningTypesModal: ViewEarningTypesModalComponent;

    @ViewChild("dataTable", { static: true }) dataTable: Table;
    @ViewChild("paginator", { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = "";
    maxTypeIDFilter: number;
    maxTypeIDFilterEmpty: number;
    minTypeIDFilter: number;
    minTypeIDFilterEmpty: number;
    typeDescFilter = "";
    activeFilter = -1;
    audtUserFilter = "";
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    createdByFilter = "";
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;

    totalRecords: number;

    constructor(
        injector: Injector,
        private _earningTypesServiceProxy: EarningTypesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getEarningTypes(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._earningTypesServiceProxy
            .getAll(
                this.filterText,
                this.maxTypeIDFilter == null
                    ? this.maxTypeIDFilterEmpty
                    : this.maxTypeIDFilter,
                this.minTypeIDFilter == null
                    ? this.minTypeIDFilterEmpty
                    : this.minTypeIDFilter,
                this.typeDescFilter,
                this.activeFilter,
                this.audtUserFilter,
                this.maxAudtDateFilter,
                this.minAudtDateFilter,
                this.createdByFilter,
                this.maxCreateDateFilter,
                this.minCreateDateFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
                this.totalRecords= this.primengTableHelper.totalRecordsCount
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createEarningTypes(): void {
        this.createOrEditEarningTypesModal.show();
    }

    deleteEarningTypes(earningTypes: EarningTypesDto): void {
        this.message.confirm("", this.l("AreYouSure"), (isConfirmed) => {
            if (isConfirmed) {
                this._earningTypesServiceProxy
                    .delete(earningTypes.id)
                    .subscribe(() => {
                        this.reloadPage();
                        this.notify.success(this.l("SuccessfullyDeleted"));
                    });
            }
        });
    }

    exportToExcel(): void {
        this._earningTypesServiceProxy
            .getEarningTypesToExcel(
                this.filterText,
                this.maxTypeIDFilter == null
                    ? this.maxTypeIDFilterEmpty
                    : this.maxTypeIDFilter,
                this.minTypeIDFilter == null
                    ? this.minTypeIDFilterEmpty
                    : this.minTypeIDFilter,
                this.typeDescFilter,
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
