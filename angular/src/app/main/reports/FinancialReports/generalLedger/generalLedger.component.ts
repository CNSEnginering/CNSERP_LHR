import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ChartOfACListingReportServiceProxy } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './generalLedger.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class GeneralLedgerReportComponent extends AppComponentBase {


    chartOfAcc="";

    reportServer: string ;
    reportUrl: string ;
    showParameters: string; 
    parameters: any ;
    language: string ;
    width: number ;
    height: number ;
    toolbar: string;

    constructor(
        injector: Injector,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,

    ) {
        super(injector);
    }

    getReport(): void {
        debugger
        this.language = "en-us";
        this.width = 100;
        this.height = 100;
        this.toolbar = "true";
        if(this.chartOfAcc=="1"){
            this.reportServer = 'http://localhost:22742/reportserver';
            this.reportUrl= 'ERP.Reports/SubledgerListing';
            this.showParameters = "true"; 
            this.parameters = {
                "tenantId" : this.appSession.tenantId
            };
        }else if(this.chartOfAcc=="2"){
            this.reportServer = 'http://localhost:22742/reportserver';
            this.reportUrl= 'ERP.Reports/ChartOfACListing';
            this.showParameters = "true"; 
            this.parameters = {
                "tenantId" : this.appSession.tenantId
            };
        }
    }
}

