import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
// import { TransferDto } from '../dto/transfer-dto';
import { invoiceKnockOffDto } from '../dtos/invoiceKnockOff-dto';

@Injectable({
  providedIn: 'root'
})
export class invoiceKnockOffService {
  url: string = "";
  url_: string = "";
  data: any;
  baseUrl: string = "";
  constructor(private http: HttpClient, @Inject(API_BASE_URL) baseUrl?: string) {
    this.baseUrl = baseUrl;
  }

  getAll(
    filter: string,
    sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/OEINVKNOCKH/GetAll?";
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
    this.url += "/api/services/app/OEINVKNOCKH/Delete?Id=" + id;
    return this.http.delete(this.url);
  }

  create(dto: invoiceKnockOffDto) {
    debugger
    this.url = this.baseUrl;
    this.url += "/api/services/app/OEINVKNOCKH/CreateOrEdit";
    return this.http.post(this.url, dto);
  }

  getDataForEdit(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/OEINVKNOCKH/GetOEINVKNOCKHForEdit?Id=" + id;
    // this.url += "&type=" + type;
    return this.http.get(this.url);
  }

  GetOGPNo(
    filter: string,
    sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/OEQH/GetOGPNo?";
    this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
    if (sorting !== undefined)
      this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&";
    if (skipCount !== undefined)
      this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
    if (maxResultCount !== undefined)
      this.url += "MaxResultCount=" + encodeURIComponent("" + maxResultCount) + "&";

    this.url += "MaxGPTypeFilter=" + encodeURIComponent("" + 1) + "&";
    this.url += "MaxTypeIDFilter=" + encodeURIComponent("" + 2);
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  GetDataToExcel(
    filter: string, sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined
  ) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/OEQH/GetDataToExcel?";
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

  GetPostedInvoices(debtor: string, custID: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/OEINVKNOCKH/GetPostedInvoices?";
    this.url += "Debtor=" + encodeURIComponent("" + debtor) + "&";
    this.url += "CustId=" + encodeURIComponent("" + custID) + "&";
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  GetDocId() {
    this.url = this.baseUrl;
    this.url += "/api/services/app/OEINVKNOCKH/GetDocId";
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  processInvoice(dto: invoiceKnockOffDto) {
    debugger
    this.url = this.baseUrl;
    this.url += "/api/services/app/OEINVKNOCKH/ProcessInvoice";
    return this.http.post(this.url, dto);
  }
}
