import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
// import { TransferDto } from '../dto/transfer-dto';
import { CreateOrEditCSUserLocHDto,CreateOrEditCSUserLocDDto } from '../dto/UserLoc-dto';

@Injectable({
  providedIn: 'root'
})
export class userLocService {
  url: string = "";
  url_: string = "";
  data: any;
  baseUrl: string = "";
  constructor(private http: HttpClient, @Inject(API_BASE_URL) baseUrl?: string) {
    this.baseUrl = baseUrl;
  }

  getAll(
    filter: string,
    
    sorting?: string | null | undefined, 
    skipCount?: number | null | undefined, maxResultCount?: number | null | undefined
   ) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/CSUserLocH/GetAll?";
    this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
 
    if (sorting !== undefined)
    this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&"; 
    if (skipCount !== undefined)
    this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&"; 
    if (maxResultCount !== undefined)
    this.url += "MaxResultCount=" + encodeURIComponent("" + maxResultCount) + "&"; 
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  getUserInfo(name: string | undefined) {
    debugger;
    let url_ = this.baseUrl + "/api/services/app/GLSecurityHeader/GetUserInfo?";
    if (name !== undefined)
        url_ += "Name=" + encodeURIComponent("" + name) + "&"; 
    url_ = url_.replace(/[?&]$/, "");
    return this.http.get(url_);
    
  }
  getUserLocation(userId:any,type:any){
    debugger
    let url_ = this.baseUrl + "/api/services/app/CSUserLocH/GetUserLoc?";
    if (userId !== undefined)
        url_ += "userId=" + encodeURIComponent("" + userId) + "&";
    if (type !== undefined)
        url_ += "type=" + encodeURIComponent("" + type) + "&"; 
    url_ = url_.replace(/[?&]$/, "");
    return this.http.get(url_);
  }
  delete(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/CSUserLocH/Delete?Id=" + id;
    return this.http.delete(this.url);
  }

  create(dto: CreateOrEditCSUserLocHDto) {
   
    this.url = this.baseUrl;
    this.url += "/api/services/app/CSUserLocH/CreateOrEdit";
    return this.http.post(this.url, dto);
  }

  getDataForEdit(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/CSUserLocH/GetCSUserLocHForEdit?Id=" + id;
    // this.url += "&type=" + type;
    return this.http.get(this.url);
  }

  getSaleHeaderRecord(locID: number,saleNo: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/CSUserLocH/GetSaleNoHeaderData?locID=" + locID;
    this.url += "&saleNo=" + saleNo;
    return this.http.get(this.url);
  }

  getSaleDetailRecord(detId: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/CSUserLocH/GetOESALEDData?detId=" + detId;
    return this.http.get(this.url);
  }

  



}
