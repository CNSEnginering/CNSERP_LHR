import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, blobToText, FileDto, throwException, PagedResultDtoOfNameValueDto } from "@shared/service-proxies/service-proxies";
import * as moment from 'moment';
import { PagedResultDtoOfGetHolidaysForViewDto, GetHolidaysForViewDto, GetHolidaysForEditOutput, CreateOrEditHolidaysDto } from '../dto/holidays-dto';

@Injectable({
    providedIn: 'root'
})
export class HolidaysServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional)
     * @param maxHolidayIDFilter (optional)
     * @param minHolidayIDFilter (optional)
     * @param maxHolidayDateFilter (optional)
     * @param minHolidayDateFilter (optional)
     * @param holidayNameFilter (optional)
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
    getAll(filter: string | null | undefined, maxHolidayIDFilter: number | null | undefined, minHolidayIDFilter: number | null | undefined, maxHolidayDateFilter: moment.Moment | null | undefined, minHolidayDateFilter: moment.Moment | null | undefined, holidayNameFilter: string | null | undefined, activeFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetHolidaysForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/Holidays/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxHolidayIDFilter !== undefined)
            url_ += "MaxHolidayIDFilter=" + encodeURIComponent("" + maxHolidayIDFilter) + "&";
        if (minHolidayIDFilter !== undefined)
            url_ += "MinHolidayIDFilter=" + encodeURIComponent("" + minHolidayIDFilter) + "&";
        if (maxHolidayDateFilter !== undefined)
            url_ += "MaxHolidayDateFilter=" + encodeURIComponent(maxHolidayDateFilter ? "" + maxHolidayDateFilter.toJSON() : "") + "&";
        if (minHolidayDateFilter !== undefined)
            url_ += "MinHolidayDateFilter=" + encodeURIComponent(minHolidayDateFilter ? "" + minHolidayDateFilter.toJSON() : "") + "&";
        if (holidayNameFilter !== undefined)
            url_ += "HolidayNameFilter=" + encodeURIComponent("" + holidayNameFilter) + "&";
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
                    return <Observable<PagedResultDtoOfGetHolidaysForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetHolidaysForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetHolidaysForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetHolidaysForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetHolidaysForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetHolidaysForViewDto>(<any>null);
    }

    /**
     * @param id (optional)
     * @return Success
     */
    getHolidaysForView(id: number | null | undefined): Observable<GetHolidaysForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/Holidays/GetHolidaysForView?";
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
            return this.processGetHolidaysForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetHolidaysForView(<any>response_);
                } catch (e) {
                    return <Observable<GetHolidaysForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetHolidaysForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetHolidaysForView(response: HttpResponseBase): Observable<GetHolidaysForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetHolidaysForViewDto.fromJS(resultData200) : new GetHolidaysForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetHolidaysForViewDto>(<any>null);
    }

    /**
     * @param id (optional)
     * @return Success
     */
    getHolidaysForEdit(id: number | null | undefined): Observable<GetHolidaysForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/Holidays/GetHolidaysForEdit?";
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
            return this.processGetHolidaysForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetHolidaysForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetHolidaysForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetHolidaysForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetHolidaysForEdit(response: HttpResponseBase): Observable<GetHolidaysForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetHolidaysForEditOutput.fromJS(resultData200) : new GetHolidaysForEditOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetHolidaysForEditOutput>(<any>null);
    }

    /**
     * @param input (optional)
     * @return Success
     */
    createOrEdit(input: CreateOrEditHolidaysDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/Holidays/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/Holidays/Delete?";
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
     * @param maxHolidayIDFilter (optional)
     * @param minHolidayIDFilter (optional)
     * @param maxHolidayDateFilter (optional)
     * @param minHolidayDateFilter (optional)
     * @param holidayNameFilter (optional)
     * @param activeFilter (optional)
     * @param audtUserFilter (optional)
     * @param maxAudtDateFilter (optional)
     * @param minAudtDateFilter (optional)
     * @param createdByFilter (optional)
     * @param maxCreateDateFilter (optional)
     * @param minCreateDateFilter (optional)
     * @return Success
     */
    getHolidaysToExcel(filter: string | null | undefined, maxHolidayIDFilter: number | null | undefined, minHolidayIDFilter: number | null | undefined, maxHolidayDateFilter: moment.Moment | null | undefined, minHolidayDateFilter: moment.Moment | null | undefined, holidayNameFilter: string | null | undefined, activeFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/Holidays/GetHolidaysToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxHolidayIDFilter !== undefined)
            url_ += "MaxHolidayIDFilter=" + encodeURIComponent("" + maxHolidayIDFilter) + "&";
        if (minHolidayIDFilter !== undefined)
            url_ += "MinHolidayIDFilter=" + encodeURIComponent("" + minHolidayIDFilter) + "&";
        if (maxHolidayDateFilter !== undefined)
            url_ += "MaxHolidayDateFilter=" + encodeURIComponent(maxHolidayDateFilter ? "" + maxHolidayDateFilter.toJSON() : "") + "&";
        if (minHolidayDateFilter !== undefined)
            url_ += "MinHolidayDateFilter=" + encodeURIComponent(minHolidayDateFilter ? "" + minHolidayDateFilter.toJSON() : "") + "&";
        if (holidayNameFilter !== undefined)
            url_ += "HolidayNameFilter=" + encodeURIComponent("" + holidayNameFilter) + "&";
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
            return this.processGetHolidaysToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetHolidaysToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetHolidaysToExcel(response: HttpResponseBase): Observable<FileDto> {
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

    getMaxHolidayId(): Observable<number> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/Holidays/GetMaxID";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetMaxHolidaytId(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetMaxHolidaytId(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processGetMaxHolidaytId(response: HttpResponseBase): Observable<number> {
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
