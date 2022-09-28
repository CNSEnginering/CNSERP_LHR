import { Component, ViewChild, Injector, Output, EventEmitter, DebugElement } from '@angular/core';
import { ModalDirective, TypeaheadOptions } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { SubCostCenterDto } from '../shared/dto/subCostCenter-dto';
import { SubCostCenterService } from '../shared/services/subCostCenter.service';
import { CostCenterLookupTableModalComponent } from '../FinderModals/costCenter-lookup-table-modal.component';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';



@Component({
    selector: 'subCostCenterModal',
    templateUrl: './create-or-edit-subCostCenter-modal.component.html'
})
export class CreateOrEditSubCostCenterModalComponent extends AppComponentBase {
    @ViewChild('financeLookupTableModal', { static: true }) financeLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('CostCenterLookupTableModal', { static: true }) CostCenterLookupTableModal: CostCenterLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    edit: boolean = false;
    active = false;
    saving = false;
    validForm: boolean = false;
    subCostCenterDto: SubCostCenterDto = new SubCostCenterDto();
    type: string = '';
    constructor(
        injector: Injector,
        private _subCostCenterService: SubCostCenterService
    ) {
        super(injector);
    }

    show(id?: number): void {
        debugger
        this.active = true;
        if (!id) {
            this.edit = false;
            this.subCostCenterDto.id = undefined;
            this.subCostCenterDto.ccid = '';
            this.subCostCenterDto.ccName = '';
            this.subCostCenterDto.active=true;
            this.subCostCenterDto.subccid = null;
            this.subCostCenterDto.subCCName = '';
            this.subCostCenterDto.accountId  = '';
            this.subCostCenterDto.accountName  = '';
            this.edit = false;
        }
        else {
            this.edit = true;
            this._subCostCenterService.getDataForEdit(id).subscribe(
                data => {   
                    console.log(this.subCostCenterDto);              
                    this.edit = true;
                    this.subCostCenterDto.id = data["result"]["subCostCenter"]["id"];
                    this.subCostCenterDto.ccid = data["result"]["subCostCenter"]["ccid"];
                    this.subCostCenterDto.subccid = data["result"]["subCostCenter"]["subccid"];
                    this.subCostCenterDto.subCCName = data["result"]["subCostCenter"]["subCCName"];
                    this.subCostCenterDto.ccName = data["result"]["subCostCenter"]["ccName"];
                    this.subCostCenterDto.accountId = data["result"]["subCostCenter"]["accountId"];
                    this.subCostCenterDto.accountName = data["result"]["subCostCenter"]["accountName"];
                    this.subCostCenterDto.active = data["result"]["subCostCenter"]["active"];
                }
            );

        }
        this.modal.show();  
    }

    save(): void {
        this.saving = true;
        debugger
        this._subCostCenterService.create(this.subCostCenterDto)
            .subscribe(() => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
        this.close();
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }

    openlookUpModal() {
        this.subCostCenterDto.ccid = '';
        this.subCostCenterDto.subccid = null;
        this.CostCenterLookupTableModal.show("CostCenter");
    }
    getNewFinanceModal(){
        this.subCostCenterDto.accountId = this.financeLookupTableModal.id;
        this.subCostCenterDto.accountName = this.financeLookupTableModal.displayName;
    }
    openSelectChartofACModal(){
       this.financeLookupTableModal.show("ChartOfAccount");
    }
    getLookUpData() {
        if (this.CostCenterLookupTableModal.data != null) {
            this.subCostCenterDto.ccid = this.CostCenterLookupTableModal.data.ccid;
            this.subCostCenterDto.ccName = this.CostCenterLookupTableModal.data.ccName;
            this._subCostCenterService.getMaxSubCostCenterId(this.subCostCenterDto.ccid).subscribe(
                data => {
                    this.subCostCenterDto.subccid = data["result"];
                }
            )
        }
    }
}
