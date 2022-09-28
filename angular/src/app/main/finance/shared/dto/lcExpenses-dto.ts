import * as moment from 'moment';
import {  ILCExpensesDto, IPagedResultDtoOfGetLCExpensesForViewDto, IGetLCExpensesForViewDto, IGetLCExpensesForEditOutput, ICreateOrEditLCExpensesDto } from '../interface/lcExpenses-interface';
export class PagedResultDtoOfGetLCExpensesForViewDto implements IPagedResultDtoOfGetLCExpensesForViewDto {
    totalCount!: number | undefined;
    items!: GetLCExpensesForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetLCExpensesForViewDto) {
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
                    this.items!.push(GetLCExpensesForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetLCExpensesForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetLCExpensesForViewDto();
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

export class GetLCExpensesForViewDto implements IGetLCExpensesForViewDto {
    lcExpenses!: LCExpensesDto | undefined;

    constructor(data?: IGetLCExpensesForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
       
        if (data) {
            this.lcExpenses = data["lcExpenses"] ? LCExpensesDto.fromJS(data["lcExpenses"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetLCExpensesForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetLCExpensesForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["lcExpenses"] = this.lcExpenses ? this.lcExpenses.toJSON() : <any>undefined;
        return data;
    }
}

export class LCExpensesDto implements ILCExpensesDto {
    expID!: number | undefined;
    expDesc!: string | undefined;
    active!: boolean;
    auditUser!: string | undefined;
    auditDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ILCExpensesDto) {
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
            this.expID = data["expID"];
            this.expDesc = data["expDesc"];
            this.active = data["active"];
            this.auditUser = data["auditUser"];
            this.auditDate = data["auditDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): LCExpensesDto {
        debugger
        data = typeof data === 'object' ? data : {};
        let result = new LCExpensesDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["expID"] = this.expID;
        data["expDesc"] = this.expDesc;
        data["active"]=this.active;
        data["auditUser"]=this.auditUser;
        data["auditDate"] = this.auditDate;
        data["createdBy"]=this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        debugger;
        return data;
    }
}

export class GetLCExpensesForEditOutput implements IGetLCExpensesForEditOutput {
    lcExpenses!: CreateOrEditLCExpensesDto | undefined;

    constructor(data?: IGetLCExpensesForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.lcExpenses = data["lcExpenses"] ? CreateOrEditLCExpensesDto.fromJS(data["lcExpenses"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetLCExpensesForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetLCExpensesForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["lcExpenses"] = this.lcExpenses ? this.lcExpenses.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditLCExpensesDto implements ICreateOrEditLCExpensesDto {
    expID!: number | undefined;
    expDesc!: string | undefined;
    active!: boolean;
    auditUser!: string | undefined;
    auditDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditLCExpensesDto) {
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
            this.expID = data["expID"];
            this.expDesc = data["expDesc"];
            this.active = data["active"];
            this.auditUser = data["auditUser"];
            this.auditDate = data["auditDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditLCExpensesDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditLCExpensesDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["expID"] = this.expID;
        data["expDesc"] = this.expDesc;
        data["active"]=this.active;
        data["auditUser"]=this.auditUser;
        data["auditDate"] = this.auditDate;
        data["createdBy"]=this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}