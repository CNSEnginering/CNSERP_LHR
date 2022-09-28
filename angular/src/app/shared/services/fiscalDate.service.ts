import { Injectable, Inject, Optional, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';

@Injectable({
    providedIn: 'root'
})
export class FiscalDateService implements OnInit {

    private http: HttpClient;
    private baseUrl: string;
    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    ngOnInit(): void {

    }

    getDate() {
        let url_ = this.baseUrl + "/api/services/app/FiscalCalenders/GetFiscalDate";
        url_ = url_.replace(/[?&]$/, "");
        return this.http.get(url_);
    }
}
