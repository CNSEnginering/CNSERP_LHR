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
    blobToText,
    throwException
} from "@shared/service-proxies/service-proxies";
import { VoucherPostingDto } from "../dto/voucher-posting-dto";
import * as moment from "moment";

@Injectable({
    providedIn: "root"
})
export class VoucherPostingServiceProxy {
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
     * @param input (optional)
     * @return Success
     */
    processVoucherPosting(
        input: VoucherPostingDto | null | undefined
    ): Observable<string> {
        debugger;
        let url_ =
            this.baseUrl +
            "/api/services/app/VoucherPosting/ProcessVoucherPosting";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(input);

        let options_: any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json"
            })
        };

        return this.http
            .request("post", url_, options_)
            .pipe(
                _observableMergeMap((response_: any) => {
                    return this.processPVoucherPosting(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processPVoucherPosting(<any>response_);
                        } catch (e) {
                            return <Observable<string>>(
                                (<any>_observableThrow(e))
                            );
                        }
                    } else
                        return <Observable<string>>(
                            (<any>_observableThrow(response_))
                        );
                })
            );
    }

    protected processPVoucherPosting(
        response: HttpResponseBase
    ): Observable<string> {
        debugger;
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
                    result200 =
                        resultData200 !== undefined ? resultData200 : <any>null;
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
        return _observableOf<string>(<any>null);
    }

    public getLastPostedDate(type: string) {
        let url_ =
            this.baseUrl +
            "/api/services/app/VoucherPosting/GetLastPostedDate?type=" +
            type;
        url_ = url_.replace(/[?&]$/, "");
        return this.http.get(url_);
    }

    public getVouchersData(
        fromDoc: number,
        toDoc: number,
        type: string,
        fromDate: string,
        toDate: string
    ) {
        debugger;
        let url_ =
            this.baseUrl + "/api/services/app/VoucherPosting/GetVouchersData?";
        if (fromDoc !== undefined)
            url_ += "fromDoc=" + encodeURIComponent("" + fromDoc) + "&";
        if (toDoc !== undefined)
            url_ += "toDoc=" + encodeURIComponent("" + toDoc) + "&";
        if (type !== undefined)
            url_ += "type=" + encodeURIComponent("" + type) + "&";
        if (fromDate !== undefined) url_ += "frmDate=" + ("" + fromDate) + "&";
        if (toDate !== undefined) url_ += "tDate=" + ("" + toDate) + "&";

        url_ = url_.replace(/[?&]$/, "");
        return this.http.get(url_);
    }

    processData(
        type: string,
        postedDate: string | null | undefined,
        postedData: number[] | null | undefined
    ) {
        debugger;
        let url_ =
            this.baseUrl +
            "/api/services/app/VoucherPosting/ProcessVouchersData?";
        // switch (options) {
        //     case "creditNote":
        //         url_ = this.baseUrl + "/api/services/app/VoucherPosting/CreditNote?";
        //         break;
        //     case "debitNote":
        //         url_ = this.baseUrl + "/api/services/app/VoucherPosting/DebitNote?";
        //         break;
        //     case "assemblies":
        //         url_ = this.baseUrl + "/api/services/app/VoucherPosting/ProcessVoucherAssemblies?";
        //         break;
        // }

        if (type !== undefined)
            url_ += "type=" + encodeURIComponent("" + type) + "&";
        if (postedDate !== undefined)
            url_ += "postDate=" + ("" + postedDate) + "&";

        url_ = url_.replace(/[?&]$/, "");
        const content_ = JSON.stringify(postedData);

        let options_: any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json"
            })
        };

        return this.http.request("post", url_, options_);
    }
}
