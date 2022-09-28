import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

import * as moment from 'moment';
import { BinaryData } from 'fs';
import { List } from 'lodash';
import { API_BASE_URL, PagedResultDtoOfGetAPTermForViewDto, blobToText, throwException, GetAPTermForViewDto, GetAPTermForEditOutput, CreateOrEditAPTermDto, FileDto } from '@shared/service-proxies/service-proxies';
import { PagedResultDtoOfGetARTermForViewDto, GetARTermForViewDto, GetARTermForEditOutput, CreateOrEditARTermDto } from '../dto/arTerm-dto';

@Injectable()
export class ARTermsServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional)
     * @param tERMDESCFilter (optional)
     * @param maxTERMRATEFilter (optional)
     * @param minTERMRATEFilter (optional)
     * @param maxAUDTDATEFilter (optional)
     * @param minAUDTDATEFilter (optional)
     * @param aUDTUSERFilter (optional)
     * @param iNACTIVEFilter (optional)
     * @param tERMTYPE (optional)
     * @param sorting (optional)
     * @param skipCount (optional)
     * @param maxResultCount (optional)
     * @return Success
     */
    getAll(filter: string | null | undefined, tERMDESCFilter: string | null | undefined, maxTERMRATEFilter: number | null | undefined, minTERMRATEFilter: number | null | undefined, maxAUDTDATEFilter: moment.Moment | null | undefined, minAUDTDATEFilter: moment.Moment | null | undefined, aUDTUSERFilter: string | null | undefined, iNACTIVEFilter: number | null | undefined, tERMTYPE: number | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetARTermForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/ARTerms/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (tERMDESCFilter !== undefined)
            url_ += "TERMDESCFilter=" + encodeURIComponent("" + tERMDESCFilter) + "&";
        if (maxTERMRATEFilter !== undefined)
            url_ += "MaxTERMRATEFilter=" + encodeURIComponent("" + maxTERMRATEFilter) + "&";
        if (minTERMRATEFilter !== undefined)
            url_ += "MinTERMRATEFilter=" + encodeURIComponent("" + minTERMRATEFilter) + "&";
        if (maxAUDTDATEFilter !== undefined)
            url_ += "MaxAUDTDATEFilter=" + encodeURIComponent(maxAUDTDATEFilter ? "" + maxAUDTDATEFilter.toJSON() : "") + "&";
        if (minAUDTDATEFilter !== undefined)
            url_ += "MinAUDTDATEFilter=" + encodeURIComponent(minAUDTDATEFilter ? "" + minAUDTDATEFilter.toJSON() : "") + "&";
        if (aUDTUSERFilter !== undefined)
            url_ += "AUDTUSERFilter=" + encodeURIComponent("" + aUDTUSERFilter) + "&";
        if (iNACTIVEFilter !== undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + iNACTIVEFilter) + "&";
        if (tERMTYPE !== undefined)
            url_ += "TERMTYPE=" + encodeURIComponent("" + tERMTYPE) + "&";
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
                    return <Observable<PagedResultDtoOfGetARTermForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetARTermForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetARTermForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfGetARTermForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetARTermForViewDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetARTermForViewDto>(<any>null);
    }

    /**
     * @param id (optional)
     * @return Success
     */
    getARTermForView(id: number | null | undefined): Observable<GetARTermForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/ARTerms/GetARTermForView?";
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
            return this.processGetARTermForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetARTermForView(<any>response_);
                } catch (e) {
                    return <Observable<GetARTermForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetARTermForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetARTermForView(response: HttpResponseBase): Observable<GetARTermForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GetARTermForViewDto.fromJS(resultData200) : new GetARTermForViewDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetARTermForViewDto>(<any>null);
    }

    /**
     * @param id (optional)
     * @return Success
     */
    getARTermForEdit(id: number | null | undefined): Observable<GetARTermForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/ARTerms/GetARTermForEdit?";
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
            return this.processGetARTermForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetARTermForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetARTermForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetARTermForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetARTermForEdit(response: HttpResponseBase): Observable<GetARTermForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GetARTermForEditOutput.fromJS(resultData200) : new GetARTermForEditOutput();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetARTermForEditOutput>(<any>null);
    }

    /**
     * @param input (optional)
     * @return Success
     */
    createOrEdit(input: CreateOrEditARTermDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/ARTerms/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/ARTerms/Delete?";
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
     * @param tERMDESCFilter (optional)
     * @param maxTERMRATEFilter (optional)
     * @param minTERMRATEFilter (optional)
     * @param maxAUDTDATEFilter (optional)
     * @param minAUDTDATEFilter (optional)
     * @param aUDTUSERFilter (optional)
     * @param iNACTIVEFilter (optional)
     * @param tERMTYPE (optional)
     * @return Success
     */
    getARTermsToExcel(filter: string | null | undefined, tERMDESCFilter: string | null | undefined, maxTERMRATEFilter: number | null | undefined, minTERMRATEFilter: number | null | undefined, maxAUDTDATEFilter: moment.Moment | null | undefined, minAUDTDATEFilter: moment.Moment | null | undefined, aUDTUSERFilter: string | null | undefined, iNACTIVEFilter: number | null | undefined, tERMTYPE: number | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/ARTerms/GetARTermsToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (tERMDESCFilter !== undefined)
            url_ += "TERMDESCFilter=" + encodeURIComponent("" + tERMDESCFilter) + "&";
        if (maxTERMRATEFilter !== undefined)
            url_ += "MaxTERMRATEFilter=" + encodeURIComponent("" + maxTERMRATEFilter) + "&";
        if (minTERMRATEFilter !== undefined)
            url_ += "MinTERMRATEFilter=" + encodeURIComponent("" + minTERMRATEFilter) + "&";
        if (maxAUDTDATEFilter !== undefined)
            url_ += "MaxAUDTDATEFilter=" + encodeURIComponent(maxAUDTDATEFilter ? "" + maxAUDTDATEFilter.toJSON() : "") + "&";
        if (minAUDTDATEFilter !== undefined)
            url_ += "MinAUDTDATEFilter=" + encodeURIComponent(minAUDTDATEFilter ? "" + minAUDTDATEFilter.toJSON() : "") + "&";
        if (aUDTUSERFilter !== undefined)
            url_ += "AUDTUSERFilter=" + encodeURIComponent("" + aUDTUSERFilter) + "&";
        if (iNACTIVEFilter !== undefined)
            url_ += "INACTIVEFilter=" + encodeURIComponent("" + iNACTIVEFilter) + "&";
        if (tERMTYPE !== undefined)
            url_ += "TERMTYPE=" + encodeURIComponent("" + tERMTYPE) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetARTermsToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetARTermsToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetARTermsToExcel(response: HttpResponseBase): Observable<FileDto> {
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
