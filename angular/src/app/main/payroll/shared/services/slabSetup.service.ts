import { Inject, Injectable, Optional } from "@angular/core";
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL } from "@shared/service-proxies/service-proxies";
import { url } from "inspector";
import { Observable } from "rxjs";
import { map } from 'rxjs/operators';
import { GetSlabSetupForEditOutput, PagedResultDtoOfGetSlabSetupForViewDto, SlabSetupDto } from "../dto/slabSetup-dto";

@Injectable({
    providedIn: 'root'
})
export class SlabSetupService {
    private http: HttpClient;
    private baseUrl: string;
    url: string

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }
    getAll(
        filter: string,
        sorting: string | null | undefined,
        skipCount: number | null | undefined,
        maxResultCount: number | null | undefined) {
        debugger;
        this.url = this.baseUrl;
        this.url += "/api/services/app/SlabSetup/GetAll?";
        this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (sorting !== undefined)
            this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&";
        if (skipCount !== undefined)
            this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        if (maxResultCount !== undefined)
            this.url += "MaxResultCount=" + encodeURIComponent("" + maxResultCount);
        this.url = this.url.replace(/[?&]$/, "");
        return this.http.get(this.url).pipe(map((response: any) => {
            return response["result"] as PagedResultDtoOfGetSlabSetupForViewDto;
        }));
    }

    delete(id: number) {
        debugger
        this.url = this.baseUrl;
        this.url += "/api/services/app/SlabSetup/Delete?Id=" + id;
        return this.http.delete(this.url);
    }

    createOrEdit(dto: SlabSetupDto) {
        debugger;
        this.url = this.baseUrl;
        this.url += "/api/services/app/SlabSetup/CreateOrEdit";
        return this.http.post(this.url, dto);
    }

    getDataForEdit(id: number) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/SlabSetup/GetSlabSetupForEdit?Id=" + id;
        return this.http.get(this.url).pipe(map((response: any) => {
            debugger
            return response["result"] as GetSlabSetupForEditOutput;
        }));
    }


    GetDataToExcel(
        filter: string, sorting: string | null | undefined,
        skipCount: number | null | undefined,
        maxResultCount: number | null | undefined
    ) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/SlabSetup/GetSlabSetupToExcel?";
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
