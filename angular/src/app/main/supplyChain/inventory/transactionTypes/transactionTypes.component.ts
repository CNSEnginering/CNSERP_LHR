import {
    Component,
    Injector,
    ViewEncapsulation,
    ViewChild
} from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { NotifyService } from "@abp/notify/notify.service";
import { AppComponentBase } from "@shared/common/app-component-base";
import { TokenAuthServiceProxy } from "@shared/service-proxies/service-proxies";
import { CreateOrEditTransactionTypeModalComponent } from "./create-or-edit-transactionType-modal.component";
import { ViewTransactionTypeModalComponent } from "./view-transactionType-modal.component";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { Table } from "primeng/components/table/table";
import { Paginator } from "primeng/components/paginator/paginator";
import { LazyLoadEvent } from "primeng/components/common/lazyloadevent";
import { FileDownloadService } from "@shared/utils/file-download.service";
import * as _ from "lodash";
import * as moment from "moment";
import { TransactionTypesService } from "../shared/services/transaction-types.service";
import { TransactionTypeDto } from "../shared/dto/transaction-types-dto";

@Component({
    templateUrl: "./transactionTypes.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TransactionTypesComponent extends AppComponentBase {
    @ViewChild("createOrEditTransactionTypeModal", { static: true })
    createOrEditTransactionTypeModal: CreateOrEditTransactionTypeModalComponent;
    @ViewChild("viewTransactionTypeModalComponent", { static: true })
    viewTransactionTypeModal: ViewTransactionTypeModalComponent;
    @ViewChild("dataTable", { static: true }) dataTable: Table;
    @ViewChild("paginator", { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = "";
    descriptionFilter = "";
    activeFilter = -1;
    createdByFilter = "";
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;
    audtUserFilter = "";
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    typeIdFilter = "";

    constructor(
        injector: Injector,
        private _transactionTypesService: TransactionTypesService,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getTransactionTypes(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._transactionTypesService
            .getAll(
                this.filterText,
                this.typeIdFilter,
                this.descriptionFilter,
                this.activeFilter,
                this.createdByFilter,
                this.maxCreateDateFilter,
                this.minCreateDateFilter,
                this.audtUserFilter,
                this.maxAudtDateFilter,
                this.minAudtDateFilter,
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

    createTransactionType(): void {
        this.createOrEditTransactionTypeModal.show();
    }

    deleteTransactionType(transactionTypeId?: number): void {
        this.message.confirm("", this.l("AreYouSure"), isConfirmed => {
            if (isConfirmed) {
                this._transactionTypesService
                    .delete(transactionTypeId)
                    .subscribe(() => {
                        this.reloadPage();
                        this.notify.success(this.l("SuccessfullyDeleted"));
                    });
            }
        });
    }

    exportToExcel(): void {
        this._transactionTypesService
            .getTransactionTypesToExcel(
                this.filterText,
                this.typeIdFilter,
                this.descriptionFilter,
                this.activeFilter,
                this.createdByFilter,
                this.maxCreateDateFilter,
                this.minCreateDateFilter,
                this.audtUserFilter,
                this.maxAudtDateFilter,
                this.minAudtDateFilter
            )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
