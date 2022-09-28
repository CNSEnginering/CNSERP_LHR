export class apinvDTo{

    id: number|null;

    docNo: number|null;

    vAccountID: string;

    subAccID: number;

    partyInvNo: string;

    partyInvDate:  string |Date | null;

    amount :number|null;
    retAmt :number|null;
    balAmt :number|null;
    recAmt :number|null;

    discAmount: number|null;

    paymentOption: string;

    bankID: string;

    bAccountID: string;

    configID: number|null;

    chequeNo: string;

    chType: string;

    curID: string;

    curRate: number|null;

    taxAuth: string;

    taxClass:number|null;
    paidAmt:number|null;
    pendingAmt:number|null;
    taxRate:number|null;

    taxAccID: string;

    taxAmount: number|null;

    docDate: Date | string | null;

    postDate:  Date | string | null;
    
    narration: string;
    
    refNo: string;
    
    payReason: string;
    
    posted: boolean;
    
    postedBy: string;
    
    approvedDate:  Date | string | null;
    approvedBy: string;
    approved: boolean;

    linkDetID:number|null;

   
    typeID: string;
    
    docStatus: string;

    cprID: number|null;

    cprNo: string;

    LocId: number|null;

    cprDate:  Date | string | null;
    vAccountName: string;
    subAccName: string;
    taxAccName:string;
}




export class GetApinvHForViewDto {
    apinvh: apinvDTo
}

export class GetApinvEditOutput {
    apinvh: apinvDTo
}

export class PagedResultDtoApinv {
    totalCount: number;
    items: GetApinvHForViewDto[]
}
export class CreateOrEditapinvDto {
    
  
    docNo: number|null;

    vAccountID: string;

    subAccID: number;

    partyInvNo: string;
    

    partyInvDate:  Date | string | null;

    amount :number|null;

    discAmount: number|null;
    paidAmt:number|null;
    pendingAmt:number|null;

    paymentOption: string;

    bankID: string;

    bAccountID: string;

    configID: number|null;
    locId: number|null;

    chequeNo: string;

    chType: string;
    approvedDate:  Date | string | null;
    approvedBy: string;
    approved: boolean;

    curID: string;

    curRate: number|null;

    taxAuth: string;

    taxClass:number|null;

    taxRate:number|null;

    taxAccID: string;

    taxAmount: number|null;

    docDate: Date | string | null;

    postDate:  Date | string | null;

    narration: string;

    refNo: string;

    payReason: string;

    posted: boolean;

    postedBy: string;

    linkDetID:number|null;

   

    docStatus: string;

    cprID: number|null;

    cprNo: string;
    vAccountName: string;
    subAccName: string;

    cprDate:  Date | string | null;

}