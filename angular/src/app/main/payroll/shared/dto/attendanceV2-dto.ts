import * as moment from 'moment';
import { ICreateOrEditAttendanceDto, IEmployeeDataForAttendanceDto } from "../interface/attendanceV2-interface";

export class CreateOrEditAttendanceDto implements ICreateOrEditAttendanceDto {
    employeeID!: number;
    employeeName!: string;
    attendanceDate!: moment.Moment;
    shiftID!: number | undefined;
    timeIn!: moment.Moment | undefined;
    timeOut!: moment.Moment | undefined;
    // breakOut!: moment.Moment | undefined;
    // breakIn!: moment.Moment | undefined;
    reason!: string;

    constructor(data?: ICreateOrEditAttendanceDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            debugger;
            this.employeeID = data["employeeID"];
            this.employeeName = data["employeeName"];
            this.attendanceDate = data["attendanceDate"];
            this.shiftID = data["shiftID"];
            this.timeIn = data["timeIn"];
            this.timeOut = data["timeOut"];
            // this.breakOut = data["breakOut"];
            // this.breakIn = data["breakIn"];
            this.reason = data["reason"];
        }
    }

    static fromJS(data: any): CreateOrEditAttendanceDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditAttendanceDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        this.timeIn = moment(this.timeIn).set({ 'year': this.attendanceDate.year(), 'month': this.attendanceDate.month(), 'date': this.attendanceDate.date() });
        this.timeOut = moment(this.timeOut).set({ 'year': this.attendanceDate.year(), 'month': this.attendanceDate.month(), 'date': this.attendanceDate.date() });

        if (moment(this.timeIn).format('A') == 'PM' && moment(this.timeOut).format('A') == 'AM') {
            this.timeOut = moment(moment(this.timeOut).add(1, 'days').format('YYYY-MM-DDTHH:mm:ss.SSS'));
        }

        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["attendanceDate"] = this.attendanceDate ? moment(this.attendanceDate).toISOString(true) : <any>undefined;
        data["shiftID"] = this.shiftID;
        data["timeIn"] = this.timeIn ? moment(this.timeIn).toISOString(true) : <any>undefined;
        data["timeOut"] = this.timeOut ? moment(this.timeOut).toISOString(true) : <any>undefined;
        // data["breakOut"] = this.breakOut ? moment(this.breakOut).toISOString(true) : <any>undefined;
        // data["breakIn"] = this.breakIn ? moment(this.breakIn).toISOString(true) : <any>undefined;
        data["reason"] = this.reason;


        return data;
    }
}

export class EmployeeDataForAttendanceDto implements IEmployeeDataForAttendanceDto {
    shiftID!: number;
    shiftName!: string;
    designationID!: number;
    designation!: string;
    timeIn!: moment.Moment | undefined;
    timeOut!: moment.Moment | undefined;
    reason!: string;

    constructor(data?: IEmployeeDataForAttendanceDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            debugger;
            this.shiftID = data["shiftID"];
            this.shiftName = data["shiftName"];
            this.designationID = data["designationID"];
            this.designation = data["designation"];
            this.timeIn = data["timeIn"];
            this.timeOut = data["timeOut"];
            this.reason = data["reason"];
        }
    }

    static fromJS(data: any): EmployeeDataForAttendanceDto {
        data = typeof data === 'object' ? data : {};
        let result = new EmployeeDataForAttendanceDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["shiftID"] = this.shiftID;
        data["shiftName"] = this.shiftName;
        data["designationID"] = this.designationID;
        data["designation"] = this.designation;
        data["timeIn"] = this.timeIn ? moment(this.timeIn).toISOString(true) : <any>undefined;
        data["timeOut"] = this.timeOut ? moment(this.timeOut).toISOString(true) : <any>undefined;
        data["reason"] = this.reason;
        return data;
    }
}

