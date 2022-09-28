export class Oedriversdto {
    id:number;
    driverID:number;
    driverName:string;
    driverCtrlAcc:string;
    accountDesc:string;
    subAccountDesc:string;
    driverSubAccID:number;
    active: boolean;
    createdBy: string;
    createDate: string | Date;
    audtUser: string;
    audtDate: string | Date;
}
export class GetcaderDToOutput {
    OeDriversDTo: Oedriversdto
}

export class PagedResultDtocader {
    totalCount: number;
    items: GetcaderDToOutput[]
}