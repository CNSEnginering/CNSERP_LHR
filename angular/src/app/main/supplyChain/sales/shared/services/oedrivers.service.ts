import { Inject, Injectable,Optional } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies";
import { map } from "rxjs/operators";
import { Oedriversdto,PagedResultDtocader } from '../dtos/oedriversdto';


@Injectable({
  providedIn: 'root'
})
export class OedriversService {
  private baseUrl: string;

  constructor(
    private http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) { 
    this.baseUrl = baseUrl ? baseUrl : "";
  }
  url: string = "";
  url_: string = "";
  getAll(
    filter: string,
    sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined,
  ) {
    debugger;
    this.url = this.baseUrl;
    this.url += "/api/services/app/OEDrivers/GetAll?";
    this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
      if (sorting !== undefined)
      this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&"; 
      if (skipCount !== undefined)
      this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&"; 
      if (maxResultCount !== undefined)
      this.url += "MaxResultCount=" + encodeURIComponent("" + maxResultCount) + "&"; 
 
      this.url = this.url.replace(/[?&]$/, "");
      return this.http.request("get", this.url);

  }
  create(dto: Oedriversdto) {
    debugger
    this.url = this.baseUrl;
    this.url += "/api/services/app/OEDrivers/CreateOrEdit";
    return this.http.post(this.url, dto);
  }
  getDataForEdit(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/OEDrivers/GetOEDriversForEdit?Id=" + id;
    return this.http.get(this.url).pipe(map((response: any) => {
       
        return response["result"] ;
    }));
}

delete(id: number) {
  this.url = this.baseUrl;
  this.url += "/api/services/app/OEDrivers/Delete?Id=" + id;
  return this.http.delete(this.url);
}
getDocNo(){
  debugger
 this.url=this.baseUrl;
 this.url+="/api/services/app/OEDrivers/GetMaxDocNo";
 this.url = this.url.replace(/[?&]$/, "");
 return this.http.get(this.url).pipe(map((response: any) => {
     return response["result"];
 }));     
}

}
