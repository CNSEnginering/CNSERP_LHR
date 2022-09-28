import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

import * as moment from 'moment';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');


@Injectable()
export class GLAccountsPermissionsServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param userIDFilter (optional) 
     * @param maxCanSeeFilter (optional) 
     * @param minCanSeeFilter (optional) 
     * @param beginAccFilter (optional) 
     * @param endAccFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional) 
     * @param sorting (optional) 
     * @param skipCount (optional) 
     * @param maxResultCount (optional) 
     * @return Success
     */
    getAll(filter: string | null | undefined, userIDFilter: string | null | undefined, maxCanSeeFilter: number | null | undefined, minCanSeeFilter: number | null | undefined, beginAccFilter: string | null | undefined, endAccFilter: string | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetGLAccountsPermissionForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/GLAccountsPermissions/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (userIDFilter !== undefined)
            url_ += "UserIDFilter=" + encodeURIComponent("" + userIDFilter) + "&"; 
        if (maxCanSeeFilter !== undefined)
            url_ += "MaxCanSeeFilter=" + encodeURIComponent("" + maxCanSeeFilter) + "&"; 
        if (minCanSeeFilter !== undefined)
            url_ += "MinCanSeeFilter=" + encodeURIComponent("" + minCanSeeFilter) + "&"; 
        if (beginAccFilter !== undefined)
            url_ += "BeginAccFilter=" + encodeURIComponent("" + beginAccFilter) + "&"; 
        if (endAccFilter !== undefined)
            url_ += "EndAccFilter=" + encodeURIComponent("" + endAccFilter) + "&"; 
        if (audtUserFilter !== undefined)
            url_ += "AudtUserFilter=" + encodeURIComponent("" + audtUserFilter) + "&"; 
        if (maxAudtDateFilter !== undefined)
            url_ += "MaxAudtDateFilter=" + encodeURIComponent(maxAudtDateFilter ? "" + maxAudtDateFilter.toJSON() : "") + "&"; 
        if (minAudtDateFilter !== undefined)
            url_ += "MinAudtDateFilter=" + encodeURIComponent(minAudtDateFilter ? "" + minAudtDateFilter.toJSON() : "") + "&"; 
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
                    return <Observable<PagedResultDtoOfGetGLAccountsPermissionForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetGLAccountsPermissionForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetGLAccountsPermissionForViewDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfGetGLAccountsPermissionForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetGLAccountsPermissionForViewDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetGLAccountsPermissionForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getGLAccountsPermissionForView(id: number | null | undefined): Observable<GetGLAccountsPermissionForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/GLAccountsPermissions/GetGLAccountsPermissionForView?";
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
            return this.processGetGLAccountsPermissionForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetGLAccountsPermissionForView(<any>response_);
                } catch (e) {
                    return <Observable<GetGLAccountsPermissionForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetGLAccountsPermissionForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetGLAccountsPermissionForView(response: HttpResponseBase): Observable<GetGLAccountsPermissionForViewDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GetGLAccountsPermissionForViewDto.fromJS(resultData200) : new GetGLAccountsPermissionForViewDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetGLAccountsPermissionForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getGLAccountsPermissionForEdit(id: number | null | undefined): Observable<GetGLAccountsPermissionForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/GLAccountsPermissions/GetGLAccountsPermissionForEdit?";
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
            return this.processGetGLAccountsPermissionForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetGLAccountsPermissionForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetGLAccountsPermissionForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetGLAccountsPermissionForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetGLAccountsPermissionForEdit(response: HttpResponseBase): Observable<GetGLAccountsPermissionForEditOutput> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GetGLAccountsPermissionForEditOutput.fromJS(resultData200) : new GetGLAccountsPermissionForEditOutput();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetGLAccountsPermissionForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditGLAccountsPermissionDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/GLAccountsPermissions/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/GLAccountsPermissions/Delete?";
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
     * @param userIDFilter (optional) 
     * @param maxCanSeeFilter (optional) 
     * @param minCanSeeFilter (optional) 
     * @param beginAccFilter (optional) 
     * @param endAccFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional) 
     * @return Success
     */
    getGLAccountsPermissionsToExcel(filter: string | null | undefined, userIDFilter: string | null | undefined, maxCanSeeFilter: number | null | undefined, minCanSeeFilter: number | null | undefined, beginAccFilter: string | null | undefined, endAccFilter: string | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/GLAccountsPermissions/GetGLAccountsPermissionsToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (userIDFilter !== undefined)
            url_ += "UserIDFilter=" + encodeURIComponent("" + userIDFilter) + "&"; 
        if (maxCanSeeFilter !== undefined)
            url_ += "MaxCanSeeFilter=" + encodeURIComponent("" + maxCanSeeFilter) + "&"; 
        if (minCanSeeFilter !== undefined)
            url_ += "MinCanSeeFilter=" + encodeURIComponent("" + minCanSeeFilter) + "&"; 
        if (beginAccFilter !== undefined)
            url_ += "BeginAccFilter=" + encodeURIComponent("" + beginAccFilter) + "&"; 
        if (endAccFilter !== undefined)
            url_ += "EndAccFilter=" + encodeURIComponent("" + endAccFilter) + "&"; 
        if (audtUserFilter !== undefined)
            url_ += "AudtUserFilter=" + encodeURIComponent("" + audtUserFilter) + "&"; 
        if (maxAudtDateFilter !== undefined)
            url_ += "MaxAudtDateFilter=" + encodeURIComponent(maxAudtDateFilter ? "" + maxAudtDateFilter.toJSON() : "") + "&"; 
        if (minAudtDateFilter !== undefined)
            url_ += "MinAudtDateFilter=" + encodeURIComponent(minAudtDateFilter ? "" + minAudtDateFilter.toJSON() : "") + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetGLAccountsPermissionsToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetGLAccountsPermissionsToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetGLAccountsPermissionsToExcel(response: HttpResponseBase): Observable<FileDto> {
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
}


export class PagedResultDtoOfGetGLAccountsPermissionForViewDto implements IPagedResultDtoOfGetGLAccountsPermissionForViewDto {
    totalCount!: number | undefined;
    items!: GetGLAccountsPermissionForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetGLAccountsPermissionForViewDto) {
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
                    this.items!.push(GetGLAccountsPermissionForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetGLAccountsPermissionForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetGLAccountsPermissionForViewDto();
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

export interface IPagedResultDtoOfGetGLAccountsPermissionForViewDto {
    totalCount: number | undefined;
    items: GetGLAccountsPermissionForViewDto[] | undefined;
}

export class GetGLAccountsPermissionForViewDto implements IGetGLAccountsPermissionForViewDto {
    glAccountsPermission!: GLAccountsPermissionDto | undefined;

    constructor(data?: IGetGLAccountsPermissionForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.glAccountsPermission = data["glAccountsPermission"] ? GLAccountsPermissionDto.fromJS(data["glAccountsPermission"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetGLAccountsPermissionForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetGLAccountsPermissionForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["glAccountsPermission"] = this.glAccountsPermission ? this.glAccountsPermission.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IGetGLAccountsPermissionForViewDto {
    glAccountsPermission: GLAccountsPermissionDto | undefined;
}

export class GLAccountsPermissionDto implements IGLAccountsPermissionDto {
    userID!: string | undefined;
    canSee!: number | undefined;
    beginAcc!: string | undefined;
    endAcc!: string | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IGLAccountsPermissionDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.userID = data["userID"];
            this.canSee = data["canSee"];
            this.beginAcc = data["beginAcc"];
            this.endAcc = data["endAcc"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.id = data["id"];
        }
    }

    static fromJS(data: any): GLAccountsPermissionDto {
        data = typeof data === 'object' ? data : {};
        let result = new GLAccountsPermissionDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["userID"] = this.userID;
        data["canSee"] = this.canSee;
        data["beginAcc"] = this.beginAcc;
        data["endAcc"] = this.endAcc;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        return data; 
    }
}

export interface IGLAccountsPermissionDto {
    userID: string | undefined;
    canSee: number | undefined;
    beginAcc: string | undefined;
    endAcc: string | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    id: number | undefined;
}

export class GetGLAccountsPermissionForEditOutput implements IGetGLAccountsPermissionForEditOutput {
    glAccountsPermission!: CreateOrEditGLAccountsPermissionDto | undefined;

    constructor(data?: IGetGLAccountsPermissionForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.glAccountsPermission = data["glAccountsPermission"] ? CreateOrEditGLAccountsPermissionDto.fromJS(data["glAccountsPermission"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetGLAccountsPermissionForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetGLAccountsPermissionForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["glAccountsPermission"] = this.glAccountsPermission ? this.glAccountsPermission.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IGetGLAccountsPermissionForEditOutput {
    glAccountsPermission: CreateOrEditGLAccountsPermissionDto | undefined;
}

export class CreateOrEditGLAccountsPermissionDto implements ICreateOrEditGLAccountsPermissionDto {
    userID!: string | undefined;
    canSee!: number | undefined;
    beginAcc!: string | undefined;
    endAcc!: string | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditGLAccountsPermissionDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.userID = data["userID"];
            this.canSee = data["canSee"];
            this.beginAcc = data["beginAcc"];
            this.endAcc = data["endAcc"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditGLAccountsPermissionDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditGLAccountsPermissionDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["userID"] = this.userID;
        data["canSee"] = this.canSee;
        data["beginAcc"] = this.beginAcc;
        data["endAcc"] = this.endAcc;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        return data; 
    }
}

export interface ICreateOrEditGLAccountsPermissionDto {
    userID: string | undefined;
    canSee: number | undefined;
    beginAcc: string | undefined;
    endAcc: string | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    id: number | undefined;
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

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if(result !== null && result !== undefined)
        return _observableThrow(result);
    else
        return _observableThrow(new SwaggerException(message, status, response, headers, null));
}

function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader(); 
            reader.onload = event => { 
                observer.next((<any>event.target).result);
                observer.complete();
            };
            reader.readAsText(blob); 
        }
    });
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