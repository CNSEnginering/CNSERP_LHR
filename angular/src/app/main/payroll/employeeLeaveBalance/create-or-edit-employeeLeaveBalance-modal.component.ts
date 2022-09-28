import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditemployeeLeaveBalanceDto } from '../shared/dto/employeeLeaveBalance-dto';
import { EmployeeLeaveBalanceServiceProxy } from '../shared/services/employeeLeaveBalance.service';
import { PayRollLookupTableModalComponent } from '@app/finders/payRoll/payRoll-lookup-table-modal.component';

@Component({
    selector: 'createOrEditEmployeeLeaveBalanceModal',
    templateUrl: './create-or-edit-employeeLeaveBalance-modal.component.html'
})
export class CreateOrEditEmployeeLeaveBalanceModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('PayRollLookupTableModal', { static: true }) PayRollLookupTableModal: PayRollLookupTableModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    target: string;
    employeeLeavesTotal: CreateOrEditemployeeLeaveBalanceDto = new CreateOrEditemployeeLeaveBalanceDto();

    createDate: Date;
    audtDate: Date;
    employeeName: string;


    constructor(
        injector: Injector,
        private _employeeLeaveBalanceServiceProxy: EmployeeLeaveBalanceServiceProxy
    ) {
        super(injector);
    }

    totalLeaves() {
        this.employeeLeavesTotal.leaves = this.employeeLeavesTotal.casual +
            this.employeeLeavesTotal.sick +
            this.employeeLeavesTotal.annual;
    }
    openEmployeeModal() {
        this.target = "Employee";
        this.PayRollLookupTableModal.id = String(this.employeeLeavesTotal.employeeID);
        this.PayRollLookupTableModal.displayName = this.employeeName;
        this.PayRollLookupTableModal.show(this.target);
    }

    getNewPayRollModal() {
        this._employeeLeaveBalanceServiceProxy.GetEmpLeavesBalance(+this.PayRollLookupTableModal.id).subscribe(
            data => {
                debugger
                if (data["result"] == false) {
                    this.employeeLeavesTotal.employeeID = Number(this.PayRollLookupTableModal.id);
                    this.employeeName = this.PayRollLookupTableModal.displayName;
                }
                else {
                    this.employeeLeavesTotal.employeeID = undefined;
                    this.employeeName = "";
                    this.notify.info("Employee leaves balance already has been added against this employee");
                }
            }
        )
    }
    setEmployeeNull() {
        this.employeeLeavesTotal.employeeID = null;
        this.employeeName = "";
    }

    show(employeeLeavesTotalId?: number): void {
        if (!employeeLeavesTotalId) {
            this.employeeLeavesTotal = new CreateOrEditemployeeLeaveBalanceDto();
            this.employeeName = "";
            this.employeeLeavesTotal.id = employeeLeavesTotalId;
            this.employeeLeavesTotal.casual = 1;
            this.employeeLeavesTotal.annual = 1;
            this.employeeLeavesTotal.sick = 1;
            this.employeeLeavesTotal.leaves = 3;
            this.active = true;
            this.modal.show();
        } else {
            this._employeeLeaveBalanceServiceProxy.getEmployeeLeavesTotalForEdit(employeeLeavesTotalId).subscribe(result => {
                this.employeeLeavesTotal = result.employeeLeavesTotal;
                this.employeeName = result.employeeLeavesTotal.employeeName;
                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this.employeeLeavesTotal.audtDate = moment();
        this.employeeLeavesTotal.audtUser = this.appSession.user.userName;

        if (!this.employeeLeavesTotal.id) {
            this.employeeLeavesTotal.createDate = moment();
            this.employeeLeavesTotal.createdBy = this.appSession.user.userName;
        }

        this._employeeLeaveBalanceServiceProxy.createOrEdit(this.employeeLeavesTotal)
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
}
