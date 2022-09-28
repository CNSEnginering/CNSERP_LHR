import * as moment from 'moment';
import { ICADJDetailDto } from '../dto/icadjDetails-dto';

export interface IPagedResultDtoOfICADJDetailDto {
    totalCount: number | undefined;
    items: ICADJDetailDto[] | undefined;
}

export interface IICADJDetailDto {
    detID: number | undefined;
    docNo: number | undefined;
    itemID: string | undefined;
    itemDesc: string | undefined;
    unit: string | undefined;
    conver: number | undefined;
    type:string | undefined;
    qty: number| undefined;
    cost: number | undefined;
    amount: number | undefined;
    remarks: string | undefined;
    id: number | undefined;
}

export interface ICreateOrEditICADJDetailDto {
    detID: number | undefined;
    docNo: number | undefined;
    itemID: string | undefined;
    itemDesc: string | undefined;
    unit: string | undefined;
    conver: number | undefined;
    type:string | undefined;
    qty: number| undefined;
    cost: number | undefined;
    amount: number | undefined;
    remarks: string | undefined;
    id: number | undefined;
}