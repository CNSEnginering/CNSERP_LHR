import { Component, OnInit, ViewChild, Output, EventEmitter, Injector, Input, ChangeDetectorRef, Renderer2, ElementRef, ViewEncapsulation, Inject, Optional } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ReportViewService } from './reportView.service';
import DevExpress from '@devexpress/analytics-core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { HttpClient } from '@angular/common/http';
import { AppConsts } from '@shared/AppConsts';
import { Router } from '@angular/router';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
@Component({
  selector: 'app-reportviewr-modal',
  templateUrl: './reportviewr-modal.component.html',
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None,
  styleUrls: [
    // "../../../../../node_modules/jquery-ui/themes/base/all.css",
    //"../../../../../node_modules/devextreme/dist/css/dx.common.css",
    //"../../../../../node_modules/devextreme/dist/css/dx.light.css",
    "../../../../../node_modules/@devexpress/analytics-core/dist/css/dx-analytics.common.css",
    "../../../../../node_modules/@devexpress/analytics-core/dist/css/dx-analytics.light.css",
    "../../../../../node_modules/devexpress-reporting/dist/css/dx-webdocumentviewer.css"
  ]
})
export class ReportviewrModalComponent extends AppComponentBase {

  @ViewChild('createOrEditModal', { static: false }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  @Input() rptobj: string;
  @Input() rptName: string;
  active: boolean;

  // URI of your backend project.
  hostUrl: string;  //='http://localhost:22742/';

  // Use this line if you use an ASP.NET MVC backend
  //invokeAction: string = 'WebDocumentViewer/Invoke';
  // Uncomment this line if you use an ASP.NET Core backend
  invokeAction: string = 'DXXRDV';

  reportServer: string;
  reportUrl: string;
  showParameters: string;
  parameters: any;
  language: string;
  width: number;
  height: number;
  toolbar: string;
  showReport: boolean;
  private reportName: string;
  param: string;
  baseUrl: string;
  constructor(
    injector: Injector,
    private router: Router,
    @Inject(API_BASE_URL) baseUrl?: string
  ) {
   
    super(injector);
    this.hostUrl = AppConsts.remoteServiceBaseUrl + '/';
    this.baseUrl = baseUrl;
  }

  // ngOnInit() {
  //   this.show();
  // }


  show(koReportURL?: string, PARAM: any = ''): void {
    debugger;
    this.reportName = koReportURL;
    this.active = false;
    this.param = PARAM;
    // this._reportViewService.GetReportDataForParams().subscribe(result => {
    //   debugger
    //   var params = JSON.parse(this.rptobj); //JSON.parse(localStorage.getItem("rptObj"));
    //   params['ServerURL'] = result["result"][0].reportPath;
    //   params['CompanyName'] = result["result"][0].companyName;
    //   this.reportServer = 'http://rpt.e-resourceplanning.com/reportserver';
    //   this.reportUrl = 'ERP.Reports/' + this.rptName; //localStorage.getItem("rptName");
    //   this.showParameters = "false";
    //   this.parameters = params;
    //   this.language = "en-us";
    //   this.width = 100;
    //   this.height = 100;
    //   this.toolbar = "true";
    //   this.showReport = true;
    // });


    debugger
    DevExpress.Analytics.Utils.ajaxSetup.ajaxSettings = {
      headers: { 'Authorization': 'Bearer ' + abp.auth.getToken(), 'Abp.TenantId': this.appSession.tenantId.toString() }
    };
    
    
    if (PARAM == '') {
      this.reportUrl = koReportURL;
    }
    else {
      this.reportUrl = koReportURL + "?" + PARAM;
    }
    this.active = true;
    if(koReportURL != "")
    {
      this.modal.show();
    }
  }

  onDemandReportPreview(params) {
    debugger
    var _url = window.location.hostname + ":" + window.location.port;
    debugger
    let repParams = "";
    if (this.reportName == "GeneralLedger") {
      repParams += params.args.GetBrickText().toString().split("-")[0] + "$";
      repParams += null + "$";
      repParams += null + "$";
      repParams += 0 + "$";
      repParams += 0 + "$";
      repParams += 99999 + "$";
      repParams += params.args.GetBrickText().toString().split("-")[1] + "$";
      repParams += params.args.GetBrickText().toString().split("-")[1] + "$";
      repParams += "PKR" + "$";
      repParams += 1 + "$";
      repParams += "Both" + "$";
      repParams = repParams.replace(/[?$]&/, "");
      this.show("CashReceipt", repParams);
    }
    else if (this.reportName == "PLSTATMENT") {
      var date = this.param.split("$");
      window.open("http://" + _url + "/app/main/reports/FinancialReports/plStatementCategoryDetail?category="
        + params.args.GetBrickText().toString()
        + "&fromDate=" + date[0]
        + "&toDate=" + date[1]
        , "_blank");
    }
    else if (this.reportName == "PlStatementCategoryDetail") {
      var date = this.param.split("$");
      window.open("http://" + _url + "/app/main/reports/FinancialReports/plStatementVoucherDetail?account="
        + params.args.GetBrickText().toString()
        + "&fromDate=" + date[0]
        + "&toDate=" + date[1]
        , "_blank");
    }
    else if (this.reportName == "PlStatementVoucherDetail") {
      var data = params.args.GetBrickText().toString().split("-");
      window.open("http://" + _url + "/app/main/reports/FinancialReports/voucherpring-report?docNo="
        + data[0]
        + "&date=" + data[2]
        + "&bookId=" + data[1]
        , "_blank");
    }
  }


  defaultZoom(params): void {
    console.log(params);
    debugger;
    params.args.reportPreview.zoom(1)
  }

  close() {
    this.active = false;
    this.showReport = false;
    this.modal.hide();
  }
}
