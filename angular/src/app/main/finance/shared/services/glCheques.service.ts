import { Injectable, Inject } from "@angular/core";
import {
    HttpClient,
    HttpHeaders,
    HttpResponseBase,
    HttpResponse
} from "@angular/common/http";
import {
    API_BASE_URL,
    blobToText,
    throwException,
    VoucherEntryDto
} from "@shared/service-proxies/service-proxies";
import {
    mergeMap as _observableMergeMap,
    catchError as _observableCatch
} from "rxjs/operators";
import {
    Observable,
    throwError as _observableThrow,
    of as _observableOf
} from "rxjs";
import { GlChequesDto } from "../dto/GlCheques-dto";
// import { saleAccountsDto } from '../dtos/saleAccounts-dto';

@Injectable({
    providedIn: "root"
})
export class GlChequesService {
    url: string = "";
    url_: string = "";
    data: any;
    baseUrl: string = "";
    protected jsonParseReviver:
        | ((key: string, value: any) => any)
        | undefined = undefined;
    constructor(
        private http: HttpClient,
        @Inject(API_BASE_URL) baseUrl?: string
    ) {
        this.baseUrl = baseUrl;
    }

    getAll(
        filter: string,
        sorting: string | null | undefined,
        skipCount: number | null | undefined,
        maxResultCount: number | null | undefined,
        chequeStatusFilter: string | null | undefined,
        TypeIDFilter :number | null| undefined
    ) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/GlCheques/GetAll?";
        this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (sorting !== undefined)
            this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&";
        if (skipCount !== undefined)
            this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        if (maxResultCount !== undefined)
            this.url +=
                "MaxResultCount=" +
                encodeURIComponent("" + maxResultCount) +
                "&";
        if (chequeStatusFilter !== undefined)
            this.url +=
                "ChequeStatusFilter=" +
                encodeURIComponent("" + chequeStatusFilter) + "&";
        if (TypeIDFilter !== undefined)
            this.url +=
                "TypeIDFilter=" +
                encodeURIComponent("" + TypeIDFilter);

        this.url_ = this.url.replace(/[?&]$/, "");
        return this.http.request("get", this.url_);
    }

    // delete(id: number) {
    //   debugger
    //   this.url = this.baseUrl;
    //   this.url += "/api/services/app/GlCheques/Delete?Id=" + id;
    //   return this.http.delete(this.url);
    // }

    delete(id: number) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/GlCheques/Delete?Id=" + id;
        return this.http.delete(this.url);
    }

    create(dto: GlChequesDto) {
        debugger;
        this.url = this.baseUrl;
        this.url += "/api/services/app/GlCheques/CreateOrEdit";
        return this.http.post(this.url, dto);
    }

    ProcessVoucherEntry(dto: VoucherEntryDto){
        debugger;
        this.url = this.baseUrl;
        this.url += "/api/services/app/VoucherEntry/ProcessVoucherEntry";
        return this.http.post(this.url, dto);
    }

    getDataForEdit(id: number) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/GlCheques/GetGlChequeForEdit?Id=" + id;
        return this.http.get(this.url);
    }

    GetDataToExcel(
        filter: string,
        sorting: string | null | undefined,
        skipCount: number | null | undefined,
        maxResultCount: number | null | undefined
    ) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/GlCheques/GetDataToExcel?";
        this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (sorting !== undefined)
            this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&";
        if (skipCount !== undefined)
            this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        if (maxResultCount !== undefined)
            this.url +=
                "MaxResultCount=" + encodeURIComponent("" + maxResultCount);
        this.url_ = this.url.replace(/[?&]$/, "");
        return this.http.request("get", this.url_);
    }

    getDocNo(type:string) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/GlCheques/GetDocId?type=" + type;
        return this.http.get(this.url);
    }
    getGlConfigId(bookId:string , accId:string ){
        this.url = this.baseUrl;
        this.url += "/api/services/app/GLCONFIG/GetGlConfigId?";
        if (bookId !== undefined)
        this.url += "bookId=" + encodeURIComponent("" + bookId) + "&";
        if (accId !== undefined)
        this.url += "accId=" + encodeURIComponent("" + accId);

        return this.http.get(this.url);
    }
    getGlConfigAccount(bookId:string , accId:string ){
        this.url = this.baseUrl;
        this.url += "/api/services/app/GLCONFIG/GetGlConfigAccount?";
        if (bookId !== undefined)
        this.url += "bookId=" + encodeURIComponent("" + bookId) + "&";
        if (accId !== undefined)
        this.url += "accId=" + encodeURIComponent("" + accId);

        return this.http.get(this.url);
    }
}
