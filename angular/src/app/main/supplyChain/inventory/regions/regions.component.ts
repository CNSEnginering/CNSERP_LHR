import {
    Component,
    OnInit,
    Injector,
    ViewChild,
    ViewEncapsulation
} from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { LazyLoadEvent } from "primeng/components/common/lazyloadevent";
import { Paginator } from "primeng/components/paginator/paginator";
import { Table } from "primeng/components/table/table";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { RegionService } from "../shared/services/regions.service";
import { CreateOrEditRegionsModalComponent } from "./create-or-edit-regions-modal.component";
@Component({
    templateUrl: "./regions.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class RegionsComponent extends AppComponentBase implements OnInit {
    filterText = "";
    costCenter: any;
    sorting: any;
    skipCount: any;
    MaxResultCount: any;

    @ViewChild("dataTable", { static: true }) dataTable: Table;
    @ViewChild("paginator", { static: true }) paginator: Paginator;
    @ViewChild("regionsModal", { static: true })
    regionsModal: CreateOrEditRegionsModalComponent;
    constructor(
        injector: Injector,
        private _regionService: RegionService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    ngOnInit() {}
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
        this._regionService
            .getAll(
                this.filterText,
                this.sorting,
                this.skipCount,
                this.MaxResultCount,
                undefined
            )
            .subscribe(data => {
                debugger;
                this.primengTableHelper.totalRecordsCount =
                    data["result"]["totalCount"];
                this.primengTableHelper.records = data["result"]["items"];
                this.primengTableHelper.hideLoadingIndicator();
            });
    }
    delete(id: any) {
        debugger;
        this.message.confirm("", this.l("AreYouSure"), isConfirmed => {
            if (isConfirmed) {
                this._regionService.delete(id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l("SuccessfullyDeleted"));
                });
            }
        });
    }
    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }
    createOrEdit(id: number) {
        this.regionsModal.show(id);
    }
    // view(data: any) {
    //     this.viewCostCenterModal.show(data);
    // }
    // exportToExcel() {
    //     this._costCenterService
    //         .GetDataToExcel(
    //             this.filterText,
    //             this.sorting,
    //             this.skipCount,
    //             this.MaxResultCount
    //         )
    //         .subscribe((result: any) => {
    //             this._fileDownloadService.downloadTempFile(result["result"]);
    //         });
    // }
}
