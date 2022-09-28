import * as moment from 'moment';
import { POPODetailDto } from '../dtos/popoDetails-dto';

export interface IPagedResultDtoOfPOPODetailDto {
    totalCount: number | undefined;
    items: POPODetailDto[] | undefined;
}

export interface IPOPODetailDto {
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

export interface ICreateOrEditPOPODetailDto {
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