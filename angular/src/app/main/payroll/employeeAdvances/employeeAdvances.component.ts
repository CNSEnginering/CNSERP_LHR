import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { EmployeeDeductionsDto } from '../shared/dto/employeeDeductions-dto';
import { ViewEmployeeDeductionsModalComponent } from '../employeeDeductions/view-employeeDeductions-modal.component';
import { CreateOrEditEmployeeAdvancesModalComponent } from './create-or-edit-employee-advances-modal.component';
import { EmployeeAdvancesServiceProxy } from '../shared/services/employeeAdvances.service';
import { ViewEmployeeAdvancesModalComponent } from './view-employeeAdvances-modal.component';

@Component({
    templateUrl: './employeeAdvances.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class EmployeeAdvancesComponent extends AppComponentBase {

    @ViewChild('createOrEditEmployeeAdvancesModal', { static: true }) createOrEditEmployeeAdvancesModal: CreateOrEditEmployeeAdvancesModalComponent;
    @ViewChild('viewEmployeeAdvancesModal', { static: true }) viewEmployeeAdvancesModal: ViewEmployeeAdvancesModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = undefined;
   



    constructor(
        injector: Injector,
        private _employeeAdvancesServiceProxy: EmployeeAdvancesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getEmployeeAdvances(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._employeeAdvancesServiceProxy.getAll(
            this.filterText,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createEmployeeAdvances(): void {
        this.createOrEditEmployeeAdvancesModal.show();
    }

    deleteEmployeeAdvances(employeeDeductions: EmployeeDeductionsDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._employeeAdvancesServiceProxy.delete(employeeDeductions.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._employeeAdvancesServiceProxy.getEmployeeAdvancesToExcel(
            this.filterText
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
