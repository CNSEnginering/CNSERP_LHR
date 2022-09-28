import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import * as moment from 'moment';
import { blobToText, throwException, API_BASE_URL, PagedResultDtoOfNameValueDto } from '@shared/service-proxies/service-proxies';

@Injectable()
export class IcSegment3ServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param seg3IdFilter (optional) 
     * @param seg3NameFilter (optional) 
     * @param seg2NameFilter (optional) 
     * @param seg1NameFilter (optional) 
     * @param sorting (optional) 
     * @param skipCount (optional) 
     * @param maxResultCount (optional) 
     * @return Success
     */
    getAll(filter: string | null | undefined, seg3IdFilter: string | null | undefined,seg3NameFilter: string | null | undefined,seg2NameFilter: string | null | undefined, seg1NameFilter: string | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetICSegment3ForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment3/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (seg3IdFilter !== undefined)
            url_ += "Seg3IdFilter=" + encodeURIComponent("" + seg3IdFilter) + "&"; 
        if (seg3NameFilter !== undefined)
            url_ += "Seg3NameFilter=" + encodeURIComponent("" + seg3NameFilter) + "&"; 
        if (seg2NameFilter !== undefined)
            url_ += "Seg2NameFilter=" + encodeURIComponent("" + seg2NameFilter) + "&"; 
        if (seg1NameFilter !== undefined)
            url_ += "Seg1NameFilter=" + encodeURIComponent("" + seg1NameFilter) + "&"; 
        if (sorting !== undefined)
            url_ += "Sorting=" + encodeURIComponent("" + sorting) + "&"; 
        if (skipCount !== undefined)
            url_ += "SkipCount=" + encodeURIComponent("" + skipCount) + "&"; 
        if (maxResultCount !== undefined)
            url_ += "MaxResultCount=" + encodeURIComponent("" + maxResultCount) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetAll(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAll(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfGetICSegment3ForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetICSegment3ForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetICSegment3ForViewDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfGetICSegment3ForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetICSegment3ForViewDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetICSegment3ForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    GetICSegment3ForView(id: number | null | undefined): Observable<GetICSegment3ForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment3/GetICSegment3ForView?";
        if (id !== undefined)
            url_ += "id=" + encodeURIComponent("" + id) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetICSegment3ForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetICSegment3ForView(<any>response_);
                } catch (e) {
                    return <Observable<GetICSegment3ForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetICSegment3ForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetICSegment3ForView(response: HttpResponseBase): Observable<GetICSegment3ForViewDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 300) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GetICSegment3ForViewDto.fromJS(resultData200) : new GetICSegment3ForViewDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetICSegment3ForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    GetICSegment3ForEdit(id: number | null | undefined): Observable<GetICSegment3ForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment3/GetICSegment3ForEdit?";
        if (id !== undefined)
            url_ += "Id=" + encodeURIComponent("" + id) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetICSegment3ForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetICSegment3ForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetICSegment3ForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetICSegment3ForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetICSegment3ForEdit(response: HttpResponseBase): Observable<GetICSegment3ForEditOutput> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GetICSegment3ForEditOutput.fromJS(resultData200) : new GetICSegment3ForEditOutput();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetICSegment3ForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditICSegment3Dto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment3/CreateOrEdit";
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
     * @param id (optional) 
     * @return Success
     */
    delete(id: number | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment3/Delete?";
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
     * @param filter (optional) 
     * @param seg3IdFilter (optional) 
     * @param seg3NameFilter (optional) 
     * @param seg2NameFilter (optional) 
     * @param seg1NameFilter (optional) 
     * @return Success
     */
    GetICSegment3ToExcel(filter: string | null | undefined, seg3IdFilter: string | null | undefined,seg3NameFilter: string | null | undefined,seg2NameFilter: string | null | undefined, seg1NameFilter: string | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment3/GetICSegment3ToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (seg3IdFilter !== undefined)
            url_ += "Seg3IdFilter=" + encodeURIComponent("" + seg3IdFilter) + "&"; 
        if (seg3NameFilter !== undefined)
            url_ += "Seg3NameFilter=" + encodeURIComponent("" + seg3NameFilter) + "&"; 
        if (seg2NameFilter !== undefined)
            url_ += "Seg2NameFilter=" + encodeURIComponent("" + seg2NameFilter) + "&"; 
        if (seg1NameFilter !== undefined)
            url_ += "Seg1NameFilter=" + encodeURIComponent("" + seg1NameFilter) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetICSegment3ToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetICSegment3ToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetICSegment3ToExcel(response: HttpResponseBase): Observable<FileDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? FileDto.fromJS(resultData200) : new FileDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<FileDto>(<any>null);
    }

    GetICSegment3ForFinder(filter: string | null | undefined, seg2ID: string |null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfNameValueDto>{
        let url_ = this.baseUrl + "/api/services/app/ICSegment3/GetICSegment3ForFinder?";
       
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (seg2ID !== undefined)
            url_ += "Seg2ID=" + encodeURIComponent("" + seg2ID) + "&";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (sorting !== undefined)
            url_ += "Sorting=" + encodeURIComponent("" + sorting) + "&";
        if (skipCount !== undefined)
            url_ += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        if (maxResultCount !== undefined)
            url_ += "MaxResultCount=" + encodeURIComponent("" + maxResultCount) + "&";
        
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            debugger;
            return this.processGetICSegment3ForFinder(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                   
                    return this.processGetICSegment3ForFinder(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfNameValueDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfNameValueDto>><any>_observableThrow(response_);
        }));
    }

    processGetICSegment3ForFinder(response: HttpResponseBase):Observable<PagedResultDtoOfNameValueDto>{
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfNameValueDto.fromJS(resultData200) : new PagedResultDtoOfNameValueDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfNameValueDto>(<any>null);
    }

    /**
     * @return Success
     */
    GetSegment3MaxId(Id:string): Observable<number> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment3/GetSegment3MaxId?";
        if (Id !== undefined)
            url_ += "id=" + encodeURIComponent("" + Id) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetSegment3MaxId(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetSegment3MaxId(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processGetSegment3MaxId(response: HttpResponseBase): Observable<number> {
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
}


export class PagedResultDtoOfGetICSegment3ForViewDto implements IPagedResultDtoOfGetICSegment3ForViewDto {
    totalCount!: number | undefined;
    items!: GetICSegment3ForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetICSegment3ForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalCount = data["totalCount"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [] as any;
                for (let item of data["items"])
                    this.items!.push(GetICSegment3ForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetICSegment3ForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetICSegment3ForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        return data; 
    }
}

export interface IPagedResultDtoOfGetICSegment3ForViewDto {
    totalCount: number | undefined;
    items: GetICSegment3ForViewDto[] | undefined;
}

export class GetICSegment3ForViewDto implements IGetICSegment3ForViewDto {
    icSegment!: ICSegment3Dto | undefined;

    constructor(data?: IGetICSegment3ForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        debugger;
        if (data) {
            this.icSegment = data["icSegment3"] ? ICSegment3Dto.fromJS(data["icSegment3"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetICSegment3ForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetICSegment3ForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["icSegment3"] = this.icSegment ? this.icSegment.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IGetICSegment3ForViewDto {
    icSegment: ICSegment3Dto | undefined;
}

export class ICSegment3Dto implements IICSegment3Dto {
    seg1ID: string | undefined;
    seg2ID: string | undefined;
    seg3ID: string | undefined;
    seg3Name: string | undefined;
    id: number | undefined;

    constructor(data?: IICSegment3Dto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.seg1ID = data["seg1Id"];
            this.seg2ID = data["seg2Id"];
            this.seg3ID = data["seg3Id"];
            this.seg3Name = data["seg3Name"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): ICSegment3Dto {
        data = typeof data === 'object' ? data : {};
        let result = new ICSegment3Dto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["seg1Id"] = this.seg1ID;
        data["seg2Id"] = this.seg2ID;
        data["seg3Id"] = this.seg3ID;
        data["seg3Name"] = this.seg3Name;
        data["id"] = this.id;
        return data; 
    }
}

export interface IICSegment3Dto {
    seg1ID: string | undefined;
    seg2ID: string | undefined;
    seg3ID: string | undefined;
    seg3Name: string | undefined;
    id: number | undefined;
}

export class GetICSegment3ForEditOutput implements IGetICSegment3ForEditOutput {
    icSegment!: CreateOrEditICSegment3Dto | undefined;
    seg1Name!: string | undefined;
    seg2Name!: string | undefined;

    constructor(data?: IGetICSegment3ForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.seg1Name = data["seg1Name"];
            this.seg2Name = data["seg2Name"];
            this.icSegment = data["icSegment3"] ? CreateOrEditICSegment3Dto.fromJS(data["icSegment3"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetICSegment3ForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetICSegment3ForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["seg1Name"] = this.seg1Name;
        data["seg2Name"] = this.seg2Name;
        data["icSegment"] = this.icSegment ? this.icSegment.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IGetICSegment3ForEditOutput {
    icSegment: CreateOrEditICSegment3Dto | undefined;
    seg1Name: string | undefined;
    seg2Name: string | undefined;
}

export class CreateOrEditICSegment3Dto implements ICreateOrEditICSegment3Dto {
    flag : boolean |undefined;
    seg1ID: string | undefined;
    seg2ID: string | undefined;
    seg3ID!: string | undefined;
    seg3Name!: string | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditICSegment3Dto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.flag = data["flag"];
            this.seg3ID = data["seg3Id"];
            this.seg1ID = data["seg1Id"];
            this.seg2ID = data["seg2Id"];
            this.seg3Name = data["seg3Name"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditICSegment3Dto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditICSegment3Dto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["flag"] = this.flag;
        data["seg1Id"] = this.seg1ID;
        data["seg2Id"] = this.seg2ID;
        data["seg3Id"] = this.seg3ID;
        data["seg3Name"] = this.seg3Name;
        data["id"] = this.id;
        return data; 
    }
}

export interface ICreateOrEditICSegment3Dto {
    flag: boolean |undefined;
    seg1ID: string | undefined;
    seg2ID: string | undefined;
    seg3ID: string | undefined;
    seg3Name: string | undefined;
    id: number | undefined;
}

export class FileDto implements IFileDto {
    fileName!: string;
    fileType!: string | undefined;
    fileToken!: string;

    constructor(data?: IFileDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.fileName = data["fileName"];
            this.fileType = data["fileType"];
            this.fileToken = data["fileToken"];
        }
    }

    static fromJS(data: any): FileDto {
        data = typeof data === 'object' ? data : {};
        let result = new FileDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["fileName"] = this.fileName;
        data["fileType"] = this.fileType;
        data["fileToken"] = this.fileToken;
        return data; 
    }
}

export interface IFileDto {
    fileName: string;
    fileType: string | undefined;
    fileToken: string;
}
