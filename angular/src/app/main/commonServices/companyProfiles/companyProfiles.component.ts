import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CompanyProfilesServiceProxy, CompanyProfileDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditCompanyProfileModalComponent } from './create-or-edit-companyProfile-modal.component';
import { ViewCompanyProfileModalComponent } from './view-companyProfile-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './companyProfiles.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class CompanyProfilesComponent extends AppComponentBase {

    @ViewChild('createOrEditCompanyProfileModal', { static: true }) createOrEditCompanyProfileModal: CreateOrEditCompanyProfileModalComponent;
    @ViewChild('viewCompanyProfileModalComponent', { static: true }) viewCompanyProfileModal: ViewCompanyProfileModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    companyNameFilter = '';
    phoneFilter = '';
    disableCreateButton = false;


    constructor(
        injector: Injector,
        private _companyProfilesServiceProxy: CompanyProfilesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getCompanyProfiles(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._companyProfilesServiceProxy.getAll(
            this.filterText,
            this.companyNameFilter,
            this.phoneFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            debugger;
            if (this.primengTableHelper.totalRecordsCount > 0) {
              this.disableCreateButton = true;
            }
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createCompanyProfile(): void {
        this.createOrEditCompanyProfileModal.show();
    }

    deleteCompanyProfile(companyProfile: CompanyProfileDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._companyProfilesServiceProxy.delete(companyProfile.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
        this.disableCreateButton = false;
    }

    exportToExcel(): void {
        this._companyProfilesServiceProxy.getCompanyProfilesToExcel(
        this.filterText,
            this.companyNameFilter,
            this.phoneFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
