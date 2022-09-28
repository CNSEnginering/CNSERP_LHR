import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import * as moment from 'moment';
import { blobToText, throwException, API_BASE_URL, PagedResultDtoOfNameValueDto } from '@shared/service-proxies/service-proxies';
//export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class IcSegment2ServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param seg2IdFilter (optional) 
     * @param seg2NameFilter (optional) 
     * @param seg1NameFilter (optional) 
     * @param sorting (optional) 
     * @param skipCount (optional) 
     * @param maxResultCount (optional) 
     * @return Success
     */
    getAll(filter: string | null | undefined, seg2IdFilter: string | null | undefined, seg2NameFilter: string | null | undefined, seg1NameFilter: string | null | undefined,  sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetICSegment2ForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment2/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (seg2IdFilter !== undefined)
            url_ += "Seg2IdFilter=" + encodeURIComponent("" + seg2IdFilter) + "&"; 
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
debugger;
        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetAll(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAll(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfGetICSegment2ForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetICSegment2ForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetICSegment2ForViewDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            debugger;
            result200 = resultData200 ? PagedResultDtoOfGetICSegment2ForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetICSegment2ForViewDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetICSegment2ForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    GetICSegment2ForView(id: number | null | undefined): Observable<GetICSegment2ForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment2/GetICSegment2ForView?";
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
            return this.processGetICSegment2ForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetICSegment2ForView(<any>response_);
                } catch (e) {
                    return <Observable<GetICSegment2ForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetICSegment2ForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetICSegment2ForView(response: HttpResponseBase): Observable<GetICSegment2ForViewDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GetICSegment2ForViewDto.fromJS(resultData200) : new GetICSegment2ForViewDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetICSegment2ForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    GetICSegment2ForEdit(id: number | null | undefined): Observable<GetICSegment2ForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment2/GetICSegment2ForEdit?";
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
            return this.processGetICSegment2ForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetICSegment2ForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetICSegment2ForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetICSegment2ForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetICSegment2ForEdit(response: HttpResponseBase): Observable<GetICSegment2ForEditOutput> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GetICSegment2ForEditOutput.fromJS(resultData200) : new GetICSegment2ForEditOutput();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetICSegment2ForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditICSegment2Dto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment2/CreateOrEdit";
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
            debugger;
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
        let url_ = this.baseUrl + "/api/services/app/ICSegment2/Delete?";
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
     * @param seg2IdFilter (optional) 
     * @param seg2NameFilter (optional) 
     * @param seg1NameFilter (optional) 
     * @return Success
     */
    GetICSegment2ToExcel(filter: string | null | undefined, seg2IdFilter: string | null | undefined, seg2NameFilter: string | null | undefined, seg1NameFilter: string | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment2/GetICSegment2ToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (seg2IdFilter !== undefined)
            url_ += "Seg2IdFilter=" + encodeURIComponent("" + seg2IdFilter) + "&"; 
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
            return this.processGetICSegment2ToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetICSegment2ToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetICSegment2ToExcel(response: HttpResponseBase): Observable<FileDto> {
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

    /**
     * @return Success
     */
    GetSegment2MaxId(id: string | null | undefined): Observable<number> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment2/GetSegment2MaxId?";
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
            return this.processGetSegment2MaxId(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetSegment2MaxId(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processGetSegment2MaxId(response: HttpResponseBase): Observable<number> {
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

    GetICSegment2ForFinder(filter: string | null | undefined, seg1ID: string |null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfNameValueDto>{
        let url_ = this.baseUrl + "/api/services/app/ICSegment2/GetICSegment2ForFinder?";
       
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (seg1ID !== undefined)
            url_ += "seg1ID=" + encodeURIComponent("" + seg1ID) + "&";
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
            return this.processGetICSegment2ForFinder(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                   
                    return this.processGetICSegment2ForFinder(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfNameValueDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfNameValueDto>><any>_observableThrow(response_);
        }));
    }

    processGetICSegment2ForFinder(response: HttpResponseBase):Observable<PagedResultDtoOfNameValueDto>{
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
}


export class PagedResultDtoOfGetICSegment2ForViewDto implements IPagedResultDtoOfGetICSegment2ForViewDto {
    totalCount!: number | undefined;
    items!: GetICSegment2ForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetICSegment2ForViewDto) {
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
                    this.items!.push(GetICSegment2ForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetICSegment2ForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetICSegment2ForViewDto();
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

export interface IPagedResultDtoOfGetICSegment2ForViewDto {
    totalCount: number | undefined;
    items: GetICSegment2ForViewDto[] | undefined;
}

export class GetICSegment2ForViewDto implements IGetICSegment2ForViewDto {
    icSegment!: ICSegment2Dto | undefined;

    constructor(data?: IGetICSegment2ForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.icSegment = data["icSegment2"] ? ICSegment2Dto.fromJS(data["icSegment2"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetICSegment2ForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetICSegment2ForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["icSegment2"] = this.icSegment ? this.icSegment.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IGetICSegment2ForViewDto {
    icSegment: ICSegment2Dto | undefined;
}

export class ICSegment2Dto implements IICSegment2Dto {
    seg1ID: string | undefined;
    seg2ID: string | undefined;
    seg2Name: string | undefined;
    id: number | undefined;

    constructor(data?: IICSegment2Dto) {
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
            this.seg2Name = data["seg2Name"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): ICSegment2Dto {
        data = typeof data === 'object' ? data : {};
        let result = new ICSegment2Dto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["seg1Id"] = this.seg1ID;
        data["seg2Id"] = this.seg2ID;
        data["seg2Name"] = this.seg2Name;
        data["id"] = this.id;
        return data; 
    }
}

export interface IICSegment2Dto {
    seg1ID: string | undefined;
    seg2ID: string | undefined;
    seg2Name: string | undefined;
    id: number | undefined;
}

export class GetICSegment2ForEditOutput implements IGetICSegment2ForEditOutput {
    icSegment!: CreateOrEditICSegment2Dto | undefined;
    seg1Name!: string |undefined;

    constructor(data?: IGetICSegment2ForEditOutput) {
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
            this.icSegment = data["icSegment2"] ? CreateOrEditICSegment2Dto.fromJS(data["icSegment2"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetICSegment2ForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetICSegment2ForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["seg1Name"] = this.seg1Name;
        data["icSegment"] = this.icSegment ? this.icSegment.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IGetICSegment2ForEditOutput {
    icSegment: CreateOrEditICSegment2Dto | undefined;
}

export class CreateOrEditICSegment2Dto implements ICreateOrEditICSegment2Dto {
    flag: boolean |undefined;
    seg1ID: string | undefined;
    seg2ID!: string | undefined;
    seg2Name!: string | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditICSegment2Dto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            debugger;
            this.flag = data["flag"];
            this.seg1ID = data["seg1Id"];
            this.seg2ID = data["seg2Id"];
            this.seg2Name = data["seg2Name"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditICSegment2Dto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditICSegment2Dto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["flag"] = this.flag;
        data["seg1Id"] = this.seg1ID;
        data["seg2Id"] = this.seg2ID;
        data["seg2Name"] = this.seg2Name;
        data["id"] = this.id;
        return data; 
    }
}

export interface ICreateOrEditICSegment2Dto {
    flag: boolean |undefined;
    seg1ID: string | undefined;
    seg2ID: string | undefined;
    seg2Name: string | undefined;
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

export class SwaggerException extends Error {
    message: string;
    status: number; 
    response: string; 
    headers: { [key: string]: any; };
    result: any; 

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isSwaggerException = true;

    static isSwaggerException(obj: any): obj is SwaggerException {
        return obj.isSwaggerException === true;
    }
}



