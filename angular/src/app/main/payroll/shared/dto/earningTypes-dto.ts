import * as moment from 'moment';
import { IPagedResultDtoOfGetEarningTypesForViewDto, IGetEarningTypesForViewDto, IEarningTypesDto, IGetEarningTypesForEditOutput, ICreateOrEditEarningTypesDto } from '../interface/earningTypes-interface';


export class PagedResultDtoOfGetEarningTypesForViewDto implements IPagedResultDtoOfGetEarningTypesForViewDto {
    totalCount!: number | undefined;
    items!: GetEarningTypesForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetEarningTypesForViewDto) {
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
                    this.items!.push(GetEarningTypesForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetEarningTypesForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetEarningTypesForViewDto();
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

export class GetEarningTypesForViewDto implements IGetEarningTypesForViewDto {
    earningTypes!: EarningTypesDto | undefined;

    constructor(data?: IGetEarningTypesForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.earningTypes = data["earningTypes"] ? EarningTypesDto.fromJS(data["earningTypes"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEarningTypesForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetEarningTypesForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["earningTypes"] = this.earningTypes ? this.earningTypes.toJSON() : <any>undefined;
        return data;
    }
}

export class EarningTypesDto implements IEarningTypesDto {
    typeID!: number | undefined;
    typeDesc!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IEarningTypesDto) {
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

    static fromJS(data: any): EarningTypesDto {
        data = typeof data === 'object' ? data : {};
        let result = new EarningTypesDto();
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

export class GetEarningTypesForEditOutput implements IGetEarningTypesForEditOutput {
    earningTypes!: CreateOrEditEarningTypesDto | undefined;

    constructor(data?: IGetEarningTypesForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.earningTypes = data["earningTypes"] ? CreateOrEditEarningTypesDto.fromJS(data["earningTypes"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEarningTypesForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetEarningTypesForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["earningTypes"] = this.earningTypes ? this.earningTypes.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditEarningTypesDto implements ICreateOrEditEarningTypesDto {
    typeID!: number;
    typeDesc!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditEarningTypesDto) {
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

    static fromJS(data: any): CreateOrEditEarningTypesDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditEarningTypesDto();
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
