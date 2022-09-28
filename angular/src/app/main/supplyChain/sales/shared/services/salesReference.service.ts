import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PagedResultDtoOfGetSalesReferenceForViewDto, GetSalesReferenceForViewDto, GetSalesReferenceForEditOutput, CreateOrEditSalesReferenceDto } from '../dtos/salesReference-dto';


@Injectable({
    providedIn: 'root'
})

export class SalesReferencesServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param maxRefIDFilter (optional) 
     * @param minRefIDFilter (optional) 
     * @param refNameFilter (optional) 
     * @param aCTIVEFilter (optional) 
     * @param maxAUDTDATEFilter (optional) 
     * @param minAUDTDATEFilter (optional) 
     * @param aUDTUSERFilter (optional) 
     * @param maxCreatedDATEFilter (optional) 
     * @param minCreatedDATEFilter (optional) 
     * @param createdUSERFilter (optional) 
     * @param sorting (optional) 
     * @param skipCount (optional) 
     * @param maxResultCount (optional) 
     * @return Success
     */
    getAll(filter: string | null | undefined, maxRefIDFilter: number | null | undefined, minRefIDFilter: number | null | undefined, refNameFilter: string | null | undefined, aCTIVEFilter: number | null | undefined, maxAUDTDATEFilter: moment.Moment | null | undefined, minAUDTDATEFilter: moment.Moment | null | undefined, aUDTUSERFilter: string | null | undefined, maxCreatedDATEFilter: moment.Moment | null | undefined, minCreatedDATEFilter: moment.Moment | null | undefined, createdUSERFilter: string | null | undefined, refType: string | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetSalesReferenceForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/SalesReferences/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxRefIDFilter !== undefined)
            url_ += "MaxRefIDFilter=" + encodeURIComponent("" + maxRefIDFilter) + "&";
        if (minRefIDFilter !== undefined)
            url_ += "MinRefIDFilter=" + encodeURIComponent("" + minRefIDFilter) + "&";
        if (refNameFilter !== undefined)
            url_ += "RefNameFilter=" + encodeURIComponent("" + refNameFilter) + "&";
        if (aCTIVEFilter !== undefined)
            url_ += "ACTIVEFilter=" + encodeURIComponent("" + aCTIVEFilter) + "&";
        if (maxAUDTDATEFilter !== undefined)
            url_ += "MaxAUDTDATEFilter=" + encodeURIComponent(maxAUDTDATEFilter ? "" + maxAUDTDATEFilter.toJSON() : "") + "&";
        if (minAUDTDATEFilter !== undefined)
            url_ += "MinAUDTDATEFilter=" + encodeURIComponent(minAUDTDATEFilter ? "" + minAUDTDATEFilter.toJSON() : "") + "&";
        if (aUDTUSERFilter !== undefined)
            url_ += "AUDTUSERFilter=" + encodeURIComponent("" + aUDTUSERFilter) + "&";
        if (maxCreatedDATEFilter !== undefined)
            url_ += "MaxCreatedDATEFilter=" + encodeURIComponent(maxCreatedDATEFilter ? "" + maxCreatedDATEFilter.toJSON() : "") + "&";
        if (minCreatedDATEFilter !== undefined)
            url_ += "MinCreatedDATEFilter=" + encodeURIComponent(minCreatedDATEFilter ? "" + minCreatedDATEFilter.toJSON() : "") + "&";
        if (createdUSERFilter !== undefined)
            url_ += "CreatedUSERFilter=" + encodeURIComponent("" + createdUSERFilter) + "&";
        if (refType !== undefined)
            url_ += "RefType=" + encodeURIComponent("" + refType) + "&";
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
                    return <Observable<PagedResultDtoOfGetSalesReferenceForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetSalesReferenceForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetSalesReferenceForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetSalesReferenceForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetSalesReferenceForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetSalesReferenceForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getSalesReferenceForView(id: number | null | undefined): Observable<GetSalesReferenceForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/SalesReferences/GetSalesReferenceForView?";
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
            return this.processGetSalesReferenceForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetSalesReferenceForView(<any>response_);
                } catch (e) {
                    return <Observable<GetSalesReferenceForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetSalesReferenceForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetSalesReferenceForView(response: HttpResponseBase): Observable<GetSalesReferenceForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetSalesReferenceForViewDto.fromJS(resultData200) : new GetSalesReferenceForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetSalesReferenceForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getSalesReferenceForEdit(id: number | null | undefined): Observable<GetSalesReferenceForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/SalesReferences/GetSalesReferenceForEdit?";
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
            return this.processGetSalesReferenceForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetSalesReferenceForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetSalesReferenceForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetSalesReferenceForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetSalesReferenceForEdit(response: HttpResponseBase): Observable<GetSalesReferenceForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetSalesReferenceForEditOutput.fromJS(resultData200) : new GetSalesReferenceForEditOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetSalesReferenceForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditSalesReferenceDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/SalesReferences/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/SalesReferences/Delete?";
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
     * @param maxRefIDFilter (optional) 
     * @param minRefIDFilter (optional) 
     * @param refNameFilter (optional) 
     * @param aCTIVEFilter (optional) 
     * @param maxAUDTDATEFilter (optional) 
     * @param minAUDTDATEFilter (optional) 
     * @param aUDTUSERFilter (optional) 
     * @param maxCreatedDATEFilter (optional) 
     * @param minCreatedDATEFilter (optional) 
     * @param createdUSERFilter (optional) 
     * @return Success
     */
    getSalesReferencesToExcel(filter: string | null | undefined, maxRefIDFilter: number | null | undefined, minRefIDFilter: number | null | undefined, refNameFilter: string | null | undefined, aCTIVEFilter: number | null | undefined, maxAUDTDATEFilter: moment.Moment | null | undefined, minAUDTDATEFilter: moment.Moment | null | undefined, aUDTUSERFilter: string | null | undefined, maxCreatedDATEFilter: moment.Moment | null | undefined, minCreatedDATEFilter: moment.Moment | null | undefined, createdUSERFilter: string | null | undefined, reftype: string): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/SalesReferences/GetSalesReferencesToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxRefIDFilter !== undefined)
            url_ += "MaxRefIDFilter=" + encodeURIComponent("" + maxRefIDFilter) + "&";
        if (minRefIDFilter !== undefined)
            url_ += "MinRefIDFilter=" + encodeURIComponent("" + minRefIDFilter) + "&";
        if (refNameFilter !== undefined)
            url_ += "RefNameFilter=" + encodeURIComponent("" + refNameFilter) + "&";
        if (aCTIVEFilter !== undefined)
            url_ += "ACTIVEFilter=" + encodeURIComponent("" + aCTIVEFilter) + "&";
        if (maxAUDTDATEFilter !== undefined)
            url_ += "MaxAUDTDATEFilter=" + encodeURIComponent(maxAUDTDATEFilter ? "" + maxAUDTDATEFilter.toJSON() : "") + "&";
        if (minAUDTDATEFilter !== undefined)
            url_ += "MinAUDTDATEFilter=" + encodeURIComponent(minAUDTDATEFilter ? "" + minAUDTDATEFilter.toJSON() : "") + "&";
        if (aUDTUSERFilter !== undefined)
            url_ += "AUDTUSERFilter=" + encodeURIComponent("" + aUDTUSERFilter) + "&";
        if (maxCreatedDATEFilter !== undefined)
            url_ += "MaxCreatedDATEFilter=" + encodeURIComponent(maxCreatedDATEFilter ? "" + maxCreatedDATEFilter.toJSON() : "") + "&";
        if (minCreatedDATEFilter !== undefined)
            url_ += "MinCreatedDATEFilter=" + encodeURIComponent(minCreatedDATEFilter ? "" + minCreatedDATEFilter.toJSON() : "") + "&";
        if (createdUSERFilter !== undefined)
            url_ += "CreatedUSERFilter=" + encodeURIComponent("" + createdUSERFilter) + "&";
        if (reftype !== undefined)
            url_ += "RefType=" + encodeURIComponent("" + reftype) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetSalesReferencesToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetSalesReferencesToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetSalesReferencesToExcel(response: HttpResponseBase): Observable<FileDto> {
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

    getMaxReferenceId(): Observable<number> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/SalesReferences/GetMaxReferenceID";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetMaxReferenceId(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetMaxReferenceId(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processGetMaxReferenceId(response: HttpResponseBase): Observable<number> {
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