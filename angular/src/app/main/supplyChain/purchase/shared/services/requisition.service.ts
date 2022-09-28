import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponseBase, HttpResponse } from '@angular/common/http';
import { API_BASE_URL, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { RequisitionDto } from '../dtos/requisition-dto';


@Injectable({
  providedIn: 'root'
})
export class RequisitionService {
  url: string = "";
  url_: string = "";
  data: any;
  baseUrl: string = "";
  protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;
  constructor(private http: HttpClient, @Inject(API_BASE_URL) baseUrl?: string) {
    this.baseUrl = baseUrl;
  }

  getAll(
    filter: string | null | undefined,maxDocNoFilter: number | null | undefined,minDocNoFilter: number | null | undefined, maxLocIDFilter: number | null | undefined, minLocIDFilter: number | null | undefined,activeFilter: number | null | undefined,postedFilter: number | null | undefined, createdByFilter: string | null | undefined,audtUserFilter: string | null | undefined,  sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined) {
    debugger;
    this.url = this.baseUrl;
    this.url += "/api/services/app/Requisitions/GetAll?";
    if (filter !== undefined)
    this.url += "Filter=" + encodeURIComponent("" + filter) + "&"; 
    if (maxDocNoFilter !== undefined)
    this.url += "MaxDocNoFilter=" + encodeURIComponent("" + maxDocNoFilter) + "&"; 
    if (minDocNoFilter !== undefined)
    this.url += "MinDocNoFilter=" + encodeURIComponent("" + minDocNoFilter) + "&"; 
    if (maxLocIDFilter !== undefined)
    this.url += "MaxLocIDFilter=" + encodeURIComponent("" + maxLocIDFilter) + "&"; 
    if (minLocIDFilter !== undefined)
    this.url += "MinLocIDFilter=" + encodeURIComponent("" + minLocIDFilter) + "&"; 
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

  delete(id: number) {
    debugger
    this.url = this.baseUrl;
    this.url += "/api/services/app/Requisitions/Delete?Id=" + id;
    return this.http.delete(this.url);
  }

  create(dto: RequisitionDto) {
    debugger;
    this.url = this.baseUrl;
    this.url += "/api/services/app/Requisitions/CreateOrEdit";
    return this.http.post(this.url, dto);
  }

  getDataForEdit(id: number, type: string) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/Requisitions/GetRequisitionForEdit?Id=" + id;
    return this.http.get(this.url);
  }

  GetDataToExcel(
    filter: string, sorting: string | null | undefined,
    skipCount: number | null | undefined,
    maxResultCount: number | null | undefined
  ) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/Requisitions/GetDataToExcel?";
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

  GetQtyInHand(itemId: string, locId: number, docId: number) {
    this.url = this.baseUrl;
    this.url += "/api/services/app/Requisitions/GetQtyInHand?";
    this.url += "itemId=" + encodeURIComponent("" + itemId) + "&";
    this.url += "locId=" + encodeURIComponent("" + locId) + "&";
    this.url += "docId=" + encodeURIComponent("" + docId);
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }

  GetDocId() {
    this.url = this.baseUrl;
    this.url += "/api/services/app/Requisitions/GetDocId";
    this.url_ = this.url.replace(/[?&]$/, "");
    return this.http.request("get", this.url_);
  }
}
