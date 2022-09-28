import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PagedResultDtoOfGetEmployeeLeavesForViewDto, GetEmployeeLeavesForViewDto, GetEmployeeLeavesForEditOutput, CreateOrEditEmployeeLeavesDto } from '../dto/employeeLeaves-dto';

@Injectable({
    providedIn: 'root'
})

export class EmployeeLeavesServiceProxy {
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
     * @param maxLeaveIDFilter (optional) 
     * @param minLeaveIDFilter (optional) 
     * @param maxSalaryYearFilter (optional) 
     * @param minSalaryYearFilter (optional) 
     * @param maxSalaryMonthFilter (optional) 
     * @param minSalaryMonthFilter (optional) 
     * @param maxStartDateFilter (optional) 
     * @param minStartDateFilter (optional) 
     * @param maxLeaveTypeFilter (optional) 
     * @param minLeaveTypeFilter (optional) 
     * @param maxCasualFilter (optional) 
     * @param minCasualFilter (optional) 
     * @param maxSickFilter (optional) 
     * @param minSickFilter (optional) 
     * @param maxAnnualFilter (optional) 
     * @param minAnnualFilter (optional) 
     * @param payTypeFilter (optional) 
     * @param remarksFilter (optional) 
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
    getAll(filter: string | null | undefined, maxEmployeeIDFilter: number | null | undefined, minEmployeeIDFilter: number | null | undefined, maxLeaveIDFilter: number | null | undefined, minLeaveIDFilter: number | null | undefined, maxSalaryYearFilter: number | null | undefined, minSalaryYearFilter: number | null | undefined, maxSalaryMonthFilter: number | null | undefined, minSalaryMonthFilter: number | null | undefined, maxStartDateFilter: moment.Moment | null | undefined, minStartDateFilter: moment.Moment | null | undefined, maxLeaveTypeFilter: number | null | undefined, minLeaveTypeFilter: number | null | undefined, maxCasualFilter: number | null | undefined, minCasualFilter: number | null | undefined, maxSickFilter: number | null | undefined, minSickFilter: number | null | undefined, maxAnnualFilter: number | null | undefined, minAnnualFilter: number | null | undefined, payTypeFilter: string | null | undefined, remarksFilter: string | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetEmployeeLeavesForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeLeaves/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxEmployeeIDFilter !== undefined)
            url_ += "MaxEmployeeIDFilter=" + encodeURIComponent("" + maxEmployeeIDFilter) + "&";
        if (minEmployeeIDFilter !== undefined)
            url_ += "MinEmployeeIDFilter=" + encodeURIComponent("" + minEmployeeIDFilter) + "&";
        if (maxLeaveIDFilter !== undefined)
            url_ += "MaxLeaveIDFilter=" + encodeURIComponent("" + maxLeaveIDFilter) + "&";
        if (minLeaveIDFilter !== undefined)
            url_ += "MinLeaveIDFilter=" + encodeURIComponent("" + minLeaveIDFilter) + "&";
        if (maxSalaryYearFilter !== undefined)
            url_ += "MaxSalaryYearFilter=" + encodeURIComponent("" + maxSalaryYearFilter) + "&";
        if (minSalaryYearFilter !== undefined)
            url_ += "MinSalaryYearFilter=" + encodeURIComponent("" + minSalaryYearFilter) + "&";
        if (maxSalaryMonthFilter !== undefined)
            url_ += "MaxSalaryMonthFilter=" + encodeURIComponent("" + maxSalaryMonthFilter) + "&";
        if (minSalaryMonthFilter !== undefined)
            url_ += "MinSalaryMonthFilter=" + encodeURIComponent("" + minSalaryMonthFilter) + "&";
        if (maxStartDateFilter !== undefined)
            url_ += "MaxStartDateFilter=" + encodeURIComponent(maxStartDateFilter ? "" + maxStartDateFilter.toJSON() : "") + "&";
        if (minStartDateFilter !== undefined)
            url_ += "MinStartDateFilter=" + encodeURIComponent(minStartDateFilter ? "" + minStartDateFilter.toJSON() : "") + "&";
        if (maxLeaveTypeFilter !== undefined)
            url_ += "MaxLeaveTypeFilter=" + encodeURIComponent("" + maxLeaveTypeFilter) + "&";
        if (minLeaveTypeFilter !== undefined)
            url_ += "MinLeaveTypeFilter=" + encodeURIComponent("" + minLeaveTypeFilter) + "&";
        if (maxCasualFilter !== undefined)
            url_ += "MaxCasualFilter=" + encodeURIComponent("" + maxCasualFilter) + "&";
        if (minCasualFilter !== undefined)
            url_ += "MinCasualFilter=" + encodeURIComponent("" + minCasualFilter) + "&";
        if (maxSickFilter !== undefined)
            url_ += "MaxSickFilter=" + encodeURIComponent("" + maxSickFilter) + "&";
        if (minSickFilter !== undefined)
            url_ += "MinSickFilter=" + encodeURIComponent("" + minSickFilter) + "&";
        if (maxAnnualFilter !== undefined)
            url_ += "MaxAnnualFilter=" + encodeURIComponent("" + maxAnnualFilter) + "&";
        if (minAnnualFilter !== undefined)
            url_ += "MinAnnualFilter=" + encodeURIComponent("" + minAnnualFilter) + "&";
        if (payTypeFilter !== undefined)
            url_ += "PayTypeFilter=" + encodeURIComponent("" + payTypeFilter) + "&";
        if (remarksFilter !== undefined)
            url_ += "RemarksFilter=" + encodeURIComponent("" + remarksFilter) + "&";
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
                    return <Observable<PagedResultDtoOfGetEmployeeLeavesForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetEmployeeLeavesForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetEmployeeLeavesForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetEmployeeLeavesForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetEmployeeLeavesForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetEmployeeLeavesForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getEmployeeLeavesForView(id: number | null | undefined): Observable<GetEmployeeLeavesForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeLeaves/GetEmployeeLeavesForView?";
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
            return this.processGetEmployeeLeavesForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeeLeavesForView(<any>response_);
                } catch (e) {
                    return <Observable<GetEmployeeLeavesForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetEmployeeLeavesForViewDto>><any>_observableThrow(response_);
        }));
    }
   
    getEmployeeLeavesForBalance(employeeID: number | null | undefined): Observable<GetEmployeeLeavesForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeLeaves/GetEmployeeLeaves?";
        if (employeeID !== undefined)
       var year= (new Date()).getFullYear();
            url_ += "salaryYear=" + encodeURIComponent("" + year) + "&";
            url_ += "frmEmpId=" + encodeURIComponent("" + employeeID) + "&";
            url_ += "toEmpId=" + encodeURIComponent("" + employeeID) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetEmployeeLeavesForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeeLeavesForView(<any>response_);
                } catch (e) {
                    return <Observable<GetEmployeeLeavesForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetEmployeeLeavesForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetEmployeeLeavesForView(response: HttpResponseBase): Observable<GetEmployeeLeavesForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetEmployeeLeavesForViewDto.fromJS(resultData200) : new GetEmployeeLeavesForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetEmployeeLeavesForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getEmployeeLeavesForEdit(id: number | null | undefined): Observable<GetEmployeeLeavesForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeLeaves/GetEmployeeLeavesForEdit?";
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
            return this.processGetEmployeeLeavesForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeeLeavesForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetEmployeeLeavesForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetEmployeeLeavesForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetEmployeeLeavesForEdit(response: HttpResponseBase): Observable<GetEmployeeLeavesForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetEmployeeLeavesForEditOutput.fromJS(resultData200) : new GetEmployeeLeavesForEditOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetEmployeeLeavesForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditEmployeeLeavesDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeLeaves/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/EmployeeLeaves/Delete?";
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
     * @param maxEmployeeIDFilter (optional) 
     * @param minEmployeeIDFilter (optional) 
     * @param maxLeaveIDFilter (optional) 
     * @param minLeaveIDFilter (optional) 
     * @param maxSalaryYearFilter (optional) 
     * @param minSalaryYearFilter (optional) 
     * @param maxSalaryMonthFilter (optional) 
     * @param minSalaryMonthFilter (optional) 
     * @param maxStartDateFilter (optional) 
     * @param minStartDateFilter (optional) 
     * @param maxLeaveTypeFilter (optional) 
     * @param minLeaveTypeFilter (optional) 
     * @param maxCasualFilter (optional) 
     * @param minCasualFilter (optional) 
     * @param maxSickFilter (optional) 
     * @param minSickFilter (optional) 
     * @param maxAnnualFilter (optional) 
     * @param minAnnualFilter (optional) 
     * @param payTypeFilter (optional) 
     * @param remarksFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @return Success
     */
    getEmployeeLeavesToExcel(filter: string | null | undefined, maxEmployeeIDFilter: number | null | undefined, minEmployeeIDFilter: number | null | undefined, maxLeaveIDFilter: number | null | undefined, minLeaveIDFilter: number | null | undefined, maxSalaryYearFilter: number | null | undefined, minSalaryYearFilter: number | null | undefined, maxSalaryMonthFilter: number | null | undefined, minSalaryMonthFilter: number | null | undefined, maxStartDateFilter: moment.Moment | null | undefined, minStartDateFilter: moment.Moment | null | undefined, maxLeaveTypeFilter: number | null | undefined, minLeaveTypeFilter: number | null | undefined, maxCasualFilter: number | null | undefined, minCasualFilter: number | null | undefined, maxSickFilter: number | null | undefined, minSickFilter: number | null | undefined, maxAnnualFilter: number | null | undefined, minAnnualFilter: number | null | undefined, payTypeFilter: string | null | undefined, remarksFilter: string | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeLeaves/GetEmployeeLeavesToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxEmployeeIDFilter !== undefined)
            url_ += "MaxEmployeeIDFilter=" + encodeURIComponent("" + maxEmployeeIDFilter) + "&";
        if (minEmployeeIDFilter !== undefined)
            url_ += "MinEmployeeIDFilter=" + encodeURIComponent("" + minEmployeeIDFilter) + "&";
        if (maxLeaveIDFilter !== undefined)
            url_ += "MaxLeaveIDFilter=" + encodeURIComponent("" + maxLeaveIDFilter) + "&";
        if (minLeaveIDFilter !== undefined)
            url_ += "MinLeaveIDFilter=" + encodeURIComponent("" + minLeaveIDFilter) + "&";
        if (maxSalaryYearFilter !== undefined)
            url_ += "MaxSalaryYearFilter=" + encodeURIComponent("" + maxSalaryYearFilter) + "&";
        if (minSalaryYearFilter !== undefined)
            url_ += "MinSalaryYearFilter=" + encodeURIComponent("" + minSalaryYearFilter) + "&";
        if (maxSalaryMonthFilter !== undefined)
            url_ += "MaxSalaryMonthFilter=" + encodeURIComponent("" + maxSalaryMonthFilter) + "&";
        if (minSalaryMonthFilter !== undefined)
            url_ += "MinSalaryMonthFilter=" + encodeURIComponent("" + minSalaryMonthFilter) + "&";
        if (maxStartDateFilter !== undefined)
            url_ += "MaxStartDateFilter=" + encodeURIComponent(maxStartDateFilter ? "" + maxStartDateFilter.toJSON() : "") + "&";
        if (minStartDateFilter !== undefined)
            url_ += "MinStartDateFilter=" + encodeURIComponent(minStartDateFilter ? "" + minStartDateFilter.toJSON() : "") + "&";
        if (maxLeaveTypeFilter !== undefined)
            url_ += "MaxLeaveTypeFilter=" + encodeURIComponent("" + maxLeaveTypeFilter) + "&";
        if (minLeaveTypeFilter !== undefined)
            url_ += "MinLeaveTypeFilter=" + encodeURIComponent("" + minLeaveTypeFilter) + "&";
        if (maxCasualFilter !== undefined)
            url_ += "MaxCasualFilter=" + encodeURIComponent("" + maxCasualFilter) + "&";
        if (minCasualFilter !== undefined)
            url_ += "MinCasualFilter=" + encodeURIComponent("" + minCasualFilter) + "&";
        if (maxSickFilter !== undefined)
            url_ += "MaxSickFilter=" + encodeURIComponent("" + maxSickFilter) + "&";
        if (minSickFilter !== undefined)
            url_ += "MinSickFilter=" + encodeURIComponent("" + minSickFilter) + "&";
        if (maxAnnualFilter !== undefined)
            url_ += "MaxAnnualFilter=" + encodeURIComponent("" + maxAnnualFilter) + "&";
        if (minAnnualFilter !== undefined)
            url_ += "MinAnnualFilter=" + encodeURIComponent("" + minAnnualFilter) + "&";
        if (payTypeFilter !== undefined)
            url_ += "PayTypeFilter=" + encodeURIComponent("" + payTypeFilter) + "&";
        if (remarksFilter !== undefined)
            url_ += "RemarksFilter=" + encodeURIComponent("" + remarksFilter) + "&";
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
            return this.processGetEmployeeLeavesToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeeLeavesToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetEmployeeLeavesToExcel(response: HttpResponseBase): Observable<FileDto> {
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

    getMaxLeaveId(): Observable<number> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeLeaves/GetMaxID";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetMaxLeaveId(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetMaxLeaveId(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processGetMaxLeaveId(response: HttpResponseBase): Observable<number> {
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

    getEmployeeName(id: number): Observable<string> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeLeaves/GetEmployeeName?";
        if (id !== undefined)
            url_ += "id=" + encodeURIComponent("" + id);
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processgetEmployeeName(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processgetEmployeeName(<any>response_);
                } catch (e) {
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else
                return <Observable<string>><any>_observableThrow(response_);
        }));
    }

    protected processgetEmployeeName(response: HttpResponseBase): Observable<string> {
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
}