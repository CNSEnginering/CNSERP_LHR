import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';

export class ReportViewService {
  url: string = "";
  url_: string = "";
  data: any;
  baseUrl: string = "";
  constructor(private http: HttpClient, @Inject(API_BASE_URL) baseUrl?: string) {
    this.baseUrl = baseUrl;
  }

  GetReportDataForParams() {
    this.url = this.baseUrl;
    this.url += "/api/services/app/CompanyProfiles/GetReportDataForParams";
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }
}
