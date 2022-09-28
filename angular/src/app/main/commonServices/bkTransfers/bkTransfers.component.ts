import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BkTransfersServiceProxy, BkTransferDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditBkTransferModalComponent } from './create-or-edit-bkTransfer-modal.component';
import { ViewBkTransferModalComponent } from './view-bkTransfer-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './bkTransfers.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class BkTransfersComponent extends AppComponentBase {

    @ViewChild('createOrEditBkTransferModal', { static: true }) createOrEditBkTransferModal: CreateOrEditBkTransferModalComponent;
    @ViewChild('viewBkTransferModalComponent', { static: true }) viewBkTransferModal: ViewBkTransferModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    cmpidFilter = '';
    maxDOCIDFilter : number;
		maxDOCIDFilterEmpty : number;
		minDOCIDFilter : number;
		minDOCIDFilterEmpty : number;
    maxDOCDATEFilter : moment.Moment;
		minDOCDATEFilter : moment.Moment;
    maxTRANSFERDATEFilter : moment.Moment;
		minTRANSFERDATEFilter : moment.Moment;
    descriptionFilter = '';
    maxFROMBANKIDFilter : number;
		maxFROMBANKIDFilterEmpty : number;
		minFROMBANKIDFilter : number;
		minFROMBANKIDFilterEmpty : number;
    maxFROMCONFIGIDFilter : number;
		maxFROMCONFIGIDFilterEmpty : number;
		minFROMCONFIGIDFilter : number;
		minFROMCONFIGIDFilterEmpty : number;
    maxTOBANKIDFilter : number;
		maxTOBANKIDFilterEmpty : number;
		minTOBANKIDFilter : number;
		minTOBANKIDFilterEmpty : number;
    maxTOCONFIGIDFilter : number;
		maxTOCONFIGIDFilterEmpty : number;
		minTOCONFIGIDFilter : number;
		minTOCONFIGIDFilterEmpty : number;
    maxAVAILLIMITFilter : number;
		maxAVAILLIMITFilterEmpty : number;
		minAVAILLIMITFilter : number;
		minAVAILLIMITFilterEmpty : number;
    maxTRANSFERAMOUNTFilter : number;
		maxTRANSFERAMOUNTFilterEmpty : number;
		minTRANSFERAMOUNTFilter : number;
		minTRANSFERAMOUNTFilterEmpty : number;
    maxAUDTDATEFilter : moment.Moment;
		minAUDTDATEFilter : moment.Moment;
    audtuserFilter = '';
        bankBANKNAMEFilter = '';




    constructor(
        injector: Injector, 
        private _bkTransfersServiceProxy: BkTransfersServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getBkTransfers(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._bkTransfersServiceProxy.getAll(
            this.filterText,
            this.cmpidFilter,
            this.maxDOCIDFilter == null ? this.maxDOCIDFilterEmpty: this.maxDOCIDFilter,
            this.minDOCIDFilter == null ? this.minDOCIDFilterEmpty: this.minDOCIDFilter,
            this.maxDOCDATEFilter,
            this.minDOCDATEFilter,
            this.maxTRANSFERDATEFilter,
            this.minTRANSFERDATEFilter,
            this.descriptionFilter,
            this.maxFROMBANKIDFilter == null ? this.maxFROMBANKIDFilterEmpty: this.maxFROMBANKIDFilter,
            this.minFROMBANKIDFilter == null ? this.minFROMBANKIDFilterEmpty: this.minFROMBANKIDFilter,
            this.maxFROMCONFIGIDFilter == null ? this.maxFROMCONFIGIDFilterEmpty: this.maxFROMCONFIGIDFilter,
            this.minFROMCONFIGIDFilter == null ? this.minFROMCONFIGIDFilterEmpty: this.minFROMCONFIGIDFilter,
            this.maxTOBANKIDFilter == null ? this.maxTOBANKIDFilterEmpty: this.maxTOBANKIDFilter,
            this.minTOBANKIDFilter == null ? this.minTOBANKIDFilterEmpty: this.minTOBANKIDFilter,
            this.maxTOCONFIGIDFilter == null ? this.maxTOCONFIGIDFilterEmpty: this.maxTOCONFIGIDFilter,
            this.minTOCONFIGIDFilter == null ? this.minTOCONFIGIDFilterEmpty: this.minTOCONFIGIDFilter,
            this.maxAVAILLIMITFilter == null ? this.maxAVAILLIMITFilterEmpty: this.maxAVAILLIMITFilter,
            this.minAVAILLIMITFilter == null ? this.minAVAILLIMITFilterEmpty: this.minAVAILLIMITFilter,
            this.maxTRANSFERAMOUNTFilter == null ? this.maxTRANSFERAMOUNTFilterEmpty: this.maxTRANSFERAMOUNTFilter,
            this.minTRANSFERAMOUNTFilter == null ? this.minTRANSFERAMOUNTFilterEmpty: this.minTRANSFERAMOUNTFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.bankBANKNAMEFilter,
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

    createBkTransfer(): void {
        this.createOrEditBkTransferModal.show();
    }

    deleteBkTransfer(bkTransfer: BkTransferDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._bkTransfersServiceProxy.delete(bkTransfer.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._bkTransfersServiceProxy.getBkTransfersToExcel(
        this.filterText,
            this.cmpidFilter,
            this.maxDOCIDFilter == null ? this.maxDOCIDFilterEmpty: this.maxDOCIDFilter,
            this.minDOCIDFilter == null ? this.minDOCIDFilterEmpty: this.minDOCIDFilter,
            this.maxDOCDATEFilter,
            this.minDOCDATEFilter,
            this.maxTRANSFERDATEFilter,
            this.minTRANSFERDATEFilter,
            this.descriptionFilter,
            this.maxFROMBANKIDFilter == null ? this.maxFROMBANKIDFilterEmpty: this.maxFROMBANKIDFilter,
            this.minFROMBANKIDFilter == null ? this.minFROMBANKIDFilterEmpty: this.minFROMBANKIDFilter,
            this.maxFROMCONFIGIDFilter == null ? this.maxFROMCONFIGIDFilterEmpty: this.maxFROMCONFIGIDFilter,
            this.minFROMCONFIGIDFilter == null ? this.minFROMCONFIGIDFilterEmpty: this.minFROMCONFIGIDFilter,
            this.maxTOBANKIDFilter == null ? this.maxTOBANKIDFilterEmpty: this.maxTOBANKIDFilter,
            this.minTOBANKIDFilter == null ? this.minTOBANKIDFilterEmpty: this.minTOBANKIDFilter,
            this.maxTOCONFIGIDFilter == null ? this.maxTOCONFIGIDFilterEmpty: this.maxTOCONFIGIDFilter,
            this.minTOCONFIGIDFilter == null ? this.minTOCONFIGIDFilterEmpty: this.minTOCONFIGIDFilter,
            this.maxAVAILLIMITFilter == null ? this.maxAVAILLIMITFilterEmpty: this.maxAVAILLIMITFilter,
            this.minAVAILLIMITFilter == null ? this.minAVAILLIMITFilterEmpty: this.minAVAILLIMITFilter,
            this.maxTRANSFERAMOUNTFilter == null ? this.maxTRANSFERAMOUNTFilterEmpty: this.maxTRANSFERAMOUNTFilter,
            this.minTRANSFERAMOUNTFilter == null ? this.minTRANSFERAMOUNTFilterEmpty: this.minTRANSFERAMOUNTFilter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.bankBANKNAMEFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
