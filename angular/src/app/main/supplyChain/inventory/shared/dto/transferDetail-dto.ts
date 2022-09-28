import { StringifyOptions } from "querystring";

export class TransferDetailDto {
    srNo: number;
    itemId: number;
    description: string
    FromLocId:number;
    qty: number;
    maxQty: number;
    remarks: number;
    lotNo:string;
    bundle:string;
    amount:number;
    conver:number;
    unit:string;
}