import * as moment from 'moment';
import { LocationDto, GetLocationForViewDto, CreateOrEditLocationDto } from '../dto/location-dto';
export interface IPagedResultDtoOfGetLocationForViewDto {
    totalCount: number | undefined;
    items: GetLocationForViewDto[] | undefined;
}

export interface IGetLocationForViewDto {
    location: LocationDto | undefined;
}

export interface IGetLocationForEditOutput {
    location: CreateOrEditLocationDto | undefined;
}

export interface ICreateOrEditLocationDto {
    locID: number | undefined;
    location: string | undefined;
    active: boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface ILocationDto {
    locID: number | undefined;
    location: string | undefined;
    active:  boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}