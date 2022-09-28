
import * as moment from 'moment';
import { GetHolidaysForViewDto, HolidaysDto, CreateOrEditHolidaysDto } from '../dto/holidays-dto';


export interface IPagedResultDtoOfGetHolidaysForViewDto {
    totalCount: number | undefined;
    items: GetHolidaysForViewDto[] | undefined;
}

export interface IGetHolidaysForViewDto {
    holidays: HolidaysDto | undefined;
}

export interface IHolidaysDto {
    holidayID: number | undefined;
    holidayDate: moment.Moment | undefined;
    holidayName: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetHolidaysForEditOutput {
    holidays: CreateOrEditHolidaysDto | undefined;
}

export interface ICreateOrEditHolidaysDto {
    holidayID: number;
    holidayDate: moment.Moment;
    holidayName: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}
