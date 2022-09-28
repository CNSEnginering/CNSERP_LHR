import * as moment from 'moment';
import { IPagedResultDtoOfGetAllowancesDetailForViewDto, IGetAllowancesDetailForViewDto, IAllowancesDetailDto, IGetAllowancesDetailForEditOutput, ICreateOrEditAllowancesDetailDto, IPagedResultDtoOfGetAllowancesDetail } from '../interface/allowanceDetails-interface';

export class PagedResultDtoOfGetAllowancesDetailForViewDto implements IPagedResultDtoOfGetAllowancesDetailForViewDto {
    totalCount!: number | undefined;
    items!: GetAllowancesDetailForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetAllowancesDetailForViewDto) {
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
                    this.items!.push(GetAllowancesDetailForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetAllowancesDetailForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetAllowancesDetailForViewDto();
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

export class PagedResultDtoOfGetAllowancesDetail implements IPagedResultDtoOfGetAllowancesDetail {
    totalCount!: number | undefined;
    items!: AllowancesDetailDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetAllowancesDetail) {
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
                    this.items!.push(AllowancesDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetAllowancesDetail {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetAllowancesDetail();
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

export class GetAllowancesDetailForViewDto implements IGetAllowancesDetailForViewDto {
    allowancesDetail!: AllowancesDetailDto | undefined;

    constructor(data?: IGetAllowancesDetailForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.allowancesDetail = data["allowancesDetail"] ? AllowancesDetailDto.fromJS(data["allowancesDetail"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetAllowancesDetailForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetAllowancesDetailForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["allowancesDetail"] = this.allowancesDetail ? this.allowancesDetail.toJSON() : <any>undefined;
        return data;
    }
}

export class AllowancesDetailDto implements IAllowancesDetailDto {
    detID!: number | undefined;
    tenantID!: number | undefined;
    employeeID!: number | undefined;
    employeeName!: string | undefined;
    allowanceType!: number | undefined;
    allowanceAmt!: number | undefined;
    allowanceQty!: number | undefined;
    milage!: number | undefined;
    parkingFees!: number | undefined;
    amount!: number | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;
    allowanceTypeName!: string | undefined;
    perlitrMilg!: number | undefined;
    repairRate!: number | undefined;
    workedDays!: number | undefined;
    constructor(data?: IAllowancesDetailDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.detID = data["detID"];
            this.tenantID = data["tenantID"];
            this.employeeID = data["employeeID"];
            this.employeeName = data["employeeName"];
            this.allowanceType = data["allowanceType"];
            this.allowanceAmt = data["allowanceAmt"];
            this.allowanceQty = data["allowanceQty"];
            this.milage = data["milage"];
            this.parkingFees = data["parkingFees"];
            this.amount = data["amount"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
            this.allowanceTypeName = data["allowanceTypeName"];
            this.perlitrMilg=data["perlitrMilg"];
            this.repairRate=data["repairRate"];
            this.workedDays=data["workedDays"];
        }
    }

    static fromJS(data: any): AllowancesDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new AllowancesDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["tenantID"] = this.tenantID;
        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["allowanceType"] = this.allowanceType;
        data["allowanceAmt"] = this.allowanceAmt;
        data["allowanceQty"] = this.allowanceQty;
        data["milage"] = this.milage;
        data["parkingFees"] = this.parkingFees;
        data["amount"] = this.amount;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        data["allowanceTypeName"] = this.allowanceTypeName;
        data["perlitrMilg"]=this.perlitrMilg;
        data["repairRate"]=this.repairRate;
        data["workedDays"]=this.workedDays;
        return data;
    }
}

export class GetAllowancesDetailForEditOutput implements IGetAllowancesDetailForEditOutput {
    allowancesDetail!: CreateOrEditAllowancesDetailDto | undefined;

    constructor(data?: IGetAllowancesDetailForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.allowancesDetail = data["allowancesDetail"] ? CreateOrEditAllowancesDetailDto.fromJS(data["allowancesDetail"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetAllowancesDetailForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetAllowancesDetailForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["allowancesDetail"] = this.allowancesDetail ? this.allowancesDetail.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditAllowancesDetailDto implements ICreateOrEditAllowancesDetailDto {
    detID!: number;
    employeeID!: number | undefined;
    employeeName!: number | undefined;
    allowanceType!: number | undefined;
    allowanceAmt!: number | undefined;
    allowanceQty!: number | undefined;
    milage!: number | undefined;
    parkingFees!: number | undefined;
    amount!: number | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;
    allowanceTypeName!: string | undefined;
    perlitrMilg!: number | undefined;
    repairRate!: number | undefined;
    workedDays!: number | undefined;

    constructor(data?: ICreateOrEditAllowancesDetailDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.detID = data["detID"];
            this.employeeID = data["employeeID"];
            this.employeeName = data["employeeName"];
            this.allowanceType = data["allowanceType"];
            this.allowanceAmt = data["allowanceAmt"];
            this.allowanceQty = data["allowanceQty"];
            this.milage = data["milage"];
            this.parkingFees = data["parkingFees"];
            this.amount = data["amount"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.allowanceTypeName = data["allowanceTypeName"];
            this.perlitrMilg=data["perlitrMilg"];
            this.repairRate=data["repairRate"];
            this.id = data["id"];
            this.workedDays = data["workedDays"];
        }
    }

    static fromJS(data: any): CreateOrEditAllowancesDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditAllowancesDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["allowanceType"] = this.allowanceType;
        data["allowanceAmt"] = this.allowanceAmt;
        data["allowanceQty"] = this.allowanceQty;
        data["milage"] = this.milage;
        data["parkingFees"] = this.parkingFees;
        data["amount"] = this.amount;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["allowanceTypeName"] = this.allowanceTypeName;
        data["perlitrMilg"]=this.perlitrMilg;
        data["repairRate"]=this.repairRate;
        data["id"] = this.id;
        data["workedDays"] = this.workedDays;
        return data;
    }
}
