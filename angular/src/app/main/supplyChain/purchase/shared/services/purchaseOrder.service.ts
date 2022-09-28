import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { PurchaseOrderDto } from '../dtos/purchaseOrder-dto';
import { POPOHeaderDto } from '../dtos/popoHeader-dto';
import { POPOHeadersService } from './popoHeader.service';
import { POPODetailsService } from './popoDetail.service';
import { POPODetailDto, PagedResultDtoOfPOPODetailDto } from '../dtos/popoDetails-dto';

@Injectable({
    providedIn: 'root'
  })
export class PurchaseOrderServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(
        private _popoHeadersServiceProxy: POPOHeadersService,
        private _popoDetailServiceProxy: POPODetailsService,
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string,
    ) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEditPurchaseOrder(input: PurchaseOrderDto | null | undefined): Observable<void> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/PurchaseOrder/CreateOrEditPurchaseOrder";
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
            return this.processCreateOrEditPurchaseOrder(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processCreateOrEditPurchaseOrder(<any>response_);
                } catch (e) {
                    return <Observable<void>><any>_observableThrow(e);
                }
            } else
                return <Observable<void>><any>_observableThrow(response_);
        }));
    }

    protected processCreateOrEditPurchaseOrder(response: HttpResponseBase): Observable<void> {
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
     * @param typeId (optional) 
     * @return Success
     */
    getMaxDocId(): Observable<number> {
        let url_ = this.baseUrl + "/api/services/app/PurchaseOrder/GetMaxDocId";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetMaxDocId(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetMaxDocId(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processGetMaxDocId(response: HttpResponseBase): Observable<number> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 !== undefined ? resultData200 : <any>null;
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<number>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    delete(id: number | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/PurchaseOrder/Delete?";
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
     * @param reqNo (optional) 
     * @return Success
     */
    pendingReqEntries(reqNo: number | null | undefined): Observable<POPOHeaderDto> {
        let url_ = this.baseUrl + "/api/services/app/PurchaseOrder/PendingReqEntries?";
        if (reqNo !== undefined)
            url_ += "reqNo=" + encodeURIComponent("" + reqNo) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this._popoHeadersServiceProxy.processGetICOPNHeaderForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this._popoHeadersServiceProxy.processGetICOPNHeaderForEdit(<any>response_);
                } catch (e) {
                    return <Observable<POPOHeaderDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<POPOHeaderDto>><any>_observableThrow(response_);
        }));
    }

    /**
     * @param reqNo
     * @return Success
     */
    pendingReqQty(reqNo: number | null | undefined): Observable<PagedResultDtoOfPOPODetailDto> {
        let url_ = this.baseUrl + "/api/services/app/PurchaseOrder/PendingReqQty?";
        if (reqNo !== undefined)
            url_ += "reqNo=" + encodeURIComponent("" + reqNo) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this._popoDetailServiceProxy.processGetPOPODData(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this._popoDetailServiceProxy.processGetPOPODData(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfPOPODetailDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfPOPODetailDto>><any>_observableThrow(response_);
        }));
    }

     /**
     * @param input (optional) 
     * @return Success
     */
    // processOpening(input: OpeningDto | null | undefined): Observable<string> {
    //     let url_ = this.baseUrl + "/api/services/app/Opening/ProcessOpening";
    //     url_ = url_.replace(/[?&]$/, "");

    //     const content_ = JSON.stringify(input);

    //     let options_ : any = {
    //         body: content_,
    //         observe: "response",
    //         responseType: "blob",
    //         headers: new HttpHeaders({
    //             "Content-Type": "application/json", 
    //         })
    //     };

    //     return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
    //         return this.processPOpening(response_);
    //     })).pipe(_observableCatch((response_: any) => {
    //         if (response_ instanceof HttpResponseBase) {
    //             try {
    //                 return this.processPOpening(<any>response_);
    //             } catch (e) {
    //                 return <Observable<string>><any>_observableThrow(e);
    //             }
    //         } else
    //             return <Observable<string>><any>_observableThrow(response_);
    //     }));
    // }

    // protected processPOpening(response: HttpResponseBase): Observable<string> {
    //     debugger;
    //     const status = response.status;
    //     const responseBlob = 
    //         response instanceof HttpResponse ? response.body : 
    //         (<any>response).error instanceof Blob ? (<any>response).error : undefined;

    //     let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
    //     if (status === 200) {
    //         return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
    //         let result200: any = null;
    //         let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
    //         result200 = resultData200 !== undefined ? resultData200 : <any>null;
    //         return _observableOf(result200);
    //         }));
    //     } else if (status !== 200 && status !== 204) {
    //         return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
    //         return throwException("An unexpected server error occurred.", status, _responseText, _headers);
    //         }));
    //     }
    //     return _observableOf<string>(<any>null);
    // }
}