import * as moment from 'moment';
import { LCExpensesDto, GetLCExpensesForViewDto, CreateOrEditLCExpensesDto } from '../dto/lcExpenses-dto';
export interface IPagedResultDtoOfGetLCExpensesForViewDto {
    totalCount: number | undefined;
    items: GetLCExpensesForViewDto[] | undefined;
}

export interface IGetLCExpensesForViewDto {
    lcExpenses: LCExpensesDto | undefined;
}

export interface IGetLCExpensesForEditOutput {
    lcExpenses: CreateOrEditLCExpensesDto | undefined;
}

export interface ICreateOrEditLCExpensesDto {
    expID: number | undefined;
    expDesc: string | undefined;
    active: boolean;
    auditUser: string | undefined;
    auditDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface ILCExpensesDto {
    expID: number | undefined;
    expDesc: string | undefined;
    active: boolean;
    auditUser: string | undefined;
    auditDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}