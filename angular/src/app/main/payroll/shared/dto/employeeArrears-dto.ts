import * as moment from 'moment';
import { IPagedResultDtoOfGetEmployeeArrearsForViewDto, IGetEmployeeArrearsForViewDto, IEmployeeArrearsDto, IGetEmployeeArrearsForEditOutput, ICreateOrEditEmployeeArrearsDto } from '../interface/employeeArrears-interface';

export class PagedResultDtoOfGetEmployeeArrearsForViewDto implements IPagedResultDtoOfGetEmployeeArrearsForViewDto {
    totalCount!: number | undefined;
    items!: GetEmployeeArrearsForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetEmployeeArrearsForViewDto) {
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
                    this.items!.push(GetEmployeeArrearsForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetEmployeeArrearsForViewDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetEmployeeArrearsForViewDto();
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

export class GetEmployeeArrearsForViewDto implements IGetEmployeeArrearsForViewDto {
    employeeArrears!: EmployeeArrearsDto | undefined;

    constructor(data?: IGetEmployeeArrearsForViewDto) {
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
            this.employeeArrears = data["employeeArrears"] ? EmployeeArrearsDto.fromJS(data["employeeArrears"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeeArrearsForViewDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new GetEmployeeArrearsForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["employeeArrears"] = this.employeeArrears ? this.employeeArrears.toJSON() : <any>undefined;
        return data; 
    }
}

export class EmployeeArrearsDto implements IEmployeeArrearsDto {
    arrearID!: number | undefined;
    employeeID!: number | undefined;
    employeeName!: string | undefined;
    salaryYear!: number | undefined;
    salaryMonth!: number | undefined;
    arrearDate!: moment.Moment | undefined;
    amount!: number | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IEmployeeArrearsDto) {
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
            this.arrearID = data["arrearID"]; 
            this.employeeID = data["employeeID"];
            this.employeeName = data["employeeName"];
            this.salaryYear = data["salaryYear"];
            this.salaryMonth = data["salaryMonth"];
            this.arrearDate = data["arrearDate"];
            this.amount = data["amount"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): EmployeeArrearsDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new EmployeeArrearsDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["arrearID"] = this.arrearID;
        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["salaryYear"] = this.salaryYear;
        data["salaryMonth"] = this.salaryMonth;
        data["arrearDate"] = this.arrearDate;
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

export class GetEmployeeArrearsForEditOutput implements IGetEmployeeArrearsForEditOutput {
    employeeArrears!: CreateOrEditEmployeeArrearsDto | undefined;

    constructor(data?: IGetEmployeeArrearsForEditOutput) {
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
            this.employeeArrears = data["employeeArrears"] ? CreateOrEditEmployeeArrearsDto.fromJS(data["employeeArrears"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeeArrearsForEditOutput {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new GetEmployeeArrearsForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["employeeArrears"] = this.employeeArrears ? this.employeeArrears.toJSON() : <any>undefined;
        return data; 
    }
}

export class CreateOrEditEmployeeArrearsDto implements ICreateOrEditEmployeeArrearsDto {
    arrearID!: number;
    employeeID!: number;
    employeeName!: string | undefined;
    salaryYear!: number;
    salaryMonth!: number;
    arrearDate!: moment.Moment | undefined;
    amount!: number | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditEmployeeArrearsDto) {
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
            this.arrearID = data["arrearID"];
            this.employeeID = data["employeeID"];
            this.employeeName = data["employeeName"];
            this.salaryYear = data["salaryYear"];
            this.salaryMonth = data["salaryMonth"];
            this.arrearDate = data["arrearDate"];
            this.amount = data["amount"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditEmployeeArrearsDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditEmployeeArrearsDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["arrearID"] = this.arrearID;
        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["salaryYear"] = this.salaryYear;
        data["salaryMonth"] = this.salaryMonth;
        data["arrearDate"] = this.arrearDate;
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