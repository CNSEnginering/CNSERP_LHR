import * as moment from 'moment';
import { IPagedResultDtoOfGetEmployeeEarningsForViewDto, IGetEmployeeEarningsForViewDto, IEmployeeEarningsDto, IGetEmployeeEarningsForEditOutput, ICreateOrEditEmployeeEarningsDto } from '../interface/employeeEarnings-interface';

export class PagedResultDtoOfGetEmployeeEarningsForViewDto implements IPagedResultDtoOfGetEmployeeEarningsForViewDto {
    totalCount!: number | undefined;
    items!: GetEmployeeEarningsForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetEmployeeEarningsForViewDto) {
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
            this.totalCount = data["totalCount"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [] as any;
                for (let item of data["items"])
                    this.items!.push(GetEmployeeEarningsForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetEmployeeEarningsForViewDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetEmployeeEarningsForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
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

export class GetEmployeeEarningsForViewDto implements IGetEmployeeEarningsForViewDto {
    employeeEarnings!: EmployeeEarningsDto | undefined;

    constructor(data?: IGetEmployeeEarningsForViewDto) {
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
            this.employeeEarnings = data["employeeEarnings"] ? EmployeeEarningsDto.fromJS(data["employeeEarnings"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeeEarningsForViewDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new GetEmployeeEarningsForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["employeeEarnings"] = this.employeeEarnings ? this.employeeEarnings.toJSON() : <any>undefined;
        return data;
    }
}

export class EmployeeEarningsDto implements IEmployeeEarningsDto {
    earningID!: number | undefined;
    employeeID!: number | undefined;
    earningTypeID!: number | undefined;
    remarks!: string | undefined;
    employeeName!: string | undefined;
    salaryYear!: number | undefined;
    salaryMonth!: number | undefined;
    earningDate!: moment.Moment | undefined;
    amount!: number | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IEmployeeEarningsDto) {
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
            this.earningID = data["earningID"];
            this.employeeID = data["employeeID"];
            this.earningTypeID = data["earningTypeID"];
            this.remarks = data["remarks"];
            this.employeeName = data["employeeName"];
            this.salaryYear = data["salaryYear"];
            this.salaryMonth = data["salaryMonth"];
            this.earningDate = data["earningDate"];
            this.amount = data["amount"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): EmployeeEarningsDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new EmployeeEarningsDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["earningID"] = this.earningID;
        data["employeeID"] = this.employeeID;
        data["earningTypeID"] = this.earningTypeID;
        data["remarks"] = this.remarks;
        data["employeeName"] = this.employeeName;
        data["salaryYear"] = this.salaryYear;
        data["salaryMonth"] = this.salaryMonth;
        data["earningDate"] = this.earningDate;
        data["amount"] = this.amount;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}

export class GetEmployeeEarningsForEditOutput implements IGetEmployeeEarningsForEditOutput {
    employeeEarnings!: CreateOrEditEmployeeEarningsDto | undefined;

    constructor(data?: IGetEmployeeEarningsForEditOutput) {
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
            this.employeeEarnings = data["employeeEarnings"] ? CreateOrEditEmployeeEarningsDto.fromJS(data["employeeEarnings"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeeEarningsForEditOutput {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new GetEmployeeEarningsForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["employeeEarnings"] = this.employeeEarnings ? this.employeeEarnings.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditEmployeeEarningsDto implements ICreateOrEditEmployeeEarningsDto {
    earningID!: number;
    employeeID!: number;
    earningTypeID!: number | undefined;
    remarks!: string | undefined;
    employeeName!: string | undefined;
    salaryYear!: number;
    salaryMonth!: number;
    earningDate!: moment.Moment | undefined;
    amount!: number | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditEmployeeEarningsDto) {
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
            this.earningID = data["earningID"];
            this.employeeID = data["employeeID"];
            this.earningTypeID = data["earningTypeID"];
            this.remarks = data["remarks"];
            this.employeeName = data["employeeName"];
            this.salaryYear = data["salaryYear"];
            this.salaryMonth = data["salaryMonth"];
            this.earningDate = data["earningDate"];
            this.amount = data["amount"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditEmployeeEarningsDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditEmployeeEarningsDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["earningID"] = this.earningID;
        data["earningTypeID"] = this.earningTypeID;
        data["remarks"] = this.remarks;
        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["salaryYear"] = this.salaryYear;
        data["salaryMonth"] = this.salaryMonth;
        data["earningDate"] = this.earningDate;
        data["amount"] = this.amount;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}
