import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditICLocationModalComponent } from './create-or-edit-icLocation-modal.component';
import { ViewICLocationModalComponent } from './view-icLocation-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { ICLocationsService } from '../shared/services/ic-locations.service';
import { ICLocationDto } from '../shared/dto/ic-locations-dto';
import { GetDataService } from "../shared/services/get-data.service";
@Component({
    templateUrl: './icLocations.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ICLocationsComponent extends AppComponentBase {

    @ViewChild('createOrEditICLocationModal', { static: true }) createOrEditICLocationModal: CreateOrEditICLocationModalComponent;
    @ViewChild('viewICLocationModalComponent', { static: true }) viewICLocationModal: ViewICLocationModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
 
    advancedFiltersAreShown = false;
    filterText = '';
    maxLocIDFilter : number;
    LocIDFilter : number;
		maxLocIDFilterEmpty : number;
		minLocIDFilter : number;
		minLocIDFilterEmpty : number;
    locNameFilter = '';
    locShortFilter = '';
    addressFilter = '';
    cityFilter = '';
    allowRecFilter = -1;
    allowNegFilter = -1;
    activeFilter = -1;
    createdByFilter = '';
    maxCreateDateFilter : moment.Moment;
		minCreateDateFilter : moment.Moment;
    audtUserFilter = '';
    maxAudtDateFilter : moment.Moment;
		minAudtDateFilter : moment.Moment;
        locations: any;

    constructor(
        injector: Injector,
        private _icLocationsService: ICLocationsService,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,private _getDataService: GetDataService
    ) {
        super(injector);
    }
    getLocations(target: string): void {
		debugger;
		this._getDataService.getList(target).subscribe(result => {
			this.locations = result;
		});
	}
    getICLocations(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }
        debugger;
        this.primengTableHelper.showLoadingIndicator();
       this.minLocIDFilter=this.maxLocIDFilter;

        this._icLocationsService.getAll(
            this.filterText,
            this.maxLocIDFilter == null ? this.maxLocIDFilterEmpty: this.maxLocIDFilter,
            this.minLocIDFilter == null ? this.minLocIDFilterEmpty: this.minLocIDFilter,
            this.locNameFilter,
            this.locShortFilter,
            this.addressFilter,
            this.cityFilter,
            this.allowRecFilter,
            this.allowNegFilter,
            this.activeFilter,
            this.createdByFilter,
            this.maxCreateDateFilter,
            this.minCreateDateFilter,
            this.audtUserFilter,
            this.maxAudtDateFilter,
            this.minAudtDateFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {debugger;
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
            this.getLocations("PICLocations");
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createICLocation(): void {
        this.createOrEditICLocationModal.show();
    }

    deleteICLocation(icLocation: ICLocationDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._icLocationsService.delete(icLocation.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._icLocationsService.getICLocationsToExcel(
        this.filterText,
            this.maxLocIDFilter == null ? this.maxLocIDFilterEmpty: this.maxLocIDFilter,
            this.minLocIDFilter == null ? this.minLocIDFilterEmpty: this.minLocIDFilter,
            this.locNameFilter,
            this.locShortFilter,
            this.addressFilter,
            this.cityFilter,
            this.allowRecFilter,
            this.allowNegFilter,
            this.activeFilter,
            this.createdByFilter,
            this.maxCreateDateFilter,
            this.minCreateDateFilter,
            this.audtUserFilter,
            this.maxAudtDateFilter,
            this.minAudtDateFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
