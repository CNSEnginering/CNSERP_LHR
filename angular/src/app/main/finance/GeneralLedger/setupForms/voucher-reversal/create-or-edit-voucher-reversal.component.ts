import { Component, Injector, OnInit, ViewChild,EventEmitter, Output } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { voRevdto } from '@app/main/finance/shared/dto/voRev-dto';
import { ModalDirective } from 'ngx-bootstrap';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { FiscalCalendersServiceProxy, TaxAuthoritiesServiceProxy, VoucherEntryServiceProxy } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { voRevservices } from '@app/main/finance/shared/services/voRev.service';
import { finalize } from 'rxjs/operators';
import { Paginator } from 'primeng/components/paginator/paginator';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { data } from 'jquery';


@Component({
  selector: 'app-create-or-editvoucher-reversal',
  templateUrl: './create-or-edit-voucher-reversal.component.html'
})
export class CreateOrEditvoucherReversalComponent extends AppComponentBase{
  @ViewChild('createOrEditVoucherReversal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;
  revdto : voRevdto = new voRevdto();
  
  
  target: any;
  active = false;
  flag: boolean = false;
  dateValid: boolean = false;
  saving = false;
  DocYear = 0;
  newDocYear = 0;
  processing=false;

  constructor(injector: Injector,
    private _vouRevservices : voRevservices,
    private _fiscalCalendarsServiceProxy: FiscalCalendersServiceProxy,
    private _voucherEntryServiceProxy: VoucherEntryServiceProxy,
    ) { 
    
    super(injector);
  }


  show(Id?: number){
    debugger
    if(!Id){
      
      this.revdto  = new voRevdto();
      
      
      this._vouRevservices.getDocNo().subscribe(res=>{
        this.revdto.maxDocNo=res; 
       
       });
       this.revdto.docDate= new Date();
       //this.revdto.newDocDate= new Date();
      this.active = true;
      this.modal.show();
    } else {
      debugger;
      this._vouRevservices.getDataForEdit(Id).subscribe(result => {
          debugger;
          this.revdto = result["glDocRev"];
          this.revdto.docDate = new Date(
            result["glDocRev"]["docDate"]
        );
        this.DocYear = moment(this.revdto.docDate).year();
          this.revdto.newDocDate = new Date(
            result["glDocRev"]["newDocDate"]
        );
        this.newDocYear = moment(this.revdto.newDocDate).year();
       
          this.active = true;
          this.modal.show();
      });
  }    
  }

  //=====================Voucher Type================

  openVoucherTypeModal() {
    debugger;
    this.target = "Voucher";
    this.FinanceLookupTableModal.id = null;
    this.FinanceLookupTableModal.displayName = "";
    this.FinanceLookupTableModal.show("Voucher");
  }

  setVoucherTypeIdNull() {
      debugger;
      this.revdto.bookID = null;
      this.setVoucherNoNull();
      this.flag = false;
  }

  getNewVoucherType() {
      debugger;
      this.revdto.detId = parseInt(this.FinanceLookupTableModal.id);
      this.revdto.narration = this.FinanceLookupTableModal.displayName;
      this.revdto.docMonth = this.FinanceLookupTableModal.docMonth;
      this.revdto.fmtDocNo = this.FinanceLookupTableModal.fmtDocNo;
      this.revdto.bookID = this.FinanceLookupTableModal.bookId;
      this.revdto.docNo = this.FinanceLookupTableModal.docNo;
      //this.revdto.docDate = this.FinanceLookupTableModal.docDate;
      this.DocYear = moment(this.revdto.docDate).year();

      this.flag = true;
  }

  setVoucherNoNull() {
    debugger;
    this.revdto.bookID = null;
    this.revdto.detId = null;
    this.revdto.fmtDocNo = null;
    this.revdto.docMonth = null;
    this.revdto.docYear = null;
    //this.revdto.docDate = null;
    this.revdto.narration = null;
    this.revdto.docNo=null;
    this.DocYear = null;
  }
  //=====================Voucher No================
  process()
  {
    debugger
      this._vouRevservices.processReversalVoucher(this.revdto).subscribe(result => {
      debugger
          if(result["result"] =="save"){
              this.saving = false;
              this.notify.info(this.l('ProcessSuccessfully'));
              this._vouRevservices
              this.close();
            this.modalSave.emit(null);
          }
          else{
            this.message.error("Not Process");
            this.processing = false;
        }
    });
  }
  //=====================GLSubledger Model==============

 getDateParams(val?, checkForDocId?) {
  if(!this.revdto.id){
  this._fiscalCalendarsServiceProxy.getFiscalYearStatus(moment(this.revdto.newDocDate), 'GL').subscribe(
    result => {
      debugger
      if (result == true) {
        if (checkForDocId == null || checkForDocId == true) {
          this.revdto.newDocMonth = moment(this.revdto.newDocDate).month() + 1;
          this.newDocYear = moment(this.revdto.newDocDate).year();
          this._voucherEntryServiceProxy
            .getMaxDocId(this.revdto.bookID, true, moment(this.revdto.newDocDate).format("LLLL"))
            .subscribe(result => {
              debugger
              this.revdto.newFmtDocNo = undefined;
              this.revdto.newFmtDocNo = result;
              
            });
        }
        if (checkForDocId == null || checkForDocId == true) {
          this.revdto.newDocMonth = moment(this.revdto.newDocDate).month() + 1;
          this.newDocYear = moment(this.revdto.newDocDate).year();
          this._voucherEntryServiceProxy
            .getMaxDocId(this.revdto.bookID, false, moment(this.revdto.newDocDate).format("LLLL"))
            .subscribe(result => {
              debugger
              this.revdto.newDocNo = undefined;
              this.revdto.newDocNo = result;
              
            });
        }

        
        debugger
        this.dateValid = true;
      }
      else {
        this.message.info("This Date Is Locked");
        this.dateValid = false;
      }
    }
  )
}else
{
this.dateValid = true;
  }
}
//=====================GLSubledger Model==============
setnewVoucherNull() {
  debugger;
  this.revdto.newDocNo = null;
  this.revdto.newFmtDocNo = null;
  this.revdto.newDocMonth = null;
  this.revdto.newDocYear = null;
  this.revdto.newDocDate = null;
  this.newDocYear = null;
}  

  getNewModal(){
      debugger
      switch (this.target) {
        case "Voucher":
          this.getNewVoucherType();
          break;
      
        default:
          break;
      }
  }
 

  
  save(): void {
    this.saving = true;
    debugger;

    this._vouRevservices.create(this.revdto)
        .pipe(finalize(() => { this.saving = false; }))
        .subscribe(() => {
            debugger;
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
