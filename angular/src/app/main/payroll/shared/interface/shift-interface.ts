import * as moment from 'moment';
import { ShiftDto, GetShiftForViewDto, CreateOrEditShiftDto } from '../dto/shift-dto';
export interface IPagedResultDtoOfGetShiftForViewDto {
    totalCount: number | undefined;
    items: GetShiftForViewDto[] | undefined;
}

export interface IGetShiftForViewDto {
    shift: ShiftDto | undefined; 
}

export interface IGetShiftForEditOutput {
    shift: CreateOrEditShiftDto | undefined;
}

export interface ICreateOrEditShiftDto {
    shiftID: number | undefined;
    shiftName: string | undefined;
    startTime: moment.Moment | undefined;
    endTime: moment.Moment | undefined;
    beforeStart: number | undefined;
    afterStart: number | undefined;
    beforeFinish: number | undefined;
    afterFinish: number | undefined;
    totalHour: number | undefined;
    active:  boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IShiftDto {
    shiftID: number | undefined;
    shiftName: string | undefined;
    startTime: moment.Moment | undefined;
    endTime: moment.Moment | undefined;
    beforeStart: number | undefined;
    afterStart: number | undefined;
    beforeFinish: number | undefined;
    afterFinish: number | undefined;
    totalHour: number | undefined;
    active:  boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}