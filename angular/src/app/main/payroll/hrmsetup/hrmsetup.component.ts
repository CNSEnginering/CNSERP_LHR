import {  AfterViewInit, Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { inject } from '@angular/core/testing';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { hrmSetupDto } from '../shared/dto/hrmSetup-dto';
import { HrmSetupServiceProxy } from '../shared/services/hrmSetup.service';
import { FinanceLookupTableModalComponent } from "@app/finders/finance/finance-lookup-table-modal.component";
import { finalize } from 'rxjs/operators';
import { ModalDirective } from 'ngx-bootstrap';



@Component({
  templateUrl: './hrmsetup.component.html',
  animations : [appModuleAnimation()]

})

export  class HrmsetupComponent extends AppComponentBase implements AfterViewInit {

  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @Input() mode: string;
  accountHrmSetup: hrmSetupDto = new hrmSetupDto();

  @ViewChild("FinanceLookupTableModal", { static: true })
    FinanceLookupTableModal: FinanceLookupTableModalComponent;
  target : string;
  chartofControlAccountName= "";
  chartofControlAccountName2= "";
  chartofControlAccountName3= "";
  chartofControlAccountName4= "";
  flag = false;
  saving = false;
  clearbtn:boolean=false;
  active = false;



  constructor
  (
    injector: Injector,
    public _hrmSetupServiceProxy : HrmSetupServiceProxy
  )
  { 
    super (injector);
  }

  ngAfterViewInit(): void {
    this._hrmSetupServiceProxy.getDataForLoad().subscribe((res)=>{
      debugger
      this.accountHrmSetup.LoanToPayable=res["loanToPayable"];
      this.accountHrmSetup.LoanToStSal=res["loanToStSal"];
      this.accountHrmSetup.AdvToPayable=res["advToPayable"];
      this.accountHrmSetup.AdvToStSal=res["advToStSal"];
      this.accountHrmSetup.Id=res["id"];
      this.chartofControlAccountName=res["advToStSalName"];
      this.chartofControlAccountName2=res["advToPayablName"];
      this.chartofControlAccountName3=res["loanToStSalName"];
      this.chartofControlAccountName4=res["loanToPayableName"];
    
      })
  }
  openSelectChartofControlModal(type:any) 
  {
    debugger;
    this.mode=type;
    this.target = "ChartOfAccount";
    this.FinanceLookupTableModal.id= this.accountHrmSetup.AdvToPayable;
    this.FinanceLookupTableModal.displayName = this.chartofControlAccountName;
   this.FinanceLookupTableModal.show(this.target, "true");
  }
   
    
  setChartofControlIdNull(type:any)
  {
      
    debugger
    this.mode=type;
    if(this.mode=="1")
    {
      this.accountHrmSetup.AdvToStSal = "";
      this.chartofControlAccountName = "";
     
     
    }else if(this.mode=="2")
    {
      this.accountHrmSetup.AdvToPayable = "";
      this.chartofControlAccountName2 = "";
      
    }else if(this.mode=="3")
    {
      this.accountHrmSetup.LoanToStSal = "";
      this.chartofControlAccountName3 = "";
      
    }else if(this.mode=="4")
    { 
      this.accountHrmSetup.LoanToPayable = "";
      this.chartofControlAccountName4 = "";
           
    }

  }

  getNewChartofControlId() 
  {
    debugger
    if(this.mode=="1"){
      this.accountHrmSetup.AdvToStSal = this.FinanceLookupTableModal.id;
      this.chartofControlAccountName = this.FinanceLookupTableModal.displayName;
    }else if(this.mode=="2"){
      this.accountHrmSetup.AdvToPayable = this.FinanceLookupTableModal.id;
      this.chartofControlAccountName2 = this.FinanceLookupTableModal.displayName;
    }else if(this.mode=="3"){
      this.accountHrmSetup.LoanToStSal = this.FinanceLookupTableModal.id;
      this.chartofControlAccountName3 = this.FinanceLookupTableModal.displayName;
    }else if(this.mode=="4"){
      this.accountHrmSetup.LoanToPayable = this.FinanceLookupTableModal.id;
      this.chartofControlAccountName4 = this.FinanceLookupTableModal.displayName;
    }
   
  }


  setChartofControlIdClear() {

    debugger
    this.accountHrmSetup.AdvToPayable = "";
    this.chartofControlAccountName = "";
    this.accountHrmSetup.AdvToStSal = "";
    this.chartofControlAccountName2 = "";
    this.accountHrmSetup.LoanToPayable = "";
    this.chartofControlAccountName3 = "";
    this.accountHrmSetup.LoanToStSal = "";
    this.chartofControlAccountName4 = "";
    this.flag=false;
    this.clearbtn=true;
  }

  getNewFinanceModal() {
    debugger;
    switch (this.target) {
      case "ChartOfAccount":
          this.getNewChartofControlId();
          break;

      default:
          break;
    }
  }
  save(): void 
  {
    this.saving=true;
    debugger;    
    this._hrmSetupServiceProxy.Create(this.accountHrmSetup)
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
