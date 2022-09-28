import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditAssetRegistrationModalComponent } from './create-or-edit-assetRegistration-modal.component';
import { ViewAssetRegistrationModalComponent } from './view-assetRegistration-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { AssetRegistrationsServiceProxy, AssetRegistrationDto } from '../shared/services/assetRegistration.service';
import { GetDataService } from '@app/main/supplyChain/inventory/shared/services/get-data.service';
import { ICSetupsService } from '../shared/services/ic-setup.service';
@Component({
  templateUrl: './assetRegistration.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class AssetRegistrationComponent extends AppComponentBase {

  @ViewChild('createOrEditAssetRegistrationModal', { static: true }) createOrEditAssetRegistrationModal: CreateOrEditAssetRegistrationModalComponent;
  @ViewChild('viewAssetRegistrationModalComponent', { static: true }) viewAssetRegistrationModal: ViewAssetRegistrationModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxAssetIDFilter: number;
  maxAssetIDFilterEmpty: number;
  minAssetIDFilter: number;
  minAssetIDFilterEmpty: number;
  fmtAssetIDFilter = '';
  assetNameFilter = '';
  itemIDFilter = '';
  maxLocIDFilter: number;
  maxLocIDFilterEmpty: number;
  minLocIDFilter: number;
  minLocIDFilterEmpty: number;
  maxRegDateFilter: moment.Moment;
  minRegDateFilter: moment.Moment;
  maxPurchaseDateFilter: moment.Moment;
  minPurchaseDateFilter: moment.Moment;
  maxExpiryDateFilter: moment.Moment;
  minExpiryDateFilter: moment.Moment;
  warrantyFilter = -1;
  AssetTypeFilter: number;
  AssetTypeFilterEmpty: number;
  maxDepRateFilter: number;
  maxDepRateFilterEmpty: number;
  minDepRateFilter: number;
  minDepRateFilterEmpty: number;
  DepMethodFilter: number;
  DepMethodFilterEmpty: number;

  serialNumberFilter = '';
  maxPurchasePriceFilter: number;
  maxPurchasePriceFilterEmpty: number;
  minPurchasePriceFilter: number;
  minPurchasePriceFilterEmpty: number;
  narrationFilter = '';
  accAssetFilter = '';
  accDeprFilter = '';
  accExpFilter = '';
  maxDepStartDateFilter: moment.Moment;
  minDepStartDateFilter: moment.Moment;
  maxAssetLifeFilter: number;
  maxAssetLifeFilterEmpty: number;
  minAssetLifeFilter: number;
  minAssetLifeFilterEmpty: number;
  maxBookValueFilter: number;
  maxBookValueFilterEmpty: number;
  minBookValueFilter: number;
  minBookValueFilterEmpty: number;
  maxLastDepAmountFilter: number;
  maxLastDepAmountFilterEmpty: number;
  minLastDepAmountFilter: number;
  minLastDepAmountFilterEmpty: number;
  maxLastDepDateFilter: moment.Moment;
  minLastDepDateFilter: moment.Moment;
  disolvedFilter = -1;
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
  locations:any;
    users:any;




  constructor(
    injector: Injector,
    private _assetRegistrationServiceProxy: AssetRegistrationsServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService,
    private _getDataService: GetDataService,
    private _icSetupsService: ICSetupsService,
  ) {
    super(injector);
    this._getDataService.getList("ICLocations").subscribe(result => {
          this.locations = result;
        });
        this._getDataService.getList("Users").subscribe(result => {
          this.users = result;
        });
  }

  getAssetRegistration(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();
    this._assetRegistrationServiceProxy.getAll(
      this.filterText,
      this.maxAssetIDFilter == null ? this.maxAssetIDFilterEmpty : this.maxAssetIDFilter,
      this.minAssetIDFilter == null ? this.minAssetIDFilterEmpty : this.minAssetIDFilter,
      this.fmtAssetIDFilter,
      this.assetNameFilter,
      this.itemIDFilter,
      this.maxLocIDFilter == null ? this.maxLocIDFilterEmpty : this.maxLocIDFilter,
      this.minLocIDFilter == null ? this.minLocIDFilterEmpty : this.minLocIDFilter,
      this.maxRegDateFilter,
      this.minRegDateFilter,
      this.maxPurchaseDateFilter,
      this.minPurchaseDateFilter,
      this.maxExpiryDateFilter,
      this.minExpiryDateFilter,
      this.warrantyFilter,
      this.AssetTypeFilter == null ? this.AssetTypeFilterEmpty : this.AssetTypeFilter,
      this.maxDepRateFilter == null ? this.maxDepRateFilterEmpty : this.maxDepRateFilter,
      this.minDepRateFilter == null ? this.minDepRateFilterEmpty : this.minDepRateFilter,
      this.DepMethodFilter == null ? this.DepMethodFilterEmpty : this.DepMethodFilter,
      this.serialNumberFilter,
      this.maxPurchasePriceFilter == null ? this.maxPurchasePriceFilterEmpty : this.maxPurchasePriceFilter,
      this.minPurchasePriceFilter == null ? this.minPurchasePriceFilterEmpty : this.minPurchasePriceFilter,
      this.narrationFilter,
      this.accAssetFilter,
      this.accDeprFilter,
      this.accExpFilter,
      this.maxDepStartDateFilter,
      this.minDepStartDateFilter,
      this.maxAssetLifeFilter == null ? this.maxAssetLifeFilterEmpty : this.maxAssetLifeFilter,
      this.minAssetLifeFilter == null ? this.minAssetLifeFilterEmpty : this.minAssetLifeFilter,
      this.maxBookValueFilter == null ? this.maxBookValueFilterEmpty : this.maxBookValueFilter,
      this.minBookValueFilter == null ? this.minBookValueFilterEmpty : this.minBookValueFilter,
      this.maxLastDepAmountFilter == null ? this.maxLastDepAmountFilterEmpty : this.maxLastDepAmountFilter,
      this.minLastDepAmountFilter == null ? this.minLastDepAmountFilterEmpty : this.minLastDepAmountFilter,
      this.maxLastDepDateFilter,
      this.minLastDepDateFilter,
      this.disolvedFilter,
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
    this.paginator.changePage(this.paginator.getPage());
  }

  createAssetRegistration(): void {
    this.GetSetUpDetail();
    this.createOrEditAssetRegistrationModal.show();
  }
  GetSetUpDetail(): void {

    // this._icSetupsService.getAll().subscribe(
    //   result => this.createOrEditAssetRegistrationModal.SetDefaultRecord(result["items"][0])
    // )

    this._getDataService.GetSetUpDetail().subscribe(result => {

      this.createOrEditAssetRegistrationModal.SetDefaultRecord(result);
    });
  }
  deleteAssetRegistration(assetRegistration: AssetRegistrationDto): void {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._assetRegistrationServiceProxy.delete(assetRegistration.id)
            .subscribe(() => {
              this.reloadPage();
              this.notify.success(this.l('SuccessfullyDeleted'));
            });
        }
      }
    );
  }

  exportToExcel(): void {
    this._assetRegistrationServiceProxy.getAssetRegistrationsToExcel(
      this.filterText,
      this.maxAssetIDFilter == null ? this.maxAssetIDFilterEmpty : this.maxAssetIDFilter,
      this.minAssetIDFilter == null ? this.minAssetIDFilterEmpty : this.minAssetIDFilter,
      this.fmtAssetIDFilter,
      this.assetNameFilter,
      this.itemIDFilter,
      this.maxLocIDFilter == null ? this.maxLocIDFilterEmpty : this.maxLocIDFilter,
      this.minLocIDFilter == null ? this.minLocIDFilterEmpty : this.minLocIDFilter,
      this.maxRegDateFilter,
      this.minRegDateFilter,
      this.maxPurchaseDateFilter,
      this.minPurchaseDateFilter,
      this.maxExpiryDateFilter,
      this.minExpiryDateFilter,
      this.warrantyFilter,
      this.AssetTypeFilter == null ? null : this.AssetTypeFilter,
      this.maxDepRateFilter == null ? this.maxDepRateFilterEmpty : this.maxDepRateFilter,
      this.minDepRateFilter == null ? this.minDepRateFilterEmpty : this.minDepRateFilter,
      this.DepMethodFilter == null ? null : this.DepMethodFilter,
      this.serialNumberFilter,
      this.maxPurchasePriceFilter == null ? this.maxPurchasePriceFilterEmpty : this.maxPurchasePriceFilter,
      this.minPurchasePriceFilter == null ? this.minPurchasePriceFilterEmpty : this.minPurchasePriceFilter,
      this.narrationFilter,
      this.accAssetFilter,
      this.accDeprFilter,
      this.accExpFilter,
      this.maxDepStartDateFilter,
      this.minDepStartDateFilter,
      this.maxAssetLifeFilter == null ? this.maxAssetLifeFilterEmpty : this.maxAssetLifeFilter,
      this.minAssetLifeFilter == null ? this.minAssetLifeFilterEmpty : this.minAssetLifeFilter,
      this.maxBookValueFilter == null ? this.maxBookValueFilterEmpty : this.maxBookValueFilter,
      this.minBookValueFilter == null ? this.minBookValueFilterEmpty : this.minBookValueFilter,
      this.maxLastDepAmountFilter == null ? this.maxLastDepAmountFilterEmpty : this.maxLastDepAmountFilter,
      this.minLastDepAmountFilter == null ? this.minLastDepAmountFilterEmpty : this.minLastDepAmountFilter,
      this.maxLastDepDateFilter,
      this.minLastDepDateFilter,
      this.disolvedFilter,
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
