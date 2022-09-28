import {
    Component,
    Injector,
    ViewEncapsulation,
    ViewChild,
    Input,
    OnInit,
} from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import {
    AccountSubLedgersServiceProxy,
    AccountSubLedgerDto,
    DemoUiComponentsServiceProxy,
} from "@shared/service-proxies/service-proxies";
import { NotifyService } from "@abp/notify/notify.service";
import { AppComponentBase } from "@shared/common/app-component-base";
import { TokenAuthServiceProxy } from "@shared/service-proxies/service-proxies";
import { CreateOrEditAccountSubLedgerModalComponent } from "./create-or-edit-accountSubLedger-modal.component";
import { ViewAccountSubLedgerModalComponent } from "./view-accountSubLedger-modal.component";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { Table } from "primeng/components/table/table";
import { Paginator } from "primeng/components/paginator/paginator";
import { LazyLoadEvent } from "primeng/components/common/lazyloadevent";
import { FileDownloadService } from "@shared/utils/file-download.service";
import * as _ from "lodash";
import * as moment from "moment";
import { FileUploaderComponent } from "@app/shared/common/file-uploader/file-uploader.component";
import { AppConsts } from "@shared/AppConsts";
import { FileUpload } from "primeng/primeng";
import { finalize } from "rxjs/operators";
import { HttpClient } from "@angular/common/http";
import { TransferAccountSubLedgerModalComponent } from "./transfer-accountSubLedger-modal.component";

@Component({
    selector: "accountSubLedgersSelector",
    templateUrl: "./accountSubLedgers.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class AccountSubLedgersComponent
    extends AppComponentBase
    implements OnInit {
    @Input() mode: string;
    @ViewChild("createOrEditAccountSubLedgerModal", { static: true })
    createOrEditAccountSubLedgerModal: CreateOrEditAccountSubLedgerModalComponent;
    @ViewChild("viewAccountSubLedgerModalComponent", { static: true })
    viewAccountSubLedgerModal: ViewAccountSubLedgerModalComponent;
    @ViewChild("dataTable", { static: true }) dataTable: Table;
    @ViewChild("paginator", { static: true }) paginator: Paginator;
    @ViewChild("ExcelFileUpload", { static: true }) excelFileUpload: FileUpload;
    @ViewChild("transferAccountSubLedgerModal", { static: true })
    transferAccountSubLedgerModal: TransferAccountSubLedgerModalComponent;

    uploadUrl: string;
    uploadedFiles: any[] = [];
    create = "Create New Sub Ledger";
    accountSubLedgers = "Sub Ledger";
    accountSubLedgersHeaderInfo = "Manage Account Sub Ledger";

    advancedFiltersAreShown = false;
    filterText = "";
    slType="";
    slDesc="";
    accountIDFilter = "";
    maxSubAccIDFilter: number;
    maxSubAccIDFilterEmpty: number;
    minSubAccIDFilter: number;
    minSubAccIDFilterEmpty: number;
    subAccNameFilter = "";
    address1Filter = "";
    address2Filter = "";
    cityFilter = "";
    phoneFilter = "";
    contactFilter = "";
    regNoFilter = "";
    taxauthFilter = "";
    maxClassIDFilter: number;
    maxClassIDFilterEmpty: number;
    minClassIDFilter: number;
    minClassIDFilterEmpty: number;
    oldSLFilter = "";
    maxLedgerTypeFilter: number;
    maxLedgerTypeFilterEmpty: number;
    minLedgerTypeFilter: number;
    minLedgerTypeFilterEmpty: number;
    agreement1Filter = "";
    agreement2Filter = "";
    maxPayTermFilter: number;
    maxPayTermFilterEmpty: number;
    minPayTermFilter: number;
    minPayTermFilterEmpty: number;
    otherConditionFilter = "";
    referenceFilter = "";
    maxAUDTDATEFilter: moment.Moment;
    minAUDTDATEFilter: moment.Moment;
    audtuserFilter = "";
    maxTenantIDFilter: number;
    maxTenantIDFilterEmpty: number;
    minTenantIDFilter: number;
    minTenantIDFilterEmpty: number;
    chartofControlAccountNameFilter = "";
    taxAuthorityTAXAUTHDESCFilter = "";

    constructor(
        injector: Injector,
        private _accountSubLedgersServiceProxy: AccountSubLedgersServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _httpClient: HttpClient
    ) {
        super(injector);
        this.uploadUrl =
            AppConsts.remoteServiceBaseUrl + "/Subledger/ImportFromExcel";
    }
    getCreate(type?: string, header?: string, headerinfo?: string) {
        debugger;
        this.create = type;
        this.accountSubLedgers = header;
        this.accountSubLedgersHeaderInfo = headerinfo;
    }

    getAccountSubLedgers(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._accountSubLedgersServiceProxy
            .getAll(
                
                this.filterText,
                this.accountIDFilter,
                this.subAccNameFilter,
                this.cityFilter,
                this.phoneFilter,
                this.contactFilter,
                this.regNoFilter,
                this.taxauthFilter,
                this.chartofControlAccountNameFilter,
                this.taxAuthorityTAXAUTHDESCFilter,
                this.slType,
                this.slDesc,
                this.minSubAccIDFilter,
                this.maxSubAccIDFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                console.log(result)
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createAccountSubLedger(): void {
        this.createOrEditAccountSubLedgerModal.show(false);
    }

    transferAccountSubLedger(): void {
        this.transferAccountSubLedgerModal.show();
    }

    deleteAccountSubLedger(accountSubLedger: AccountSubLedgerDto): void {
        this.message.confirm("", (isConfirmed) => {
            if (isConfirmed) {
                this._accountSubLedgersServiceProxy
                    .delete(accountSubLedger.id, accountSubLedger.accountID)
                    .subscribe(() => {
                        this.reloadPage();
                        this.notify.success(this.l("SuccessfullyDeleted"));
                    });
            }
        });
    }

    exportToExcel(): void {
        this._accountSubLedgersServiceProxy
            .getAccountSubLedgersToExcel(
                this.filterText,
                this.accountIDFilter,
                this.subAccNameFilter,
                this.cityFilter,
                this.phoneFilter,
                this.contactFilter,
                this.regNoFilter,
                this.taxauthFilter,
                this.chartofControlAccountNameFilter,
                this.taxAuthorityTAXAUTHDESCFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    ngOnInit() {
        console.log("Account Sub Ledger :" + this.mode);
        if (this.mode != undefined) this.slType = this.mode;
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

        this._httpClient
            .post<any>(this.uploadUrl, formData)
            .pipe(finalize(() => this.excelFileUpload.clear()))
            .subscribe((response) => {
                if (response.success) {
                    this.notify.success(this.l("ImportSubLedgerProcessStart"));
                } else if (response.error != null) {
                    this.notify.error(this.l("ImportSubLedgerUploadFailed"));
                }
            });
    }

    onUploadExcelError(): void {
        this.notify.error(this.l("ImportSubLedgerUploadFailed"));
    }
}
