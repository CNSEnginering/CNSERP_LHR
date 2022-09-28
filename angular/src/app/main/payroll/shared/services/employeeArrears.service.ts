import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PagedResultDtoOfGetEmployeeArrearsForViewDto, GetEmployeeArrearsForViewDto, GetEmployeeArrearsForEditOutput, CreateOrEditEmployeeArrearsDto } from '../dto/employeeArrears-dto';

@Injectable({
    providedIn: 'root'
})
export class EmployeeArrearsServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param maxArrearIDFilter (optional) 
     * @param minArrearIDFilter (optional) 
     * @param maxEmployeeIDFilter (optional) 
     * @param minEmployeeIDFilter (optional) 
     * @param employeeNameFilter (optional) 
     * @param maxSalaryYearFilter (optional) 
     * @param minSalaryYearFilter (optional) 
     * @param maxSalaryMonthFilter (optional) 
     * @param minSalaryMonthFilter (optional) 
     * @param maxArrearDateFilter (optional) 
     * @param minArrearDateFilter (optional) 
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
    getAll(filter: string | null | undefined, maxArrearIDFilter: number | null | undefined, minArrearIDFilter: number | null | undefined, maxEmployeeIDFilter: number | null | undefined, minEmployeeIDFilter: number | null | undefined, employeeNameFilter: string | null | undefined, maxSalaryYearFilter: number | null | undefined, minSalaryYearFilter: number | null | undefined, maxSalaryMonthFilter: number | null | undefined, minSalaryMonthFilter: number | null | undefined, maxArrearDateFilter: moment.Moment | null | undefined, minArrearDateFilter: moment.Moment | null | undefined, maxAmountFilter: number | null | undefined, minAmountFilter: number | null | undefined, activeFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetEmployeeArrearsForViewDto> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeArrears/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (maxArrearIDFilter !== undefined)
            url_ += "MaxArrearIDFilter=" + encodeURIComponent("" + maxArrearIDFilter) + "&"; 
        if (minArrearIDFilter !== undefined)
            url_ += "MinArrearIDFilter=" + encodeURIComponent("" + minArrearIDFilter) + "&"; 
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
        if (maxArrearDateFilter !== undefined)
            url_ += "MaxArrearDateFilter=" + encodeURIComponent(maxArrearDateFilter ? "" + maxArrearDateFilter.toJSON() : "") + "&"; 
        if (minArrearDateFilter !== undefined)
            url_ += "MinArrearDateFilter=" + encodeURIComponent(minArrearDateFilter ? "" + minArrearDateFilter.toJSON() : "") + "&"; 
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
                    return <Observable<PagedResultDtoOfGetEmployeeArrearsForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetEmployeeArrearsForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetEmployeeArrearsForViewDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfGetEmployeeArrearsForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetEmployeeArrearsForViewDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetEmployeeArrearsForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getEmployeeArrearsForView(id: number | null | undefined): Observable<GetEmployeeArrearsForViewDto> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeArrears/GetEmployeeArrearsForView?";
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
            return this.processGetEmployeeArrearsForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeeArrearsForView(<any>response_);
                } catch (e) {
                    return <Observable<GetEmployeeArrearsForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetEmployeeArrearsForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetEmployeeArrearsForView(response: HttpResponseBase): Observable<GetEmployeeArrearsForViewDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GetEmployeeArrearsForViewDto.fromJS(resultData200) : new GetEmployeeArrearsForViewDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetEmployeeArrearsForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getEmployeeArrearsForEdit(id: number | null | undefined): Observable<GetEmployeeArrearsForEditOutput> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeArrears/GetEmployeeArrearsForEdit?";
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
            return this.processGetEmployeeArrearsForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeeArrearsForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetEmployeeArrearsForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetEmployeeArrearsForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetEmployeeArrearsForEdit(response: HttpResponseBase): Observable<GetEmployeeArrearsForEditOutput> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GetEmployeeArrearsForEditOutput.fromJS(resultData200) : new GetEmployeeArrearsForEditOutput();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetEmployeeArrearsForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditEmployeeArrearsDto | null | undefined): Observable<void> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeArrears/CreateOrEdit";
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
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeArrears/Delete?";
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
     * @param maxArrearIDFilter (optional) 
     * @param minArrearIDFilter (optional) 
     * @param maxEmployeeIDFilter (optional) 
     * @param minEmployeeIDFilter (optional) 
     * @param employeeNameFilter (optional) 
     * @param maxSalaryYearFilter (optional) 
     * @param minSalaryYearFilter (optional) 
     * @param maxSalaryMonthFilter (optional) 
     * @param minSalaryMonthFilter (optional) 
     * @param maxArrearDateFilter (optional) 
     * @param minArrearDateFilter (optional) 
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
    getEmployeeArrearsToExcel(filter: string | null | undefined, maxArrearIDFilter: number | null | undefined, minArrearIDFilter: number | null | undefined, maxEmployeeIDFilter: number | null | undefined, minEmployeeIDFilter: number | null | undefined, employeeNameFilter: string | null | undefined, maxSalaryYearFilter: number | null | undefined, minSalaryYearFilter: number | null | undefined, maxSalaryMonthFilter: number | null | undefined, minSalaryMonthFilter: number | null | undefined, maxArrearDateFilter: moment.Moment | null | undefined, minArrearDateFilter: moment.Moment | null | undefined, maxAmountFilter: number | null | undefined, minAmountFilter: number | null | undefined, activeFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeArrears/GetEmployeeArrearsToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (maxArrearIDFilter !== undefined)
            url_ += "MaxArrearIDFilter=" + encodeURIComponent("" + maxArrearIDFilter) + "&"; 
        if (minArrearIDFilter !== undefined)
            url_ += "MinArrearIDFilter=" + encodeURIComponent("" + minArrearIDFilter) + "&"; 
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
        if (maxArrearDateFilter !== undefined)
            url_ += "MaxArrearDateFilter=" + encodeURIComponent(maxArrearDateFilter ? "" + maxArrearDateFilter.toJSON() : "") + "&"; 
        if (minArrearDateFilter !== undefined)
            url_ += "MinArrearDateFilter=" + encodeURIComponent(minArrearDateFilter ? "" + minArrearDateFilter.toJSON() : "") + "&"; 
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

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetEmployeeArrearsToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeeArrearsToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetEmployeeArrearsToExcel(response: HttpResponseBase): Observable<FileDto> {
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
    getMaxArrearId(): Observable<number> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeArrears/GetMaxID";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetMaxArrearId(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetMaxArrearId(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processGetMaxArrearId(response: HttpResponseBase): Observable<number> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
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