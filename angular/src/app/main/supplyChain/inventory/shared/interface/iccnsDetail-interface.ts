import * as moment from 'moment';
import { ICCNSDetailDto } from '../dto/iccnsDetails-dto';

export interface IPagedResultDtoOfICCNSDetailDto {
    totalCount: number | undefined;
    items: ICCNSDetailDto[] | undefined;
}

export interface IICCNSDetailDto {
    detID: number | undefined;
    docNo: number | undefined;
    itemID: string | undefined;
    itemDesc: string | undefined;
    unit: string | undefined;
    conver: number | undefined;
    qty: number| undefined;
    cost: number | undefined;
    amount: number | undefined;
    remarks: string | undefined;
    engNo: string | undefined;
    subCCID: string | undefined;
    subCCName: string | undefined;
    id: number | undefined;
}

export interface ICreateOrEditICCNSDetailDto {
    detID: number | undefined;
    docNo: number | undefined;
    itemID: string | undefined;
    unit: string | undefined;
    conver: number | undefined;
    qty: number| undefined;
    cost: number | undefined;
    amount: number | undefined;
    remarks: string | undefined;
    engNo: string | undefined;
    subCCID: string | undefined;
    id: number | undefined;
}