import { invoiceKnockOffDetailDto } from "./invoiceKnockOffDetail-dto";

export class invoiceKnockOffDto{
    id:number;
    docNo: number;
    gllocid: number | null;
    docDate: string | Date;
    postDate: string | Date;
    debtorCtrlAc: string;
    custID: number | null;
    amount: number | null;
    totalAdjust: number | null;
    paymentOption: string;
    bankID: string;
    bAccountID: string;
    configID: number | null;
    insType: number | null;
    insNo: string;
    curID: string;
    curRate: number | null;
    narration: string;
    posted: boolean;
    postedBy: string;
    postedDate: string | null;
    
    invoiceKnockOffDetailDto:invoiceKnockOffDetailDto[] = new Array<invoiceKnockOffDetailDto>();
}