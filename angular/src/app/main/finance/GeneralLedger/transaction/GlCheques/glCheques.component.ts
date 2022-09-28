import {
    Component,
    OnInit,
    Injector,
    ViewChild,
    ViewEncapsulation,
    Output,
    EventEmitter
} from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { Table } from "primeng/table";
import { Paginator } from "primeng/primeng";
import { LazyLoadEvent } from "primeng/api";
import { GlChequesService } from "@app/main/finance/shared/services/glCheques.service";
import { CreateOrEditGlChequesModalComponent } from "./create-or-edit-glCheques-modal.component";
import { ViewGlChequesModalComponent } from "./view-glCheques-modal.component";
import { ActivatedRoute } from "@angular/router";

@Component({
    templateUrl: "./glCheques.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class GlChequescomponent extends AppComponentBase implements OnInit {
    @ViewChild("dataTable", { static: true }) dataTable: Table;
    @ViewChild("paginator", { static: true }) paginator: Paginator;
    @ViewChild("glCheques", { static: true })
    glCheques: CreateOrEditGlChequesModalComponent;
    @ViewChild("viewGlChequesModal", { static: true })
    viewGlChequesModal: ViewGlChequesModalComponent;
    advancedFiltersAreShown = false;
    filterText = "";
    sorting: any;
    skipCount: any;
    MaxResultCount: any;
    chequeStatus: any;

    typeID = 1;
    viewOption: any;

    constructor(
        injector: Injector,
        private _fileDownloadService: FileDownloadService,
        private _service: GlChequesService,
        private _activatedRoute: ActivatedRoute,
    ) {
        super(injector);
    }

    ngOnInit() {
        this.viewOption = this._activatedRoute.snapshot.data.viewOption;
        if (this.viewOption === "PostDatedCheque") {

            this.typeID = 1;
            this.chequeStatus = 0;
        }
        else
        {
            this.typeID = 0;
            this.chequeStatus = 0;
        }

    }
    reloadPage(): void {
        this.getAll();
    }
    chequeStatusChange(event) {
        debugger;
        this.getAll();
    }
    getAll(event?: LazyLoadEvent) {
        this.sorting = this.primengTableHelper.getSorting(this.dataTable);
        this.skipCount = this.primengTableHelper.getSkipCount(
            this.paginator,
            event
        );
        this.MaxResultCount = this.primengTableHelper.getMaxResultCount(
            this.paginator,
            event
        );

        this.primengTableHelper.showLoadingIndicator();
        this._service
            .getAll(
                this.filterText,
                this.sorting,
                this.skipCount,
                this.MaxResultCount,
                this.chequeStatus,
                this.typeID
            )
            .subscribe(data => {
                console.log(data);
                this.primengTableHelper.totalRecordsCount =
                    data["result"]["totalCount"];
                this.primengTableHelper.records = data["result"]["items"];
                this.primengTableHelper.hideLoadingIndicator();
            });
    }
    delete(id: number) {
        this.message.confirm("", this.l("AreYouSure"), isConfirmed => {
            if (isConfirmed) {
                this._service.delete(id).subscribe(data => {
                    this.reloadPage();
                    this.notify.success(this.l("SuccessfullyDeleted"));
                });
            }
        });
    }
    createOrEdit(id: number) {
        var type = this.viewOption == "PostDatedCheque" ? " (AR)" :" (AP)"
        this.glCheques.show(id,type);
    }
    exportToExcel() {
        this._service
            .GetDataToExcel(
                this.filterText,
                this.sorting,
                this.skipCount,
                this.MaxResultCount
            )
            .subscribe((result: any) => {
                this._fileDownloadService.downloadTempFile(result["result"]);
            });
    }
    view(data: any) {
        this.viewGlChequesModal.show(data);
    }
}
