import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, blobToText, throwException } from "@shared/service-proxies/service-proxies";
import { PagedResultDtoOfGetLCExpensesDetailForViewDto, GetLCExpensesDetailForViewDto, GetLCExpensesDetailForEditOutput, CreateOrEditLCExpensesDetailDto, ListResultDtoOfLCExpenses } from '../dto/lcExpensesDetail-dto';
import { url } from 'inspector';

@Injectable({
    providedIn: 'root'
})

export class LCExpensesDetailsServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }
    /**
     * @param filter (optional) 
     * @param maxDetIDFilter (optional) 
     * @param minDetIDFilter (optional) 
     * @param maxLocIDFilter (optional) 
     * @param minLocIDFilter (optional) 
     * @param maxDocNoFilter (optional) 
     * @param minDocNoFilter (optional) 
     * @param expDescFilter (optional) 
     * @param maxAmountFilter (optional) 
     * @param minAmountFilter (optional) 
     * @param sorting (optional) 
     * @param skipCount (optional) 
     * @param maxResultCount (optional) 
     * @return Success
     */
    getAll(filter: string | null | undefined, maxDetIDFilter: number | null | undefined, minDetIDFilter: number | null | undefined, maxLocIDFilter: number | null | undefined,
        minLocIDFilter: number | null | undefined, maxDocNoFilter: number | null | undefined, minDocNoFilter: number | null | undefined, expDescFilter: string | null | undefined,
        maxAmountFilter: number | null | undefined, minAmountFilter: number | null | undefined,
        sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined):
        Observable<PagedResultDtoOfGetLCExpensesDetailForViewDto> {

        let url_ = this.baseUrl + "/api/services/app/LCExpensesDetail/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxDetIDFilter !== undefined)
            url_ += "maxDetIDFilter=" + encodeURIComponent("" + maxDetIDFilter) + "&";
        if (minDetIDFilter !== undefined)
            url_ += "minDetIDFilter=" + encodeURIComponent("" + minDetIDFilter) + "&";
        if (maxLocIDFilter !== undefined)
            url_ += "MaxLocIDFilter=" + encodeURIComponent("" + maxLocIDFilter) + "&";
        if (minLocIDFilter !== undefined)
            url_ += "MinLocIDFilter=" + encodeURIComponent("" + minLocIDFilter) + "&";
        if (maxDocNoFilter !== undefined)
            url_ += "MaxDocNoFilter=" + encodeURIComponent("" + maxDocNoFilter) + "&";
        if (minDocNoFilter !== undefined)
            url_ += "MinDocNoFilter=" + encodeURIComponent("" + minDocNoFilter) + "&";
        if (expDescFilter !== undefined)
            url_ += "ExpDescFilter=" + encodeURIComponent("" + expDescFilter) + "&";
        if (maxAmountFilter !== undefined)
            url_ += "MaxAmountFilter=" + encodeURIComponent("" + maxAmountFilter) + "&";
        if (minAmountFilter !== undefined)
            url_ += "MinAmountFilter=" + encodeURIComponent("" + minAmountFilter) + "&";
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
                    return <Observable<PagedResultDtoOfGetLCExpensesDetailForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetLCExpensesDetailForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetLCExpensesDetailForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetLCExpensesDetailForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetLCExpensesDetailForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetLCExpensesDetailForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getLCExpensesDetailForView(id: number | null | undefined): Observable<GetLCExpensesDetailForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/LCExpensesDetail/GetLCExpensesDetailForView?";
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
            return this.processGetLCExpensesDetailForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetLCExpensesDetailForView(<any>response_);
                } catch (e) {
                    return <Observable<GetLCExpensesDetailForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetLCExpensesDetailForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetLCExpensesDetailForView(response: HttpResponseBase): Observable<GetLCExpensesDetailForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetLCExpensesDetailForViewDto.fromJS(resultData200) : new GetLCExpensesDetailForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetLCExpensesDetailForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getLCExpensesDetailForEdit(id: number | null | undefined): Observable<GetLCExpensesDetailForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/LCExpensesDetail/GetLCExpensesDetailForEdit?";
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
            return this.processGetLCExpensesDetailForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetLCExpensesDetailForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetLCExpensesDetailForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetLCExpensesDetailForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetLCExpensesDetailForEdit(response: HttpResponseBase): Observable<GetLCExpensesDetailForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetLCExpensesDetailForEditOutput.fromJS(resultData200) : new GetLCExpensesDetailForEditOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetLCExpensesDetailForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditLCExpensesDetailDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/LCExpensesDetail/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/LCExpensesDetail/Delete?";
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

    getLCExpenses(): Observable<ListResultDtoOfLCExpenses> {
        let url_ = this.baseUrl + "/api/services/app/LCExpensesDetail/GetLCExpenses";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response: any) => {
            return this.processGetLCExpenses(response);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetLCExpenses(<any>response_);
                } catch (e) {
                    return <Observable<ListResultDtoOfLCExpenses>><any>_observableThrow(e);
                }
            } else
                return <Observable<ListResultDtoOfLCExpenses>><any>_observableThrow(response_);
        }));
    }

    protected processGetLCExpenses(response: HttpResponseBase): Observable<ListResultDtoOfLCExpenses> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? ListResultDtoOfLCExpenses.fromJS(resultData200) : new ListResultDtoOfLCExpenses();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, headers);
            }));
        }
        return _observableOf<ListResultDtoOfLCExpenses>(<any>null);
    }
}