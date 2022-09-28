import { Injectable, Inject, Optional } from "@angular/core";
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { HttpClient, HttpHeaders, HttpResponseBase, HttpResponse } from "@angular/common/http";
import { API_BASE_URL, blobToText, throwException, FileDto } from "@shared/service-proxies/service-proxies";
import * as  moment from "moment";
import { CreateOrEditAssetRegistrationDetailDto } from "./assetRegistrationDetail.service";

@Injectable()
export class AssetRegistrationsServiceProxy {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    /**
     * @param filter (optional) 
     * @param maxAssetIDFilter (optional) 
     * @param minAssetIDFilter (optional) 
     * @param fmtAssetIDFilter (optional) 
     * @param assetNameFilter (optional) 
     * @param itemIDFilter (optional) 
     * @param maxLocIDFilter (optional) 
     * @param minLocIDFilter (optional) 
     * @param maxRegDateFilter (optional) 
     * @param minRegDateFilter (optional) 
     * @param maxPurchaseDateFilter (optional) 
     * @param minPurchaseDateFilter (optional) 
     * @param maxExpiryDateFilter (optional) 
     * @param minExpiryDateFilter (optional) 
     * @param warrantyFilter (optional) 
     * @param maxAssetTypeFilter (optional) 
     * @param minAssetTypeFilter (optional) 
     * @param maxDepRateFilter (optional) 
     * @param minDepRateFilter (optional) 
     * @param maxDepMethodFilter (optional) 
     * @param minDepMethodFilter (optional) 
     * @param serialNumberFilter (optional) 
     * @param maxPurchasePriceFilter (optional) 
     * @param minPurchasePriceFilter (optional) 
     * @param narrationFilter (optional) 
     * @param accAssetFilter (optional) 
     * @param accDeprFilter (optional) 
     * @param accExpFilter (optional) 
     * @param maxDepStartDateFilter (optional) 
     * @param minDepStartDateFilter (optional) 
     * @param maxAssetLifeFilter (optional) 
     * @param minAssetLifeFilter (optional) 
     * @param maxBookValueFilter (optional) 
     * @param minBookValueFilter (optional) 
     * @param maxLastDepAmountFilter (optional) 
     * @param minLastDepAmountFilter (optional) 
     * @param maxLastDepDateFilter (optional) 
     * @param minLastDepDateFilter (optional) 
     * @param disolvedFilter (optional) 
     * @param maxActiveFilter (optional) 
     * @param minActiveFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @param sorting (optional) 
     * @param skipCount (optional) 
     * @param maxResultCount (optional) 
     * @return Success
     */
    getAll(filter: string | null | undefined, maxAssetIDFilter: number | null | undefined, minAssetIDFilter: number | null | undefined, fmtAssetIDFilter: string | null | undefined, assetNameFilter: string | null | undefined, itemIDFilter: string | null | undefined, maxLocIDFilter: number | null | undefined, minLocIDFilter: number | null | undefined, maxRegDateFilter: moment.Moment | null | undefined, minRegDateFilter: moment.Moment | null | undefined, maxPurchaseDateFilter: moment.Moment | null | undefined, minPurchaseDateFilter: moment.Moment | null | undefined, maxExpiryDateFilter: moment.Moment | null | undefined, minExpiryDateFilter: moment.Moment | null | undefined, warrantyFilter: number | null | undefined, AssetTypeFilter: number | null | undefined, maxDepRateFilter: number | null | undefined, minDepRateFilter: number | null | undefined, DepMethodFilter: number | null | undefined, serialNumberFilter: string | null | undefined, maxPurchasePriceFilter: number | null | undefined, minPurchasePriceFilter: number | null | undefined, narrationFilter: string | null | undefined, accAssetFilter: string | null | undefined, accDeprFilter: string | null | undefined, accExpFilter: string | null | undefined, maxDepStartDateFilter: moment.Moment | null | undefined, minDepStartDateFilter: moment.Moment | null | undefined, maxAssetLifeFilter: number | null | undefined, minAssetLifeFilter: number | null | undefined, maxBookValueFilter: number | null | undefined, minBookValueFilter: number | null | undefined, maxLastDepAmountFilter: number | null | undefined, minLastDepAmountFilter: number | null | undefined, maxLastDepDateFilter: moment.Moment | null | undefined, minLastDepDateFilter: moment.Moment | null | undefined, disolvedFilter: number | null | undefined, maxActiveFilter: number | null | undefined, minActiveFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined, sorting: string | null | undefined, skipCount: number | null | undefined, maxResultCount: number | null | undefined): Observable<PagedResultDtoOfGetAssetRegistrationForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/AssetRegistration/GetAll?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxAssetIDFilter !== undefined)
            url_ += "MaxAssetIDFilter=" + encodeURIComponent("" + maxAssetIDFilter) + "&";
        if (minAssetIDFilter !== undefined)
            url_ += "MinAssetIDFilter=" + encodeURIComponent("" + minAssetIDFilter) + "&";
        if (fmtAssetIDFilter !== undefined)
            url_ += "FmtAssetIDFilter=" + encodeURIComponent("" + fmtAssetIDFilter) + "&";
        if (assetNameFilter !== undefined)
            url_ += "AssetNameFilter=" + encodeURIComponent("" + assetNameFilter) + "&";
        if (itemIDFilter !== undefined)
            url_ += "ItemIDFilter=" + encodeURIComponent("" + itemIDFilter) + "&";
        if (maxLocIDFilter !== undefined)
            url_ += "MaxLocIDFilter=" + encodeURIComponent("" + maxLocIDFilter) + "&";
        if (minLocIDFilter !== undefined)
            url_ += "MinLocIDFilter=" + encodeURIComponent("" + minLocIDFilter) + "&";
        if (maxRegDateFilter !== undefined)
            url_ += "MaxRegDateFilter=" + encodeURIComponent(maxRegDateFilter ? "" + maxRegDateFilter.toJSON() : "") + "&";
        if (minRegDateFilter !== undefined)
            url_ += "MinRegDateFilter=" + encodeURIComponent(minRegDateFilter ? "" + minRegDateFilter.toJSON() : "") + "&";
        if (maxPurchaseDateFilter !== undefined)
            url_ += "MaxPurchaseDateFilter=" + encodeURIComponent(maxPurchaseDateFilter ? "" + maxPurchaseDateFilter.toJSON() : "") + "&";
        if (minPurchaseDateFilter !== undefined)
            url_ += "MinPurchaseDateFilter=" + encodeURIComponent(minPurchaseDateFilter ? "" + minPurchaseDateFilter.toJSON() : "") + "&";
        if (maxExpiryDateFilter !== undefined)
            url_ += "MaxExpiryDateFilter=" + encodeURIComponent(maxExpiryDateFilter ? "" + maxExpiryDateFilter.toJSON() : "") + "&";
        if (minExpiryDateFilter !== undefined)
            url_ += "MinExpiryDateFilter=" + encodeURIComponent(minExpiryDateFilter ? "" + minExpiryDateFilter.toJSON() : "") + "&";
        if (warrantyFilter !== undefined)
            url_ += "WarrantyFilter=" + encodeURIComponent("" + warrantyFilter) + "&";
        if (AssetTypeFilter !== undefined)
            url_ += "AssetTypeFilter=" + encodeURIComponent("" + AssetTypeFilter) + "&";
        if (maxDepRateFilter !== undefined)
            url_ += "MaxDepRateFilter=" + encodeURIComponent("" + maxDepRateFilter) + "&";
        if (minDepRateFilter !== undefined)
            url_ += "MinDepRateFilter=" + encodeURIComponent("" + minDepRateFilter) + "&";
        if (DepMethodFilter !== undefined)
            url_ += "DepMethodFilter=" + encodeURIComponent("" + DepMethodFilter) + "&";
        if (serialNumberFilter !== undefined)
            url_ += "SerialNumberFilter=" + encodeURIComponent("" + serialNumberFilter) + "&";
        if (maxPurchasePriceFilter !== undefined)
            url_ += "MaxPurchasePriceFilter=" + encodeURIComponent("" + maxPurchasePriceFilter) + "&";
        if (minPurchasePriceFilter !== undefined)
            url_ += "MinPurchasePriceFilter=" + encodeURIComponent("" + minPurchasePriceFilter) + "&";
        if (narrationFilter !== undefined)
            url_ += "NarrationFilter=" + encodeURIComponent("" + narrationFilter) + "&";
        if (accAssetFilter !== undefined)
            url_ += "AccAssetFilter=" + encodeURIComponent("" + accAssetFilter) + "&";
        if (accDeprFilter !== undefined)
            url_ += "AccDeprFilter=" + encodeURIComponent("" + accDeprFilter) + "&";
        if (accExpFilter !== undefined)
            url_ += "AccExpFilter=" + encodeURIComponent("" + accExpFilter) + "&";
        if (maxDepStartDateFilter !== undefined)
            url_ += "MaxDepStartDateFilter=" + encodeURIComponent(maxDepStartDateFilter ? "" + maxDepStartDateFilter.toJSON() : "") + "&";
        if (minDepStartDateFilter !== undefined)
            url_ += "MinDepStartDateFilter=" + encodeURIComponent(minDepStartDateFilter ? "" + minDepStartDateFilter.toJSON() : "") + "&";
        if (maxAssetLifeFilter !== undefined)
            url_ += "MaxAssetLifeFilter=" + encodeURIComponent("" + maxAssetLifeFilter) + "&";
        if (minAssetLifeFilter !== undefined)
            url_ += "MinAssetLifeFilter=" + encodeURIComponent("" + minAssetLifeFilter) + "&";
        if (maxBookValueFilter !== undefined)
            url_ += "MaxBookValueFilter=" + encodeURIComponent("" + maxBookValueFilter) + "&";
        if (minBookValueFilter !== undefined)
            url_ += "MinBookValueFilter=" + encodeURIComponent("" + minBookValueFilter) + "&";
        if (maxLastDepAmountFilter !== undefined)
            url_ += "MaxLastDepAmountFilter=" + encodeURIComponent("" + maxLastDepAmountFilter) + "&";
        if (minLastDepAmountFilter !== undefined)
            url_ += "MinLastDepAmountFilter=" + encodeURIComponent("" + minLastDepAmountFilter) + "&";
        if (maxLastDepDateFilter !== undefined)
            url_ += "MaxLastDepDateFilter=" + encodeURIComponent(maxLastDepDateFilter ? "" + maxLastDepDateFilter.toJSON() : "") + "&";
        if (minLastDepDateFilter !== undefined)
            url_ += "MinLastDepDateFilter=" + encodeURIComponent(minLastDepDateFilter ? "" + minLastDepDateFilter.toJSON() : "") + "&";
        if (disolvedFilter !== undefined)
            url_ += "DisolvedFilter=" + encodeURIComponent("" + disolvedFilter) + "&";
        if (maxActiveFilter !== undefined)
            url_ += "MaxActiveFilter=" + encodeURIComponent("" + maxActiveFilter) + "&";
        if (minActiveFilter !== undefined)
            url_ += "MinActiveFilter=" + encodeURIComponent("" + minActiveFilter) + "&";
        if (audtUserFilter !== undefined)
            url_ += "AudtUserFilter=" + encodeURIComponent("" + audtUserFilter) + "&";
        if (maxAudtDateFilter !== undefined)
            url_ += "MaxAudtDateFilter=" + encodeURIComponent(maxAudtDateFilter ? "" + maxAudtDateFilter.toJSON() : "") + "&";
        if (minAudtDateFilter !== undefined)
            url_ += "MinAudtDateFilter=" + encodeURIComponent(minAudtDateFilter ? "" + minAudtDateFilter.toJSON() : "") + "&";
        if (createdByFilter !== undefined)
            url_ += "CreatedByFilter=" + encodeURIComponent("" + createdByFilter) + "&";
        if (maxCreateDateFilter !== undefined)
            url_ += "MaxCreateDateFilter=" + encodeURIComponent(maxCreateDateFilter ? "" + maxCreateDateFilter.toJSON() : "") + "&";
        if (minCreateDateFilter !== undefined)
            url_ += "MinCreateDateFilter=" + encodeURIComponent(minCreateDateFilter ? "" + minCreateDateFilter.toJSON() : "") + "&";
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
                    return <Observable<PagedResultDtoOfGetAssetRegistrationForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<PagedResultDtoOfGetAssetRegistrationForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<PagedResultDtoOfGetAssetRegistrationForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? PagedResultDtoOfGetAssetRegistrationForViewDto.fromJS(resultData200) : new PagedResultDtoOfGetAssetRegistrationForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PagedResultDtoOfGetAssetRegistrationForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getAssetRegistrationForView(id: number | null | undefined): Observable<GetAssetRegistrationForViewDto> {
        let url_ = this.baseUrl + "/api/services/app/AssetRegistration/GetAssetRegistrationForView?";
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
            return this.processGetAssetRegistrationForView(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAssetRegistrationForView(<any>response_);
                } catch (e) {
                    return <Observable<GetAssetRegistrationForViewDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetAssetRegistrationForViewDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAssetRegistrationForView(response: HttpResponseBase): Observable<GetAssetRegistrationForViewDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetAssetRegistrationForViewDto.fromJS(resultData200) : new GetAssetRegistrationForViewDto();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetAssetRegistrationForViewDto>(<any>null);
    }

    /**
     * @param id (optional) 
     * @return Success
     */
    getAssetRegistrationForEdit(id: number | null | undefined): Observable<GetAssetRegistrationForEditOutput> {
        let url_ = this.baseUrl + "/api/services/app/AssetRegistration/GetAssetRegistrationForEdit?";
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
            return this.processGetAssetRegistrationForEdit(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAssetRegistrationForEdit(<any>response_);
                } catch (e) {
                    return <Observable<GetAssetRegistrationForEditOutput>><any>_observableThrow(e);
                }
            } else
                return <Observable<GetAssetRegistrationForEditOutput>><any>_observableThrow(response_);
        }));
    }

    protected processGetAssetRegistrationForEdit(response: HttpResponseBase): Observable<GetAssetRegistrationForEditOutput> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } };
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GetAssetRegistrationForEditOutput.fromJS(resultData200) : new GetAssetRegistrationForEditOutput();
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<GetAssetRegistrationForEditOutput>(<any>null);
    }

    /**
     * @param input (optional) 
     * @return Success
     */
    createOrEdit(input: CreateOrEditAssetRegistrationDto | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/AssetRegistration/CreateOrEdit";
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
        let url_ = this.baseUrl + "/api/services/app/AssetRegistration/Delete?";
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
     * @param maxAssetIDFilter (optional) 
     * @param minAssetIDFilter (optional) 
     * @param fmtAssetIDFilter (optional) 
     * @param assetNameFilter (optional) 
     * @param itemIDFilter (optional) 
     * @param maxLocIDFilter (optional) 
     * @param minLocIDFilter (optional) 
     * @param maxRegDateFilter (optional) 
     * @param minRegDateFilter (optional) 
     * @param maxPurchaseDateFilter (optional) 
     * @param minPurchaseDateFilter (optional) 
     * @param maxExpiryDateFilter (optional) 
     * @param minExpiryDateFilter (optional) 
     * @param warrantyFilter (optional) 
     * @param maxAssetTypeFilter (optional) 
     * @param minAssetTypeFilter (optional) 
     * @param maxDepRateFilter (optional) 
     * @param minDepRateFilter (optional) 
     * @param maxDepMethodFilter (optional) 
     * @param minDepMethodFilter (optional) 
     * @param serialNumberFilter (optional) 
     * @param maxPurchasePriceFilter (optional) 
     * @param minPurchasePriceFilter (optional) 
     * @param narrationFilter (optional) 
     * @param accAssetFilter (optional) 
     * @param accDeprFilter (optional) 
     * @param accExpFilter (optional) 
     * @param maxDepStartDateFilter (optional) 
     * @param minDepStartDateFilter (optional) 
     * @param maxAssetLifeFilter (optional) 
     * @param minAssetLifeFilter (optional) 
     * @param maxBookValueFilter (optional) 
     * @param minBookValueFilter (optional) 
     * @param maxLastDepAmountFilter (optional) 
     * @param minLastDepAmountFilter (optional) 
     * @param maxLastDepDateFilter (optional) 
     * @param minLastDepDateFilter (optional) 
     * @param disolvedFilter (optional) 
     * @param maxActiveFilter (optional) 
     * @param minActiveFilter (optional) 
     * @param audtUserFilter (optional) 
     * @param maxAudtDateFilter (optional) 
     * @param minAudtDateFilter (optional) 
     * @param createdByFilter (optional) 
     * @param maxCreateDateFilter (optional) 
     * @param minCreateDateFilter (optional) 
     * @return Success
     */
    getAssetRegistrationsToExcel(filter: string | null | undefined, maxAssetIDFilter: number | null | undefined, minAssetIDFilter: number | null | undefined, fmtAssetIDFilter: string | null | undefined, assetNameFilter: string | null | undefined, itemIDFilter: string | null | undefined, maxLocIDFilter: number | null | undefined, minLocIDFilter: number | null | undefined, maxRegDateFilter: moment.Moment | null | undefined, minRegDateFilter: moment.Moment | null | undefined, maxPurchaseDateFilter: moment.Moment | null | undefined, minPurchaseDateFilter: moment.Moment | null | undefined, maxExpiryDateFilter: moment.Moment | null | undefined, minExpiryDateFilter: moment.Moment | null | undefined, warrantyFilter: number | null | undefined, AssetTypeFilter: number | null | undefined, maxDepRateFilter: number | null | undefined, minDepRateFilter: number | null | undefined, DepMethodFilter: number | null | undefined, serialNumberFilter: string | null | undefined, maxPurchasePriceFilter: number | null | undefined, minPurchasePriceFilter: number | null | undefined, narrationFilter: string | null | undefined, accAssetFilter: string | null | undefined, accDeprFilter: string | null | undefined, accExpFilter: string | null | undefined, maxDepStartDateFilter: moment.Moment | null | undefined, minDepStartDateFilter: moment.Moment | null | undefined, maxAssetLifeFilter: number | null | undefined, minAssetLifeFilter: number | null | undefined, maxBookValueFilter: number | null | undefined, minBookValueFilter: number | null | undefined, maxLastDepAmountFilter: number | null | undefined, minLastDepAmountFilter: number | null | undefined, maxLastDepDateFilter: moment.Moment | null | undefined, minLastDepDateFilter: moment.Moment | null | undefined, disolvedFilter: number | null | undefined, maxActiveFilter: number | null | undefined, minActiveFilter: number | null | undefined, audtUserFilter: string | null | undefined, maxAudtDateFilter: moment.Moment | null | undefined, minAudtDateFilter: moment.Moment | null | undefined, createdByFilter: string | null | undefined, maxCreateDateFilter: moment.Moment | null | undefined, minCreateDateFilter: moment.Moment | null | undefined): Observable<FileDto> {
        let url_ = this.baseUrl + "/api/services/app/AssetRegistration/GetAssetRegistrationToExcel?";
        if (filter !== undefined)
            url_ += "Filter=" + encodeURIComponent("" + filter) + "&";
        if (maxAssetIDFilter !== undefined)
            url_ += "MaxAssetIDFilter=" + encodeURIComponent("" + maxAssetIDFilter) + "&";
        if (minAssetIDFilter !== undefined)
            url_ += "MinAssetIDFilter=" + encodeURIComponent("" + minAssetIDFilter) + "&";
        if (fmtAssetIDFilter !== undefined)
            url_ += "FmtAssetIDFilter=" + encodeURIComponent("" + fmtAssetIDFilter) + "&";
        if (assetNameFilter !== undefined)
            url_ += "AssetNameFilter=" + encodeURIComponent("" + assetNameFilter) + "&";
        if (itemIDFilter !== undefined)
            url_ += "ItemIDFilter=" + encodeURIComponent("" + itemIDFilter) + "&";
        if (maxLocIDFilter !== undefined)
            url_ += "MaxLocIDFilter=" + encodeURIComponent("" + maxLocIDFilter) + "&";
        if (minLocIDFilter !== undefined)
            url_ += "MinLocIDFilter=" + encodeURIComponent("" + minLocIDFilter) + "&";
        if (maxRegDateFilter !== undefined)
            url_ += "MaxRegDateFilter=" + encodeURIComponent(maxRegDateFilter ? "" + maxRegDateFilter.toJSON() : "") + "&";
        if (minRegDateFilter !== undefined)
            url_ += "MinRegDateFilter=" + encodeURIComponent(minRegDateFilter ? "" + minRegDateFilter.toJSON() : "") + "&";
        if (maxPurchaseDateFilter !== undefined)
            url_ += "MaxPurchaseDateFilter=" + encodeURIComponent(maxPurchaseDateFilter ? "" + maxPurchaseDateFilter.toJSON() : "") + "&";
        if (minPurchaseDateFilter !== undefined)
            url_ += "MinPurchaseDateFilter=" + encodeURIComponent(minPurchaseDateFilter ? "" + minPurchaseDateFilter.toJSON() : "") + "&";
        if (maxExpiryDateFilter !== undefined)
            url_ += "MaxExpiryDateFilter=" + encodeURIComponent(maxExpiryDateFilter ? "" + maxExpiryDateFilter.toJSON() : "") + "&";
        if (minExpiryDateFilter !== undefined)
            url_ += "MinExpiryDateFilter=" + encodeURIComponent(minExpiryDateFilter ? "" + minExpiryDateFilter.toJSON() : "") + "&";
        if (warrantyFilter !== undefined)
            url_ += "WarrantyFilter=" + encodeURIComponent("" + warrantyFilter) + "&";
        if (AssetTypeFilter !== undefined)
            url_ += "AssetTypeFilter=" + encodeURIComponent("" + AssetTypeFilter) + "&";
        if (maxDepRateFilter !== undefined)
            url_ += "MaxDepRateFilter=" + encodeURIComponent("" + maxDepRateFilter) + "&";
        if (minDepRateFilter !== undefined)
            url_ += "MinDepRateFilter=" + encodeURIComponent("" + minDepRateFilter) + "&";
        if (DepMethodFilter !== undefined)
            url_ += "DepMethodFilter=" + encodeURIComponent("" + DepMethodFilter) + "&";
        if (serialNumberFilter !== undefined)
            url_ += "SerialNumberFilter=" + encodeURIComponent("" + serialNumberFilter) + "&";
        if (maxPurchasePriceFilter !== undefined)
            url_ += "MaxPurchasePriceFilter=" + encodeURIComponent("" + maxPurchasePriceFilter) + "&";
        if (minPurchasePriceFilter !== undefined)
            url_ += "MinPurchasePriceFilter=" + encodeURIComponent("" + minPurchasePriceFilter) + "&";
        if (narrationFilter !== undefined)
            url_ += "NarrationFilter=" + encodeURIComponent("" + narrationFilter) + "&";
        if (accAssetFilter !== undefined)
            url_ += "AccAssetFilter=" + encodeURIComponent("" + accAssetFilter) + "&";
        if (accDeprFilter !== undefined)
            url_ += "AccDeprFilter=" + encodeURIComponent("" + accDeprFilter) + "&";
        if (accExpFilter !== undefined)
            url_ += "AccExpFilter=" + encodeURIComponent("" + accExpFilter) + "&";
        if (maxDepStartDateFilter !== undefined)
            url_ += "MaxDepStartDateFilter=" + encodeURIComponent(maxDepStartDateFilter ? "" + maxDepStartDateFilter.toJSON() : "") + "&";
        if (minDepStartDateFilter !== undefined)
            url_ += "MinDepStartDateFilter=" + encodeURIComponent(minDepStartDateFilter ? "" + minDepStartDateFilter.toJSON() : "") + "&";
        if (maxAssetLifeFilter !== undefined)
            url_ += "MaxAssetLifeFilter=" + encodeURIComponent("" + maxAssetLifeFilter) + "&";
        if (minAssetLifeFilter !== undefined)
            url_ += "MinAssetLifeFilter=" + encodeURIComponent("" + minAssetLifeFilter) + "&";
        if (maxBookValueFilter !== undefined)
            url_ += "MaxBookValueFilter=" + encodeURIComponent("" + maxBookValueFilter) + "&";
        if (minBookValueFilter !== undefined)
            url_ += "MinBookValueFilter=" + encodeURIComponent("" + minBookValueFilter) + "&";
        if (maxLastDepAmountFilter !== undefined)
            url_ += "MaxLastDepAmountFilter=" + encodeURIComponent("" + maxLastDepAmountFilter) + "&";
        if (minLastDepAmountFilter !== undefined)
            url_ += "MinLastDepAmountFilter=" + encodeURIComponent("" + minLastDepAmountFilter) + "&";
        if (maxLastDepDateFilter !== undefined)
            url_ += "MaxLastDepDateFilter=" + encodeURIComponent(maxLastDepDateFilter ? "" + maxLastDepDateFilter.toJSON() : "") + "&";
        if (minLastDepDateFilter !== undefined)
            url_ += "MinLastDepDateFilter=" + encodeURIComponent(minLastDepDateFilter ? "" + minLastDepDateFilter.toJSON() : "") + "&";
        if (disolvedFilter !== undefined)
            url_ += "DisolvedFilter=" + encodeURIComponent("" + disolvedFilter) + "&";
        if (maxActiveFilter !== undefined)
            url_ += "MaxActiveFilter=" + encodeURIComponent("" + maxActiveFilter) + "&";
        if (minActiveFilter !== undefined)
            url_ += "MinActiveFilter=" + encodeURIComponent("" + minActiveFilter) + "&";
        if (audtUserFilter !== undefined)
            url_ += "AudtUserFilter=" + encodeURIComponent("" + audtUserFilter) + "&";
        if (maxAudtDateFilter !== undefined)
            url_ += "MaxAudtDateFilter=" + encodeURIComponent(maxAudtDateFilter ? "" + maxAudtDateFilter.toJSON() : "") + "&";
        if (minAudtDateFilter !== undefined)
            url_ += "MinAudtDateFilter=" + encodeURIComponent(minAudtDateFilter ? "" + minAudtDateFilter.toJSON() : "") + "&";
        if (createdByFilter !== undefined)
            url_ += "CreatedByFilter=" + encodeURIComponent("" + createdByFilter) + "&";
        if (maxCreateDateFilter !== undefined)
            url_ += "MaxCreateDateFilter=" + encodeURIComponent(maxCreateDateFilter ? "" + maxCreateDateFilter.toJSON() : "") + "&";
        if (minCreateDateFilter !== undefined)
            url_ += "MinCreateDateFilter=" + encodeURIComponent(minCreateDateFilter ? "" + minCreateDateFilter.toJSON() : "") + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetAssetRegistrationsToExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAssetRegistrationsToExcel(<any>response_);
                } catch (e) {
                    return <Observable<FileDto>><any>_observableThrow(e);
                }
            } else
                return <Observable<FileDto>><any>_observableThrow(response_);
        }));
    }

    protected processGetAssetRegistrationsToExcel(response: HttpResponseBase): Observable<FileDto> {
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

    GetAssetRegMaxId(): Observable<string[]> {
        let url_ = this.baseUrl + "/api/services/app/AssetRegistration/GetAssetRegID";
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetAssetRegMaxId(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAssetRegMaxId(<any>response_);
                } catch (e) {
                    return <Observable<string[]>><any>_observableThrow(e);
                }
            } else
                return <Observable<string[]>><any>_observableThrow(response_);
        }));
    }

    protected processGetAssetRegMaxId(response: HttpResponseBase): Observable<string[]> {
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
        return _observableOf<string[]>(<any>null);
    }
}


export class PagedResultDtoOfGetAssetRegistrationForViewDto implements IPagedResultDtoOfGetAssetRegistrationForViewDto {
    totalCount!: number | undefined;
    items!: GetAssetRegistrationForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetAssetRegistrationForViewDto) {
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
                    this.items!.push(GetAssetRegistrationForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetAssetRegistrationForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetAssetRegistrationForViewDto();
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

export interface IPagedResultDtoOfGetAssetRegistrationForViewDto {
    totalCount: number | undefined;
    items: GetAssetRegistrationForViewDto[] | undefined;
}

export class GetAssetRegistrationForViewDto implements IGetAssetRegistrationForViewDto {
    assetRegistration!: AssetRegistrationDto | undefined;

    constructor(data?: IGetAssetRegistrationForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.assetRegistration = data["assetRegistration"] ? AssetRegistrationDto.fromJS(data["assetRegistration"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetAssetRegistrationForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetAssetRegistrationForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["assetRegistration"] = this.assetRegistration ? this.assetRegistration.toJSON() : <any>undefined;
        return data;
    }
}

export interface IGetAssetRegistrationForViewDto {
    assetRegistration: AssetRegistrationDto | undefined;
}

export class AssetRegistrationDto implements IAssetRegistrationDto {
    assetID!: number | undefined;
    fmtAssetID!: string | undefined;
    assetName!: string | undefined;
    itemID!: string | undefined;
    locID!: number | undefined;
    regDate!: moment.Moment | undefined;
    purchaseDate!: moment.Moment | undefined;
    expiryDate!: moment.Moment | undefined;
    warranty!: boolean | undefined;
    assetType!: number | undefined;
    depRate!: number | undefined;
    depMethod!: number | undefined;
    serialNumber!: string | undefined;
    purchasePrice!: number | undefined;
    narration!: string | undefined;
    accAsset!: string | undefined;
    accDepr!: string | undefined;
    accExp!: string | undefined;
    depStartDate!: moment.Moment | undefined;
    assetLife!: number | undefined;
    bookValue!: number | undefined;
    lastDepAmount!: number | undefined;
    lastDepDate!: moment.Moment | undefined;
    disolved!: boolean | undefined;
    active!: number | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    accumulatedDepAmt!: number | undefined;
    insurance!: boolean | undefined;
    finance!: boolean | undefined;
    id!: number | undefined;
    refID!:number | undefined;

    constructor(data?: IAssetRegistrationDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.assetID = data["assetID"];
            this.fmtAssetID = data["fmtAssetID"];
            this.assetName = data["assetName"];
            this.itemID = data["itemID"];
            this.locID = data["locID"];
            this.regDate = data["regDate"] ? moment(data["regDate"].toString()) : <any>undefined;
            this.purchaseDate = data["purchaseDate"] ? moment(data["purchaseDate"].toString()) : <any>undefined;
            this.expiryDate = data["expiryDate"] ? moment(data["expiryDate"].toString()) : <any>undefined;
            this.warranty = data["warranty"];
            this.assetType = data["assetType"];
            this.depRate = data["depRate"];
            this.depMethod = data["depMethod"];
            this.serialNumber = data["serialNumber"];
            this.purchasePrice = data["purchasePrice"];
            this.narration = data["narration"];
            this.accAsset = data["accAsset"];
            this.accDepr = data["accDepr"];
            this.accExp = data["accExp"];
            this.depStartDate = data["depStartDate"] ? moment(data["depStartDate"].toString()) : <any>undefined;
            this.assetLife = data["assetLife"];
            this.bookValue = data["bookValue"];
            this.lastDepAmount = data["lastDepAmount"];
            this.lastDepDate = data["lastDepDate"] ? moment(data["lastDepDate"].toString()) : <any>undefined;
            this.disolved = data["disolved"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.accumulatedDepAmt = data["accumulatedDepAmt"];
            this.finance = data["finance"];
            this.insurance = data["insurance"];
            this.id = data["id"];
            this.refID = data["refID"];
        }
    }

    static fromJS(data: any): AssetRegistrationDto {
        data = typeof data === 'object' ? data : {};
        let result = new AssetRegistrationDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["assetID"] = this.assetID;
        data["fmtAssetID"] = this.fmtAssetID;
        data["assetName"] = this.assetName;
        data["itemID"] = this.itemID;
        data["locID"] = this.locID;
        data["regDate"] = this.regDate ? this.regDate.toISOString() : <any>undefined;
        data["purchaseDate"] = this.purchaseDate ? this.purchaseDate.toISOString() : <any>undefined;
        data["expiryDate"] = this.expiryDate ? this.expiryDate.toISOString() : <any>undefined;
        data["warranty"] = this.warranty;
        data["assetType"] = this.assetType;
        data["depRate"] = this.depRate;
        data["depMethod"] = this.depMethod;
        data["serialNumber"] = this.serialNumber;
        data["purchasePrice"] = this.purchasePrice;
        data["narration"] = this.narration;
        data["accAsset"] = this.accAsset;
        data["accDepr"] = this.accDepr;
        data["accExp"] = this.accExp;
        data["depStartDate"] = this.depStartDate ? this.depStartDate.toISOString() : <any>undefined;
        data["assetLife"] = this.assetLife;
        data["bookValue"] = this.bookValue;
        data["lastDepAmount"] = this.lastDepAmount;
        data["lastDepDate"] = this.lastDepDate ? this.lastDepDate.toISOString() : <any>undefined;
        data["disolved"] = this.disolved;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["accumulatedDepAmt"] = this.accumulatedDepAmt;
        data["finance"] = this.finance;
        data["insurance"] = this.insurance;
        data["id"] = this.id;
        data["refID"]=this.refID;
        return data;
    }
}

export interface IAssetRegistrationDto {
    assetID: number | undefined;
    fmtAssetID: string | undefined;
    assetName: string | undefined;
    itemID: string | undefined;
    locID: number | undefined;
    regDate: moment.Moment | undefined;
    purchaseDate: moment.Moment | undefined;
    expiryDate: moment.Moment | undefined;
    warranty: boolean | undefined;
    assetType: number | undefined;
    depRate: number | undefined;
    depMethod: number | undefined;
    serialNumber: string | undefined;
    purchasePrice: number | undefined;
    narration: string | undefined;
    accAsset: string | undefined;
    accDepr: string | undefined;
    accExp: string | undefined;
    depStartDate: moment.Moment | undefined;
    assetLife: number | undefined;
    bookValue: number | undefined;
    lastDepAmount: number | undefined;
    lastDepDate: moment.Moment | undefined;
    disolved: boolean | undefined;
    active: number | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    accumulatedDepAmt: number | undefined;
    insurance: boolean | undefined;
    finance: boolean | undefined;
    id: number | undefined;
    refID:number | undefined;
}

export class GetAssetRegistrationForEditOutput implements IGetAssetRegistrationForEditOutput {
    assetRegistration!: CreateOrEditAssetRegistrationDto | undefined;
    itemName!: string | undefined;
    locationName!: string | undefined;
    assetAccName!: string | undefined;
    accDeprName!: string | undefined;
    accExpName!: string | undefined;
    refName!:string | undefined;
    constructor(data?: IGetAssetRegistrationForEditOutput) {
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
            this.assetRegistration = data["assetRegistration"] ? CreateOrEditAssetRegistrationDto.fromJS(data["assetRegistration"]) : <any>undefined;
            this.itemName = data["itemName"];
            this.locationName = data["locationName"];
            this.assetAccName = data["assetAccName"];
            this.accExpName = data["accExpName"];
            this.accDeprName = data["accDeprName"];
            this.refName = data["refName"]
        }
    }

    static fromJS(data: any): GetAssetRegistrationForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetAssetRegistrationForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["assetRegistration"] = this.assetRegistration ? this.assetRegistration.toJSON() : <any>undefined;
        return data;
    }
}

export interface IGetAssetRegistrationForEditOutput {
    assetRegistration: CreateOrEditAssetRegistrationDto | undefined;
    itemName: string | undefined;
    locationName: string | undefined;
    assetAccName: string | undefined;
    accDeprName: string | undefined;
    accExpName: string | undefined;
    refName:string | undefined;
}

export class CreateOrEditAssetRegistrationDto implements ICreateOrEditAssetRegistrationDto {
    createAssetRegistrationDetail!: CreateOrEditAssetRegistrationDetailDto | undefined; 
    assetID!: number | undefined;
    fmtAssetID!: string | undefined;
    assetName!: string | undefined;
    itemID!: string | undefined;
    locID!: number | undefined;
    regDate!: moment.Moment | undefined;
    purchaseDate!: moment.Moment | undefined;
    expiryDate!: moment.Moment | undefined;
    warranty!: boolean | undefined;
    assetType!: number | undefined;
    depRate!: number | undefined;
    depMethod!: number | undefined;
    serialNumber!: string | undefined;
    purchasePrice!: number | undefined;
    narration!: string | undefined;
    accAsset!: string | undefined;
    accDepr!: string | undefined;
    accExp!: string | undefined;
    depStartDate!: moment.Moment | undefined;
    assetLife!: number | undefined;
    bookValue!: number | undefined;
    lastDepAmount!: number | undefined;
    lastDepDate!: moment.Moment | undefined;
    disolved!: boolean | undefined;
    active!: number | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    accumulatedDepAmt: number | undefined;
    insurance!: boolean | undefined;
    finance!: boolean | undefined;
    id!: number | undefined;
    refID:number | undefined;

    constructor(data?: ICreateOrEditAssetRegistrationDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.createAssetRegistrationDetail = data["createAssetRegistrationDetail"] ? CreateOrEditAssetRegistrationDetailDto.fromJS(data["createAssetRegistrationDetail"]) : <any>undefined
            this.assetID = data["assetID"];
            this.fmtAssetID = data["fmtAssetID"];
            this.assetName = data["assetName"];
            this.itemID = data["itemID"];
            this.locID = data["locID"];
            this.regDate = data["regDate"] ? moment(data["regDate"].toString()) : <any>undefined;
            this.purchaseDate = data["purchaseDate"] ? moment(data["purchaseDate"].toString()) : <any>undefined;
            this.expiryDate = data["expiryDate"] ? moment(data["expiryDate"].toString()) : <any>undefined;
            this.warranty = data["warranty"];
            this.assetType = data["assetType"];
            this.depRate = data["depRate"];
            this.depMethod = data["depMethod"];
            this.serialNumber = data["serialNumber"];
            this.purchasePrice = data["purchasePrice"];
            this.narration = data["narration"];
            this.accAsset = data["accAsset"];
            this.accDepr = data["accDepr"];
            this.accExp = data["accExp"];
            this.depStartDate = data["depStartDate"] ? moment(data["depStartDate"].toString()) : <any>undefined;
            this.assetLife = data["assetLife"];
            this.bookValue = data["bookValue"];
            this.lastDepAmount = data["lastDepAmount"];
            this.lastDepDate = data["lastDepDate"] ? moment(data["lastDepDate"].toString()) : <any>undefined;
            this.disolved = data["disolved"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.accumulatedDepAmt = data["accumulatedDepAmt"];
            this.finance = data["finance"];
            this.insurance = data["insurance"];
            this.id = data["id"];
            this.refID=data["refID"];
        }
    }

    static fromJS(data: any): CreateOrEditAssetRegistrationDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditAssetRegistrationDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["createAssetRegistrationDetail"] = this.createAssetRegistrationDetail ? this.createAssetRegistrationDetail.toJSON() : <any>undefined;
        data["assetID"] = this.assetID;
        data["fmtAssetID"] = this.fmtAssetID;
        data["assetName"] = this.assetName;
        data["itemID"] = this.itemID;
        data["locID"] = this.locID;
        data["regDate"] = this.regDate ? this.regDate.toISOString() : <any>undefined;
        data["purchaseDate"] = this.purchaseDate ? this.purchaseDate.toISOString() : <any>undefined;
        data["expiryDate"] = this.expiryDate ? this.expiryDate.toISOString() : <any>undefined;
        data["warranty"] = this.warranty;
        data["assetType"] = this.assetType;
        data["depRate"] = this.depRate;
        data["depMethod"] = this.depMethod;
        data["serialNumber"] = this.serialNumber;
        data["purchasePrice"] = this.purchasePrice;
        data["narration"] = this.narration;
        data["accAsset"] = this.accAsset;
        data["accDepr"] = this.accDepr;
        data["accExp"] = this.accExp;
        data["depStartDate"] = this.depStartDate ? this.depStartDate.toISOString() : <any>undefined;
        data["assetLife"] = this.assetLife;
        data["bookValue"] = this.bookValue;
        data["lastDepAmount"] = this.lastDepAmount;
        data["lastDepDate"] = this.lastDepDate ? this.lastDepDate.toISOString() : <any>undefined;
        data["disolved"] = this.disolved;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["accumulatedDepAmt"] = this.accumulatedDepAmt;
        data["finance"] = this.finance;
        data["insurance"] = this.insurance;
        data["id"] = this.id;
        data["refID"]=this.refID;
        return data;
    }
}

export interface ICreateOrEditAssetRegistrationDto {
    createAssetRegistrationDetail: CreateOrEditAssetRegistrationDetailDto | undefined; 
    assetID: number | undefined;
    fmtAssetID: string | undefined;
    assetName: string | undefined;
    itemID: string | undefined;
    locID: number | undefined;
    regDate: moment.Moment | undefined;
    purchaseDate: moment.Moment | undefined;
    expiryDate: moment.Moment | undefined;
    warranty: boolean | undefined;
    assetType: number | undefined;
    depRate: number | undefined;
    depMethod: number | undefined;
    serialNumber: string | undefined;
    purchasePrice: number | undefined;
    narration: string | undefined;
    accAsset: string | undefined;
    accDepr: string | undefined;
    accExp: string | undefined;
    depStartDate: moment.Moment | undefined;
    assetLife: number | undefined;
    bookValue: number | undefined;
    lastDepAmount: number | undefined;
    lastDepDate: moment.Moment | undefined;
    disolved: boolean | undefined;
    active: number | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    accumulatedDepAmt: number | undefined;
    insurance: boolean | undefined;
    finance: boolean | undefined;
    id: number | undefined;
    refID:number | undefined;
}

