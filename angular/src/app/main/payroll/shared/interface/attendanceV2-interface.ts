import * as moment from 'moment';

export interface ICreateOrEditAttendanceDto {
    employeeID: number;
    employeeName: string;
    attendanceDate: moment.Moment;
    shiftID: number | undefined;
    timeIn: moment.Moment | undefined;
    timeOut: moment.Moment | undefined;
    // breakOut: moment.Moment | undefined;
    // breakIn: moment.Moment | undefined;
    reason: string;
}

export interface IEmployeeDataForAttendanceDto{
    shiftID: number;
    shiftName: string;
    designationID: number;
    designation: string;
    timeIn: moment.Moment | undefined;
    timeOut: moment.Moment | undefined;
    reason: string;
}
