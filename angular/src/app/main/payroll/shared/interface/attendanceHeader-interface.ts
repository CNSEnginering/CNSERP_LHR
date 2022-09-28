import * as moment from 'moment';
import { AttendanceHeaderDto, CreateOrEditAttendanceHeaderDto, GetAttendanceHeaderForViewDto } from '../dto/attendanceHeader-dto';
import { CreateOrEditAttendanceDetailDto } from '../dto/attendanceDetail-dto';

export interface IPagedResultDtoOfAttendanceHeaderDto {
    totalCount: number | undefined;
    items: AttendanceHeaderDto[] | undefined;
}

export interface IAttendanceHeaderDto {
    docDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface ICreateOrEditAttendanceHeaderDto {
    flag: boolean |undefined
    attendanceDetail: CreateOrEditAttendanceDetailDto [] | undefined;
    docDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}


export interface IGetAttendanceHeaderForEditOutput {
    attendanceHeader: CreateOrEditAttendanceHeaderDto | undefined;
}


export interface IGetAttendanceHeaderForViewDto {
    attendanceHeader: AttendanceHeaderDto | undefined;
}

export interface IPagedResultDtoOfGetAttendanceHeaderForViewDto {
    totalCount: number | undefined;
    items: GetAttendanceHeaderForViewDto[] | undefined;
}