import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';

import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { InventoryLookupTableModalComponent } from '@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component';
import { ModalDirective } from 'ngx-bootstrap';
import { MFACSETDto } from '../../shared/dto/manufacacc.dto';
import { ManufacAccServiceProxy } from '../../shared/service/manufacAcc.service';


@Component({
    selector: 'ManufacAccModal',
    templateUrl: './create-or-edit-manufacAcc-modal.component.html'
})
export class CreateOrEditManufacAccModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild('InventoryLookupTableModal', { static: true }) InventoryLookupTableModal: InventoryLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    transCheck: boolean = false;
    accs: MFACSETDto = new MFACSETDto();
    target: any;
    editMode: boolean = false;
    isCodeLength = false;

    constructor(
        injector: Injector,
        private _mfacAccountsService: ManufacAccServiceProxy,
    ) {
        super(injector);
    }

    show(id?: number): void {
        this.active = true;
        this.transCheck = false;
        if (!id) {
            this.editMode = false;

            this.accs = new MFACSETDto();
        }
        else {
            this.editMode = true;

            this.isCodeLength = true;
            this.primengTableHelper.showLoadingIndicator();
            this._mfacAccountsService.getDataForEdit(id).subscribe(data => {
                console.log(data);
                debugger;
                this.accs = data.mfacset
            });
        }
        this.modal.show();
    }

    save(): void {
        this.saving = true;
        this._mfacAccountsService.create(this.accs)
            .subscribe(() => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
        this.close();
    }

    getNewFinanceModal() {
        debugger
        this.getNewChartOfAC(this.target);
    }

    //////////////////////////////////////Chart of Account///////////////////////////////////////////////
    openSelectChartofACModal(ac) {
        this.target = "ChartOfAccount";
        if (ac == "wip") {
            this.FinanceLookupTableModal.id = this.accs.wipacct;
            this.FinanceLookupTableModal.displayName = this.accs.wipacct;
            this.FinanceLookupTableModal.show(this.target);
        }
        else if (ac == "labacct") {
            this.FinanceLookupTableModal.id = this.accs.setlabacct;
            this.FinanceLookupTableModal.displayName = this.accs.setlabacct;
            this.FinanceLookupTableModal.show(this.target);
        }
        else if (ac == "runlabacct") {
            this.FinanceLookupTableModal.id = this.accs.runlabacct;
            this.FinanceLookupTableModal.displayName = this.accs.runlabacct;
            this.FinanceLookupTableModal.show(this.target);
        }
        else if (ac == "ovhacct") {
            this.FinanceLookupTableModal.id = this.accs.ovhacct;
            this.FinanceLookupTableModal.displayName = this.accs.ovhacct;
            this.FinanceLookupTableModal.show(this.target, "true");
        }

        this.target = ac;
    }

    getNewChartOfAC(ac) {
        if (ac == "wip") {
            this.accs.wipacct = this.FinanceLookupTableModal.id;
            this.accs.wipaccDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (ac == "labacct") {
            this.accs.setlabacct = this.FinanceLookupTableModal.id;
            this.accs.labaccDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (ac == "runlabacct") {
            this.accs.runlabacct = this.FinanceLookupTableModal.id;
            this.accs.runLabAccDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (ac == "ovhacct") {
            this.accs.ovhacct = this.FinanceLookupTableModal.id;
            this.accs.ovhacctDesc = this.FinanceLookupTableModal.displayName;
        }

    }


    close(): void {
        this.active = false;
        this.isCodeLength = false;
        this.modal.hide();
    }
    codeLength(): void {
        if (this.accs.acctset.length < 3) {
            this.isCodeLength = false;
            this.message.warn("", "Code is not valid");
        } else {
            this.isCodeLength = true;
        }
    }


}

