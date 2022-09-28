import { ARINVDDto } from "./arinvd-dto";

export class ARINVHDto {
    id: number;
    docNo: number;
    docDate: Date | string | null;
    invDate: Date | string | null;
    locID: number | null;
    routID: number | null;
    refNo: number;
    saleTypeID: string;
    paymentOption: string;
    narration: string;
    bankID: string;
    accountID: string;
    configID: number | null;
    chequeNo: string;
    linkDetID: number | null;
    audtUser: string;
    audtDate: string | null;
    createdBy: string;
    createDate: Date | string | null;
    posted: boolean;
    postedBy: string;
    postedDate: Date | string | null;
    arinvDetailDto: ARINVDDto[] = new Array<ARINVDDto>();
}