import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
@Injectable({
    providedIn: 'root'
  })
  export class LogService {
    private http: HttpClient;
    private baseUrl: string;
    url:string;
    url_:string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param target (optional)
     * @return Success
     */
    getAll(
    FormName: string,
        DocNo: number | null | undefined,
        ) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/common/FormLog?";
        this.url += "FormName=" + encodeURIComponent("" + FormName) + "&";
        this.url += "DocNo=" + encodeURIComponent("" + DocNo) + "&";
        this.url_ = this.url.replace(/[?&]$/, "");
        return this.http.request("get", this.url_);
      }
   

}