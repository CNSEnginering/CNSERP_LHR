import * as moment from 'moment';
import { IPagedResultDtoOfGetEmployeeTypeForViewDto, IGetEmployeeTypeForViewDto, IEmployeeTypeDto, IGetEmployeeTypeForEditOutput, ICreateOrEditEmployeeTypeDto } from '../interface/employeeType-interface';


export class PagedResultDtoOfGetEmployeeTypeForViewDto implements IPagedResultDtoOfGetEmployeeTypeForViewDto {
    totalCount!: number | undefined;
    items!: GetEmployeeTypeForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetEmployeeTypeForViewDto) {
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
                    this.items!.push(GetEmployeeTypeForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetEmployeeTypeForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetEmployeeTypeForViewDto();
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

export class GetEmployeeTypeForViewDto implements IGetEmployeeTypeForViewDto {
    employeeType!: EmployeeTypeDto | undefined;

    constructor(data?: IGetEmployeeTypeForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.employeeType = data["employeeType"] ? EmployeeTypeDto.fromJS(data["employeeType"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeeTypeForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetEmployeeTypeForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["employeeType"] = this.employeeType ? this.employeeType.toJSON() : <any>undefined;
        return data;
    }
}

export class EmployeeTypeDto implements IEmployeeTypeDto {
    typeID!: number | undefined;
    empType!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IEmployeeTypeDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.typeID = data["typeID"];
            this.empType = data["empType"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): EmployeeTypeDto {
        data = typeof data === 'object' ? data : {};
        let result = new EmployeeTypeDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["typeID"] = this.typeID;
        data["empType"] = this.empType;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}

export class GetEmployeeTypeForEditOutput implements IGetEmployeeTypeForEditOutput {
    employeeType!: CreateOrEditEmployeeTypeDto | undefined;

    constructor(data?: IGetEmployeeTypeForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.employeeType = data["employeeType"] ? CreateOrEditEmployeeTypeDto.fromJS(data["employeeType"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeeTypeForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetEmployeeTypeForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["employeeType"] = this.employeeType ? this.employeeType.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditEmployeeTypeDto implements ICreateOrEditEmployeeTypeDto {
    typeID!: number;
    empType!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditEmployeeTypeDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.typeID = data["typeID"];
            this.empType = data["empType"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditEmployeeTypeDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditEmployeeTypeDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["typeID"] = this.typeID;
        data["empType"] = this.empType;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}
