import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, blobToText, FileDto, throwException, PagedResultDtoOfNameValueDto } from "@shared/service-proxies/service-proxies";
import * as moment from 'moment';
import { PagedResultDtoOfGetAllowancesForViewDto, GetAllowancesForViewDto, GetAllowancesForEditOutput, CreateOrEditAllowancesDto } from '../dto/allowances-dto';
import { PagedResultDtoOfGetAllowancesDetailForViewDto, PagedResultDtoOfGetAllowancesDetail } from '../dto/allowanceDetails-dto';
import { IPagedResultDtoOfAttendanceDetailsDto } from '../interface/attendanceDetail-interface';
import { IPagedResultDtoOfGetAllowancesDetailForViewDto, IPagedResultDtoOfGetAllowancesDetail } from '../interface/allowanceDetails-interface';
import { AllowanceSetupDto } from '../dto/allowanceSetup-dto';

@Injectable({
    providedIn: 'root'
})

export class AllowancesServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param maxDocIDFilter (optional) 
     * @param minDocIDFilter (optional) 
     * @param maxDocdateFilter (optional) 
     * @param minDocdateFilter (optional) 
     * @param maxDocMonthFilter (optional) 
     * @param minDocMonthFilter (optional) 
     * @param maxDocYearFilter (optional) 
     * @param minDocYearFilter (optional) 
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
    getAll(filter: string | null | undefined, maxDocIDFilter: number | null | undefined, minDocIDFilter: number | null | undefined, maxDocdateFilter: moment.Moment | null | undefined, minDocdateFilter: moment.Moment | null | undefined, maxDocMonthFilter: number | null | undefined, minDocMonthFilter: number | null | undefined, maxDocYearFilter: number | null | undefined, minDocYearFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetAllowancesForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/Allowances/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxDocIDFilter !== undefined)
            url_ += "MaxDocIDFilter=" + encodeURIComponent("" + maxDocIDFilter) + "&";
        if (minDocIDFilter !== undefined)
            url_ += "MinDocIDFilter=" + encodeURIComponent("" + minDocIDFilter) + "&";
        if (maxDocdateFilter !== undefined)
            url_ += "MaxDocdateFilter=" + encodeURIComponent(maxDocdateFilter ? "" + maxDocdateFilter.toJSON() : "") + "&";
        if (minDocdateFilter !== undefined)
            url_ += "MinDocdateFilter=" + encodeURIComponent(minDocdateFilter ? "" + minDocdateFilter.toJSON() : "") + "&";
        if (maxDocMonthFilter !== undefined)
            url_ += "MaxDocMonthFilter=" + encodeURIComponent("" + maxDocMonthFilter) + "&";
        if (minDocMonthFilter !== undefined)
            url_ += "MinDocMonthFilter=" + encodeURIComponent("" + minDocMonthFilter) + "&";
        if (maxDocYearFilter !== undefined)
            url_ += "MaxDocYearFilter=" + encodeURIComponent("" + maxDocYearFilter) + "&";
        if (minDocYearFilter !== undefined)
            url_ += "MinDocYearFilter=" + encodeURIComponent("" + minDocYearFilter) + "&";
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
                    return <Observable<PagedResultDtoOfGetAllowancesForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetAllowancesForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetAllowancesForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetAllowancesForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetAllowancesForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetAllowancesForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getAllowancesForView(id: number | null | undefined): Observable<GetAllowancesForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/Allowances/GetAllowancesForView?";
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
            return this.processGetAllowancesForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAllowancesForView(<any>response_);
                } catch (e) {
                    return <Observable<GetAllowancesForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetAllowancesForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAllowancesForView(response: HttpResponseBase): Observable<GetAllowancesForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetAllowancesForViewDto.fromJS(resultData200) : new GetAllowancesForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetAllowancesForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getAllowancesForEdit(id: number | null | undefined): Observable<GetAllowancesForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/Allowances/GetAllowancesForEdit?";
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
            return this.processGetAllowancesForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAllowancesForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetAllowancesForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetAllowancesForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetAllowancesForEdit(response: HttpResponseBase): Observable<GetAllowancesForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetAllowancesForEditOutput.fromJS(resultData200) : new GetAllowancesForEditOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetAllowancesForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditAllowancesDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/Allowances/CreateOrEdit";
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
    getValidMonthID(input: CreateOrEditAllowancesDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/Allowances/CheckvalidMonth?";
        url_ += "Month=" + encodeURIComponent("" + input.docMonth) + "&";
        url_ += "Year=" + encodeURIComponent("" + input.docYear) + "&";
       
        url_ = url_.replace(/[?&]$/, "");

       

      let options_: any = {
        observe: "response",
        responseType: "blob",
        headers: new HttpHeaders({
            "Accept": "application/json"
        })
    };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
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

    protected processgetValidMonthID(response: HttpResponseBase): Observable<number> {
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
        let url_ = this.baseUrl + "/api/services/app/Allowances/Delete?";
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
     * @param maxDocIDFilter (optional) 
     * @param minDocIDFilter (optional) 
     * @param maxDocdateFilter (optional) 
     * @param minDocdateFilter (optional) 
     * @param maxDocMonthFilter (optional) 
     * @param minDocMonthFilter (optional) 
     * @param maxDocYearFilter (optional) 
     * @param minDocYearFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @return Success
     */
    getAllowancesToExcel(filter: string | null | undefined, maxDocIDFilter: number | null | undefined, minDocIDFilter: number | null | undefined, maxDocdateFilter: moment.Moment | null | undefined, minDocdateFilter: moment.Moment | null | undefined, maxDocMonthFilter: number | null | undefined, minDocMonthFilter: number | null | undefined, maxDocYearFilter: number | null | undefined, minDocYearFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/Allowances/GetAllowancesToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxDocIDFilter !== undefined)
            url_ += "MaxDocIDFilter=" + encodeURIComponent("" + maxDocIDFilter) + "&";
        if (minDocIDFilter !== undefined)
            url_ += "MinDocIDFilter=" + encodeURIComponent("" + minDocIDFilter) + "&";
        if (maxDocdateFilter !== undefined)
            url_ += "MaxDocdateFilter=" + encodeURIComponent(maxDocdateFilter ? "" + maxDocdateFilter.toJSON() : "") + "&";
        if (minDocdateFilter !== undefined)
            url_ += "MinDocdateFilter=" + encodeURIComponent(minDocdateFilter ? "" + minDocdateFilter.toJSON() : "") + "&";
        if (maxDocMonthFilter !== undefined)
            url_ += "MaxDocMonthFilter=" + encodeURIComponent("" + maxDocMonthFilter) + "&";
        if (minDocMonthFilter !== undefined)
            url_ += "MinDocMonthFilter=" + encodeURIComponent("" + minDocMonthFilter) + "&";
        if (maxDocYearFilter !== undefined)
            url_ += "MaxDocYearFilter=" + encodeURIComponent("" + maxDocYearFilter) + "&";
        if (minDocYearFilter !== undefined)
            url_ += "MinDocYearFilter=" + encodeURIComponent("" + minDocYearFilter) + "&";
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
            return this.processGetAllowancesToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAllowancesToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAllowancesToExcel(response: HttpResponseBase): Observable<FileDto> {
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
    url: string = "";
    getDataForMonth(MMS: string, YYS: string) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/Allowances/GetValidMonthYear?MM=" + MMS+"&YY="+YYS;
        // this.url += "&type=" + type;
        debugger;
        return this.http.get(this.url);
      }
    getMaxtID(): Observable<number> {
        let url_ = this.baseUrl + "/api/services/app/Allowances/GetMaxID";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetMaxID(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetMaxID(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processGetMaxID(response: HttpResponseBase): Observable<number> {
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

    getCarAllowanceData(docMonth: string,docYear: string): Observable<IPagedResultDtoOfGetAllowancesDetail> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/Allowances/GetCarAllowanceData?";
        // if (docDate !== undefined)
        url_ += "DocMonth=" + docMonth + "&";
        url_ += "DocYear=" + docYear;
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetCarAllowanceData(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetCarAllowanceData(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfGetAllowancesDetail>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetAllowancesDetail>><any>_observableThrow(response_);
        }));
    }

    protected processGetCarAllowanceData(response: HttpResponseBase): Observable<PagedResultDtoOfGetAllowancesDetail> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetAllowancesDetail.fromJS(resultData200) : new PagedResultDtoOfGetAllowancesDetail();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetAllowancesDetail>(<any>null);
    }

    

}