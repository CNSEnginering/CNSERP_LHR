import * as moment from 'moment';
import {  IDesignationDto, IPagedResultDtoOfGetDesignationForViewDto, IGetDesignationForViewDto, IGetDesignationForEditOutput, ICreateOrEditDesignationDto } from '../interface/designation-interface';
export class PagedResultDtoOfGetDesignationForViewDto implements IPagedResultDtoOfGetDesignationForViewDto {
    totalCount!: number | undefined;
    items!: GetDesignationForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetDesignationForViewDto) {
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
                    this.items!.push(GetDesignationForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetDesignationForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetDesignationForViewDto();
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

export class GetDesignationForViewDto implements IGetDesignationForViewDto {
    designation!: DesignationDto | undefined;

    constructor(data?: IGetDesignationForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
       
        if (data) {
            this.designation = data["designation"] ? DesignationDto.fromJS(data["designation"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetDesignationForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetDesignationForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["designation"] = this.designation ? this.designation.toJSON() : <any>undefined;
        return data;
    }
}

export class DesignationDto implements IDesignationDto {
    designationID!: number | undefined;
    designation!: string | undefined;
    active!: boolean;;
    audtUser! :string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IDesignationDto) {
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
            this.designation = data["designation"];
            this.designationID = data["designationID"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): DesignationDto {
        debugger
        data = typeof data === 'object' ? data : {};
        let result = new DesignationDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["designation"] = this.designation;
        data["designationID"] = this.designationID;
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

export class GetDesignationForEditOutput implements IGetDesignationForEditOutput {
    designation!: CreateOrEditDesignationDto | undefined;

    constructor(data?: IGetDesignationForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.designation = data["designation"] ? CreateOrEditDesignationDto.fromJS(data["designation"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetDesignationForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetDesignationForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["designation"] = this.designation ? this.designation.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditDesignationDto implements ICreateOrEditDesignationDto {
    designationID!: number | undefined;
    designation!: string | undefined;
    active!: boolean;;
    audtUser! :string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;
    sortId!:number|undefined;

    constructor(data?: ICreateOrEditDesignationDto) {
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
            this.designation = data["designation"];
            this.designationID = data["designationID"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
            this.sortId=data["sortId"];
        }
    }

    static fromJS(data: any): CreateOrEditDesignationDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditDesignationDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["designation"] = this.designation;
        data["designationID"] = this.designationID;
        data["active"]=this.active;
        data["audtUser"]=this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"]=this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        data["sortId"]=this.sortId;
        return data;
    }
}