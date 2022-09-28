import * as moment from 'moment';
import { IPagedResultDtoOfGetEmployeeDeductionsForViewDto, IGetEmployeeDeductionsForViewDto, IEmployeeDeductionsDto, IGetEmployeeDeductionsForEditOutput, ICreateOrEditEmployeeDeductionsDto } from '../interface/employeeDeductions-interface';

export class PagedResultDtoOfGetEmployeeDeductionsForViewDto implements IPagedResultDtoOfGetEmployeeDeductionsForViewDto {
    totalCount!: number | undefined;
    items!: GetEmployeeDeductionsForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetEmployeeDeductionsForViewDto) {
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
                    this.items!.push(GetEmployeeDeductionsForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetEmployeeDeductionsForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetEmployeeDeductionsForViewDto();
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

export class GetEmployeeDeductionsForViewDto implements IGetEmployeeDeductionsForViewDto {
    employeeDeductions!: EmployeeDeductionsDto | undefined;

    constructor(data?: IGetEmployeeDeductionsForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.employeeDeductions = data["employeeDeductions"] ? EmployeeDeductionsDto.fromJS(data["employeeDeductions"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeeDeductionsForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetEmployeeDeductionsForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["employeeDeductions"] = this.employeeDeductions ? this.employeeDeductions.toJSON() : <any>undefined;
        return data;
    }
}

export class EmployeeDeductionsDto implements IEmployeeDeductionsDto {
    deductionID!: number | undefined;
    employeeID!: number | undefined;
    employeeName!: string | undefined;
    deductionType!: number | undefined;
    remarks!: string | undefined;
    salaryYear!: number | undefined;
    salaryMonth!: number | undefined;
    deductionDate!: moment.Moment | undefined;
    amount!: number | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IEmployeeDeductionsDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.deductionID = data["deductionID"];
            this.employeeID = data["employeeID"];
            this.employeeName = data["employeeName"];
            this.deductionType = data["deductionType"];
            this.remarks = data["remarks"];
            this.salaryYear = data["salaryYear"];
            this.salaryMonth = data["salaryMonth"];
            this.deductionDate = data["deductionDate"];
            this.amount = data["amount"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): EmployeeDeductionsDto {
        data = typeof data === 'object' ? data : {};
        let result = new EmployeeDeductionsDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["deductionID"] = this.deductionID;
        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["deductionType"] = this.deductionType;
        data["remarks"] = this.remarks;
        data["salaryYear"] = this.salaryYear;
        data["salaryMonth"] = this.salaryMonth;
        data["deductionDate"] = this.deductionDate;
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

export class GetEmployeeDeductionsForEditOutput implements IGetEmployeeDeductionsForEditOutput {
    employeeDeductions!: CreateOrEditEmployeeDeductionsDto | undefined;

    constructor(data?: IGetEmployeeDeductionsForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.employeeDeductions = data["employeeDeductions"] ? CreateOrEditEmployeeDeductionsDto.fromJS(data["employeeDeductions"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeeDeductionsForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetEmployeeDeductionsForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["employeeDeductions"] = this.employeeDeductions ? this.employeeDeductions.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditEmployeeDeductionsDto implements ICreateOrEditEmployeeDeductionsDto {
    deductionID!: number;
    employeeID!: number;
    employeeName!: string | undefined;
    deductionType!: number | undefined;
    remarks!: string | undefined;
    salaryYear!: number;
    salaryMonth!: number;
    deductionDate!: Date | undefined;
    amount!: number | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!:  Date | undefined;
    createdBy!: string | undefined;
    createDate!:  Date| undefined;
    id!: number | undefined;
    detId!:number | undefined;

    constructor(data?: ICreateOrEditEmployeeDeductionsDto) {
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
            this.deductionID = data["deductionID"];
            this.employeeID = data["employeeID"];
            this.employeeName = data["employeeName"];
            this.deductionType = data["deductionType"];
            this.remarks = data["remarks"];
            this.salaryYear = data["salaryYear"];
            this.salaryMonth = data["salaryMonth"];
            this.deductionDate = data["deductionDate"];
            this.amount = data["amount"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
            this.detId = data["detId"];
        }
    }

    static fromJS(data: any): CreateOrEditEmployeeDeductionsDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditEmployeeDeductionsDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["deductionID"] = this.deductionID;
        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["deductionType"] = this.deductionType;
        data["remarks"] = this.remarks;
        data["salaryYear"] = this.salaryYear;
        data["salaryMonth"] = this.salaryMonth;
        data["deductionDate"] = this.deductionDate;
        data["amount"] = this.amount;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        data["detId"]= this.detId;
        return data;
    }
}





