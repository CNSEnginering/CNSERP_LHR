import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, blobToText, FileDto, throwException, CustomFileDto } from "@shared/service-proxies/service-proxies";
import * as moment from 'moment';
import { PagedResultDtoOfGetEmployeesForViewDto, GetEmployeesForEditOutput, CreateOrEditEmployeesDto } from '../dto/employee-dto';
import { BinaryData } from 'fs';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { List } from 'lodash';
import { url } from 'inspector';
import { EmployerBankDto } from '../dto/employerBank-dto';

@Injectable({
    providedIn: 'root'
})

export class EmployeesServiceProxy {
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
     * @param fatherNameFilter (optional) 
     * @param maxdate_of_birthFilter (optional) 
     * @param mindate_of_birthFilter (optional) 
     * @param home_addressFilter (optional) 
     * @param phoneNoFilter (optional) 
     * @param ntnFilter (optional) 
     * @param maxapointment_dateFilter (optional) 
     * @param minapointment_dateFilter (optional) 
     * @param maxdate_of_joiningFilter (optional) 
     * @param mindate_of_joiningFilter (optional) 
     * @param maxdate_of_leavingFilter (optional) 
     * @param mindate_of_leavingFilter (optional) 
     * @param cityFilter (optional) 
     * @param cnicFilter (optional) 
     * @param maxEdIDFilter (optional) 
     * @param minEdIDFilter (optional) 
     * @param maxDeptIDFilter (optional) 
     * @param minDeptIDFilter (optional) 
     * @param maxDesignationIDFilter (optional) 
     * @param minDesignationIDFilter (optional) 
     * @param genderFilter (optional) 
     * @param statusFilter (optional) 
     * @param maxShiftIDFilter (optional) 
     * @param minShiftIDFilter (optional) 
     * @param maxTypeIDFilter (optional) 
     * @param minTypeIDFilter (optional) 
     * @param maxSecIDFilter (optional) 
     * @param minSecIDFilter (optional) 
     * @param maxReligionIDFilter (optional) 
     * @param minReligionIDFilter (optional) 
     * @param social_securityFilter (optional) 
     * @param eobiFilter (optional) 
     * @param wppfFilter (optional) 
     * @param payment_modeFilter (optional) 
     * @param bank_nameFilter (optional) 
     * @param account_noFilter (optional) 
     * @param academic_qualificationFilter (optional) 
     * @param professional_qualificationFilter (optional) 
     * @param maxfirst_rest_daysFilter (optional) 
     * @param minfirst_rest_daysFilter (optional) 
     * @param maxsecond_rest_daysFilter (optional) 
     * @param minsecond_rest_daysFilter (optional) 
     * @param maxfirst_rest_days_w2Filter (optional) 
     * @param minfirst_rest_days_w2Filter (optional) 
     * @param maxsecond_rest_days_w2Filter (optional) 
     * @param minsecond_rest_days_w2Filter (optional) 
     * @param bloodGroupFilter (optional) 
     * @param referenceFilter (optional)
     * @param visa_DetailsFilter (optional) 
     * @param driving_LicenceFilter (optional) 
     * @param maxDuty_HoursFilter (optional) 
     * @param minDuty_HoursFilter (optional) 
     * @param activeFilter (optional) 
     * @param confirmedFilter (optional) 
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
    getAll(filter: string | null | undefined, maxEmployeeIDFilter: number | null | undefined, minEmployeeIDFilter: number | null | undefined, employeeNameFilter: string | null | undefined, fatherNameFilter: string | null | undefined, maxdate_of_birthFilter: moment.Moment | null | undefined, mindate_of_birthFilter: moment.Moment | null | undefined, home_addressFilter: string | null | undefined, phoneNoFilter: string | null | undefined, ntnFilter: string | null | undefined, maxapointment_dateFilter: moment.Moment | null | undefined, minapointment_dateFilter: moment.Moment | null | undefined, maxdate_of_joiningFilter: moment.Moment | null | undefined, mindate_of_joiningFilter: moment.Moment | null | undefined, maxdate_of_leavingFilter: moment.Moment | null | undefined, mindate_of_leavingFilter: moment.Moment | null | undefined, cityFilter: string | null | undefined, cnicFilter: string | null | undefined, maxEdIDFilter: number | null | undefined, minEdIDFilter: number | null | undefined, maxDeptIDFilter: number | null | undefined, minDeptIDFilter: number | null | undefined, maxDesignationIDFilter: number | null | undefined, minDesignationIDFilter: number | null | undefined, genderFilter: string | null | undefined, statusFilter: number | null | undefined, maxShiftIDFilter: number | null | undefined, minShiftIDFilter: number | null | undefined, maxTypeIDFilter: number | null | undefined, minTypeIDFilter: number | null | undefined, maxSecIDFilter: number | null | undefined, minSecIDFilter: number | null | undefined, maxReligionIDFilter: number | null | undefined, minReligionIDFilter: number | null | undefined, social_securityFilter: number | null | undefined, eobiFilter: number | null | undefined, wppfFilter: number | null | undefined, payment_modeFilter: string | null | undefined, bank_nameFilter: string | null | undefined, account_noFilter: string | null | undefined, academic_qualificationFilter: string | null | undefined, professional_qualificationFilter: string | null | undefined, maxfirst_rest_daysFilter: number | null | undefined, minfirst_rest_daysFilter: number | null | undefined, maxsecond_rest_daysFilter: number | null | undefined, minsecond_rest_daysFilter: number | null | undefined, maxfirst_rest_days_w2Filter: number | null | undefined, minfirst_rest_days_w2Filter: number | null | undefined, maxsecond_rest_days_w2Filter: number | null | undefined, minsecond_rest_days_w2Filter: number | null | undefined, bloodGroupFilter: string | null | undefined, referenceFilter: string | null | undefined, visa_DetailsFilter: string | null | undefined, driving_LicenceFilter: string | null | undefined, maxDuty_HoursFilter: number | null | undefined, minDuty_HoursFilter: number | null | undefined, activeFilter: number | null | undefined, confirmedFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetEmployeesForViewDto> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/Employees/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxEmployeeIDFilter !== undefined)
            url_ += "MaxEmployeeIDFilter=" + encodeURIComponent("" + maxEmployeeIDFilter) + "&";
        if (minEmployeeIDFilter !== undefined)
            url_ += "MinEmployeeIDFilter=" + encodeURIComponent("" + minEmployeeIDFilter) + "&";
        if (employeeNameFilter !== undefined)
            url_ += "EmployeeNameFilter=" + encodeURIComponent("" + employeeNameFilter) + "&";
        if (fatherNameFilter !== undefined)
            url_ += "FatherNameFilter=" + encodeURIComponent("" + fatherNameFilter) + "&";
        if (maxdate_of_birthFilter !== undefined)
            url_ += "Maxdate_of_birthFilter=" + encodeURIComponent(maxdate_of_birthFilter ? "" + maxdate_of_birthFilter.toJSON() : "") + "&";
        if (mindate_of_birthFilter !== undefined)
            url_ += "Mindate_of_birthFilter=" + encodeURIComponent(mindate_of_birthFilter ? "" + mindate_of_birthFilter.toJSON() : "") + "&";
        if (home_addressFilter !== undefined)
            url_ += "home_addressFilter=" + encodeURIComponent("" + home_addressFilter) + "&";
        if (maxapointment_dateFilter !== undefined)
            url_ += "Maxapointment_dateFilter=" + encodeURIComponent(maxapointment_dateFilter ? "" + maxapointment_dateFilter.toJSON() : "") + "&";
        if (minapointment_dateFilter !== undefined)
            url_ += "Minapointment_dateFilter=" + encodeURIComponent(minapointment_dateFilter ? "" + minapointment_dateFilter.toJSON() : "") + "&";
        if (maxdate_of_joiningFilter !== undefined)
            url_ += "Maxdate_of_joiningFilter=" + encodeURIComponent(maxdate_of_joiningFilter ? "" + maxdate_of_joiningFilter.toJSON() : "") + "&";
        if (mindate_of_joiningFilter !== undefined)
            url_ += "Mindate_of_joiningFilter=" + encodeURIComponent(mindate_of_joiningFilter ? "" + mindate_of_joiningFilter.toJSON() : "") + "&";
        if (maxdate_of_leavingFilter !== undefined)
            url_ += "Maxdate_of_leavingFilter=" + encodeURIComponent(maxdate_of_leavingFilter ? "" + maxdate_of_leavingFilter.toJSON() : "") + "&";
        if (mindate_of_leavingFilter !== undefined)
            url_ += "Mindate_of_leavingFilter=" + encodeURIComponent(mindate_of_leavingFilter ? "" + mindate_of_leavingFilter.toJSON() : "") + "&";
        if (cityFilter !== undefined)
            url_ += "CityFilter=" + encodeURIComponent("" + cityFilter) + "&";
        if (cnicFilter !== undefined)
            url_ += "CnicFilter=" + encodeURIComponent("" + cnicFilter) + "&";
        if (maxEdIDFilter !== undefined)
            url_ += "MaxEdIDFilter=" + encodeURIComponent("" + maxEdIDFilter) + "&";
        if (minEdIDFilter !== undefined)
            url_ += "MinEdIDFilter=" + encodeURIComponent("" + minEdIDFilter) + "&";
        if (maxDeptIDFilter !== undefined)
            url_ += "MaxDeptIDFilter=" + encodeURIComponent("" + maxDeptIDFilter) + "&";
        if (minDeptIDFilter !== undefined)
            url_ += "MinDeptIDFilter=" + encodeURIComponent("" + minDeptIDFilter) + "&";
        if (maxDesignationIDFilter !== undefined)
            url_ += "MaxDesignationIDFilter=" + encodeURIComponent("" + maxDesignationIDFilter) + "&";
        if (minDesignationIDFilter !== undefined)
            url_ += "MinDesignationIDFilter=" + encodeURIComponent("" + minDesignationIDFilter) + "&";
        if (genderFilter !== undefined)
            url_ += "GenderFilter=" + encodeURIComponent("" + genderFilter) + "&";
        if (statusFilter !== undefined)
            url_ += "StatusFilter=" + encodeURIComponent("" + statusFilter) + "&";
        if (maxShiftIDFilter !== undefined)
            url_ += "MaxShiftIDFilter=" + encodeURIComponent("" + maxShiftIDFilter) + "&";
        if (minShiftIDFilter !== undefined)
            url_ += "MinShiftIDFilter=" + encodeURIComponent("" + minShiftIDFilter) + "&";
        if (maxTypeIDFilter !== undefined)
            url_ += "MaxTypeIDFilter=" + encodeURIComponent("" + maxTypeIDFilter) + "&";
        if (minTypeIDFilter !== undefined)
            url_ += "MinTypeIDFilter=" + encodeURIComponent("" + minTypeIDFilter) + "&";
        if (maxSecIDFilter !== undefined)
            url_ += "MaxSecIDFilter=" + encodeURIComponent("" + maxSecIDFilter) + "&";
        if (minSecIDFilter !== undefined)
            url_ += "MinSecIDFilter=" + encodeURIComponent("" + minSecIDFilter) + "&";
        if (maxReligionIDFilter !== undefined)
            url_ += "MaxReligionIDFilter=" + encodeURIComponent("" + maxReligionIDFilter) + "&";
        if (minReligionIDFilter !== undefined)
            url_ += "MinReligionIDFilter=" + encodeURIComponent("" + minReligionIDFilter) + "&";
        if (social_securityFilter !== undefined)
            url_ += "social_securityFilter=" + encodeURIComponent("" + social_securityFilter) + "&";
        if (eobiFilter !== undefined)
            url_ += "eobiFilter=" + encodeURIComponent("" + eobiFilter) + "&";
        if (wppfFilter !== undefined)
            url_ += "wppfFilter=" + encodeURIComponent("" + wppfFilter) + "&";
        if (payment_modeFilter !== undefined)
            url_ += "payment_modeFilter=" + encodeURIComponent("" + payment_modeFilter) + "&";
        if (bank_nameFilter !== undefined)
            url_ += "bank_nameFilter=" + encodeURIComponent("" + bank_nameFilter) + "&";
        if (account_noFilter !== undefined)
            url_ += "account_noFilter=" + encodeURIComponent("" + account_noFilter) + "&";
        if (academic_qualificationFilter !== undefined)
            url_ += "academic_qualificationFilter=" + encodeURIComponent("" + academic_qualificationFilter) + "&";
        if (professional_qualificationFilter !== undefined)
            url_ += "professional_qualificationFilter=" + encodeURIComponent("" + professional_qualificationFilter) + "&";
        if (maxfirst_rest_daysFilter !== undefined)
            url_ += "Maxfirst_rest_daysFilter=" + encodeURIComponent("" + maxfirst_rest_daysFilter) + "&";
        if (minfirst_rest_daysFilter !== undefined)
            url_ += "Minfirst_rest_daysFilter=" + encodeURIComponent("" + minfirst_rest_daysFilter) + "&";
        if (maxsecond_rest_daysFilter !== undefined)
            url_ += "Maxsecond_rest_daysFilter=" + encodeURIComponent("" + maxsecond_rest_daysFilter) + "&";
        if (minsecond_rest_daysFilter !== undefined)
            url_ += "Minsecond_rest_daysFilter=" + encodeURIComponent("" + minsecond_rest_daysFilter) + "&";
        if (maxfirst_rest_days_w2Filter !== undefined)
            url_ += "Maxfirst_rest_days_w2Filter=" + encodeURIComponent("" + maxfirst_rest_days_w2Filter) + "&";
        if (minfirst_rest_days_w2Filter !== undefined)
            url_ += "Minfirst_rest_days_w2Filter=" + encodeURIComponent("" + minfirst_rest_days_w2Filter) + "&";
        if (maxsecond_rest_days_w2Filter !== undefined)
            url_ += "Maxsecond_rest_days_w2Filter=" + encodeURIComponent("" + maxsecond_rest_days_w2Filter) + "&";
        if (minsecond_rest_days_w2Filter !== undefined)
            url_ += "Minsecond_rest_days_w2Filter=" + encodeURIComponent("" + minsecond_rest_days_w2Filter) + "&";
        if (bloodGroupFilter !== undefined)
            url_ += "BloodGroupFilter=" + encodeURIComponent("" + bloodGroupFilter) + "&";
        if (referenceFilter !== undefined)
            url_ += "ReferenceFilter=" + encodeURIComponent("" + referenceFilter) + "&";
        if (visa_DetailsFilter !== undefined)
            url_ += "Visa_DetailsFilter=" + encodeURIComponent("" + visa_DetailsFilter) + "&";
        if (driving_LicenceFilter !== undefined)
            url_ += "Driving_LicenceFilter=" + encodeURIComponent("" + driving_LicenceFilter) + "&";
        if (maxDuty_HoursFilter !== undefined)
            url_ += "MaxDuty_HoursFilter=" + encodeURIComponent("" + maxDuty_HoursFilter) + "&";
        if (minDuty_HoursFilter !== undefined)
            url_ += "MinDuty_HoursFilter=" + encodeURIComponent("" + minDuty_HoursFilter) + "&";
        if (activeFilter !== undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&";
        if (confirmedFilter !== undefined)
            url_ += "ConfirmedFilter=" + encodeURIComponent("" + confirmedFilter) + "&";
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
                    return <Observable<PagedResultDtoOfGetEmployeesForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetEmployeesForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetEmployeesForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetEmployeesForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetEmployeesForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetEmployeesForViewDto>(<any>null);
    }

    getActiveEmployees (){
        let url_ = this.baseUrl + "/api/services/app/Employees/GetActiveEmployees";
       return this.http.request("get",url_);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getEmployeesForEdit(id: number | null | undefined): Observable<GetEmployeesForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/Employees/GetEmployeesForEdit?";
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
            return this.processGetEmployeesForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeesForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetEmployeesForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetEmployeesForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetEmployeesForEdit(response: HttpResponseBase): Observable<GetEmployeesForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetEmployeesForEditOutput.fromJS(resultData200) : new GetEmployeesForEditOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetEmployeesForEditOutput>(<any>null);
    }
   
    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditEmployeesDto | null | undefined): Observable<void> {
        console.log(input);
        let url_ = this.baseUrl + "/api/services/app/Employees/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/Employees/Delete?";
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
     * @param fatherNameFilter (optional) 
     * @param maxdate_of_birthFilter (optional) 
     * @param mindate_of_birthFilter (optional) 
     * @param home_addressFilter (optional) 
     * @param phoneNoFilter (optional) 
     * @param ntnFilter (optional) 
     * @param maxapointment_dateFilter (optional) 
     * @param minapointment_dateFilter (optional) 
     * @param maxdate_of_joiningFilter (optional) 
     * @param mindate_of_joiningFilter (optional) 
     * @param maxdate_of_leavingFilter (optional) 
     * @param mindate_of_leavingFilter (optional) 
     * @param cityFilter (optional) 
     * @param cnicFilter (optional) 
     * @param maxEdIDFilter (optional) 
     * @param minEdIDFilter (optional) 
     * @param maxDeptIDFilter (optional) 
     * @param minDeptIDFilter (optional) 
     * @param maxDesignationIDFilter (optional) 
     * @param minDesignationIDFilter (optional) 
     * @param genderFilter (optional) 
     * @param statusFilter (optional) 
     * @param maxShiftIDFilter (optional) 
     * @param minShiftIDFilter (optional) 
     * @param maxTypeIDFilter (optional) 
     * @param minTypeIDFilter (optional) 
     * @param maxSecIDFilter (optional) 
     * @param minSecIDFilter (optional) 
     * @param maxReligionIDFilter (optional) 
     * @param minReligionIDFilter (optional) 
     * @param social_securityFilter (optional) 
     * @param eobiFilter (optional) 
     * @param wppfFilter (optional) 
     * @param payment_modeFilter (optional) 
     * @param bank_nameFilter (optional) 
     * @param account_noFilter (optional) 
     * @param academic_qualificationFilter (optional) 
     * @param professional_qualificationFilter (optional) 
     * @param maxfirst_rest_daysFilter (optional) 
     * @param minfirst_rest_daysFilter (optional) 
     * @param maxsecond_rest_daysFilter (optional) 
     * @param minsecond_rest_daysFilter (optional) 
     * @param maxfirst_rest_days_w2Filter (optional) 
     * @param minfirst_rest_days_w2Filter (optional) 
     * @param maxsecond_rest_days_w2Filter (optional) 
     * @param minsecond_rest_days_w2Filter (optional) 
     * @param bloodGroupFilter (optional) 
     * @param referenceFilter (optional)
     * @param visa_DetailsFilter (optional) 
     * @param driving_LicenceFilter (optional) 
     * @param maxDuty_HoursFilter (optional) 
     * @param minDuty_HoursFilter (optional) 
     * @param activeFilter (optional) 
     * @param confirmedFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @return Success
     */
    getEmployeesToExcel(filter: string | null | undefined, 
        maxEmployeeIDFilter: number | null | undefined, 
        minEmployeeIDFilter: number | null | undefined,
         employeeNameFilter: string | null | undefined,
          fatherNameFilter: string | null | undefined,
           maxdate_of_birthFilter: moment.Moment | null | undefined,
            mindate_of_birthFilter: moment.Moment | null | undefined,
             home_addressFilter: string | null | undefined,
              phoneNoFilter: string | null | undefined,
               ntnFilter: string | null | undefined,
                maxapointment_dateFilter: moment.Moment | null | undefined,
                 minapointment_dateFilter: moment.Moment | null | undefined,
                  maxdate_of_joiningFilter: moment.Moment | null | undefined,
                   mindate_of_joiningFilter: moment.Moment | null | undefined,
                    maxdate_of_leavingFilter: moment.Moment | null | undefined,
                     mindate_of_leavingFilter: moment.Moment | null | undefined,
                      cityFilter: string | null | undefined, 
                      cnicFilter: string | null | undefined,
                       maxEdIDFilter: number | null | undefined,
                        minEdIDFilter: number | null | undefined, 
                        maxDeptIDFilter: number | null | undefined,
                         minDeptIDFilter: number | null | undefined,
                          maxDesignationIDFilter: number | null | undefined,
                           minDesignationIDFilter: number | null | undefined, 
                           genderFilter: string | null | undefined,
                            statusFilter: number | null | undefined,
                             maxShiftIDFilter: number | null | undefined,
                              minShiftIDFilter: number | null | undefined,
                               maxTypeIDFilter: number | null | undefined,
                                minTypeIDFilter: number | null | undefined,
                                 maxSecIDFilter: number | null | undefined,
                                  minSecIDFilter: number | null | undefined, maxReligionIDFilter: number | null | undefined, minReligionIDFilter: number | null | undefined, social_securityFilter: number | null | undefined, eobiFilter: number | null | undefined, wppfFilter: number | null | undefined, payment_modeFilter: string | null | undefined, bank_nameFilter: string | null | undefined, account_noFilter: string | null | undefined, academic_qualificationFilter: string | null | undefined, professional_qualificationFilter: string | null | undefined, maxfirst_rest_daysFilter: number | null | undefined, minfirst_rest_daysFilter: number | null | undefined, maxsecond_rest_daysFilter: number | null | undefined, minsecond_rest_daysFilter: number | null | undefined, maxfirst_rest_days_w2Filter: number | null | undefined, minfirst_rest_days_w2Filter: number | null | undefined, maxsecond_rest_days_w2Filter: number | null | undefined, minsecond_rest_days_w2Filter: number | null | undefined, bloodGroupFilter: string | null | undefined, referenceFilter: string | null | undefined, visa_DetailsFilter: string | null | undefined, driving_LicenceFilter: string | null | undefined, maxDuty_HoursFilter: number | null | undefined, minDuty_HoursFilter: number | null | undefined, activeFilter: number | null | undefined, confirmedFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined)
    : Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/Employees/GetEmployeesToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxEmployeeIDFilter !== undefined)
            url_ += "MaxEmployeeIDFilter=" + encodeURIComponent("" + maxEmployeeIDFilter) + "&";
        if (minEmployeeIDFilter !== undefined)
            url_ += "MinEmployeeIDFilter=" + encodeURIComponent("" + minEmployeeIDFilter) + "&";
        if (employeeNameFilter !== undefined)
            url_ += "EmployeeNameFilter=" + encodeURIComponent("" + employeeNameFilter) + "&";
        if (fatherNameFilter !== undefined)
            url_ += "FatherNameFilter=" + encodeURIComponent("" + fatherNameFilter) + "&";
        if (maxdate_of_birthFilter !== undefined)
            url_ += "Maxdate_of_birthFilter=" + encodeURIComponent(maxdate_of_birthFilter ? "" + maxdate_of_birthFilter.toJSON() : "") + "&";
        if (mindate_of_birthFilter !== undefined)
            url_ += "Mindate_of_birthFilter=" + encodeURIComponent(mindate_of_birthFilter ? "" + mindate_of_birthFilter.toJSON() : "") + "&";
        if (home_addressFilter !== undefined)
            url_ += "home_addressFilter=" + encodeURIComponent("" + home_addressFilter) + "&";
        if (maxapointment_dateFilter !== undefined)
            url_ += "Maxapointment_dateFilter=" + encodeURIComponent(maxapointment_dateFilter ? "" + maxapointment_dateFilter.toJSON() : "") + "&";
        if (minapointment_dateFilter !== undefined)
            url_ += "Minapointment_dateFilter=" + encodeURIComponent(minapointment_dateFilter ? "" + minapointment_dateFilter.toJSON() : "") + "&";
        if (maxdate_of_joiningFilter !== undefined)
            url_ += "Maxdate_of_joiningFilter=" + encodeURIComponent(maxdate_of_joiningFilter ? "" + maxdate_of_joiningFilter.toJSON() : "") + "&";
        if (mindate_of_joiningFilter !== undefined)
            url_ += "Mindate_of_joiningFilter=" + encodeURIComponent(mindate_of_joiningFilter ? "" + mindate_of_joiningFilter.toJSON() : "") + "&";
        if (maxdate_of_leavingFilter !== undefined)
            url_ += "Maxdate_of_leavingFilter=" + encodeURIComponent(maxdate_of_leavingFilter ? "" + maxdate_of_leavingFilter.toJSON() : "") + "&";
        if (mindate_of_leavingFilter !== undefined)
            url_ += "Mindate_of_leavingFilter=" + encodeURIComponent(mindate_of_leavingFilter ? "" + mindate_of_leavingFilter.toJSON() : "") + "&";
        if (cityFilter !== undefined)
            url_ += "CityFilter=" + encodeURIComponent("" + cityFilter) + "&";
        if (cnicFilter !== undefined)
            url_ += "CnicFilter=" + encodeURIComponent("" + cnicFilter) + "&";
        if (maxEdIDFilter !== undefined)
            url_ += "MaxEdIDFilter=" + encodeURIComponent("" + maxEdIDFilter) + "&";
        if (minEdIDFilter !== undefined)
            url_ += "MinEdIDFilter=" + encodeURIComponent("" + minEdIDFilter) + "&";
        if (maxDeptIDFilter !== undefined)
            url_ += "MaxDeptIDFilter=" + encodeURIComponent("" + maxDeptIDFilter) + "&";
        if (minDeptIDFilter !== undefined)
            url_ += "MinDeptIDFilter=" + encodeURIComponent("" + minDeptIDFilter) + "&";
        if (maxDesignationIDFilter !== undefined)
            url_ += "MaxDesignationIDFilter=" + encodeURIComponent("" + maxDesignationIDFilter) + "&";
        if (minDesignationIDFilter !== undefined)
            url_ += "MinDesignationIDFilter=" + encodeURIComponent("" + minDesignationIDFilter) + "&";
        if (genderFilter !== undefined)
            url_ += "GenderFilter=" + encodeURIComponent("" + genderFilter) + "&";
        if (statusFilter !== undefined)
            url_ += "StatusFilter=" + encodeURIComponent("" + statusFilter) + "&";
        if (maxShiftIDFilter !== undefined)
            url_ += "MaxShiftIDFilter=" + encodeURIComponent("" + maxShiftIDFilter) + "&";
        if (minShiftIDFilter !== undefined)
            url_ += "MinShiftIDFilter=" + encodeURIComponent("" + minShiftIDFilter) + "&";
        if (maxTypeIDFilter !== undefined)
            url_ += "MaxTypeIDFilter=" + encodeURIComponent("" + maxTypeIDFilter) + "&";
        if (minTypeIDFilter !== undefined)
            url_ += "MinTypeIDFilter=" + encodeURIComponent("" + minTypeIDFilter) + "&";
        if (maxSecIDFilter !== undefined)
            url_ += "MaxSecIDFilter=" + encodeURIComponent("" + maxSecIDFilter) + "&";
        if (minSecIDFilter !== undefined)
            url_ += "MinSecIDFilter=" + encodeURIComponent("" + minSecIDFilter) + "&";
        if (maxReligionIDFilter !== undefined)
            url_ += "MaxReligionIDFilter=" + encodeURIComponent("" + maxReligionIDFilter) + "&";
        if (minReligionIDFilter !== undefined)
            url_ += "MinReligionIDFilter=" + encodeURIComponent("" + minReligionIDFilter) + "&";
        if (social_securityFilter !== undefined)
            url_ += "social_securityFilter=" + encodeURIComponent("" + social_securityFilter) + "&";
        if (eobiFilter !== undefined)
            url_ += "eobiFilter=" + encodeURIComponent("" + eobiFilter) + "&";
        if (wppfFilter !== undefined)
            url_ += "wppfFilter=" + encodeURIComponent("" + wppfFilter) + "&";
        if (payment_modeFilter !== undefined)
            url_ += "payment_modeFilter=" + encodeURIComponent("" + payment_modeFilter) + "&";
        if (bank_nameFilter !== undefined)
            url_ += "bank_nameFilter=" + encodeURIComponent("" + bank_nameFilter) + "&";
        if (account_noFilter !== undefined)
            url_ += "account_noFilter=" + encodeURIComponent("" + account_noFilter) + "&";
        if (academic_qualificationFilter !== undefined)
            url_ += "academic_qualificationFilter=" + encodeURIComponent("" + academic_qualificationFilter) + "&";
        if (professional_qualificationFilter !== undefined)
            url_ += "professional_qualificationFilter=" + encodeURIComponent("" + professional_qualificationFilter) + "&";
        if (maxfirst_rest_daysFilter !== undefined)
            url_ += "Maxfirst_rest_daysFilter=" + encodeURIComponent("" + maxfirst_rest_daysFilter) + "&";
        if (minfirst_rest_daysFilter !== undefined)
            url_ += "Minfirst_rest_daysFilter=" + encodeURIComponent("" + minfirst_rest_daysFilter) + "&";
        if (maxsecond_rest_daysFilter !== undefined)
            url_ += "Maxsecond_rest_daysFilter=" + encodeURIComponent("" + maxsecond_rest_daysFilter) + "&";
        if (minsecond_rest_daysFilter !== undefined)
            url_ += "Minsecond_rest_daysFilter=" + encodeURIComponent("" + minsecond_rest_daysFilter) + "&";
        if (maxfirst_rest_days_w2Filter !== undefined)
            url_ += "Maxfirst_rest_days_w2Filter=" + encodeURIComponent("" + maxfirst_rest_days_w2Filter) + "&";
        if (minfirst_rest_days_w2Filter !== undefined)
            url_ += "Minfirst_rest_days_w2Filter=" + encodeURIComponent("" + minfirst_rest_days_w2Filter) + "&";
        if (maxsecond_rest_days_w2Filter !== undefined)
            url_ += "Maxsecond_rest_days_w2Filter=" + encodeURIComponent("" + maxsecond_rest_days_w2Filter) + "&";
        if (minsecond_rest_days_w2Filter !== undefined)
            url_ += "Minsecond_rest_days_w2Filter=" + encodeURIComponent("" + minsecond_rest_days_w2Filter) + "&";
        if (bloodGroupFilter !== undefined)
            url_ += "BloodGroupFilter=" + encodeURIComponent("" + bloodGroupFilter) + "&";
        if (referenceFilter !== undefined)
            url_ += "ReferenceFilter=" + encodeURIComponent("" + referenceFilter) + "&";
        if (visa_DetailsFilter !== undefined)
            url_ += "Visa_DetailsFilter=" + encodeURIComponent("" + visa_DetailsFilter) + "&";
        if (driving_LicenceFilter !== undefined)
            url_ += "Driving_LicenceFilter=" + encodeURIComponent("" + driving_LicenceFilter) + "&";
        if (maxDuty_HoursFilter !== undefined)
            url_ += "MaxDuty_HoursFilter=" + encodeURIComponent("" + maxDuty_HoursFilter) + "&";
        if (minDuty_HoursFilter !== undefined)
            url_ += "MinDuty_HoursFilter=" + encodeURIComponent("" + minDuty_HoursFilter) + "&";
        if (activeFilter !== undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&";
        if (confirmedFilter !== undefined)
            url_ += "ConfirmedFilter=" + encodeURIComponent("" + confirmedFilter) + "&";
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
            return this.processGetEmployeesToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetEmployeesToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetEmployeesToExcel(response: HttpResponseBase): Observable<FileDto> {
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

    getMaxEmployeeId(): Observable<number> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/Employees/GetMaxID";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response: any) => {
            return this.processGetMaxEmployeeId(response);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetMaxEmployeeId(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processGetMaxEmployeeId(response: HttpResponseBase): Observable<number> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 !== undefined ? resultData200 : <any>null;
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, headers);
            }));
        }
        return _observableOf<number>(<any>null);
    }
    getPickerName(id: number, type: string): Observable<string> {
        let url_ = this.baseUrl + "/api/services/app/Employees/GetPickerName?";
        if (id !== undefined)
            url_ += "id=" + encodeURIComponent("" + id) + "&";
        if (type !== undefined)
            url_ += "type=" + encodeURIComponent("" + type) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processgetPickerName(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processgetPickerName(<any>response_);
                } catch (e) {
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else
                return <Observable<string>><any>_observableThrow(response_);
        }));
    }

    protected processgetPickerName(response: HttpResponseBase): Observable<string> {
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

    getImage(appID: number, docID: number): Observable<List<BinaryData>> {

        debugger;
        let url_ = this.baseUrl + "/DemoUiComponents/GetImageData?";
        if (appID !== undefined)
            url_ += "AppID=" + encodeURIComponent("" + appID) + "&";
        if (docID !== undefined)
            url_ += "DocID=" + encodeURIComponent("" + docID) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetImage(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetImage(<any>response_);
                } catch (e) {
                    return <Observable<List<BinaryData>>><any>_observableThrow(e);
                }
            } else
                return <Observable<List<BinaryData>>><any>_observableThrow(response_);
        }));
    }

    protected processGetImage(response: HttpResponseBase): Observable<List<BinaryData>> {
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
                result200 = resultData200 !== undefined ? resultData200 : <any>null;
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<List<BinaryData>>(<any>null);

    }


    getFile(appID: number, docID: number): Observable<List<BinaryData>> {

        debugger;
        let url_ = this.baseUrl + "/DemoUiComponents/GetFileData?";
        if (appID !== undefined)
            url_ += "AppID=" + encodeURIComponent("" + appID) + "&";
        if (docID !== undefined)
            url_ += "DocID=" + encodeURIComponent("" + docID) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetImage(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetFile(<any>response_);
                } catch (e) {
                    return <Observable<List<BinaryData>>><any>_observableThrow(e);
                }
            } else
                return <Observable<List<BinaryData>>><any>_observableThrow(response_);
        }));
    }

    protected processGetFile(response: HttpResponseBase): Observable<List<BinaryData>> {
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
                result200 = resultData200 !== undefined ? resultData200 : <any>null;
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<List<BinaryData>>(<any>null);

    }


    checkValidEmpId(empId):any{
        let url_ = this.baseUrl + "/api/services/app/Employees/GetEmployeeIDValid?";
        url_ += "empId=" + encodeURIComponent("" + empId) 
        return this.http.request("get",url_);

    }

    getEmployerBankList() 
    {
        let url_ = this.baseUrl + "/api/services/app/EmployerBank/GetEmployerBanks";
        return this.http.request("get",url_);
        

    }

    


    // getFile(appID: number, docID: number): Observable<List<string>> {

    //     debugger;
    //     let url_ = this.baseUrl + "/DemoUiComponents/GetFileData?";
    //     if (appID !== undefined)
    //         url_ += "AppID=" + encodeURIComponent("" + appID) + "&";
    //     if (docID !== undefined)
    //         url_ += "DocID=" + encodeURIComponent("" + docID) + "&";
    //     url_ = url_.replace(/[?&]$/, "");

    //     let options_: any = {
    //         observe: "response",
    //         responseType: "blob",
    //         headers: new HttpHeaders({
    //             "Accept": "application/json"
    //         })
    //     };

    //     return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
    //         return this.processGetFile(response_);
    //     })).pipe(_observableCatch((response_: any) => {
    //         if (response_ instanceof HttpResponseBase) {
    //             try {
    //                 return this.processGetFile(<any>response_);
    //             } catch (e) {
    //                 return <Observable<List<string>>><any>_observableThrow(e);
    //             }
    //         } else
    //             return <Observable<List<string>>><any>_observableThrow(response_);
    //     }));
    // }

    // protected processGetFile(response: HttpResponseBase): Observable<List<string>> {
    //     debugger;
    //     const status = response.status;
    //     const responseBlob =
    //         response instanceof HttpResponse ? response.body :
    //             (<any>response).error instanceof Blob ? (<any>response).error : undefined;

    //     let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
    //     if (status === 200) {
    //         return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
    //             let result200: any = null;
    //             let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
    //             result200 = resultData200 !== undefined ? resultData200 : <any>null;
    //             return _observableOf(result200);
    //         }));
    //     } else if (status !== 200 && status !== 204) {
    //         return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
    //             return throwException("An unexpected server error occurred.", status, _responseText, _headers);
    //         }));
    //     }
    //     return _observableOf<List<string>>(<any>null);

    // }
    
}