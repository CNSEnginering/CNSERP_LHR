import { Component, OnInit, ViewChild, Injector, Output, EventEmitter } from "@angular/core";
import { CreateOrEditLoansTypeDto } from "../shared/dto/loanTypes-dto";
import { ModalDirective } from "ngx-bootstrap";
import { LoansTypeService } from "../shared/services/loansType.service";
import { finalize } from "rxjs/operators";
import { AppComponentBase } from "@shared/common/app-component-base";

@Component({
    selector: "createOrEditEmployeeLoansTypeModal",
    templateUrl: "./create-or-edit-employee-loans-type-modal.component.html",
})
export class CreateOrEditEmployeeLoansTypeModalComponent extends AppComponentBase {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    active = false;
    saving = false;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    loansType: CreateOrEditLoansTypeDto = new CreateOrEditLoansTypeDto();

    constructor(
        injector: Injector,
        private _loansTypeService: LoansTypeService
    ) {
        super(injector);
    }

    show(id?: number): void {
        debugger;

        if (!id) {
            this.loansType = new CreateOrEditLoansTypeDto();
            this.loansType.LoanTypeID = id;

            this.loansType.LoanTypeName = "";
            this._loansTypeService
                .getMaxId()
                .subscribe((res) => (this.loansType.LoanTypeID = res["result"]));
            this.active = true;
            this.modal.show();
        } else {
            this._loansTypeService
                .getEmployeeLoansTypeForEdit(id)
                .subscribe((result) => {
                    this.loansType = result.loansType;
                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        debugger;
        this.saving = true;
        this._loansTypeService
            .createOrEdit(this.loansType)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l("SavedSuccessfully"));
                this.modalSave.emit(null);
                this.close();
            });
    }

    close(): void {
        debugger;
        this.active = false;
        this.modal.hide();
    }
}
