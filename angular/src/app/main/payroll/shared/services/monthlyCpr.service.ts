import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies";
import { map } from "rxjs/operators";
import { GetMonthlyCprForViewDto,CreateOrEditMonthlyCprDto, MonthlyCPRDto, PagedResultDtoMonthlyCpr, GetMonthlyCprEditOutput } from "../dto/monthlyCpr-dto";

@Injectable({
    providedIn: "root",
})
export class monthlyCprServiceProxy {
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
        monthFilter: number | null | undefined,
        yearFilter: number | null | undefined,
        //sorting: number | null | undefined,
        skipCount: number | null | undefined,
       ) {
        debugger;
        this.url = this.baseUrl;
        this.url += "/api/services/app/MonthlyCPR/GetAll?";
        this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (monthFilter !== undefined)
            this.url += "MaxSalaryMonthFilter=" + encodeURIComponent("" + monthFilter) + "&";
        if (yearFilter !== undefined)
            this.url += "MaxSalaryYearFilter=" + encodeURIComponent("" + yearFilter) + "&";
        if (skipCount !== undefined)
            this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        this.url = this.url.replace(/[?&]$/, "");
        return this.http.get(this.url).pipe(map((response: any) => {
            console.log(response);
            return response["result"] as PagedResultDtoMonthlyCpr;
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
        this.url += "/api/services/app/MonthlyCPR/Delete?Id=" + id;
        return this.http.delete(this.url);
    }

    create(dto: CreateOrEditMonthlyCprDto) {
        debugger;
        this.url = this.baseUrl;
        this.url += "/api/services/app/MonthlyCPR/CreateOrEdit";
        return this.http.post(this.url, dto);
    }

    getDataForEdit(id: number) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/MonthlyCPR/GetMonthlyCPRForEdit?Id=" + id;
        return this.http.get(this.url).pipe(map((response: any) => {
            return response["result"] as GetMonthlyCprEditOutput;
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
    
    GetTotalTaxAmount(salaryYear: number, salaryMonth: number) {
        let url_ =
            this.baseUrl +
            "/api/services/app/MonthlyCPR/GetTotalTaxAmount?SalaryYear=" +
            salaryYear +
            "&SalaryMonth=" +
            salaryMonth;
        url_ = url_.replace(/[?&]$/, "");
        return this.http.get(url_);
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