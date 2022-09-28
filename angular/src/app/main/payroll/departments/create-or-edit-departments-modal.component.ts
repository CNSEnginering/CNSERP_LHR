import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditDepartmentDto } from '../shared/dto/department-dto';
import { DepartmentsServiceProxy } from '../shared/services/department.service';
import { FinanceLookupTableModalComponent } from "@app/finders/finance/finance-lookup-table-modal.component";
@Component({
    selector: 'createOrEditDepartmentModal',
    templateUrl: './create-or-edit-departments-modal.component.html'
})
export class CreateOrEditDepartmentModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild("FinanceLookupTableModal", { static: true })
    FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    target:string;
    active = false;
    saving = false;
    chartofControlAccountName:string;
    department: CreateOrEditDepartmentDto = new CreateOrEditDepartmentDto();
    
    constructor(
        injector: Injector,
        private _departmentsServiceProxy: DepartmentsServiceProxy
    ) {
        super(injector);
    }
    openCaderModal() {
        debugger;
        this.target = "Cader";
        //this.FinanceLookupTableModal.id = this.department.cader_Id;
        this.FinanceLookupTableModal.displayName = this.department.caderName;
        this.FinanceLookupTableModal.show(this.target, "true");
    }
    openSelectChartofControlModal() {
        debugger;
        this.target = "ChartOfAccount";
        this.FinanceLookupTableModal.id = this.department.expenseAcc;
        this.FinanceLookupTableModal.displayName = this.chartofControlAccountName;
        this.FinanceLookupTableModal.show(this.target, "true");
    }
    getNewFinanceModal() {
        switch (this.target) {
            case "ChartOfAccount":
                this.getNewChartofControlId();
                break;
            case "Cader":
                this.getNewCaderId();
                break;
            default:
                break;
        }
    }
    getNewChartofControlId() {
        this.department.expenseAcc = this.FinanceLookupTableModal.id;
        this.chartofControlAccountName = this.FinanceLookupTableModal.displayName;
    }
    getNewCaderId() {
        debugger
        this.department.cader_Id = parseInt(this.FinanceLookupTableModal.id);
        this.department.caderName = this.FinanceLookupTableModal.displayName;
    }
    show(departmentId?: number): void {
        debugger;

        if (!departmentId) {
            this.department = new CreateOrEditDepartmentDto();
            this.department.id = departmentId;

            this.department.active=1;
            this._departmentsServiceProxy.getMaxDeptId().subscribe(result => {
                this.department.deptID = result;
            });
            this.active = true;
            this.modal.show();
        } else {
            debugger
            this._departmentsServiceProxy.getDepartmentForEdit(departmentId).subscribe(result => {
                this.department = result.department;
                debugger
                this.chartofControlAccountName=result.department.expenseAccName;
                this.active = true;
                this.modal.show();
            });
        }
    }
    setChartofControlIdNull() {
        this.department.expenseAcc = null;
        this.chartofControlAccountName = "";
    }
    setCaderIdNull() {
        this.department.cader_Id = null;
        this.department.caderName = "";
    }
    save(): void {
        debugger;
        this.saving = true;

        this.department.audtDate=moment();
        this.department.audtUser=this.appSession.user.userName;

        if (!this.department.id) {
            this.department.createDate = moment();
            this.department.createdBy = this.appSession.user.userName;
        }

        this._departmentsServiceProxy.createOrEdit(this.department)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
                this.chartofControlAccountName=null;
            });
    }

    
    close(): void {
        debugger;
        this.active = false;
        this.modal.hide();
    }
}
