import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { InventoryLookupTableModalComponent } from '@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ICUOMsService } from '@app/main/supplyChain/inventory/shared/services/ic-uoms.service';
import { IC_UNITDto } from '@app/main/supplyChain/inventory/shared/services/ic-units-service';
import { ModalDirective } from 'ngx-bootstrap';
import { MFtooltySETDto } from '../../shared/dto/mftoolty.dto';
import { mftooltyServiceProxy } from '../../shared/service/mftoolty.service';
import { AccountSubLedgersServiceProxy, ProfileServiceProxy, NameValueDto, ComboboxItemDto, throwException, AccountSubLedgerChartofControlLookupTableDto } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'mftooltyModel',
  templateUrl: './create-or-edit-mftoolty.component.html',
  styleUrls: ['./create-or-edit-mftoolty.component.css']
})
export class CreateOrEditMftooltyComponent extends AppComponentBase {

  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  @ViewChild('InventoryLookupTableModal', { static: true }) InventoryLookupTableModal: InventoryLookupTableModalComponent;


  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  active = false;
  saving = false;

  dto: MFtooltySETDto = new MFtooltySETDto();
  editMode: boolean = false;
  unitOfmeasure: ComboboxItemDto[] = [];
  unitMeasure: IC_UNITDto = new IC_UNITDto();
  constructor(
      injector: Injector,
      private _mftooltyService: mftooltyServiceProxy,
      private _unitofMeasure: ICUOMsService,
  ) {
      super(injector);
  }
  getUnitofMeasure(): void {
    debugger
    this._unitofMeasure.getUnitofMeasureForCombobox(
    ).subscribe(result => {
        
        
        this.unitOfmeasure = result.items
    })
}

  show(id?: number): void {
      this.active = true;
      debugger
      this.getUnitofMeasure();
 if (!id) {
     this.editMode = false;
   this.dto = new MFtooltySETDto();
 }
 else {
    this.editMode = true;
      this.primengTableHelper.showLoadingIndicator();
        this._mftooltyService.getDataForEdit(id).subscribe(data => {

            debugger;
            this.dto = data.mftoolty;
            //this.dto.unit.valueOf=data.mftoolty.unit;
        });
    }
     
      this.modal.show();
  }

  save(): void {
      this.saving = true;
      
      this._mftooltyService.create(this.dto)
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
      this.dto.comments=null;
     this.dto.status=null;
      this.dto.tooltydesc=null;
      this.dto.tooltyid=null;
     this.dto.unit=null;
      this.dto.unitcost=0;

      this.modal.hide();
  }
  getNewLocation() {
      //this.dto.typeid = Number(this.InventoryLookupTableModal.id);
      //this.dto.locDesc = this.InventoryLookupTableModal.displayName;
  }

  openLocationModal() {
     // this.InventoryLookupTableModal.id = String(this.dto.locid);
     // this.InventoryLookupTableModal.displayName = this.dto.locDesc;
      this.InventoryLookupTableModal.show('Location');

  }
  openlookUpModal(type: string) {
    //this.ItemPricingLookupTableModal.data = null;
    //this.ItemPricingLookupTableModal.show(type);
}

  setLocIdNull() {
    //  this.dto.locid = null
     // this.dto.locDesc = null
  }

}
