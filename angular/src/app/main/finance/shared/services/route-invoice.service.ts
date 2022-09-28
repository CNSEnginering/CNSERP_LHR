import { ARINVHDto } from './../dto/arinvh-dto';
import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
@Injectable({
  providedIn: 'root'
})
export class RouteInvoiceService {

  url: string = "";
  url_: string = "";
  baseUrl: string = "";
  data: any;

  constructor(private http: HttpClient, @Inject(API_BASE_URL) baseUrl?: string) {
    this.baseUrl = baseUrl;
  }

  getAll(
    filter: string,
    sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/ARINVH/GetAll?";
    this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
    if (sorting !== undefined)
      this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&";
    if (skipCount !== undefined)
      this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
    if (maxResultCount !== undefined)
      this.url += "MaxResultCount=" + encodeURIComponent("" + maxResultCount);
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  delete(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/ARINVH/Delete?Id=" + id;
    return this.http.delete(this.url);
  }

  create(dto: ARINVHDto) {
    debugger
    this.url = this.baseUrl;
    this.url += "/api/services/app/ARINVH/CreateOrEdit";
    return this.http.post(this.url, dto);
  }

  GetDocId() {
    debugger
    this.url = this.baseUrl;
    this.url += "/api/services/app/ARINVH/GetDocId";
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  getDataForEdit(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/ARINVH/GetARINVHForEdit?Id=" + id;
    // this.url += "&type=" + type;
    return this.http.get(this.url);
  }

  GetPostedInvoices(routID: number, invDate: Date | string) {
    debugger
    this.url = this.baseUrl;
    this.url += "/api/services/app/ARINVH/GetPostedInvoices?";
    //this.url += "SaleTypeID=" + encodeURIComponent("" + saleTypeID) + "&";
    this.url += "RoutID=" + encodeURIComponent("" + routID) + "&";
    this.url += "InvDate=" + encodeURIComponent("" + moment(invDate).format("YYYY/MM/DD")) + "&";
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }
}
