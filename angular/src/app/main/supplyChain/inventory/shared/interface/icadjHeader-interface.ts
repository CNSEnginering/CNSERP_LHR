import * as moment from 'moment';
import { ICADJHeaderDto } from '../dto/icadjHeader-dto';

export interface IPagedResultDtoOfICADJHeaderDto {
    totalCount: number | undefined;
    items: ICADJHeaderDto[] | undefined;
}

export interface IICADJHeaderDto {
    docNo:number | undefined;
    docDate: moment.Moment | undefined;
    narration: string | undefined;
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

export interface ICreateOrEditICADJHeaderDto {
    docNo:number | undefined;
    docDate: moment.Moment | undefined;
    narration: string | undefined;
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