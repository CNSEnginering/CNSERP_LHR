import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { finalize } from "rxjs/operators";
import {
    CurrencyRatesServiceProxy,
    CreateOrEditCurrencyRateDto,
} from "@shared/service-proxies/service-proxies";
import { AppComponentBase } from "@shared/common/app-component-base";
import * as moment from "moment";
import { CurrencyRateCompanyProfileLookupTableModalComponent } from "./currencyRate-companyProfile-lookup-table-modal.component";

@Component({
    selector: "createOrEditCurrencyRateModal",
    templateUrl: "./create-or-edit-currencyRate-modal.component.html",
})
export class CreateOrEditCurrencyRateModalComponent extends AppComponentBase {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild("currencyRateCompanyProfileLookupTableModal", { static: true })
    currencyRateCompanyProfileLookupTableModal: CurrencyRateCompanyProfileLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    isCodeLength = false;
    active = false;
    saving = false;
mode="add";
    currencyRate: CreateOrEditCurrencyRateDto = new CreateOrEditCurrencyRateDto();

    companyProfileCompanyName = "";

    constructor(
        injector: Injector,
        private _currencyRatesServiceProxy: CurrencyRatesServiceProxy
    ) {
        super(injector);
    }

    show(currencyRateId?: string): void {
        if (!currencyRateId) {
            this.currencyRate = new CreateOrEditCurrencyRateDto();
            this.currencyRate.id = currencyRateId;
            this.currencyRate.audtdate = moment().startOf("day");
            this.currencyRate.ratedate = moment().startOf("day");
            this.companyProfileCompanyName = "";

            this.active = true;
            this.modal.show();
        } else {
           this.mode="edit";
            this.primengTableHelper.showLoadingIndicator();

            this._currencyRatesServiceProxy
                .getCurrencyRateForEdit(currencyRateId)
                .pipe(
                    finalize(() =>
                        this.primengTableHelper.hideLoadingIndicator()
                    )
                )
                .subscribe((result) => {
                    debugger;
                    console.log(result.currencyHistory);
                    this.currencyRate = result.currencyRate;

                    this.currencyRate.symbol
                        ? this.currencyRate.symbol.replace(/^\s+|\s+$/gm, "")
                        : "";

                    this.companyProfileCompanyName =
                        result.companyProfileCompanyName;
                    this.primengTableHelper.totalRecordsCount =
                        result.currencyHistory.length;
                    this.primengTableHelper.records = result.currencyHistory;

                    this.isCodeLength = true;
                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        if (this.validateForm()) {
            this.saving = true;

            this._currencyRatesServiceProxy
                .createOrEdit(this.currencyRate)
                .pipe(
                    finalize(() => {
                        this.saving = false;
                        this.isCodeLength = false;
                    })
                )
                .subscribe(() => {
                    this.notify.info(this.l("SavedSuccessfully"));
                    this.close();
                    this.modalSave.emit(null);
                });
        }
    }

    openSelectCompanyProfileModal() {
        //this.currencyRateCompanyProfileLookupTableModal.id = this.currencyRate.cmpid;
        this.currencyRateCompanyProfileLookupTableModal.displayName = this.companyProfileCompanyName;
        this.currencyRateCompanyProfileLookupTableModal.show();
    }

    setCompanyProfileIdNull() {
        // this.currencyRate.cmpid = null;
        this.companyProfileCompanyName = "";
    }

    getNewCompanyProfileId() {
        // this.currencyRate.cmpid = this.currencyRateCompanyProfileLookupTableModal.id;
        this.companyProfileCompanyName = this.currencyRateCompanyProfileLookupTableModal.displayName;
    }

    close(): void {
        this.isCodeLength = false;
        this.active = false;
        this.modal.hide();
    }

    codeLength(): void {
        if (this.currencyRate.id.length < 3) {
            this.isCodeLength = false;
            this.message.warn("", "Currency code is not valid");
        } else {
            this.isCodeLength = true;
        }
    }

    validateForm(): boolean {
        if (!this.currencyRate.curname) {
            this.message.warn("", "Please Enter Description");
            return false;
        } else if (!this.currencyRate.symbol) {
            this.message.warn("", "Please Select Currency Symbol");
            return false;
        } else if (!this.currencyRate.currate) {
            this.message.warn("", "Please Enter Rate");
            return false;
        }
        return true;
    }
}
