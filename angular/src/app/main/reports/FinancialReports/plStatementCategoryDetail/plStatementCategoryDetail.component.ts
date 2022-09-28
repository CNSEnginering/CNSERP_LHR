import { Component, OnInit, Injector, ViewChild, AfterViewInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
import * as moment from 'moment';
import { FiscalDateService } from '@app/shared/services/fiscalDate.service';
import { Route, Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'plStatementCategoryDetail',
  templateUrl: './plStatementCategoryDetail.component.html'
})
export class PlStatementCategoryDetailComponent extends AppComponentBase implements 
OnInit,AfterViewInit{

  @ViewChild("reportView", { static: true })
  reportView: ReportviewrModalComponent;

  fromDate;
  toDate;
  category;
  constructor(
    injector: Injector,
    private _reportService: FiscalDateService,
    private _route: ActivatedRoute
  ) {
    super(injector)
  }
  ngAfterViewInit(): void {
    this._route.queryParams.subscribe(params => {
      this.category = params['category'];
      this.fromDate = params['fromDate'];
      this.toDate = params['toDate'];
      setTimeout(()=>{
        this.getReport()
      },5000); 
    });
  }
  
  
  ngOnInit(): void {

  }
  
  getReport() {
    let repParams = "";
    repParams += this.fromDate + "$";
    repParams += this.toDate + "$";
    repParams += this.category + "$";
    repParams = repParams.replace(/[?$]&/, "");
    this.reportView.show("PlStatementCategoryDetail", repParams);
  }


}
