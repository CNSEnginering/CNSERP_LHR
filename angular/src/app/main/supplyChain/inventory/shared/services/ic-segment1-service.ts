import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import * as moment from 'moment';
import { API_BASE_URL, blobToText, throwException, NameValueDto, PagedResultDtoOfNameValueDto } from '@shared/service-proxies/service-proxies';


@Injectable()
export class IcSegment1ServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;


    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param seg1IDFilter (optional) 
     * @param seg1NameFilter (optional) 
     * @param sorting (optional) 
     * @param skipCount (optional) 
     * @param maxResultCount (optional) 
     * @return Success
     */
    getAll(filter: string | null | undefined, seg1IDFilter: string | null | undefined, seg1NameFilter: string | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetICSegment1ForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment/GetAll?";
       
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (seg1IDFilter !== undefined)
            url_ += "Seg1IDFilter=" + encodeURIComponent("" + seg1IDFilter) + "&";
        if (seg1NameFilter !== undefined)
            url_ += "Seg1NameFilter=" + encodeURIComponent("" + seg1NameFilter) + "&";
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
            
            return this.processGetAll(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                   
                    return this.processGetAll(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfGetICSegment1ForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetICSegment1ForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetICSegment1ForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetICSegment1ForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetICSegment1ForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetICSegment1ForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    GetICSegment1ForView(id: number | null | undefined): Observable<GetICSegment1ForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment/GetICSegment1ForView?";
        if (id !== undefined)
            url_ += "id=" + encodeURIComponent("" + id) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetICSegment1ForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetICSegment1ForView(<any>response_);
                } catch (e) {
                    return <Observable<GetICSegment1ForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetICSegment1ForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetICSegment1ForView(response: HttpResponseBase): Observable<GetICSegment1ForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetICSegment1ForViewDto.fromJS(resultData200) : new GetICSegment1ForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetICSegment1ForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    GetICSegment1ForEdit(id: number | null | undefined): Observable<GetICSegment1ForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment/GetICSegment1ForEdit?";
        if (id !== undefined)
            url_ += "Id=" + encodeURIComponent("" + id) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            debugger
            return this.processGetICSegment1ForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    debugger
                    return this.processGetICSegment1ForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetICSegment1ForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetICSegment1ForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetICSegment1ForEdit(response: HttpResponseBase): Observable<GetICSegment1ForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetICSegment1ForEditOutput.fromJS(resultData200) : new GetICSegment1ForEditOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetICSegment1ForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditICSegment1Dto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/ICSegment/Delete?";
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

     /**
     * @param filter (optional) 
     * @param seg1IDFilter (optional) 
     * @param seg1NameFilter (optional) 
     * @return Success
     */
    GetICSegment1ToExcel(filter: string | null | undefined, seg1IDFilter: string | null | undefined,
        seg1NameFilter: string | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment/GetICSegment1ToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (seg1IDFilter !== undefined)
            url_ += "Seg1IDFilter=" + encodeURIComponent("" + seg1IDFilter) + "&";
        if (seg1NameFilter !== undefined)
            url_ += "Seg1NameFilter=" + encodeURIComponent("" + seg1NameFilter) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetICSegment1ToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetICSegment1ToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetICSegment1ToExcel(response: HttpResponseBase): Observable<FileDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
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

    GetICSegment1ForFinder(filter: string | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfNameValueDto>{
        let url_ = this.baseUrl + "/api/services/app/ICSegment/GetICSegment1ForFinder?";
       
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
            return this.processGetICSegment1ForFinder(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                   debugger;
                    return this.processGetICSegment1ForFinder(<any>response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfNameValueDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfNameValueDto>><any>_observableThrow(response_);
        }));
    }

    processGetICSegment1ForFinder(response: HttpResponseBase):Observable<PagedResultDtoOfNameValueDto>{
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
    GetSegment1MaxId(): Observable<number> {
        let url_ = this.baseUrl + "/api/services/app/ICSegment/GetSegment1MaxId";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetSegment1MaxId(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetSegment1MaxId(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processGetSegment1MaxId(response: HttpResponseBase): Observable<number> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
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


export class PagedResultDtoOfGetICSegment1ForViewDto implements IPagedResultDtoOfGetICSegment1ForViewDto {
    totalCount!: number | undefined;
    items!: GetICSegment1ForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetICSegment1ForViewDto) {
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
                    this.items!.push(GetICSegment1ForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetICSegment1ForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetICSegment1ForViewDto();
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

export interface IPagedResultDtoOfGetICSegment1ForViewDto {
    totalCount: number | undefined;
    items: GetICSegment1ForViewDto[] | undefined;
}

export class GetICSegment1ForViewDto implements IGetICSegment1ForViewDto {
    icSegment!: ICSegment1Dto | undefined;

    constructor(data?: IGetICSegment1ForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
       
        if (data) {
            this.icSegment = data["icSegment1"] ? ICSegment1Dto.fromJS(data["icSegment1"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetICSegment1ForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetICSegment1ForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["icSegment1"] = this.icSegment ? this.icSegment.toJSON() : <any>undefined;
        return data;
    }
}

export interface IGetICSegment1ForViewDto {
    icSegment: ICSegment1Dto | undefined;
}

export class ICSegment1Dto implements IICSegment1Dto {
    seg1ID: string | undefined;
    seg1Name: string | undefined;
    id: number | undefined;

    constructor(data?: IICSegment1Dto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.seg1ID = data["seg1ID"];
            this.seg1Name = data["seg1Name"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): ICSegment1Dto {
        data = typeof data === 'object' ? data : {};
        let result = new ICSegment1Dto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["seg1ID"] = this.seg1ID;
        data["seg1Name"] = this.seg1Name;
        data["id"] = this.id;
        return data;
    }
}

export interface IICSegment1Dto {
    seg1ID: string | undefined;
    seg1Name: string | undefined;
    id: number | undefined;
}

export class GetICSegment1ForEditOutput implements IGetICSegment1ForEditOutput {
    icSegment!: CreateOrEditICSegment1Dto | undefined;

    constructor(data?: IGetICSegment1ForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.icSegment = data["icSegment1"] ? CreateOrEditICSegment1Dto.fromJS(data["icSegment1"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetICSegment1ForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetICSegment1ForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["icSegment"] = this.icSegment ? this.icSegment.toJSON() : <any>undefined;
        return data;
    }
}

export interface IGetICSegment1ForEditOutput {
    icSegment: CreateOrEditICSegment1Dto | undefined;
}

export class CreateOrEditICSegment1Dto implements ICreateOrEditICSegment1Dto {
    flag: boolean | undefined;
    seg1ID!: string | undefined;
    seg1Name!: string | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditICSegment1Dto) {
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
            this.seg1ID = data["seg1ID"];
            this.seg1Name = data["seg1Name"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditICSegment1Dto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditICSegment1Dto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["flag"] = this.flag 
        data["seg1ID"] = this.seg1ID;
        data["seg1Name"] = this.seg1Name;
        data["id"] = this.id;
        return data;
    }
}

export interface ICreateOrEditICSegment1Dto {
    flag: boolean |undefined;
    seg1ID: string | undefined;
    seg1Name: string | undefined;
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
