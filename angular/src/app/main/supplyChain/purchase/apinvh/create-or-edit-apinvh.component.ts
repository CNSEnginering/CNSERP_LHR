import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective, parseDate } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditapinvDto } from '../../purchase/shared/dtos/apiInv-dto';
import { apinvDTo } from '../../purchase/shared/dtos/apiInv-dto';
import { ApinvHServiceProxy } from '../shared/services/apinvH.service';
import { CommonServiceLookupTableModalComponent } from '@app/finders/commonService/commonService-lookup-table-modal.component';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { AgGridExtend } from '@app/shared/common/ag-grid-extend/ag-grid-extend';
import { GLTRHeadersServiceProxy, FiscalCalendarsServiceProxy, FiscalCalendersServiceProxy } from '@shared/service-proxies/service-proxies';
import { Observable, of } from 'rxjs';
import { NgForm } from '@angular/forms';
import { invalid } from '@angular/compiler/src/render3/view/util';
import { DatePipe } from '@angular/common';
import { GetDataService } from '../../inventory/shared/services/get-data.service';
import { ApprovalService } from '../../periodics/shared/services/approval-service.';
@Component({
  selector: 'app-create-or-edit-apinvh',
  templateUrl: './create-or-edit-apinvh.component.html',
  providers: [DatePipe]
})
export class CreateOrEditApinvhComponent  extends AppComponentBase {

  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  @ViewChild('f', { static: true }) forms: NgForm;
  @ViewChild('commonServiceLookupTableModal', { static: true }) commonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
  @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  CreateOrEditapinvH: CreateOrEditapinvDto = new CreateOrEditapinvDto();
  apinvDto: apinvDTo = new apinvDTo();

  active = false;
    saving = false;
    subLedgerAccList = [];
    accountID = '';
    accountDesc = '';
    subAccID = '';
    subAccDesc = '';
    totalAmount = 0;
    InvAmount = 0;
    private setParms;
    TypeIdShow = '';
    LockDocDate: Date;
    depositDate: Date;
    locations:any;
    creditLimit = true;
    tab: string;
    
    agGridExtend: AgGridExtend = new AgGridExtend();
    checkedval:boolean=true;
    auditTime: Date;
    docDate:  string;
    postDate: string;
    accountIdC = '';
    processing = false;
    processing1 = false;
    target: string;
    totalDebit: number;
    todaysDate: Date = new Date();
    totalCredit: number;
    totalBalance: number;
    taxAccDesc: string;
    taxClassDesc: string;
    arTaxClassDesc: string;
    ictaxClassDesc: string;
    taxAuthDesc: string;
    ictaxAuthDesc: string;
    locDesc: string;
    instrumentNoChk: boolean = false;
    typeTaxClass: string;
    validDate: boolean = false;
    constructor(
        injector: Injector,
        private _apinvHServiceProxy: ApinvHServiceProxy,private datePipe: DatePipe,
        private _approvelService: ApprovalService,   private _getDataService: GetDataService,
    ) {
        super(injector);
    }
    SetDefaultRecord(result:any){
        console.log(result);
          this.apinvDto.curID=result.currency;
          this.apinvDto.LocId=result.currentLocID;
          this.apinvDto.curRate=result.currate;
         
      }
      GetSetUpDetail(): void {
        this._getDataService.GetSetUpDetail().subscribe(result => {
          
           this.SetDefaultRecord(result);
        });
      }

      onOptionsSelected(event){
        debugger
        const value = event.target.value;
       // this.selected = value;
       if(value==4 || value==5){
         this.apinvDto.chequeNo="-";
       }else{
        this.apinvDto.chequeNo="";
       }
      
   }
    show( maxId?: number,Records?:any): void {
        this.auditTime = null;
        debugger;
        this.getLocations("ICLocations");
        if (maxId) {
           // this.CreateOrEditapinvH = new CreateOrEditapinvDto();
            this.apinvDto.docNo = maxId;
            this.apinvDto.id = null;
            //this.docDate =new Date().getDate();// moment().endOf('day');
            //this.postDate = moment().endOf('day');
            this.apinvDto.vAccountID = "";
            this.apinvDto.narration = "";
            this.apinvDto.payReason = "";
            this.apinvDto.subAccID = 0;
            this.accountDesc = "";
            this.apinvDto.recAmt=0;
            this.subAccDesc = "";
            this.apinvDto.postDate=new Date();
            this.apinvDto.paymentOption='Cash';
            this.apinvDto.partyInvNo="";
            this.apinvDto.discAmount=null;
            this.apinvDto.amount=0;
            this.apinvDto.approved = false;
            this.apinvDto.paidAmt=0;
            this.apinvDto.pendingAmt=0;
            this.apinvDto.posted = false;
            this.apinvDto.discAmount=0;
            this.apinvDto.partyInvDate="";
            this.apinvDto.docDate=new Date();
            this.ictaxClassDesc = "";
            this.ictaxAuthDesc = "";
            this.totalAmount = 0;
            this.totalCredit = 0;
            this.totalDebit = 0;
            this.totalBalance = 0;
            this.active = true;
            //this.getDateParams();
            this.modal.show();
        } else {
           this._apinvHServiceProxy.getDataForEdit(Records.apinvh.id).subscribe(result => {
               debugger
                this.apinvDto = result.apinvh;
                this.active = true;
                this.totalAmount=result.apinvh.amount-result.apinvh.discAmount;
               // this.docDate= this.docDate== null ? moment(result.apinvh.docDate).format('DD-MM-YYYY') : null; 
               // this.postDate= this.postDate== null ? moment(result.apinvh.postDate).format('DD-MM-YYYY') : null; 
              this.accountDesc=result.apinvh.vAccountName ;
              this.ictaxAuthDesc = result.apinvh.taxAccName;
              this.subAccDesc=result.apinvh.subAccName ; 
              this.apinvDto.LocId=result["apinvh"]["locId"];
              this.apinvDto.docDate=new Date(result.apinvh.docDate);//moment(result.apinvh.docDate).format('DD/MM/YYYY'); 
              this.apinvDto.postDate=new Date(result.apinvh.postDate);// moment(result.apinvh.postDate).format('DD/MM/YYYY'); 
              this.apinvDto.partyInvDate=new Date(result.apinvh.partyInvDate);//moment(result.apinvh.partyInvDate).format('DD/MM/YYYY'); 
               this.modal.show();
               
            });
           
        }
     
    }

    showInvoice(DocNo:any,accid:any,subAccid:any): void {
        this.getLocations("ICLocations");
        debugger;
        this.GetSetUpDetail();
        this._apinvHServiceProxy.getMaxDocId().subscribe(result => {
          debugger; 
          
          this.apinvDto.docNo=result;
        });
        this.apinvDto.id = null;
        //this.docDate =new Date().getDate();// moment().endOf('day');
        //this.postDate = moment().endOf('day');
        this.apinvDto.vAccountID = "";
        this.apinvDto.narration = "";
        this.apinvDto.approved = false;
        this.apinvDto.posted = false;
        this.apinvDto.payReason = "";
        this.apinvDto.subAccID = 0;
        this.apinvDto.paidAmt=0;
        this.apinvDto.pendingAmt=0;
        this.accountDesc = "";
        this.subAccDesc = "";
        this.apinvDto.postDate=new Date();
        this.apinvDto.paymentOption='Cash';
        this.apinvDto.partyInvNo="";
        this.apinvDto.discAmount=null;
        this.apinvDto.amount=0;
        this.apinvDto.discAmount=0;
        this.apinvDto.partyInvDate="";
        this.apinvDto.docDate=new Date();
        this.ictaxClassDesc = "";
        this.ictaxAuthDesc = "";
        this.totalAmount = 0;
        this.totalCredit = 0;
        this.totalDebit = 0;
        this.totalBalance = 0;
     
        this._apinvHServiceProxy.getAmountforInvoice(parseInt(DocNo)).subscribe(
            res => {
            //if(res["countRec"]==0){
                this.apinvDto.recAmt=res["recAmt"];
                this.apinvDto.retAmt=res["retAmt"];
                this.apinvDto.amount=res["balAmt"];
                this.totalAmount=res["balAmt"];
                this.apinvDto.vAccountID=res["vAccountID"];
                this.apinvDto.subAccID=res["subAccID"];
                this.apinvDto.partyInvNo=res["partyInvNo"];
                this.apinvDto.pendingAmt=res["pendingAmt"];
                this.apinvDto.partyInvDate =parseDate(res["partyInvDate"]);
                this.subAccDesc=res["subaccName"];
                this.accountDesc=res["accountName"];
                this.active = true;       
                this.modal.show();
           // }else{
            //    this.message.info("Invoices Already Generated.");
           // }
            
            });
        
               
           
           
        
     
    }
    processReceiptEntry():void{
        debugger;
        
        this.message.confirm(
            "Invoice Process ",
            (isConfirmed) => {
                if (isConfirmed) {
                    this.processing = true; 
                    debugger;


                this._apinvHServiceProxy.ProcessInvoice(this.apinvDto).subscribe(result => {
                    debugger
                        if(result["result"]=="Save"){
                            this.saving = false;
                            this.notify.info(this.l('ProcessSuccessfully'));
                            this.close();
                            this.modalSave.emit(null);
                        }else if(result["result"]=="NoAccount"){
                            this.message.warn("Account Receivable not found","Account Required");
                            this.processing = false;
                        }else if(result["result"]=="CreditDebitMismatch"){
                            this.message.warn("Credit and Debit amounts are mismatched","Cannot Process");
                            this.processing = false;
                        }else{
                            this.message.error("Input not valid","Verify Input");
                            this.processing = false;
                        } 
                    });
                }
            }
        );
    }
    approveDoc(id: number,mode, approve) {
        debugger;
        this.message.confirm(
            approve?'Approve Invoice':'Unapprove Invoice',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._approvelService.ApprovalData("Apinv", [id], mode, approve)
                        .subscribe(() => {
                            if (approve == true) {
                                this.notify.success(this.l('SuccessfullyApproved'));
                                this.close();
                                this.modalSave.emit(null);
                            } else {
                                this.notify.success(this.l('SuccessfullyUnApproved'));
                                this.close();
                                this.modalSave.emit(null);
                            }
                        });
                }
            }
        );
    }

    save(): void {
        debugger
        if(this.apinvDto.pendingAmt>=this.apinvDto.paidAmt){
            this.saveAfter();
        }else{
            this.message.info("Paid Amount should be less than the Pending amount !");
        }
        
        
    }
    getLocations(target:string):void{
      
        this._getDataService.getList(target).subscribe(result => {
            debugger;
            this.locations = result;
        }); 
    }

    saveAfter() {
        if(this.apinvDto.pendingAmt>=this.apinvDto.paidAmt){
            this.message.confirm(
                'Save Invoice',
                (isConfirmed) => {
                    if (isConfirmed) {
                        this.saving = true;
                        debugger
                        this.apinvDto.docDate=this.apinvDto.docDate.toLocaleString();
                        this.apinvDto.postDate=this.apinvDto.postDate.toLocaleString();
                        //this.apinvDto.docDate=this.apinvDto.docDate==null?new Date():new Date(this.apinvDto.docDate);
                        //this.apinvDto.postDate=this.apinvDto.postDate==null?new Date():new Date(this.apinvDto.postDate);
                        //this.apinvDto.partyInvDate=new Date(this.apinvDto.partyInvDate); 
                        debugger
                        this._apinvHServiceProxy.create(this.apinvDto)
                            .pipe(finalize(() => { this.saving = false; }))
                            .subscribe(() => {
                                 this.notify.info(this.l('Saved Successfully'));
                                // this.message.confirm("Press 'Yes' for create new Invoice", this.l('SavedSuccessfully'), (isConfirmed) => {
                                //     debugger
                                //     if (isConfirmed) {
                                      
                                //         this.apinvDto.docNo = this.apinvDto.docNo + 1;
                                        
                                //     } else {
                                //         this.close();
                                        
                                //     }
                                this.modal.hide();
                                    this.modalSave.emit(null);
                                //});
                            });
                    }
                }
            );
        }else{
            this.message.info("Paid Amount should be less than the Pending amount !");
        }
       

    }

  


    getNewFinanceModal() {
        debugger;
        switch (this.target) {
            case "ChartOfAccount":
                this.getNewChartOfAC();
                break;
            case "SubLedger":
                this.getNewSubAcc();
                break;
            case "GLLocation":
                this.getNewLocation();
                break;
            case "ChequeBookDetail":
                this.getChequeBookDetail();
                break;
            case "ArTerm":
                this.getArTerm();
                break;
            case "IncomeTax":
                this.getNewTaxClass();
                break;
                case "InvoiceNoPV":
                this.getNewInvoicePV();
                break;
            case "Cash":
                this.getNewCash();
                break;
            default:
                break;
        }
    }
    
    getArTerm() {
        debugger
        this.apinvDto.taxClass = Number(this.FinanceLookupTableModal.id);
        this.arTaxClassDesc = this.FinanceLookupTableModal.displayName;
        this.apinvDto.taxRate = this.FinanceLookupTableModal.taxRate;
        this.apinvDto.vAccountID = this.FinanceLookupTableModal.accountID;
        this.apinvDto.amount = Math.round((this.apinvDto.taxRate * this.apinvDto.taxAmount) / 100);
    }
    getChequeBookDetail() {
        debugger
        this.apinvDto.chequeNo = this.FinanceLookupTableModal.id;
    }

    getNewCommonServiceModal() {
        switch (this.target) {
            case "Bank":
            case "Cash":
                this.getNewBankId();
                break;
            case "Currency":
                this.getNewCurrencyRateId();
                break;
            case "TaxAuthority":
                this.getNewTaxAuthority();
                break;
            case "TaxClass":
                this.getNewTaxClass();
                break;
            case "IncomeTax":
                this.getNewTaxClass();
                break;
            case "CPR":
                this.getNewCPR()
            default:
                break;
        }
    }
    //=====================Currency Rate Model================
    openSelectCurrencyRateModal() {
        debugger;
        this.target = "Currency";
        this.commonServiceLookupTableModal.id = this.apinvDto.curID;
        this.commonServiceLookupTableModal.currRate = this.apinvDto.curRate;
        this.commonServiceLookupTableModal.show(this.target);
    }


    setCurrencyRateIdNull() {
        this.apinvDto.curID = '';
        this.apinvDto.curRate = null;
    }


    getNewCurrencyRateId() {
        debugger;
        this.apinvDto.curID = this.commonServiceLookupTableModal.id;
        this.apinvDto.curRate = this.commonServiceLookupTableModal.currRate;
    }
    //================Currency Rate Model===============

    //================CPR Modal=========================
    openSelectCPRModal() {
        this.target = "CPR";
        this.commonServiceLookupTableModal.id = "";
        this.commonServiceLookupTableModal.displayName = "";
        this.commonServiceLookupTableModal.show(this.target);

    }
    getNewCPR() {

        this.apinvDto.cprID = Number(this.commonServiceLookupTableModal.id);
        this.apinvDto.cprNo = this.commonServiceLookupTableModal.displayName;
    }

    //=====================Bank Model==================
    openSelectBankIdModal() {
        debugger;
        if(this.apinvDto.paymentOption=='Bank'){
            this.target = this.apinvDto.paymentOption;
            this.commonServiceLookupTableModal.id = this.apinvDto.bankID;
            this.commonServiceLookupTableModal.accountId = this.apinvDto.bAccountID;
            this.commonServiceLookupTableModal.show(this.apinvDto.paymentOption, this.apinvDto.paymentOption == "Bank" ? "1" : "2");
    
        }else{
            this.target = "GLConfig";
            this.FinanceLookupTableModal.id = this.apinvDto.vAccountID;
            this.FinanceLookupTableModal.displayName = this.accountDesc;
            this.FinanceLookupTableModal.show(this.target,'CP');
            this.target = this.apinvDto.paymentOption;
        }
        
        
    }


    setBankIdNull() {
        this.apinvDto.bankID = '';
        this.apinvDto.bAccountID = '';
    }


    getNewBankId() {
        debugger;
        this. setBankIdNull();
        this.apinvDto.bankID = this.commonServiceLookupTableModal.id;
        this.apinvDto.bAccountID = this.commonServiceLookupTableModal.accountId;
    }
    getNewCash() {
        debugger;
        this. setBankIdNull();
        this.apinvDto.bankID = this.FinanceLookupTableModal.displayName;
        this.apinvDto.bAccountID = this.FinanceLookupTableModal.accountID;
    }
    //=====================Bank Model======================


    //=====================Chart of Ac Model================
    openAccountIDModal() {
      debugger;
      this.target = "ChartOfAccount";
      this.FinanceLookupTableModal.id = this.apinvDto.vAccountID;
      this.FinanceLookupTableModal.displayName = this.accountDesc;
      this.FinanceLookupTableModal.show(this.target);
  }

  setAccountIDNull() {
      this.apinvDto.vAccountID = "";
      this.accountDesc = "";
      this.setSubAccIDNull();
  }

    getNewChartOfAC() {
        debugger;
        this.apinvDto.vAccountID = this.FinanceLookupTableModal.id;
        this.accountDesc = this.FinanceLookupTableModal.displayName;
        this.setSubAccIDNull();
    }
    //=====================Chart of Ac Model================

    //=====================Sub Account Model================
    openSubAccIDModal() {
      debugger;
      var account = this.apinvDto.vAccountID;
      if (account == "" || account == null) {
          this.message.warn(
              this.l("Please select account first"),
              "Account Required"
          );
          return;
      }
      this.target = "SubLedger";
      this.FinanceLookupTableModal.id = String(this.apinvDto.subAccID);
      this.FinanceLookupTableModal.displayName = this.subAccDesc;
      this.FinanceLookupTableModal.show(this.target, account);
  }

  setSubAccIDNull() {
      this.apinvDto.subAccID = null;
      this.subAccDesc = "";
  }

  openInvoiceNoModal() {
    debugger;
    this.target = "InvoiceNoPV";
    this.FinanceLookupTableModal.id = String(this.apinvDto.partyInvNo);
    this.FinanceLookupTableModal.displayName = this.apinvDto.partyInvNo;
    this.FinanceLookupTableModal.docDate = null;
    this.FinanceLookupTableModal.amount = this.apinvDto.amount;
    this.FinanceLookupTableModal.show(this.target,"InvoiceNoPV",this.apinvDto.vAccountID,null,this.apinvDto.subAccID);
}
GetAmount(){
    
    if(this.apinvDto.partyInvNo.toString()!="" && this.apinvDto.partyInvNo.toString()!=null){

      this._apinvHServiceProxy.getAmountforInvoice(parseInt(this.apinvDto.partyInvNo)).subscribe(
        res => {
            
         this.apinvDto.recAmt=res["recAmt"];
         this.apinvDto.retAmt=res["retAmt"];
         this.apinvDto.amount=res["balAmt"];
         this.apinvDto.pendingAmt=res["pendingAmt"];
         this.totalAmount=res["balAmt"];
       
        });

    }
}
getNewInvoicePV(){
    this.apinvDto.partyInvNo=this.FinanceLookupTableModal.id;
    this.apinvDto.partyInvDate=this.FinanceLookupTableModal.docDate;//moment().format('YYYY-MM-DD');
    //this.apinvDto.amount=this.FinanceLookupTableModal.amount;
}
setInvoiceNoNull() {
    this.apinvDto.partyInvNo = null;
    this.apinvDto.partyInvNo = "";
    this.apinvDto.partyInvDate = null;
    this.apinvDto.amount = null;
}
    getNewSubAcc() {
        this.apinvDto.subAccID = Number(this.FinanceLookupTableModal.id);
        this.subAccDesc = this.FinanceLookupTableModal.displayName;
    }
    //=====================Sub Account Model================
    CalculateDiscAmt(){
        debugger
        if(this.apinvDto.amount!=null){
         this.totalAmount=this.apinvDto.amount-this.apinvDto.discAmount;
        }
    }
    //=====================Tax Authority Model================
    openSelectTaxAuthorityModal() {
        this.target = "TaxAuthority";
        //this.tab = tab;
        this.commonServiceLookupTableModal.id = this.apinvDto.taxAuth;
        this.commonServiceLookupTableModal.displayName = this.taxAuthDesc;
        this.commonServiceLookupTableModal.show(this.target);
    }

    setTaxAuthorityIdNull() {
        this.apinvDto.taxAuth = '';
        this.ictaxAuthDesc = '';
        this.apinvDto.taxAccID = '';
        this.apinvDto.taxRate = 0;
        this.apinvDto.taxClass =null;
    }

    getNewTaxAuthority() {
        debugger
        // if (this.tab == "Tab2") {
        //     if (this.commonServiceLookupTableModal.id != this.apinvDto.taxAuth)
        //         this.setTaxAuthorityIdNull();
        //     this.apinvDto.taxAuth = this.commonServiceLookupTableModal.id;
        //     this.ictaxAuthDesc = this.commonServiceLookupTableModal.displayName;
        // }
        // else {
        //     if (this.commonServiceLookupTableModal.id != this.apinvDto.taxAuth)
        //         this.setTaxClassIdNull();
        //     this.apinvDto.taxAuth = this.commonServiceLookupTableModal.id;
        //     this.taxAuthDesc = this.commonServiceLookupTableModal.displayName;
        // }
        this.setTaxAuthorityIdNull();
        this.setTaxClassIdNull();
        this.apinvDto.taxAuth = this.commonServiceLookupTableModal.id;
        this.ictaxAuthDesc = this.commonServiceLookupTableModal.displayName;
    }
    //=====================Tax Authority Model================

    //=====================Tax Class================
    openSelectTaxClassModal(tab: string, typeTaxClass: string) {
        debugger
        this.tab = tab;
        if (tab == 'Tab2' && typeTaxClass == "IncomeTax") {
            if (this.apinvDto.taxAuth == "" || this.apinvDto.taxAuth == null) {
                this.message.warn(this.l('Please select Tax authority'), 'Tax Authority Required');
                return;
            }
            this.target = "IncomeTax";
            this.commonServiceLookupTableModal.id = String(this.apinvDto.taxClass);
            this.commonServiceLookupTableModal.displayName = this.taxClassDesc;
            this.commonServiceLookupTableModal.accountId = this.apinvDto.taxAccID;
            this.commonServiceLookupTableModal.taxRate = this.apinvDto.taxRate;
            this.commonServiceLookupTableModal.show("TaxClass", this.apinvDto.taxAuth);
        }
        else if (tab == 'Tab2' && typeTaxClass != "TaxClass") {
            // if (this.glinvHeader.taxAuth == "" || this.glinvHeader.taxAuth == null) {
            //     this.message.warn(this.l('Please select Tax authority'), 'Tax Authority Required');
            //     return;
            // }
            this.target = "ArTerm";
            this.FinanceLookupTableModal.id = String(this.apinvDto.taxClass);
            this.FinanceLookupTableModal.displayName = this.taxClassDesc;
            this.FinanceLookupTableModal.accountID = this.apinvDto.taxAccID;
            this.FinanceLookupTableModal.taxRate = this.apinvDto.taxRate;
            this.FinanceLookupTableModal.show(this.target, "", "", " Ar Term");
        }
        else {
            if (this.apinvDto.taxAuth == "" || this.apinvDto.taxAuth == null) {
                this.message.warn(this.l('Please select Tax authority'), 'Tax Authority Required');
                return;
            }
            this.target = "TaxClass";
            this.commonServiceLookupTableModal.id = String(this.apinvDto.taxClass);
            this.commonServiceLookupTableModal.displayName = this.taxClassDesc;
            this.commonServiceLookupTableModal.accountId = this.apinvDto.taxAccID;
            this.commonServiceLookupTableModal.taxRate = this.apinvDto.taxRate;
            this.commonServiceLookupTableModal.show(this.target, this.apinvDto.taxAuth);
        }
    }
    getNewTaxClass() {
        debugger
        if (this.target == 'IncomeTax') {
            this.apinvDto.taxClass = Number(this.commonServiceLookupTableModal.id);
            this.ictaxClassDesc = this.commonServiceLookupTableModal.displayName;
            this.apinvDto.taxRate = this.commonServiceLookupTableModal.taxRate;
            this.apinvDto.taxAccID = this.commonServiceLookupTableModal.accountId;
            this.apinvDto.taxAmount = Math.round((this.apinvDto.taxRate * this.totalAmount) / 100);
        }
        else {
            this.apinvDto.taxClass = Number(this.commonServiceLookupTableModal.id);
            this.taxClassDesc = this.commonServiceLookupTableModal.displayName;
            this.apinvDto.taxRate = this.commonServiceLookupTableModal.taxRate;
            this.apinvDto.taxAccID = this.commonServiceLookupTableModal.accountId;
            this.apinvDto.taxAmount = Math.round((this.apinvDto.taxRate * this.totalAmount) / 100);
        }
    }
    setTaxClassIdNull() {
        debugger
        this.apinvDto.taxClass = null;
        this.taxClassDesc = '';
        this.apinvDto.taxAccID = '';
        this.apinvDto.taxRate = 0;
    }

    openInstrumentNo() {
        if (this.apinvDto.bankID != "" && this.apinvDto.bankID != null) {
            this.target = "ChequeBookDetail";
            this.FinanceLookupTableModal.id = "";
            this.FinanceLookupTableModal.displayName = "";
            this.FinanceLookupTableModal.show("ChequeBookDetail", this.apinvDto.bAccountID, "", " Instrument No");
        }
        else {
            this.message.confirm("Please select account first");
        }
    }
    //=====================Tax Class================

    //=====================Location Model================
    openSelectLocationModal() {
        this.target = "GLLocation";
        this.FinanceLookupTableModal.id = String(this.apinvDto.LocId);
        this.FinanceLookupTableModal.displayName = this.locDesc;
        this.FinanceLookupTableModal.show(this.target);
    }
    getNewLocation() {
        debugger;
        this.apinvDto.LocId = Number(this.FinanceLookupTableModal.id);
        this.locDesc = this.FinanceLookupTableModal.displayName;
    }
    setLocationIDNull() {
        this.apinvDto.LocId = null;
        this.locDesc = "";

    }
    //=====================Location Model================

    close(): void {

        this.active = false;
        this.modal.hide();
        this.forms.reset();
    }
}
