import { CreateOrEditEmployeeDeductionsDto } from "./employeeDeductions-dto";
import { CreateOrEditEmployeeEarningsDto } from "./employeeEarnings-dto";

export class AdjDetail{
    deductionDetail:CreateOrEditEmployeeDeductionsDto[];
    earningDetail:CreateOrEditEmployeeEarningsDto[];
}

export class AdjHDto {
    id!: number | undefined;
    docType!: number | undefined;
    typeID!: number | undefined;
    docID!: number | undefined;
    docdate!: Date | undefined;
    salaryYear!: number | undefined;
    salaryMonth!: number | undefined;
    adjDetails!: AdjDetail;
    createdBy!: string;
    createDate!: Date;
    audtUser!: string;
    audtDate!: Date;

    constructor(data?: AdjHDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.adjDetails = data["adjDetails"];
            this.docType = data["docType"];
            this.typeID = data["typeID"];
            this.docID = data["docID"];
            this.docdate = data["docdate"];
            this.salaryYear = data["salaryYear"];
            this.salaryMonth = data["salaryMonth"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): AdjHDto {
        data = typeof data === "object" ? data : {};
        let result = new AdjHDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === "object" ? data : {};
        data["adjDetails"] = this.adjDetails;
        data["docType"] = this.docType;
        data["typeID"] = this.typeID;
        data["docID"] = this.docID;
        data["docdate"] = this.docdate;
        data["salaryYear"] = this.salaryYear;
        data["salaryMonth"] = this.salaryMonth;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}

export enum AdjustmentDocType {
    Earning = 1,
    Deduction = 2,
}

export interface IPagedResultDtoOfGetAdjHForViewDto {
    totalCount: number | undefined;
    items: GetAdjHForViewDto[] | undefined;
}

export interface IGetAdjHForViewDto {
    AdjHDto: AdjHDto | undefined;
}

export class PagedResultDtoOfGetAdjHForViewDto
    implements IPagedResultDtoOfGetAdjHForViewDto {
    totalCount!: number | undefined;
    items!: GetAdjHForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetAdjHForViewDto) {
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
                    this.items!.push(GetAdjHForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetAdjHForViewDto {
        data = typeof data === "object" ? data : {};
        let result = new PagedResultDtoOfGetAdjHForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === "object" ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items) data["items"].push(item.toJSON());
        }
        return data;
    }
}

export class GetAdjHForViewDto implements IGetAdjHForViewDto {
    AdjHDto!: AdjHDto | undefined;

    constructor(data?: IGetAdjHForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.AdjHDto = data["adjH"]
                ? AdjHDto.fromJS(data["adjH"])
                : <any>undefined;
        }
    }

    static fromJS(data: any): GetAdjHForViewDto {
        data = typeof data === "object" ? data : {};
        let result = new GetAdjHForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === "object" ? data : {};
        data["adjH"] = this.AdjHDto ? this.AdjHDto.toJSON() : <any>undefined;
        return data;
    }


    
}

export class GetAdjHForEditOutput implements IGetAdjHForEditOutput {
    AdjHDto!: AdjHDto | undefined;

    constructor(data?: IGetAdjHForEditOutput) {
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
            this.AdjHDto = data["adjH"] ? AdjHDto.fromJS(data["adjH"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetAdjHForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetAdjHForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["adjH"] = this.AdjHDto ? this.AdjHDto.toJSON() : <any>undefined;
        return data;
    }
}

export interface IGetAdjHForEditOutput {
    AdjHDto: AdjHDto | undefined;
}