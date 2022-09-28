import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetAccountSubLedgerForViewDto, AccountSubLedgerDto, VendorActivityServiceProxy, VenderActivityDto, SubControlDetailControlDetailLookupTableDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LazyLoadEvent } from 'primeng/api';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import * as moment from 'moment';


@Component({
    selector: 'viewAccountSubLedgerModal',
    templateUrl: './view-accountSubLedger-modal.component.html'
})
export class ViewAccountSubLedgerModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator1', { static: true }) paginator: Paginator;

    active = false;
    saving = false;
    venderActivity: VenderActivityDto;
    item: GetAccountSubLedgerForViewDto;
    SubaccID : number;
    AccountID: string;

    lasPayment: number;
    outstandingBal: number;
    RunningTotalSum = 0;


    constructor(

        injector: Injector,
        private _vendorActivityServiceProxy: VendorActivityServiceProxy
    ) {
        super(injector);
        this.item = new GetAccountSubLedgerForViewDto();
        this.item.accountSubLedger = new AccountSubLedgerDto();
    }

    show(item: GetAccountSubLedgerForViewDto): void {


        this.item = item;
debugger;
       this.getVendorActivityList( this.item.accountSubLedger.id, this.item.accountSubLedger.accountID)
        this.active = true;
        this.modal.show();
    }

    getVendorActivityList(Subacc:number, acc:string ) {
        // if (this.primengTableHelper.shouldResetPaging(event)) {
        //     this.paginator.changePage(0);
        //     return;
        // }

        this.primengTableHelper.showLoadingIndicator();
        this._vendorActivityServiceProxy.getVendorActivityForView(
            Subacc,
            acc,
        ).subscribe(result => {
            debugger;
            //this.primengTableHelper.totalRecordsCount = result.totalCount;
            if (result.items["length"] !== 0) {
                this.lasPayment = result.items[0].lastPayment;
                this.outstandingBal = result.items[0].outstandingBalance;
            } else{
                this.lasPayment = 0;
                this.outstandingBal = 0;
            }

            result.items.forEach(element => {
                this.RunningTotalSum = this.RunningTotalSum + element.venderActivity.runningTotal
            });

            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }
   

    close(): void {
        this.RunningTotalSum = 0;
        this.active = false;
        this.modal.hide();
    }
}
