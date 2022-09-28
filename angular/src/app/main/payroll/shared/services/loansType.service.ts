import { Injectable, Inject, Optional } from "@angular/core";
import {
    HttpClient,
    HttpHeaders,
    HttpResponseBase,
    HttpResponse,
} from "@angular/common/http";
import {
    API_BASE_URL,
    throwException,
    blobToText,
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import {
    PagedResultDtoOfGetLoansTypeForViewDto,
    CreateOrEditLoansTypeDto,
    GetLoansTypeForEditOutput,
} from "../dto/loanTypes-dto";
import {
    mergeMap as _observableMergeMap,
    catchError as _observableCatch,
} from "rxjs/operators";
import {
    Observable,
    throwError as _observableThrow,
    of as _observableOf,
} from "rxjs";

@Injectable({
    providedIn: "root",
})
export class LoansTypeService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver:
        | ((key: string, value: any) => any)
        | undefined = undefined;

    constructor(
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }
    /**
     * @param filter (optional)
     * @param maxDeptIDFilter (optional)
     * @param minDeptIDFilter (optional)
     * @param deptNameFilter (optional)
     * @param maxActiveFilter (optional)
     * @param minActiveFilter (optional)
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
    getAll(
        filter: string | null | undefined,
        maxDeptIDFilter: number | null | undefined,
        minDeptIDFilter: number | null | undefined,
        deptNameFilter: string | null | undefined,
        maxActiveFilter: number | null | undefined,
        minActiveFilter: number | null | undefined,
        audtUserFilter: string | null | undefined,
        maxAudtDateFilter: moment.Moment | null | undefined,
        minAudtDateFilter: moment.Moment | null | undefined,
        createdByFilter: string | null | undefined,
        maxCreateDateFilter: moment.Moment | null | undefined,
        minCreateDateFilter: moment.Moment | null | undefined,
        sorting: string | null | undefined,
        skipCount: number | null | undefined,
        maxResultCount: number | null | undefined
    ): Observable<PagedResultDtoOfGetLoansTypeForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeLoansType/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxDeptIDFilter !== undefined)
            url_ +=
                "MaxDeptIDFilter=" +
                encodeURIComponent("" + maxDeptIDFilter) +
                "&";
        if (minDeptIDFilter !== undefined)
            url_ +=
                "MinDeptIDFilter=" +
                encodeURIComponent("" + minDeptIDFilter) +
                "&";
        if (deptNameFilter !== undefined)
            url_ +=
                "DeptNameFilter=" +
                encodeURIComponent("" + deptNameFilter) +
                "&";
        if (maxActiveFilter !== undefined)
            url_ +=
                "MaxActiveFilter=" +
                encodeURIComponent("" + maxActiveFilter) +
                "&";
        if (minActiveFilter !== undefined)
            url_ +=
                "MinActiveFilter=" +
                encodeURIComponent("" + minActiveFilter) +
                "&";
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
                                Observable<
                                    PagedResultDtoOfGetLoansTypeForViewDto
                                >
                            >(<any>_observableThrow(e));
                        }
                    } else
                        return <
                            Observable<PagedResultDtoOfGetLoansTypeForViewDto>
                        >(<any>_observableThrow(response_));
                })
            );
    }

    protected processGetAll(
        response: HttpResponseBase
    ): Observable<PagedResultDtoOfGetLoansTypeForViewDto> {
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
                        ? PagedResultDtoOfGetLoansTypeForViewDto.fromJS(
                              resultData200
                          )
                        : new PagedResultDtoOfGetLoansTypeForViewDto();
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
        return _observableOf<PagedResultDtoOfGetLoansTypeForViewDto>(<any>null);
    }

    /**
     * @param input (optional)
     * @return Success
     */
    createOrEdit(
        input: CreateOrEditLoansTypeDto | null | undefined
    ): Observable<void> {
        let url_ =
            this.baseUrl + "/api/services/app/EmployeeLoansType/CreateOrEdit";
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

    /**
     * @param id (optional)
     * @return Success
     */
    delete(id: number | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeLoansType/Delete?";
        if (id !== undefined) url_ += "Id=" + encodeURIComponent("" + id) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({}),
        };

        return this.http
            .request("delete", url_, options_)
            .pipe(
                _observableMergeMap((response_: any) => {
                    return this.processDelete(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processDelete(<any>response_);
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
    protected processDelete(response: HttpResponseBase): Observable<void> {
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

    getEmployeeLoansTypeForEdit(
        id: number | null | undefined
    ): Observable<GetLoansTypeForEditOutput> {
        let url_ =
            this.baseUrl +
            "/api/services/app/EmployeeLoansType/GetEmployeeLoansTypeForEdit?";
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
                    return this.processEmployeeLoansTypeForEdit(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processEmployeeLoansTypeForEdit(
                                <any>response_
                            );
                        } catch (e) {
                            return <Observable<GetLoansTypeForEditOutput>>(
                                (<any>_observableThrow(e))
                            );
                        }
                    } else
                        return <Observable<GetLoansTypeForEditOutput>>(
                            (<any>_observableThrow(response_))
                        );
                })
            );
    }

    protected processEmployeeLoansTypeForEdit(
        response: HttpResponseBase
    ): Observable<GetLoansTypeForEditOutput> {
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
                        ? GetLoansTypeForEditOutput.fromJS(resultData200)
                        : new GetLoansTypeForEditOutput();
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
        return _observableOf<GetLoansTypeForEditOutput>(<any>null);
    }

    getMaxId() {
        let url_ =
            this.baseUrl + "/api/services/app/EmployeeLoansType/GetMaxID";
        return this.http.request("get", url_);
    }
}
