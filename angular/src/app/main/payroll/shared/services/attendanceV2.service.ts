import { mergeMap as _observableMergeMap, catchError as _observableCatch, map } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, blobToText, throwException } from "@shared/service-proxies/service-proxies";
import { CreateOrEditAttendanceDto, EmployeeDataForAttendanceDto } from '../dto/attendanceV2-dto';
import * as moment from 'moment';
import { PagedResultDtoOfAttendanceDetailDto, CreateOrEditAttendanceDetailDto, AttendanceDetailDto } from '../dto/attendanceDetail-dto';
import { IPagedResultDtoOfAttendanceDetailsDto } from '../interface/attendanceDetail-interface';
import { CreateOrEditAttendanceHeaderDto } from '../dto/attendanceHeader-dto';


@Injectable({
    providedIn: 'root'
})

export class AttendanceServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    url: string = "";
    url_: string = "";

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param input (optional)
     * @return Success
     */
    updateAttendance(input: CreateOrEditAttendanceDto | null | undefined): Observable<string> {
        let url_ = this.baseUrl + "/api/services/app/Attendance/UpdateAttendance";
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

        return this.http.request("put", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processUpdateAttendance(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processUpdateAttendance(<any>response_);
                } catch (e) {
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else
                return <Observable<string>><any>_observableThrow(response_);
        }));
    }

    protected processUpdateAttendance(response: HttpResponseBase): Observable<string> {
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
        return _observableOf<string>(<any>null);
    }


    /**
     * @param employeeID (optional)
     * @param attendanceDate (optional)
     * @return Success
     */
    employeeDataForAttendance(employeeID: number | null | undefined, attendanceDate: moment.Moment | null | undefined): Observable<EmployeeDataForAttendanceDto> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/Attendance/EmployeeDataForAttendance?";
        if (employeeID !== undefined)
            url_ += "employeeID=" + encodeURIComponent("" + employeeID) + "&";
        if (attendanceDate !== undefined)
            url_ += "attendanceDate=" + encodeURIComponent(attendanceDate ? "" + attendanceDate.toJSON() : "") + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processEmployeeDataForAttendance(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processEmployeeDataForAttendance(<any>response_);
                } catch (e) {
                    return <Observable<EmployeeDataForAttendanceDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<EmployeeDataForAttendanceDto>><any>_observableThrow(response_);
        }));
    }

    protected processEmployeeDataForAttendance(response: HttpResponseBase): Observable<EmployeeDataForAttendanceDto> {
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
                result200 = resultData200 ? EmployeeDataForAttendanceDto.fromJS(resultData200) : new EmployeeDataForAttendanceDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<EmployeeDataForAttendanceDto>(<any>null);
    }

    /**
     * @param fromDate
     * @param toDate
     * @return Success
     */
    markWholeMonthAttendance(fromDate: moment.Moment | null | undefined, toDate: moment.Moment | null | undefined): Observable<string> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/Attendance/MarkWholeMonthAttendance?";
        if (fromDate !== undefined)
            url_ += "fromDate=" + encodeURIComponent(fromDate ? "" + fromDate.toJSON() : "") + "&";
        if (toDate !== undefined)
            url_ += "toDate=" + encodeURIComponent(toDate ? "" + toDate.toJSON() : "") + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processMarkWholeMonthAttendance(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processMarkWholeMonthAttendance(<any>response_);
                } catch (e) {
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else
                return <Observable<string>><any>_observableThrow(response_);
        }));
    }

    protected processMarkWholeMonthAttendance(response: HttpResponseBase): Observable<string> {
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
        return _observableOf<string>(<any>null);
    }

    /**
 * @param fromDate
 * @param toDate
 * @return Success
 */
    scheduleAttendance(fromDate: moment.Moment | null | undefined, toDate: moment.Moment | null | undefined): Observable<string> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/Attendance/ScheduleAttendance?";
        if (fromDate !== undefined)
            url_ += "fromDate=" + encodeURIComponent(fromDate ? "" + fromDate.toJSON() : "") + "&";
        if (toDate !== undefined)
            url_ += "toDate=" + encodeURIComponent(toDate ? "" + toDate.toJSON() : "") + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processScheduleAttendancet(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processScheduleAttendancet(<any>response_);
                } catch (e) {
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else
                return <Observable<string>><any>_observableThrow(response_);
        }));
    }

    protected processScheduleAttendancet(response: HttpResponseBase): Observable<string> {
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
        return _observableOf<string>(<any>null);
    }

    getAttendanceData(attendanceDate: moment.Moment | null | undefined): Observable<IPagedResultDtoOfAttendanceDetailsDto> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/Attendance/GetAttendanceData?";
        if (attendanceDate !== undefined)
            url_ += "attendanceDate=" + encodeURIComponent(attendanceDate ? "" + attendanceDate.toJSON() : "") + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetAttendanceData(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAttendanceData(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfAttendanceDetailDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfAttendanceDetailDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAttendanceData(response: HttpResponseBase): Observable<PagedResultDtoOfAttendanceDetailDto> {
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
    updateBulkAttendance(input: CreateOrEditAttendanceHeaderDto | null | undefined): Observable<void> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/Attendance/UpdateBulkAttendance";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(input);

        console.log(content_);
        let options_: any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json",
            })
        };

        return this.http.request("put", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processUpdateBulkAttendance(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processUpdateBulkAttendance(<any>response_);
                } catch (e) {
                    return <Observable<void>><any>_observableThrow(e);
                }
            } else
                return <Observable<void>><any>_observableThrow(response_);
        }));
    }

    protected processUpdateBulkAttendance(response: HttpResponseBase): Observable<void> {
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



    getEmployeeAttendanceData(attendanceDate: Date, empId: number) {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/Attendance/GetEmployeeAttendance?";
        if (attendanceDate !== undefined)
            url_ += "attendanceDate=" + encodeURIComponent(attendanceDate ? "" + attendanceDate.toJSON() : "") + "&";
        if (empId !== undefined)
            url_ += "empId=" + encodeURIComponent(empId.toString()) + "&";
        url_ = url_.replace(/[?&]$/, "");


        return this.http.request("get", url_).pipe(map((response_: any) => {
            return response_["result"] as Observable<IPagedResultDtoOfAttendanceDetailsDto>;
        }));
    }


    updateBulkAttendanceByEmp(input: AttendanceDetailDto[] | null | undefined) {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/Attendance/UpdateBulkAttendanceByEmp";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(input);

        console.log(content_);
        let options_: any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json",
            })
        };

        return this.http.request("put", url_, options_);
    }


}
