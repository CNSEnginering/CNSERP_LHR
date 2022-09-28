import * as moment from 'moment';
import { ReligionDto, GetReligionForViewDto, CreateOrEditReligionDto } from '../dto/religion-dto';
export interface IPagedResultDtoOfGetReligionForViewDto {
    totalCount: number | undefined;
    items: GetReligionForViewDto[] | undefined;
}

export interface IGetReligionForViewDto {
    religion: ReligionDto | undefined;
}

export interface IGetReligionForEditOutput {
    religion: CreateOrEditReligionDto | undefined;
}

export interface ICreateOrEditReligionDto {
    religionID: number | undefined;
    religion: string | undefined;
    active: boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IReligionDto {
    religionID: number | undefined;
    religion: string | undefined;
    active:  boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}