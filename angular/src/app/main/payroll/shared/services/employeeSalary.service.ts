import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { map } from "rxjs/operators";
import { PagedResultDtoOfGetEmployeeSalaryForViewDto, GetEmployeeSalaryForViewDto, GetEmployeeSalaryForEditOutput, CreateOrEditEmployeeSalaryDto, PagedResultDtoOfEmployeeSalaryDto } from '../dto/employeeSalary-dto';

@Injectable({
    providedIn: 'root'
})
export class EmployeeSalaryServiceProxy {
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
     * @param maxBank_AmountFilter (optional) 
     * @param minBank_AmountFilter (optional)
     * @param maxStartDateFilter (optional) 
     * @param minStartDateFilter (optional) 
     * @param maxGross_SalaryFilter (optional) 
     * @param minGross_SalaryFilter (optional) 
     * @param maxBasic_SalaryFilter (optional) 
     * @param minBasic_SalaryFilter (optional) 
     * @param maxTaxFilter (optional) 
     * @param minTaxFilter (optional) 
     * @param maxHouse_RentFilter (optional) 
     * @param minHouse_RentFilter (optional) 
     * @param maxNet_SalaryFilter (optional) 
     * @param minNet_SalaryFilter (optional) 
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
    getAll(filter: string | null | undefined, maxEmployeeIDFilter: number | null | undefined, minEmployeeIDFilter: number | null | undefined, employeeNameFilter: string | null | undefined, maxBank_AmountFilter: number | null | undefined, minBank_AmountFilter: number | null | undefined, maxStartDateFilter: moment.Moment | null | undefined, minStartDateFilter: moment.Moment | null | undefined, maxGross_SalaryFilter: number | null | undefined, minGross_SalaryFilter: number | null | undefined, maxBasic_SalaryFilter: number | null | undefined, minBasic_SalaryFilter: number | null | undefined, maxTaxFilter: number | null | undefined, minTaxFilter: number | null | undefined, maxHouse_RentFilter: number | null | undefined, minHouse_RentFilter: number | null | undefined, maxNet_SalaryFilter: number | null | undefined, minNet_SalaryFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetEmployeeSalaryForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeSalary/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxEmployeeIDFilter !== undefined)
            url_ += "MaxEmployeeIDFilter=" + encodeURIComponent("" + maxEmployeeIDFilter) + "&";
        if (minEmployeeIDFilter !== undefined)
            url_ += "MinEmployeeIDFilter=" + encodeURIComponent("" + minEmployeeIDFilter) + "&";
        if (employeeNameFilter !== undefined)
            url_ += "EmployeeNameFilter=" + encodeURIComponent("" + employeeNameFilter) + "&";
        if (maxBank_AmountFilter !== undefined)
            url_ += "MaxBank_AmountFilter=" + encodeURIComponent("" + maxBank_AmountFilter) + "&";
        if (minBank_AmountFilter !== undefined)
            url_ += "MinBank_AmountFilter=" + encodeURIComponent("" + minBank_AmountFilter) + "&";
        if (maxStartDateFilter !== undefined)
            url_ += "MaxStartDateFilter=" + encodeURIComponent(maxStartDateFilter ? "" + maxStartDateFilter.toJSON() : "") + "&";
        if (minStartDateFilter !== undefined)
            url_ += "MinStartDateFilter=" + encodeURIComponent(minStartDateFilter ? "" + minStartDateFilter.toJSON() : "") + "&";
        if (maxGross_SalaryFilter !== undefined)
            url_ += "MaxGross_SalaryFilter=" + encodeURIComponent("" + maxGross_SalaryFilter) + "&";
        if (minGross_SalaryFilter !== undefined)
            url_ += "MinGross_SalaryFilter=" + encodeURIComponent("" + minGross_SalaryFilter) + "&";
        if (maxBasic_SalaryFilter !== undefined)
            url_ += "MaxBasic_SalaryFilter=" + encodeURIComponent("" + maxBasic_SalaryFilter) + "&";
        if (minBasic_SalaryFilter !== undefined)
            url_ += "MinBasic_SalaryFilter=" + encodeURIComponent("" + minBasic_SalaryFilter) + "&";
        if (maxTaxFilter !== undefined)
            url_ += "MaxTaxFilter=" + encodeURIComponent("" + maxTaxFilter) + "&";
        if (minTaxFilter !== undefined)
            url_ += "MinTaxFilter=" + encodeURIComponent("" + minTaxFilter) + "&";
        if (maxHouse_RentFilter !== undefined)
            url_ += "MaxHouse_RentFilter=" + encodeURIComponent("" + maxHouse_RentFilter) + "&";
        if (minHouse_RentFilter !== undefined)
            url_ += "MinHouse_RentFilter=" + encodeURIComponent("" + minHouse_RentFilter) + "&";
        if (maxNet_SalaryFilter !== undefined)
            url_ += "MaxNet_SalaryFilter=" + encodeURIComponent("" + maxNet_SalaryFilter) + "&";
        if (minNet_SalaryFilter !== undefined)
            url_ += "MinNet_SalaryFilter=" + encodeURIComponent("" + minNet_SalaryFilter) + "&";
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
                    return <Observable<PagedResultDtoOfGetEmployeeSalaryForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetEmployeeSalaryForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetEmployeeSalaryForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetEmployeeSalaryForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetEmployeeSalaryForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetEmployeeSalaryForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getEmployeeSalaryHistory(id: number | null | undefined): Observable<PagedResultDtoOfEmployeeSalaryDto> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeSalary/GetEmployeeSalaryHistory?";
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
            return this.processGetEmployeeSalaryForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeeSalaryForView(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfEmployeeSalaryDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfEmployeeSalaryDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetEmployeeSalaryForView(response: HttpResponseBase): Observable<PagedResultDtoOfEmployeeSalaryDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfEmployeeSalaryDto.fromJS(resultData200) : new PagedResultDtoOfEmployeeSalaryDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfEmployeeSalaryDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getEmployeeSalaryForEdit(id: number | null | undefined): Observable<GetEmployeeSalaryForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeSalary/GetEmployeeSalaryForEdit?";
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
            return this.processGetEmployeeSalaryForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeeSalaryForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetEmployeeSalaryForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetEmployeeSalaryForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetEmployeeSalaryForEdit(response: HttpResponseBase): Observable<GetEmployeeSalaryForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetEmployeeSalaryForEditOutput.fromJS(resultData200) : new GetEmployeeSalaryForEditOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetEmployeeSalaryForEditOutput>(<any>null);
    }

    getEmpSlab(Sal: number) {
       
       let url = this.baseUrl;
        url += "/api/services/app/SlabSetup/GetSlabSetupForSalary?amt=" + Sal;
        debugger
        return this.http.get(url).pipe(map((response: any) => {
            debugger
            return response["result"];
        }));
    }
    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditEmployeeSalaryDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeSalary/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/EmployeeSalary/Delete?";
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
     * @param employeeNameFilter (optional) 
     * @param maxBank_AmountFilter (optional) 
     * @param minBank_AmountFilter (optional)
     * @param maxStartDateFilter (optional) 
     * @param minStartDateFilter (optional) 
     * @param maxGross_SalaryFilter (optional) 
     * @param minGross_SalaryFilter (optional) 
     * @param maxBasic_SalaryFilter (optional) 
     * @param minBasic_SalaryFilter (optional) 
     * @param maxTaxFilter (optional) 
     * @param minTaxFilter (optional) 
     * @param maxHouse_RentFilter (optional) 
     * @param minHouse_RentFilter (optional) 
     * @param maxNet_SalaryFilter (optional) 
     * @param minNet_SalaryFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @return Success
     */
    getEmployeeSalaryToExcel(filter: string | null | undefined, maxEmployeeIDFilter: number | null | undefined, minEmployeeIDFilter: number | null | undefined, employeeNameFilter: string | null | undefined, maxBank_AmountFilter: number | null | undefined, minBank_AmountFilter: number | null | undefined, maxStartDateFilter: moment.Moment | null | undefined, minStartDateFilter: moment.Moment | null | undefined, maxGross_SalaryFilter: number | null | undefined, minGross_SalaryFilter: number | null | undefined, maxBasic_SalaryFilter: number | null | undefined, minBasic_SalaryFilter: number | null | undefined, maxTaxFilter: number | null | undefined, minTaxFilter: number | null | undefined, maxHouse_RentFilter: number | null | undefined, minHouse_RentFilter: number | null | undefined, maxNet_SalaryFilter: number | null | undefined, minNet_SalaryFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeSalary/GetEmployeeSalaryToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxEmployeeIDFilter !== undefined)
            url_ += "MaxEmployeeIDFilter=" + encodeURIComponent("" + maxEmployeeIDFilter) + "&";
        if (minEmployeeIDFilter !== undefined)
            url_ += "MinEmployeeIDFilter=" + encodeURIComponent("" + minEmployeeIDFilter) + "&";
        if (employeeNameFilter !== undefined)
            url_ += "EmployeeNameFilter=" + encodeURIComponent("" + employeeNameFilter) + "&";
        if (maxBank_AmountFilter !== undefined)
            url_ += "MaxBank_AmountFilter=" + encodeURIComponent("" + maxBank_AmountFilter) + "&";
        if (minBank_AmountFilter !== undefined)
            url_ += "MinBank_AmountFilter=" + encodeURIComponent("" + minBank_AmountFilter) + "&";
        if (maxStartDateFilter !== undefined)
            url_ += "MaxStartDateFilter=" + encodeURIComponent(maxStartDateFilter ? "" + maxStartDateFilter.toJSON() : "") + "&";
        if (minStartDateFilter !== undefined)
            url_ += "MinStartDateFilter=" + encodeURIComponent(minStartDateFilter ? "" + minStartDateFilter.toJSON() : "") + "&";
        if (maxGross_SalaryFilter !== undefined)
            url_ += "MaxGross_SalaryFilter=" + encodeURIComponent("" + maxGross_SalaryFilter) + "&";
        if (minGross_SalaryFilter !== undefined)
            url_ += "MinGross_SalaryFilter=" + encodeURIComponent("" + minGross_SalaryFilter) + "&";
        if (maxBasic_SalaryFilter !== undefined)
            url_ += "MaxBasic_SalaryFilter=" + encodeURIComponent("" + maxBasic_SalaryFilter) + "&";
        if (minBasic_SalaryFilter !== undefined)
            url_ += "MinBasic_SalaryFilter=" + encodeURIComponent("" + minBasic_SalaryFilter) + "&";
        if (maxTaxFilter !== undefined)
            url_ += "MaxTaxFilter=" + encodeURIComponent("" + maxTaxFilter) + "&";
        if (minTaxFilter !== undefined)
            url_ += "MinTaxFilter=" + encodeURIComponent("" + minTaxFilter) + "&";
        if (maxHouse_RentFilter !== undefined)
            url_ += "MaxHouse_RentFilter=" + encodeURIComponent("" + maxHouse_RentFilter) + "&";
        if (minHouse_RentFilter !== undefined)
            url_ += "MinHouse_RentFilter=" + encodeURIComponent("" + minHouse_RentFilter) + "&";
        if (maxNet_SalaryFilter !== undefined)
            url_ += "MaxNet_SalaryFilter=" + encodeURIComponent("" + maxNet_SalaryFilter) + "&";
        if (minNet_SalaryFilter !== undefined)
            url_ += "MinNet_SalaryFilter=" + encodeURIComponent("" + minNet_SalaryFilter) + "&";
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
            return this.processGetEmployeeSalaryToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeeSalaryToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetEmployeeSalaryToExcel(response: HttpResponseBase): Observable<FileDto> {
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