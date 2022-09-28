import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, blobToText, FileDto, throwException, PagedResultDtoOfNameValueDto } from "@shared/service-proxies/service-proxies";
import * as moment from 'moment';
import { PagedResultDtoOfGetRecurringVoucherForViewDto, GetRecurringVoucherForViewDto, GetRecurringVoucherForEditOutput, CreateOrEditRecurringVoucherDto } from '../dto/recurringVouchers-dto';

@Injectable({
    providedIn: 'root'
})
export class RecurringVouchersServiceProxy {
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
     * @param bookIDFilter (optional) 
     * @param maxVoucherNoFilter (optional) 
     * @param minVoucherNoFilter (optional) 
     * @param fmtVoucherNoFilter (optional) 
     * @param maxVoucherDateFilter (optional) 
     * @param minVoucherDateFilter (optional) 
     * @param maxVoucherMonthFilter (optional) 
     * @param minVoucherMonthFilter (optional) 
     * @param maxConfigIDFilter (optional) 
     * @param minConfigIDFilter (optional) 
     * @param referenceFilter (optional) 
     * @param activeFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @param sorting (optional) 
     * @param skipCount (optional) 
     * @param maxResultCount (optional) 
     * @return Success
     */
    getAll(filter: string | null | undefined, maxDocNoFilter: number | null | undefined, minDocNoFilter: number | null | undefined, bookIDFilter: string | null | undefined, maxVoucherNoFilter: number | null | undefined, minVoucherNoFilter: number | null | undefined, fmtVoucherNoFilter: string | null | undefined, maxVoucherDateFilter: moment.Moment | null | undefined, minVoucherDateFilter: moment.Moment | null | undefined, maxVoucherMonthFilter: number | null | undefined, minVoucherMonthFilter: number | null | undefined, maxConfigIDFilter: number | null | undefined, minConfigIDFilter: number | null | undefined, referenceFilter: string | null | undefined, activeFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetRecurringVoucherForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/RecurringVouchers/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxDocNoFilter !== undefined)
            url_ += "MaxDocNoFilter=" + encodeURIComponent("" + maxDocNoFilter) + "&";
        if (minDocNoFilter !== undefined)
            url_ += "MinDocNoFilter=" + encodeURIComponent("" + minDocNoFilter) + "&";
        if (bookIDFilter !== undefined)
            url_ += "BookIDFilter=" + encodeURIComponent("" + bookIDFilter) + "&";
        if (maxVoucherNoFilter !== undefined)
            url_ += "MaxVoucherNoFilter=" + encodeURIComponent("" + maxVoucherNoFilter) + "&";
        if (minVoucherNoFilter !== undefined)
            url_ += "MinVoucherNoFilter=" + encodeURIComponent("" + minVoucherNoFilter) + "&";
        if (fmtVoucherNoFilter !== undefined)
            url_ += "FmtVoucherNoFilter=" + encodeURIComponent("" + fmtVoucherNoFilter) + "&";
        if (maxVoucherDateFilter !== undefined)
            url_ += "MaxVoucherDateFilter=" + encodeURIComponent(maxVoucherDateFilter ? "" + maxVoucherDateFilter.toJSON() : "") + "&";
        if (minVoucherDateFilter !== undefined)
            url_ += "MinVoucherDateFilter=" + encodeURIComponent(minVoucherDateFilter ? "" + minVoucherDateFilter.toJSON() : "") + "&";
        if (maxVoucherMonthFilter !== undefined)
            url_ += "MaxVoucherMonthFilter=" + encodeURIComponent("" + maxVoucherMonthFilter) + "&";
        if (minVoucherMonthFilter !== undefined)
            url_ += "MinVoucherMonthFilter=" + encodeURIComponent("" + minVoucherMonthFilter) + "&";
        if (maxConfigIDFilter !== undefined)
            url_ += "MaxConfigIDFilter=" + encodeURIComponent("" + maxConfigIDFilter) + "&";
        if (minConfigIDFilter !== undefined)
            url_ += "MinConfigIDFilter=" + encodeURIComponent("" + minConfigIDFilter) + "&";
        if (referenceFilter !== undefined)
            url_ += "ReferenceFilter=" + encodeURIComponent("" + referenceFilter) + "&";
        if (activeFilter !== undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&";
        if (audtUserFilter !== undefined)
            url_ += "AudtUserFilter=" + encodeURIComponent("" + audtUserFilter) + "&";
        if (maxAudtDateFilter !== undefined)
            url_ += "MaxAudtDateFilter=" + encodeURIComponent(maxAudtDateFilter ? "" + maxAudtDateFilter.toJSON() : "") + "&";
        if (minAudtDateFilter !== undefined)
            url_ += "MinAudtDateFilter=" + encodeURIComponent(minAudtDateFilter ? "" + minAudtDateFilter.toJSON() : "") + "&";
        if (createdByFilter !== undefined)
            url_ += "CreatedByFilter=" + encodeURIComponent("" + createdByFilter) + "&";
        if (maxCreateDateFilter !== undefined)
            url_ += "MaxCreateDateFilter=" + encodeURIComponent(maxCreateDateFilter ? "" + maxCreateDateFilter.toJSON() : "") + "&";
        if (minCreateDateFilter !== undefined)
            url_ += "MinCreateDateFilter=" + encodeURIComponent(minCreateDateFilter ? "" + minCreateDateFilter.toJSON() : "") + "&";
        if (sorting !== undefined)
            url_ += "Sorting=" + encodeURIComponent("" + sorting) + "&";
        if (skipCount !== undefined)
            url_ += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        if (maxResultCount !== undefined)
            url_ += "MaxResultCount=" + encodeURIComponent("" + maxResultCount) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetAll(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAll(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfGetRecurringVoucherForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetRecurringVoucherForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetRecurringVoucherForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetRecurringVoucherForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetRecurringVoucherForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetRecurringVoucherForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getRecurringVoucherForView(id: number | null | undefined): Observable<GetRecurringVoucherForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/RecurringVouchers/GetRecurringVoucherForView?";
        if (id !== undefined)
            url_ += "id=" + encodeURIComponent("" + id) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetRecurringVoucherForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetRecurringVoucherForView(<any>response_);
                } catch (e) {
                    return <Observable<GetRecurringVoucherForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetRecurringVoucherForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetRecurringVoucherForView(response: HttpResponseBase): Observable<GetRecurringVoucherForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetRecurringVoucherForViewDto.fromJS(resultData200) : new GetRecurringVoucherForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetRecurringVoucherForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getRecurringVoucherForEdit(id: number | null | undefined): Observable<GetRecurringVoucherForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/RecurringVouchers/GetRecurringVoucherForEdit?";
        if (id !== undefined)
            url_ += "Id=" + encodeURIComponent("" + id) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetRecurringVoucherForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetRecurringVoucherForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetRecurringVoucherForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetRecurringVoucherForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetRecurringVoucherForEdit(response: HttpResponseBase): Observable<GetRecurringVoucherForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetRecurringVoucherForEditOutput.fromJS(resultData200) : new GetRecurringVoucherForEditOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetRecurringVoucherForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditRecurringVoucherDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/RecurringVouchers/CreateOrEdit";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(input);

        let options_: any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json",
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_: any) => {
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

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
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
        let url_ = this.baseUrl + "/api/services/app/RecurringVouchers/Delete?";
        if (id !== undefined)
            url_ += "Id=" + encodeURIComponent("" + id) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
            })
        };

        return this.http.request("delete", url_, options_).pipe(_observableMergeMap((response_: any) => {
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

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
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
     * @param maxDocNoFilter (optional) 
     * @param minDocNoFilter (optional) 
     * @param bookIDFilter (optional) 
     * @param maxVoucherNoFilter (optional) 
     * @param minVoucherNoFilter (optional) 
     * @param fmtVoucherNoFilter (optional) 
     * @param maxVoucherDateFilter (optional) 
     * @param minVoucherDateFilter (optional) 
     * @param maxVoucherMonthFilter (optional) 
     * @param minVoucherMonthFilter (optional) 
     * @param maxConfigIDFilter (optional) 
     * @param minConfigIDFilter (optional) 
     * @param referenceFilter (optional) 
     * @param activeFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @return Success
     */
    getRecurringVouchersToExcel(filter: string | null | undefined, maxDocNoFilter: number | null | undefined, minDocNoFilter: number | null | undefined, bookIDFilter: string | null | undefined, maxVoucherNoFilter: number | null | undefined, minVoucherNoFilter: number | null | undefined, fmtVoucherNoFilter: string | null | undefined, maxVoucherDateFilter: moment.Moment | null | undefined, minVoucherDateFilter: moment.Moment | null | undefined, maxVoucherMonthFilter: number | null | undefined, minVoucherMonthFilter: number | null | undefined, maxConfigIDFilter: number | null | undefined, minConfigIDFilter: number | null | undefined, referenceFilter: string | null | undefined, activeFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/RecurringVouchers/GetRecurringVouchersToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxDocNoFilter !== undefined)
            url_ += "MaxDocNoFilter=" + encodeURIComponent("" + maxDocNoFilter) + "&";
        if (minDocNoFilter !== undefined)
            url_ += "MinDocNoFilter=" + encodeURIComponent("" + minDocNoFilter) + "&";
        if (bookIDFilter !== undefined)
            url_ += "BookIDFilter=" + encodeURIComponent("" + bookIDFilter) + "&";
        if (maxVoucherNoFilter !== undefined)
            url_ += "MaxVoucherNoFilter=" + encodeURIComponent("" + maxVoucherNoFilter) + "&";
        if (minVoucherNoFilter !== undefined)
            url_ += "MinVoucherNoFilter=" + encodeURIComponent("" + minVoucherNoFilter) + "&";
        if (fmtVoucherNoFilter !== undefined)
            url_ += "FmtVoucherNoFilter=" + encodeURIComponent("" + fmtVoucherNoFilter) + "&";
        if (maxVoucherDateFilter !== undefined)
            url_ += "MaxVoucherDateFilter=" + encodeURIComponent(maxVoucherDateFilter ? "" + maxVoucherDateFilter.toJSON() : "") + "&";
        if (minVoucherDateFilter !== undefined)
            url_ += "MinVoucherDateFilter=" + encodeURIComponent(minVoucherDateFilter ? "" + minVoucherDateFilter.toJSON() : "") + "&";
        if (maxVoucherMonthFilter !== undefined)
            url_ += "MaxVoucherMonthFilter=" + encodeURIComponent("" + maxVoucherMonthFilter) + "&";
        if (minVoucherMonthFilter !== undefined)
            url_ += "MinVoucherMonthFilter=" + encodeURIComponent("" + minVoucherMonthFilter) + "&";
        if (maxConfigIDFilter !== undefined)
            url_ += "MaxConfigIDFilter=" + encodeURIComponent("" + maxConfigIDFilter) + "&";
        if (minConfigIDFilter !== undefined)
            url_ += "MinConfigIDFilter=" + encodeURIComponent("" + minConfigIDFilter) + "&";
        if (referenceFilter !== undefined)
            url_ += "ReferenceFilter=" + encodeURIComponent("" + referenceFilter) + "&";
        if (activeFilter !== undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&";
        if (audtUserFilter !== undefined)
            url_ += "AudtUserFilter=" + encodeURIComponent("" + audtUserFilter) + "&";
        if (maxAudtDateFilter !== undefined)
            url_ += "MaxAudtDateFilter=" + encodeURIComponent(maxAudtDateFilter ? "" + maxAudtDateFilter.toJSON() : "") + "&";
        if (minAudtDateFilter !== undefined)
            url_ += "MinAudtDateFilter=" + encodeURIComponent(minAudtDateFilter ? "" + minAudtDateFilter.toJSON() : "") + "&";
        if (createdByFilter !== undefined)
            url_ += "CreatedByFilter=" + encodeURIComponent("" + createdByFilter) + "&";
        if (maxCreateDateFilter !== undefined)
            url_ += "MaxCreateDateFilter=" + encodeURIComponent(maxCreateDateFilter ? "" + maxCreateDateFilter.toJSON() : "") + "&";
        if (minCreateDateFilter !== undefined)
            url_ += "MinCreateDateFilter=" + encodeURIComponent(minCreateDateFilter ? "" + minCreateDateFilter.toJSON() : "") + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetRecurringVouchersToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetRecurringVouchersToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetRecurringVouchersToExcel(response: HttpResponseBase): Observable<FileDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
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

    getMaxDocId(): Observable<number> {
        let url_ = this.baseUrl + "/api/services/app/RecurringVouchers/GetMaxID";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetMaxDocId(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetMaxDocId(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processGetMaxDocId(response: HttpResponseBase): Observable<number> {
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