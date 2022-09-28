import { saleQutationDetailDto } from "./saleQutationDetail-dto";

export class saleQutationDto{
    id:number;
    locID: number;
    docNo: number;
    docDate?: Date|string;
    mDocNo: string;
    mDocDate?: Date|string;
    basicStyle!: string | undefined;
    license!: string | undefined;
    typeID: string;
    saleTypeDesc:string;
    custID?: number;
    customerDesc:string;
    chartofAccountDesc:string;
    salesCtrlAcc: string;
    narration: string;
    noteText: string;
    payTerms: string;
    delvTerms: string;
    validityTerms: string;
    approved: boolean;
    approvedBy: string;
    approvedDate?: string;
    linkDetID?: number;
    active: boolean;
    taxAuth1: string;
    taxClass1: number;
    taxClassDesc1:string;
    netAmount:number;
    
    taxRate1: number;
    taxAmt1: number;

    taxAuth2: string;
    taxClass2: number;
    taxClassDesc2:string;
    taxRate2: number;
    taxAmt2: number;

    taxAuth3: string;
    taxClass3: number;
    taxClassDesc3:string;
    taxRate3: number;
    taxAmt3: number;

    taxAuth4: string;
    taxClass4: number;
    taxClassDesc4:string;
    taxRate4: number;
    taxAmt4: number;

    taxAuth5: string;
    taxClass5: number;
    taxClassDesc5:string;
    taxRate5: number;
    taxAmt5: number;

    saleDoc:string|undefined;
    qutationDetailDto:saleQutationDetailDto[] = new Array<saleQutationDetailDto>();
}
