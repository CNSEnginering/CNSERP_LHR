import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies";
import { map } from "rxjs/operators";
import { GeticLotEditOutput,CreateOrEditLotNoDto, ICLOTDto, PagedResultDtoiclot } from "../dto/icLot-dto";

@Injectable({
    providedIn: "root",
})
export class iclotServiceProxy {
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
        this.url += "/api/services/app/ICLOT/GetAll?";
        this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (sorting !== undefined)
            this.url += "MaxSalaryMonthFilter=" + encodeURIComponent("" + sorting) + "&";
        if (skipCount !== undefined)
            this.url += "MaxSalaryYearFilter=" + encodeURIComponent("" + skipCount) + "&";
        this.url = this.url.replace(/[?&]$/, "");
        return this.http.get(this.url).pipe(map((response: any) => {
            console.log(response);
            return response["result"] as PagedResultDtoiclot;
        }));
    }

    getMaxDocId(){
        this.url = this.baseUrl;
        this.url += "/api/services/app/ICLOT/GetmaxDocNo";
        return this.http.get(this.url).pipe(map((response: any) => {
            debugger
            return response["result"];
        }));
    }
    delete(id: number) {
        debugger
        this.url = this.baseUrl;
        this.url += "/api/services/app/ICLOT/Delete?Id=" + id;
        return this.http.delete(this.url);
    }

    create(dto: CreateOrEditLotNoDto) {
        debugger;
        this.url = this.baseUrl;
        this.url += "/api/services/app/ICLOT/CreateOrEdit";
        return this.http.post(this.url, dto);
    }

    getDataForEdit(id: number) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/ICLOT/GetICLOTForEdit?Id=" + id;
        return this.http.get(this.url).pipe(map((response: any) => {
            debugger
            return response["result"] as GeticLotEditOutput;
        }));
    }
    getAmountforInvoice(id: number) {
        debugger
        this.url = this.baseUrl;
        this.url += "/api/services/app/ICLOT/GetDocNoAmount?id=" + id;
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
        this.url += "/api/services/app/ICLOT/GetSalaryLockToExcel?";
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