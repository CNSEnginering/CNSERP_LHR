import { RequisitionDetailDto } from "./requisitionDetail-dto";

export class RequisitionDto {
    id: number;
    locID: number;
    locName:string;
    docNo: number;
    docDate: Date;
    expArrivalDate:Date;
    ordNo:number;
    ccid:string;
    costCenterName:string;
    narration:string;
    totalQty:number;
    arrivalDate:Date;
    reqNo:string;
    hold!: boolean;
    active!: boolean;
    approved!:boolean;
    posted!: boolean;
    RequisitionDetailDto:RequisitionDetailDto[] = new Array<RequisitionDetailDto>()
}