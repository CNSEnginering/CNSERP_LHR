import * as moment from 'moment';
import { IPagedResultDtoOfGetEmployeeSalaryForViewDto, IGetEmployeeSalaryForViewDto, IEmployeeSalaryDto, IGetEmployeeSalaryForEditOutput, ICreateOrEditEmployeeSalaryDto, IPagedResultDtoOfEmployeeSalaryDto } from '../interface/employeeSalary-interface';

export class PagedResultDtoOfGetEmployeeSalaryForViewDto implements IPagedResultDtoOfGetEmployeeSalaryForViewDto {
    totalCount!: number | undefined;
    items!: GetEmployeeSalaryForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetEmployeeSalaryForViewDto) {
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
                    this.items!.push(GetEmployeeSalaryForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetEmployeeSalaryForViewDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetEmployeeSalaryForViewDto();
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

export class GetEmployeeSalaryForViewDto implements IGetEmployeeSalaryForViewDto {
    employeeSalary!: EmployeeSalaryDto | undefined;

    constructor(data?: IGetEmployeeSalaryForViewDto) {
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
            this.employeeSalary = data["employeeSalary"] ? EmployeeSalaryDto.fromJS(data["employeeSalary"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeeSalaryForViewDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new GetEmployeeSalaryForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["employeeSalary"] = this.employeeSalary ? this.employeeSalary.toJSON() : <any>undefined;
        return data; 
    }
}

export class GetEmployeeSalaryForEditOutput implements IGetEmployeeSalaryForEditOutput {
    employeeSalary!: CreateOrEditEmployeeSalaryDto | undefined;

    constructor(data?: IGetEmployeeSalaryForEditOutput) {
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
            this.employeeSalary = data["employeeSalary"] ? CreateOrEditEmployeeSalaryDto.fromJS(data["employeeSalary"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeeSalaryForEditOutput {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new GetEmployeeSalaryForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["employeeSalary"] = this.employeeSalary ? this.employeeSalary.toJSON() : <any>undefined;
        return data; 
    }
}

export class CreateOrEditEmployeeSalaryDto implements ICreateOrEditEmployeeSalaryDto {
    employeeID!: number | undefined;
    employeeName!: string | undefined;
    bank_Amount!: number | undefined;
    startDate!: moment.Moment | undefined;
    gross_Salary!: number | undefined;
    basic_Salary!: number | undefined;
    tax!: number | undefined;
    house_Rent!: number | undefined;
    net_Salary!: number | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditEmployeeSalaryDto) {
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
            this.employeeID = data["employeeID"];
            this.employeeName = data["employeeName"];
            this.bank_Amount = data["bank_Amount"];
            this.startDate = data["startDate"];
            this.gross_Salary = data["gross_Salary"];
            this.basic_Salary = data["basic_Salary"];
            this.tax = data["tax"];
            this.house_Rent = data["house_Rent"];
            this.net_Salary = data["net_Salary"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditEmployeeSalaryDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditEmployeeSalaryDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["bank_Amount"] = this.bank_Amount;
        data["startDate"] = this.startDate;
        data["gross_Salary"] = this.gross_Salary;
        data["basic_Salary"] = this.basic_Salary;
        data["tax"] = this.tax;
        data["house_Rent"] = this.house_Rent;
        data["net_Salary"] = this.net_Salary;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data; 
    }
}

export class EmployeeSalaryDto implements IEmployeeSalaryDto {
    employeeID!: number | undefined;
    employeeName!: string | undefined;
    bank_Amount!: number | undefined;
    startDate!: moment.Moment | undefined;
    gross_Salary!: number | undefined;
    basic_Salary!: number | undefined;
    tax!: number | undefined;
    house_Rent!: number | undefined;
    net_Salary!: number | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IEmployeeSalaryDto) {
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
            this.employeeID = data["employeeID"];
            this.employeeName = data["employeeName"];
            this.bank_Amount = data["bank_Amount"];
            this.startDate = data["startDate"];
            this.gross_Salary = data["gross_Salary"];
            this.basic_Salary = data["basic_Salary"];
            this.tax = data["tax"];
            this.house_Rent = data["house_Rent"];
            this.net_Salary = data["net_Salary"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): EmployeeSalaryDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new EmployeeSalaryDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["bank_Amount"] = this.bank_Amount;
        data["startDate"] = this.startDate;
        data["gross_Salary"] = this.gross_Salary;
        data["basic_Salary"] = this.basic_Salary;
        data["tax"] = this.tax;
        data["house_Rent"] = this.house_Rent;
        data["net_Salary"] = this.net_Salary;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data; 
    }
}

export class PagedResultDtoOfEmployeeSalaryDto implements IPagedResultDtoOfEmployeeSalaryDto {
    totalCount!: number | undefined;
    items!: EmployeeSalaryDto[] | undefined;

    constructor(data?: IPagedResultDtoOfEmployeeSalaryDto) {
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
                    this.items!.push(EmployeeSalaryDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfEmployeeSalaryDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfEmployeeSalaryDto();
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