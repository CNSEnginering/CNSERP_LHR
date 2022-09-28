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
import { CreateOrEditDepartmentModalComponent } from './create-or-edit-departments-modal.component';
import { DepartmentsServiceProxy } from '../shared/services/department.service';
import { DepartmentDto } from '../shared/dto/department-dto';
import { ViewDepartmentModalComponent } from './view-departments-modal.component';

@Component({
    templateUrl: './departments.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class DepartmentsComponent extends AppComponentBase {

    @ViewChild('createOrEditDepartmentModal', { static: true }) createOrEditDepartmentModal: CreateOrEditDepartmentModalComponent;
    @ViewChild('viewDepartmentModalComponent', { static: true }) viewDepartmentModal: ViewDepartmentModalComponent;
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
        private _departmentsServiceProxy: DepartmentsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getDepartments(event?: LazyLoadEvent) {
        debugger;
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._departmentsServiceProxy.getAll(
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
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        debugger;
        this.paginator.changePage(this.paginator.getPage());
    }

    createDepartment(): void {
        debugger;
        this.createOrEditDepartmentModal.show();
    }

    deleteDepartment(department: DepartmentDto): void {
        debugger;
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._departmentsServiceProxy.delete(department.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        debugger;
        this._departmentsServiceProxy.getDepartmentsToExcel(
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
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
