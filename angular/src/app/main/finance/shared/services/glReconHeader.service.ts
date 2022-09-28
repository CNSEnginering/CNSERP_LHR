import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PagedResultDtoOfGLReconHeaderDto, GLReconHeaderDto, PagedResultDtoOfGetGLReconHeaderForViewDto, GetBankReconcileForEditOutput } from '../dto/glReconHeader-dto';

@Injectable({
    providedIn: 'root'
  })
export class GLReconHeadersService {
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
     * @param maxOrdNoFilter (optional) 
     * @param minOrdNoFilter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional) 
     * @return Success
     */
    getAll(filter: string | null | undefined,docNoFilter: string | null | undefined, maxDocDateFilter: moment.Moment | null| undefined, minDocDateFilter: moment.Moment |null |undefined, bankIDFilter : string | null | undefined, bankNameFilter : string | null |undefined,  sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetGLReconHeaderForViewDto> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/BankReconciles/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (docNoFilter !== undefined)
            url_ += "DocNoFilter=" + encodeURIComponent("" + docNoFilter) + "&"; 
        if (maxDocDateFilter !== undefined)
            url_ += "MaxDocDateFilter=" + encodeURIComponent(maxDocDateFilter ? "" + maxDocDateFilter.toJSON() : "") + "&";
        if (minDocDateFilter !== undefined)
            url_ += "MinDocDateFilter=" + encodeURIComponent(minDocDateFilter ? "" + minDocDateFilter.toJSON() : "") + "&";
        if (bankIDFilter !== undefined)
            url_ += "BankIDFilter=" + encodeURIComponent("" + bankIDFilter) + "&"; 
        if (bankNameFilter !== undefined)
            url_ += "BankNameFilter=" + encodeURIComponent("" + bankNameFilter) + "&"; 
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
                    return <Observable<PagedResultDtoOfGetGLReconHeaderForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetGLReconHeaderForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetGLReconHeaderForViewDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfGetGLReconHeaderForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetGLReconHeaderForViewDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetGLReconHeaderForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getGLReconHeaderForEdit(id: number | null | undefined): Observable<GetBankReconcileForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/BankReconciles/GetBankReconcileForEdit?";
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
            return this.processGetGLReconHeaderForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetGLReconHeaderForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetBankReconcileForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetBankReconcileForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetGLReconHeaderForEdit(response: HttpResponseBase): Observable<GetBankReconcileForEditOutput> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GetBankReconcileForEditOutput.fromJS(resultData200) : new GetBankReconcileForEditOutput();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetBankReconcileForEditOutput>(<any>null);
    }

    getBeginningBal(bankID: string): Observable<number> {
        let url_ = this.baseUrl + "/api/services/app/BankReconciles/GetBeginningBalance?";
        if (bankID !== undefined)
            url_ += "bankID=" + encodeURIComponent("" + bankID) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processgetBeginningBal(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processgetBeginningBal(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processgetBeginningBal(response: HttpResponseBase): Observable<number> {
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
        return _observableOf<number>(<any>null);
    }
}