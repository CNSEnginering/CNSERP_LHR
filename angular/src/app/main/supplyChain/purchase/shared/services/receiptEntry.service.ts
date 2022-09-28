import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { ReceiptEntryDto } from '../dtos/receiptEntry-dto';
import { PORECDetailsService } from './porecDetail.service';
import { PagedResultDtoOfPORECDetailDto } from '../dtos/porecDetails-dto';
import { PagedResultDtoOfPORECHeaderDto, PORECHeaderDto, CreateOrEditPORECHeaderDto } from '../dtos/porecHeader-dto';
import { PORECHeadersService } from './porecHeader.service';
import { Guid } from "guid-typescript";
@Injectable({
    providedIn: 'root'
  })
export class ReceiptEntryServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(
        private _porecHeaderServiceProxy: PORECHeadersService,
        private _porecDetailServiceProxy: PORECDetailsService,
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
    createOrEditReceiptEntry(input: ReceiptEntryDto | null | undefined): Observable<void> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/ReceiptEntry/CreateOrEditReceiptEntry";
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
            return this.processCreateOrEditReceiptEntry(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processCreateOrEditReceiptEntry(<any>response_);
                } catch (e) {
                    return <Observable<void>><any>_observableThrow(e);
                }
            } else
                return <Observable<void>><any>_observableThrow(response_);
        }));
    }

    protected processCreateOrEditReceiptEntry(response: HttpResponseBase): Observable<void> {
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
        let url_ = this.baseUrl + "/api/services/app/ReceiptEntry/GetMaxDocId";
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
        let url_ = this.baseUrl + "/api/services/app/ReceiptEntry/Delete?";
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
     * @param poNo
     * @return Success
     */
    getPONoHeaderRecord(locID: number | null | undefined,poNo: number | null | undefined): Observable<PORECHeaderDto> {
        let url_ = this.baseUrl + "/api/services/app/PORECHeaders/GetPONoHeaderData?";
        if (locID !== undefined)
            url_ += "locID=" + encodeURIComponent("" + locID) + "&"; 
        if (poNo !== undefined)
            url_ += "poNo=" + encodeURIComponent("" + poNo) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this._porecHeaderServiceProxy.processGetPORECHeaderForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this._porecHeaderServiceProxy.processGetPORECHeaderForEdit(<any>response_);
                } catch (e) {
                    return <Observable<PORECHeaderDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PORECHeaderDto>><any>_observableThrow(response_);
        }));
    }

     /**
     * @param poNo
     * @return Success
     */
    getPONoRecords(locID: number | null | undefined,poNo: number | null | undefined): Observable<PagedResultDtoOfPORECDetailDto> {
        let url_ = this.baseUrl + "/api/services/app/ReceiptEntry/GetPONoData?";
        if (poNo !== undefined)
            url_ += "poNo=" + encodeURIComponent("" + poNo) + "&"; 
        if (locID !== undefined)
            url_ += "locID=" + encodeURIComponent("" + locID) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this._porecDetailServiceProxy.processGetPORECDData(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this._porecDetailServiceProxy.processGetPORECDData(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfPORECDetailDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfPORECDetailDto>><any>_observableThrow(response_);
        }));
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    processReceiptEntry(input: CreateOrEditPORECHeaderDto | null | undefined): Observable<string> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/ReceiptEntry/ProcessReceiptEntry";
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
            return this.processPReceiptEntry(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processPReceiptEntry(<any>response_);
                } catch (e) {
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else
                return <Observable<string>><any>_observableThrow(response_);
        }));
    }


     downloadFile(id: Guid, contentType: string,fileName:string) {
        debugger
        let url = this.baseUrl;
        url += '/File/DownloadBinaryFile?id=' + id + '&contentType=' + contentType + '&fileName=' + fileName;
       
        url = url.replace(/[?&]$/, "");
        console.log(url)
        return this.http.get(url);
    }

    deleteFile(appID: number, docID: number) {
        let url = this.baseUrl;
        url += "/DemoUiComponents/DeleteFile?";
        if (appID !== undefined)
            url += "AppID=" + encodeURIComponent("" + appID) + "&";
        if (docID !== undefined)
            url += "DocID=" + encodeURIComponent("" + docID) + "&";
        url = url.replace(/[?&]$/, "");
        return this.http.get(url);
    }

    protected processPReceiptEntry(response: HttpResponseBase): Observable<string> {
        debugger;
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
        return _observableOf<string>(<any>null);
    }
}