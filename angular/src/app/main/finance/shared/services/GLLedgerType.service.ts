import { Inject, Optional, Injectable } from "@angular/core";
import { HttpClient, HttpResponseBase, HttpResponse, HttpHeaders } from "@angular/common/http";
import { API_BASE_URL, blobToText, throwException, FileDto, ComboboxItemDto } from "@shared/service-proxies/service-proxies";
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';

@Injectable()
export class LedgerTypesServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param ledgerIDFilter (optional) 
     * @param ledgerDescFilter (optional) 
     * @param activeFilter (optional) 
     * @param sorting (optional) 
     * @param skipCount (optional) 
     * @param maxResultCount (optional) 
     * @return Success
     */
    getAll(filter: string | null | undefined, ledgerIDFilter: number | null | undefined, ledgerDescFilter: string | null | undefined, activeFilter: number | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetLedgerTypeForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/LedgerTypes/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (ledgerIDFilter !== undefined)
            url_ += "LedgerIDFilter=" + encodeURIComponent("" + ledgerIDFilter) + "&"; 
        if (ledgerDescFilter !== undefined)
            url_ += "LedgerDescFilter=" + encodeURIComponent("" + ledgerDescFilter) + "&"; 
        if (activeFilter !== undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&"; 
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
                    return <Observable<PagedResultDtoOfGetLedgerTypeForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetLedgerTypeForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetLedgerTypeForViewDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfGetLedgerTypeForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetLedgerTypeForViewDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetLedgerTypeForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getLedgerTypeForView(id: number | null | undefined): Observable<GetLedgerTypeForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/LedgerTypes/GetLedgerTypeForView?";
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
            return this.processGetLedgerTypeForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetLedgerTypeForView(<any>response_);
                } catch (e) {
                    return <Observable<GetLedgerTypeForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetLedgerTypeForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetLedgerTypeForView(response: HttpResponseBase): Observable<GetLedgerTypeForViewDto> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GetLedgerTypeForViewDto.fromJS(resultData200) : new GetLedgerTypeForViewDto();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetLedgerTypeForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getLedgerTypeForEdit(id: number | null | undefined): Observable<GetLedgerTypeForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/LedgerTypes/GetLedgerTypeForEdit?";
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
            return this.processGetLedgerTypeForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetLedgerTypeForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetLedgerTypeForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetLedgerTypeForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetLedgerTypeForEdit(response: HttpResponseBase): Observable<GetLedgerTypeForEditOutput> {
        const status = response.status;
        const responseBlob = 
            response instanceof HttpResponse ? response.body : 
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GetLedgerTypeForEditOutput.fromJS(resultData200) : new GetLedgerTypeForEditOutput();
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetLedgerTypeForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditLedgerTypeDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/LedgerTypes/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/LedgerTypes/Delete?";
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
     * @param ledgerIDFilter (optional) 
     * @param ledgerDescFilter (optional) 
     * @param activeFilter (optional) 
     * @return Success
     */
    getLedgerTypesToExcel(filter: string | null | undefined, ledgerIDFilter: number | null | undefined, ledgerDescFilter: string | null | undefined, activeFilter: number | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/LedgerTypes/GetLedgerTypesToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&"; 
        if (ledgerIDFilter !== undefined)
            url_ += "LedgerIDFilter=" + encodeURIComponent("" + ledgerIDFilter) + "&"; 
        if (ledgerDescFilter !== undefined)
            url_ += "LedgerDescFilter=" + encodeURIComponent("" + ledgerDescFilter) + "&"; 
        if (activeFilter !== undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&"; 
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetLedgerTypesToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetLedgerTypesToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetLedgerTypesToExcel(response: HttpResponseBase): Observable<FileDto> {
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

    getMaxLedgerTypeID(): Observable<number> {
        debugger;
        let url_ = this.baseUrl + "/api/services/app/LedgerTypes/GetMaxLedgerTypeID";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetMaxLedgerTypeID(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetMaxLedgerTypeID(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processGetMaxLedgerTypeID(response: HttpResponseBase): Observable<number> {
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

    // getLedgerTypesForCombobox(input: string | null | undefined): Observable<ListResultDtoOfComboboxItemDto> {
    //     let url_ = this.baseUrl + "/api/services/app/LedgerTypes/GetLedgerTypesForCombobox?";
    //     if (input !== undefined)
    //         url_ += "input=" + encodeURIComponent("" + input) + "&"; 
    //     url_ = url_.replace(/[?&]$/, "");

    //     let options_ : any = {
    //         observe: "response",
    //         responseType: "blob",
    //         headers: new HttpHeaders({
    //             "Accept": "application/json"
    //         })
    //     };

    //     return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
    //         return this.processGetLedgerTypesForCombobox(response_);
    //     })).pipe(_observableCatch((response_: any) => {
    //         if (response_ instanceof HttpResponseBase) {
    //             try {
    //                 return this.processGetLedgerTypesForCombobox(<any>response_);
    //             } catch (e) {
    //                 return <Observable<ListResultDtoOfComboboxItemDto>><any>_observableThrow(e);
    //             }
    //         } else
    //             return <Observable<ListResultDtoOfComboboxItemDto>><any>_observableThrow(response_);
    //     }));
    // }

    // protected processGetLedgerTypesForCombobox(response: HttpResponseBase): Observable<ListResultDtoOfComboboxItemDto> {
    //     const status = response.status;
    //     const responseBlob = 
    //         response instanceof HttpResponse ? response.body : 
    //         (<any>response).error instanceof Blob ? (<any>response).error : undefined;

    //     let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
    //     if (status === 200) {
    //         return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
    //         let result200: any = null;
    //         let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
    //         result200 = resultData200 ? ListResultDtoOfComboboxItemDto.fromJS(resultData200) : new ListResultDtoOfComboboxItemDto();
    //         return _observableOf(result200);
    //         }));
    //     } else if (status !== 200 && status !== 204) {
    //         return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
    //         return throwException("An unexpected server error occurred.", status, _responseText, _headers);
    //         }));
    //     }
    //     return _observableOf<ListResultDtoOfComboboxItemDto>(<any>null);
    // }
}

export class PagedResultDtoOfGetLedgerTypeForViewDto implements IPagedResultDtoOfGetLedgerTypeForViewDto {
    totalCount!: number | undefined;
    items!: GetLedgerTypeForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetLedgerTypeForViewDto) {
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
                    this.items!.push(GetLedgerTypeForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetLedgerTypeForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetLedgerTypeForViewDto();
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

export interface IPagedResultDtoOfGetLedgerTypeForViewDto {
    totalCount: number | undefined;
    items: GetLedgerTypeForViewDto[] | undefined;
}

export class GetLedgerTypeForViewDto implements IGetLedgerTypeForViewDto {
    ledgerType!: LedgerTypeDto | undefined;

    constructor(data?: IGetLedgerTypeForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.ledgerType = data["ledgerType"] ? LedgerTypeDto.fromJS(data["ledgerType"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetLedgerTypeForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetLedgerTypeForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["ledgerType"] = this.ledgerType ? this.ledgerType.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IGetLedgerTypeForViewDto {
    ledgerType: LedgerTypeDto | undefined;
}

export class LedgerTypeDto implements ILedgerTypeDto {
    ledgerID!: number | undefined;
    ledgerDesc!: string | undefined;
    active!: boolean | undefined;
    id!: number | undefined;

    constructor(data?: ILedgerTypeDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.ledgerID = data["ledgerID"];
            this.ledgerDesc = data["ledgerDesc"];
            this.active = data["active"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): LedgerTypeDto {
        data = typeof data === 'object' ? data : {};
        let result = new LedgerTypeDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["ledgerID"] = this.ledgerID;
        data["ledgerDesc"] = this.ledgerDesc;
        data["active"] = this.active;
        data["id"] = this.id;
        return data; 
    }
}

export interface ILedgerTypeDto {
    ledgerID: number | undefined;
    ledgerDesc: string | undefined;
    active: boolean | undefined;
    id: number | undefined;
}

export class GetLedgerTypeForEditOutput implements IGetLedgerTypeForEditOutput {
    ledgerType!: CreateOrEditLedgerTypeDto | undefined;

    constructor(data?: IGetLedgerTypeForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.ledgerType = data["ledgerType"] ? CreateOrEditLedgerTypeDto.fromJS(data["ledgerType"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetLedgerTypeForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetLedgerTypeForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["ledgerType"] = this.ledgerType ? this.ledgerType.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IGetLedgerTypeForEditOutput {
    ledgerType: CreateOrEditLedgerTypeDto | undefined;
}

export class CreateOrEditLedgerTypeDto implements ICreateOrEditLedgerTypeDto {
    ledgerID!: number | undefined;
    ledgerDesc!: string;
    active!: boolean | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditLedgerTypeDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.ledgerID = data["ledgerID"];
            this.ledgerDesc = data["ledgerDesc"];
            this.active = data["active"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditLedgerTypeDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditLedgerTypeDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["ledgerID"] = this.ledgerID;
        data["ledgerDesc"] = this.ledgerDesc;
        data["active"] = this.active;
        data["id"] = this.id;
        return data; 
    }
}

export interface ICreateOrEditLedgerTypeDto {
    ledgerID: number | undefined;
    ledgerDesc: string;
    active: boolean | undefined;
    id: number | undefined;
}
// export class ListResultDtoOfComboboxItemDto implements IListResultDtoOfComboboxItemDto {
//     items!: ComboboxItemDto[] | undefined;

//     constructor(data?: IListResultDtoOfComboboxItemDto) {
//         if (data) {
//             for (var property in data) {
//                 if (data.hasOwnProperty(property))
//                     (<any>this)[property] = (<any>data)[property];
//             }
//         }
//     }

//     init(data?: any) {
//         if (data) {
//             if (data["items"] && data["items"].constructor === Array) {
//                 this.items = [] as any;
//                 for (let item of data["items"])
//                     this.items!.push(ComboboxItemDto.fromJS(item));
//             }
//         }
//     }

//     static fromJS(data: any): ListResultDtoOfComboboxItemDto {
//         data = typeof data === 'object' ? data : {};
//         let result = new ListResultDtoOfComboboxItemDto();
//         result.init(data);
//         return result;
//     }

//     toJSON(data?: any) {
//         data = typeof data === 'object' ? data : {};
//         if (this.items && this.items.constructor === Array) {
//             data["items"] = [];
//             for (let item of this.items)
//                 data["items"].push(item.toJSON());
//         }
//         return data; 
//     }
// }

// export interface IListResultDtoOfComboboxItemDto {
//     items: ComboboxItemDto[] | undefined;
// }
