import {  OnInit, Injectable,  Inject, Optional } from '@angular/core';
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { HttpClient, HttpResponseBase, HttpResponse, HttpHeaders } from "@angular/common/http";
import { ComboboxItemDto, API_BASE_URL, blobToText, throwException } from '@shared/service-proxies/service-proxies';

@Injectable()
export class LegderTypeComboboxService  implements OnInit {

    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;
    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }
  ngOnInit() {
  }

  getLedgerTypesForCombobox(input: string | null | undefined): Observable<ListResultDtoOfComboboxItemDto> {
    let url_ = this.baseUrl + "/api/services/app/LedgerTypes/GetLedgerTypesForCombobox?";
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
        return this.processGetLedgerTypesForCombobox(response_);
    })).pipe(_observableCatch((response_: any) => {
        if (response_ instanceof HttpResponseBase) {
            try {
                return this.processGetLedgerTypesForCombobox(<any>response_);
            } catch (e) {
                return <Observable<ListResultDtoOfComboboxItemDto>><any>_observableThrow(e);
            }
        } else
            return <Observable<ListResultDtoOfComboboxItemDto>><any>_observableThrow(response_);
    }));
}

protected processGetLedgerTypesForCombobox(response: HttpResponseBase): Observable<ListResultDtoOfComboboxItemDto> {
    const status = response.status;
    const responseBlob = 
        response instanceof HttpResponse ? response.body : 
        (<any>response).error instanceof Blob ? (<any>response).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
    if (status === 200) {
        return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result200: any = null;
        let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
        result200 = resultData200 ? ListResultDtoOfComboboxItemDto.fromJS(resultData200) : new ListResultDtoOfComboboxItemDto();
        return _observableOf(result200);
        }));
    } else if (status !== 200 && status !== 204) {
        return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }));
    }
    return _observableOf<ListResultDtoOfComboboxItemDto>(<any>null);
}

}

export class ListResultDtoOfComboboxItemDto implements IListResultDtoOfComboboxItemDto {
  items!: ComboboxItemDto[] | undefined;

  constructor(data?: IListResultDtoOfComboboxItemDto) {
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
                  this.items!.push(ComboboxItemDto.fromJS(item));
          }
      }
  }

  static fromJS(data: any): ListResultDtoOfComboboxItemDto {
      data = typeof data === 'object' ? data : {};
      let result = new ListResultDtoOfComboboxItemDto();
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

export interface IListResultDtoOfComboboxItemDto {
  items: ComboboxItemDto[] | undefined;
}
