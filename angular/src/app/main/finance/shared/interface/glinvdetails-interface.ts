import * as moment from 'moment';
import { GLINVDetailDto } from '../dto/glinvDetails-dto';

export interface IPagedResultDtoOfGLINVDetailDto {
    totalCount: number | undefined;
    items: GLINVDetailDto[] | undefined;
}

export interface IGLINVDetailDto {
    detID: number | undefined;
    accountID: string | undefined;
    accountDesc: string | undefined;
    subAccID: number | undefined;
    subAccDesc: string | undefined;
    narration: string| undefined;
    amount: number | undefined;
    isAuto: boolean;
    id: number | undefined;
}

export interface ICreateOrEditGLINVDetailDto {
    detID: number | undefined;
    accountID: string | undefined;
    accountDesc: string | undefined;
    subAccID: number | undefined;
    subAccDesc: string | undefined;
    narration: string| undefined;
    amount: number | undefined;
    isAuto: boolean;
    id: number | undefined;
}