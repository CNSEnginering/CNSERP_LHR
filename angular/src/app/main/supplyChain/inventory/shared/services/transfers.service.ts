import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponseBase, HttpResponse } from '@angular/common/http';
import { API_BASE_URL, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { GatePassHeaderDto } from '../dto/gatePassHeader-dto';
import { stringify } from 'querystring';
import { TransferDto } from '../dto/transfer-dto';

@Injectable({
  providedIn: 'root'
})
export class TransfersService {
  url: string = "";
  url_: string = "";
  data: any;
  baseUrl: string = "";
  protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;
  constructor(private http: HttpClient, @Inject(API_BASE_URL) baseUrl?: string) {
    this.baseUrl = baseUrl;
  }

  getAll(
    filter: string | null | undefined,maxDocNoFilter: number | null | undefined,minDocNoFilter: number | null | undefined,activeFilter: number | null | undefined,postedFilter: number | null | undefined, createdByFilter: string | null | undefined,audtUserFilter: string | null | undefined,  sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined) {
    debugger;
    this.url = this.baseUrl;
    this.url += "/api/services/app/Transfers/GetAll?";
    if (filter !== undefined)
    this.url += "Filter=" + encodeURIComponent("" + filter) + "&"; 
    if (maxDocNoFilter !== undefined)
    this.url += "MaxDocNoFilter=" + encodeURIComponent("" + maxDocNoFilter) + "&"; 
    if (minDocNoFilter !== undefined)
    this.url += "MinDocNoFilter=" + encodeURIComponent("" + minDocNoFilter) + "&";
    if (activeFilter !== undefined)
    this.url += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&"; 
    if (postedFilter !== undefined)
    this.url += "PostedFilter=" + encodeURIComponent("" + postedFilter) + "&";
    if (createdByFilter !== undefined)
    this.url += "CreatedByFilter=" + encodeURIComponent("" + createdByFilter) + "&";
    if (audtUserFilter !== undefined)
    this.url += "AudtUserFilter=" + encodeURIComponent("" + audtUserFilter) + "&";
    if (sorting !== undefined)
    this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&";
    if (skipCount !== undefined)
    this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&"; 
    if (maxResultCount !== undefined)
    this.url += "MaxResultCount=" + encodeURIComponent("" + maxResultCount) + "&"; 
    this.url = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url);
  }

  checkCostCenterIdIfExists(ccId: string) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/GatePasses/GetcheckCostCenterIdIfExists?ccId=" + ccId;
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  delete(id: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/Transfers/Delete?Id=" + id;
    return this.http.delete(this.url);
  }

  create(dto: TransferDto) {
    debugger;
    this.url = this.baseUrl;
    this.url += "/api/services/app/Transfers/CreateOrEdit";
    return this.http.post(this.url, dto);
  }

  getDataForEdit(id: number, type: string) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/Transfers/GetTransferForEdit?Id=" + id;
    // this.url += "&type=" + type;
    debugger;
    return this.http.get(this.url);
  }

  GetOGPNo(
    filter: string,
    sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/GatePasses/GetOGPNo?";
    this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
    if (sorting !== undefined)
      this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&";
    if (skipCount !== undefined)
      this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
    if (maxResultCount !== undefined)
      this.url += "MaxResultCount=" + encodeURIComponent("" + maxResultCount) + "&";

    this.url += "MaxGPTypeFilter=" + encodeURIComponent("" + 1) + "&";
    this.url += "MaxTypeIDFilter=" + encodeURIComponent("" + 2);
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  GetDataToExcel(
    filter: string, sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined
  ) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/Transfers/GetDataToExcel?";
    this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
    if (sorting !== undefined)
      this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&";
    if (skipCount !== undefined)
      this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
    if (maxResultCount !== undefined)
      this.url += "MaxResultCount=" + encodeURIComponent("" + maxResultCount);
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }
  GetQtyInHand(locId: number, itemId: string, Docdate:any) {
    debugger
    let url_ =
        this.baseUrl +
        "/api/services/app/ICCNSDetails/GetQtyInHand?locId=" +
        locId +
        "&itemId=" +
        itemId +"&docDate=" +Docdate;

   // if (docId != undefined) url_ = url_ + "&docId=" + docId;

    url_ = url_.replace(/[?&]$/, "");

    return this.http.get(url_);
}

  // GetQtyInHand(itemId: string, locId: number, docId: number) {
  //   this.url = this.baseUrl;
  //   this.url += "/api/services/app/Transfers/GetQtyInHand?";
  //   this.url += "itemId=" + encodeURIComponent("" + itemId) + "&";
  //   this.url += "locId=" + encodeURIComponent("" + locId) + "&";
  //   this.url += "docId=" + encodeURIComponent("" + docId);
  //   this.url_ = this.url.replace(/[?&]$/, "");
  //   return this.http.request("get", this.url_);
  // }

  GetDocId() {
    this.url = this.baseUrl;
    this.url += "/api/services/app/Transfers/GetDocId";
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }
  processTransfer(dto: TransferDto | null | undefined): Observable<string>{
    debugger;
    let url_ = this.baseUrl + "/api/services/app/Transfers/ProcessTransfer";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(dto);

        let options_: any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json"
            })
        };

    return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
        return this.processPTransfer(response_);
    })).pipe(_observableCatch((response_: any) => {
        debugger;
        if (response_ instanceof HttpResponseBase) {
            try {
                return this.processPTransfer(<any>response_);
            } catch (e) {
                return <Observable<string>><any>_observableThrow(e);
            }
        } else
            return <Observable<string>><any>_observableThrow(response_);
    }));
}
protected processPTransfer(response: HttpResponseBase): Observable<string> {
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
