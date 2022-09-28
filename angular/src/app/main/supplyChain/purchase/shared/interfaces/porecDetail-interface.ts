import * as moment from 'moment';
import { PORECDetailDto } from '../dtos/porecDetails-dto';

export interface IPagedResultDtoOfPORECDetailDto {
    totalCount: number | undefined;
    items: PORECDetailDto[] | undefined;
}

export interface IPORECDetailDto {
    detID: number | undefined;
    locID: number | undefined;
    docNo: number | undefined;
    itemID: string | undefined;
    unit: string | undefined;
    conver: number | undefined;
    poQty: number| undefined;
    qty: number| undefined;
    rate: number | undefined;
    amount: number | undefined;
    taxAuth: string | undefined;
    taxClass: number | undefined;
    taxRate: number | undefined;
    taxAmt: number | undefined;
    remarks: string | undefined;
    netAmount: number | undefined;
    id: number | undefined;
}

export interface ICreateOrEditPORECDetailDto {
    detID: number | undefined;
    locID: number | undefined;
    docNo: number | undefined;
    itemID: string | undefined;
    unit: string | undefined;
    conver: number | undefined;
    qty: number| undefined;
    rate: number | undefined;
    amount: number | undefined;
    taxAuth: string | undefined;
    taxClass: number | undefined;
    taxRate: number | undefined;
    taxAmt: number | undefined;
    remarks: string | undefined;
    netAmount: number | undefined;
    id: number | undefined;
}