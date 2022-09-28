import * as moment from 'moment';
import { CreateOrEditDeductionTypesDto, DeductionTypesDto, GetDeductionTypesForViewDto } from '../dto/deductionTypes-dto';


export interface ICreateOrEditDeductionTypesDto {
    typeID: number;
    typeDesc: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetDeductionTypesForEditOutput {
    deductionTypes: CreateOrEditDeductionTypesDto | undefined;
}

export interface IDeductionTypesDto {
    typeID: number | undefined;
    typeDesc: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetDeductionTypesForViewDto {
    deductionTypes: DeductionTypesDto | undefined;
}

export interface IPagedResultDtoOfGetDeductionTypesForViewDto {
    totalCount: number | undefined;
    items: GetDeductionTypesForViewDto[] | undefined;
}

