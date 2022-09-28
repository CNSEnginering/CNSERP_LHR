import { Component, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { Oedriversdto } from '../shared/dtos/oedriversdto';
import { OedriversService } from '../shared/services/oedrivers.service';
import { finalize } from 'rxjs/operators';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';

@Component({
  selector: 'app-creat-or-edit-oedrivers',
  templateUrl: './creat-or-edit-oedrivers.component.html',

})
export class CreatOrEditOedriversComponent extends AppComponentBase {
  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;

  OedriversDto: Oedriversdto = new Oedriversdto();
  active = false;
  saving = false;
  editMode = false;
  accountDesc: string;
  subAccountDesc: string;
  target: string;
  constructor(
    injector: Injector,
    private _OedriversService: OedriversService
  ) {
    super(injector);
  }
  createDate: Date;
  audtDate: Date;
  ngOnInit() {
  }
  show(id?: any, maxId?: number): void {
    debugger
    if (!id) {
      this.accountDesc = "";
      this.subAccountDesc = "";
      this.OedriversDto = new Oedriversdto();
      this.OedriversDto.driverID = maxId;
      this.OedriversDto.driverName = "";
      this.OedriversDto.active = true;
      this.OedriversDto.driverCtrlAcc = "";
      this.OedriversDto.driverSubAccID = null;
      this.active = true;
      this.modal.show();
    } else {
      debugger;

      this._OedriversService.getDataForEdit(id).subscribe(result => {
        debugger;

        this.OedriversDto = result["oeDrivers"];


        this.active = true;
        this.modal.show();
      });

    };
  }
  save() {
    this.saving = true;
    debugger;
    this.OedriversDto.audtDate = new Date;
    this.OedriversDto.audtUser = this.appSession.user.userName;
    if (this.editMode == false) {
      this.OedriversDto.createDate = new Date;
      this.OedriversDto.createdBy = this.appSession.user.userName;
    }
    debugger
    this._OedriversService.create(this.OedriversDto)
      .pipe(finalize(() => { this.saving = false; }))
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.close();
        this.modalSave.emit(null);
      });


  }
  getNewFinanceModal() {
    debugger;
    switch (this.target) {

      case "ChartOfAccount":
        this.getNewAccountID();
        break;
      case "SubLedger":
        this.getSubAccID();
        break;
      default:
        break;
    }
  }

  openAccountIDModal() {
    debugger;
    this.target = "ChartOfAccount";
    this.FinanceLookupTableModal.id = this.OedriversDto.driverCtrlAcc;
    this.FinanceLookupTableModal.displayName = this.OedriversDto.accountDesc;
    this.FinanceLookupTableModal.show(this.target);
  }

  setAccountIDNull() {
    this.OedriversDto.driverCtrlAcc = "";
    this.OedriversDto.accountDesc = "";
    this.setSubAccIDNull();
  }

  getNewAccountID() {
    debugger;
    this.OedriversDto.driverCtrlAcc = this.FinanceLookupTableModal.id;
    this.OedriversDto.accountDesc = this.FinanceLookupTableModal.displayName;
  }

  openSubAccIDModal() {
    debugger;
    var account = this.OedriversDto.driverCtrlAcc;
    if (account == "" || account == null) {
      this.message.warn(
        this.l("Please select account first"),
        "Account Required"
      );
      return;
    }
    this.target = "SubLedger";
    this.FinanceLookupTableModal.id = String(this.OedriversDto.driverSubAccID);
    this.FinanceLookupTableModal.displayName = this.OedriversDto.subAccountDesc;
    this.FinanceLookupTableModal.show(this.target, account);
  }

  setSubAccIDNull() {
    this.OedriversDto.driverSubAccID = null;
    this.OedriversDto.subAccountDesc = "";
  }

  getSubAccID() {
    this.OedriversDto.driverSubAccID = Number(this.FinanceLookupTableModal.id);
    this.OedriversDto.subAccountDesc = this.FinanceLookupTableModal.displayName;
  }
  close(): void {
    this.active = false;
    this.modal.hide();
  }
}
