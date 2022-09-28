import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
import { CaderHead } from '../dto/CaderH-dto';
import { Cader_link_DDto } from '../dto/CaderD-dto';

@Injectable({
  providedIn: 'root'
})
export class CaderHDService {
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
    this.url += "/api/services/app/Cader_link_H/GetAll?";
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
  getList(){
    this.url = this.baseUrl;
    this.url += "/api/services/app/Cader_link_H/GetCaderList";
    // this.url += "&type=" + type;
    return this.http.get(this.url);
  }
  getPayTypeList(){
    this.url = this.baseUrl;
    this.url += "/api/services/app/Cader_link_H/GetPayTypeList";
    // this.url += "&type=" + type;
    return this.http.get(this.url);
  }

  delete(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/Cader_link_H/Delete?Id=" + id;
    return this.http.delete(this.url);
  }

  create(dto: CaderHead) {
    debugger
    this.url = this.baseUrl;
    this.url += "/api/services/app/Cader_link_H/CreateOrEdit";
    return this.http.post(this.url, dto);
  }

  getDataForEdit(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/Cader_link_H/GetCader_link_HForEdit?Id=" + id;
    // this.url += "&type=" + type;
    return this.http.get(this.url);
  }

 
}
