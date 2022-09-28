import * as moment from 'moment';
import { ICWOHeaderDto } from '../dto/icwoHeader-dto';

export interface IPagedResultDtoOfICWOHeaderDto {
    totalCount: number | undefined;
    items: ICWOHeaderDto[] | undefined;
}

export interface IICWOHeaderDto {
    docNo:number | undefined;
    docDate: moment.Moment | undefined;
    narration: string | undefined;
    ccid:string | undefined;
    ccDesc:string | undefined;
    locID:number | undefined;
    totalQty:number | undefined;
    totalAmt:number | undefined;
    approved:boolean;
    approvedBy:string | undefined;
    approvedDate:moment.Moment | undefined;
    refrence:string | undefined;
    active:boolean;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface ICreateOrEditICWOHeaderDto {
    docNo:number | undefined;
    docDate: moment.Moment | undefined;
    narration: string | undefined;
    ccid:string | undefined;
    locID:number | undefined;
    totalQty:number | undefined;
    totalAmt:number | undefined;
    approved:boolean;
    approvedBy:string | undefined;
    approvedDate:moment.Moment | undefined;
    refrence:string | undefined;
    active:boolean;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    id: number | undefined;
}