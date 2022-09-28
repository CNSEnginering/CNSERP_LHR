import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditRecurringVoucherDto } from '../shared/dto/recurringVouchers-dto';
import { RecurringVouchersServiceProxy } from '../shared/services/recurringVouchers.service';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';


@Component({
    selector: 'createOrEditRecurringVoucherModal',
    templateUrl: './create-or-edit-recurringVoucher-modal.component.html'
})
export class CreateOrEditRecurringVoucherModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    recurringVoucher: CreateOrEditRecurringVoucherDto = new CreateOrEditRecurringVoucherDto();

    voucherDate: Date;
    flag: boolean = false;
    target: any;

    constructor(
        injector: Injector,
        private _recurringVouchersServiceProxy: RecurringVouchersServiceProxy
    ) {
        super(injector);
    }

    show(recurringVoucherId?: number): void {
        this.voucherDate = null;

        if (!recurringVoucherId) {
            this.recurringVoucher = new CreateOrEditRecurringVoucherDto();
            this.recurringVoucher.id = recurringVoucherId;
            this.recurringVoucher.active = true;
            this.flag = false;
            
            this._recurringVouchersServiceProxy.getMaxDocId().subscribe(result => {
                this.recurringVoucher.docNo = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            this._recurringVouchersServiceProxy.getRecurringVoucherForEdit(recurringVoucherId).subscribe(result => {
                this.recurringVoucher = result.recurringVoucher;

                this.voucherDate = moment(this.recurringVoucher.voucherDate).toDate();

                this.active = true;
                this.modal.show();
            });
        }

    }

    save(): void {
        this.saving = true;

        this.recurringVoucher.audtDate = moment();
        this.recurringVoucher.audtUser = this.appSession.user.userName;

        if (!this.recurringVoucher.id) {
            this.recurringVoucher.createDate = moment();
            this.recurringVoucher.createdBy = this.appSession.user.userName;
        }
        this.recurringVoucher.voucherDate = moment(this.voucherDate);
        this._recurringVouchersServiceProxy.createOrEdit(this.recurringVoucher)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }

    getNewFinanceModal() {
        debugger;
        switch (this.target) {
            case "VoucherType":
                this.getNewVoucherType();
                break;
            case "VoucherNo":
                this.getNewVoucherNo();
                break;
            default:
                break;
        }
    }

    //=====================Voucher Type================

    openVoucherTypeModal() {
        debugger;
        this.target = "VoucherType";
        this.FinanceLookupTableModal.id = null;
        this.FinanceLookupTableModal.displayName = "";
        this.FinanceLookupTableModal.show("VoucherType");
    }

    setVoucherTypeIdNull() {
        debugger;
        this.recurringVoucher.bookID = null;
        this.setVoucherNoNull();
        this.flag = false;
    }

    getNewVoucherType() {
        debugger;
        this.recurringVoucher.bookID = this.FinanceLookupTableModal.id;
        this.flag = true;
    }

    //=====================Voucher No================

    openVoucherNoModal() {
        debugger;
        this.target = "VoucherNo";
        this.FinanceLookupTableModal.id = null;
        this.FinanceLookupTableModal.displayName = "";
        this.FinanceLookupTableModal.show("VoucherNo", this.recurringVoucher.bookID);
    }

    setVoucherNoNull() {
        debugger;
        this.recurringVoucher.voucherNo = null;
        this.voucherDate = null;
        this.recurringVoucher.voucherMonth = null;
        this.recurringVoucher.configID = null;

    }

    getNewVoucherNo() {
        debugger;
        this.recurringVoucher.voucherNo = Number(this.FinanceLookupTableModal.id);
        this.voucherDate = this.FinanceLookupTableModal.docDate;
        this.recurringVoucher.voucherMonth = this.FinanceLookupTableModal.docMonth;
        this.recurringVoucher.configID = this.FinanceLookupTableModal.configID;
    }


}
