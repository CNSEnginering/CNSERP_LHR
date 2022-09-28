import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PagedResultDtoOfGLINVHeaderDto, GLINVHeaderDto } from '../dto/glinvHeader-dto';

@Injectable({
    providedIn: 'root'
  })
export class GLINVHeadersService {
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
     * @param bankIDFilter (optional) 
     * @param maxDocDateFilter (optional) 
     * @param minDocDateFilter (optional) 
     * @param maxPostDateFilter (optional) 
     * @param minPostDateFilter (optional) 
     * @param partyInvNoFilter (optional) 
     * @param minPartyInvDateFilter (optional) 
     * @param maxPartyInvDateFilter (optional) 
     * @param postedFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @return Success
     */
    getAll(filter: string | null | undefined,maxDocNoFilter: number | null | undefined,minDocNoFilter: number | null | undefined, bankIDFilter: string | null | undefined,typeIDfilter: string | null | undefined,   maxDocDateFilter: moment.Moment, minDocDateFilter: moment.Moment,maxPostDateFilter: moment.Moment, minPostDateFilter: moment.Moment, partyInvNoFilter: string | null | undefined, minPartyInvDateFilter: moment.Moment, maxPartyInvDateFilter: moment.Moment, postedFilter: number | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined,audtUserFilter: string | null | undefined,maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined,  sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGLINVHeaderDto> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/GLINVHeaders/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (maxDocNoFilter !== undefined)
            url_ += "MaxDocNoFilter=" + encodeURIComponent("" + maxDocNoFilter) + "&"; 
        if (minDocNoFilter !== undefined)
            url_ += "MinDocNoFilter=" + encodeURIComponent("" + minDocNoFilter) + "&"; 
        if (bankIDFilter !== undefined)
            url_ += "BankIDFilter=" + encodeURIComponent("" + bankIDFilter) + "&"; 
        if (typeIDfilter !== undefined)
            url_ += "TypeIDfilter=" + encodeURIComponent("" + typeIDfilter) + "&"; 
        if (maxDocDateFilter !== undefined)
            url_ += "MaxDocDateFilter=" + encodeURIComponent(maxDocDateFilter ? "" + maxDocDateFilter.toJSON() : "") + "&"; 
        if (minDocDateFilter !== undefined)
            url_ += "MinDocDateFilter=" + encodeURIComponent(minDocDateFilter ? "" + minDocDateFilter.toJSON() : "") + "&"; 
        if (maxPostDateFilter !== undefined)
            url_ += "MaxPostDateFilter=" + encodeURIComponent(maxPostDateFilter ? "" + maxPostDateFilter.toJSON() : "") + "&"; 
        if (minPostDateFilter !== undefined)
            url_ += "MinPostDateFilter=" + encodeURIComponent(minPostDateFilter ? "" + minPostDateFilter.toJSON() : "") + "&"; 
        if (partyInvNoFilter !== undefined) 
            url_ += "PartyInvNoFilter=" + encodeURIComponent("" + partyInvNoFilter) + "&"; 
        if (minPartyInvDateFilter !== undefined)
            url_ += "MinPartyInvDateFilter=" + encodeURIComponent(minPartyInvDateFilter ? "" + minPartyInvDateFilter.toJSON() : "") + "&"; 
        if (maxPartyInvDateFilter !== undefined)
            url_ += "MaxPartyInvDateFilter=" + encodeURIComponent(maxPartyInvDateFilter ? "" + maxPartyInvDateFilter.toJSON() : "") + "&";
        if (postedFilter !== undefined)
            url_ += "PostedFilter=" + encodeURIComponent("" + postedFilter) + "&"; 
        if (createdByFilter !== undefined)
            url_ += "CreatedByFilter=" + encodeURIComponent("" + createdByFilter) + "&"; 
        if (maxCreateDateFilter !== undefined)
            url_ += "MaxCreateDateFilter=" + encodeURIComponent(maxCreateDateFilter ? "" + maxCreateDateFilter.toJSON() : "") + "&"; 
        if (minCreateDateFilter !== undefined)
            url_ += "MinCreateDateFilter=" + encodeURIComponent(minCreateDateFilter ? "" + minCreateDateFilter.toJSON() : "") + "&"; 
        if (audtUserFilter !== undefined)
            url_ += "AudtUserFilter=" + encodeURIComponent("" + audtUserFilter) + "&"; 
        if (maxAudtDateFilter !== undefined)
            url_ += "MaxAudtDateFilter=" + encodeURIComponent(maxAudtDateFilter ? "" + maxAudtDateFilter.toJSON() : "") + "&"; 
        if (minAudtDateFilter !== undefined)
            url_ += "MinAudtDateFilter=" + encodeURIComponent(minAudtDateFilter ? "" + minAudtDateFilter.toJSON() : "") + "&"; 
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
                    return <Observable<PagedResultDtoOfGLINVHeaderDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGLINVHeaderDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGLINVHeaderDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfGLINVHeaderDto.fromJS(resultData200) : new PagedResultDtoOfGLINVHeaderDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGLINVHeaderDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getGLINVHeaderForEdit(id: number | null | undefined): Observable<GLINVHeaderDto> {
        let url_ = this.baseUrl + "/api/services/app/GLINVHeaders/GetGLINVHeaderForEdit?";
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
            return this.processGetGLINVHeaderForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetGLINVHeaderForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GLINVHeaderDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GLINVHeaderDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetGLINVHeaderForEdit(response: HttpResponseBase): Observable<GLINVHeaderDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GLINVHeaderDto.fromJS(resultData200) : new GLINVHeaderDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GLINVHeaderDto>(<any>null);
    }
}