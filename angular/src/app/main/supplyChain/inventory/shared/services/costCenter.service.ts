import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CostCenterDto } from '../dto/costCenter-dto';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';

@Injectable({
  providedIn: 'root'
})
export class CostCenterService {
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
    maxResultCount: number | null | undefined,
    maxActiveFilter: string | null | undefined
  ) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/CostCenters/GetAll?";
    this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
    if (sorting !== undefined)
      this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&";
    if (skipCount !== undefined)
      this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
    if (maxResultCount !== undefined)
      this.url += "MaxResultCount=" + encodeURIComponent("" + maxResultCount)+ "&";
    if (maxActiveFilter !== undefined)
      this.url += "MaxActiveFilter=" + encodeURIComponent("" + maxActiveFilter);

    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  checkCostCenterIdIfExists(ccId: string) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/CostCenters/GetcheckCostCenterIdIfExists?ccId=" + ccId;
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  delete(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/CostCenters/Delete?Id=" + id;
    return this.http.delete(this.url);
  }

  create(costCenterDto: CostCenterDto) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/CostCenters/CreateOrEdit";
    return this.http.post(this.url, costCenterDto);
  }

  getDataForEdit(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/CostCenters/GetCostCenterForEdit?Id=" + id;
    return this.http.get(this.url);
  }


  GetDataToExcel(
    filter: string, sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined
  ) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/CostCenters/GetDataToExcel?";
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
