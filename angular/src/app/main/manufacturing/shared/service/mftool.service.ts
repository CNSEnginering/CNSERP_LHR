import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies";
import { map } from "rxjs/operators";
import { PagedResultDtoMFtoolSET, MFtoolSETDto, GetMFtoolSETDtoForEditOutput } from "../dto/mftool.dto";

@Injectable({
    providedIn: "root",
})
export class mftoolServiceProxy {
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
        this.url += "/api/services/app/MFTOOL/GetAll?";
        this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (sorting !== undefined)
            this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&";
        if (skipCount !== undefined)
            this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        if (maxResultCount !== undefined)
            this.url += "MaxResultCount=" + encodeURIComponent("" + maxResultCount);
        this.url = this.url.replace(/[?&]$/, "");
        return this.http.get(this.url).pipe(map((response: any) => {
            
            return response["result"] as PagedResultDtoMFtoolSET;
        }));
    }

    delete(id: number) {
        debugger
        this.url = this.baseUrl;
        this.url += "/api/services/app/MFTOOL/Delete?Id=" + id;
        return this.http.delete(this.url);
    }

    create(dto: MFtoolSETDto) {
        debugger;
        this.url = this.baseUrl;
        this.url += "/api/services/app/MFTOOL/CreateOrEdit";
        return this.http.post(this.url, dto);
    }

    getDataForEdit(id: number) {
        debugger
        this.url = this.baseUrl;
        this.url += "/api/services/app/MFTOOL/GetMFTOOLForEdit?Id=" + id;
        return this.http.get(this.url).pipe(map((response: any) => {
            
            return response["result"] as GetMFtoolSETDtoForEditOutput;
        }));
    }


    GetDataToExcel(
        filter: string, sorting: string | null | undefined,
        skipCount: number | null | undefined,
        maxResultCount: number | null | undefined
    ) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/MFTOOL/GetMFTOOLToExcel?";
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