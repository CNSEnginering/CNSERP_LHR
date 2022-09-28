import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

import * as moment from 'moment';
import { API_BASE_URL, blobToText, throwException, FileDto } from '@shared/service-proxies/service-proxies';


@Injectable()
export class PLCategoriesServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param maxTenantIDFilter (optional) 
     * @param minTenantIDFilter (optional) 
     * @param typeIDFilter (optional) 
     * @param headingTextFilter (optional) 
     * @param maxSortOrderFilter (optional) 
     * @param minSortOrderFilter (optional) 
     * @param sorting (optional) 
     * @param skipCount (optional) 
     * @param maxResultCount (optional) 
     * @return Success
     */
    getAll(filter: string | null | undefined, maxTenantIDFilter: number | null | undefined, minTenantIDFilter: number | null | undefined, typeIDFilter: string | null | undefined, headingTextFilter: string | null | undefined, maxSortOrderFilter: number | null | undefined, minSortOrderFilter: number | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetPLCategoryForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/PLCategories/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (maxTenantIDFilter !== undefined)
            url_ += "MaxTenantIDFilter=" + encodeURIComponent("" + maxTenantIDFilter) + "&"; 
        if (minTenantIDFilter !== undefined)
            url_ += "MinTenantIDFilter=" + encodeURIComponent("" + minTenantIDFilter) + "&"; 
        if (typeIDFilter !== undefined)
            url_ += "TypeIDFilter=" + encodeURIComponent("" + typeIDFilter) + "&"; 
        if (headingTextFilter !== undefined)
            url_ += "HeadingTextFilter=" + encodeURIComponent("" + headingTextFilter) + "&"; 
        if (maxSortOrderFilter !== undefined)
            url_ += "MaxSortOrderFilter=" + encodeURIComponent("" + maxSortOrderFilter) + "&"; 
        if (minSortOrderFilter !== undefined)
            url_ += "MinSortOrderFilter=" + encodeURIComponent("" + minSortOrderFilter) + "&"; 
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
                    return <Observable<PagedResultDtoOfGetPLCategoryForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetPLCategoryForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetPLCategoryForViewDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfGetPLCategoryForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetPLCategoryForViewDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetPLCategoryForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getPLCategoryForView(id: number | null | undefined): Observable<GetPLCategoryForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/PLCategories/GetPLCategoryForView?";
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
            return this.processGetPLCategoryForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetPLCategoryForView(<any>response_);
                } catch (e) {
                    return <Observable<GetPLCategoryForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetPLCategoryForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetPLCategoryForView(response: HttpResponseBase): Observable<GetPLCategoryForViewDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GetPLCategoryForViewDto.fromJS(resultData200) : new GetPLCategoryForViewDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetPLCategoryForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getPLCategoryForEdit(id: number | null | undefined): Observable<GetPLCategoryForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/PLCategories/GetPLCategoryForEdit?";
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
            return this.processGetPLCategoryForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetPLCategoryForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetPLCategoryForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetPLCategoryForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetPLCategoryForEdit(response: HttpResponseBase): Observable<GetPLCategoryForEditOutput> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GetPLCategoryForEditOutput.fromJS(resultData200) : new GetPLCategoryForEditOutput();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetPLCategoryForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditPLCategoryDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/PLCategories/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/PLCategories/Delete?";
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
     * @param maxTenantIDFilter (optional) 
     * @param minTenantIDFilter (optional) 
     * @param typeIDFilter (optional) 
     * @param headingTextFilter (optional) 
     * @param maxSortOrderFilter (optional) 
     * @param minSortOrderFilter (optional) 
     * @return Success
     */
    getPLCategoriesToExcel(filter: string | null | undefined, maxTenantIDFilter: number | null | undefined, minTenantIDFilter: number | null | undefined, typeIDFilter: string | null | undefined, headingTextFilter: string | null | undefined, maxSortOrderFilter: number | null | undefined, minSortOrderFilter: number | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/PLCategories/GetPLCategoriesToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (maxTenantIDFilter !== undefined)
            url_ += "MaxTenantIDFilter=" + encodeURIComponent("" + maxTenantIDFilter) + "&"; 
        if (minTenantIDFilter !== undefined)
            url_ += "MinTenantIDFilter=" + encodeURIComponent("" + minTenantIDFilter) + "&"; 
        if (typeIDFilter !== undefined)
            url_ += "TypeIDFilter=" + encodeURIComponent("" + typeIDFilter) + "&"; 
        if (headingTextFilter !== undefined)
            url_ += "HeadingTextFilter=" + encodeURIComponent("" + headingTextFilter) + "&"; 
        if (maxSortOrderFilter !== undefined)
            url_ += "MaxSortOrderFilter=" + encodeURIComponent("" + maxSortOrderFilter) + "&"; 
        if (minSortOrderFilter !== undefined)
            url_ += "MinSortOrderFilter=" + encodeURIComponent("" + minSortOrderFilter) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetPLCategoriesToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetPLCategoriesToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetPLCategoriesToExcel(response: HttpResponseBase): Observable<FileDto> {
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

    getCategoryList(input: string | null | undefined): Observable<ListResultDtoOfPLCategoryComboboxItemDto> {
        let url_ = this.baseUrl + "/api/services/app/PLCategories/getCategoryList?";
        if (input !== undefined)
            url_ += "input=" + encodeURIComponent("" + input) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetCategoryList(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetCategoryList(<any>response_);
                } catch (e) {
                    return <Observable<ListResultDtoOfPLCategoryComboboxItemDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<ListResultDtoOfPLCategoryComboboxItemDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetCategoryList(response: HttpResponseBase): Observable<ListResultDtoOfPLCategoryComboboxItemDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? ListResultDtoOfPLCategoryComboboxItemDto.fromJS(resultData200) : new ListResultDtoOfPLCategoryComboboxItemDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<ListResultDtoOfPLCategoryComboboxItemDto>(<any>null);
    }
}



export class PagedResultDtoOfGetPLCategoryForViewDto implements IPagedResultDtoOfGetPLCategoryForViewDto {
    totalCount!: number | undefined;
    items!: GetPLCategoryForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetPLCategoryForViewDto) {
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
                    this.items!.push(GetPLCategoryForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetPLCategoryForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetPLCategoryForViewDto();
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

export interface IPagedResultDtoOfGetPLCategoryForViewDto {
    totalCount: number | undefined;
    items: GetPLCategoryForViewDto[] | undefined;
}

export class GetPLCategoryForViewDto implements IGetPLCategoryForViewDto {
    plCategory!: PLCategoryDto | undefined;

    constructor(data?: IGetPLCategoryForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.plCategory = data["plCategory"] ? PLCategoryDto.fromJS(data["plCategory"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetPLCategoryForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetPLCategoryForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["plCategory"] = this.plCategory ? this.plCategory.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IGetPLCategoryForViewDto {
    plCategory: PLCategoryDto | undefined;
}

export class PLCategoryDto implements IPLCategoryDto {
    tenantID!: number | undefined;
    typeID!: string | undefined;
    headingText!: string | undefined;
    sortOrder!: number | undefined;
    id!: number | undefined;

    constructor(data?: IPLCategoryDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.tenantID = data["tenantID"];
            this.typeID = data["typeID"];
            this.headingText = data["headingText"];
            this.sortOrder = data["sortOrder"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): PLCategoryDto {
        data = typeof data === 'object' ? data : {};
        let result = new PLCategoryDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["tenantID"] = this.tenantID;
        data["typeID"] = this.typeID;
        data["headingText"] = this.headingText;
        data["sortOrder"] = this.sortOrder;
        data["id"] = this.id;
        return data; 
    }
}

export interface IPLCategoryDto {
    tenantID: number | undefined;
    typeID: string | undefined;
    headingText: string | undefined;
    sortOrder: number | undefined;
    id: number | undefined;
}

export class GetPLCategoryForEditOutput implements IGetPLCategoryForEditOutput {
    plCategory!: CreateOrEditPLCategoryDto | undefined;

    constructor(data?: IGetPLCategoryForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.plCategory = data["plCategory"] ? CreateOrEditPLCategoryDto.fromJS(data["plCategory"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetPLCategoryForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetPLCategoryForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["plCategory"] = this.plCategory ? this.plCategory.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IGetPLCategoryForEditOutput {
    plCategory: CreateOrEditPLCategoryDto | undefined;
}

export class CreateOrEditPLCategoryDto implements ICreateOrEditPLCategoryDto {
    tenantID!: number | undefined;
    typeID!: string | undefined;
    headingText!: string | undefined;
    sortOrder!: number | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditPLCategoryDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.tenantID = data["tenantID"];
            this.typeID = data["typeID"];
            this.headingText = data["headingText"];
            this.sortOrder = data["sortOrder"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditPLCategoryDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditPLCategoryDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["tenantID"] = this.tenantID;
        data["typeID"] = this.typeID;
        data["headingText"] = this.headingText;
        data["sortOrder"] = this.sortOrder;
        data["id"] = this.id;
        return data; 
    }
}

export interface ICreateOrEditPLCategoryDto {
    tenantID: number | undefined;
    typeID: string | undefined;
    headingText: string | undefined;
    sortOrder: number | undefined;
    id: number | undefined;
}

export class ListResultDtoOfPLCategoryComboboxItemDto implements IListResultDtoOfPLCategoryComboboxItemDto {
    items!: PLCategoryComboboxItemDto[] | undefined;

    constructor(data?: IListResultDtoOfPLCategoryComboboxItemDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [] as any;
                for (let item of data["items"])
                    this.items!.push(PLCategoryComboboxItemDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ListResultDtoOfPLCategoryComboboxItemDto {
        data = typeof data === 'object' ? data : {};
        let result = new ListResultDtoOfPLCategoryComboboxItemDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        return data; 
    }
}

export interface IListResultDtoOfPLCategoryComboboxItemDto {
    items: PLCategoryComboboxItemDto[] | undefined;
}

export class PLCategoryComboboxItemDto implements IPLCategoryComboboxItemDto {
    id!: number | undefined;
    value!: string | undefined;
    displayText!: string | undefined;
    isSelected!: boolean | undefined;
    sortOrder!: number | undefined

    constructor(data?: IPLCategoryComboboxItemDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.id = data["id"];
            this.value = data["value"];
            this.displayText = data["displayText"];
            this.isSelected = data["isSelected"];
            this.sortOrder = data["sortOrder"];
        }
    }

    static fromJS(data: any): PLCategoryComboboxItemDto {
        data = typeof data === 'object' ? data : {};
        let result = new PLCategoryComboboxItemDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["value"] = this.value;
        data["displayText"] = this.displayText;
        data["isSelected"] = this.isSelected;
        data["sortOrder"] = this.sortOrder;
        return data; 
    }
}

export interface IPLCategoryComboboxItemDto {
    id: number | undefined;
    value: string | undefined;
    displayText: string | undefined;
    isSelected: boolean | undefined;
    sortOrder: number | undefined
}
