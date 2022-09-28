import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, blobToText, FileDto, throwException, PagedResultDtoOfNameValueDto } from "@shared/service-proxies/service-proxies";
import * as moment from 'moment';
import { PagedResultDtoOfGetAttendanceDetailForViewDto, GetAttendanceDetailForViewDto, GetAttendanceDetailForEditOutput, CreateOrEditAttendanceDetailDto, PagedResultDtoOfAttendanceDetailDto } from '../dto/attendanceDetail-dto';
import { IPagedResultDtoOfAttendanceDetailsDto } from '../interface/attendanceDetail-interface';

@Injectable({
    providedIn: 'root'
})

export class AttendanceDetailsServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param maxEmployeeIDFilter (optional) 
     * @param minEmployeeIDFilter (optional) 
     * @param employeeNameFilter (optional) 
     * @param maxAttendanceDateFilter (optional) 
     * @param minAttendanceDateFilter (optional) 
     * @param maxShiftIDFilter (optional) 
     * @param minShiftIDFilter (optional) 
     * @param maxDetIDFilter (optional) 
     * @param minDetIDFilter (optional) 
     * @param maxTimeInFilter (optional) 
     * @param minTimeInFilter (optional) 
     * @param maxTimeOutFilter (optional) 
     * @param minTimeOutFilter (optional) 
     * @param maxBreakOutFilter (optional) 
     * @param minBreakOutFilter (optional) 
     * @param maxBreakInFilter (optional) 
     * @param minBreakInFilter (optional) 
     * @param maxTotalHrsFilter (optional) 
     * @param minTotalHrsFilter (optional) 
     * @param sorting (optional) 
     * @param skipCount (optional) 
     * @param maxResultCount (optional) 
     * @return Success
     */
    getAll(filter: string | null | undefined, maxEmployeeIDFilter: number | null | undefined, minEmployeeIDFilter: number | null | undefined, employeeNameFilter: string | null | undefined, maxAttendanceDateFilter: moment.Moment | null | undefined, minAttendanceDateFilter: moment.Moment | null | undefined, maxShiftIDFilter: number | null | undefined, minShiftIDFilter: number | null | undefined, maxDetIDFilter: number | null | undefined, minDetIDFilter: number | null | undefined, maxTimeInFilter: moment.Moment | null | undefined, minTimeInFilter: moment.Moment | null | undefined, maxTimeOutFilter: moment.Moment | null | undefined, minTimeOutFilter: moment.Moment | null | undefined, maxBreakOutFilter: moment.Moment | null | undefined, minBreakOutFilter: moment.Moment | null | undefined, maxBreakInFilter: moment.Moment | null | undefined, minBreakInFilter: moment.Moment | null | undefined, maxTotalHrsFilter: number | null | undefined, minTotalHrsFilter: number | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetAttendanceDetailForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/AttendanceDetails/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxEmployeeIDFilter !== undefined)
            url_ += "MaxEmployeeIDFilter=" + encodeURIComponent("" + maxEmployeeIDFilter) + "&";
        if (minEmployeeIDFilter !== undefined)
            url_ += "MinEmployeeIDFilter=" + encodeURIComponent("" + minEmployeeIDFilter) + "&";
        if (employeeNameFilter !== undefined)
            url_ += "EmployeeNameFilter=" + encodeURIComponent("" + employeeNameFilter) + "&";
        if (maxAttendanceDateFilter !== undefined)
            url_ += "MaxAttendanceDateFilter=" + encodeURIComponent(maxAttendanceDateFilter ? "" + maxAttendanceDateFilter.toJSON() : "") + "&";
        if (minAttendanceDateFilter !== undefined)
            url_ += "MinAttendanceDateFilter=" + encodeURIComponent(minAttendanceDateFilter ? "" + minAttendanceDateFilter.toJSON() : "") + "&";
        if (maxShiftIDFilter !== undefined)
            url_ += "MaxShiftIDFilter=" + encodeURIComponent("" + maxShiftIDFilter) + "&";
        if (minShiftIDFilter !== undefined)
            url_ += "MinShiftIDFilter=" + encodeURIComponent("" + minShiftIDFilter) + "&";
        if (maxDetIDFilter !== undefined)
            url_ += "maxDetIDFilter=" + encodeURIComponent("" + maxDetIDFilter) + "&";
        if (minDetIDFilter !== undefined)
            url_ += "minDetIDFilter=" + encodeURIComponent("" + minDetIDFilter) + "&";
        if (maxTimeInFilter !== undefined)
            url_ += "MaxTimeInFilter=" + encodeURIComponent(maxTimeInFilter ? "" + maxTimeInFilter.toJSON() : "") + "&";
        if (minTimeInFilter !== undefined)
            url_ += "MinTimeInFilter=" + encodeURIComponent(minTimeInFilter ? "" + minTimeInFilter.toJSON() : "") + "&";
        if (maxTimeOutFilter !== undefined)
            url_ += "MaxTimeOutFilter=" + encodeURIComponent(maxTimeOutFilter ? "" + maxTimeOutFilter.toJSON() : "") + "&";
        if (minTimeOutFilter !== undefined)
            url_ += "MinTimeOutFilter=" + encodeURIComponent(minTimeOutFilter ? "" + minTimeOutFilter.toJSON() : "") + "&";
        if (maxBreakOutFilter !== undefined)
            url_ += "MaxBreakOutFilter=" + encodeURIComponent(maxBreakOutFilter ? "" + maxBreakOutFilter.toJSON() : "") + "&";
        if (minBreakOutFilter !== undefined)
            url_ += "MinBreakOutFilter=" + encodeURIComponent(minBreakOutFilter ? "" + minBreakOutFilter.toJSON() : "") + "&";
        if (maxBreakInFilter !== undefined)
            url_ += "MaxBreakInFilter=" + encodeURIComponent(maxBreakInFilter ? "" + maxBreakInFilter.toJSON() : "") + "&";
        if (minBreakInFilter !== undefined)
            url_ += "MinBreakInFilter=" + encodeURIComponent(minBreakInFilter ? "" + minBreakInFilter.toJSON() : "") + "&";
        if (maxTotalHrsFilter !== undefined)
            url_ += "MaxTotalHrsFilter=" + encodeURIComponent("" + maxTotalHrsFilter) + "&";
        if (minTotalHrsFilter !== undefined)
            url_ += "MinTotalHrsFilter=" + encodeURIComponent("" + minTotalHrsFilter) + "&";
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
                    return <Observable<PagedResultDtoOfGetAttendanceDetailForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetAttendanceDetailForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetAttendanceDetailForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetAttendanceDetailForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetAttendanceDetailForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetAttendanceDetailForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getAttendanceDetailForView(id: number | null | undefined): Observable<GetAttendanceDetailForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/AttendanceDetails/GetAttendanceDetailForView?";
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
            return this.processGetAttendanceDetailForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAttendanceDetailForView(<any>response_);
                } catch (e) {
                    return <Observable<GetAttendanceDetailForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetAttendanceDetailForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAttendanceDetailForView(response: HttpResponseBase): Observable<GetAttendanceDetailForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetAttendanceDetailForViewDto.fromJS(resultData200) : new GetAttendanceDetailForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetAttendanceDetailForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getAttendanceDetailForEdit(id: number | null | undefined): Observable<GetAttendanceDetailForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/AttendanceDetails/GetAttendanceDetailForEdit?";
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
            return this.processGetAttendanceDetailForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAttendanceDetailForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetAttendanceDetailForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetAttendanceDetailForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetAttendanceDetailForEdit(response: HttpResponseBase): Observable<GetAttendanceDetailForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetAttendanceDetailForEditOutput.fromJS(resultData200) : new GetAttendanceDetailForEditOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetAttendanceDetailForEditOutput>(<any>null);
    }

    getEmployeeDetails(): Observable<IPagedResultDtoOfAttendanceDetailsDto> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/AttendanceDetail/GetEmployeesData";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetEmployeeDetails(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeeDetails(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfAttendanceDetailDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfAttendanceDetailDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetEmployeeDetails(response: HttpResponseBase): Observable<PagedResultDtoOfAttendanceDetailDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfAttendanceDetailDto.fromJS(resultData200) : new PagedResultDtoOfAttendanceDetailDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfAttendanceDetailDto>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditAttendanceDetailDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/AttendanceDetails/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/AttendanceDetails/Delete?";
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
     * @param id (optional) 
     * @return Success
     */
    getAttendanceInExcelFile(id: number | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/AttendanceDetail/GetAttendanceInExcelFile?";
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
            return this.processGetAttendanceInExcelFile(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAttendanceInExcelFile(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAttendanceInExcelFile(response: HttpResponseBase): Observable<FileDto> {
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
}