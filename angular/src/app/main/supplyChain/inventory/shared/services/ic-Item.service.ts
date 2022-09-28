import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient, HttpHeaders, HttpResponseBase, HttpResponse } from "@angular/common/http";
import { API_BASE_URL, blobToText, throwException } from "@shared/service-proxies/service-proxies";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { DateTimeService } from "@app/shared/common/timing/date-time.service";
import * as  moment from "moment";
import { CreateOrEditIC_UNITDto, IC_UNITDto, GetIC_UNITForEditOutput } from "./ic-units-service";
import { IndexedSlideList } from "ngx-bootstrap/carousel/models";

@Injectable({
    providedIn: 'root'
})
export class IcItemServiceProxy {


    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param itemIdFilter      (optional) 
     * @param descpFilter       (optional) 
     * @param seg1NameFilter      (optional) 
     * @param seg2NameFilter      (optional) 
     * @param seg3NameFilter      (optional) 
     * @param creationDateFilter (optional) 
     * @param itemCtgFilter       (optional) 
     * @param itemTypeFilter        (optional) 
     * @param itemStatusFilter      (optional) 
     * @param stockUnitFilter       (optional) 
     * @param packingFilter         (optional) 
     * @param weightFilter      (optional) 
     * @param taxableFilter         (optional) 
     * @param saleableFilter        (optional) 
     * @param activeFilter      (optional) 
     * @param barcodeFilter         (optional) 
     * @return Success
     */
    getAll(filter: string | null | undefined, itemIdFilter: string | null | undefined, descpFilter: string | null | undefined, seg1NameFilter: string | null | undefined, seg2NameFilter: string | null | undefined, seg3NameFilter: string | null | undefined, itemCtgFilter: number | null | undefined, itemTypeFilter: number | null | undefined, itemStatusFilter: number | null | undefined, stockUnitFilter: string | null | undefined, packingFilter: number | null | undefined, weightFilter: number | null | undefined, taxableFilter: boolean | null | undefined, saleableFilter: boolean | null | undefined, activeFilter: boolean | null | undefined, barcodeFilter: string | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetICItemForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/ICItem/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (itemIdFilter !== undefined)
            url_ += "ItemIdFilter=" + encodeURIComponent("" + itemIdFilter) + "&";
        if (descpFilter !== undefined)
            url_ += "DescpFilter=" + encodeURIComponent("" + descpFilter) + "&";
        if (seg1NameFilter !== undefined)
            url_ += "Seg1NameFilter=" + encodeURIComponent("" + seg1NameFilter) + "&";
        if (seg2NameFilter !== undefined)
            url_ += "Seg2NameFilter=" + encodeURIComponent("" + seg2NameFilter) + "&";
        if (seg3NameFilter !== undefined)
            url_ += "Seg3NameFilter=" + encodeURIComponent("" + seg3NameFilter) + "&";
        if (itemCtgFilter !== undefined)
            url_ += "ItemCtgFilter=" + encodeURIComponent("" + itemCtgFilter) + "&";
        if (itemTypeFilter !== undefined)
            url_ += "ItemTypeFilter=" + encodeURIComponent("" + itemTypeFilter) + "&";
        if (itemStatusFilter !== undefined)
            url_ += "ItemStatusFilter=" + encodeURIComponent("" + itemStatusFilter) + "&";
        if (stockUnitFilter !== undefined)
            url_ += "StockUnitFilter=" + encodeURIComponent("" + stockUnitFilter) + "&";
        if (packingFilter !== undefined)
            url_ += "PackingFilter=" + encodeURIComponent("" + packingFilter) + "&";
        if (weightFilter !== undefined)
            url_ += "WeightFilter=" + encodeURIComponent("" + weightFilter) + "&";
        if (taxableFilter !== undefined)
            url_ += "TaxableFilter=" + encodeURIComponent("" + taxableFilter) + "&";
        if (saleableFilter !== undefined)
            url_ += "SaleableFilter=" + encodeURIComponent("" + saleableFilter) + "&";
        if (activeFilter !== undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&";
        if (barcodeFilter !== undefined)
            url_ += "BarcodeFilter=" + encodeURIComponent("" + barcodeFilter) + "&";
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
                    return <Observable<PagedResultDtoOfGetICItemForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetICItemForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetICItemForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetICItemForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetICItemForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetICItemForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    GetIcItemForView(id: number | null | undefined): Observable<GetICItemForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/ICItem/GetIcItemForView?";
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
            return this.processGetIcItemForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetIcItemForView(<any>response_);
                } catch (e) {
                    return <Observable<GetICItemForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetICItemForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetIcItemForView(response: HttpResponseBase): Observable<GetICItemForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 300) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetICItemForViewDto.fromJS(resultData200) : new GetICItemForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetICItemForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    GetIcItemForEdit(id: number | null | undefined): Observable<GetICItemForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/ICItem/GetIcItemForEdit?";
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
            return this.processGetIcItemForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetIcItemForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetICItemForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetICItemForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetIcItemForEdit(response: HttpResponseBase): Observable<GetICItemForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetICItemForEditOutput.fromJS(resultData200) : new GetICItemForEditOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetICItemForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditICItemDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/ICItem/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/ICItem/Delete?";
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
     * @param seg3IdFilter (optional) 
     * @param seg3NameFilter (optional) 
     * @param seg2NameFilter (optional) 
     * @param seg1NameFilter (optional) 
     * @return Success
     */
    GetIcItemToExcel(filter: string | null | undefined, itemIdFilter: string | null | undefined, descpFilter: string | null | undefined, seg1NameFilter: string | null | undefined, seg2NameFilter: string | null | undefined, seg3NameFilter: string | null | undefined, itemCtgFilter: number | null | undefined, itemTypeFilter: number | null | undefined, itemStatusFilter: number | null | undefined, stockUnitFilter: string | null | undefined, packingFilter: number | null | undefined, weightFilter: number | null | undefined, taxableFilter: boolean | null | undefined, saleableFilter: boolean | null | undefined, activeFilter: boolean | null | undefined, barcodeFilter: string | null | undefined, ): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/ICItem/GetIcItemToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (itemIdFilter !== undefined)
            url_ += "ItemIdFilter=" + encodeURIComponent("" + itemIdFilter) + "&";
        if (descpFilter !== undefined)
            url_ += "DescpFilter=" + encodeURIComponent("" + descpFilter) + "&";
        if (seg1NameFilter !== undefined)
            url_ += "Seg1NameFilter=" + encodeURIComponent("" + seg1NameFilter) + "&";
        if (seg2NameFilter !== undefined)
            url_ += "Seg2NameFilter=" + encodeURIComponent("" + seg2NameFilter) + "&";
        if (seg3NameFilter !== undefined)
            url_ += "Seg3NameFilter=" + encodeURIComponent("" + seg3NameFilter) + "&";
        if (itemCtgFilter !== undefined)
            url_ += "ItemCtgFilter=" + encodeURIComponent("" + itemCtgFilter) + "&";
        if (itemTypeFilter !== undefined)
            url_ += "ItemTypeFilter=" + encodeURIComponent("" + itemTypeFilter) + "&";
        if (itemStatusFilter !== undefined)
            url_ += "ItemStatusFilter=" + encodeURIComponent("" + itemStatusFilter) + "&";
        if (stockUnitFilter !== undefined)
            url_ += "StockUnitFilter=" + encodeURIComponent("" + stockUnitFilter) + "&";
        if (packingFilter !== undefined)
            url_ += "PackingFilter=" + encodeURIComponent("" + packingFilter) + "&";
        if (weightFilter !== undefined)
            url_ += "WeightFilter=" + encodeURIComponent("" + weightFilter) + "&";
        if (taxableFilter !== undefined)
            url_ += "TaxableFilter=" + encodeURIComponent("" + taxableFilter) + "&";
        if (saleableFilter !== undefined)
            url_ += "SaleableFilter=" + encodeURIComponent("" + saleableFilter) + "&";
        if (activeFilter !== undefined)
            url_ += "ActiveFilter=" + encodeURIComponent("" + activeFilter) + "&";
        if (barcodeFilter !== undefined)
            url_ += "BarcodeFilter=" + encodeURIComponent("" + barcodeFilter) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetIcItemToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetIcItemToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetIcItemToExcel(response: HttpResponseBase): Observable<FileDto> {
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

    /**
     * @return Success
     */
    GetIcItemMaxId(seg3ID: string): Observable<number> {
        let url_ = this.baseUrl + "/api/services/app/ICItem/GetIcItemMaxId?";
        if (seg3ID !== undefined)
            url_ += "Seg3ID=" + encodeURIComponent("" + seg3ID) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetIcItemMaxId(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetIcItemMaxId(<any>response_);
                } catch (e) {
                    return <Observable<number>><any>_observableThrow(e);
                }
            } else
                return <Observable<number>><any>_observableThrow(response_);
        }));
    }

    protected processGetIcItemMaxId(response: HttpResponseBase): Observable<number> {
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

    updateItemPicture(input: UpdateItemPictureInput | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/ICItem/UpdateItemPicture";
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

        return this.http.request("put", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processupdateItemPicture(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processupdateItemPicture(<any>response_);
                } catch (e) {
                    return <Observable<void>><any>_observableThrow(e);
                }
            } else
                return <Observable<void>><any>_observableThrow(response_);
        }));
    }

    protected processupdateItemPicture(response: HttpResponseBase): Observable<void> {
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

    getItemPicture(itemId: string | null | undefined): Observable<GetItemPictureOutput> {
        let url_ = this.baseUrl + "/api/services/app/ICItem/GetItemPicture?";
        if (itemId !== undefined)
            url_ += "ItemID=" + encodeURIComponent("" + itemId) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetItemPicture(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetItemPicture(<any>response_);
                } catch (e) {
                    return <Observable<GetItemPictureOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetItemPictureOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetItemPicture(response: HttpResponseBase): Observable<GetItemPictureOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetItemPictureOutput.fromJS(resultData200) : new GetItemPictureOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetItemPictureOutput>(<any>null);
    }


}


export class PagedResultDtoOfGetICItemForViewDto implements IPagedResultDtoOfGetICItemForViewDto {
    totalCount!: number | undefined;
    items!: GetICItemForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetICItemForViewDto) {
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
                    this.items!.push(GetICItemForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetICItemForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetICItemForViewDto();
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

export interface IPagedResultDtoOfGetICItemForViewDto {
    totalCount: number | undefined;
    items: GetICItemForViewDto[] | undefined;
}

export class GetICItemForViewDto implements IGetICItemForViewDto {
    icItem!: ICItemDto | undefined;

    constructor(data?: IGetICItemForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.icItem = data["icItem"] ? ICItemDto.fromJS(data["icItem"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetICItemForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetICItemForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["icItem"] = this.icItem ? this.icItem.toJSON() : <any>undefined;
        return data;
    }
}

export interface IGetICItemForViewDto {
    icItem: ICItemDto | undefined;
}

export class ICItemDto implements IICItemDto {
    seg1Id!: string;
    seg2Id!: string;
    seg3Id!: string;
    itemId!: string
    alternateItemID!: string | undefined;
    descp!: string | undefined;
    creationDate!: moment.Moment | undefined;
    itemCtg!: number | undefined;
    itemType!: number | undefined;
    itemStatus!: number | undefined;
    stockUnit!: string | undefined;
    packing!: number | undefined;
    weight!: number | undefined;
    taxable!: boolean | undefined;
    saleable!: boolean | undefined;
    active!: boolean | undefined;
    barcode!: string | undefined;
    altItemID!: string | undefined;
    altDescp!: string | undefined;
    opt1!: string | undefined;
    opt2!: string | undefined;
    opt3!: string | undefined;
    opt4!: number | undefined;
    opt5!: number | undefined;
    defPriceList!: string | undefined;
    defVendorAC!: string | undefined;
    defVendorID!: number | undefined;
    defCustAC!: string | undefined;
    defCustID!: number | undefined;
    defTaxAuth!: string | undefined;
    defTaxClassID!: number | undefined;
    picture!: string | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    conver!: number | undefined;
    itemSrNo!: string | undefined;
    venitemcode!: string | undefined;
    venSrNo!: string | undefined;
    venLotNo!: string | undefined;
    manufectureDate!: moment.Moment | undefined;
    expirydate!: moment.Moment | undefined;
    warrantyinfo!: string | undefined;
    id!: number | undefined;
    constructor(data?: IICItemDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.seg1Id = data["seg1Id"];
            this.seg2Id = data["seg2Id"];
            this.seg3Id = data["seg3Id"];
            this.itemId = data["itemId"];
            this.alternateItemID = data["alternateItemID"];
            this.descp = data["descp"];
            this.creationDate = data["creationDate"] ? moment(data["creationDate"].toString()) : <any>undefined;
            this.itemCtg = data["itemCtg"];
            this.itemType = data["itemType"];
            this.itemStatus = data["itemStatus"];
            this.stockUnit = data["stockUnit"];
            this.packing = data["packing"];
            this.weight = data["weight"];
            this.taxable = data["taxable"];
            this.saleable = data["saleable"];
            this.active = data["active"];
            this.barcode = data["barcode"];
            this.altItemID = data["altItemID"];
            this.altDescp = data["altDescp"];
            this.opt1 = data["opt1"];
            this.opt2 = data["opt2"];
            this.opt3 = data["opt3"];
            this.opt4 = data["opt4"];
            this.opt5 = data["opt5"];
            this.defPriceList = data["defPriceList"];
            this.defVendorAC = data["defVendorAC"];
            this.defVendorID = data["defVendorID"];
            this.defCustAC = data["defCustAC"];
            this.defCustID = data["defCustID"];
            this.defTaxAuth = data["defTaxAuth"];
            this.defTaxClassID = data["defTaxClassID"];
            this.picture = data["picture"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.conver = data["conver"];
            this.itemSrNo = data["itemSrNo"];
            this.venitemcode = data["venitemcode"];
            this.venSrNo = data["venSrNo"];
            this.venLotNo = data["venLotNo"];
            this.manufectureDate = data["manufectureDate"] ? moment(data["manufectureDate"].toString()) : <any>undefined;
            this.expirydate = data["expirydate"] ? moment(data["expirydate"].toString()) : <any>undefined;
            this.warrantyinfo = data["warrantyinfo"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): ICItemDto {
        data = typeof data === 'object' ? data : {};
        let result = new ICItemDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["seg1Id"] = this.seg1Id;
        data["seg2Id"] = this.seg2Id;
        data["seg3Id"] = this.seg3Id;
        data["ItemId"] = this.itemId;
        data["alternateItemID"] = this.alternateItemID;
        data["descp"] = this.descp;
        data["creationDate"] = this.creationDate ? this.creationDate.toISOString() : <any>undefined;
        data["itemCtg"] = this.itemCtg;
        data["itemType"] = this.itemType;
        data["itemStatus"] = this.itemStatus;
        data["stockUnit"] = this.stockUnit;
        data["packing"] = this.packing;
        data["weight"] = this.weight;
        data["taxable"] = this.taxable;
        data["saleable"] = this.saleable;
        data["active"] = this.active;
        data["barcode"] = this.barcode;
        data["altItemID"] = this.altItemID;
        data["altDescp"] = this.altDescp;
        data["opt1"] = this.opt1;
        data["opt2"] = this.opt2;
        data["opt3"] = this.opt3;
        data["opt4"] = this.opt4;
        data["opt5"] = this.opt5;
        data["defPriceList"] = this.defPriceList;
        data["defVendorAC"] = this.defVendorAC;
        data["defVendorID"] = this.defVendorID;
        data["defCustAC"] = this.defCustAC;
        data["defCustID"] = this.defCustID;
        data["defTaxAuth"] = this.defTaxAuth;
        data["defTaxClassID"] = this.defTaxClassID;
        data["picture"] = this.picture;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["conver"] = this.conver;
        data["itemSrNo"] = this.itemSrNo;
        data["venitemcode"] = this.venitemcode;
        data["venSrNo"] = this.venSrNo;
        data["venLotNo"] = this.venLotNo;
        data["manufectureDate"] = this.manufectureDate ? this.manufectureDate.toISOString() : <any>undefined;
        data["expirydate"] = this.expirydate ? this.expirydate.toISOString() : <any>undefined;
        data["warrantyinfo"] = this.warrantyinfo;
        data["id"] = this.id;
        return data;
    }
}

export interface IICItemDto {
    seg1Id: string;
    seg2Id: string;
    seg3Id: string;
    itemId: string
    alternateItemID: string | undefined;
    descp: string | undefined;
    creationDate: moment.Moment | undefined;
    itemCtg: number | undefined;
    itemType: number | undefined;
    itemStatus: number | undefined;
    stockUnit: string | undefined;
    packing: number | undefined;
    weight: number | undefined;
    taxable: boolean | undefined;
    saleable: boolean | undefined;
    active: boolean | undefined;
    barcode: string | undefined;
    altItemID: string | undefined;
    altDescp: string | undefined;
    opt1: string | undefined;
    opt2: string | undefined;
    opt3: string | undefined;
    opt4: number | undefined;
    opt5: number | undefined;
    defPriceList: string | undefined;
    defVendorAC: string | undefined;
    defVendorID: number | undefined;
    defCustAC: string | undefined;
    defCustID: number | undefined;
    defTaxAuth: string | undefined;
    defTaxClassID: number | undefined;
    picture: string | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    conver: number | undefined;
    itemSrNo: string | undefined;
    venitemcode: string | undefined;
    venSrNo: string | undefined;
    venLotNo: string | undefined;
    manufectureDate: moment.Moment | undefined;
    expirydate: moment.Moment | undefined;
    warrantyinfo: string | undefined;
    id: number | undefined;
    

}

export class GetICItemForEditOutput implements IGetICItemForEditOutput {
    icItem!: CreateOrEditICItemDto | undefined;
    seg1Name!: string | undefined;
    seg2Name!: string | undefined;
    seg3Name!: string | undefined;
    DefTaxAuthDesc!: string | undefined;
    defVendorAccDesc!: string | undefined;
    defCustomerAccDesc!: string | undefined;
    option4Desc!: string | undefined;
    option5Desc!: string | undefined;

    constructor(data?: IGetICItemForEditOutput) {
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
            this.seg1Name = data["seg1Name"];
            this.seg2Name = data["seg2Name"];
            this.seg3Name = data["seg3Name"];
            this.option4Desc = data["option4Desc"];
            this.option5Desc = data["option5Desc"];
            this.DefTaxAuthDesc = data["defTaxAuthDesc"];
            this.defVendorAccDesc = data["defVendorAccDesc"];
            this.defCustomerAccDesc = data["defCustomerAccDesc"];
            this.icItem = data["icItem"] ? CreateOrEditICItemDto.fromJS(data["icItem"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetICItemForEditOutput {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new GetICItemForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["seg1Name"] = this.seg1Name;
        data["seg2Name"] = this.seg2Name;
        data["seg3Name"] = this.seg3Name;
        data["option4Desc"] = this.option4Desc;
        data["option5Desc"] = this.option5Desc;
        data["DefTaxAuthDesc"] = this.DefTaxAuthDesc;
        data["defVendorAccDesc"] = this.defVendorAccDesc;
        data["defCustomerAccDesc"] = this.defCustomerAccDesc;


        data["icItem"] = this.icItem ? this.icItem.toJSON() : <any>undefined;
        return data;
    }
}

export interface IGetICItemForEditOutput {
    icItem: CreateOrEditICItemDto | undefined;
    seg1Name: string | undefined;
    seg2Name: string | undefined;
    seg3Name: string | undefined;
    DefTaxAuthDesc: string | undefined;
    defVendorAccDesc: string | undefined;
    defCustomerAccDesc: string | undefined;
    option4Desc: string | undefined;
    option5Desc: string | undefined;


}

export class CreateOrEditICItemDto implements ICreateOrEditICItemDto {
    seg1Id!: string;
    seg2Id!: string;
    seg3Id!: string;
    flag !: boolean | undefined;
    itemId!: string
    alternateItemID!: string | undefined;
    descp!: string;
    creationDate!: moment.Moment | undefined;
    itemCtg!: number | undefined;
    itemType!: number | undefined;
    itemStatus!: number | undefined;
    stockUnit!: string | undefined;
    packing!: number | undefined;
    weight!: number | undefined;
    taxable!: boolean | undefined;
    saleable!: boolean | undefined;
    active!: boolean | undefined;
    barcode!: string | undefined;
    altItemID!: string | undefined;
    altDescp!: string | undefined;
    opt1!: string | undefined;
    opt2!: string | undefined;
    opt3!: string | undefined;
    opt4!: number | undefined;
    opt5!: number | undefined;
    defPriceList!: string | undefined;
    defVendorAC!: string | undefined;
    defVendorID!: number | undefined;
    defCustAC!: string | undefined;
    defCustID!: number | undefined;
    defTaxAuth!: string | undefined;
    defTaxClassID!: number | undefined;
    picture!: string | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    conver!: number | undefined;
    itemSrNo!: string | undefined;
    venitemcode!: string | undefined;
    venSrNo!: string | undefined;
    venLotNo!: string | undefined;
    manufectureDate!: moment.Moment | undefined;
    expirydate!: moment.Moment | undefined;
    warrantyinfo!: string | undefined;
    id!: number | undefined;
    
    iC_Units!: CreateOrEditIC_UNITDto[] | undefined
    constructor(data?: ICreateOrEditICItemDto) {
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
            this.seg1Id = data["seg1Id"];
            this.seg2Id = data["seg2Id"];
            this.seg3Id = data["seg3Id"];
            this.flag = data["flag"];
            this.itemId = data["itemId"];
            this.alternateItemID = data["alternateItemID"];
            this.descp = data["descp"];
            this.creationDate = data["creationDate"] ? moment(data["creationDate"].toString()) : <any>undefined;
            this.itemCtg = data["itemCtg"];
            this.itemType = data["itemType"];
            this.itemStatus = data["itemStatus"];
            this.stockUnit = data["stockUnit"];
            this.packing = data["packing"];
            this.weight = data["weight"];
            this.taxable = data["taxable"];
            this.saleable = data["saleable"];
            this.active = data["active"];
            this.barcode = data["barcode"];
            this.altItemID = data["altItemID"];
            this.altDescp = data["altDescp"];
            this.opt1 = data["opt1"];
            this.opt2 = data["opt2"];
            this.opt3 = data["opt3"];
            this.opt4 = data["opt4"];
            this.opt5 = data["opt5"];
            this.defPriceList = data["defPriceList"];
            this.defVendorAC = data["defVendorAC"];
            this.defVendorID = data["defVendorID"];
            this.defCustAC = data["defCustAC"];
            this.defCustID = data["defCustID"];
            this.defTaxAuth = data["defTaxAuth"];
            this.defTaxClassID = data["defTaxClassID"];
            this.picture = data["picture"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.conver = data["conver"];
            this.itemSrNo = data["itemSrNo"];
            this.venitemcode = data["venitemcode"];
            this.venSrNo = data["venSrNo"];
            this.venLotNo = data["venLotNo"];
            this.manufectureDate = data["manufectureDate"] ? moment(data["manufectureDate"].toString()) : <any>undefined;
            this.expirydate = data["expirydate"] ? moment(data["expirydate"].toString()) : <any>undefined;
            this.warrantyinfo = data["warrantyinfo"];
            this.id = data["id"];



            this.iC_Units = [] as any;
            for (let item of data["iC_Units"])
                this.iC_Units!.push(CreateOrEditIC_UNITDto.fromJS(item));


        }
    }

    static fromJS(data: any): CreateOrEditICItemDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditICItemDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger
        data = typeof data === 'object' ? data : {};
        data["seg1Id"] = this.seg1Id;
        data["seg2Id"] = this.seg2Id;
        data["seg3Id"] = this.seg3Id;
        data["flag"] = this.flag;
        data["ItemId"] = this.itemId;
        data["alternateItemID"] = this.alternateItemID;
        data["descp"] = this.descp;
        data["creationDate"] = this.creationDate ? this.creationDate.toISOString() : <any>undefined;
        data["itemCtg"] = this.itemCtg;
        data["itemType"] = this.itemType;
        data["itemStatus"] = this.itemStatus;
        data["stockUnit"] = this.stockUnit;
        data["packing"] = this.packing;
        data["weight"] = this.weight;
        data["taxable"] = this.taxable;
        data["saleable"] = this.saleable;
        data["active"] = this.active;
        data["barcode"] = this.barcode;
        data["altItemID"] = this.altItemID;
        data["altDescp"] = this.altDescp;
        data["opt1"] = this.opt1;
        data["opt2"] = this.opt2;
        data["opt3"] = this.opt3;
        data["opt4"] = this.opt4;
        data["opt5"] = this.opt5;
        data["defPriceList"] = this.defPriceList;
        data["defVendorAC"] = this.defVendorAC;
        data["defVendorID"] = this.defVendorID;
        data["defCustAC"] = this.defCustAC;
        data["defCustID"] = this.defCustID;
        data["defTaxAuth"] = this.defTaxAuth;
        data["defTaxClassID"] = this.defTaxClassID;
        data["picture"] = this.picture;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["conver"] = this.conver;
        data["itemSrNo"] = this.itemSrNo;
        data["venitemcode"] = this.venitemcode;
        data["venSrNo"] = this.venSrNo;
        data["venLotNo"] = this.venLotNo;
        data["manufectureDate"] = this.manufectureDate ? this.manufectureDate.toISOString() : <any>undefined;
        data["expirydate"] = this.expirydate ? this.expirydate.toISOString() : <any>undefined;
        data["warrantyinfo"] = this.warrantyinfo;
        data["id"] = this.id;
        
        


        if (this.iC_Units && this.iC_Units.constructor === Array) {
            data["iC_Units"] = [];
            for (let item of this.iC_Units)
                data["iC_Units"].push(item.toJSON());
        }
        return data;
    }
}

export interface ICreateOrEditICItemDto {
    seg1Id: string;
    seg2Id: string;
    seg3Id: string;
    flag: boolean | undefined;
    itemId: string
    alternateItemID: string | undefined;
    descp: string;
    creationDate: moment.Moment | undefined;
    itemCtg: number | undefined;
    itemType: number | undefined;
    itemStatus: number | undefined;
    stockUnit: string | undefined;
    packing: number | undefined;
    weight: number | undefined;
    taxable: boolean | undefined;
    saleable: boolean | undefined;
    active: boolean | undefined;
    barcode: string | undefined;
    altItemID: string | undefined;
    altDescp: string | undefined;
    opt1: string | undefined;
    opt2: string | undefined;
    opt3: string | undefined;
    opt4: number | undefined;
    opt5: number | undefined;
    defPriceList: string | undefined;
    defVendorAC: string | undefined;
    defVendorID: number | undefined;
    defCustAC: string | undefined;
    defCustID: number | undefined;
    defTaxAuth: string | undefined;
    defTaxClassID: number | undefined;
    picture: string | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    conver: number | undefined;
    itemSrNo: string | undefined;
    venitemcode: string | undefined;
    venSrNo: string | undefined;
    venLotNo: string | undefined;
    manufectureDate: moment.Moment | undefined;
    expirydate: moment.Moment | undefined;
    warrantyinfo: string | undefined;
    id: number | undefined;
    
    iC_Units: CreateOrEditIC_UNITDto[] | undefined
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

export class UpdateItemPictureInput implements IUpdateItemPictureInput {
    fileToken!: string;
    x!: number | undefined;
    y!: number | undefined;
    width!: number | undefined;
    height!: number | undefined;
    itemID!: string | undefined;

    constructor(data?: IUpdateItemPictureInput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.fileToken = data["fileToken"];
            this.x = data["x"];
            this.y = data["y"];
            this.width = data["width"];
            this.height = data["height"];
            this.itemID = data["itemID"];
        }
    }

    static fromJS(data: any): UpdateItemPictureInput {
        data = typeof data === 'object' ? data : {};
        let result = new UpdateItemPictureInput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["fileToken"] = this.fileToken;
        data["x"] = this.x;
        data["y"] = this.y;
        data["width"] = this.width;
        data["height"] = this.height;
        data["itemID"] = this.itemID;
        return data;
    }
}

export interface IUpdateItemPictureInput {
    fileToken: string;
    x: number | undefined;
    y: number | undefined;
    width: number | undefined;
    height: number | undefined;
    itemID: string | undefined;
}

export class GetItemPictureOutput implements IGetItemPictureOutput {
    itemPicture!: string | undefined;

    constructor(data?: IGetItemPictureOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.itemPicture = data["itemPicture"];
        }
    }

    static fromJS(data: any): GetItemPictureOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetItemPictureOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["itemPicture"] = this.itemPicture;
        return data;
    }
}

export interface IGetItemPictureOutput {
    itemPicture: string | undefined;
}


