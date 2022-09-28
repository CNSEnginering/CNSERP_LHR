import * as moment from 'moment';
import { IPagedResultDtoOfGetDeductionTypesForViewDto, IGetDeductionTypesForViewDto, IDeductionTypesDto, IGetDeductionTypesForEditOutput, ICreateOrEditDeductionTypesDto } from '../interface/deductionTypes-interface';


export class PagedResultDtoOfGetDeductionTypesForViewDto implements IPagedResultDtoOfGetDeductionTypesForViewDto {
    totalCount!: number | undefined;
    items!: GetDeductionTypesForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetDeductionTypesForViewDto) {
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
                    this.items!.push(GetDeductionTypesForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetDeductionTypesForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetDeductionTypesForViewDto();
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

export class GetDeductionTypesForViewDto implements IGetDeductionTypesForViewDto {
    deductionTypes!: DeductionTypesDto | undefined;

    constructor(data?: IGetDeductionTypesForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.deductionTypes = data["deductionTypes"] ? DeductionTypesDto.fromJS(data["deductionTypes"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetDeductionTypesForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetDeductionTypesForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["deductionTypes"] = this.deductionTypes ? this.deductionTypes.toJSON() : <any>undefined;
        return data;
    }
}

export class DeductionTypesDto implements IDeductionTypesDto {
    typeID!: number | undefined;
    typeDesc!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IDeductionTypesDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.typeID = data["typeID"];
            this.typeDesc = data["typeDesc"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): DeductionTypesDto {
        data = typeof data === 'object' ? data : {};
        let result = new DeductionTypesDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["typeID"] = this.typeID;
        data["typeDesc"] = this.typeDesc;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}

export class GetDeductionTypesForEditOutput implements IGetDeductionTypesForEditOutput {
    deductionTypes!: CreateOrEditDeductionTypesDto | undefined;

    constructor(data?: IGetDeductionTypesForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.deductionTypes = data["deductionTypes"] ? CreateOrEditDeductionTypesDto.fromJS(data["deductionTypes"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetDeductionTypesForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetDeductionTypesForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["deductionTypes"] = this.deductionTypes ? this.deductionTypes.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditDeductionTypesDto implements ICreateOrEditDeductionTypesDto {
    typeID!: number;
    typeDesc!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditDeductionTypesDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.typeID = data["typeID"];
            this.typeDesc = data["typeDesc"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditDeductionTypesDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditDeductionTypesDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["typeID"] = this.typeID;
        data["typeDesc"] = this.typeDesc;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}
