import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PagedResultDtoOfGLINVDetailDto, GLINVDetailDto } from '../dto/glinvDetails-dto';

@Injectable({
    providedIn: 'root'
  })
export class GLINVDetailsService {
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
    getGLINVDData(detId: number | null | undefined): Observable<PagedResultDtoOfGLINVDetailDto> {
        let url_ = this.baseUrl + "/api/services/app/GLINVDetails/GetGLINVDData?";
        if (detId !== undefined)
            url_ += "detId=" + encodeURIComponent("" + detId) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetGLINVDData(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetGLINVDData(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfGLINVDetailDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGLINVDetailDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetGLINVDData(response: HttpResponseBase): Observable<PagedResultDtoOfGLINVDetailDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfGLINVDetailDto.fromJS(resultData200) : new PagedResultDtoOfGLINVDetailDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGLINVDetailDto>(<any>null);
    }
}