import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies";
import { map } from "rxjs/operators";
import { GetsalarylockEditOutput,CreateOrEditsalarylockDto, salarylockDTo, PagedResultDtosalarylock } from "../dto/salarylock-dto";

@Injectable({
    providedIn: "root",
})
export class SalarylockServiceProxy {
    private baseUrl: string;
    protected jsonParseReviver:
        | ((key: string, value: any) => any)
        | undefined = undefined;
    constructor(
        private http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    url: string = "";

    getAll(
        filter: string,
        sorting: number | null | undefined,
        skipCount: number | null | undefined,
       ) {
        debugger;
        this.url = this.baseUrl;
        this.url += "/api/services/app/SalaryLock/GetAll?";
        this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (sorting !== undefined)
            this.url += "MaxSalaryMonthFilter=" + encodeURIComponent("" + sorting) + "&";
        if (skipCount !== undefined)
            this.url += "MaxSalaryYearFilter=" + encodeURIComponent("" + skipCount) + "&";
        this.url = this.url.replace(/[?&]$/, "");
        return this.http.get(this.url).pipe(map((response: any) => {
            console.log(response);
            return response["result"] as PagedResultDtosalarylock;
        }));
    }

    getMaxDocId(){
        this.url = this.baseUrl;
        this.url += "/api/services/app/SalaryLock/GetmaxDocNo";
        return this.http.get(this.url).pipe(map((response: any) => {
            debugger
            return response["result"];
        }));
    }
    delete(id: number) {
        debugger
        this.url = this.baseUrl;
        this.url += "/api/services/app/SalaryLock/Delete?Id=" + id;
        return this.http.delete(this.url);
    }

    create(dto: CreateOrEditsalarylockDto) {
        debugger;
        this.url = this.baseUrl;
        this.url += "/api/services/app/SalaryLock/CreateOrEdit";
        return this.http.post(this.url, dto);
    }

    getDataForEdit(id: number) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/SalaryLock/GetSalaryLockForEdit?Id=" + id;
        return this.http.get(this.url).pipe(map((response: any) => {
            debugger
            return response["result"] as GetsalarylockEditOutput;
        }));
    }
    getAmountforInvoice(id: number) {
        debugger
        this.url = this.baseUrl;
        this.url += "/api/services/app/SalaryLock/GetDocNoAmount?id=" + id;
        return this.http.get(this.url).pipe(map((response: any) => {
            debugger
            return response["result"];
        }));
    }
    

    GetDataToExcel(
        filter: string, sorting: string | null | undefined,
        skipCount: string | null | undefined,
        maxResultCount: number | null | undefined
    ) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/SalaryLock/GetSalaryLockToExcel?";
        this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (sorting !== undefined)
            this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&";
        if (skipCount !== undefined)
            this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        if (maxResultCount !== undefined)
            this.url += "MaxResultCount=" + encodeURIComponent("" + maxResultCount);
        this.url = this.url.replace(/[?&]$/, "");
        return this.http.request("get", this.url);
    }

}