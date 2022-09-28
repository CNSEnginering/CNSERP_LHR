import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
import { InventoryGlLinkDto } from '../dto/inventory-glLink-dto';
@Injectable({
  providedIn: 'root'
})
export class InventoryGlLinkService {
  url: string = "";
  url_: string = "";
  data: any;
  baseUrl: string = "";
  constructor(private http: HttpClient, @Inject(API_BASE_URL) baseUrl?: string) {
    this.baseUrl = baseUrl;
  }

  getAll(filter: string, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/InventoryGlLinks/GetAll?";
    this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
    if (sorting !== undefined)
      this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&";
    if (skipCount !== undefined)
      this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
    if (maxResultCount !== undefined)
      this.url += "MaxResultCount=" + encodeURIComponent("" + maxResultCount);
    debugger
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  delete(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/InventoryGlLinks/Delete?Id=" + id;
    return this.http.delete(this.url);
  }

  create(priceList: InventoryGlLinkDto) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/InventoryGlLinks/CreateOrEdit";
    return this.http.post(this.url, priceList);
  }
  GetGLlinkSeg() {
    this.url = this.baseUrl;
    this.url += "/api/services/app/InventoryGlLinks/GetGLlinkSeg";
    return this.http.get(this.url);
  }

  GetSegIdAgainstLoc(locId: number, segId: string) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/InventoryGlLinks/GetSegIdAgainstLoc?locId=" + locId + "&seg=" + segId;
    return this.http.get(this.url);
  }

  GetInventoryGlLinkForEdit(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/InventoryGlLinks/GetInventoryGlLinkForEdit?id=" + id;
    return this.http.get(this.url);
  }

  GetInventoryGlLinksToExcel(
    filter: string, sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined
  ) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/InventoryGlLinks/GetInventoryGlLinksToExcel?";
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
