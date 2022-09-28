import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ToolTypeLookupTableModalComponent } from '@app/finders/Manufacturing/Tooltype-Modal/ToolType-table-modal.component';
import { AppComponentBase } from '@shared/common/app-component-base';
//import { ICUOMsService } from '@app/main/supplyChain/inventory/shared/services/ic-uoms.service';
//import { IC_UNITDto } from '@app/main/supplyChain/inventory/shared/services/ic-units-service';
import { ModalDirective } from 'ngx-bootstrap';
import { MFtoolSETDto } from '../../shared/dto/mftool.dto';
import { mftoolServiceProxy } from '../../shared/service/mftool.service';
import { AccountSubLedgersServiceProxy, ProfileServiceProxy, NameValueDto, ComboboxItemDto, throwException, AccountSubLedgerChartofControlLookupTableDto } from '@shared/service-proxies/service-proxies';


@Component({
  selector: 'mftoolModel',
  templateUrl: './create-or-edit-mftool.component.html',
  styleUrls: ['./create-or-edit-mftool.component.css']
})
export class CreateOrEditMftoolComponent extends AppComponentBase {
  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  @ViewChild('tooltypeLookupTableModal', { static: true }) tooltypeLookupTableModal: ToolTypeLookupTableModalComponent;


  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  active = false;
  saving = false;
  target: any;
  dto: MFtoolSETDto = new MFtoolSETDto();
  editMode: boolean = false;
  unitOfmeasure: ComboboxItemDto[] = [];
  //unitMeasure: IC_UNITDto = new IC_UNITDto();
  constructor(
    injector: Injector,
    private _mftoolService: mftoolServiceProxy,
    // private _unitofMeasure: ICUOMsService,
  ) {
    super(injector);
  }
  //   getUnitofMeasure(): void {
  //     debugger
  //     this._unitofMeasure.getUnitofMeasureForCombobox(
  //     ).subscribe(result => {


  //         this.unitOfmeasure = result.items
  //     })
  // }

  show(id?: number): void {
    this.active = true;
    debugger
    //this.getUnitofMeasure();
    if (!id) {
      this.editMode = false;
      this.dto = new MFtoolSETDto();
    }
    else {
      this.editMode = true;
      this.primengTableHelper.showLoadingIndicator();
      this._mftoolService.getDataForEdit(id).subscribe(data => {

        debugger;
        this.dto = data.mftool;
        //this.dto.unit.valueOf=data.mftoolty.unit;
      });
    }

    this.modal.show();
  }

  save(): void {
    this.saving = true;

    this._mftoolService.create(this.dto)
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

    this.dto.status = null;
    this.dto.tooldesc = null;
    this.dto.toolid = null;


    this.modal.hide();
  }
  openlookUpModal() {
   
    this.tooltypeLookupTableModal.show();
  }

  getToolType() {
    this.dto.tooltyid = this.tooltypeLookupTableModal.id
    this.dto.tooltypedesc = this.tooltypeLookupTableModal.displayName
  }




}
