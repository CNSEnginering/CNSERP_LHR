import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import { VoucherPostingDto } from '../dto/voucher-posting-dto';


@Injectable({
    providedIn: 'root'
})

export class ApprovalService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }


    getData(
        options: string,
        fromDate: any,
        toDate: any,
        Mode: string,
        fromDoc: number,
        ToDoc: number,
        skipCount: number,
        maxResultCount: number) {
        let url_ = "";
        switch (options) {
            case "opening":
                url_ = this.baseUrl + "/api/services/app/Opening/GetDetailsForApproval?fromDate=";
                break;
            case "gatePass":
                url_ = this.baseUrl + "/api/services/app/GatePasses/GetDetailsForApproval?fromDate=";
                break;
            case "transfer":
                url_ = this.baseUrl + "/api/services/app/Transfers/GetDetailsForApproval?fromDate=";
                break;
            case "adjustment":
                url_ = this.baseUrl + "/api/services/app/Adjustment/GetDetailsForApproval?fromDate=";
                break;
            case "consumption":
                url_ = this.baseUrl + "/api/services/app/Consumption/GetDetailsForApproval?fromDate=";
                break;
            case "assembly":
                url_ = this.baseUrl + "/api/services/app/Assemblies/GetDetailsForApproval?fromDate=";
                break;
            case "requisitions":
                url_ = this.baseUrl + "/api/services/app/Requisitions/GetDetailsForApproval?fromDate=";
                break;
            case "purchase":
                url_ = this.baseUrl + "/api/services/app/PurchaseOrder/GetDetailsForApproval?fromDate=";
                break;
            case "receipt":
                url_ = this.baseUrl + "/api/services/app/ReceiptEntry/GetDetailsForApproval?fromDate=";
                break;
            case "receiptReturn":
                url_ = this.baseUrl + "/api/services/app/ReceiptReturn/GetDetailsForApproval?fromDate=";
                break;
        }

        url_ = url_ + fromDate + "&toDate="
            + toDate + "&mode="
            + Mode + "&fromDoc="
            + fromDoc + "&toDoc="
            + ToDoc + "&skipCount="
            + skipCount + "&maxResultCount="
            + maxResultCount;
        url_ = url_.replace(/[?&]$/, "");
        return this.http.get(url_);
    }

    ApprovalData(options: string,
        postedData: number[] | null | undefined,
        mode: string | null | undefined,
        bit: boolean | null | undefined) {
            debugger
        let url_ = "";
        switch (options) {
            case "opening":
                url_ = this.baseUrl + "/api/services/app/Opening/ApprovalData?";
                break;
            case "gatePass":
                url_ = this.baseUrl + "/api/services/app/GatePasses/ApprovalData?";
                break;
            case "transfer":
                url_ = this.baseUrl + "/api/services/app/Transfers/ApprovalData?";
                break;
            case "adjustment":
                url_ = this.baseUrl + "/api/services/app/Adjustment/ApprovalData?";
                break;
            case "workOrder":
                url_ = this.baseUrl + "/api/services/app/WorkOrder/ApprovalData?";
                break;
            case "consumption":
                url_ = this.baseUrl + "/api/services/app/Consumption/ApprovalData?";
                break;
            case "assembly":
                url_ = this.baseUrl + "/api/services/app/Assemblies/ApprovalData?";
                break;
            case "requisitions":
                url_ = this.baseUrl + "/api/services/app/Requisitions/ApprovalData?";
                break;
            case "purchase":
                url_ = this.baseUrl + "/api/services/app/PurchaseOrder/ApprovalData?";
                break;
            case "receipt":
                url_ = this.baseUrl + "/api/services/app/ReceiptEntry/ApprovalData?";
                break;
            case "receiptReturn":
                url_ = this.baseUrl + "/api/services/app/ReceiptReturn/ApprovalData?";
                break;
            case "SaleQuotation":
                url_ = this.baseUrl + "/api/services/app/oeqh/ApprovalData?";
                break;
            case "saleEntry":
                url_ = this.baseUrl + "/api/services/app/SaleEntry/ApprovalData?";
                break;
            case "SaleReturn":
                    url_=this.baseUrl+"/api/services/app/SaleReturn/ApprovalData?";
                break;
            case "Apinv":
                url_ = this.baseUrl + "/api/services/app/apinvh/ApprovalData?";
        }


        if (mode !== undefined)
            url_ += "Mode=" + encodeURIComponent("" + mode) + "&";
        if (bit !== undefined)
            url_ += "bit=" + encodeURIComponent("" + bit) + "&";
        url_ = url_.replace(/[?&]$/, "");

        url_ = url_.replace(/[?&]$/, "");
        const content_ = JSON.stringify(postedData);

        let options_: any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json",
            })
        };

        return this.http.request("post", url_, options_);
    }

}