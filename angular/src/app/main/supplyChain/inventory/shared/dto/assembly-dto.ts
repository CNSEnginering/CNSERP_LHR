import { AssemblyDetailDto } from "./assemblyDetail-dto";

export class AssemblyDto{
    id:number;
    docNo:number;
    docDate:Date;
    locId:number;
    locDesc:string;
    narration:string;
    ordNo:string;
    overHead:number;
    assemblyDetailDto:AssemblyDetailDto[] = new Array<AssemblyDetailDto>();
}