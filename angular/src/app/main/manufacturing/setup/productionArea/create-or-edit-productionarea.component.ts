import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { InventoryLookupTableModalComponent } from '@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component';
import { AppComponentBase } from '@shared/common/app-component-base';

import { ModalDirective } from 'ngx-bootstrap';
import { MFAREADto } from '../../shared/dto/productionarea.dto';
import { ProductionAreaServiceProxy } from '../../shared/service/productarea.service';


@Component({
    selector: 'prodAreaModal',
    templateUrl: './create-or-edit-productionarea.component.html'
})
export class CreateOrEditProductionAreaComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('InventoryLookupTableModal', { static: true }) InventoryLookupTableModal: InventoryLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    dto: MFAREADto = new MFAREADto();
    editMode: boolean = false;

    constructor(
        injector: Injector,
        private _prodareaService: ProductionAreaServiceProxy,
    ) {
        super(injector);
    }

    show(id?: number): void {
        this.active = true;
        if (!id) {
            this.editMode = false;

            this.dto = new MFAREADto();
        }
        else {
            this.editMode = true;

            this.primengTableHelper.showLoadingIndicator();
            this._prodareaService.getDataForEdit(id).subscribe(data => {

                debugger;
                this.dto = data.mfarea
            });
        }
        this.modal.show();
    }

    save(): void {
        this.saving = true;
        this._prodareaService.create(this.dto)
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
    getNewLocation() {
        this.dto.locid = Number(this.InventoryLookupTableModal.id);
        this.dto.locDesc = this.InventoryLookupTableModal.displayName;
    }

    openLocationModal() {
        this.InventoryLookupTableModal.id = String(this.dto.locid);
        this.InventoryLookupTableModal.displayName = this.dto.locDesc;
        this.InventoryLookupTableModal.show('Location');

    }

    setLocIdNull() {
        this.dto.locid = null
        this.dto.locDesc = null
    }
}

