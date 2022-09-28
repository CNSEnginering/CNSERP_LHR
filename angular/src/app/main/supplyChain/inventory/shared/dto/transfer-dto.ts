import { TransferDetailDto } from "./transferDetail-dto";

export class TransferDto {
    id: number;
    docNo: number;
    docDate: Date;
    fromLocId: number;
    toLocId: number;
    fromLocDesc: string;
    toLocDesc: string;
    narration: string;
    totalQty: number;
    totalAmt: number;
    posted: boolean;
    approved: boolean;
    postedBy: string;
    postedDate: Date;
    linkDetID: number;
    ordNo: string;
    hold: string;
    audtUser: string;
    audtDate: Date;
    createdBy: string;
    createDate: Date;
    vehicleNo: string;
    referenceNo: string;
    transferDetailDto: TransferDetailDto[] = new Array<TransferDetailDto>();
    ccid:string;
    ccdesc:string;
}
