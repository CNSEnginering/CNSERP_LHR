import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ICOPT4ServiceProxy  } from '../../shared/services/ic-opt4-service';
import { ICOPT4Dto} from '../../shared/dto/ic-opt4-dto';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditICOPT4ModalComponent } from './create-or-edit-icopT4-modal.component';
import { ViewICOPT4ModalComponent } from './view-icopT4-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { ICSetupsService } from '../../shared/services/ic-setup.service';

@Component({
    templateUrl: './icopT4.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ICOPT4Component extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditICOPT4Modal', { static: true }) createOrEditICOPT4Modal: CreateOrEditICOPT4ModalComponent;
    @ViewChild('viewICOPT4ModalComponent', { static: true }) viewICOPT4Modal: ViewICOPT4ModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxOptIDFilter : number;
		maxOptIDFilterEmpty : number;
		minOptIDFilter : number;
		minOptIDFilterEmpty : number;
    descpFilter = '';
    activeFilter = -1;
    audtUserFilter = '';
    maxAudtDateFilter : moment.Moment;
		minAudtDateFilter : moment.Moment;
    createdByFilter = '';
    maxCreateDateFilter : moment.Moment;
		minCreateDateFilter : moment.Moment;


        seg1IdFilter: '';
        segment2Filter = '';
        segment3Filter = '';
        maxAllowNegativeFilter : number;
            maxAllowNegativeFilterEmpty : number;
            minAllowNegativeFilter : number;
            minAllowNegativeFilterEmpty : number;
        maxErrSrNoFilter : number;
            maxErrSrNoFilterEmpty : number;
            minErrSrNoFilter : number;
            minErrSrNoFilterEmpty : number;
        maxCostingMethodFilter : number;
            maxCostingMethodFilterEmpty : number;
            minCostingMethodFilter : number;
            minCostingMethodFilterEmpty : number;
        prBookIDFilter = '';
        rtBookIDFilter = '';
        cnsBookIDFilter = '';
        slBookIDFilter = '';
        srBookIDFilter = '';
        trBookIDFilter = '';
        prdBookIDFilter = '';
        pyRecBookIDFilter = '';
        adjBookIDFilter = '';
        asmBookIDFilter = '';
        wsBookIDFilter = '';
        dsBookIDFilter = '';
        maxCurrentLocIDFilter : number;
            maxCurrentLocIDFilterEmpty : number;
            minCurrentLocIDFilter : number;
            minCurrentLocIDFilterEmpty : number;
        opt4Filter = '';
        opt5Filter = '';
        
        maxCreateadOnFilter : moment.Moment;
            minCreateadOnFilter : moment.Moment;

            optSetup: string;



    constructor(
        injector: Injector,
        private _icopT4ServiceProxy: ICOPT4ServiceProxy,
        private _icSetupsService: ICSetupsService,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    

    ngOnInit(): void {
        this._icSetupsService.getAll(
          this.filterText,
          this.seg1IdFilter,
          this.segment2Filter,
          this.segment3Filter,
          this.maxErrSrNoFilter == null ? this.maxErrSrNoFilterEmpty: this.maxErrSrNoFilter,
          this.minErrSrNoFilter == null ? this.minErrSrNoFilterEmpty: this.minErrSrNoFilter,
          this.maxCostingMethodFilter == null ? this.maxCostingMethodFilterEmpty: this.maxCostingMethodFilter,
          this.minCostingMethodFilter == null ? this.minCostingMethodFilterEmpty: this.minCostingMethodFilter,
          this.prBookIDFilter,
          this.rtBookIDFilter,
          this.cnsBookIDFilter,
          this.slBookIDFilter,
          this.srBookIDFilter,
          this.trBookIDFilter,
          this.prdBookIDFilter,
          this.pyRecBookIDFilter,
          this.adjBookIDFilter,
          this.asmBookIDFilter,
          this.wsBookIDFilter,
          this.dsBookIDFilter,
          this.maxCurrentLocIDFilter == null ? this.maxCurrentLocIDFilterEmpty: this.maxCurrentLocIDFilter,
          this.minCurrentLocIDFilter == null ? this.minCurrentLocIDFilterEmpty: this.minCurrentLocIDFilter,
          this.opt4Filter,
          this.opt5Filter,
          this.createdByFilter,
          this.maxCreateadOnFilter,
          this.minCreateadOnFilter,
          null,
          0,
          2147483646
      ).subscribe(result => {
          debugger;
          this.primengTableHelper.totalRecordsCount = result.totalCount;
          this.optSetup = result.items[0].opt4;
      });
      }

    getICOPT4(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._icopT4ServiceProxy.getAll(
            this.filterText,
            this.maxOptIDFilter == null ? this.maxOptIDFilterEmpty: this.maxOptIDFilter,
            this.minOptIDFilter == null ? this.minOptIDFilterEmpty: this.minOptIDFilter,
            this.descpFilter,
            this.activeFilter,
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

    createICOPT4(): void {
        this.createOrEditICOPT4Modal.show();
    }

    deleteICOPT4(icopT4: ICOPT4Dto): void {
        debugger;
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._icopT4ServiceProxy.delete(icopT4.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._icopT4ServiceProxy.GetICOPT4ToExcel(
        this.filterText,
            this.maxOptIDFilter == null ? this.maxOptIDFilterEmpty: this.maxOptIDFilter,
            this.minOptIDFilter == null ? this.minOptIDFilterEmpty: this.minOptIDFilter,
            this.descpFilter,
            this.activeFilter,
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
