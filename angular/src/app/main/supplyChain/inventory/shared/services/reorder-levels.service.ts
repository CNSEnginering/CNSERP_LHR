import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PagedResultDtoOfReorderLevelDto, ReorderLevelDto } from '../dto/reorder-levels-dto';

@Injectable({
    providedIn: 'root'
  })
export class ReorderLevelsService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param maxLocIdFilter (optional) 
     * @param minLocIdFilter (optional) 
     * @param itemIdFilter (optional) 
     * @param maxMinLevelFilter (optional) 
     * @param minMinLevelFilter (optional)
     * @param maxMaxLevelFilter (optional) 
     * @param minMaxLevelFilter (optional) 
     * @param maxOrdLevelFilter (optional) 
     * @param minOrdLevelFilter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional)
     * @return Success
     */
    getAll(filter: string | null | undefined,maxLocIdFilter: number | null | undefined,minLocIdFilter: number | null | undefined,itemIdFilter: string | null | undefined,maxMinLevelFilter: number | null | undefined,minMinLevelFilter: number | null | undefined,maxMaxLevelFilter: number | null | undefined, minMaxLevelFilter: number | null | undefined, maxOrdLevelFilter: number | null | undefined,minOrdLevelFilter: number | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined,audtUserFilter: string | null | undefined,maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined,  sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfReorderLevelDto> {
        let url_ = this.baseUrl + "/api/services/app/ReorderLevels/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (maxLocIdFilter !== undefined)
            url_ += "MaxLocIdFilter=" + encodeURIComponent("" + maxLocIdFilter) + "&"; 
        if (minLocIdFilter !== undefined)
            url_ += "MinLocIdFilter=" + encodeURIComponent("" + minLocIdFilter) + "&";  
        if (itemIdFilter !== undefined)
            url_ += "ItemIdFilter=" + encodeURIComponent("" + itemIdFilter) + "&"; 
        if (maxMinLevelFilter !== undefined)
            url_ += "MaxMinLevelFilter=" + encodeURIComponent("" + maxMinLevelFilter) + "&"; 
        if (minMinLevelFilter !== undefined)
            url_ += "MinMinLevelFilter=" + encodeURIComponent("" + minMinLevelFilter) + "&"; 
        if (maxMaxLevelFilter !== undefined)
            url_ += "MaxMaxLevelFilter=" + encodeURIComponent("" + maxMaxLevelFilter) + "&"; 
        if (minMaxLevelFilter !== undefined)
            url_ += "MinMaxLevelFilter=" + encodeURIComponent("" + minMaxLevelFilter) + "&"; 
        if (maxOrdLevelFilter !== undefined)
            url_ += "MaxOrdLevelFilter=" + encodeURIComponent("" + maxOrdLevelFilter) + "&"; 
        if (minOrdLevelFilter !== undefined)
            url_ += "MinOrdLevelFilter=" + encodeURIComponent("" + minOrdLevelFilter) + "&"; 
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
                    return <Observable<PagedResultDtoOfReorderLevelDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfReorderLevelDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfReorderLevelDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfReorderLevelDto.fromJS(resultData200) : new PagedResultDtoOfReorderLevelDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfReorderLevelDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getReorderLevelForView(id: number | null | undefined): Observable<ReorderLevelDto> {
        let url_ = this.baseUrl + "/api/services/app/ReorderLevels/GetReorderLevelForView?";
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
            return this.processGetReorderLevelForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetReorderLevelForView(<any>response_);
                } catch (e) {
                    return <Observable<ReorderLevelDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<ReorderLevelDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetReorderLevelForView(response: HttpResponseBase): Observable<ReorderLevelDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? ReorderLevelDto.fromJS(resultData200) : new ReorderLevelDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<ReorderLevelDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getReorderLevelForEdit(id: number | null | undefined): Observable<ReorderLevelDto> {
        let url_ = this.baseUrl + "/api/services/app/ReorderLevels/GetReorderLevelForEdit?";
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
            return this.processGetReorderLevelForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetReorderLevelForEdit(<any>response_);
                } catch (e) {
                    return <Observable<ReorderLevelDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<ReorderLevelDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetReorderLevelForEdit(response: HttpResponseBase): Observable<ReorderLevelDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? ReorderLevelDto.fromJS(resultData200) : new ReorderLevelDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<ReorderLevelDto>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: ReorderLevelDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/ReorderLevels/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/ReorderLevels/Delete?";
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
     * @param maxLocIdFilter (optional) 
     * @param minLocIdFilter (optional) 
     * @param itemIdFilter (optional) 
     * @param maxMinLevelFilter (optional) 
     * @param minMinLevelFilter (optional)
     * @param maxMaxLevelFilter (optional) 
     * @param minMaxLevelFilter (optional) 
     * @param maxOrdLevelFilter (optional) 
     * @param minOrdLevelFilter (optional)
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional)
     * @return Success
     */
    getReorderLevelsToExcel(filter: string | null | undefined, maxLocIdFilter: number | null | undefined,minLocIdFilter: number | null | undefined,itemIdFilter: string | null | undefined,maxMinLevelFilter: number | null | undefined,minMinLevelFilter: number | null | undefined,maxMaxLevelFilter: number | null | undefined, minMaxLevelFilter: number | null | undefined, maxOrdLevelFilter: number | null | undefined,minOrdLevelFilter: number | null | undefined, createdByFilter: string | null | undefined,  maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined,audtUserFilter: string | null | undefined,maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/ReorderLevels/GetReorderLevelsToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (maxLocIdFilter !== undefined)
            url_ += "MaxLocIdFilter=" + encodeURIComponent("" + maxLocIdFilter) + "&"; 
        if (minLocIdFilter !== undefined)
            url_ += "MinLocIdFilter=" + encodeURIComponent("" + minLocIdFilter) + "&";  
        if (itemIdFilter !== undefined)
            url_ += "ItemIdFilter=" + encodeURIComponent("" + itemIdFilter) + "&"; 
        if (maxMinLevelFilter !== undefined)
            url_ += "MaxMinLevelFilter=" + encodeURIComponent("" + maxMinLevelFilter) + "&"; 
        if (minMinLevelFilter !== undefined)
            url_ += "MinMinLevelFilter=" + encodeURIComponent("" + minMinLevelFilter) + "&"; 
        if (maxMaxLevelFilter !== undefined)
            url_ += "MaxMaxLevelFilter=" + encodeURIComponent("" + maxMaxLevelFilter) + "&"; 
        if (minMaxLevelFilter !== undefined)
            url_ += "MinMaxLevelFilter=" + encodeURIComponent("" + minMaxLevelFilter) + "&"; 
        if (maxOrdLevelFilter !== undefined)
            url_ += "MaxOrdLevelFilter=" + encodeURIComponent("" + maxOrdLevelFilter) + "&"; 
        if (minOrdLevelFilter !== undefined)
            url_ += "MinOrdLevelFilter=" + encodeURIComponent("" + minOrdLevelFilter) + "&"; 
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
            return this.processGetReorderLevelsToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetReorderLevelsToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetReorderLevelsToExcel(response: HttpResponseBase): Observable<FileDto> {
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