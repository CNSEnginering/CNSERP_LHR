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
import { PagedResultDtoOfICCNSDetailDto } from "../dto/iccnsDetails-dto";

@Injectable({
    providedIn: "root",
})
export class ICCNSDetailsService {
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
     * @param orderNo
     * @return Success
     */
    getICWODData(
        orderNo: number | null | undefined,
        locId: number
    ): Observable<PagedResultDtoOfICCNSDetailDto> {
        let url_ =
            this.baseUrl + "/api/services/app/ICCNSDetails/GetICWODData?";
        if (orderNo !== undefined)
            url_ += "orderNo=" + encodeURIComponent("" + orderNo) + "&";
        if (locId !== undefined)
            url_ += "locId=" + encodeURIComponent("" + locId) + "&";
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
                    return this.processGetICCNSDData(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processGetICCNSDData(<any>response_);
                        } catch (e) {
                            return <Observable<PagedResultDtoOfICCNSDetailDto>>(
                                (<any>_observableThrow(e))
                            );
                        }
                    } else
                        return <Observable<PagedResultDtoOfICCNSDetailDto>>(
                            (<any>_observableThrow(response_))
                        );
                })
            );
    }

    /**
     * @param detId
     * @param ccId
     * @return Success
     */
    getICCNSDData(
        detId: number | null | undefined,
        ccId: string | null | undefined
    ): Observable<PagedResultDtoOfICCNSDetailDto> {
        let url_ =
            this.baseUrl + "/api/services/app/ICCNSDetails/GetICCNSDData?";
        if (detId !== undefined)
            url_ += "detId=" + encodeURIComponent("" + detId) + "&";
        if (ccId !== undefined)
            url_ += "ccId=" + encodeURIComponent("" + ccId) + "&";
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
                    return this.processGetICCNSDData(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processGetICCNSDData(<any>response_);
                        } catch (e) {
                            return <Observable<PagedResultDtoOfICCNSDetailDto>>(
                                (<any>_observableThrow(e))
                            );
                        }
                    } else
                        return <Observable<PagedResultDtoOfICCNSDetailDto>>(
                            (<any>_observableThrow(response_))
                        );
                })
            );
    }

    protected processGetICCNSDData(
        response: HttpResponseBase
    ): Observable<PagedResultDtoOfICCNSDetailDto> {
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
                        ? PagedResultDtoOfICCNSDetailDto.fromJS(resultData200)
                        : new PagedResultDtoOfICCNSDetailDto();
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
        return _observableOf<PagedResultDtoOfICCNSDetailDto>(<any>null);
    }
}
