import * as moment from 'moment';
import { IPagedResultDtoOfGetHolidaysForViewDto, IGetHolidaysForViewDto, IHolidaysDto, IGetHolidaysForEditOutput, ICreateOrEditHolidaysDto } from '../interface/holidays-interface';


export class PagedResultDtoOfGetHolidaysForViewDto implements IPagedResultDtoOfGetHolidaysForViewDto {
    totalCount!: number | undefined;
    items!: GetHolidaysForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetHolidaysForViewDto) {
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
                    this.items!.push(GetHolidaysForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetHolidaysForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetHolidaysForViewDto();
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

export class GetHolidaysForViewDto implements IGetHolidaysForViewDto {
    holidays!: HolidaysDto | undefined;

    constructor(data?: IGetHolidaysForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.holidays = data["holidays"] ? HolidaysDto.fromJS(data["holidays"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetHolidaysForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetHolidaysForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["holidays"] = this.holidays ? this.holidays.toJSON() : <any>undefined;
        return data;
    }
}

export class HolidaysDto implements IHolidaysDto {
    holidayID!: number | undefined;
    holidayDate!: moment.Moment | undefined;
    holidayName!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IHolidaysDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.holidayID = data["holidayID"];
            this.holidayDate = data["holidayDate"];
            this.holidayName = data["holidayName"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): HolidaysDto {
        data = typeof data === 'object' ? data : {};
        let result = new HolidaysDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["holidayID"] = this.holidayID;
        data["holidayDate"] = this.holidayDate ? moment(this.holidayDate).toISOString(true) : <any>undefined;
        data["holidayName"] = this.holidayName;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}

export class GetHolidaysForEditOutput implements IGetHolidaysForEditOutput {
    holidays!: CreateOrEditHolidaysDto | undefined;

    constructor(data?: IGetHolidaysForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.holidays = data["holidays"] ? CreateOrEditHolidaysDto.fromJS(data["holidays"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetHolidaysForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetHolidaysForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["holidays"] = this.holidays ? this.holidays.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditHolidaysDto implements ICreateOrEditHolidaysDto {
    holidayID!: number;
    holidayDate!: moment.Moment;
    holidayName!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditHolidaysDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.holidayID = data["holidayID"];
            this.holidayDate = data["holidayDate"];
            this.holidayName = data["holidayName"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditHolidaysDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditHolidaysDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["holidayID"] = this.holidayID;
        data["holidayDate"] = this.holidayDate ? moment(this.holidayDate).toISOString(true) : <any>undefined;
        data["holidayName"] = this.holidayName;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}

