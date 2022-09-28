import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { SessionServiceProxy, CompanyProfilesServiceProxy } from '@shared/service-proxies/service-proxies';
import { ReportViewService } from './reportView.service';

@Component({
    templateUrl: './reportView.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ReportViewComponent extends AppComponentBase {
    reportServer: string;
    reportUrl: string;
    showParameters: string;
    parameters: any;
    language: string;
    width: number;
    height: number;
    toolbar: string;
    constructor(
        injector: Injector,
        private session: SessionServiceProxy,
        private _companyProfilesServiceProxy: CompanyProfilesServiceProxy,
        private _reportViewService: ReportViewService
    ) {
        super(injector);
    }



    ngOnInit() {
        this.getReport();
    }


    getReport() {
        this._reportViewService.GetReportDataForParams().subscribe(result => {
            debugger
            var params = JSON.parse(localStorage.getItem("rptObj"));
            params['ServerURL'] = result["result"][0].reportPath;
            params['CompanyName'] = result["result"][0].companyName;
            this.reportServer = 'http://rpt.e-resourceplanning.com/reportserver';
            this.reportUrl = 'ERP.Reports/' + localStorage.getItem("rptName");
            this.showParameters = "false";
            this.parameters = params;
            this.language = "en-us";
            this.width = 100;
            this.height = 100;
            this.toolbar = "true";
        });
    }
}

