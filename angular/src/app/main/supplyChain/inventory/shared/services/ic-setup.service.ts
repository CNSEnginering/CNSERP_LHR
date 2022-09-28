import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PagedResultDtoOfICSetupDto, ICSetupDto } from '../dto/ic-setup-dto';

@Injectable({
    providedIn: 'root'
  })
export class ICSetupsService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param segment1Filter (optional) 
     * @param segment2Filter (optional) 
     * @param segment3Filter (optional) 
     * @param maxErrSrNoFilter (optional) 
     * @param minErrSrNoFilter (optional) 
     * @param maxCostingMethodFilter (optional) 
     * @param minCostingMethodFilter (optional) 
     * @param pRBookIDFilter (optional) 
     * @param rTBookIDFilter (optional) 
     * @param cnsBookIDFilter (optional) 
     * @param sLBookIDFilter (optional) 
     * @param sRBookIDFilter (optional) 
     * @param tRBookIDFilter (optional) 
     * @param prdBookIDFilter (optional) 
     * @param pyRecBookIDFilter (optional) 
     * @param adjBookIDFilter (optional) 
     * @param asmBookIDFilter (optional) 
     * @param wSBookIDFilter (optional) 
     * @param dSBookIDFilter (optional) 
     * @param maxCurrentLocIDFilter (optional) 
     * @param minCurrentLocIDFilter (optional) 
     * @param opt4Filter (optional) 
     * @param opt5Filter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateadOnFilter (optional) 
     * @param minCreateadOnFilter (optional) 
     * @param sorting (optional) 
     * @param skipCount (optional) 
     * @param maxResultCount (optional) 
     * @return Success
     */
    getAll(filter?: string | null | undefined, segment1Filter?: string | null | undefined, segment2Filter?: string | null | undefined, segment3Filter?: string | null | undefined, maxErrSrNoFilter?: number | null | undefined, minErrSrNoFilter?: number | null | undefined, maxCostingMethodFilter?: number | null | undefined, minCostingMethodFilter?: number | null | undefined, pRBookIDFilter?: string | null | undefined, rTBookIDFilter?: string | null | undefined, cnsBookIDFilter?: string | null | undefined, sLBookIDFilter?: string | null | undefined, sRBookIDFilter?: string | null | undefined, tRBookIDFilter?: string | null | undefined, prdBookIDFilter?: string | null | undefined, pyRecBookIDFilter?: string | null | undefined, adjBookIDFilter?: string | null | undefined, asmBookIDFilter?: string | null | undefined, wSBookIDFilter?: string | null | undefined, dSBookIDFilter?: string | null | undefined,  maxCurrentLocIDFilter?: number | null | undefined, minCurrentLocIDFilter?: number | null | undefined,  opt4Filter?: string | null | undefined, opt5Filter?: string | null | undefined, createdByFilter?: string | null | undefined, maxCreateadOnFilter?: moment.Moment | null | undefined, minCreateadOnFilter?: moment.Moment | null | undefined, sorting?: string | null | undefined, skipCount?: number | null | undefined, maxResultCount?: number | null | undefined): Observable<PagedResultDtoOfICSetupDto> {
        let url_ = this.baseUrl + "/api/services/app/ICSetups/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (segment1Filter !== undefined)
            url_ += "Segment1Filter=" + encodeURIComponent("" + segment1Filter) + "&"; 
        if (segment2Filter !== undefined)
            url_ += "Segment2Filter=" + encodeURIComponent("" + segment2Filter) + "&"; 
        if (segment3Filter !== undefined)
            url_ += "Segment3Filter=" + encodeURIComponent("" + segment3Filter) + "&"; 
        if (maxErrSrNoFilter !== undefined)
            url_ += "MaxErrSrNoFilter=" + encodeURIComponent("" + maxErrSrNoFilter) + "&"; 
        if (minErrSrNoFilter !== undefined)
            url_ += "MinErrSrNoFilter=" + encodeURIComponent("" + minErrSrNoFilter) + "&"; 
        if (maxCostingMethodFilter !== undefined)
            url_ += "MaxCostingMethodFilter=" + encodeURIComponent("" + maxCostingMethodFilter) + "&"; 
        if (minCostingMethodFilter !== undefined)
            url_ += "MinCostingMethodFilter=" + encodeURIComponent("" + minCostingMethodFilter) + "&"; 
        if (pRBookIDFilter !== undefined)
            url_ += "PRBookIDFilter=" + encodeURIComponent("" + pRBookIDFilter) + "&"; 
        if (rTBookIDFilter !== undefined)
            url_ += "RTBookIDFilter=" + encodeURIComponent("" + rTBookIDFilter) + "&"; 
        if (cnsBookIDFilter !== undefined)
            url_ += "CnsBookIDFilter=" + encodeURIComponent("" + cnsBookIDFilter) + "&"; 
        if (sLBookIDFilter !== undefined)
            url_ += "SLBookIDFilter=" + encodeURIComponent("" + sLBookIDFilter) + "&"; 
        if (sRBookIDFilter !== undefined)
            url_ += "SRBookIDFilter=" + encodeURIComponent("" + sRBookIDFilter) + "&"; 
        if (tRBookIDFilter !== undefined)
            url_ += "TRBookIDFilter=" + encodeURIComponent("" + tRBookIDFilter) + "&"; 
        if (prdBookIDFilter !== undefined)
            url_ += "PrdBookIDFilter=" + encodeURIComponent("" + prdBookIDFilter) + "&"; 
        if (pyRecBookIDFilter !== undefined)
            url_ += "PyRecBookIDFilter=" + encodeURIComponent("" + pyRecBookIDFilter) + "&"; 
        if (adjBookIDFilter !== undefined)
            url_ += "AdjBookIDFilter=" + encodeURIComponent("" + adjBookIDFilter) + "&"; 
        if (asmBookIDFilter !== undefined)
            url_ += "AsmBookIDFilter=" + encodeURIComponent("" + asmBookIDFilter) + "&"; 
        if (wSBookIDFilter !== undefined)
            url_ += "WSBookIDFilter=" + encodeURIComponent("" + wSBookIDFilter) + "&"; 
        if (dSBookIDFilter !== undefined)
            url_ += "DSBookIDFilter=" + encodeURIComponent("" + dSBookIDFilter) + "&"; 
        if (maxCurrentLocIDFilter !== undefined)
            url_ += "MaxCurrentLocIDFilter=" + encodeURIComponent("" + maxCurrentLocIDFilter) + "&"; 
        if (minCurrentLocIDFilter !== undefined)
            url_ += "MinCurrentLocIDFilter=" + encodeURIComponent("" + minCurrentLocIDFilter) + "&"; 
        if (opt4Filter !== undefined)
            url_ += "Opt4Filter=" + encodeURIComponent("" + opt4Filter) + "&"; 
        if (opt5Filter !== undefined)
            url_ += "Opt5Filter=" + encodeURIComponent("" + opt5Filter) + "&"; 
        if (createdByFilter !== undefined)
            url_ += "CreatedByFilter=" + encodeURIComponent("" + createdByFilter) + "&"; 
        if (maxCreateadOnFilter !== undefined)
            url_ += "MaxCreateadOnFilter=" + encodeURIComponent(maxCreateadOnFilter ? "" + maxCreateadOnFilter.toJSON() : "") + "&"; 
        if (minCreateadOnFilter !== undefined)
            url_ += "MinCreateadOnFilter=" + encodeURIComponent(minCreateadOnFilter ? "" + minCreateadOnFilter.toJSON() : "") + "&"; 
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
                    return <Observable<PagedResultDtoOfICSetupDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfICSetupDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfICSetupDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfICSetupDto.fromJS(resultData200) : new PagedResultDtoOfICSetupDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfICSetupDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getICSetupForView(id: number | null | undefined): Observable<ICSetupDto> {
        let url_ = this.baseUrl + "/api/services/app/ICSetups/GetICSetupForView?";
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
            return this.processGetICSetupForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetICSetupForView(<any>response_);
                } catch (e) {
                    return <Observable<ICSetupDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<ICSetupDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetICSetupForView(response: HttpResponseBase): Observable<ICSetupDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? ICSetupDto.fromJS(resultData200) : new ICSetupDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<ICSetupDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getICSetupForEdit(id: number | null | undefined): Observable<ICSetupDto> {
        let url_ = this.baseUrl + "/api/services/app/ICSetups/GetICSetupForEdit?";
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
            return this.processGetICSetupForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetICSetupForEdit(<any>response_);
                } catch (e) {
                    return <Observable<ICSetupDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<ICSetupDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetICSetupForEdit(response: HttpResponseBase): Observable<ICSetupDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? ICSetupDto.fromJS(resultData200) : new ICSetupDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<ICSetupDto>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: ICSetupDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/ICSetups/CreateOrEdit";
        url_ = url_.replace(/[?&]$/, "");
debugger
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
        let url_ = this.baseUrl + "/api/services/app/ICSetups/Delete?";
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
     * @param segment1Filter (optional) 
     * @param segment2Filter (optional) 
     * @param segment3Filter (optional) 
     * @param maxAllowNegativeFilter (optional) 
     * @param minAllowNegativeFilter (optional) 
     * @param maxErrSrNoFilter (optional) 
     * @param minErrSrNoFilter (optional) 
     * @param maxCostingMethodFilter (optional) 
     * @param minCostingMethodFilter (optional) 
     * @param pRBookIDFilter (optional) 
     * @param rTBookIDFilter (optional) 
     * @param cnsBookIDFilter (optional) 
     * @param sLBookIDFilter (optional) 
     * @param sRBookIDFilter (optional) 
     * @param tRBookIDFilter (optional) 
     * @param prdBookIDFilter (optional) 
     * @param pyRecBookIDFilter (optional) 
     * @param adjBookIDFilter (optional) 
     * @param asmBookIDFilter (optional) 
     * @param wSBookIDFilter (optional) 
     * @param dSBookIDFilter (optional) 
     * @param maxCurrentLocIDFilter (optional) 
     * @param minCurrentLocIDFilter (optional) 
     * @param opt4Filter (optional) 
     * @param opt5Filter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateadOnFilter (optional) 
     * @param minCreateadOnFilter (optional) 
     * @param sorting (optional) 
     * @param skipCount (optional) 
     * @param maxResultCount (optional) 
     * @return Success
     */
    getICSetupsToExcel(filter: string | null | undefined, segment1Filter: string | null | undefined, segment2Filter: string | null | undefined, segment3Filter: string | null | undefined, maxErrSrNoFilter: number | null | undefined, minErrSrNoFilter: number | null | undefined, maxCostingMethodFilter: number | null | undefined, minCostingMethodFilter: number | null | undefined, pRBookIDFilter: string | null | undefined, rTBookIDFilter: string | null | undefined, cnsBookIDFilter: string | null | undefined, sLBookIDFilter: string | null | undefined, sRBookIDFilter: string | null | undefined, tRBookIDFilter: string | null | undefined, prdBookIDFilter: string | null | undefined, pyRecBookIDFilter: string | null | undefined, adjBookIDFilter: string | null | undefined, asmBookIDFilter: string | null | undefined, wSBookIDFilter: string | null | undefined, dSBookIDFilter: string | null | undefined, maxCurrentLocIDFilter: number | null | undefined, minCurrentLocIDFilter: number | null | undefined, opt4Filter: string | null | undefined, opt5Filter: string | null | undefined, createdByFilter: string | null | undefined, maxCreateadOnFilter: moment.Moment | null | undefined, minCreateadOnFilter: moment.Moment | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/ICSetups/GetICSetupsToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (segment1Filter !== undefined)
            url_ += "Segment1Filter=" + encodeURIComponent("" + segment1Filter) + "&"; 
        if (segment2Filter !== undefined)
            url_ += "Segment2Filter=" + encodeURIComponent("" + segment2Filter) + "&"; 
        if (segment3Filter !== undefined)
            url_ += "Segment3Filter=" + encodeURIComponent("" + segment3Filter) + "&"; 
        if (maxErrSrNoFilter !== undefined)
            url_ += "MaxErrSrNoFilter=" + encodeURIComponent("" + maxErrSrNoFilter) + "&"; 
        if (minErrSrNoFilter !== undefined)
            url_ += "MinErrSrNoFilter=" + encodeURIComponent("" + minErrSrNoFilter) + "&"; 
        if (maxCostingMethodFilter !== undefined)
            url_ += "MaxCostingMethodFilter=" + encodeURIComponent("" + maxCostingMethodFilter) + "&"; 
        if (minCostingMethodFilter !== undefined)
            url_ += "MinCostingMethodFilter=" + encodeURIComponent("" + minCostingMethodFilter) + "&"; 
        if (pRBookIDFilter !== undefined)
            url_ += "PRBookIDFilter=" + encodeURIComponent("" + pRBookIDFilter) + "&"; 
        if (rTBookIDFilter !== undefined)
            url_ += "RTBookIDFilter=" + encodeURIComponent("" + rTBookIDFilter) + "&"; 
        if (cnsBookIDFilter !== undefined)
            url_ += "CnsBookIDFilter=" + encodeURIComponent("" + cnsBookIDFilter) + "&"; 
        if (sLBookIDFilter !== undefined)
            url_ += "SLBookIDFilter=" + encodeURIComponent("" + sLBookIDFilter) + "&"; 
        if (sRBookIDFilter !== undefined)
            url_ += "SRBookIDFilter=" + encodeURIComponent("" + sRBookIDFilter) + "&"; 
        if (tRBookIDFilter !== undefined)
            url_ += "TRBookIDFilter=" + encodeURIComponent("" + tRBookIDFilter) + "&"; 
        if (prdBookIDFilter !== undefined)
            url_ += "PrdBookIDFilter=" + encodeURIComponent("" + prdBookIDFilter) + "&"; 
        if (pyRecBookIDFilter !== undefined)
            url_ += "PyRecBookIDFilter=" + encodeURIComponent("" + pyRecBookIDFilter) + "&"; 
        if (adjBookIDFilter !== undefined)
            url_ += "AdjBookIDFilter=" + encodeURIComponent("" + adjBookIDFilter) + "&"; 
        if (asmBookIDFilter !== undefined)
            url_ += "AsmBookIDFilter=" + encodeURIComponent("" + asmBookIDFilter) + "&"; 
        if (wSBookIDFilter !== undefined)
            url_ += "WSBookIDFilter=" + encodeURIComponent("" + wSBookIDFilter) + "&"; 
        if (dSBookIDFilter !== undefined)
            url_ += "DSBookIDFilter=" + encodeURIComponent("" + dSBookIDFilter) + "&"; 
        if (maxCurrentLocIDFilter !== undefined)
            url_ += "MaxCurrentLocIDFilter=" + encodeURIComponent("" + maxCurrentLocIDFilter) + "&"; 
        if (minCurrentLocIDFilter !== undefined)
            url_ += "MinCurrentLocIDFilter=" + encodeURIComponent("" + minCurrentLocIDFilter) + "&"; 
        if (opt4Filter !== undefined)
            url_ += "Opt4Filter=" + encodeURIComponent("" + opt4Filter) + "&"; 
        if (opt5Filter !== undefined)
            url_ += "Opt5Filter=" + encodeURIComponent("" + opt5Filter) + "&"; 
        if (createdByFilter !== undefined)
            url_ += "CreatedByFilter=" + encodeURIComponent("" + createdByFilter) + "&"; 
        if (maxCreateadOnFilter !== undefined)
            url_ += "MaxCreateadOnFilter=" + encodeURIComponent(maxCreateadOnFilter ? "" + maxCreateadOnFilter.toJSON() : "") + "&"; 
        if (minCreateadOnFilter !== undefined)
            url_ += "MinCreateadOnFilter=" + encodeURIComponent(minCreateadOnFilter ? "" + minCreateadOnFilter.toJSON() : "") + "&"; 
        
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetICSetupsToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetICSetupsToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetICSetupsToExcel(response: HttpResponseBase): Observable<FileDto> {
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
}

