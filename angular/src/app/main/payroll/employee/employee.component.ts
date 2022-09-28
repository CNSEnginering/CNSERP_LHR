import {
    Component,
    Injector,
    ViewEncapsulation,
    ViewChild,
} from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { NotifyService } from "@abp/notify/notify.service";
import { AppComponentBase } from "@shared/common/app-component-base";
import { TokenAuthServiceProxy } from "@shared/service-proxies/service-proxies";
import { ViewEmployeeModalComponent } from "./view-employee-modal.component";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { Table } from "primeng/components/table/table";
import { Paginator } from "primeng/components/paginator/paginator";
import { LazyLoadEvent } from "primeng/components/common/lazyloadevent";
import { FileDownloadService } from "@shared/utils/file-download.service";
import * as _ from "lodash";
import * as moment from "moment";
import { EmployeesDto } from "../shared/dto/employee-dto";
import { EmployeesServiceProxy } from "../shared/services/employee-service";
import { CreateOrEditEmployeesModalComponent } from "./create-or-edit-employee-modal.component";
import { FileUpload } from "primeng/primeng";
import { AppConsts } from "@shared/AppConsts";
import { HttpClient } from "@angular/common/http";
import { finalize } from "rxjs/operators";

@Component({
    templateUrl: "./employee.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class EmployeeComponent extends AppComponentBase {
    @ViewChild("createOrEditEmployeeModal", { static: true })
    createOrEditEmployeeModal: CreateOrEditEmployeesModalComponent;
    @ViewChild("viewEmployeeModalComponent", { static: true })
    viewEmployeeModal: ViewEmployeeModalComponent;
    @ViewChild("dataTable", { static: true }) dataTable: Table;
    @ViewChild("paginator", { static: true }) paginator: Paginator;
    @ViewChild("ExcelFileUpload", { static: true }) excelFileUpload: FileUpload;

    advancedFiltersAreShown = false;
    filterText = "";
    maxEmployeeIDFilter: number;
    maxEmployeeIDFilterEmpty: number;
    minEmployeeIDFilter: number;
    minEmployeeIDFilterEmpty: number;
    employeeNameFilter = "";
    fatherNameFilter = "";
    maxdate_of_birthFilter: moment.Moment;
    mindate_of_birthFilter: moment.Moment;
    home_addressFilter = "";
    phoneNoFilter = "";
    ntnFilter = "";
    maxapointment_dateFilter: moment.Moment;
    minapointment_dateFilter: moment.Moment;
    maxdate_of_joiningFilter: moment.Moment;
    mindate_of_joiningFilter: moment.Moment;
    maxdate_of_leavingFilter: moment.Moment;
    mindate_of_leavingFilter: moment.Moment;
    cityFilter = "";
    cnicFilter = "";
    maxEdIDFilter: number;
    maxEdIDFilterEmpty: number;
    minEdIDFilter: number;
    minEdIDFilterEmpty: number;
    maxDeptIDFilter: number;
    maxDeptIDFilterEmpty: number;
    minDeptIDFilter: number;
    minDeptIDFilterEmpty: number;
    maxDesignationIDFilter: number;
    maxDesignationIDFilterEmpty: number;
    minDesignationIDFilter: number;
    minDesignationIDFilterEmpty: number;
    genderFilter = "";
    statusFilter = -1;
    maxShiftIDFilter: number;
    maxShiftIDFilterEmpty: number;
    minShiftIDFilter: number;
    minShiftIDFilterEmpty: number;
    maxTypeIDFilter: number;
    maxTypeIDFilterEmpty: number;
    minTypeIDFilter: number;
    minTypeIDFilterEmpty: number;
    maxSecIDFilter: number;
    maxSecIDFilterEmpty: number;
    minSecIDFilter: number;
    minSecIDFilterEmpty: number;
    maxReligionIDFilter: number;
    maxReligionIDFilterEmpty: number;
    minReligionIDFilter: number;
    minReligionIDFilterEmpty: number;
    social_securityFilter = -1;
    eobiFilter = -1;
    wppfFilter = -1;
    payment_modeFilter = "";
    bank_nameFilter = "";
    account_noFilter = "";
    academic_qualificationFilter = "";
    professional_qualificationFilter = "";
    maxfirst_rest_daysFilter: number;
    maxfirst_rest_daysFilterEmpty: number;
    minfirst_rest_daysFilter: number;
    minfirst_rest_daysFilterEmpty: number;
    maxsecond_rest_daysFilter: number;
    maxsecond_rest_daysFilterEmpty: number;
    minsecond_rest_daysFilter: number;
    minsecond_rest_daysFilterEmpty: number;
    maxfirst_rest_days_w2Filter: number;
    maxfirst_rest_days_w2FilterEmpty: number;
    minfirst_rest_days_w2Filter: number;
    minfirst_rest_days_w2FilterEmpty: number;
    maxsecond_rest_days_w2Filter: number;
    maxsecond_rest_days_w2FilterEmpty: number;
    minsecond_rest_days_w2Filter: number;
    minsecond_rest_days_w2FilterEmpty: number;
    bloodGroupFilter = "";
    referenceFilter = "";
    visa_DetailsFilter = "";
    driving_LicenceFilter = "";
    maxDuty_HoursFilter: number;
    maxDuty_HoursFilterEmpty: number;
    minDuty_HoursFilter: number;
    minDuty_HoursFilterEmpty: number;
    activeFilter = -1;
    confirmedFilter = -1;
    audtUserFilter = "";
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    createdByFilter = "";
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;
    uploadUrl: string;

    constructor(
        injector: Injector,
        private _employeeServiceProxy: EmployeesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _httpClient: HttpClient
    ) {
        super(injector);
        this.uploadUrl =
            AppConsts.remoteServiceBaseUrl + "/Employees/ImportFromExcel";
        //console.log( this.uploadUrl);
    }

    getEmployee(event?: LazyLoadEvent) {
        debugger;
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._employeeServiceProxy
            .getAll(
                this.filterText,
                this.maxEmployeeIDFilter == null
                    ? this.maxEmployeeIDFilterEmpty
                    : this.maxEmployeeIDFilter,
                this.minEmployeeIDFilter == null
                    ? this.minEmployeeIDFilterEmpty
                    : this.minEmployeeIDFilter,
                this.employeeNameFilter,
                this.fatherNameFilter,
                this.maxdate_of_birthFilter,
                this.mindate_of_birthFilter,
                this.home_addressFilter,
                this.phoneNoFilter,
                this.ntnFilter,
                this.maxapointment_dateFilter,
                this.minapointment_dateFilter,
                this.maxdate_of_joiningFilter,
                this.mindate_of_joiningFilter,
                this.maxdate_of_leavingFilter,
                this.mindate_of_leavingFilter,
                this.cityFilter,
                this.cnicFilter,
                this.maxEdIDFilter == null
                    ? this.maxEdIDFilterEmpty
                    : this.maxEdIDFilter,
                this.minEdIDFilter == null
                    ? this.minEdIDFilterEmpty
                    : this.minEdIDFilter,
                this.maxDeptIDFilter == null
                    ? this.maxDeptIDFilterEmpty
                    : this.maxDeptIDFilter,
                this.minDeptIDFilter == null
                    ? this.minDeptIDFilterEmpty
                    : this.minDeptIDFilter,
                this.maxDesignationIDFilter == null
                    ? this.maxDesignationIDFilterEmpty
                    : this.maxDesignationIDFilter,
                this.minDesignationIDFilter == null
                    ? this.minDesignationIDFilterEmpty
                    : this.minDesignationIDFilter,
                this.genderFilter,
                this.statusFilter,
                this.maxShiftIDFilter == null
                    ? this.maxShiftIDFilterEmpty
                    : this.maxShiftIDFilter,
                this.minShiftIDFilter == null
                    ? this.minShiftIDFilterEmpty
                    : this.minShiftIDFilter,
                this.maxTypeIDFilter == null
                    ? this.maxTypeIDFilterEmpty
                    : this.maxTypeIDFilter,
                this.minTypeIDFilter == null
                    ? this.minTypeIDFilterEmpty
                    : this.minTypeIDFilter,
                this.maxSecIDFilter == null
                    ? this.maxSecIDFilterEmpty
                    : this.maxSecIDFilter,
                this.minSecIDFilter == null
                    ? this.minSecIDFilterEmpty
                    : this.minSecIDFilter,
                this.maxReligionIDFilter == null
                    ? this.maxReligionIDFilterEmpty
                    : this.maxReligionIDFilter,
                this.minReligionIDFilter == null
                    ? this.minReligionIDFilterEmpty
                    : this.minReligionIDFilter,
                this.social_securityFilter,
                this.eobiFilter,
                this.wppfFilter,
                this.payment_modeFilter,
                this.bank_nameFilter,
                this.account_noFilter,
                this.academic_qualificationFilter,
                this.professional_qualificationFilter,
                this.maxfirst_rest_daysFilter == null
                    ? this.maxfirst_rest_daysFilterEmpty
                    : this.maxfirst_rest_daysFilter,
                this.minfirst_rest_daysFilter == null
                    ? this.minfirst_rest_daysFilterEmpty
                    : this.minfirst_rest_daysFilter,
                this.maxsecond_rest_daysFilter == null
                    ? this.maxsecond_rest_daysFilterEmpty
                    : this.maxsecond_rest_daysFilter,
                this.minsecond_rest_daysFilter == null
                    ? this.minsecond_rest_daysFilterEmpty
                    : this.minsecond_rest_daysFilter,
                this.maxfirst_rest_days_w2Filter == null
                    ? this.maxfirst_rest_days_w2FilterEmpty
                    : this.maxfirst_rest_days_w2Filter,
                this.minfirst_rest_days_w2Filter == null
                    ? this.minfirst_rest_days_w2FilterEmpty
                    : this.minfirst_rest_days_w2Filter,
                this.maxsecond_rest_days_w2Filter == null
                    ? this.maxsecond_rest_days_w2FilterEmpty
                    : this.maxsecond_rest_days_w2Filter,
                this.minsecond_rest_days_w2Filter == null
                    ? this.minsecond_rest_days_w2FilterEmpty
                    : this.minsecond_rest_days_w2Filter,
                this.bloodGroupFilter,
                this.referenceFilter,
                this.visa_DetailsFilter,
                this.driving_LicenceFilter,
                this.maxDuty_HoursFilter == null
                    ? this.maxDuty_HoursFilterEmpty
                    : this.maxDuty_HoursFilter,
                this.minDuty_HoursFilter == null
                    ? this.minDuty_HoursFilterEmpty
                    : this.minDuty_HoursFilter,
                this.activeFilter,
                this.confirmedFilter,
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
                debugger;
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createEmployee(): void {
        this.createOrEditEmployeeModal.show();
    }

    deleteEmployee(Employee: EmployeesDto): void {
        this.message.confirm("", this.l("AreYouSure"), (isConfirmed) => {
            if (isConfirmed) {
                this._employeeServiceProxy.delete(Employee.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l("SuccessfullyDeleted"));
                });
            }
        });
    }

    exportToExcel(): void {
        this._employeeServiceProxy
            .getEmployeesToExcel(
                this.filterText,
                this.maxEmployeeIDFilter == null
                    ? this.maxEmployeeIDFilterEmpty
                    : this.maxEmployeeIDFilter,
                this.minEmployeeIDFilter == null
                    ? this.minEmployeeIDFilterEmpty
                    : this.minEmployeeIDFilter,
                this.employeeNameFilter,
                this.fatherNameFilter,
                this.maxdate_of_birthFilter,
                this.mindate_of_birthFilter,
                this.home_addressFilter,
                this.phoneNoFilter,
                this.ntnFilter,
                this.maxapointment_dateFilter,
                this.minapointment_dateFilter,
                this.maxdate_of_joiningFilter,
                this.mindate_of_joiningFilter,
                this.maxdate_of_leavingFilter,
                this.mindate_of_leavingFilter,
                this.cityFilter,
                this.cnicFilter,
                this.maxEdIDFilter == null
                    ? this.maxEdIDFilterEmpty
                    : this.maxEdIDFilter,
                this.minEdIDFilter == null
                    ? this.minEdIDFilterEmpty
                    : this.minEdIDFilter,
                this.maxDeptIDFilter == null
                    ? this.maxDeptIDFilterEmpty
                    : this.maxDeptIDFilter,
                this.minDeptIDFilter == null
                    ? this.minDeptIDFilterEmpty
                    : this.minDeptIDFilter,
                this.maxDesignationIDFilter == null
                    ? this.maxDesignationIDFilterEmpty
                    : this.maxDesignationIDFilter,
                this.minDesignationIDFilter == null
                    ? this.minDesignationIDFilterEmpty
                    : this.minDesignationIDFilter,
                this.genderFilter,
                this.statusFilter,
                this.maxShiftIDFilter == null
                    ? this.maxShiftIDFilterEmpty
                    : this.maxShiftIDFilter,
                this.minShiftIDFilter == null
                    ? this.minShiftIDFilterEmpty
                    : this.minShiftIDFilter,
                this.maxTypeIDFilter == null
                    ? this.maxTypeIDFilterEmpty
                    : this.maxTypeIDFilter,
                this.minTypeIDFilter == null
                    ? this.minTypeIDFilterEmpty
                    : this.minTypeIDFilter,
                this.maxSecIDFilter == null
                    ? this.maxSecIDFilterEmpty
                    : this.maxSecIDFilter,
                this.minSecIDFilter == null
                    ? this.minSecIDFilterEmpty
                    : this.minSecIDFilter,
                this.maxReligionIDFilter == null
                    ? this.maxReligionIDFilterEmpty
                    : this.maxReligionIDFilter,
                this.minReligionIDFilter == null
                    ? this.minReligionIDFilterEmpty
                    : this.minReligionIDFilter,
                this.social_securityFilter,
                this.eobiFilter,
                this.wppfFilter,
                this.payment_modeFilter,
                this.bank_nameFilter,
                this.account_noFilter,
                this.academic_qualificationFilter,
                this.professional_qualificationFilter,
                this.maxfirst_rest_daysFilter == null
                    ? this.maxfirst_rest_daysFilterEmpty
                    : this.maxfirst_rest_daysFilter,
                this.minfirst_rest_daysFilter == null
                    ? this.minfirst_rest_daysFilterEmpty
                    : this.minfirst_rest_daysFilter,
                this.maxsecond_rest_daysFilter == null
                    ? this.maxsecond_rest_daysFilterEmpty
                    : this.maxsecond_rest_daysFilter,
                this.minsecond_rest_daysFilter == null
                    ? this.minsecond_rest_daysFilterEmpty
                    : this.minsecond_rest_daysFilter,
                this.maxfirst_rest_days_w2Filter == null
                    ? this.maxfirst_rest_days_w2FilterEmpty
                    : this.maxfirst_rest_days_w2Filter,
                this.minfirst_rest_days_w2Filter == null
                    ? this.minfirst_rest_days_w2FilterEmpty
                    : this.minfirst_rest_days_w2Filter,
                this.maxsecond_rest_days_w2Filter == null
                    ? this.maxsecond_rest_days_w2FilterEmpty
                    : this.maxsecond_rest_days_w2Filter,
                this.minsecond_rest_days_w2Filter == null
                    ? this.minsecond_rest_days_w2FilterEmpty
                    : this.minsecond_rest_days_w2Filter,
                this.bloodGroupFilter,
                this.referenceFilter,
                this.visa_DetailsFilter,
                this.driving_LicenceFilter,
                this.maxDuty_HoursFilter == null
                    ? this.maxDuty_HoursFilterEmpty
                    : this.maxDuty_HoursFilter,
                this.minDuty_HoursFilter == null
                    ? this.minDuty_HoursFilterEmpty
                    : this.minDuty_HoursFilter,
                this.activeFilter,
                this.confirmedFilter,
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

    uploadedFiles: any[] = [];
    onUpload(event): void {
        for (const file of event.files) {
            this.uploadedFiles.push(file);
        }
    }

    uploadExcel(data: { files: File }): void {
        const formData: FormData = new FormData();
        const file = data.files[0];
        formData.append("file", file, file.name);
        abp.ui.setBusy(undefined, "", 1);
        this._httpClient
            .post<any>(this.uploadUrl, formData)
            .pipe(finalize(() => this.excelFileUpload.clear()))
            .subscribe((response) => {
                if (response["error"]["message"] === null)
                    this.notify.success(
                        this.l("AllEmployeesSuccessfullyImportedFromExcel")
                    );
                else this.message.error(response["error"]["message"]);

                abp.ui.clearBusy();
            });
    }

    onUploadExcelError(): void {
        this.notify.error(this.l("ImportEmployeeUploadFailed"));
    }
}
