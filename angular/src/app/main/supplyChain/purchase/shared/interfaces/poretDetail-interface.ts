import * as moment from 'moment';
import { PORETDetailDto } from '../dtos/poretDetails-dto';

export interface IPagedResultDtoOfPORETDetailDto {
    totalCount: number | undefined;
    items: PORETDetailDto[] | undefined;
}

export interface IPORETDetailDto {
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

export interface ICreateOrEditPORETDetailDto {
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