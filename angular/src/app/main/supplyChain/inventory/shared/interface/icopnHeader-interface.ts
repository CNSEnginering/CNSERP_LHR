import * as moment from 'moment';
import { ICOPNHeaderDto } from '../dto/icopnHeader-dto';

export interface IPagedResultDtoOfICOPNHeaderDto {
    totalCount: number | undefined;
    items: ICOPNHeaderDto[] | undefined;
}

export interface IICOPNHeaderDto {
    docNo:number | undefined;
    docDate: moment.Moment | undefined;
    narration: string | undefined;
    locID:number | undefined;
    totalItems:number | undefined;
    totalQty:number | undefined;
    totalAmt:number | undefined;
    posted:boolean;
    postedBy:string | undefined;
    postedDate:moment.Moment | undefined;
    approved:boolean;
    approvedBy:string | undefined;
    approvedDate:moment.Moment | undefined;
    linkDetID:number | undefined;
    ordNo:string | undefined;
    active:boolean;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface ICreateOrEditICOPNHeaderDto {
    docNo:number | undefined;
    docDate: moment.Moment | undefined;
    narration: string | undefined;
    locID:number | undefined;
    totalItems:number | undefined;
    totalQty:number | undefined;
    totalAmt:number | undefined;
    posted:boolean;
    postedBy:string | undefined;
    postedDate:moment.Moment | undefined;
    approved:boolean;
    approvedBy:string | undefined;
    approvedDate:moment.Moment | undefined;
    linkDetID:number | undefined;
    ordNo:string | undefined;
    active:boolean;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    id: number | undefined;
}