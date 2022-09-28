import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, blobToText, FileDto, throwException } from "@shared/service-proxies/service-proxies";
import {   GLSLGroupsDto } from '../dto/glslgroup-dto';
import * as moment from 'moment';

@Injectable({
    providedIn: 'root'
})
export class GLSlGroupServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    url:string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;


    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {

        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    getAll(
        filter: string,
        sorting: string | null | undefined,
        skipCount: number | null | undefined,
        maxResultCount: number | null | undefined) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/glslgroups/GetAll?";
        this.url += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (sorting !== undefined)
          this.url += "Sorting=" + encodeURIComponent("" + sorting) + "&";
        if (skipCount !== undefined)
          this.url += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        if (maxResultCount !== undefined)
          this.url += "MaxResultCount=" + encodeURIComponent("" + maxResultCount);
        let url_ = this.url.replace(/[?&]$/, "");
        return this.http.request("get", url_);
      }
      delete(id: number) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/glslgroups/Delete?Id=" + id;
        return this.http.delete(this.url);
      }
    
      create(dto: GLSLGroupsDto) {
       
        this.url = this.baseUrl;
        this.url += "/api/services/app/glslgroups/CreateOrEdit";
        return this.http.post(this.url, dto);
      }
    
      getDataForEdit(id: number) {
        this.url = this.baseUrl;
        this.url += "/api/services/app/glslgroups/GetGLSLGroupsForEdit?Id=" + id;
        return this.http.get(this.url);
      }
      getMaxDocId(){
        this.url = this.baseUrl;
        this.url += "/api/services/app/glslgroups/getMaxDocId";
        return this.http.get(this.url);
      }
}