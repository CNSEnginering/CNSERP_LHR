import * as moment from 'moment';
import { ICOPNDetailDto } from '../dto/icopnDetails-dto';

export interface IPagedResultDtoOfICOPNDetailDto {
    totalCount: number | undefined;
    items: ICOPNDetailDto[] | undefined;
}

export interface IICOPNDetailDto {
    detID: number | undefined;
    locID: number | undefined;
    docNo: number | undefined;
    itemID: string | undefined;
    itemDesc: string | undefined;
    unit: string | undefined;
    conver: number | undefined;
    qty: number| undefined;
    rate: number | undefined;
    amount: number | undefined;
    remarks: string | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface ICreateOrEditICOPNDetailDto {
    detID: number | undefined;
    locID: number | undefined;
    docNo: number | undefined;
    itemID: string | undefined;
    unit: string | undefined;
    conver: number | undefined;
    qty: number| undefined;
    rate: number | undefined;
    amount: number | undefined;
    remarks: string | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    id: number | undefined;
}