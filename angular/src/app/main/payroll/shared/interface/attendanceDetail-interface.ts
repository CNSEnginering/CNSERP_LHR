import * as moment from 'moment';
import { GetAttendanceDetailForViewDto, AttendanceDetailDto, CreateOrEditAttendanceDetailDto } from '../dto/attendanceDetail-dto';


export interface IPagedResultDtoOfGetAttendanceDetailForViewDto {
    totalCount: number | undefined;
    items: GetAttendanceDetailForViewDto[] | undefined;
}
export interface IPagedResultDtoOfAttendanceDetailsDto {
    totalCount: number | undefined;
    items: AttendanceDetailDto[] | undefined;
}

export interface IGetAttendanceDetailForViewDto {
    attendanceDetail: AttendanceDetailDto | undefined;
}

export interface IAttendanceDetailDto {
    employeeID: number | undefined;
    employeeName: string | undefined;
    attendanceDate: moment.Moment | undefined;
    shiftID: number | undefined;
    detID: number | undefined;
    timeIn: moment.Moment | undefined;
    timeOut: moment.Moment | undefined;
    breakOut: moment.Moment | undefined;
    breakIn: moment.Moment | undefined;
    totalHrs: number | undefined;
    id: number | undefined;
}

export interface IGetAttendanceDetailForEditOutput {
    attendanceDetail: CreateOrEditAttendanceDetailDto | undefined;
}

export interface ICreateOrEditAttendanceDetailDto {
    employeeID: number;
    employeeName: string;
    attendanceDate: moment.Moment;
    shiftID: number | undefined;
    detID: number | undefined;
    timeIn: moment.Moment | undefined;
    timeOut: moment.Moment | undefined;
    breakOut: moment.Moment | undefined;
    breakIn: moment.Moment | undefined;
    totalHrs: number | undefined;
    id: number | undefined;
}