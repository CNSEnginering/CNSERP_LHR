import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEmployeeSalaryModalComponent } from './create-or-edit-employeeSalary-modal.component';
import { ViewEmployeeSalaryModalComponent } from './view-employeeSalary-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { EmployeeSalaryServiceProxy } from '../shared/services/employeeSalary.service';
import { EmployeeSalaryDto } from '../shared/dto/employeeSalary-dto';
import { finalize } from 'rxjs/operators';
import { FileUpload } from 'primeng/primeng';
import { HttpClient } from '@angular/common/http';
import { AppConsts } from '@shared/AppConsts';

@Component({
    templateUrl: './employeeSalary.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class EmployeeSalaryComponent extends AppComponentBase {

    @ViewChild('createOrEditEmployeeSalaryModal', { static: true }) createOrEditEmployeeSalaryModal: CreateOrEditEmployeeSalaryModalComponent;
    @ViewChild('viewEmployeeSalaryModalComponent', { static: true }) viewEmployeeSalaryModal: ViewEmployeeSalaryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('ExcelFileUpload', { static: true }) excelFileUpload: FileUpload;

    advancedFiltersAreShown = false;
    filterText = '';
    maxEmployeeIDFilter: number;
    maxEmployeeIDFilterEmpty: number;
    minEmployeeIDFilter: number;
    minEmployeeIDFilterEmpty: number;
    employeeNameFilter = '';
    maxBank_AmountFilter: number;
    maxBank_AmountFilterEmpty: number;
    minBank_AmountFilter: number;
    minBank_AmountFilterEmpty: number;
    maxStartDateFilter: moment.Moment;
    minStartDateFilter: moment.Moment;
    maxGross_SalaryFilter: number;
    maxGross_SalaryFilterEmpty: number;
    minGross_SalaryFilter: number;
    minGross_SalaryFilterEmpty: number;
    maxBasic_SalaryFilter: number;
    maxBasic_SalaryFilterEmpty: number;
    minBasic_SalaryFilter: number;
    minBasic_SalaryFilterEmpty: number;
    maxTaxFilter: number;
    maxTaxFilterEmpty: number;
    minTaxFilter: number;
    minTaxFilterEmpty: number;
    maxHouse_RentFilter: number;
    maxHouse_RentFilterEmpty: number;
    minHouse_RentFilter: number;
    minHouse_RentFilterEmpty: number; 
    maxNet_SalaryFilter: number;
    maxNet_SalaryFilterEmpty: number;
    minNet_SalaryFilter: number;
    minNet_SalaryFilterEmpty: number; 
    audtUserFilter = '';
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    createdByFilter = '';
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;
    uploadUrl: string;



    constructor(
        injector: Injector,
        private _employeeSalaryServiceProxy: EmployeeSalaryServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _httpClient: HttpClient
    ) {        
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/EmployeesSalary/ImportFromExcel';
    }

    getEmployeeSalary(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._employeeSalaryServiceProxy.getAll(
            this.filterText,
            this.maxEmployeeIDFilter == null ? this.maxEmployeeIDFilterEmpty : this.maxEmployeeIDFilter,
            this.minEmployeeIDFilter == null ? this.minEmployeeIDFilterEmpty : this.minEmployeeIDFilter,
            this.employeeNameFilter,
            this.maxBank_AmountFilter == null ? this.maxBank_AmountFilterEmpty : this.maxBank_AmountFilter,
            this.minBank_AmountFilter == null ? this.minBank_AmountFilterEmpty : this.minBank_AmountFilter,
            this.maxStartDateFilter,
            this.minStartDateFilter,
            this.maxGross_SalaryFilter == null ? this.maxGross_SalaryFilterEmpty : this.maxGross_SalaryFilter,
            this.minGross_SalaryFilter == null ? this.minGross_SalaryFilterEmpty : this.minGross_SalaryFilter,
            this.maxBasic_SalaryFilter == null ? this.maxBasic_SalaryFilterEmpty : this.maxBasic_SalaryFilter,
            this.minBasic_SalaryFilter == null ? this.minBasic_SalaryFilterEmpty : this.minBasic_SalaryFilter,
            this.maxTaxFilter == null ? this.maxTaxFilterEmpty : this.maxTaxFilter,
            this.minTaxFilter == null ? this.minTaxFilterEmpty : this.minTaxFilter,
            this.maxHouse_RentFilter == null ? this.maxHouse_RentFilterEmpty : this.maxHouse_RentFilter,
            this.minHouse_RentFilter == null ? this.minHouse_RentFilterEmpty : this.minHouse_RentFilter,
            this.maxNet_SalaryFilter == null ? this.maxNet_SalaryFilterEmpty : this.maxNet_SalaryFilter,
            this.minNet_SalaryFilter == null ? this.minNet_SalaryFilterEmpty : this.minNet_SalaryFilter,
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
            debugger;
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createEmployeeSalary(): void {
        this.createOrEditEmployeeSalaryModal.show();
    }

    deleteEmployeeSalary(employeeSalary: EmployeeSalaryDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._employeeSalaryServiceProxy.delete(employeeSalary.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._employeeSalaryServiceProxy.getEmployeeSalaryToExcel(
            this.filterText,
            this.maxEmployeeIDFilter == null ? this.maxEmployeeIDFilterEmpty : this.maxEmployeeIDFilter,
            this.minEmployeeIDFilter == null ? this.minEmployeeIDFilterEmpty : this.minEmployeeIDFilter,
            this.employeeNameFilter,
            this.maxBank_AmountFilter == null ? this.maxBank_AmountFilterEmpty : this.maxBank_AmountFilter,
            this.minBank_AmountFilter == null ? this.minBank_AmountFilterEmpty : this.minBank_AmountFilter,
            this.maxStartDateFilter,
            this.minStartDateFilter,
            this.maxGross_SalaryFilter == null ? this.maxGross_SalaryFilterEmpty : this.maxGross_SalaryFilter,
            this.minGross_SalaryFilter == null ? this.minGross_SalaryFilterEmpty : this.minGross_SalaryFilter,
            this.maxBasic_SalaryFilter == null ? this.maxBasic_SalaryFilterEmpty : this.maxBasic_SalaryFilter,
            this.minBasic_SalaryFilter == null ? this.minBasic_SalaryFilterEmpty : this.minBasic_SalaryFilter,
            this.maxTaxFilter == null ? this.maxTaxFilterEmpty : this.maxTaxFilter,
            this.minTaxFilter == null ? this.minTaxFilterEmpty : this.minTaxFilter,
            this.maxHouse_RentFilter == null ? this.maxHouse_RentFilterEmpty : this.maxHouse_RentFilter,
            this.minHouse_RentFilter == null ? this.minHouse_RentFilterEmpty : this.minHouse_RentFilter,
            this.maxNet_SalaryFilter == null ? this.maxNet_SalaryFilterEmpty : this.maxNet_SalaryFilter,
            this.minNet_SalaryFilter == null ? this.minNet_SalaryFilterEmpty : this.minNet_SalaryFilter,
            this.audtUserFilter,
            this.maxAudtDateFilter,
            this.minAudtDateFilter,
            this.createdByFilter,
            this.maxCreateDateFilter,
            this.minCreateDateFilter,
        )
            .subscribe(result => {
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
    formData.append('file', file, file.name);

    this._httpClient
        .post<any>(this.uploadUrl, formData)
        .pipe(finalize(() => this.excelFileUpload.clear()))
        .subscribe(response => {
            debugger;
            if (response["error"]["message"] === null) {
                this.notify.success(this.l('ImportEmployeeSalaryProcessStart'));
            } else  {
                this.notify.error(this.l('ImportEmployeeSalaryUploadFailed'));
            }
        });
}

onUploadExcelError(): void {
    this.notify.error(this.l('ImportEmployeeSalaryUploadFailed'));
}

}
