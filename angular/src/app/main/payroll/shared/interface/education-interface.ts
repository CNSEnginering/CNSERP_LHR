import * as moment from 'moment';
import { GetEducationForViewDto, EducationDto, CreateOrEditEducationDto } from '../dto/education-dto';

export interface IPagedResultDtoOfGetEducationForViewDto {
    totalCount: number | undefined;
    items: GetEducationForViewDto[] | undefined;
}

export interface IGetEducationForViewDto {
    education: EducationDto | undefined;
}

export interface IEducationDto {
    edID: number | undefined;
    eduction: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetEducationForEditOutput {
    education: CreateOrEditEducationDto | undefined;
}

export interface ICreateOrEditEducationDto {
    edID: number;
    eduction: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}
