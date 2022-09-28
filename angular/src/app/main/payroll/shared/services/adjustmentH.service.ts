import {
    HttpClient,
    HttpHeaders,
    HttpResponse,
    HttpResponseBase,
} from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import {
    API_BASE_URL,
    blobToText,
    throwException,
} from "@shared/service-proxies/service-proxies";
import { url } from "inspector";
import {
    AdjHDto,
    GetAdjHForEditOutput,
    PagedResultDtoOfGetAdjHForViewDto,
} from "../dto/AdjH-dto";
import {
    Observable,
    throwError as _observableThrow,
    of as _observableOf,
} from "rxjs";
import {
    mergeMap as _observableMergeMap,
    catchError as _observableCatch,
} from "rxjs/operators";
import * as moment from "moment";

@Injectable({
    providedIn: "root",
})
export class AdjustmentHServiceProxy {
    private baseUrl: string;
    protected jsonParseReviver:
        | ((key: string, value: any) => any)
        | undefined = undefined;
    constructor(
        private http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    getMaxDocID() {
        let url_ = this.baseUrl + "/api/services/app/AdjH/GetMaxDocID";
        url_ = url_.replace(/[?&]$/, "");

        return this.http.get(url_);
    }

    createOrEdit(input: AdjHDto | null | undefined) {
        let url_ = this.baseUrl + "/api/services/app/AdjH/CreateOrEdit";
        url_ = url_.replace(/[?&]$/, "");
        const content_ = JSON.stringify(input);

        let options_: any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json",
            }),
        };

        debugger;

        return this.http
            .request("post", url_, options_)
            .pipe(
                _observableMergeMap((response_: any) => {
                    return this.processCreateOrEdit(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processCreateOrEdit(<any>response_);
                        } catch (e) {
                            return <Observable<void>>(<any>_observableThrow(e));
                        }
                    } else
                        return <Observable<void>>(
                            (<any>_observableThrow(response_))
                        );
                })
            );
    }

    protected processCreateOrEdit(
        response: HttpResponseBase
    ): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse
                ? response.body
                : (<any>response).error instanceof Blob
                    ? (<any>response).error
                    : undefined;

        let _headers: any = {};
        if (response.headers) {
            for (let key of response.headers.keys()) {
                _headers[key] = response.headers.get(key);
            }
        }
        if (status === 200) {
            return blobToText(responseBlob).pipe(
                _observableMergeMap((_responseText) => {
                    return _observableOf<void>(<any>null);
                })
            );
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(
                _observableMergeMap((_responseText) => {
                    return throwException(
                        "An unexpected server error occurred.",
                        status,
                        _responseText,
                        _headers
                    );
                })
            );
        }
        return _observableOf<void>(<any>null);
    }

    getAll(
        filter: string | null | undefined,
        maxDocIDFilter: number | null | undefined,
        minDocIDFilter: number | null | undefined,
        maxEmployeeIDFilter: number | null | undefined,
        minEmployeeIDFilter: number | null | undefined,
        employeeNameFilter: string | null | undefined,
        maxSalaryYearFilter: number | null | undefined,
        minSalaryYearFilter: number | null | undefined,
        maxSalaryMonthFilter: number | null | undefined,
        minSalaryMonthFilter: number | null | undefined,
        maxDocDateFilter: moment.Moment | null | undefined,
        minDocDateFilter: moment.Moment | null | undefined,
        maxAmountFilter: number | null | undefined,
        minAmountFilter: number | null | undefined,
        activeFilter: number | null | undefined,
        audtUserFilter: string | null | undefined,
        maxAudtDateFilter: moment.Moment | null | undefined,
        minAudtDateFilter: moment.Moment | null | undefined,
        createdByFilter: string | null | undefined,
        maxCreateDateFilter: moment.Moment | null | undefined,
        minCreateDateFilter: moment.Moment | null | undefined,
        docType: number | null | undefined,
        sorting: string | null | undefined,
        skipCount: number | null | undefined,
        maxResultCount: number | null | undefined
    ): Observable<PagedResultDtoOfGetAdjHForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/AdjH/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxDocIDFilter !== undefined)
            url_ +=
                "MaxDocIDFilter=" +
                encodeURIComponent("" + maxDocIDFilter) +
                "&";
        if (minDocIDFilter !== undefined)
            url_ +=
                "MinDocIDFilter=" +
                encodeURIComponent("" + minDocIDFilter) +
                "&";
        if (maxEmployeeIDFilter !== undefined)
            url_ +=
                "MaxEmployeeIDFilter=" +
                encodeURIComponent("" + maxEmployeeIDFilter) +
                "&";
        if (minEmployeeIDFilter !== undefined)
            url_ +=
                "MinEmployeeIDFilter=" +
                encodeURIComponent("" + minEmployeeIDFilter) +
                "&";
        if (employeeNameFilter !== undefined)
            url_ +=
                "EmployeeNameFilter=" +
                encodeURIComponent("" + employeeNameFilter) +
                "&";
        if (maxSalaryYearFilter !== undefined)
            url_ +=
                "MaxSalaryYearFilter=" +
                encodeURIComponent("" + maxSalaryYearFilter) +
                "&";
        if (minSalaryYearFilter !== undefined)
            url_ +=
                "MinSalaryYearFilter=" +
                encodeURIComponent("" + minSalaryYearFilter) +
                "&";
        if (maxSalaryMonthFilter !== undefined)
            url_ +=
                "MaxSalaryMonthFilter=" +
                encodeURIComponent("" + maxSalaryMonthFilter) +
                "&";
        if (minSalaryMonthFilter !== undefined)
            url_ +=
                "MinSalaryMonthFilter=" +
                encodeURIComponent("" + minSalaryMonthFilter) +
                "&";
        if (maxDocDateFilter !== undefined)
            url_ +=
                "MaxDocDateFilter=" +
                encodeURIComponent(
                    maxDocDateFilter ? "" + maxDocDateFilter.toJSON() : ""
                ) +
                "&";
        if (minDocDateFilter !== undefined)
            url_ +=
                "MinDocDateFilter=" +
                encodeURIComponent(
                    minDocDateFilter ? "" + minDocDateFilter.toJSON() : ""
                ) +
                "&";
        if (maxAmountFilter !== undefined)
            url_ +=
                "MaxAmountFilter=" +
                encodeURIComponent("" + maxAmountFilter) +
                "&";
        if (minAmountFilter !== undefined)
            url_ +=
                "MinAmountFilter=" +
                encodeURIComponent("" + minAmountFilter) +
                "&";
        if (activeFilter !== undefined)
            url_ +=
                "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&";
        if (audtUserFilter !== undefined)
            url_ +=
                "AudtUserFilter=" +
                encodeURIComponent("" + audtUserFilter) +
                "&";
        if (maxAudtDateFilter !== undefined)
            url_ +=
                "MaxAudtDateFilter=" +
                encodeURIComponent(
                    maxAudtDateFilter ? "" + maxAudtDateFilter.toJSON() : ""
                ) +
                "&";
        if (minAudtDateFilter !== undefined)
            url_ +=
                "MinAudtDateFilter=" +
                encodeURIComponent(
                    minAudtDateFilter ? "" + minAudtDateFilter.toJSON() : ""
                ) +
                "&";
        if (createdByFilter !== undefined)
            url_ +=
                "CreatedByFilter=" +
                encodeURIComponent("" + createdByFilter) +
                "&";
        if (maxCreateDateFilter !== undefined)
            url_ +=
                "MaxCreateDateFilter=" +
                encodeURIComponent(
                    maxCreateDateFilter ? "" + maxCreateDateFilter.toJSON() : ""
                ) +
                "&";
        if (minCreateDateFilter !== undefined)
            url_ +=
                "MinCreateDateFilter=" +
                encodeURIComponent(
                    minCreateDateFilter ? "" + minCreateDateFilter.toJSON() : ""
                ) +
                "&";
        if (docType !== undefined)
            url_ +=
                "MaxDocTypeFilter=" + encodeURIComponent("" + docType) + "&";
        if (sorting !== undefined)
            url_ += "Sorting=" + encodeURIComponent("" + sorting) + "&";
        if (skipCount !== undefined)
            url_ += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        if (maxResultCount !== undefined)
            url_ +=
                "MaxResultCount=" +
                encodeURIComponent("" + maxResultCount) +
                "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                Accept: "application/json",
            }),
        };

        return this.http
            .request("get", url_, options_)
            .pipe(
                _observableMergeMap((response_: any) => {
                    return this.processGetAll(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processGetAll(<any>response_);
                        } catch (e) {
                            return <
                                Observable<PagedResultDtoOfGetAdjHForViewDto>
                                >(<any>_observableThrow(e));
                        }
                    } else
                        return <Observable<PagedResultDtoOfGetAdjHForViewDto>>(
                            (<any>_observableThrow(response_))
                        );
                })
            );
    }

    protected processGetAll(
        response: HttpResponseBase
    ): Observable<PagedResultDtoOfGetAdjHForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse
                ? response.body
                : (<any>response).error instanceof Blob
                    ? (<any>response).error
                    : undefined;

        let _headers: any = {};
        if (response.headers) {
            for (let key of response.headers.keys()) {
                _headers[key] = response.headers.get(key);
            }
        }
        if (status === 200) {
            return blobToText(responseBlob).pipe(
                _observableMergeMap((_responseText) => {
                    let result200: any = null;
                    let resultData200 =
                        _responseText === ""
                            ? null
                            : JSON.parse(_responseText, this.jsonParseReviver);
                    result200 = resultData200
                        ? PagedResultDtoOfGetAdjHForViewDto.fromJS(
                            resultData200
                        )
                        : new PagedResultDtoOfGetAdjHForViewDto();
                    return _observableOf(result200);
                })
            );
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(
                _observableMergeMap((_responseText) => {
                    return throwException(
                        "An unexpected server error occurred.",
                        status,
                        _responseText,
                        _headers
                    );
                })
            );
        }
        return _observableOf<PagedResultDtoOfGetAdjHForViewDto>(<any>null);
    }

    getAdjHForEdit(
        id: number | null | undefined
    ): Observable<GetAdjHForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/AdjH/GetAdjHForEdit?";
        if (id !== undefined) url_ += "Id=" + encodeURIComponent("" + id) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                Accept: "application/json",
            }),
        };

        return this.http
            .request("get", url_, options_)
            .pipe(
                _observableMergeMap((response_: any) => {
                    return this.processGetAdjHForEdit(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processGetAdjHForEdit(<any>response_);
                        } catch (e) {
                            return <Observable<GetAdjHForEditOutput>>(
                                (<any>_observableThrow(e))
                            );
                        }
                    } else
                        return <Observable<GetAdjHForEditOutput>>(
                            (<any>_observableThrow(response_))
                        );
                })
            );
    }

    protected processGetAdjHForEdit(
        response: HttpResponseBase
    ): Observable<GetAdjHForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse
                ? response.body
                : (<any>response).error instanceof Blob
                    ? (<any>response).error
                    : undefined;

        let _headers: any = {};
        if (response.headers) {
            for (let key of response.headers.keys()) {
                _headers[key] = response.headers.get(key);
            }
        }
        if (status === 200) {
            return blobToText(responseBlob).pipe(
                _observableMergeMap((_responseText) => {
                    let result200: any = null;
                    let resultData200 =
                        _responseText === ""
                            ? null
                            : JSON.parse(_responseText, this.jsonParseReviver);
                    result200 = resultData200
                        ? GetAdjHForEditOutput.fromJS(resultData200)
                        : new GetAdjHForEditOutput();
                    return _observableOf(result200);
                })
            );
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(
                _observableMergeMap((_responseText) => {
                    return throwException(
                        "An unexpected server error occurred.",
                        status,
                        _responseText,
                        _headers
                    );
                })
            );
        }
        return _observableOf<GetAdjHForEditOutput>(<any>null);
    }

    deleteAdjH(adjh: AdjHDto) {
        let url_ = this.baseUrl + "/api/services/app/AdjH/Delete?";
      //  url_ = url_.replace(/[?&]$/, "");

        if (adjh.id !== undefined)
            url_ += "Id=" + encodeURIComponent("" + adjh.id) + "&";
            if (adjh.docType !== undefined)
            url_ += "DocType=" + encodeURIComponent("" + adjh.docType) + "&";
        url_ = url_.replace(/[?&]$/, "");
debugger
        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
            })
        };

        return this.http.request("delete", url_);
    }
}
