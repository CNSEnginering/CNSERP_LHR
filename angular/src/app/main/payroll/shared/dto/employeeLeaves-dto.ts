import * as moment from 'moment';
import { IPagedResultDtoOfGetEmployeeLeavesForViewDto, IGetEmployeeLeavesForViewDto, IEmployeeLeavesDto, IGetEmployeeLeavesForEditOutput, ICreateOrEditEmployeeLeavesDto } from '../interface/employeeLeaves-interface';

export class PagedResultDtoOfGetEmployeeLeavesForViewDto implements IPagedResultDtoOfGetEmployeeLeavesForViewDto {
    totalCount!: number | undefined;
    items!: GetEmployeeLeavesForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetEmployeeLeavesForViewDto) {
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
                    this.items!.push(GetEmployeeLeavesForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetEmployeeLeavesForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetEmployeeLeavesForViewDto();
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

export class GetEmployeeLeavesForViewDto implements IGetEmployeeLeavesForViewDto {
    employeeLeaves!: EmployeeLeavesDto | undefined;

    constructor(data?: IGetEmployeeLeavesForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.employeeLeaves = data["employeeLeaves"] ? EmployeeLeavesDto.fromJS(data["employeeLeaves"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeeLeavesForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetEmployeeLeavesForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["employeeLeaves"] = this.employeeLeaves ? this.employeeLeaves.toJSON() : <any>undefined;
        return data;
    }
}

export class EmployeeLeavesDto implements IEmployeeLeavesDto {
    employeeID!: number | undefined;
    leaveID!: number | undefined;
    employeeName!: string | undefined;
    deptName!: string | undefined;
    salaryYear!: number | undefined;
    salaryMonth!: number | undefined;
    startDate!: moment.Moment | undefined;
    leaveType!: number | undefined;
    casual!: number | undefined;
    sick!: number | undefined;
    annual!: number | undefined;
    payType!: string | undefined;
    remarks!: string | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IEmployeeLeavesDto) {
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
            this.leaveID = data["leaveID"];
            this.employeeName = data["employeeName"];
            this.deptName = data["deptName"];
            this.salaryYear = data["salaryYear"];
            this.salaryMonth = data["salaryMonth"];
            this.startDate = data["startDate"];
            this.leaveType = data["leaveType"];
            this.casual = data["casual"];
            this.sick = data["sick"];
            this.annual = data["annual"];
            this.payType = data["payType"];
            this.remarks = data["remarks"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): EmployeeLeavesDto {
        data = typeof data === 'object' ? data : {};
        let result = new EmployeeLeavesDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["employeeID"] = this.employeeID;
        data["leaveID"] = this.leaveID;
        data["employeeName"] = this.employeeName;
        data["deptName"] = this.deptName;
        data["salaryYear"] = this.salaryYear;
        data["salaryMonth"] = this.salaryMonth;
        data["startDate"] = this.startDate;
        data["leaveType"] = this.leaveType;
        data["casual"] = this.casual;
        data["sick"] = this.sick;
        data["annual"] = this.annual;
        data["payType"] = this.payType;
        data["remarks"] = this.remarks;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}

export class GetEmployeeLeavesForEditOutput implements IGetEmployeeLeavesForEditOutput {
    employeeLeaves!: CreateOrEditEmployeeLeavesDto | undefined;

    constructor(data?: IGetEmployeeLeavesForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.employeeLeaves = data["employeeLeaves"] ? CreateOrEditEmployeeLeavesDto.fromJS(data["employeeLeaves"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeeLeavesForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetEmployeeLeavesForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["employeeLeaves"] = this.employeeLeaves ? this.employeeLeaves.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditEmployeeLeavesDto implements ICreateOrEditEmployeeLeavesDto {
    employeeID!: number;
    leaveID!: number;
    salaryYear!: number;
    salaryMonth!: number | undefined;
    startDate!: moment.Moment | undefined;
    leaveType!: number | undefined;
    casual!: number | undefined;
    sick!: number | undefined;
    annual!: number | undefined;
    payType!: string | undefined;
    remarks!: string | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditEmployeeLeavesDto) {
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
            this.leaveID = data["leaveID"];
            this.salaryYear = data["salaryYear"];
            this.salaryMonth = data["salaryMonth"];
            this.startDate = data["startDate"];
            this.leaveType = data["leaveType"];
            this.casual = data["casual"];
            this.sick = data["sick"];
            this.annual = data["annual"];
            this.payType = data["payType"];
            this.remarks = data["remarks"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditEmployeeLeavesDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditEmployeeLeavesDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["employeeID"] = this.employeeID;
        data["leaveID"] = this.leaveID;
        data["salaryYear"] = this.salaryYear;
        data["salaryMonth"] = this.salaryMonth;
        data["startDate"] = this.startDate;
        data["leaveType"] = this.leaveType;
        data["casual"] = this.casual;
        data["sick"] = this.sick;
        data["annual"] = this.annual;
        data["payType"] = this.payType;
        data["remarks"] = this.remarks;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}
