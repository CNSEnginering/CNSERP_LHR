import * as moment from 'moment';
import { IAttendanceHeaderDto, ICreateOrEditAttendanceHeaderDto, IPagedResultDtoOfAttendanceHeaderDto, IGetAttendanceHeaderForEditOutput, IGetAttendanceHeaderForViewDto, IPagedResultDtoOfGetAttendanceHeaderForViewDto } from '../interface/attendanceHeader-interface';
import { CreateOrEditAttendanceDetailDto } from './attendanceDetail-dto';

export class PagedResultDtoOfAttendanceHeaderDto implements IPagedResultDtoOfAttendanceHeaderDto {
    totalCount!: number | undefined;
    items!: AttendanceHeaderDto[] | undefined;

    constructor(data?: IPagedResultDtoOfAttendanceHeaderDto) {
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
                    this.items!.push(AttendanceHeaderDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfAttendanceHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfAttendanceHeaderDto();
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

export class AttendanceHeaderDto implements IAttendanceHeaderDto {
    docDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IAttendanceHeaderDto) {
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
            this.docDate = data["docDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): AttendanceHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new AttendanceHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["docDate"] = this.docDate;
        data["id"] = this.id;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        return data;
    }
}

export class CreateOrEditAttendanceHeaderDto implements ICreateOrEditAttendanceHeaderDto {
    flag!: boolean | undefined
    attendanceDetail!: CreateOrEditAttendanceDetailDto[] | undefined;
    docDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditAttendanceHeaderDto) {
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
            this.docDate = data["docDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
            this.attendanceDetail = [] as any;
            for (let item of data["attendanceDetail"])
                this.attendanceDetail!.push(CreateOrEditAttendanceDetailDto.fromJS(item));
        }
    }

    static fromJS(data: any): CreateOrEditAttendanceHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditAttendanceHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["flag"] = this.flag;
        data["docDate"] = this.docDate ? moment(this.docDate).toISOString(true) : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        if (this.attendanceDetail && this.attendanceDetail.constructor === Array) {
            data["attendanceDetail"] = [];
            for (let item of this.attendanceDetail)
                data["attendanceDetail"].push(item);
        }
        return data;
    }
}

export class PagedResultDtoOfGetAttendanceHeaderForViewDto implements IPagedResultDtoOfGetAttendanceHeaderForViewDto {
    totalCount!: number | undefined;
    items!: GetAttendanceHeaderForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetAttendanceHeaderForViewDto) {
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
                    this.items!.push(GetAttendanceHeaderForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetAttendanceHeaderForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetAttendanceHeaderForViewDto();
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

export class GetAttendanceHeaderForViewDto implements IGetAttendanceHeaderForViewDto {
    attendanceHeader!: AttendanceHeaderDto | undefined;

    constructor(data?: IGetAttendanceHeaderForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        debugger;
        if (data) {
            this.attendanceHeader = data["attendanceHeader"] ? AttendanceHeaderDto.fromJS(data["attendanceHeader"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetAttendanceHeaderForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetAttendanceHeaderForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["attendanceHeader"] = this.attendanceHeader ? this.attendanceHeader.toJSON() : <any>undefined;
        return data;
    }
}

export class GetAttendanceHeaderForEditOutput implements IGetAttendanceHeaderForEditOutput {

    attendanceHeader: CreateOrEditAttendanceHeaderDto | undefined;


    constructor(data?: IGetAttendanceHeaderForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        debugger;
        if (data) {
            this.attendanceHeader = data["attendanceHeader"] ? CreateOrEditAttendanceHeaderDto.fromJS(data["attendanceHeader"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetAttendanceHeaderForEditOutput {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new GetAttendanceHeaderForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};

        data["attendanceHeader"] = this.attendanceHeader ? this.attendanceHeader.toJSON() : <any>undefined;
        return data;
    }
}
