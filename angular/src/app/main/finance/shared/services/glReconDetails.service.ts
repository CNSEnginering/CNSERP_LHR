import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PagedResultDtoOfGLReconDetailsDto, listResultDtoOfBankReconcileDetail } from '../dto/glReconDetails-dto';

@Injectable({
    providedIn: 'root'
})
export class GLReconDetailsService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param detId
     * @return Success
     */
    getGLReconDData(detId: number | null | undefined): Observable<PagedResultDtoOfGLReconDetailsDto> {
        let url_ = this.baseUrl + "/api/services/app/ICCNSDetails/GetICCNSDData?";
        if (detId !== undefined)
            url_ += "detId=" + encodeURIComponent("" + detId) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetGLReconDData(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetGLReconDData(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfGLReconDetailsDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGLReconDetailsDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetGLReconDData(response: HttpResponseBase): Observable<PagedResultDtoOfGLReconDetailsDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGLReconDetailsDto.fromJS(resultData200) : new PagedResultDtoOfGLReconDetailsDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGLReconDetailsDto>(<any>null);
    }

    GetListOFdetail(bank: string | undefined, date: string | undefined): Observable<listResultDtoOfBankReconcileDetail> {
        debugger
        let url_ = this.baseUrl + "/api/services/app/BankReconcileDetails/GetListOFdetail?";
        if (bank !== undefined)
            url_ += "bank=" + encodeURIComponent("" + bank) + "&";
        if (date !== undefined)
            url_ += "date=" + encodeURIComponent("" + date) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetListOFdetail(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetListOFdetail(<any>response_);
                } catch (e) {
                    return <Observable<listResultDtoOfBankReconcileDetail>><any>_observableThrow(e);
                }
            } else
                return <Observable<listResultDtoOfBankReconcileDetail>><any>_observableThrow(response_);
        }));
    }

    protected processGetListOFdetail(response: HttpResponseBase): Observable<listResultDtoOfBankReconcileDetail> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? listResultDtoOfBankReconcileDetail.fromJS(resultData200) : new listResultDtoOfBankReconcileDetail();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<listResultDtoOfBankReconcileDetail>(<any>null);
    }
}