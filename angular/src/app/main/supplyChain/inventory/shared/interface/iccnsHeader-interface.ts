import * as moment from 'moment';
import { ICCNSHeaderDto } from '../dto/iccnsHeader-dto';

export interface IPagedResultDtoOfICCNSHeaderDto {
    totalCount: number | undefined;
    items: ICCNSHeaderDto[] | undefined;
}

export interface IICCNSHeaderDto {
    docNo:number | undefined;
    docDate: moment.Moment | undefined;
    narration: string | undefined;
    ccid:string | undefined;
    ccDesc:string | undefined;
    locID:number | undefined;
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

export interface ICreateOrEditICCNSHeaderDto {
    docNo:number | undefined;
    docDate: moment.Moment | undefined;
    narration: string | undefined;
    ccid:string | undefined;
    locID:number | undefined;
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