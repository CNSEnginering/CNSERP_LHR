import * as moment from 'moment';
import { GetSubDesignationsForViewDto, SubDesignationsDto, CreateOrEditSubDesignationsDto } from '../dto/subDesignations-dto';


export interface IPagedResultDtoOfGetSubDesignationsForViewDto {
    totalCount: number | undefined;
    items: GetSubDesignationsForViewDto[] | undefined;
}

export interface IGetSubDesignationsForViewDto {
    subDesignations: SubDesignationsDto | undefined;
}

export interface ISubDesignationsDto {
    subDesignationID: number | undefined;
    subDesignation: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetSubDesignationsForEditOutput {
    subDesignations: CreateOrEditSubDesignationsDto | undefined;
}

export interface ICreateOrEditSubDesignationsDto {
    subDesignationID: number;
    subDesignation: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

