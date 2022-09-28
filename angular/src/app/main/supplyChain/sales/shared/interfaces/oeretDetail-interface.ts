import * as moment from 'moment';
import { OERETDetailDto } from '../dtos/oeretDetails-dto';

export interface IPagedResultDtoOfOERETDetailDto {
    totalCount: number | undefined;
    items: OERETDetailDto[] | undefined;
}

export interface IOERETDetailDto {
    detID:number | undefined;
    locID:number | undefined;
    docNo:number | undefined;
    itemID: string | undefined; 
    unit: string | undefined; 
    conver: number | undefined;
    qty: number | undefined;
    rate: number | undefined;
    amount: number | undefined;
    disc: number | undefined;
    taxAuth: string | undefined;
    taxClass: number | undefined;
    taxRate: number | undefined;
    taxAmt: number | undefined;
    remarks: string | undefined;
    netAmount: string | undefined;
    id: number | undefined;
}

export interface ICreateOrEditOERETDetailDto {
    detID:number | undefined;
    locID:number | undefined;
    docNo:number | undefined;
    itemID: string | undefined; 
    unit: string | undefined; 
    conver: number | undefined;
    qty: number | undefined;
    rate: number | undefined;
    amount: number | undefined;
    disc: number | undefined;
    taxAuth: string | undefined;
    taxClass: number | undefined;
    taxRate: number | undefined;
    taxAmt: number | undefined;
    remarks: string | undefined;
    netAmount: string | undefined;
    id: number | undefined;
}