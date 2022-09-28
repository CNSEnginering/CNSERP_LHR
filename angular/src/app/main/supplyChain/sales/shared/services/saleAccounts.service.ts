import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponseBase, HttpResponse } from '@angular/common/http';
import { API_BASE_URL, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { saleAccountsDto } from '../dtos/saleAccounts-dto';


@Injectable({
  providedIn: 'root'
})
export class SaleAccountsService {
  url: string = "";
  url_: string = "";
  data: any;
  baseUrl: string = "";
  protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;
  constructor(private http: HttpClient, @Inject(API_BASE_URL) baseUrl?: string) {
    this.baseUrl = baseUrl;
  }

  getAll(
    filter: string,
    sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined) {
    debugger;
    this.url = this.baseUrl;
    this.url += "/api/services/app/SaleAccounts/GetAll?";
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
    debugger
    this.url = this.baseUrl;
    this.url += "/api/services/app/SaleAccounts/Delete?Id=" + id;
    return this.http.delete(this.url);
  }

  create(dto: saleAccountsDto) {
    debugger;
    this.url = this.baseUrl;
    this.url += "/api/services/app/SaleAccounts/CreateOrEdit";
    return this.http.post(this.url, dto);
  }

  getDataForEdit(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/SaleAccounts/GetOECOLLForEdit?Id=" + id;
    return this.http.get(this.url);
  }

  GetDataToExcel(
    filter: string, sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined
  ) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/SaleAccounts/GetDataToExcel?";
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



  GetTransAgainstLoc(locId: number, typeId: string) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/SaleAccounts/GetTransAgainstLoc?locId=" + locId + "&typeId=" + typeId;
    return this.http.get(this.url);
  }
}
