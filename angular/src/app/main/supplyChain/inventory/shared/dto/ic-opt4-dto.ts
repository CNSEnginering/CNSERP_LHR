import * as moment from 'moment';
import {  IICOPT4Dto, IPagedResultDtoOfGetICOPT4ForViewDto, IGetICOPT4ForViewDto, IGetICOPT4ForEditOutput, ICreateOrEditICOPT4Dto } from '../interface/ic-opt4-interface';
import { ICOPT5Component } from '../../setupForms/ic-OPT5/icopT5.component';
export class PagedResultDtoOfGetICOPT4ForViewDto implements IPagedResultDtoOfGetICOPT4ForViewDto {
    totalCount!: number | undefined;
    items!: GetICOPT4ForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetICOPT4ForViewDto) {
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
                    this.items!.push(GetICOPT4ForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetICOPT4ForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetICOPT4ForViewDto();
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

export class GetICOPT4ForViewDto implements IGetICOPT4ForViewDto {
    iCOPT4!: ICOPT4Dto | undefined;

    constructor(data?: IGetICOPT4ForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
       
        if (data) {
            this.iCOPT4 = data["icopT4"] ? ICOPT4Dto.fromJS(data["icopT4"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetICOPT4ForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetICOPT4ForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["icopT4"] = this.iCOPT4 ? this.iCOPT4.toJSON() : <any>undefined;
        return data;
    }
}

export class ICOPT4Dto implements IICOPT4Dto {
    optID: number | undefined;
    descp!: string | undefined;
    active!: boolean;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id: number | undefined;

    constructor(data?: IICOPT4Dto) {
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
            this.descp = data["descp"];
            this.optID = data["optID"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): ICOPT4Dto {
        debugger
        data = typeof data === 'object' ? data : {};
        let result = new ICOPT4Dto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["descp"] = this.descp;
        data["optID"] = this.optID;
        data["active"]=this.active;
        data["id"] = this.id;
        data["audtUser"]=this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"]=this.createdBy;
        data["createDate"] = this.createDate;
        debugger;
        return data;
    }
}

export class GetICOPT4ForEditOutput implements IGetICOPT4ForEditOutput {
    iCOPT4!: CreateOrEditICOPT4Dto | undefined;

    constructor(data?: IGetICOPT4ForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.iCOPT4 = data["icopT4"] ? CreateOrEditICOPT4Dto.fromJS(data["icopT4"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetICOPT4ForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetICOPT4ForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["icopT4"] = this.iCOPT4 ? this.iCOPT4.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditICOPT4Dto implements ICreateOrEditICOPT4Dto {
    flag: boolean | undefined;
    optID: number | undefined;
    descp!: string | undefined;
    active!: boolean;
    audtUser! :string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditICOPT4Dto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.flag = data["flag"];
            this.descp = data["descp"];
            this.optID = data["optID"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditICOPT4Dto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditICOPT4Dto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["flag"] = this.flag 
        data["descp"] = this.descp;
        data["optID"] = this.optID;
        data["active"]=this.active;
        data["audtUser"]=this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"]=this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}