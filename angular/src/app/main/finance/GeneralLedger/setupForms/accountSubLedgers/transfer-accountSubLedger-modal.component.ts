import { Component, ViewChild, Injector, Output, EventEmitter, Input } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { AccountSubLedgersServiceProxy } from '@shared/service-proxies/service-proxies';


@Component({
    selector: 'transferAccountSubLedgerModal',
    templateUrl: './transfer-accountSubLedger-modal.component.html'
})
export class TransferAccountSubLedgerModalComponent extends AppComponentBase {
    @Input() mode: string;
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    processing1: boolean;

    target: string;
    toOrFromCheck: string;

    fromAccountID: string;
    toAccountID: string;
    fromChartofControlAccountName: string;
    toChartofControlAccountName: string;

    constructor(
        injector: Injector,
        private _accountSubLedgersServiceProxy: AccountSubLedgersServiceProxy
    ) {
        super(injector);
    }

    show(): void {
        debugger;
        this.fromAccountID = null;
        this.fromChartofControlAccountName = '';
        this.toAccountID = null;
        this.toChartofControlAccountName = '';

        this.active = true;
        this.modal.show();

    }

    transfer(): void {
        debugger;
        this.saving = true;

        this._accountSubLedgersServiceProxy.transferSubledgers(this.fromAccountID, this.toAccountID).subscribe(result => {
            debugger
            if (result == "done") {
                this.saving = false;
                this.notify.info(this.l('Transferred Successfully'));
                this.close();
                this.modalSave.emit(null);
            } else {
                this.saving = false;
                this.notify.error(this.l('Transfer Failed'));
                this.close();
                this.modalSave.emit(null);
            }
        });

    }


    close(): void {
        debugger;
        this.active = false;
        this.modal.hide();
    }

    ////////////////////////////////////////////////

    openSelectChartofControlModal(input) {
        debugger;
        this.toOrFromCheck = input;
        this.target = "ChartOfAccount"
        this.FinanceLookupTableModal.id = this.fromAccountID;
        this.FinanceLookupTableModal.displayName = this.fromChartofControlAccountName;
        if (this.mode == "customerMaster") {
            this.FinanceLookupTableModal.show(this.target, "", "Customer");
        }
        else if (this.mode == "vendorMaster") {
            this.FinanceLookupTableModal.show(this.target, "", "Vendor");
        }
        else
            this.FinanceLookupTableModal.show(this.target,"true");

    }

    setChartofControlIdNull(input) {
        if (input == "from") {
            this.fromAccountID = null;
            this.fromChartofControlAccountName = '';
        }
        else if (input == "to") {
            this.toAccountID = null;
            this.toChartofControlAccountName = '';
        }
    }

    getNewFinanceModal() {
        debugger;
        switch (this.target) {
            case "ChartOfAccount":
                this.getNewChartofControlId();
                break;

            default:
                break;
        }
    }

    getNewChartofControlId() {
        if (this.toOrFromCheck == "from") {
            this.fromAccountID = this.FinanceLookupTableModal.id;
            this.fromChartofControlAccountName = this.FinanceLookupTableModal.displayName;
        }
        else if (this.toOrFromCheck == "to") {
            this.toAccountID = this.FinanceLookupTableModal.id;
            this.toChartofControlAccountName = this.FinanceLookupTableModal.displayName;
        }
    }


}
