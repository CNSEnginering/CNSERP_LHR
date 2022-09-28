import * as moment from 'moment';
import { IPagedResultDtoOfGetAttendanceDetailForViewDto, IGetAttendanceDetailForViewDto, IAttendanceDetailDto, IGetAttendanceDetailForEditOutput, ICreateOrEditAttendanceDetailDto, IPagedResultDtoOfAttendanceDetailsDto } from '../interface/attendanceDetail-interface';


export class PagedResultDtoOfGetAttendanceDetailForViewDto implements IPagedResultDtoOfGetAttendanceDetailForViewDto {
    totalCount!: number | undefined;
    items!: GetAttendanceDetailForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetAttendanceDetailForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalCount = data["totalCount"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [] as any;
                for (let item of data["items"])
                    this.items!.push(GetAttendanceDetailForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetAttendanceDetailForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetAttendanceDetailForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        return data;
    }
}

export class PagedResultDtoOfAttendanceDetailDto implements IPagedResultDtoOfAttendanceDetailsDto {
    totalCount!: number | undefined;
    items!: AttendanceDetailDto[] | undefined;

    constructor(data?: IPagedResultDtoOfAttendanceDetailsDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalCount = data["totalCount"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [] as any;
                for (let item of data["items"])
                    this.items!.push(AttendanceDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfAttendanceDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfAttendanceDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        return data;
    }
}

export class GetAttendanceDetailForViewDto implements IGetAttendanceDetailForViewDto {
    attendanceDetail!: AttendanceDetailDto | undefined;

    constructor(data?: IGetAttendanceDetailForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.attendanceDetail = data["attendanceDetail"] ? AttendanceDetailDto.fromJS(data["attendanceDetail"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetAttendanceDetailForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetAttendanceDetailForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["attendanceDetail"] = this.attendanceDetail ? this.attendanceDetail.toJSON() : <any>undefined;
        return data;
    }
}

export class AttendanceDetailDto implements IAttendanceDetailDto {
    employeeID!: number | undefined;
    employeeName!: string | undefined;
    attendanceDate!: moment.Moment | undefined;
    shiftID!: number | undefined;
    detID!: number | undefined;
    timeIn!: moment.Moment | undefined;
    timeOut!: moment.Moment | undefined;
    breakOut!: moment.Moment | undefined;
    breakIn!: moment.Moment | undefined;
    totalHrs!: number | undefined;
    id!: number | undefined;

    constructor(data?: IAttendanceDetailDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.employeeID = data["employeeID"];
            this.employeeName = data["employeeName"];
            this.attendanceDate = data["attendanceDate"];
            this.shiftID = data["shiftID"];
            this.detID = data["detID"];
            this.timeIn = data["timeIn"];
            this.timeOut = data["timeOut"];
            this.breakOut = data["breakOut"];
            this.breakIn = data["breakIn"];
            this.totalHrs = data["totalHrs"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): AttendanceDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new AttendanceDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        // this.timeIn = moment(this.timeIn).set({ 'year': this.attendanceDate.year(), 'month': this.attendanceDate.month(), 'date': this.attendanceDate.date() });
        // this.timeOut = moment(this.timeOut).set({ 'year': this.attendanceDate.year(), 'month': this.attendanceDate.month(), 'date': this.attendanceDate.date() });

        // if (moment(this.timeIn).format('A') == 'PM' && moment(this.timeOut).format('A') == 'AM') {
        //     this.timeOut = moment(moment(this.timeOut).add(1, 'days').format('YYYY-MM-DDTHH:mm:ss.SSS'));
        // }

        // this.breakOut = moment(this.breakOut).set({ 'year': this.attendanceDate.year(), 'month': this.attendanceDate.month(), 'date': this.attendanceDate.date() });
        // this.breakIn = moment(this.breakIn).set({ 'year': this.attendanceDate.year(), 'month': this.attendanceDate.month(), 'date': this.attendanceDate.date() });

        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["attendanceDate"] = this.attendanceDate ? moment(this.attendanceDate).toISOString(true) : <any>undefined;
        data["shiftID"] = this.shiftID;
        data["detID"] = this.detID;
        data["timeIn"] = this.timeIn ? moment(this.timeIn).toISOString(true) : <any>undefined;
        data["timeOut"] = this.timeOut ? moment(this.timeOut).toISOString(true) : <any>undefined;
        data["breakOut"] = this.breakOut ? moment(this.breakOut).toISOString(true) : <any>undefined;
        data["breakIn"] = this.breakIn ? moment(this.breakIn).toISOString(true) : <any>undefined;
        data["totalHrs"] = this.totalHrs;
        data["id"] = this.id;
        return data;
    }
}

export class GetAttendanceDetailForEditOutput implements IGetAttendanceDetailForEditOutput {
    attendanceDetail!: CreateOrEditAttendanceDetailDto | undefined;

    constructor(data?: IGetAttendanceDetailForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.attendanceDetail = data["attendanceDetail"] ? CreateOrEditAttendanceDetailDto.fromJS(data["attendanceDetail"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetAttendanceDetailForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetAttendanceDetailForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["attendanceDetail"] = this.attendanceDetail ? this.attendanceDetail.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditAttendanceDetailDto implements ICreateOrEditAttendanceDetailDto {
    employeeID!: number;
    employeeName!: string;
    attendanceDate!: moment.Moment;
    shiftID!: number | undefined;
    detID!: number | undefined;
    timeIn!: moment.Moment | undefined;
    timeOut!: moment.Moment | undefined;
    breakOut!: moment.Moment | undefined;
    breakIn!: moment.Moment | undefined;
    totalHrs!: number | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditAttendanceDetailDto) {
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
            this.detID = data["detID"];
            this.timeIn = data["timeIn"];
            this.timeOut = data["timeOut"];
            this.breakOut = data["breakOut"];
            this.breakIn = data["breakIn"];
            this.totalHrs = data["totalHrs"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditAttendanceDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditAttendanceDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        // this.timeIn = moment(this.timeIn).set({ 'year': this.attendanceDate.year(), 'month': this.attendanceDate.month(), 'date': this.attendanceDate.date() });
        // this.timeOut = moment(this.timeOut).set({ 'year': this.attendanceDate.year(), 'month': this.attendanceDate.month(), 'date': this.attendanceDate.date() });

        // if (moment(this.timeIn).format('A') == 'PM' && moment(this.timeOut).format('A') == 'AM') {
        //     this.timeOut = moment(moment(this.timeOut).add(1, 'days').format('YYYY-MM-DDTHH:mm:ss.SSS'));
        // }

        // this.breakOut = moment(this.breakOut).set({ 'year': this.attendanceDate.year(), 'month': this.attendanceDate.month(), 'date': this.attendanceDate.date() });
        // this.breakIn = moment(this.breakIn).set({ 'year': this.attendanceDate.year(), 'month': this.attendanceDate.month(), 'date': this.attendanceDate.date() });

        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["attendanceDate"] = this.attendanceDate ? moment(this.attendanceDate).toISOString(true) : <any>undefined;
        data["shiftID"] = this.shiftID;
        data["detID"] = this.detID;
        data["timeIn"] = this.timeIn ? moment(this.timeIn).toISOString(true) : <any>undefined;
        data["timeOut"] = this.timeOut ? moment(this.timeOut).toISOString(true) : <any>undefined;
        data["breakOut"] = this.breakOut ? moment(this.breakOut).toISOString(true) : <any>undefined;
        data["breakIn"] = this.breakIn ? moment(this.breakIn).toISOString(true) : <any>undefined;
        data["totalHrs"] = this.totalHrs;
        data["id"] = this.id;
        return data;
    }
}

