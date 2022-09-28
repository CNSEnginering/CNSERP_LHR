import * as moment from 'moment';
import { IPagedResultDtoOfGetemployeeLoansForViewDto, IGetemployeeLoansForViewDto, IemployeeLoansDto, IGetemployeeLoansForEditOutput, ICreateOrEditemployeeLoansDto } from '../interface/employeeLoans-interface';

export class PagedResultDtoOfGetemployeeLoansForViewDto implements IPagedResultDtoOfGetemployeeLoansForViewDto {
    totalCount!: number | undefined;
    items!: GetemployeeLoansForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetemployeeLoansForViewDto) {
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
                    this.items!.push(GetemployeeLoansForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetemployeeLoansForViewDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetemployeeLoansForViewDto();
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

export class GetemployeeLoansForViewDto implements IGetemployeeLoansForViewDto {
    employeeLoans!: employeeLoansDto | undefined;

    constructor(data?: IGetemployeeLoansForViewDto) {
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
            this.employeeLoans = data["employeeLoans"] ? employeeLoansDto.fromJS(data["employeeLoans"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetemployeeLoansForViewDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new GetemployeeLoansForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["employeeLoans"] = this.employeeLoans ? this.employeeLoans.toJSON() : <any>undefined;
        return data;
    }
}

export class employeeLoansDto implements IemployeeLoansDto {
    loanID!: number | undefined;
    employeeID!: number | undefined;
    loanDate!: string| undefined;
    amount!: string | undefined;
    noi!: number | undefined;
    id!: number | undefined;
    cancelled!:boolean|false;
    cancelledData!: string| undefined;

    constructor(data?: IemployeeLoansDto) {
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
            this.loanID = data["loanID"];
            this.employeeID = data["employeeID"];
            this.loanDate = data["loanDate"];
            this.amount = data["amount"];
            this.noi = data["noi"];
            this.id = data["id"];
            this.cancelled = data["cancelled"];
            this.cancelledData=data["cancelledData"]
        }
    }

    static fromJS(data: any): employeeLoansDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new employeeLoansDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["loanID"] = this.loanID;
        data["employeeID"] = this.employeeID;
        data["loanDate"] = this.loanDate;
        data["amount"] = this.amount;
        data["noi"] = this.noi;
        data["id"] = this.id;
        data["cancelled"] = this.cancelled;
        data["cancelledData"]=this.cancelledData
        return data;
    }
}

export class CreateOrEditemployeeLoansDto implements ICreateOrEditemployeeLoansDto {
    LoanID!: number;
    EmployeeID!: number | undefined;
    LoanDate!: string| undefined;
    Amount!: number | undefined;
    NOI!: number| undefined;
    id!: number | undefined;
    loanTypeID!: number | undefined;
    insAmt!: number | undefined;
    remarks!: string | undefined;
    employeeName!: string | undefined;
    employeeLoanTypeName!: string | undefined;
    posted:boolean|false;
    approved:boolean|false;
    cancelled:boolean|false;
    cancelledData: string| undefined;
    constructor(data?: ICreateOrEditemployeeLoansDto) {
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
            this.LoanID = data["loanID"];
            this.EmployeeID = data["employeeID"];
            this.LoanDate = data["loanDate"];
            this.Amount = data["amount"];
            this.NOI = data["noi"];
            this.id = data["id"];
            this.loanTypeID = data["loanTypeID"];
            this.insAmt = data["insAmt"];
            this.remarks = data["remarks"];
            this.employeeLoanTypeName = data["employeeLoanTypeName"];
            this.employeeName = data["employeeName"];
            this.approved=data["approved"];
            this.posted=data["posted"];
            this.cancelled = data["cancelled"];
            this.cancelledData=data["cancelledData"]
        }
    }

    static fromJS(data: any): CreateOrEditemployeeLoansDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditemployeeLoansDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["loanID"] = this.LoanID;
        data["employeeID"] = this.EmployeeID;
        data["loanDate"] = this.LoanDate;
        data["amount"] = this.Amount;
        data["noi"] = this.NOI;
        data["id"] = this.id;
        data["loanTypeID"] = this.loanTypeID;
        data["insAmt"] = this.insAmt;
        data["remarks"] = this.remarks;
        data["employeeLoanTypeName"] = this.employeeLoanTypeName;
        data["employeeName"] = this.employeeName;
        data["approved"]=this.approved;
        data["posted"]=this.posted;
        data["cancelled"] = this.cancelled;
        data["cancelledData"]=this.cancelledData;
        return data;
    }
}

export class GetemployeeLoansForEditOutput implements IGetemployeeLoansForEditOutput {
    employeeLoans!: CreateOrEditemployeeLoansDto | undefined;

    constructor(data?: IGetemployeeLoansForEditOutput) {
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
            this.employeeLoans = data["employeeLoans"] ? CreateOrEditemployeeLoansDto.fromJS(data["employeeLoans"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetemployeeLoansForEditOutput {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new GetemployeeLoansForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["employeeLoans"] = this.employeeLoans ? this.employeeLoans.toJSON() : <any>undefined;
        return data;
    }
}












