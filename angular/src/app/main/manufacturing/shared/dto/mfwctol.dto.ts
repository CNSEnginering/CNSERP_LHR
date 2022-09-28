import { StringifyOptions } from "querystring";

export class CreateOrEditMFWCTOLDto {
    srNo:number;
    detID?: number;
    wCID: string;
    tOOLTYID: string;
    tOOLTYDESC: string;
    uOM: string;
    rEQQTY?: number;
    uNITCOST?: number;
    tOTALCOST?: number;
}