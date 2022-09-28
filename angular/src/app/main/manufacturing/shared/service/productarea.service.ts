import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies";
import { map } from "rxjs/operators";
import { PagedResultDtoMFAREA, MFAREADto, GetMFAREAForEditOutput } from "../dto/productionarea.dto";

@Injectable({
    providedIn: "root",
})
export class ProductionAreaServiceProxy {
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
        sorting: string | null | undefined,
        skipCount: number | null | undefined,
        maxResultCount: number | null | undefined) {
        debugger;
        this.url = this.baseUrl;
        this.url += "/api/services/app/MFAREA/GetAll?";
        this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (sorting !== undefined)
            this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&";
        if (skipCount !== undefined)
            this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        if (maxResultCount !== undefined)
            this.url += "MaxResultCount=" + encodeURIComponent("" + maxResultCount);
        this.url = this.url.replace(/[?&]$/, "");
        return this.http.get(this.url).pipe(map((response: any) => {
            return response["result"] as PagedResultDtoMFAREA;
        }));
    }

    delete(id: number) {
        debugger
        this.url = this.baseUrl;
        this.url += "/api/services/app/MFAREA/Delete?Id=" + id;
        return this.http.delete(this.url);
    }

    create(dto: MFAREADto) {
        debugger;
        this.url = this.baseUrl;
        this.url += "/api/services/app/MFAREA/CreateOrEdit";
        return this.http.post(this.url, dto);
    }

    getDataForEdit(id: number) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/MFAREA/GetMFAREAForEdit?Id=" + id;
        return this.http.get(this.url).pipe(map((response: any) => {
            debugger
            return response["result"] as GetMFAREAForEditOutput;
        }));
    }


    GetDataToExcel(
        filter: string, sorting: string | null | undefined,
        skipCount: number | null | undefined,
        maxResultCount: number | null | undefined
    ) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/MFAREA/GetMFAREAToExcel?";
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