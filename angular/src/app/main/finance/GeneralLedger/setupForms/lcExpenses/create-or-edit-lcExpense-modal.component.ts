import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { finalize } from "rxjs/operators";
import { AppComponentBase } from "@shared/common/app-component-base";
import * as moment from "moment";
import { LCExpensesServiceProxy } from "@app/main/finance/shared/services/lcExpenses.service";
import { CreateOrEditLCExpensesDto } from "@app/main/finance/shared/dto/lcExpenses-dto";

@Component({
    selector: "createOrEditLCExpenseModal",
    templateUrl: "./create-or-edit-lcExpense-modal.component.html"
})
export class CreateOrEditLCExpenseModalComponent extends AppComponentBase {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    lcExpenses: CreateOrEditLCExpensesDto = new CreateOrEditLCExpensesDto();

    createDate: Date;
    audtDate: Date;

    constructor(
        injector: Injector,
        private _lcExpenseServiceProxy: LCExpensesServiceProxy
    ) {
        super(injector);
    }

    show(lcExpenseId?: number): void {
        debugger;
        if (!lcExpenseId) {
            debugger;
            this.lcExpenses = new CreateOrEditLCExpensesDto();
            this.lcExpenses.id = lcExpenseId;
            this.lcExpenses.active = true;
            this._lcExpenseServiceProxy
                .getMaxLCExpenseId()
                .subscribe(result => {
                    this.lcExpenses.expID = result;
                });

            this.active = true;
            this.modal.show();
        } else {
            debugger;
            this._lcExpenseServiceProxy
                .getLCExpenseForEdit(lcExpenseId)
                .subscribe(result => {
                    debugger;
                    this.lcExpenses = result.lcExpenses;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;
        debugger;

        this.lcExpenses.auditDate = moment();
        this.lcExpenses.auditUser = this.appSession.user.userName;

        if (!this.lcExpenses.id) {
            this.lcExpenses.createDate = moment();
            this.lcExpenses.createdBy = this.appSession.user.userName;
        }

        this._lcExpenseServiceProxy
            .createOrEdit(this.lcExpenses)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                debugger;
                this.notify.info(this.l("SavedSuccessfully"));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
