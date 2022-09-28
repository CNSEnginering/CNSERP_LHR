import {
    Component,
    Injector,
    ViewEncapsulation,
    ViewChild,
} from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import {
    ChartofControlsServiceProxy,
    ChartofControlDto,
} from "@shared/service-proxies/service-proxies";
import { NotifyService } from "@abp/notify/notify.service";
import { AppComponentBase } from "@shared/common/app-component-base";
import { TokenAuthServiceProxy } from "@shared/service-proxies/service-proxies";
import { CreateOrEditChartofControlModalComponent } from "./create-or-edit-chartofControl-modal.component";
import { ViewChartofControlModalComponent } from "./view-chartofControl-modal.component";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { Table } from "primeng/components/table/table";
import { Paginator } from "primeng/components/paginator/paginator";
import { LazyLoadEvent } from "primeng/components/common/lazyloadevent";
import { FileDownloadService } from "@shared/utils/file-download.service";
import * as _ from "lodash";
import * as moment from "moment";
import { FileUpload } from "primeng/primeng";
import { AppConsts } from "@shared/AppConsts";
import { HttpClient } from "@angular/common/http";
import { finalize } from "rxjs/operators";

@Component({
    templateUrl: "./chartofControls.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ChartofControlsComponent extends AppComponentBase {
    @ViewChild("createOrEditChartofControlModal", { static: true })
    createOrEditChartofControlModal: CreateOrEditChartofControlModalComponent;
    @ViewChild("viewChartofControlModalComponent", { static: true })
    viewChartofControlModal: ViewChartofControlModalComponent;
    @ViewChild("dataTable", { static: true }) dataTable: Table;
    @ViewChild("paginator", { static: true }) paginator: Paginator;
    @ViewChild("ExcelFileUpload", { static: true }) excelFileUpload: FileUpload;

    uploadUrl: string;
    uploadedFiles: any[] = [];

    advancedFiltersAreShown = false;
    filterText = "";
    accountIDFilter = "";
    accountNameFilter = "";
    subLedgerFilter = -1;
    maxOptFldFilter: number;
    maxOptFldFilterEmpty: number;
    minOptFldFilter: number;
    minOptFldFilterEmpty: number;
    maxSLTypeFilter: number;
    maxSLTypeFilterEmpty: number;
    minSLTypeFilter: number;
    minSLTypeFilterEmpty: number;
    inactiveFilter = -1;
    maxCreationDateFilter: moment.Moment;
    minCreationDateFilter: moment.Moment;
    auditUserFilter = "";
    maxAuditTimeFilter: moment.Moment;
    minAuditTimeFilter: moment.Moment;
    oldCodeFilter = "";
    controlDetailSegmentNameFilter = "";
    subControlDetailSegmentNameFilter = "";
    segmentlevel3SegmentNameFilter = "";

    segment1 = "";
    segment2 = "";
    segment3 = "";

    constructor(
        injector: Injector,
        private _chartofControlsServiceProxy: ChartofControlsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _httpClient: HttpClient
    ) {
        super(injector);
        this.uploadUrl =
            AppConsts.remoteServiceBaseUrl + "/ChartofAccount/ImportFromExcel";
    }

    getChartofControls(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._chartofControlsServiceProxy
            .getAll(
                this.filterText,
                this.accountIDFilter,
                this.accountNameFilter,
                this.subLedgerFilter,
                this.maxOptFldFilter == null
                    ? this.maxOptFldFilterEmpty
                    : this.maxOptFldFilter,
                this.minOptFldFilter == null
                    ? this.minOptFldFilterEmpty
                    : this.minOptFldFilter,
                this.maxSLTypeFilter == null
                    ? this.maxSLTypeFilterEmpty
                    : this.maxSLTypeFilter,
                this.minSLTypeFilter == null
                    ? this.minSLTypeFilterEmpty
                    : this.minSLTypeFilter,
                this.inactiveFilter,
                this.maxCreationDateFilter,
                this.minCreationDateFilter,
                this.auditUserFilter,
                this.maxAuditTimeFilter,
                this.minAuditTimeFilter,
                this.oldCodeFilter,
                this.controlDetailSegmentNameFilter,
                this.subControlDetailSegmentNameFilter,
                this.segmentlevel3SegmentNameFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
        debugger;
        this._chartofControlsServiceProxy.getSegmentName().subscribe((res) => {
            this.segment1 = res.items[0].glOption.seg1Name;
            this.segment2 = res.items[0].glOption.seg2Name;
            this.segment3 = res.items[0].glOption.seg3Name;
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createChartofControl(): void {
        this.createOrEditChartofControlModal.show(false);
    }

    deleteChartofControl(chartofControl: ChartofControlDto): void {
        this.message.confirm("", (isConfirmed) => {
            if (isConfirmed) {
                this._chartofControlsServiceProxy
                    .delete(chartofControl.id)
                    .subscribe(() => {
                        this.reloadPage();
                        this.notify.success(this.l("SuccessfullyDeleted"));
                    });
            }
        });
    }

    exportToExcel(): void {
        this._chartofControlsServiceProxy
            .getChartofControlsToExcel(
                this.filterText,
                this.accountIDFilter,
                this.accountNameFilter,
                this.subLedgerFilter,
                this.maxOptFldFilter == null
                    ? this.maxOptFldFilterEmpty
                    : this.maxOptFldFilter,
                this.minOptFldFilter == null
                    ? this.minOptFldFilterEmpty
                    : this.minOptFldFilter,
                this.maxSLTypeFilter == null
                    ? this.maxSLTypeFilterEmpty
                    : this.maxSLTypeFilter,
                this.minSLTypeFilter == null
                    ? this.minSLTypeFilterEmpty
                    : this.minSLTypeFilter,
                this.inactiveFilter,
                this.maxCreationDateFilter,
                this.minCreationDateFilter,
                this.auditUserFilter,
                this.maxAuditTimeFilter,
                this.minAuditTimeFilter,
                this.oldCodeFilter,
                this.controlDetailSegmentNameFilter,
                this.subControlDetailSegmentNameFilter,
                this.segmentlevel3SegmentNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    onUpload(event): void {
        for (const file of event.files) {
            this.uploadedFiles.push(file);
        }
    }

    onBeforeSend(event): void {
        event.xhr.setRequestHeader(
            "Authorization",
            "Bearer " + abp.auth.getToken()
        );
    }

    uploadExcel(data: { files: File }): void {
        const formData: FormData = new FormData();
        const file = data.files[0];
        formData.append("file", file, file.name);
        abp.ui.setBusy(undefined, '', 1);
        this._httpClient
            .post<any>(this.uploadUrl, formData)
            .pipe(finalize(() => this.excelFileUpload.clear()))
            .subscribe((response) => {
                debugger
                if (response["error"]["message"] === null || response["error"]["message"]==="")
                    this.notify.success(
                        this.l("AllChartofAccountSuccessfullyImportedFromExcel")
                    );
                else this.message.error(response["error"]["message"]);

                abp.ui.clearBusy();
            });
    }

    onUploadExcelError(): void {
        this.notify.error(this.l("ImportChartofAccountUploadFailed"));
    }
}
