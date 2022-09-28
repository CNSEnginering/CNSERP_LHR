import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PriceListDto } from '../dto/priceList-dto';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';

@Injectable({
  providedIn: 'root'
})
export class PriceListService {
  url: string = "";
  url_: string = "";
  baseUrl:string = "";
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
    this.url += "/api/services/app/PriceLists/GetAll?";
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

  activeListOfPriceList(
    filter: string,
    sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/PriceLists/GetActiveListOfPriceList?";
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

  priceListCheckIfExists(priceList: string) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/PriceLists/GetPriceListStatusIfExists?";
    this.url += "PriceListChk=" + encodeURIComponent("" + priceList);
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  delete(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/PriceLists/Delete?Id=" + id;
    return this.http.delete(this.url);
  }

  create(priceList: PriceListDto) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/PriceLists/CreateOrEdit";
    return this.http.post(this.url, priceList);
  }

  getDataForEdit(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/PriceLists/GetPriceListForEdit?Id=" + id;
    return this.http.get(this.url);
  }

  
  GetDataToExcel(
    filter: string, sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined
  ) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/PriceLists/GetDataToExcel?";
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
}
