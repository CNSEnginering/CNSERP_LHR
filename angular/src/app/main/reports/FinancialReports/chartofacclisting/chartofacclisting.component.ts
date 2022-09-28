import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ChartOfACListingReportServiceProxy, ComboboxItemDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import * as _ from 'lodash';
import * as moment from 'moment';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { LegderTypeComboboxService } from '@app/shared/common/legdertype-combobox/legdertype-combobox.service';
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';

@Component({
    templateUrl: './chartofacclisting.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ChartOfAccountListingReportComponent extends AppComponentBase implements OnInit {
    


    @ViewChild('fromAccountFinder', { static: true }) fromAccountFinder: FinanceLookupTableModalComponent;
    @ViewChild('toAccountFinder', { static: true }) toAccountFinder: FinanceLookupTableModalComponent;
    @ViewChild('reportView', { static: true }) reportView: ReportviewrModalComponent;
    chartOfAcc = "2";

    reportServer: string;
    reportUrl: string;
    showParameters: string;
    parameters: any;
    language: string;
    width: number;
    height: number;
    toolbar: string;
    rptObj: any;
    fromAccount: any;
    slType=0;
    fromAccountName: string;
    toAccount: any;
    toAccountName: any;
    ledgerTypes: ComboboxItemDto[];

    report="";

    constructor(
        injector: Injector,
        private _chartOfACListingReportServiceProxy: ChartOfACListingReportServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _LegderTypeComboboxService: LegderTypeComboboxService,
        private route: Router

    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.fromAccount = '00-000-00-0000';
        this.fromAccountName = '';
        this.toAccount = '99-999-99-9999';
        this.toAccountName = '';
        this.getLedgerTypes();
    }

    getReport() {
      let dxrep = false;
      let repParams = ''
      switch (this.chartOfAcc) {
        case "1":
          this.report = "ChartOfACListing";
          if (this.slType !== undefined)
            repParams += encodeURIComponent("" + this.slType) + "$";
          if (this.fromAccount !== undefined)
            repParams += encodeURIComponent("" + this.fromAccount) + "$";
          if (this.toAccount !== undefined)
            repParams += encodeURIComponent("" + this.toAccount) + "$";
  
          repParams = repParams.replace(/[?$]&/, "");
          dxrep = true;
          break;
        case "2":
          this.report = "SubledgerListing";
          if (this.slType !== undefined)
            repParams += encodeURIComponent("" + this.slType) + "$";
  
          repParams = repParams.replace(/[?$]&/, "");
          dxrep = true;
          break;
      
        default:
          break;
      }

      if (dxrep) {
        this.reportView.show(this.report, repParams)
      }
      // else {
  
  
      //   localStorage.setItem('rptObj', JSON.stringify(this.rptObj));
      //   localStorage.setItem('rptName', this.report);
  
      //   this.route.navigateByUrl('/app/main/reports/ReportView');
      // }
        // if (this.chartOfAcc == "1") {
          

        //     this.rptObj = {
        //         "slType":this.slType,
        //         "fromAccount":this.fromAccount,
        //         "toAccount": this.toAccount
        //     };
        //     localStorage.setItem('rptObj', JSON.stringify(this.rptObj));
        //     localStorage.setItem('rptName', 'ChartOfACListing');
        //     this.route.navigateByUrl('/app/main/reports/ReportView');
        // } else if (this.chartOfAcc == "2") {

        //     this.rptObj = {
        //         "tenantId": this.appSession.tenantId,
        //         "slType":this.slType
        //     };
        //     localStorage.setItem('rptObj', JSON.stringify(this.rptObj));
        //     localStorage.setItem('rptName', 'SubledgerListing');
        //     this.route.navigateByUrl('/app/main/reports/ReportView');

        // } 

    }
    selectFromAccount() {
        this.fromAccountFinder.id = this.fromAccount;
        this.fromAccountFinder.displayName = this.fromAccountName;
        this.fromAccountFinder.show("ChartOfAccount");
      }
    
    
      setFromAccount() {
        this.fromAccount = '00-000-00-0000';
        this.fromAccountName = '';
        
      }
    
      getToAccount(){
        this.toAccount = this.toAccountFinder.id;
        this.fromAccountName = this.toAccountFinder.displayName;
      }
    
      getFromAccount() {
       
          this.fromAccount = this.fromAccountFinder.id;
          this.fromAccountName = this.fromAccountFinder.displayName;
          this.toAccount = this.toAccountFinder.id;
          this.toAccountName = this.toAccountFinder.displayName;
    
      }
    
      selectToAccount() {
        this.toAccountFinder.id = this.toAccount;
        this.toAccountFinder.displayName = this.toAccountName;
        this.toAccountFinder.show("ChartOfAccount");
      }
    
    
      setToAccount() {
        this.toAccount = '99-999-99-9999';
        this.toAccountName = '';
      }

      getLedgerTypes() {
        this._LegderTypeComboboxService.getLedgerTypesForCombobox('').subscribe(res => {
          debugger;
          this.ledgerTypes = res.items
        })
      }
}

