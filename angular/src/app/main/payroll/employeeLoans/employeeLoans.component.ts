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
// import { CreateOrEditDepartmentModalComponent } from './create-or-edit-departments-modal.component';
import { DepartmentsServiceProxy } from '../shared/services/department.service';
import { DepartmentDto } from '../shared/dto/department-dto';
import { EmployeeLoansService } from '../shared/services/employeeLoans.service';
import { CreateOrEditEmployeeLoansModalComponent } from './create-or-edit-employeeLoans-modal.component';
import { employeeLoansDto } from '../shared/dto/employeeLoans-dto';
// import { ViewDepartmentModalComponent } from './view-departments-modal.component';

@Component({
    templateUrl: './employeeLoans.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class EmployeeLoansComponent extends AppComponentBase {

     @ViewChild('createOrEditEmployeeLoansModal', { static: true }) createOrEditEmployeeLoansModal: CreateOrEditEmployeeLoansModalComponent;
    // @ViewChild('viewDepartmentModalComponent', { static: true }) viewDepartmentModal: ViewDepartmentModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDeptIDFilter: number;
    maxDeptIDFilterEmpty: number;
    minDeptIDFilter: number;
    minDeptIDFilterEmpty: number;
    deptNameFilter = '';
    maxActiveFilter: number;
    maxActiveFilterEmpty: number;
    minActiveFilter: number;
    minActiveFilterEmpty: number;
    audtUserFilter = '';
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;
    createdByFilter = '';
    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;




    constructor(
        injector: Injector,
        private _employeeLoanService: EmployeeLoansService,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getEmployeeLoans(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        debugger
        this._employeeLoanService.getAll(
            this.filterText,
            this.maxDeptIDFilter == null ? this.maxDeptIDFilterEmpty : this.maxDeptIDFilter,
            this.minDeptIDFilter == null ? this.minDeptIDFilterEmpty : this.minDeptIDFilter,
            this.deptNameFilter,
            this.maxActiveFilter == null ? this.maxActiveFilterEmpty : this.maxActiveFilter,
            this.minActiveFilter == null ? this.minActiveFilterEmpty : this.minActiveFilter,
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
            console.log(result);
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createLoans(id?:number): void {
        this.createOrEditEmployeeLoansModal.show(id);
    }

    deleteEmployeeLoan(employeeLoan: employeeLoansDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._employeeLoanService.delete(employeeLoan.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    // exportToExcel(): void {
    //     debugger;
    //     this._departmentsServiceProxy.getDepartmentsToExcel(
    //         this.filterText,
    //         this.maxDeptIDFilter == null ? this.maxDeptIDFilterEmpty : this.maxDeptIDFilter,
    //         this.minDeptIDFilter == null ? this.minDeptIDFilterEmpty : this.minDeptIDFilter,
    //         this.deptNameFilter,
    //         this.maxActiveFilter == null ? this.maxActiveFilterEmpty : this.maxActiveFilter,
    //         this.minActiveFilter == null ? this.minActiveFilterEmpty : this.minActiveFilter,
    //         this.audtUserFilter,
    //         this.maxAudtDateFilter,
    //         this.minAudtDateFilter,
    //         this.createdByFilter,
    //         this.maxCreateDateFilter,
    //         this.minCreateDateFilter,
    //     )
    //         .subscribe(result => {
    //             this._fileDownloadService.downloadTempFile(result);
    //         });
    // }
}
