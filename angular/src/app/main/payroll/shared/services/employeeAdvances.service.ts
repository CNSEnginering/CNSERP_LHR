import {
    mergeMap as _observableMergeMap,
    catchError as _observableCatch,
} from "rxjs/operators";
import {
    Observable,
    throwError as _observableThrow,
    of as _observableOf,
} from "rxjs";
import { Injectable, Optional, Inject } from "@angular/core";
import {
    HttpClient,
    HttpHeaders,
    HttpResponse,
    HttpResponseBase,
} from "@angular/common/http";
import {
    API_BASE_URL,
    FileDto,
    blobToText,
    throwException,
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import {
    PagedResultDtoOfGetEmployeeAdvancesForViewDto,
    GetEmployeeAdvancesForViewDto,
    GetEmployeeAdvancesForEditOutput,
    CreateOrEditEmployeeAdvancesDto,
} from "../dto/advances-dto";

@Injectable({
    providedIn: "root",
})
export class EmployeeAdvancesServiceProxy {
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
     * @param sorting (optional)
     * @param skipCount (optional)
     * @param maxResultCount (optional)
     * @return Success
     */
    getAll(
        filter: string | null | undefined,
        sorting: string | null | undefined,
        skipCount: number | null | undefined,
        maxResultCount: number | null | undefined
    ): Observable<PagedResultDtoOfGetEmployeeAdvancesForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeAdvances/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";

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
                                    PagedResultDtoOfGetEmployeeAdvancesForViewDto
                                >
                            >(<any>_observableThrow(e));
                        }
                    } else
                        return <
                            Observable<
                                PagedResultDtoOfGetEmployeeAdvancesForViewDto
                            >
                        >(<any>_observableThrow(response_));
                })
            );
    }

    protected processGetAll(
        response: HttpResponseBase
    ): Observable<PagedResultDtoOfGetEmployeeAdvancesForViewDto> {
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
                        ? PagedResultDtoOfGetEmployeeAdvancesForViewDto.fromJS(
                              resultData200
                          )
                        : new PagedResultDtoOfGetEmployeeAdvancesForViewDto();
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
        return _observableOf<PagedResultDtoOfGetEmployeeAdvancesForViewDto>(
            <any>null
        );
    }

    /**
     * @param id (optional)
     * @return Success
     */
    getEmployeeAdvancesForView(
        id: number | null | undefined
    ): Observable<GetEmployeeAdvancesForViewDto> {
        let url_ =
            this.baseUrl +
            "/api/services/app/EmployeeAdvances/GetEmployeeAdvancesForView?";
        if (id !== undefined) url_ += "id=" + encodeURIComponent("" + id) + "&";
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
                    return this.processGetEmployeeAdvancesForView(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processGetEmployeeAdvancesForView(
                                <any>response_
                            );
                        } catch (e) {
                            return <Observable<GetEmployeeAdvancesForViewDto>>(
                                (<any>_observableThrow(e))
                            );
                        }
                    } else
                        return <Observable<GetEmployeeAdvancesForViewDto>>(
                            (<any>_observableThrow(response_))
                        );
                })
            );
    }

    protected processGetEmployeeAdvancesForView(
        response: HttpResponseBase
    ): Observable<GetEmployeeAdvancesForViewDto> {
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
                        ? GetEmployeeAdvancesForViewDto.fromJS(resultData200)
                        : new GetEmployeeAdvancesForViewDto();
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
        return _observableOf<GetEmployeeAdvancesForViewDto>(<any>null);
    }

    /**
     * @param id (optional)
     * @return Success
     */
    getEmployeeAdvancesForEdit(
        id: number | null | undefined
    ): Observable<GetEmployeeAdvancesForEditOutput> {
        let url_ =
            this.baseUrl +
            "/api/services/app/EmployeeAdvances/GetEmployeeAdvancesForEdit?";
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
                    return this.processGetEmployeeAdvancesForEdit(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processGetEmployeeAdvancesForEdit(
                                <any>response_
                            );
                        } catch (e) {
                            return <
                                Observable<GetEmployeeAdvancesForEditOutput>
                            >(<any>_observableThrow(e));
                        }
                    } else
                        return <Observable<GetEmployeeAdvancesForEditOutput>>(
                            (<any>_observableThrow(response_))
                        );
                })
            );
    }

    protected processGetEmployeeAdvancesForEdit(
        response: HttpResponseBase
    ): Observable<GetEmployeeAdvancesForEditOutput> {
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
                        ? GetEmployeeAdvancesForEditOutput.fromJS(resultData200)
                        : new GetEmployeeAdvancesForEditOutput();
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
        return _observableOf<GetEmployeeAdvancesForEditOutput>(<any>null);
    }

    /**
     * @param input (optional)
     * @return Success
     */
    createOrEdit(
        input: CreateOrEditEmployeeAdvancesDto | null | undefined
    ): Observable<void> {
        let url_ =
            this.baseUrl + "/api/services/app/EmployeeAdvances/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/EmployeeAdvances/Delete?";
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

    /**
     * @param filter (optional)
     * @param maxDeductionIDFilter (optional)
     * @param minDeductionIDFilter (optional)
     * @param maxEmployeeIDFilter (optional)
     * @param minEmployeeIDFilter (optional)
     * @param employeeNameFilter (optional)
     * @param maxSalaryYearFilter (optional)
     * @param minSalaryYearFilter (optional)
     * @param maxSalaryMonthFilter (optional)
     * @param minSalaryMonthFilter (optional)
     * @param maxDeductionDateFilter (optional)
     * @param minDeductionDateFilter (optional)
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
    getEmployeeAdvancesToExcel(
        filter: string | null | undefined
    ): Observable<FileDto> {
        let url_ =
            this.baseUrl +
            "/api/services/app/EmployeeAdvances/GetEmployeeAdvancesToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";

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
                    return this.processGetEmployeeAdvancesToExcel(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processGetEmployeeAdvancesToExcel(
                                <any>response_
                            );
                        } catch (e) {
                            return <Observable<FileDto>>(
                                (<any>_observableThrow(e))
                            );
                        }
                    } else
                        return <Observable<FileDto>>(
                            (<any>_observableThrow(response_))
                        );
                })
            );
    }

    protected processGetEmployeeAdvancesToExcel(
        response: HttpResponseBase
    ): Observable<FileDto> {
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
                        ? FileDto.fromJS(resultData200)
                        : new FileDto();
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
        return _observableOf<FileDto>(<any>null);
    }

    getMaxAdvanceId(): Observable<number> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/EmployeeAdvances/GetMaxID";
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
                    return this.processGetMaxDeductionId(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processGetMaxDeductionId(
                                <any>response_
                            );
                        } catch (e) {
                            return <Observable<number>>(
                                (<any>_observableThrow(e))
                            );
                        }
                    } else
                        return <Observable<number>>(
                            (<any>_observableThrow(response_))
                        );
                })
            );
    }

    protected processGetMaxDeductionId(
        response: HttpResponseBase
    ): Observable<number> {
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
                    result200 =
                        resultData200 !== undefined ? resultData200 : <any>null;
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
        return _observableOf<number>(<any>null);
    }

    isValidAdvanceAmount(amount: number, empId: number) {
        debugger;
        let url_ =
            this.baseUrl +
            "/api/services/app/EmployeeAdvances/GetIsValidAmount?";
        if (amount !== undefined)
            url_ += "advanceAmount=" + encodeURIComponent("" + amount) + "&";
        if (empId !== undefined)
            url_ += "EmpID=" + encodeURIComponent("" + empId) + "&";
        url_ = url_.replace(/[?&]$/, "");

       return this.http.get(url_);
    }
}
