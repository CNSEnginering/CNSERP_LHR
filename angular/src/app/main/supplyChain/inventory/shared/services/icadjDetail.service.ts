import {
    mergeMap as _observableMergeMap,
    catchError as _observableCatch
} from "rxjs/operators";
import {
    Observable,
    throwError as _observableThrow,
    of as _observableOf
} from "rxjs";
import { Injectable, Optional, Inject } from "@angular/core";
import {
    HttpClient,
    HttpHeaders,
    HttpResponse,
    HttpResponseBase
} from "@angular/common/http";
import {
    API_BASE_URL,
    FileDto,
    blobToText,
    throwException
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PagedResultDtoOfICADJDetailDto } from "../dto/icadjDetails-dto";

@Injectable({
    providedIn: "root"
})
export class ICADJDetailsService {
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

    getConsumptionData(docNo: number | null | undefined) {
        let url_ =
            this.baseUrl + "/api/services/app/ICADJDetails/GetConsumptionData?";
        if (docNo !== undefined)
            url_ += "docNo=" + encodeURIComponent("" + docNo) + "&";
        url_ = url_.replace(/[?&]$/, "");

        return this.http.get(url_);
    }

    /**
     * @param detId
     * @return Success
     */
    getICADJDData(
        detId: number | null | undefined
    ): Observable<PagedResultDtoOfICADJDetailDto> {
        let url_ =
            this.baseUrl + "/api/services/app/ICADJDetails/GetICADJDData?";
        if (detId !== undefined)
            url_ += "detId=" + encodeURIComponent("" + detId) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                Accept: "application/json"
            })
        };

        return this.http
            .request("get", url_, options_)
            .pipe(
                _observableMergeMap((response_: any) => {
                    return this.processGetICADJDData(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processGetICADJDData(<any>response_);
                        } catch (e) {
                            return <Observable<PagedResultDtoOfICADJDetailDto>>(
                                (<any>_observableThrow(e))
                            );
                        }
                    } else
                        return <Observable<PagedResultDtoOfICADJDetailDto>>(
                            (<any>_observableThrow(response_))
                        );
                })
            );
    }

    protected processGetICADJDData(
        response: HttpResponseBase
    ): Observable<PagedResultDtoOfICADJDetailDto> {
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
                _observableMergeMap(_responseText => {
                    let result200: any = null;
                    let resultData200 =
                        _responseText === ""
                            ? null
                            : JSON.parse(_responseText, this.jsonParseReviver);
                    result200 = resultData200
                        ? PagedResultDtoOfICADJDetailDto.fromJS(resultData200)
                        : new PagedResultDtoOfICADJDetailDto();
                    return _observableOf(result200);
                })
            );
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(
                _observableMergeMap(_responseText => {
                    return throwException(
                        "An unexpected server error occurred.",
                        status,
                        _responseText,
                        _headers
                    );
                })
            );
        }
        return _observableOf<PagedResultDtoOfICADJDetailDto>(<any>null);
    }
}
