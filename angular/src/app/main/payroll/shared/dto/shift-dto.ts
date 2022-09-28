import * as moment from 'moment';
import {  IShiftDto, IPagedResultDtoOfGetShiftForViewDto, IGetShiftForViewDto, IGetShiftForEditOutput, ICreateOrEditShiftDto } from '../interface/shift-interface';
export class PagedResultDtoOfGetShiftForViewDto implements IPagedResultDtoOfGetShiftForViewDto {
    totalCount!: number | undefined;
    items!: GetShiftForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetShiftForViewDto) {
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
                    this.items!.push(GetShiftForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetShiftForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetShiftForViewDto();
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

export class GetShiftForViewDto implements IGetShiftForViewDto {
    shift!: ShiftDto | undefined;

    constructor(data?: IGetShiftForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
       
        if (data) {
            this.shift = data["shift"] ? ShiftDto.fromJS(data["shift"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetShiftForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetShiftForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["shift"] = this.shift ? this.shift.toJSON() : <any>undefined;
        return data;
    }
}

export class ShiftDto implements IShiftDto {
    shiftID!: number | undefined;
    shiftName!: string | undefined;
    startTime!: moment.Moment | undefined;
    endTime!: moment.Moment | undefined;
    beforeStart!: number | undefined;
    afterStart!: number | undefined;
    beforeFinish!: number | undefined;
    afterFinish!: number | undefined;
    totalHour!: number | undefined;
    active!:  boolean;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IShiftDto) {
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
            this.shiftName = data["shiftName"];
            this.shiftID = data["shiftID"];
            this.startTime = data["startTime"];
            this.endTime = data["endTime"];
            this.beforeStart = data["beforeStart"];
            this.beforeFinish = data["beforeFinish"];
            this.afterStart = data["afterStart"];
            this.afterFinish = data["afterFinish"];
            this.totalHour = data["totalHour"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): ShiftDto {
        debugger
        data = typeof data === 'object' ? data : {};
        let result = new ShiftDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["shiftName"] = this.shiftName;
        data["shiftID"] = this.shiftID;
        data["startTime"] = this.startTime;
        data["endTime"] = this.endTime;
        data["beforeStart"] = this.beforeStart
        data["beforeFinish"] = this.beforeFinish;
        data["afterStart"] = this.afterStart;
        data["afterFinish"] = this.afterFinish;
        data["totalHour"] = this.totalHour;
        data["active"]=this.active;
        data["id"] = this.id;
        data["audtUser"]=this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"]=this.createdBy;
        data["createDate"] = this.createDate;
        debugger;
        return data;
    }
}

export class GetShiftForEditOutput implements IGetShiftForEditOutput {
    shift!: CreateOrEditShiftDto | undefined;

    constructor(data?: IGetShiftForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.shift = data["shift"] ? CreateOrEditShiftDto.fromJS(data["shift"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetShiftForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetShiftForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["shift"] = this.shift ? this.shift.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditShiftDto implements ICreateOrEditShiftDto {
    shiftID!: number | undefined;
    shiftName!: string | undefined;
    startTime!: moment.Moment | undefined;
    endTime!: moment.Moment | undefined;
    beforeStart!: number | undefined;
    afterStart!: number | undefined;
    beforeFinish!: number | undefined;
    afterFinish!: number | undefined;
    totalHour!: number | undefined;
    active!:  boolean;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditShiftDto) {
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
            this.shiftName = data["shiftName"];
            this.shiftID = data["shiftID"];
            this.startTime = data["startTime"] ;
            this.endTime = data["endTime"];
            this.beforeStart = data["beforeStart"];
            this.beforeFinish = data["beforeFinish"];
            this.afterStart = data["afterStart"];
            this.afterFinish = data["afterFinish"];
            this.totalHour = data["totalHour"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditShiftDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditShiftDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["shiftName"] = this.shiftName;
        data["shiftID"] = this.shiftID;
        data["startTime"] = this.startTime ? moment(this.startTime).toISOString(true): <any>undefined;
        data["endTime"] = this.endTime ? moment(this.endTime).toISOString(true): <any>undefined;
        data["beforeStart"] = this.beforeStart
        data["beforeFinish"] = this.beforeFinish;
        data["afterStart"] = this.afterStart;
        data["afterFinish"] = this.afterFinish;
        data["totalHour"] = this.totalHour;
        data["active"]=this.active;
        data["id"] = this.id;
        data["audtUser"]=this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"]=this.createdBy;
        data["createDate"] = this.createDate;
        return data;
    }
}