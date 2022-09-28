import * as moment from "moment";
import {
    IPagedResultDtoOfGetSalaryLoanStopForViewDto,
    IGetSalaryLoanStopForViewDto,
    ISalaryLoanStopDto,
    IGetSalaryLoanStopForEditOutput,
    ICreateOrEditSalaryLoanStopDto,
} from "../interface/SalaryLoanStop-interface";

export class PagedResultDtoOfGetSalaryLoanStopForViewDto
    implements IPagedResultDtoOfGetSalaryLoanStopForViewDto {
    totalCount!: number | undefined;
    items!: GetSalaryLoanStopForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetSalaryLoanStopForViewDto) {
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
                    this.items!.push(GetSalaryLoanStopForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetSalaryLoanStopForViewDto {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new PagedResultDtoOfGetSalaryLoanStopForViewDto();
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

export class GetSalaryLoanStopForViewDto
    implements IGetSalaryLoanStopForViewDto {
    SalaryLoanStop!: SalaryLoanStopDto | undefined;

    constructor(data?: IGetSalaryLoanStopForViewDto) {
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
            this.SalaryLoanStop = data["stopSalary"]
                ? SalaryLoanStopDto.fromJS(data["stopSalary"])
                : <any>undefined;
        }
    }

    static fromJS(data: any): GetSalaryLoanStopForViewDto {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new GetSalaryLoanStopForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === "object" ? data : {};
        data["stopSalary"] = this.SalaryLoanStop
            ? this.SalaryLoanStop.toJSON()
            : <any>undefined;
        return data;
    }
}

export class SalaryLoanStopDto implements ISalaryLoanStopDto {
    typeID!: number | undefined;
    employeeID!: number | undefined;
    salaryYear!: number | undefined;
    salaryMonth!: number | undefined;
    active!: boolean | undefined;
    id!: number | undefined;
    include!: boolean | undefined;
    remarks!: string | undefined;
    loanID!: number | undefined;

    constructor(data?: ISalaryLoanStopDto) {
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
            this.typeID = data["typeID"];
            this.employeeID = data["employeeID"];
            this.salaryYear = data["salaryYear"];
            this.salaryMonth = data["salaryMonth"];
            this.active = data["active"];
            this.remarks = data["remarks"];
            this.id = data["id"];
            this.include = data["active"];
            this.loanID = data["loanID"];
        }
    }

    static fromJS(data: any): SalaryLoanStopDto {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new SalaryLoanStopDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === "object" ? data : {};
        data["typeID"] = this.typeID;
        data["employeeID"] = this.employeeID;
        data["salaryYear"] = this.salaryYear;
        data["salaryMonth"] = this.salaryMonth;
        data["active"] = this.active;
        data["remarks"] = this.remarks;
        data["id"] = this.id;
        data["active"] = this.include;
        data["loanID"] = this.loanID;
        return data;
    }
}

export class CreateOrEditSalaryLoanStopDto
    implements ICreateOrEditSalaryLoanStopDto {
    typeID!: number;
    employeeID!: number | undefined;
    salaryYear!: number | undefined;
    salaryMonth!: number | undefined;
    id!: number | undefined;
    active!: boolean | undefined;
    remarks!: string | undefined;
    include!: boolean | undefined;
    loanID!: number | undefined;

    constructor(data?: ICreateOrEditSalaryLoanStopDto) {
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
            this.typeID = data["typeID"];
            this.employeeID = data["employeeID"];
            this.salaryYear = data["salaryYear"];
            this.salaryMonth = data["salaryMonth"];
            this.active = data["active"];
            this.id = data["id"];
            this.remarks = data["remarks"];
            this.include = data["active"];
            this.loanID = data["loanID"];
        }
    }

    static fromJS(data: any): CreateOrEditSalaryLoanStopDto {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new CreateOrEditSalaryLoanStopDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === "object" ? data : {};
        data["typeID"] = this.typeID;
        data["employeeID"] = this.employeeID;
        data["salaryYear"] = this.salaryYear;
        data["salaryMonth"] = this.salaryMonth;
        data["active"] = this.active;
        data["remarks"] = this.remarks;
        data["id"] = this.id;
        data["active"] = this.include;
        data["loanID"] = this.loanID;
        return data;
    }
}

export class GetSalaryLoanStopForEditOutput
    implements IGetSalaryLoanStopForEditOutput {
    SalaryLoanStop!: CreateOrEditSalaryLoanStopDto | undefined;

    constructor(data?: IGetSalaryLoanStopForEditOutput) {
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
            this.SalaryLoanStop = data["stopSalary"]
                ? CreateOrEditSalaryLoanStopDto.fromJS(data["stopSalary"])
                : <any>undefined;
        }
    }

    static fromJS(data: any): GetSalaryLoanStopForEditOutput {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new GetSalaryLoanStopForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === "object" ? data : {};
        data["stopSalary"] = this.SalaryLoanStop
            ? this.SalaryLoanStop.toJSON()
            : <any>undefined;
        return data;
    }
}
