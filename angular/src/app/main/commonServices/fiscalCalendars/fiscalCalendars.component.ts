import {
    Component,
    Injector,
    ViewEncapsulation,
    ViewChild
} from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import {
    FiscalCalendarsServiceProxy,
    FiscalCalendarDto
} from "@shared/service-proxies/service-proxies";
import { NotifyService } from "@abp/notify/notify.service";
import { AppComponentBase } from "@shared/common/app-component-base";
import { TokenAuthServiceProxy } from "@shared/service-proxies/service-proxies";
import { CreateOrEditFiscalCalendarModalComponent } from "./create-or-edit-fiscalCalendar-modal.component";
import { ViewFiscalCalendarModalComponent } from "./view-fiscalCalendar-modal.component";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { Table } from "primeng/components/table/table";
import { Paginator } from "primeng/components/paginator/paginator";
import { LazyLoadEvent } from "primeng/components/common/lazyloadevent";
import { FileDownloadService } from "@shared/utils/file-download.service";
import * as _ from "lodash";
import * as moment from "moment";

@Component({
    templateUrl: "./fiscalCalendars.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class FiscalCalendarsComponent extends AppComponentBase {
    @ViewChild("createOrEditFiscalCalendarModal", { static: true })
    createOrEditFiscalCalendarModal: CreateOrEditFiscalCalendarModalComponent;
    @ViewChild("viewFiscalCalendarModalComponent", { static: true })
    viewFiscalCalendarModal: ViewFiscalCalendarModalComponent;
    @ViewChild("dataTable", { static: true }) dataTable: Table;
    @ViewChild("paginator", { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = "";
    maxAUDTDATEFilter: moment.Moment;
    minAUDTDATEFilter: moment.Moment;
    audtuserFilter = "";
    maxPERIODSFilter: number;
    maxPERIODSFilterEmpty: number;
    minPERIODSFilter: number;
    minPERIODSFilterEmpty: number;
    maxQTR4PERDFilter: number;
    maxQTR4PERDFilterEmpty: number;
    minQTR4PERDFilter: number;
    minQTR4PERDFilterEmpty: number;
    maxACTIVEFilter: number;
    maxACTIVEFilterEmpty: number;
    minACTIVEFilter: number;
    minACTIVEFilterEmpty: number;
    maxBGNDATE1Filter: moment.Moment;
    minBGNDATE1Filter: moment.Moment;
    maxBGNDATE2Filter: moment.Moment;
    minBGNDATE2Filter: moment.Moment;
    maxBGNDATE3Filter: moment.Moment;
    minBGNDATE3Filter: moment.Moment;
    maxBGNDATE4Filter: moment.Moment;
    minBGNDATE4Filter: moment.Moment;
    maxBGNDATE5Filter: moment.Moment;
    minBGNDATE5Filter: moment.Moment;
    maxBGNDATE6Filter: moment.Moment;
    minBGNDATE6Filter: moment.Moment;
    maxBGNDATE7Filter: moment.Moment;
    minBGNDATE7Filter: moment.Moment;
    maxBGNDATE8Filter: moment.Moment;
    minBGNDATE8Filter: moment.Moment;
    maxBGNDATE9Filter: moment.Moment;
    minBGNDATE9Filter: moment.Moment;
    maxBGNDATE10Filter: moment.Moment;
    minBGNDATE10Filter: moment.Moment;
    maxBGNDATE11Filter: moment.Moment;
    minBGNDATE11Filter: moment.Moment;
    maxBGNDATE12Filter: moment.Moment;
    minBGNDATE12Filter: moment.Moment;
    maxBGNDATE13Filter: moment.Moment;
    minBGNDATE13Filter: moment.Moment;
    maxENDDATE1Filter: moment.Moment;
    minENDDATE1Filter: moment.Moment;
    maxENDDATE2Filter: moment.Moment;
    minENDDATE2Filter: moment.Moment;
    maxENDDATE3Filter: moment.Moment;
    minENDDATE3Filter: moment.Moment;
    maxENDDATE4Filter: moment.Moment;
    minENDDATE4Filter: moment.Moment;
    maxENDDATE5Filter: moment.Moment;
    minENDDATE5Filter: moment.Moment;
    maxENDDATE6Filter: moment.Moment;
    minENDDATE6Filter: moment.Moment;
    maxENDDATE7Filter: moment.Moment;
    minENDDATE7Filter: moment.Moment;
    maxENDDATE8Filter: moment.Moment;
    minENDDATE8Filter: moment.Moment;
    maxENDDATE9Filter: moment.Moment;
    minENDDATE9Filter: moment.Moment;
    maxENDDATE10Filter: moment.Moment;
    minENDDATE10Filter: moment.Moment;
    maxENDDATE11Filter: moment.Moment;
    minENDDATE11Filter: moment.Moment;
    maxENDDATE12Filter: moment.Moment;
    minENDDATE12Filter: moment.Moment;
    maxENDDATE13Filter: moment.Moment;
    minENDDATE13Filter: moment.Moment;

    constructor(
        injector: Injector,
        private _fiscalCalendarsServiceProxy: FiscalCalendarsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getFiscalCalendars(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._fiscalCalendarsServiceProxy
            .getAll(
                this.filterText,
                this.maxAUDTDATEFilter,
                this.minAUDTDATEFilter,
                this.audtuserFilter,
                this.maxPERIODSFilter == null
                    ? this.maxPERIODSFilterEmpty
                    : this.maxPERIODSFilter,
                this.minPERIODSFilter == null
                    ? this.minPERIODSFilterEmpty
                    : this.minPERIODSFilter,
                this.maxQTR4PERDFilter == null
                    ? this.maxQTR4PERDFilterEmpty
                    : this.maxQTR4PERDFilter,
                this.minQTR4PERDFilter == null
                    ? this.minQTR4PERDFilterEmpty
                    : this.minQTR4PERDFilter,
                this.maxACTIVEFilter == null
                    ? this.maxACTIVEFilterEmpty
                    : this.maxACTIVEFilter,
                this.minACTIVEFilter == null
                    ? this.minACTIVEFilterEmpty
                    : this.minACTIVEFilter,
                this.maxBGNDATE1Filter,
                this.minBGNDATE1Filter,
                this.maxBGNDATE2Filter,
                this.minBGNDATE2Filter,
                this.maxBGNDATE3Filter,
                this.minBGNDATE3Filter,
                this.maxBGNDATE4Filter,
                this.minBGNDATE4Filter,
                this.maxBGNDATE5Filter,
                this.minBGNDATE5Filter,
                this.maxBGNDATE6Filter,
                this.minBGNDATE6Filter,
                this.maxBGNDATE7Filter,
                this.minBGNDATE7Filter,
                this.maxBGNDATE8Filter,
                this.minBGNDATE8Filter,
                this.maxBGNDATE9Filter,
                this.minBGNDATE9Filter,
                this.maxBGNDATE10Filter,
                this.minBGNDATE10Filter,
                this.maxBGNDATE11Filter,
                this.minBGNDATE11Filter,
                this.maxBGNDATE12Filter,
                this.minBGNDATE12Filter,
                this.maxBGNDATE13Filter,
                this.minBGNDATE13Filter,
                this.maxENDDATE1Filter,
                this.minENDDATE1Filter,
                this.maxENDDATE2Filter,
                this.minENDDATE2Filter,
                this.maxENDDATE3Filter,
                this.minENDDATE3Filter,
                this.maxENDDATE4Filter,
                this.minENDDATE4Filter,
                this.maxENDDATE5Filter,
                this.minENDDATE5Filter,
                this.maxENDDATE6Filter,
                this.minENDDATE6Filter,
                this.maxENDDATE7Filter,
                this.minENDDATE7Filter,
                this.maxENDDATE8Filter,
                this.minENDDATE8Filter,
                this.maxENDDATE9Filter,
                this.minENDDATE9Filter,
                this.maxENDDATE10Filter,
                this.minENDDATE10Filter,
                this.maxENDDATE11Filter,
                this.minENDDATE11Filter,
                this.maxENDDATE12Filter,
                this.minENDDATE12Filter,
                this.maxENDDATE13Filter,
                this.minENDDATE13Filter,
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

    createFiscalCalendar(): void {
        this.createOrEditFiscalCalendarModal.show();
    }

    deleteFiscalCalendar(fiscalCalendar: FiscalCalendarDto): void {
        this.message.confirm("", isConfirmed => {
            if (isConfirmed) {
                this._fiscalCalendarsServiceProxy
                    .delete(fiscalCalendar.id)
                    .subscribe(() => {
                        this.reloadPage();
                        this.notify.success(this.l("SuccessfullyDeleted"));
                    });
            }
        });
    }

    exportToExcel(): void {
        this._fiscalCalendarsServiceProxy
            .getFiscalCalendarsToExcel(
                this.filterText,
                this.maxAUDTDATEFilter,
                this.minAUDTDATEFilter,
                this.audtuserFilter,
                this.maxPERIODSFilter == null
                    ? this.maxPERIODSFilterEmpty
                    : this.maxPERIODSFilter,
                this.minPERIODSFilter == null
                    ? this.minPERIODSFilterEmpty
                    : this.minPERIODSFilter,
                this.maxQTR4PERDFilter == null
                    ? this.maxQTR4PERDFilterEmpty
                    : this.maxQTR4PERDFilter,
                this.minQTR4PERDFilter == null
                    ? this.minQTR4PERDFilterEmpty
                    : this.minQTR4PERDFilter,
                this.maxACTIVEFilter == null
                    ? this.maxACTIVEFilterEmpty
                    : this.maxACTIVEFilter,
                this.minACTIVEFilter == null
                    ? this.minACTIVEFilterEmpty
                    : this.minACTIVEFilter,
                this.maxBGNDATE1Filter,
                this.minBGNDATE1Filter,
                this.maxBGNDATE2Filter,
                this.minBGNDATE2Filter,
                this.maxBGNDATE3Filter,
                this.minBGNDATE3Filter,
                this.maxBGNDATE4Filter,
                this.minBGNDATE4Filter,
                this.maxBGNDATE5Filter,
                this.minBGNDATE5Filter,
                this.maxBGNDATE6Filter,
                this.minBGNDATE6Filter,
                this.maxBGNDATE7Filter,
                this.minBGNDATE7Filter,
                this.maxBGNDATE8Filter,
                this.minBGNDATE8Filter,
                this.maxBGNDATE9Filter,
                this.minBGNDATE9Filter,
                this.maxBGNDATE10Filter,
                this.minBGNDATE10Filter,
                this.maxBGNDATE11Filter,
                this.minBGNDATE11Filter,
                this.maxBGNDATE12Filter,
                this.minBGNDATE12Filter,
                this.maxBGNDATE13Filter,
                this.minBGNDATE13Filter,
                this.maxENDDATE1Filter,
                this.minENDDATE1Filter,
                this.maxENDDATE2Filter,
                this.minENDDATE2Filter,
                this.maxENDDATE3Filter,
                this.minENDDATE3Filter,
                this.maxENDDATE4Filter,
                this.minENDDATE4Filter,
                this.maxENDDATE5Filter,
                this.minENDDATE5Filter,
                this.maxENDDATE6Filter,
                this.minENDDATE6Filter,
                this.maxENDDATE7Filter,
                this.minENDDATE7Filter,
                this.maxENDDATE8Filter,
                this.minENDDATE8Filter,
                this.maxENDDATE9Filter,
                this.minENDDATE9Filter,
                this.maxENDDATE10Filter,
                this.minENDDATE10Filter,
                this.maxENDDATE11Filter,
                this.minENDDATE11Filter,
                this.maxENDDATE12Filter,
                this.minENDDATE12Filter,
                this.maxENDDATE13Filter,
                this.minENDDATE13Filter
            )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
