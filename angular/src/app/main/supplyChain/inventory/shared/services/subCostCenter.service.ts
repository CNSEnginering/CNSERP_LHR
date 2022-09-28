import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SubCostCenterDto } from '../dto/subCostCenter-dto';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';

@Injectable({
  providedIn: 'root'
})
export class SubCostCenterService {
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
    this.url += "/api/services/app/SubCostCenters/GetAll?";
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

  getAllSubCostCenterForLookupTable(
    filter: string,
    ccidFilter: string,
    sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/SubCostCenters/GetAllSubCostCenterForLookupTable?";
    this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
    this.url += "CCIDFilter=" + encodeURIComponent("" + ccidFilter) + "&";
    if (sorting !== undefined)
      this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&";
    if (skipCount !== undefined)
      this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
    if (maxResultCount !== undefined)
      this.url += "MaxResultCount=" + encodeURIComponent("" + maxResultCount);
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  checkCostCenterIdIfExists(ccId: string) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/SubCostCenters/GetcheckCostCenterIdIfExists?ccId=" + ccId;
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  delete(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/SubCostCenters/Delete?Id=" + id;
    return this.http.delete(this.url);
  }

  create(subCostCenterDto: SubCostCenterDto) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/SubCostCenters/CreateOrEdit";
    return this.http.post(this.url, subCostCenterDto);
  }

  getDataForEdit(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/SubCostCenters/GetSubCostCenterForEdit?Id=" + id;
    return this.http.get(this.url);
  }

  getMaxSubCostCenterId(id: string) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/SubCostCenters/GetSubCostCenterId?CCId=" + id;
    return this.http.get(this.url);
  }

   
  GetDataToExcel(
    filter: string, sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined
  ) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/SubCostCenters/GetDataToExcel?";
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
