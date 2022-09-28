import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { API_BASE_URL, FileDto, blobToText, throwException } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';

@Injectable()
export class IcUnitServiceProxy{
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;


    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    getAll(filter: string | null | undefined, unitFilter: string | null | undefined, converFilter: string | null | undefined, activeFilter: boolean | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetIC_UnitForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/IC_UNITs/GetAll?";
       
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (unitFilter !== undefined)
            url_ += "UnitFilter=" + encodeURIComponent("" + unitFilter) + "&";
        if (converFilter !== undefined)
            url_ += "ConverFilter=" + encodeURIComponent("" + converFilter) + "&";
        if (activeFilter !== undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&";
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
                    return <Observable<PagedResultDtoOfGetIC_UnitForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetIC_UnitForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetIC_UnitForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetIC_UnitForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetIC_UnitForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetIC_UnitForViewDto>(<any>null);
    }

    GetIC_UnitForView(id: number | null | undefined): Observable<GetIC_UNITForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/IC_UNITs/GetIC_UNITForEdit?";
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
            return this.processGetIC_UnitForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetIC_UnitForView(<any>response_);
                } catch (e) {
                    return <Observable<GetIC_UNITForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetIC_UNITForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetIC_UnitForView(response: HttpResponseBase): Observable<GetIC_UNITForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetIC_UNITForViewDto.fromJS(resultData200) : new GetIC_UNITForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetIC_UNITForViewDto>(<any>null);
    }

    GetIC_UNITForEdit(id: number | null | undefined): Observable<GetIC_UNITForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/IC_UNITs/GetIC_UNITForEdit?";
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
            return this.processGetIC_UNITForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    debugger
                    return this.processGetIC_UNITForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetIC_UNITForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetIC_UNITForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetIC_UNITForEdit(response: HttpResponseBase): Observable<GetIC_UNITForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetIC_UNITForEditOutput.fromJS(resultData200) : new GetIC_UNITForEditOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetIC_UNITForEditOutput>(<any>null);
    }

    createOrEdit(input: CreateOrEditIC_UNITDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/IC_UNITs/CreateOrEdit";
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

    delete(id: number | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/IC_UNITs/Delete?";
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

    GetUnitList(id: number | null | undefined): Observable<IC_UNITDto> {
        let url_ = this.baseUrl + "/api/services/app/IC_UNITs/Delete?";
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
            return this.processGetUnitList(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetUnitList(<any>response_);
                } catch (e) {
                    return <Observable<IC_UNITDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<IC_UNITDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetUnitList(response: HttpResponseBase): Observable<IC_UNITDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? IC_UNITDto.fromJS(resultData200) : new IC_UNITDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<IC_UNITDto>(<any>null);
    } 
}


export class PagedResultDtoOfGetIC_UnitForViewDto implements IPagedResultDtoOfGetIC_UnitForViewDto {
    totalCount!: number | undefined;
    items!: GetIC_UNITForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetIC_UnitForViewDto) {
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
                    this.items!.push(GetIC_UNITForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetIC_UnitForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetIC_UnitForViewDto();
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

export interface IPagedResultDtoOfGetIC_UnitForViewDto {
    totalCount: number | undefined;
    items: GetIC_UNITForViewDto[] | undefined;
}

export class GetIC_UNITForEditOutput implements  IGetIC_UNITForEditOutput{

    ic_Unit!: CreateOrEditIC_UNITDto[] | undefined 

    constructor(data?:IGetIC_UNITForEditOutput){
        if (data) {
            for (let property in data) {
                if (data.hasOwnProperty(property)) 
                    (<any>this)[property] = (<any>data)[property]
            }
        }
    }

    init(data?:any){
        if (data) {
            if (data["items"] && data["items"].constructor === Array) {
                this.ic_Unit = [] as any;
                for (let item of data["items"])
                    this.ic_Unit!.push(CreateOrEditIC_UNITDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any) : GetIC_UNITForEditOutput {
        data = typeof data === 'object' ? data : {}
        let result = new GetIC_UNITForEditOutput();
        result.init(data)
        return result;
    }

    toJSON(data?:any){
        data = typeof data === 'object' ? data : {};
        if (this.ic_Unit && this.ic_Unit.constructor === Array) {
            data["items"] = [];
            for (let item of this.ic_Unit)
                data["items"].push(item.toJSON());
        }
        return data; 
    }
}

export interface IGetIC_UNITForEditOutput {
    ic_Unit: CreateOrEditIC_UNITDto[] | undefined;
}

export class CreateOrEditIC_UNITDto implements ICreateOrEditIC_UNITDto {
    active!: boolean | undefined;
    itemId!: string | undefined
    conver!: number | undefined;
    unit!: string | undefined;
    id!: number | undefined;

    constructor (data?:ICreateOrEditIC_UNITDto){
        if (data) {
            for (let property in data ) {
             if (data.hasOwnProperty(property))
                (<any>this)[property] = (<any>data)[property]   
            }
        }

    }

    init(data?:any){
        if (data) {
            this.itemId = data["itemId"]
            this.unit = data["unit"];
            this.conver = data["conver"];
            this.active = data["active"];
            this.id = data["id"];
        }
    }

    static fromJS(data?: any): CreateOrEditIC_UNITDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditIC_UNITDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["itemId"] = this.itemId;
        data["unit"] = this.unit;
        data["conver"] = this.conver;
        data["active"] = this.active;
        data["id"] = this.id;
        return data;
    }
}

export interface ICreateOrEditIC_UNITDto {
    unit: string | undefined;
    itemId: string | undefined
    conver: number | undefined;
    active: boolean | undefined;
    id:number| undefined;
}

export class GetIC_UNITForViewDto implements IGetIC_UNITForViewDto {
    ic_Unit!: IC_UNITDto | undefined;

    constructor(data?:IGetIC_UNITForViewDto){
        if (data) {
            for (let property in data) {
                if (data.hasOwnProperty(property)) 
                    (<any>this)[property] = (<any>data)[property]
            }
        }
    }

    init(data?:any){
        if (data) {
            this.ic_Unit = data["ic_Unit"] ? IC_UNITDto.fromJS(data["ic_Unit"]) : <any>undefined;
        }
    }

    static fromJS(data: any) : GetIC_UNITForViewDto {
        data = typeof data === 'object' ? data : {}
        let result = new GetIC_UNITForViewDto();
        result.init(data)
        return result;
    }

    toJSON(data?:any){
        if (data) {
            data = typeof data === 'object' ? data : {};
            data["ic_Unit"] = this.ic_Unit ? this.ic_Unit.toJSON() : <any>undefined;
            return data;
        }
    }

    
}

export interface IGetIC_UNITForViewDto{
    ic_Unit: IC_UNITDto | undefined;
}

export class IC_UNITDto implements IIC_UNITDto {
    itemId!: string | undefined
    unit!: string | undefined;
    conver!: number| undefined;
    active!: boolean | undefined;
    id!:number | undefined;

    constructor(data?: IC_UNITDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property]
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.itemId = data["itemId"]
            this.unit = data["unit"];
            this.conver = data["conver"];
            this.active = data["active"];
            this.id = data["id"];
        }
    }

    static fromJS(data?: any): IC_UNITDto {
        data = typeof data === 'object' ? data : {};
        let result = new IC_UNITDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["itemId"] = this.itemId;
        data["unit"] = this.unit;
        data["conver"] = this.conver;
        data["active"] = this.active;
        data["id"] = this.id;
        return data;
    }

}

export interface IIC_UNITDto {
    itemId: string | undefined
    unit: string | undefined;
    conver: number | undefined;
    active: boolean | undefined;
    id:number| undefined;
}


