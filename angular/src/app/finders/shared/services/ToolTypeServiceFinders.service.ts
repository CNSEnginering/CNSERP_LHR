import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PagedResultDtoOfToolTypeFindersDto } from '../dtos/ToolTypeFinders-dto';
import { map } from "rxjs/operators";
@Injectable({
    providedIn: 'root'
})
export class ToolTypeFindersService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param target (optional)
     * @return Success
     */
    getAllToolTyFindersForLookupTable(filter: string | null | undefined, target: string | null | undefined,

        paramFilter: string | null | undefined, param2Filter: string | null | undefined, param3Filter: number | null | undefined,
        sorting: string | null | undefined, skipCount: number | null | undefined,
        maxResultCount: number | null | undefined): Observable<PagedResultDtoOfToolTypeFindersDto> {
        debugger
        let url_ = this.baseUrl + "/api/services/app/MFFinders/GetAllToolTypelookupTable?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (target !== undefined)
            url_ += "Target=" + encodeURIComponent("" + target) + "&";
        if (paramFilter !== undefined)
            url_ += "ParamFilter=" + encodeURIComponent("" + paramFilter) + "&";
        if (param2Filter !== undefined)
            url_ += "Param2Filter=" + encodeURIComponent("" + param2Filter) + "&";
        if (param3Filter !== undefined)
            url_ += "Param3Filter=" + encodeURIComponent("" + param3Filter) + "&";

        url_ = url_.replace(/[?&]$/, "");
        // let options_: any = {
        //     observe: "response",
        //     responseType: "blob",
        //     headers: new HttpHeaders({
        //         "Accept": "application/json"
        //     })
        // };

        return this.http.get(url_).pipe(map((response: any) => {
            debugger
            return response["result"] as PagedResultDtoOfToolTypeFindersDto;
        }));

    }


}