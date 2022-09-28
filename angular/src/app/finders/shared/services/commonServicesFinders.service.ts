import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PagedResultDtoOfCommonServiceFindersDto } from '../dtos/commonServicesFinders-dto';

import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
  })
export class CommonServiceFindersService {
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
     url: string = "";
    getAllCommonServicesFindersForLookupTable(filter: string | null | undefined,target:string |null | undefined, paramFilter:string |null | undefined,sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfCommonServiceFindersDto> {
        let url_ = this.baseUrl + "/api/services/app/CommonServicesFinders/GetCommonServiceLookupTable?";
        debugger
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (target !== undefined)
            url_ += "Target=" + encodeURIComponent("" + target) + "&"; 
        if (paramFilter !== undefined)
            url_ += "ParamFilter=" + encodeURIComponent("" + paramFilter) + "&"; 
        if (sorting !== undefined)
            url_ += "Sorting=" + encodeURIComponent("" + sorting) + "&"; 
        if (skipCount !== undefined)
            url_ += "SkipCount=" + encodeURIComponent("" + skipCount) + "&"; 
        if (maxResultCount !== undefined)
            url_ += "MaxResultCount=" + encodeURIComponent("" + maxResultCount) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetAll(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAll(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfCommonServiceFindersDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfCommonServiceFindersDto>><any>_observableThrow(response_);
        }));
    }
    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfCommonServiceFindersDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfCommonServiceFindersDto.fromJS(resultData200) : new PagedResultDtoOfCommonServiceFindersDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfCommonServiceFindersDto>(<any>null);
    }

    getAllForm(target: string | null | undefined) 
    {
     debugger;
     this.url = this.baseUrl;
     this.url += "/api/services/app/CommonServicesFinders/GetFormIDList?";      
     if (target !== undefined)
     this.url += "Target=" + encodeURIComponent("" + target) + "&"; 
     this.url = this.url.replace(/[?&]$/, "");
     return this.http.get(this.url).pipe(map((response: any) => {
         debugger
         return response["result"];
     }));
    }
}