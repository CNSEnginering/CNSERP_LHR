import { costSheetDetailDto } from "./costSheetDetail-dto";

export class costSheetDto{
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
    
    directCost:number;
    commRate:number;
    commAmt:number;
    ohRate:number;
    ohAmt:number;
    taxRate:number;
    taxAmt:number;
    totalCost:number;
    profRate:number;
    profAmt:number;
    salePrice:number;
    costPP:number;
    salePP:number;
    usRate:number;
    saleUS:number;
    

    saleDoc:string|undefined;
    itemName:string|undefined;
    orderQty:number;
    qutationDetailDto:costSheetDetailDto[] = new Array<costSheetDetailDto>();
}
