import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies";
import { map } from "rxjs/operators";
import { PagedResultDtocader } from "../dto/cader-dto";

import { CreateOrEdithrmSetupDto, hrmSetupDto } from "../dto/hrmSetup-dto";

@Injectable({
    providedIn: "root",
})
export class HrmSetupServiceProxy
{
      private baseUrl: string;
      protected jsonParseReviver:
        | ((key: string, value: any) => any)
        | undefined = undefined;
      constructor(
        private http: HttpClient,
        @Optional () @Inject(API_BASE_URL) baseUrl?: string
      ) {
        this.baseUrl = baseUrl ? baseUrl : "";
        }
        url: string = "";
        getAll(
            filter: string,
            sorting: number | null | undefined,
            skipCount: number | null | undefined,
           )
        {
            debugger;
            this.url = this.baseUrl;
            this.url += "/api/services/app/HrmSetup/GetAll?";
            this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
            if (sorting !== undefined)
            this.url += "MaxSalaryMonthFilter=" + encodeURIComponent("" + sorting) + "&";
        if (skipCount !== undefined)
            this.url += "MaxSalaryYearFilter=" + encodeURIComponent("" + skipCount) + "&";
        this.url = this.url.replace(/[?&]$/, "");
        return this.http.get(this.url).pipe(map((response: any) => {
            console.log(response);
            return response["result"] as PagedResultDtocader;
        }));
       }   

      delete(id: number) {
        debugger
        this.url = this.baseUrl;
        this.url += "/api/services/app/HrmSetup/Delete?Id=" + id;
        return this.http.delete(this.url);
      }

    
      Create(dto: any) {
        debugger;
        this.url = this.baseUrl;
        this.url += "/api/services/app/HrmSetup/CreateOrEdit";
        return this.http.post(this.url, dto);
      }
      getDataForLoad() {
        this.url = this.baseUrl;
        this.url += "/api/services/app/HrmSetup/GetsetupForLoad";
        return this.http.get(this.url).pipe(map((response: any) => {
           
            return response["result"];
        }));
       }
      getDataForEdit(id: number) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/HrmSetup/GetCaderForEdit?Id=" + id;
        return this.http.get(this.url).pipe(map((response: any) => {
           
            return response["result"];
        }));
       }
}