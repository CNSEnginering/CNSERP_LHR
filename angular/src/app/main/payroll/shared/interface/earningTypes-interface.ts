import * as moment from 'moment';
import { GetEarningTypesForViewDto, EarningTypesDto, CreateOrEditEarningTypesDto } from '../dto/earningTypes-dto';


export interface IPagedResultDtoOfGetEarningTypesForViewDto {
    totalCount: number | undefined;
    items: GetEarningTypesForViewDto[] | undefined;
}

export interface IGetEarningTypesForViewDto {
    earningTypes: EarningTypesDto | undefined;
}

export interface IEarningTypesDto {
    typeID: number | undefined;
    typeDesc: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetEarningTypesForEditOutput {
    earningTypes: CreateOrEditEarningTypesDto | undefined;
}

export interface ICreateOrEditEarningTypesDto {
    typeID: number;
    typeDesc: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}
