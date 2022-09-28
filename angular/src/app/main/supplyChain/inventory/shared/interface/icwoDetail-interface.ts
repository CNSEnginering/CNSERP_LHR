import * as moment from 'moment';
import { ICWODetailDto } from '../dto/icwoDetails-dto';

export interface IPagedResultDtoOfICWODetailDto {
    totalCount: number | undefined;
    items: ICWODetailDto[] | undefined;
}

export interface IICWODetailDto {
    detID: number | undefined;
    docNo: number | undefined;
    locID: number | undefined;
    itemID: string | undefined;
    itemDesc: string | undefined;
    unit: string | undefined;
    conver: number | undefined;
    qty: number| undefined;
    cost: number | undefined;
    amount: number | undefined;
    remarks: string | undefined;
    id: number | undefined;
}

export interface ICreateOrEditICWODetailDto {
    detID: number | undefined;
    docNo: number | undefined;
    locID: number | undefined;
    itemID: string | undefined;
    unit: string | undefined;
    conver: number | undefined;
    qty: number| undefined;
    cost: number | undefined;
    amount: number | undefined;
    remarks: string | undefined;
    id: number | undefined;
}