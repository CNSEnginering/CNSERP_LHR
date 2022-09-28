import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PagedResultDtoOfICCNSHeaderDto, ICCNSHeaderDto } from '../dto/iccnsHeader-dto';
import { BinaryData } from 'fs';
import { List } from 'lodash';

@Injectable({
    providedIn: 'root'
  })
export class ICCNSHeadersService {
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
    getAll(filter: string | null | undefined,maxDocNoFilter: number | null | undefined,minDocNoFilter: number | null | undefined, maxDocDateFilter: moment.Moment, minDocDateFilter: moment.Moment, maxLocIDFilter: number | null | undefined, minLocIDFilter: number | null | undefined,activeFilter: number | null | undefined,postedFilter: number | null | undefined, minLinkDetIDFilter: number | null | undefined,maxLinkDetIDFilter: number | null | undefined,ordNoFilter: string | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined,audtUserFilter: string | null | undefined,maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined,  sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfICCNSHeaderDto> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/ICCNSHeaders/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (maxDocNoFilter !== undefined)
            url_ += "MaxDocNoFilter=" + encodeURIComponent("" + maxDocNoFilter) + "&"; 
        if (minDocNoFilter !== undefined)
            url_ += "MinDocNoFilter=" + encodeURIComponent("" + minDocNoFilter) + "&"; 
        if (maxDocDateFilter !== undefined)
            url_ += "MaxDocDateFilter=" + encodeURIComponent(maxDocDateFilter ? "" + maxDocDateFilter.toJSON() : "") + "&"; 
        if (minDocDateFilter !== undefined)
            url_ += "MinDocDateFilter=" + encodeURIComponent(minDocDateFilter ? "" + minDocDateFilter.toJSON() : "") + "&"; 
        if (maxLocIDFilter !== undefined)
            url_ += "MaxLocIDFilter=" + encodeURIComponent("" + maxLocIDFilter) + "&"; 
        if (minLocIDFilter !== undefined)
            url_ += "MinLocIDFilter=" + encodeURIComponent("" + minLocIDFilter) + "&";
        if (activeFilter !== undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&"; 
        if (postedFilter !== undefined)
            url_ += "PostedFilter=" + encodeURIComponent("" + postedFilter) + "&"; 
        if (maxLinkDetIDFilter !== undefined)
            url_ += "MaxLinkDetIDFilter=" + encodeURIComponent("" + maxLinkDetIDFilter) + "&"; 
        if (minLinkDetIDFilter !== undefined)
            url_ += "MinLinkDetIDFilter=" + encodeURIComponent("" + minLinkDetIDFilter) + "&"; 
        if (ordNoFilter !== undefined)
            url_ += "OrdNoFilter=" + encodeURIComponent("" + ordNoFilter) + "&"; 
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
                    return <Observable<PagedResultDtoOfICCNSHeaderDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfICCNSHeaderDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfICCNSHeaderDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfICCNSHeaderDto.fromJS(resultData200) : new PagedResultDtoOfICCNSHeaderDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfICCNSHeaderDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getICCNSHeaderForEdit(id: number | null | undefined): Observable<ICCNSHeaderDto> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/ICCNSHeaders/GetICCNSHeaderForEdit?";
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
            return this.processGetICCNSHeaderForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetICCNSHeaderForEdit(<any>response_);
                } catch (e) {
                    return <Observable<ICCNSHeaderDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<ICCNSHeaderDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetICCNSHeaderForEdit(response: HttpResponseBase): Observable<ICCNSHeaderDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? ICCNSHeaderDto.fromJS(resultData200) : new ICCNSHeaderDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<ICCNSHeaderDto>(<any>null);
    }
    
    getImage(appID: number, docID: number): Observable<List<BinaryData>> {

        debugger;
        let url_ = this.baseUrl + "/DemoUiComponents/GetImageData?";
        if (appID !== undefined)
            url_ += "AppID=" + encodeURIComponent("" + appID) + "&";
        if (docID !== undefined)
            url_ += "DocID=" + encodeURIComponent("" + docID) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetImage(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetImage(<any>response_);
                } catch (e) {
                    return <Observable<List<BinaryData>>><any>_observableThrow(e);
                }
            } else
                return <Observable<List<BinaryData>>><any>_observableThrow(response_);
        }));
    }

    protected processGetImage(response: HttpResponseBase): Observable<List<BinaryData>> {
        debugger;
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 !== undefined ? resultData200 : <any>null;
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<List<BinaryData>>(<any>null);

    }
}