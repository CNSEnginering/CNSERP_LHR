import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ItemPricingDto } from '../dto/itemPricing-dto';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';

@Injectable({
  providedIn: 'root'
})
export class ItemPricingService {
  url: string = "";
  url_: string = "";
  data: any;
  baseUrl:string = "";
  constructor(private http: HttpClient, @Inject(API_BASE_URL) baseUrl?: string) {
    this.baseUrl = baseUrl;
  }

  getAll(
    filter: string,
    sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/ItemPricings/GetAll?";
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
    this.url += "/api/services/app/ItemPricings/GetPriceListStatusIfExists?";
    this.url += "PriceListChk=" + encodeURIComponent("" + priceList);
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  delete(id: string) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/ItemPricings/Delete?Id=" + id;
    return this.http.delete(this.url);
  }

  create(itemPricing: ItemPricingDto[]) {
    debugger
    this.url = this.baseUrl;
    this.url += "/api/services/app/ItemPricings/CreateOrEdit";
    return this.http.post(this.url, itemPricing);
  }

  getDataForEdit(id: string) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/ItemPricings/GetItemPricingForEdit?Id=" + id;
    return this.http.get(this.url);
  }


  checkItemIdAgainstPriceList(priceList: string, itemId: string) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/ItemPricings/GetItemIdCheckAgainstPriceList?priceList=" + priceList + "&itemId=" + itemId;
    return this.http.get(this.url);
  }

  GetItemPricingToExcel(
    filter: string, sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined
  ) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/ItemPricings/GetItemPricingToExcel?";
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

  CheckPriceListExists(priceList:string){
    this.url = this.baseUrl;
    this.url += "/api/services/app/ItemPricings/GetCheckPriceListExists?priceList=" + priceList;
    //this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url);
  }
}
