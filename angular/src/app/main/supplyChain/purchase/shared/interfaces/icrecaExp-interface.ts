import * as moment from 'moment';
import { ICRECAExpDto } from '../dtos/icrecaExp-dto';

export interface IPagedResultDtoOfICRECAExpDto {
    totalCount: number | undefined;
    items: ICRECAExpDto[] | undefined;
}

export interface IICRECAExpDto {
    detID: number | undefined;
    locID: number | undefined;
    docNo: number | undefined;
    expType:string | undefined;
    accountID: string | undefined;
    accountName: string | undefined;
    amount: number | undefined;
    id: number | undefined;
}

export interface ICreateOrEditICRECAExpDto {
    detID: number | undefined;
    locID: number | undefined;
    docNo: number | undefined;
    expType:string | undefined;
    accountID: string | undefined;
    amount: number | undefined;
    id: number | undefined;
}