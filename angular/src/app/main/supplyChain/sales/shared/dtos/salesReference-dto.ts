import * as moment from 'moment';
import { IPagedResultDtoOfGetSalesReferenceForViewDto, IGetSalesReferenceForViewDto, ISalesReferenceDto, IGetSalesReferenceForEditOutput, ICreateOrEditSalesReferenceDto } from '../interfaces/salesReference-interface';


export class PagedResultDtoOfGetSalesReferenceForViewDto implements IPagedResultDtoOfGetSalesReferenceForViewDto {
    totalCount!: number | undefined;
    items!: GetSalesReferenceForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetSalesReferenceForViewDto) {
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
                    this.items!.push(GetSalesReferenceForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetSalesReferenceForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetSalesReferenceForViewDto();
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

export class GetSalesReferenceForViewDto implements IGetSalesReferenceForViewDto {
    salesReference!: SalesReferenceDto | undefined;

    constructor(data?: IGetSalesReferenceForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.salesReference = data["salesReference"] ? SalesReferenceDto.fromJS(data["salesReference"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetSalesReferenceForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetSalesReferenceForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["salesReference"] = this.salesReference ? this.salesReference.toJSON() : <any>undefined;
        return data;
    }
}

export class SalesReferenceDto implements ISalesReferenceDto {
    refID!: number | undefined;
    refName!: string | undefined;
    active!: boolean | undefined;
    audtdate!: moment.Moment | undefined;
    audtuser!: string | undefined;
    createdDATE!: moment.Moment | undefined;
    createdUSER!: string | undefined;
    id!: number | undefined;
    refType!:string | undefined;

    constructor(data?: ISalesReferenceDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.refID = data["refID"];
            this.refName = data["refName"];
            this.active = data["active"];
            this.audtdate = data["audtdate"];
            this.audtuser = data["audtuser"];
            this.createdDATE = data["createdDATE"];
            this.createdUSER = data["createdUSER"];
            this.id = data["id"];
            this.refType = data["refType"];
        }
    }

    static fromJS(data: any): SalesReferenceDto {
        data = typeof data === 'object' ? data : {};
        let result = new SalesReferenceDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["refID"] = this.refID;
        data["refName"] = this.refName;
        data["active"] = this.active;
        data["audtdate"] = this.audtdate;
        data["audtuser"] = this.audtuser;
        data["createdDATE"] = this.createdDATE;
        data["createdUSER"] = this.createdUSER;
        data["id"] = this.id;
        data["refType"]=this.refType ;
        return data;
    }
}

export class GetSalesReferenceForEditOutput implements IGetSalesReferenceForEditOutput {
    salesReference!: CreateOrEditSalesReferenceDto | undefined;

    constructor(data?: IGetSalesReferenceForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.salesReference = data["salesReference"] ? CreateOrEditSalesReferenceDto.fromJS(data["salesReference"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetSalesReferenceForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetSalesReferenceForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["salesReference"] = this.salesReference ? this.salesReference.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditSalesReferenceDto implements ICreateOrEditSalesReferenceDto {
    refID!: number;
    refName!: string | undefined;
    active!: boolean | undefined;
    audtdate!: moment.Moment | undefined;
    audtuser!: string | undefined;
    createdDATE!: moment.Moment | undefined;
    createdUSER!: string | undefined;
    id!: number | undefined;
    refType!:string | undefined;

    constructor(data?: ICreateOrEditSalesReferenceDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.refID = data["refID"];
            this.refName = data["refName"];
            this.active = data["active"];
            this.audtdate = data["audtdate"];
            this.audtuser = data["audtuser"];
            this.createdDATE = data["createdDATE"];
            this.createdUSER = data["createdUSER"];
            this.id = data["id"];
            this.refType = data["refType"];
        }
    }

    static fromJS(data: any): CreateOrEditSalesReferenceDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditSalesReferenceDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["refID"] = this.refID;
        data["refName"] = this.refName;
        data["active"] = this.active;
        data["audtdate"] = this.audtdate;
        data["audtuser"] = this.audtuser;
        data["createdDATE"] = this.createdDATE;
        data["createdUSER"] = this.createdUSER;
        data["id"] = this.id;
        data["refType"]=this.refType ;
        return data;
    }
}