import * as moment from "moment";
import { IEmployeeAdvancesDto, IPagedResultDtoOfGetEmployeeAdvancesForViewDto, IGetEmployeeAdvancesForViewDto, ICreateOrEditEmployeeAdvancesDto, IGetEmployeeAdvancesForEditOutput } from "../interface/advances-interface";


export class PagedResultDtoOfGetEmployeeAdvancesForViewDto
    implements IPagedResultDtoOfGetEmployeeAdvancesForViewDto {
    totalCount!: number | undefined;
    items!: GetEmployeeAdvancesForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetEmployeeAdvancesForViewDto) {
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
                    this.items!.push(GetEmployeeAdvancesForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetEmployeeAdvancesForViewDto {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new PagedResultDtoOfGetEmployeeAdvancesForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === "object" ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items) data["items"].push(item.toJSON());
        }
        return data;
    }
}

export class GetEmployeeAdvancesForViewDto implements IGetEmployeeAdvancesForViewDto {
    employeeAdvances!: EmployeeAdvancesDto | undefined;

    constructor(data?: IGetEmployeeAdvancesForViewDto) {
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
            this.employeeAdvances = data["employeeAdvances"]
                ? EmployeeAdvancesDto.fromJS(data["employeeAdvances"])
                : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeeAdvancesForViewDto {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new GetEmployeeAdvancesForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === "object" ? data : {};
        data["employeeAdvances"] = this.employeeAdvances
            ? this.employeeAdvances.toJSON()
            : <any>undefined;
        return data;
    }
}

export class EmployeeAdvancesDto implements IEmployeeAdvancesDto {
    advanceID: number;
    employeeID: number;
    employeeName: string;
    remarks: string;
    salaryYear: number;
    salaryMonth: number;
    advanceDate: moment.Moment;
    amount: number;
    active: boolean;
    audtUser: string;
    audtDate: moment.Moment;
    createdBy: string;
    createDate: moment.Moment;
    id: number;


    constructor(data?: IEmployeeAdvancesDto) {
        debugger;
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
            this.advanceID = data["advanceID"];
            this.employeeID = data["employeeID"];
            this.employeeName = data["employeeName"];
            this.remarks = data["remarks"];
            this.salaryYear = data["salaryYear"];
            this.salaryMonth = data["salaryMonth"];
            this.advanceDate = data["advanceDate"];
            this.amount = data["amount"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): EmployeeAdvancesDto {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new EmployeeAdvancesDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["advanceID"] = this.advanceID;
        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["remarks"] = this.remarks;
        data["salaryYear"] = this.salaryYear;
        data["salaryMonth"] = this.salaryMonth;
        data["advanceDate"] = this.advanceDate;
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

export class CreateOrEditEmployeeAdvancesDto implements ICreateOrEditEmployeeAdvancesDto {
   
    constructor(data?: ICreateOrEditEmployeeAdvancesDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }
    id: number;
    advanceID: number;
    employeeID: number;
    employeeName: string;
    remarks: string;
    salaryYear: number;
    salaryMonth: number;
    advanceDate: moment.Moment;
    amount: number;
    active: boolean;
    audtUser: string;
    audtDate: moment.Moment;
    createdBy: string;
    createDate: moment.Moment;
    

    init(data?: any) {
        debugger;
        if (data) {
            this.advanceID = data["advanceID"];
            this.employeeID = data["employeeID"];
            this.employeeName = data["employeeName"];
            this.remarks = data["remarks"];
            this.salaryYear = data["salaryYear"];
            this.salaryMonth = data["salaryMonth"];
            this.advanceDate = data["advanceDate"];
            this.amount = data["amount"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditEmployeeAdvancesDto {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new CreateOrEditEmployeeAdvancesDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["advanceID"] = this.advanceID;
        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["remarks"] = this.remarks;
        data["salaryYear"] = this.salaryYear;
        data["salaryMonth"] = this.salaryMonth;
        data["advanceDate"] = this.advanceDate;
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

export class GetEmployeeAdvancesForEditOutput implements IGetEmployeeAdvancesForEditOutput {
    employeeAdvances!: CreateOrEditEmployeeAdvancesDto | undefined;

    constructor(data?: IGetEmployeeAdvancesForEditOutput) {
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
            this.employeeAdvances = data["employeeAdvances"]
                ? CreateOrEditEmployeeAdvancesDto.fromJS(data["employeeAdvances"])
                : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeeAdvancesForEditOutput {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new GetEmployeeAdvancesForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === "object" ? data : {};
        data["employeeAdvances"] = this.employeeAdvances
            ? this.employeeAdvances.toJSON()
            : <any>undefined;
        return data;
    }
}
