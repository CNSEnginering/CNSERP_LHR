import * as moment from 'moment';
import { IemployeeLeaveBalanceDto, IPagedResultDtoOfGetemployeeLeaveBalanceForViewDto, IGetemployeeLeaveBalanceForViewDto, IGetemployeeLeaveBalanceForEditOutput, ICreateOrEditemployeeLeaveBalanceDto } from '../interface/employeeLeaveBalance-interface';
export class PagedResultDtoOfGetemployeeLeaveBalanceForViewDto implements IPagedResultDtoOfGetemployeeLeaveBalanceForViewDto {
    totalCount!: number | undefined;
    items!: GetemployeeLeaveBalanceForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetemployeeLeaveBalanceForViewDto) {
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
                    this.items!.push(GetemployeeLeaveBalanceForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetemployeeLeaveBalanceForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetemployeeLeaveBalanceForViewDto();
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

export class GetemployeeLeaveBalanceForViewDto implements IGetemployeeLeaveBalanceForViewDto {
    employeeLeavesTotal!: employeeLeaveBalanceDto | undefined;

    constructor(data?: IGetemployeeLeaveBalanceForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        console.log(data);
        if (data) {
            this.employeeLeavesTotal = data["employeeLeavesTotal"] ? employeeLeaveBalanceDto.fromJS(data["employeeLeavesTotal"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetemployeeLeaveBalanceForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetemployeeLeaveBalanceForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["employeeLeavesTotal"] = this.employeeLeavesTotal ? this.employeeLeavesTotal.toJSON() : <any>undefined;
        return data;
    }
}

export class employeeLeaveBalanceDto implements IemployeeLeaveBalanceDto {
    employeeID: number | undefined;
    salaryYear: number | undefined;
    leaves: number | undefined;
    casual: number;
    sick: number;
    annual: number;
    cpl: number;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;

    constructor(data?: IemployeeLeaveBalanceDto) {
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
            this.casual = data["casual"];
            this.salaryYear = data["salaryYear"];
            this.leaves = data["leaves"];
            this.sick = data["sick"];
            this.cpl = data["cpl"];
            this.annual = data["annual"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): employeeLeaveBalanceDto {
        data = typeof data === 'object' ? data : {};
        let result = new employeeLeaveBalanceDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["employeeID"] = this.employeeID;
        data["salaryYear"] = this.salaryYear;
        data["leaves"] = this.leaves;
        data["casual"] = this.casual;
        data["sick"] = this.sick;
        data["cpl"] = this.cpl;
        data["annual"] = this.annual;
        data["id"] = this.id;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        return data;
    }
}

export class GetemployeeLeaveBalanceForEditOutput implements IGetemployeeLeaveBalanceForEditOutput {
    employeeLeavesTotal!: CreateOrEditemployeeLeaveBalanceDto | undefined;
    constructor(data?: IGetemployeeLeaveBalanceForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.employeeLeavesTotal = data["employeeLeavesTotal"] ? CreateOrEditemployeeLeaveBalanceDto.fromJS(data["employeeLeavesTotal"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetemployeeLeaveBalanceForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetemployeeLeaveBalanceForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["employeeLeavesTotal"] = this.employeeLeavesTotal ? this.employeeLeavesTotal.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditemployeeLeaveBalanceDto implements ICreateOrEditemployeeLeaveBalanceDto {
    employeeID: number | undefined;
    employeeName: string | undefined;
    leaves: number | undefined;
    salaryYear: number | undefined;
    casual: number;
    sick: number;
    annual: number;
    cpl: number;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;

    constructor(data?: ICreateOrEditemployeeLeaveBalanceDto) {
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
            this.casual = data["casual"];
            this.leaves = data["leaves"];
            this.salaryYear = data["salaryYear"];
            this.sick = data["sick"];
            this.cpl = data["cpl"];
            this.annual = data["annual"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditemployeeLeaveBalanceDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditemployeeLeaveBalanceDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["salaryYear"] = this.salaryYear;
        data["leaves"] = this.leaves;
        data["casual"] = this.casual;
        data["sick"] = this.sick;
        data["cpl"] = this.cpl;
        data["annual"] = this.annual;
        data["id"] = this.id;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        return data;
    }
}