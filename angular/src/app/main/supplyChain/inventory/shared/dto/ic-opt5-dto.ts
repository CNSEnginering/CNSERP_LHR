import * as moment from 'moment';
import { IPagedResultDtoOfGetICOPT5ForViewDto, IGetICOPT5ForViewDto,IICOPT5Dto, ICreateOrEditICOPT5Dto, IGetICOPT5ForEditOutput } from '../interface/ic-opt5-interface';

export class PagedResultDtoOfGetICOPT5ForViewDto implements IPagedResultDtoOfGetICOPT5ForViewDto {
    totalCount!: number | undefined;
    items!: GetICOPT5ForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetICOPT5ForViewDto) {
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
                    this.items!.push(GetICOPT5ForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetICOPT5ForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetICOPT5ForViewDto();
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

export class GetICOPT5ForViewDto implements IGetICOPT5ForViewDto {
    iCOPT5!: ICOPT5Dto | undefined;

    constructor(data?: IGetICOPT5ForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.iCOPT5 = data["icopT5"] ? ICOPT5Dto.fromJS(data["icopT5"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetICOPT5ForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetICOPT5ForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["icopT5"] = this.iCOPT5 ? this.iCOPT5.toJSON() : <any>undefined;
        return data;
    }
}

export class ICOPT5Dto implements IICOPT5Dto {
    optID: number | undefined;
    descp: string | undefined;
    id!: number | undefined;
    active!: boolean;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;


    constructor(data?: IICOPT5Dto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            
            this.optID = data["optID"];
            this.descp = data["descp"];
            this.id = data["id"];
            this.active = data["active"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
        }
    }

    static fromJS(data: any): ICOPT5Dto {
        data = typeof data === 'object' ? data : {};
        let result = new ICOPT5Dto();
        result.init(data);
        
        return result;
    }

    toJSON(data?: any) {
        
        data = typeof data === 'object' ? data : {};
        data["optID"] = this.optID;
        data["descp"] = this.descp;
        data["id"] = this.id;
        data["active"] = this.active;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        return data;
    }
}

export class GetICOPT5ForEditOutput implements IGetICOPT5ForEditOutput {
    ICOPT5!: CreateOrEditICOPT5Dto | undefined;

    constructor(data?: IGetICOPT5ForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.ICOPT5 = data["icopT5"] ? CreateOrEditICOPT5Dto.fromJS(data["icopT5"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetICOPT5ForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetICOPT5ForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["icopT5"] = this.ICOPT5 ? this.ICOPT5.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditICOPT5Dto implements ICreateOrEditICOPT5Dto {
    flag: boolean | undefined;
    optID: number | undefined;
    descp: string | undefined;
    id!: number | undefined;
    active!: boolean;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;

    constructor(data?: ICreateOrEditICOPT5Dto) {
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
            this.flag = data["flag"];
            this.descp = data["descp"];
            this.optID = data["optID"];
            this.id = data["id"];
            this.active = data["active"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
        }
    }

    static fromJS(data: any): CreateOrEditICOPT5Dto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditICOPT5Dto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        debugger;
        data["flag"] = this.flag
        data["descp"] = this.descp;
        data["optID"] = this.optID;
        data["id"] = this.id;
        data["active"] = this.active;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        return data;
    }
}
