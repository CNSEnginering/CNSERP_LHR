import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PagedResultDtoOfTransactionTypeDto, TransactionTypeDto } from '../dto/transaction-types-dto';

@Injectable({
    providedIn: 'root'
  })
export class TransactionTypesService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param typeIdFilter (optional) 
     * @param descriptionFilter (optional)
     * @param activeFilter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional)
     * @return Success
     */
    getAll(filter: string | null | undefined,typeIdFilter: string | null | undefined, descriptionFilter: string | null | undefined, activeFilter: number | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined,audtUserFilter: string | null | undefined,maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined,  sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfTransactionTypeDto> {
        let url_ = this.baseUrl + "/api/services/app/TransactionTypes/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (typeIdFilter !== undefined)
            url_ += "TypeIdFilter=" + encodeURIComponent("" + typeIdFilter) + "&"; 
        if (descriptionFilter !== undefined)
            url_ += "DescriptionFilter=" + encodeURIComponent("" + descriptionFilter) + "&"; 
        if (activeFilter !== undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&"; 
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
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAll(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfTransactionTypeDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfTransactionTypeDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfTransactionTypeDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfTransactionTypeDto.fromJS(resultData200) : new PagedResultDtoOfTransactionTypeDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfTransactionTypeDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getTransactionTypeForView(id: number | null | undefined): Observable<TransactionTypeDto> {
        let url_ = this.baseUrl + "/api/services/app/TransactionTypes/GetTransactionTypeForView?";
        if (id !== undefined)
            url_ += "id=" + encodeURIComponent("" + id) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetTransactionTypeForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetTransactionTypeForView(<any>response_);
                } catch (e) {
                    return <Observable<TransactionTypeDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<TransactionTypeDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetTransactionTypeForView(response: HttpResponseBase): Observable<TransactionTypeDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? TransactionTypeDto.fromJS(resultData200) : new TransactionTypeDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<TransactionTypeDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getTransactionTypeForEdit(id: number | null | undefined): Observable<TransactionTypeDto> {
        let url_ = this.baseUrl + "/api/services/app/TransactionTypes/GetTransactionTypeForEdit?";
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
            return this.processGetTransactionTypeForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetTransactionTypeForEdit(<any>response_);
                } catch (e) {
                    return <Observable<TransactionTypeDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<TransactionTypeDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetTransactionTypeForEdit(response: HttpResponseBase): Observable<TransactionTypeDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? TransactionTypeDto.fromJS(resultData200) : new TransactionTypeDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<TransactionTypeDto>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: TransactionTypeDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/TransactionTypes/CreateOrEdit";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(input);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json", 
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processCreateOrEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processCreateOrEdit(<any>response_);
                } catch (e) {
                    return <Observable<void>><any>_observableThrow(e);
                }
            } else
                return <Observable<void>><any>_observableThrow(response_);
        }));
    }

    protected processCreateOrEdit(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return _observableOf<void>(<any>null);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    delete(id: number | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/TransactionTypes/Delete?";
        if (id !== undefined)
            url_ += "Id=" + encodeURIComponent("" + id) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
            })
        };

        return this.http.request("delete", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processDelete(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processDelete(<any>response_);
                } catch (e) {
                    return <Observable<void>><any>_observableThrow(e);
                }
            } else
                return <Observable<void>><any>_observableThrow(response_);
        }));
    }

    protected processDelete(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return _observableOf<void>(<any>null);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(<any>null);
    }

    /**
     * @param filter (optional) 
     * @param typeIdFilter (optional) 
     * @param descriptionFilter (optional)
     * @param activeFilter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional)
     * @return Success
     */
    getTransactionTypesToExcel(filter: string | null | undefined, typeIdFilter: string | null | undefined, descriptionFilter: string | null | undefined, activeFilter: number | null | undefined, createdByFilter: string | null | undefined,  maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined,audtUserFilter: string | null | undefined,maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/TransactionTypes/GetTransactionTypesToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (typeIdFilter !== undefined)
            url_ += "TypeIdFilter=" + encodeURIComponent("" + typeIdFilter) + "&"; 
        if (descriptionFilter !== undefined)
            url_ += "DescriptionFilter=" + encodeURIComponent("" + descriptionFilter) + "&";  
        if (activeFilter !== undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&"; 
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
        
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetTransactionTypesToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetTransactionTypesToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetTransactionTypesToExcel(response: HttpResponseBase): Observable<FileDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? FileDto.fromJS(resultData200) : new FileDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<FileDto>(<any>null);
    }
}