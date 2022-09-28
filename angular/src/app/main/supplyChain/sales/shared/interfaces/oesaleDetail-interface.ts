import * as moment from 'moment';
import { OESALEDetailDto } from '../dtos/oesaleDetails-dto';

export interface IPagedResultDtoOfOESALEDetailDto {
    totalCount: number | undefined;
    items: OESALEDetailDto[] | undefined;
}

export interface IOESALEDetailDto {
    detID:number | undefined;
    locID:number | undefined;
    docNo:number | undefined;
    itemID: string | undefined; 
    unit: string | undefined; 
    conver: number | undefined;
    qty: number | undefined;
    rate: number | undefined;
    amount: number | undefined;
    exlTaxAmount: number | undefined;
    disc: number | undefined;
    taxAuth: string | undefined;
    taxClass: number | undefined;
    taxRate: number | undefined;
    taxAmt: number | undefined;
    remarks: string | undefined;
    netAmount: string | undefined;
    id: number | undefined;
}

export interface ICreateOrEditOESALEDetailDto {
    detID:number | undefined;
    locID:number | undefined;
    docNo:number | undefined;
    itemID: string | undefined; 
    unit: string | undefined; 
    conver: number | undefined;
    qty: number | undefined;
    rate: number | undefined;
    amount: number | undefined;
    exlTaxAmount : number | undefined;
    disc: number | undefined;
    taxAuth: string | undefined;
    taxClass: number | undefined;
    taxRate: number | undefined;
    taxAmt: number | undefined;
    remarks: string | undefined;
    netAmount: string | undefined;
    id: number | undefined;
}