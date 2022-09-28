import { GatePassDetailDto } from "./gatePassDetail-dto";

export class GatePassHeaderDto {
    id: number;
    docNo: number;
    docDate: any;
    partyId: number;
    partyName: string;
    accountId: string;
    accountName:string
    typeId: number;
    narration: string;
    gpType: string;
    dcNo:string;
    driverName: string;
    vehicleNo: string;
    gpDocNo: number;
    orderNo: number;
    GatePassDetailDto: GatePassDetailDto[] = new Array<GatePassDetailDto>();
}