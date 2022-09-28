import { Inject, Injectable ,Optional} from '@angular/core';
import { API_BASE_URL, blobToText, throwException} from "@shared/service-proxies/service-proxies";
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { AppConsts } from '@shared/AppConsts';
import { FileDto } from '@shared/service-proxies/service-proxies';
import { Guid } from "guid-typescript";

@Injectable()
export class FileDownloadService {
    private http: HttpClient;
    private baseUrl: string;
    constructor(@Inject(HttpClient) http: HttpClient,@Optional() @Inject(API_BASE_URL) baseUrl?: string){
        this.baseUrl = baseUrl ? baseUrl : "";
    }
    downloadTempFile(file: FileDto) {
        const url = AppConsts.remoteServiceBaseUrl + '/File/DownloadTempFile?fileType=' + file.fileType + '&fileToken=' + file.fileToken + '&fileName=' + file.fileName;
        location.href = url; //TODO: This causes reloading of same page in Firefox
    }
    downloadBinaryFile(id: Guid, contentType: string, fileName:string) {
        debugger;
        const url = AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFile?id=' + id + '&contentType=' + contentType + '&fileName=' + fileName;
        location.href = url; //TODO: This causes reloading of same page in Firefox
    }
    deleteFile(appID: number, docID: number) {
        debugger
        let url = this.baseUrl;
        url += "/DemoUiComponents/DeleteFile?";
        if (appID !== undefined)
            url += "AppID=" + encodeURIComponent("" + appID) + "&";
        if (docID !== undefined)
            url += "DocID=" + encodeURIComponent("" + docID) + "&";
        url = url.replace(/[?&]$/, "");
        return this.http.get(url);
    }
}
