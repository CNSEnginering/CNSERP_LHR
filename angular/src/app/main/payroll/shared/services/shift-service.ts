import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, blobToText, FileDto, throwException, PagedResultDtoOfNameValueDto } from "@shared/service-proxies/service-proxies";
import { ShiftDto, GetShiftForViewDto, CreateOrEditShiftDto, PagedResultDtoOfGetShiftForViewDto, GetShiftForEditOutput } from '../dto/shift-dto';
import * as moment from 'moment';
import { data } from 'jquery';

@Injectable({
    providedIn: 'root'
})
export class ShiftServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;


    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {

        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param maxShiftIDFilter (optional) 
     * @param minShiftIDFilter (optional) 
     * @param shiftNameFilter (optional) 
     * @param maxStartTimeFilter (optional) 
     * @param minStartTimeFilter (optional) 
     * @param maxEndTimeFilter (optional) 
     * @param minEndTimeFilter (optional) 
     * @param maxBeforeStartFilter (optional) 
     * @param minBeforeStartFilter (optional) 
     * @param maxAfterStartFilter (optional) 
     * @param minAfterStartFilter (optional) 
     * @param maxBeforeFinishFilter (optional) 
     * @param minBeforeFinishFilter (optional) 
     * @param maxAfterFinishFilter (optional) 
     * @param minAfterFinishFilter (optional) 
     * @param maxTotalHourFilter (optional) 
     * @param minTotalHourFilter (optional) 
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
    getAll(filter: string | null | undefined, maxShiftIDFilter: number | null | undefined, minShiftIDFilter: number | null | undefined, shiftNameFilter: string | null | undefined, maxStartTimeFilter: moment.Moment | null | undefined, minStartTimeFilter: moment.Moment | null | undefined,
        maxEndTimeFilter: moment.Moment | null | undefined, minEndTimeFilter: moment.Moment | null | undefined, maxBeforeStartFilter: number | null | undefined, minBeforeStartFilter: number | null | undefined, maxAfterStartFilter: number | null | undefined, minAfterStartFilter: number | null | undefined,
        maxBeforeFinishFilter: number | null | undefined, minBeforeFinishFilter: number | null | undefined, maxAfterFinishFilter: number | null | undefined, minAfterFinishFilter: number | null | undefined, maxTotalHourFilter: number | null | undefined, minTotalHourFilter: number | null | undefined,
        activeFilter: number | null | undefined, auditUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined,
        minCreateDateFilter: moment.Moment | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetShiftForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/Shift/GetAll?";

        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxShiftIDFilter !== undefined)
            url_ += "MaxShiftIDFilter=" + encodeURIComponent("" + maxShiftIDFilter) + "&";
        if (minShiftIDFilter !== undefined)
            url_ += "MinShiftIDFilter=" + encodeURIComponent("" + minShiftIDFilter) + "&";
        if (shiftNameFilter !== undefined)
            url_ += "ShiftNameFilter=" + encodeURIComponent("" + shiftNameFilter) + "&";
        if (maxStartTimeFilter !== undefined)
            url_ += "MaxStartTimeFilter=" + encodeURIComponent(maxStartTimeFilter ? "" + maxStartTimeFilter.toJSON() : "") + "&";
        if (minStartTimeFilter !== undefined)
            url_ += "MinStartTimeFilter=" + encodeURIComponent(minStartTimeFilter ? "" + minStartTimeFilter.toJSON() : "") + "&";
        if (maxEndTimeFilter !== undefined)
            url_ += "MaxEndTimeFilter=" + encodeURIComponent(maxEndTimeFilter ? "" + maxEndTimeFilter.toJSON() : "") + "&";
        if (minEndTimeFilter !== undefined)
            url_ += "MinEndTimeFilter=" + encodeURIComponent(minEndTimeFilter ? "" + minEndTimeFilter.toJSON() : "") + "&";
        if (maxBeforeStartFilter !== undefined)
            url_ += "MaxBeforeStartFilter=" + encodeURIComponent("" + maxBeforeStartFilter) + "&";
        if (minBeforeStartFilter !== undefined)
            url_ += "MinBeforeStartFilter=" + encodeURIComponent("" + minBeforeStartFilter) + "&";
        if (maxAfterStartFilter !== undefined)
            url_ += "MaxAfterStartFilter=" + encodeURIComponent("" + maxAfterStartFilter) + "&";
        if (minAfterStartFilter !== undefined)
            url_ += "MinAfterStartFilter=" + encodeURIComponent("" + minAfterStartFilter) + "&";
        if (maxBeforeFinishFilter !== undefined)
            url_ += "MaxBeforeFinishFilter=" + encodeURIComponent("" + maxBeforeFinishFilter) + "&";
        if (minBeforeFinishFilter !== undefined)
            url_ += "MinBeforeFinishFilter=" + encodeURIComponent("" + minBeforeFinishFilter) + "&";
        if (maxAfterFinishFilter !== undefined)
            url_ += "MaxAfterFinishFilter=" + encodeURIComponent("" + maxAfterFinishFilter) + "&";
        if (minAfterFinishFilter !== undefined)
            url_ += "MinAfterFinishFilter=" + encodeURIComponent("" + minAfterFinishFilter) + "&";
        if (maxTotalHourFilter !== undefined)
            url_ += "MaxTotalHourFilter=" + encodeURIComponent("" + maxTotalHourFilter) + "&";
        if (minTotalHourFilter !== undefined)
            url_ += "MinTotalHourFilter=" + encodeURIComponent("" + minTotalHourFilter) + "&";
        if (activeFilter != undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&";
        if (auditUserFilter !== undefined)
            url_ += "auditUserFilter=" + encodeURIComponent("" + auditUserFilter) + "&";
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
            debugger;
            return this.processGetAll(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {

                    return this.processGetAll(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfGetShiftForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetShiftForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetShiftForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetShiftForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetShiftForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetShiftForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    GetShiftForView(id: number | null | undefined): Observable<GetShiftForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/Shift/GetShiftForView?";
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
            return this.processGetShiftForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetShiftForView(<any>response_);
                } catch (e) {
                    return <Observable<GetShiftForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetShiftForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetShiftForView(response: HttpResponseBase): Observable<GetShiftForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetShiftForViewDto.fromJS(resultData200) : new GetShiftForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetShiftForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getShiftForEdit(id: number | null | undefined): Observable<GetShiftForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/Shift/GetShiftForEdit?";
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

            return this.processGetShiftForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {

                    return this.processGetShiftForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetShiftForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetShiftForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetShiftForEdit(response: HttpResponseBase): Observable<GetShiftForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetShiftForEditOutput.fromJS(resultData200) : new GetShiftForEditOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetShiftForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditShiftDto | null | undefined): Observable<void> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/Shift/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/Shift/Delete?";
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
            debugger;
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
        debugger;
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
    * @param maxShiftIDFilter (optional) 
    * @param minShiftIDFilter (optional) 
    * @param shiftNameFilter (optional) 
    * @param maxStartTimeFilter (optional) 
    * @param minStartTimeFilter (optional) 
    * @param maxEndTimeFilter (optional) 
    * @param minEndTimeFilter (optional) 
    * @param maxBeforeStartFilter (optional) 
    * @param minBeforeStartFilter (optional) 
    * @param maxAfterStartFilter (optional) 
    * @param minAfterStartFilter (optional) 
    * @param maxBeforeFinishFilter (optional) 
    * @param minBeforeFinishFilter (optional) 
    * @param maxAfterFinishFilter (optional) 
    * @param minAfterFinishFilter (optional) 
    * @param maxTotalHourFilter (optional) 
    * @param minTotalHourFilter (optional) 
    * @param activeFilter (optional)
    * @param audtUserFilter (optional) 
    * @param maxAudtDateFilter (optional) 
    * @param minAudtDateFilter (optional) 
    * @param createdByFilter (optional) 
    * @param maxCreateDateFilter (optional) 
    * @param minCreateDateFilter (optional)  
    * @return Success
    */
    GetShiftToExcel(filter: string | null | undefined, maxShiftIDFilter: number | null | undefined, minShiftIDFilter: number | null | undefined, shiftNameFilter: string | null | undefined, maxStartTimeFilter: moment.Moment | null | undefined, minStartTimeFilter: moment.Moment | null | undefined,
        maxEndTimeFilter: moment.Moment | null | undefined, minEndTimeFilter: moment.Moment | null | undefined, maxBeforeStartFilter: number | null | undefined, minBeforeStartFilter: number | null | undefined, maxAfterStartFilter: number | null | undefined, minAfterStartFilter: number | null | undefined,
        maxBeforeFinishFilter: number | null | undefined, minBeforeFinishFilter: number | null | undefined, maxAfterFinishFilter: number | null | undefined, minAfterFinishFilter: number | null | undefined, maxTotalHourFilter: number | null | undefined, minTotalHourFilter: number | null | undefined,
        activeFilter: number | null | undefined, auditUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined,
        minCreateDateFilter: moment.Moment | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/Shift/GetShiftToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxShiftIDFilter !== undefined)
            url_ += "MaxShiftIDFilter=" + encodeURIComponent("" + maxShiftIDFilter) + "&";
        if (minShiftIDFilter !== undefined)
            url_ += "MinShiftIDFilter=" + encodeURIComponent("" + minShiftIDFilter) + "&";
        if (shiftNameFilter !== undefined)
            url_ += "ShiftNameFilter=" + encodeURIComponent("" + shiftNameFilter) + "&";
        if (maxStartTimeFilter !== undefined)
            url_ += "MaxStartTimeFilter=" + encodeURIComponent(maxStartTimeFilter ? "" + maxStartTimeFilter.toJSON() : "") + "&";
        if (minStartTimeFilter !== undefined)
            url_ += "MinStartTimeFilter=" + encodeURIComponent(minStartTimeFilter ? "" + minStartTimeFilter.toJSON() : "") + "&";
        if (maxEndTimeFilter !== undefined)
            url_ += "MaxEndTimeFilter=" + encodeURIComponent(maxEndTimeFilter ? "" + maxEndTimeFilter.toJSON() : "") + "&";
        if (minEndTimeFilter !== undefined)
            url_ += "MinEndTimeFilter=" + encodeURIComponent(minEndTimeFilter ? "" + minEndTimeFilter.toJSON() : "") + "&";
        if (maxBeforeStartFilter !== undefined)
            url_ += "MaxBeforeStartFilter=" + encodeURIComponent("" + maxBeforeStartFilter) + "&";
        if (minBeforeStartFilter !== undefined)
            url_ += "MinBeforeStartFilter=" + encodeURIComponent("" + minBeforeStartFilter) + "&";
        if (maxAfterStartFilter !== undefined)
            url_ += "MaxAfterStartFilter=" + encodeURIComponent("" + maxAfterStartFilter) + "&";
        if (minAfterStartFilter !== undefined)
            url_ += "MinAfterStartFilter=" + encodeURIComponent("" + minAfterStartFilter) + "&";
        if (maxBeforeFinishFilter !== undefined)
            url_ += "MaxBeforeFinishFilter=" + encodeURIComponent("" + maxBeforeFinishFilter) + "&";
        if (minBeforeFinishFilter !== undefined)
            url_ += "MinBeforeFinishFilter=" + encodeURIComponent("" + minBeforeFinishFilter) + "&";
        if (maxAfterFinishFilter !== undefined)
            url_ += "MaxAfterFinishFilter=" + encodeURIComponent("" + maxAfterFinishFilter) + "&";
        if (minAfterFinishFilter !== undefined)
            url_ += "MinAfterFinishFilter=" + encodeURIComponent("" + minAfterFinishFilter) + "&";
        if (maxTotalHourFilter !== undefined)
            url_ += "MaxTotalHourFilter=" + encodeURIComponent("" + maxTotalHourFilter) + "&";
        if (minTotalHourFilter !== undefined)
            url_ += "MinTotalHourFilter=" + encodeURIComponent("" + minTotalHourFilter) + "&";
        if (activeFilter != undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&";
        if (auditUserFilter !== undefined)
            url_ += "auditUserFilter=" + encodeURIComponent("" + auditUserFilter) + "&";
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
            return this.processGetShiftToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetShiftToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetShiftToExcel(response: HttpResponseBase): Observable<FileDto> {
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

    getMaxShiftId(): Observable<number> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/Shift/GetMaxID";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetMaxShiftId(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetMaxShiftId(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processGetMaxShiftId(response: HttpResponseBase): Observable<number> {
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


    getTotalHrs(myTime, myTime1) {
        let url_ = this.baseUrl + "/api/services/app/Shift/getToatalHours?";
        if (myTime !== undefined)
            url_ += "startTime=" + encodeURIComponent("" + myTime) + "&";
        if (myTime1 !== undefined)
            url_ += "endTime=" + encodeURIComponent("" + myTime1) + "&";
        url_ = url_.replace(/[?&]$/, "");

        debugger;
        return this.http.request("get", url_);
    }
}
