import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
import { TransferDto } from '../dto/transfer-dto';
import { AssemblyDto } from '../dto/assembly-dto';

@Injectable({
  providedIn: 'root'
})
export class AssemblyService {
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
    this.url += "/api/services/app/Assemblies/GetAll?";
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
    this.url += "/api/services/app/Assemblies/Delete?Id=" + id;
    return this.http.delete(this.url);
  }

  create(dto: AssemblyDto) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/Assemblies/CreateOrEdit";
    return this.http.post(this.url, dto);
  }

  getDataForEdit(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/Assemblies/GetAssemblyForEdit?Id=" + id;
    // this.url += "&type=" + type;
    return this.http.get(this.url);
  }

  GetOGPNo(
    filter: string,
    sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/Assemblies/GetOGPNo?";
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
    this.url += "/api/services/app/Assemblies/GetDataToExcel?";
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

  GetQtyInHand(itemId: string, locId: number, docId: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/Assemblies/GetQtyInHand?";
    this.url += "itemId=" + encodeURIComponent("" + itemId) + "&";
    this.url += "locId=" + encodeURIComponent("" + locId) + "&";
    this.url += "docId=" + encodeURIComponent("" + docId);
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  GetDocId() {
    this.url = this.baseUrl;
    this.url += "/api/services/app/Assemblies/GetDocId";
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }
}
