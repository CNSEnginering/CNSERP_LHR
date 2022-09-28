import { Component, ViewChild, Injector, Output, EventEmitter, DebugElement } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CostCenterLookupTableModalComponent } from '../FinderModals/costCenter-lookup-table-modal.component';
import { ItemPricingService } from '../shared/services/itemPricing.service';
import { ThemeSettingsDto } from '@shared/service-proxies/service-proxies';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { CostCenterDto } from '../shared/dto/costCenter-dto';
import { CostCenterService } from '../shared/services/costCenter.service';



@Component({
    selector: 'costCenterModal',
    templateUrl: './create-or-edit-costCenter-modal.component.html'
})
export class CreateOrEditCostCenterModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('CostCenterLookupTableModal', { static: true }) CostCenterLookupTableModal: CostCenterLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    edit: boolean = false;
    active = false;
    saving = false;
    validForm: boolean = false;
    costCenterDto: CostCenterDto = new CostCenterDto();
    type: string = '';
    constructor(
        injector: Injector,
        private _costCenterService: CostCenterService
    ) {
        super(injector);
    }

    show(id?: number): void {
        debugger
        this.active = true;
        if (!id) {
             this.edit = false;
             this.costCenterDto.ccid = '';
             this.costCenterDto.ccName = '';
             this.costCenterDto.subAccId = 0;
             this.costCenterDto.subAccName = '';
             this.costCenterDto.accountID = '';
             this.costCenterDto.accountName = '';
             this.costCenterDto.active = true;
             this.costCenterDto.id = undefined;
        }
        else {
            this.edit = true;
            this._costCenterService.getDataForEdit(id).subscribe(
                data => {
                    this.costCenterDto.ccid = data["result"]["costCenter"]["ccid"];
                    this.costCenterDto.ccName = data["result"]["costCenter"]["ccName"];
                    this.costCenterDto.subAccId = data["result"]["costCenter"]["subAccID"];
                    this.costCenterDto.subAccName = data["result"]["costCenter"]["subAccName"];
                    this.costCenterDto.accountID = data["result"]["costCenter"]["accountID"];
                    this.costCenterDto.active = data["result"]["costCenter"]["active"];
                    this.costCenterDto.accountName = data["result"]["costCenter"]["accountName"];
                    this.costCenterDto.id = data["result"]["costCenter"]["id"];
                }
            );

        }
        this.modal.show();
    }

    save(): void {
        debugger
        this.saving = true;
        this._costCenterService.create(this.costCenterDto)
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

    openlookUpModal(type: string) {
        this.type = type;
        this.CostCenterLookupTableModal.data = null;
        if(this.type != "Account")
        {
            this.CostCenterLookupTableModal.accId = this.costCenterDto.accountID;
           if(this.CostCenterLookupTableModal.accId != '')
           {
            this.CostCenterLookupTableModal.show(type);
           }
        }
        else
        {
            this.CostCenterLookupTableModal.show(type);
        }

    }

     getLookUpData() {
        if (this.type == "Account") {
            if(this.CostCenterLookupTableModal.data != null)
            {
                this.costCenterDto.accountID = this.CostCenterLookupTableModal.data.id;
                this.costCenterDto.accountName = this.CostCenterLookupTableModal.data.accountName;
               
            }
            this.costCenterDto.subAccId = 0;
            this.costCenterDto.subAccName = '';
        }
        else {
            this.costCenterDto.subAccId = this.CostCenterLookupTableModal.data.id;
            this.costCenterDto.subAccName = this.CostCenterLookupTableModal.data.subAccName;
        }
    }

    checkCostCenterId() {
        this._costCenterService.checkCostCenterIdIfExists(this.costCenterDto.ccid)
            .subscribe((data) => {
                if (data["result"] == true) {
                    this.saving = false;
                    this.notify.info(this.l('Cost Center Id Already Exists'));
                    this.costCenterDto.ccid = '';
                }
            });
    }

}
