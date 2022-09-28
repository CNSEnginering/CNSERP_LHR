import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PagedResultDtoOfPORETHeaderDto, PORETHeaderDto } from '../dtos/poretHeader-dto';

@Injectable({
    providedIn: 'root'
  })
export class PORETHeadersService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param maxDocNoFilter (optional) 
     * @param minDocNoFilter (optional) 
     * @param maxDocDateFilter (optional) 
     * @param minDocDateFilter (optional) 
     * @param maxLocIDFilter (optional) 
     * @param minLocIDFilter (optional) 
     * @param postedFilter (optional) 
     * @param maxLinkDetIDFilter (optional) 
     * @param minLinkDetIDFilter (optional) 
     * @param ordNoFilter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional) 
     * @return Success
     */
    getAll(filter: string | null | undefined,maxDocNoFilter: number | null | undefined,minDocNoFilter: number | null | undefined,maxLocIDFilter: number | null | undefined, minLocIDFilter: number | null | undefined,activeFilter: number | null | undefined,postedFilter: number | null | undefined, createdByFilter: string | null | undefined,audtUserFilter: string | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfPORETHeaderDto> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/PORETHeaders/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (maxDocNoFilter !== undefined)
            url_ += "MaxDocNoFilter=" + encodeURIComponent("" + maxDocNoFilter) + "&"; 
        if (minDocNoFilter !== undefined)
            url_ += "MinDocNoFilter=" + encodeURIComponent("" + minDocNoFilter) + "&"; 
        if (maxLocIDFilter !== undefined)
            url_ += "MaxLocIDFilter=" + encodeURIComponent("" + maxLocIDFilter) + "&"; 
        if (minLocIDFilter !== undefined)
            url_ += "MinLocIDFilter=" + encodeURIComponent("" + minLocIDFilter) + "&"; 
        if (activeFilter !== undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&"; 
        if (postedFilter !== undefined)
            url_ += "PostedFilter=" + encodeURIComponent("" + postedFilter) + "&"; 
        if (createdByFilter !== undefined)
            url_ += "CreatedByFilter=" + encodeURIComponent("" + createdByFilter) + "&"; 
        if (audtUserFilter !== undefined)
            url_ += "AudtUserFilter=" + encodeURIComponent("" + audtUserFilter) + "&"; 
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
        })).pipe(_observableCatch((response_: any) => {debugger;
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAll(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfPORETHeaderDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfPORETHeaderDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfPORETHeaderDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfPORETHeaderDto.fromJS(resultData200) : new PagedResultDtoOfPORETHeaderDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfPORETHeaderDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getPORETHeaderForEdit(id: number | null | undefined): Observable<PORETHeaderDto> {
        let url_ = this.baseUrl + "/api/services/app/PORETHeaders/GetPORETHeaderForEdit?";
        if (id !== undefined)
            url_ += "Id=" + encodeURIComponent("" + id) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetPORETHeaderForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetPORETHeaderForEdit(<any>response_);
                } catch (e) {
                    return <Observable<PORETHeaderDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PORETHeaderDto>><any>_observableThrow(response_);
        }));
    }

    public processGetPORETHeaderForEdit(response: HttpResponseBase): Observable<PORETHeaderDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PORETHeaderDto.fromJS(resultData200) : new PORETHeaderDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PORETHeaderDto>(<any>null);
    }
}