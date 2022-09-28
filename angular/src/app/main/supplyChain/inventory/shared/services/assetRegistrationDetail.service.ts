import { Injectable, Inject, Optional } from "@angular/core";
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { HttpClient, HttpHeaders, HttpResponseBase, HttpResponse } from "@angular/common/http";
import { API_BASE_URL, blobToText, throwException, FileDto } from "@shared/service-proxies/service-proxies";
import * as  moment from "moment";


@Injectable()
export class AssetRegistrationDetailServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

  
  

    createOrEdit(input: CreateOrEditAssetRegistrationDetailDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/AssetRegistrationDetial/CreateOrEdit";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(input);

        let options_: any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json",
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processCreateOrEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processCreateOrEdit(<any>response_);
                } catch (e) {
                    return <Observable<void>><any>_observableThrow(e);
                }
            } else
                return <Observable<void>><any>_observableThrow(response_);
        }));
    }

    protected processCreateOrEdit(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
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
     * @param id (optional) 
     * @return Success
     */
    delete(id: number | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/AssetRegistration/Delete?";
        if (id !== undefined)
            url_ += "Id=" + encodeURIComponent("" + id) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
            })
        };

        return this.http.request("delete", url_, options_).pipe(_observableMergeMap((response_: any) => {
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

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
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

}


export class CreateOrEditAssetRegistrationDetailDto implements ICreateOrEditAssetRegistrationDetailDto {
    assetID!: number | undefined;
    depStartDate!: moment.Moment | undefined;
    depMethod!: number | undefined;
    assetLife!: number | undefined;
    bookValue!: number | undefined;
    lastDepAmount!: number | undefined;
    lastDepDate!: moment.Moment | undefined;
    accumulatedDepAmt!: number | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditAssetRegistrationDetailDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.assetID = data["assetID"];
            this.depStartDate = data["depStartDate"] ? moment(data["depStartDate"].toString()) : <any>undefined;
            this.depMethod = data["depMethod"];
            this.assetLife = data["assetLife"];
            this.bookValue = data["bookValue"];
            this.lastDepAmount = data["lastDepAmount"] ;
            this.lastDepDate = data["lastDepDate"] ? moment(data["lastDepDate"].toString()) : <any>undefined;
            this.accumulatedDepAmt = data["accumulatedDepAmt"] ;
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditAssetRegistrationDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditAssetRegistrationDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["assetID"] = this.assetID;
        data["depStartDate"] = this.depStartDate ? this.depStartDate.toISOString() : <any>undefined;
        data["depMethod"] = this.depMethod;
        data["assetLife"] = this.assetLife;
        data["bookValue"] = this.bookValue;
        data["lastDepAmount"] = this.lastDepAmount;
        data["lastDepDate"] = this.lastDepDate ? this.lastDepDate.toISOString() : <any>undefined;
        data["accumulatedDepAmt"] = this.accumulatedDepAmt;
        data["id"] = this.id;
        return data;
    }
}

export interface ICreateOrEditAssetRegistrationDetailDto {
    assetID: number | undefined;
    depStartDate: moment.Moment | undefined;
    depMethod: number | undefined;
    assetLife: number | undefined;
    bookValue: number | undefined;
    lastDepAmount: number | undefined;
    lastDepDate: moment.Moment | undefined;
    accumulatedDepAmt: number | undefined;
    id: number | undefined;
}