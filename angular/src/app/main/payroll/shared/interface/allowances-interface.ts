import * as moment from 'moment';
import { GetAllowancesForViewDto, AllowancesDto, CreateOrEditAllowancesDto } from '../dto/allowances-dto';

export interface IPagedResultDtoOfGetAllowancesForViewDto {
    totalCount: number | undefined;
    items: GetAllowancesForViewDto[] | undefined;
}

export interface IGetAllowancesForViewDto {
    allowances: AllowancesDto | undefined;
}

export interface IAllowancesDto {
    docID: number | undefined;
    docdate: moment.Moment | undefined;
    docMonth: number | undefined;
    docYear: number | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetAllowancesForEditOutput {
    allowances: CreateOrEditAllowancesDto | undefined;
}

export interface ICreateOrEditAllowancesDto {
    docID: number | undefined;
    docdate: moment.Moment;
    docMonth: number | undefined;
    docYear: number | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}