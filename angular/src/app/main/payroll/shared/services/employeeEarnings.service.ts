import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PagedResultDtoOfGetEmployeeEarningsForViewDto, GetEmployeeEarningsForViewDto, GetEmployeeEarningsForEditOutput, CreateOrEditEmployeeEarningsDto } from '../dto/employeeEarnings-dto';

@Injectable({
    providedIn: 'root'
})
export class EmployeeEarningsServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param maxEarningIDFilter (optional) 
     * @param minEarningIDFilter (optional) 
     * @param maxEmployeeIDFilter (optional) 
     * @param minEmployeeIDFilter (optional) 
     * @param employeeNameFilter (optional) 
     * @param maxSalaryYearFilter (optional) 
     * @param minSalaryYearFilter (optional) 
     * @param maxSalaryMonthFilter (optional) 
     * @param minSalaryMonthFilter (optional) 
     * @param maxEarningDateFilter (optional) 
     * @param minEarningDateFilter (optional) 
     * @param maxAmountFilter (optional) 
     * @param minAmountFilter (optional) 
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
    getAll(filter: string | null | undefined, maxEarningIDFilter: number | null | undefined, minEarningIDFilter: number | null | undefined, maxEmployeeIDFilter: number | null | undefined, minEmployeeIDFilter: number | null | undefined, employeeNameFilter: string | null | undefined, maxSalaryYearFilter: number | null | undefined, minSalaryYearFilter: number | null | undefined, maxSalaryMonthFilter: number | null | undefined, minSalaryMonthFilter: number | null | undefined, maxEarningDateFilter: moment.Moment | null | undefined, minEarningDateFilter: moment.Moment | null | undefined, maxAmountFilter: number | null | undefined, minAmountFilter: number | null | undefined, activeFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetEmployeeEarningsForViewDto> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeEarnings/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxEarningIDFilter !== undefined)
            url_ += "MaxEarningIDFilter=" + encodeURIComponent("" + maxEarningIDFilter) + "&";
        if (minEarningIDFilter !== undefined)
            url_ += "MinEarningIDFilter=" + encodeURIComponent("" + minEarningIDFilter) + "&";
        if (maxEmployeeIDFilter !== undefined)
            url_ += "MaxEmployeeIDFilter=" + encodeURIComponent("" + maxEmployeeIDFilter) + "&";
        if (minEmployeeIDFilter !== undefined)
            url_ += "MinEmployeeIDFilter=" + encodeURIComponent("" + minEmployeeIDFilter) + "&";
        if (employeeNameFilter !== undefined)
            url_ += "EmployeeNameFilter=" + encodeURIComponent("" + employeeNameFilter) + "&";
        if (maxSalaryYearFilter !== undefined)
            url_ += "MaxSalaryYearFilter=" + encodeURIComponent("" + maxSalaryYearFilter) + "&";
        if (minSalaryYearFilter !== undefined)
            url_ += "MinSalaryYearFilter=" + encodeURIComponent("" + minSalaryYearFilter) + "&";
        if (maxSalaryMonthFilter !== undefined)
            url_ += "MaxSalaryMonthFilter=" + encodeURIComponent("" + maxSalaryMonthFilter) + "&";
        if (minSalaryMonthFilter !== undefined)
            url_ += "MinSalaryMonthFilter=" + encodeURIComponent("" + minSalaryMonthFilter) + "&";
        if (maxEarningDateFilter !== undefined)
            url_ += "MaxEarningDateFilter=" + encodeURIComponent(maxEarningDateFilter ? "" + maxEarningDateFilter.toJSON() : "") + "&";
        if (minEarningDateFilter !== undefined)
            url_ += "MinEarningDateFilter=" + encodeURIComponent(minEarningDateFilter ? "" + minEarningDateFilter.toJSON() : "") + "&";
        if (maxAmountFilter !== undefined)
            url_ += "MaxAmountFilter=" + encodeURIComponent("" + maxAmountFilter) + "&";
        if (minAmountFilter !== undefined)
            url_ += "MinAmountFilter=" + encodeURIComponent("" + minAmountFilter) + "&";
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
                    return <Observable<PagedResultDtoOfGetEmployeeEarningsForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetEmployeeEarningsForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetEmployeeEarningsForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetEmployeeEarningsForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetEmployeeEarningsForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetEmployeeEarningsForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getEmployeeEarningsForView(id: number | null | undefined): Observable<GetEmployeeEarningsForViewDto> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeEarnings/GetEmployeeEarningsForView?";
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
            return this.processGetEmployeeEarningsForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeeEarningsForView(<any>response_);
                } catch (e) {
                    return <Observable<GetEmployeeEarningsForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetEmployeeEarningsForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetEmployeeEarningsForView(response: HttpResponseBase): Observable<GetEmployeeEarningsForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetEmployeeEarningsForViewDto.fromJS(resultData200) : new GetEmployeeEarningsForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetEmployeeEarningsForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getEmployeeEarningsForEdit(id: number | null | undefined): Observable<GetEmployeeEarningsForEditOutput> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeEarnings/GetEmployeeEarningsForEdit?";
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
            return this.processGetEmployeeEarningsForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeeEarningsForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetEmployeeEarningsForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetEmployeeEarningsForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetEmployeeEarningsForEdit(response: HttpResponseBase): Observable<GetEmployeeEarningsForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetEmployeeEarningsForEditOutput.fromJS(resultData200) : new GetEmployeeEarningsForEditOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetEmployeeEarningsForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditEmployeeEarningsDto | null | undefined): Observable<void> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeEarnings/CreateOrEdit";
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
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeEarnings/Delete?";
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
     * @param maxEarningIDFilter (optional) 
     * @param minEarningIDFilter (optional) 
     * @param maxEmployeeIDFilter (optional) 
     * @param minEmployeeIDFilter (optional) 
     * @param employeeNameFilter (optional)
     * @param maxSalaryYearFilter (optional) 
     * @param minSalaryYearFilter (optional) 
     * @param maxSalaryMonthFilter (optional) 
     * @param minSalaryMonthFilter (optional) 
     * @param maxEarningDateFilter (optional) 
     * @param minEarningDateFilter (optional) 
     * @param maxAmountFilter (optional) 
     * @param minAmountFilter (optional) 
     * @param activeFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @return Success
     */
    getEmployeeEarningsToExcel(filter: string | null | undefined, maxEarningIDFilter: number | null | undefined, minEarningIDFilter: number | null | undefined, maxEmployeeIDFilter: number | null | undefined, minEmployeeIDFilter: number | null | undefined, employeeNameFilter: string | null | undefined, maxSalaryYearFilter: number | null | undefined, minSalaryYearFilter: number | null | undefined, maxSalaryMonthFilter: number | null | undefined, minSalaryMonthFilter: number | null | undefined, maxEarningDateFilter: moment.Moment | null | undefined, minEarningDateFilter: moment.Moment | null | undefined, maxAmountFilter: number | null | undefined, minAmountFilter: number | null | undefined, activeFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeEarnings/GetEmployeeEarningsToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxEarningIDFilter !== undefined)
            url_ += "MaxEarningIDFilter=" + encodeURIComponent("" + maxEarningIDFilter) + "&";
        if (minEarningIDFilter !== undefined)
            url_ += "MinEarningIDFilter=" + encodeURIComponent("" + minEarningIDFilter) + "&";
        if (maxEmployeeIDFilter !== undefined)
            url_ += "MaxEmployeeIDFilter=" + encodeURIComponent("" + maxEmployeeIDFilter) + "&";
        if (minEmployeeIDFilter !== undefined)
            url_ += "MinEmployeeIDFilter=" + encodeURIComponent("" + minEmployeeIDFilter) + "&";
        if (employeeNameFilter !== undefined)
            url_ += "EmployeeNameFilter=" + encodeURIComponent("" + employeeNameFilter) + "&";
        if (maxSalaryYearFilter !== undefined)
            url_ += "MaxSalaryYearFilter=" + encodeURIComponent("" + maxSalaryYearFilter) + "&";
        if (minSalaryYearFilter !== undefined)
            url_ += "MinSalaryYearFilter=" + encodeURIComponent("" + minSalaryYearFilter) + "&";
        if (maxSalaryMonthFilter !== undefined)
            url_ += "MaxSalaryMonthFilter=" + encodeURIComponent("" + maxSalaryMonthFilter) + "&";
        if (minSalaryMonthFilter !== undefined)
            url_ += "MinSalaryMonthFilter=" + encodeURIComponent("" + minSalaryMonthFilter) + "&";
        if (maxEarningDateFilter !== undefined)
            url_ += "MaxEarningDateFilter=" + encodeURIComponent(maxEarningDateFilter ? "" + maxEarningDateFilter.toJSON() : "") + "&";
        if (minEarningDateFilter !== undefined)
            url_ += "MinEarningDateFilter=" + encodeURIComponent(minEarningDateFilter ? "" + minEarningDateFilter.toJSON() : "") + "&";
        if (maxAmountFilter !== undefined)
            url_ += "MaxAmountFilter=" + encodeURIComponent("" + maxAmountFilter) + "&";
        if (minAmountFilter !== undefined)
            url_ += "MinAmountFilter=" + encodeURIComponent("" + minAmountFilter) + "&";
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
            return this.processGetEmployeeEarningsToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeeEarningsToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetEmployeeEarningsToExcel(response: HttpResponseBase): Observable<FileDto> {
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

    getMaxEarningId(): Observable<number> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeEarnings/GetMaxID";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetMaxEarningId(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetMaxEarningId(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processGetMaxEarningId(response: HttpResponseBase): Observable<number> {
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