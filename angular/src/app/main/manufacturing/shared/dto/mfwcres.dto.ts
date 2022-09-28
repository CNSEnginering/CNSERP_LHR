import { StringifyOptions } from "querystring";
export class CreateOrEditMFWCRESDto {
    srNo:number;
    detID?: number;
    wCID: string;
    rESID: string;
    rESDESC: string;
    uOM: string;
    rEQQTY?: number;
    uNITCOST?: number;
    tOTALCOST?: number;
}