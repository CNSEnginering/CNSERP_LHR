import * as moment from 'moment';
import { DesignationDto, GetDesignationForViewDto, CreateOrEditDesignationDto } from '../dto/designation-dto';
export interface IPagedResultDtoOfGetDesignationForViewDto {
    totalCount: number | undefined;
    items: GetDesignationForViewDto[] | undefined;
}

export interface IGetDesignationForViewDto {
    designation: DesignationDto | undefined;
}

export interface IGetDesignationForEditOutput {
    designation: CreateOrEditDesignationDto | undefined;
}

export interface ICreateOrEditDesignationDto {
    designationID: number | undefined;
    designation: string | undefined;
    active: boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IDesignationDto {
    designationID: number | undefined;
    designation: string | undefined;
    active:  boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}