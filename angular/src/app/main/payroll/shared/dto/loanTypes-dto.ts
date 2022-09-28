import * as moment from "moment";
import {
    IPagedResultDtoOfGetLoansTypeForViewDto,
    IGetLoansTypeForViewDto,
    ILoansTypeDto,
    ICreateOrEditLoansTypeDto,
    IGetLoansTypeForEditOutput,
} from "../interface/loansType-interface";

export class PagedResultDtoOfGetLoansTypeForViewDto
    implements IPagedResultDtoOfGetLoansTypeForViewDto {
    totalCount!: number | undefined;
    items!: GetLoansTypeForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetLoansTypeForViewDto) {
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
                    this.items!.push(GetLoansTypeForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetLoansTypeForViewDto {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new PagedResultDtoOfGetLoansTypeForViewDto();
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

export class GetLoansTypeForViewDto implements IGetLoansTypeForViewDto {
    loansType!: loansTypeDto | undefined;

    constructor(data?: IGetLoansTypeForViewDto) {
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
            this.loansType = data["employeeLoansType"]
                ? loansTypeDto.fromJS(data["employeeLoansType"])
                : <any>undefined;
        }
    }

    static fromJS(data: any): GetLoansTypeForViewDto {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new GetLoansTypeForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === "object" ? data : {};
        data["employeeLoansType"] = this.loansType
            ? this.loansType.toJSON()
            : <any>undefined;
        return data;
    }
}

export class loansTypeDto implements ILoansTypeDto {
    loanTypeID!: number | undefined;
    loanTypeName!: string | undefined;
    id!: number | undefined;

    constructor(data?: ILoansTypeDto) {
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
            this.loanTypeID = data["loanTypeID"];
            this.loanTypeName = data["loanTypeName"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): loansTypeDto {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new loansTypeDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === "object" ? data : {};
        data["loanTypeID"] = this.loanTypeID;
        data["loanTypeName"] = this.loanTypeName;
        data["id"] = this.id;
        return data;
    }
}

export class CreateOrEditLoansTypeDto implements ICreateOrEditLoansTypeDto {
    LoanTypeID: number | undefined;
    LoanTypeName: string | undefined;
    id: number | undefined;

    constructor(data?: ICreateOrEditLoansTypeDto) {
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
            this.LoanTypeID = data["loanTypeID"];
            this.LoanTypeName = data["loanTypeName"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditLoansTypeDto {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new CreateOrEditLoansTypeDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === "object" ? data : {};
        data["loanTypeID"] = this.LoanTypeID;
        data["loanTypeName"] = this.LoanTypeName;
        data["id"] = this.id;
        return data;
    }
}

export class GetLoansTypeForEditOutput implements IGetLoansTypeForEditOutput {
    loansType!: CreateOrEditLoansTypeDto | undefined;

    constructor(data?: IGetLoansTypeForEditOutput) {
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
            this.loansType = data["employeeLoansType"]
                ? CreateOrEditLoansTypeDto.fromJS(data["employeeLoansType"])
                : <any>undefined;
        }
    }

    static fromJS(data: any): GetLoansTypeForEditOutput {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new GetLoansTypeForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === "object" ? data : {};
        data["employeeLoansType"] = this.loansType
            ? this.loansType.toJSON()
            : <any>undefined;
        return data;
    }
}
