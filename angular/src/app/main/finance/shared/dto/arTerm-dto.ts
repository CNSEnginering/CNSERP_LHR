import * as moment from "moment";

export class PagedResultDtoOfGetARTermForViewDto implements IPagedResultDtoOfGetARTermForViewDto {
    totalCount!: number | undefined;
    items!: GetARTermForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetARTermForViewDto) {
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
                    this.items!.push(GetARTermForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetARTermForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetARTermForViewDto();
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

export interface IPagedResultDtoOfGetARTermForViewDto {
    totalCount: number | undefined;
    items: GetARTermForViewDto[] | undefined;
}

export class GetARTermForViewDto implements IGetARTermForViewDto {
    arTerm!: ARTermDto | undefined;

    constructor(data?: IGetARTermForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.arTerm = data["arTerm"] ? ARTermDto.fromJS(data["arTerm"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetARTermForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetARTermForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["arTerm"] = this.arTerm ? this.arTerm.toJSON() : <any>undefined;
        return data;
    }
}

export interface IGetARTermForViewDto {
    arTerm: ARTermDto | undefined;
}

export class ARTermDto implements IARTermDto {
    // termdesc!: string | undefined;
    // termrate!: number | undefined;
    // audtdate!: moment.Moment | undefined;
    // audtuser!: string | undefined;
    // inactive!: boolean | undefined;
    // termType!: number | undefined;
    // taxStatus!: number | undefined;
    // id!: number | undefined;
    termDesc: string | undefined;
    termRate: number | undefined;
    audtDate: moment.Moment | undefined;
    audtUser: string | undefined;
    active: boolean;
    termAccId: string | undefined;
    termId: number | undefined;
    id: number | undefined;
    accountName: string | undefined;
    constructor(data?: IARTermDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            // this.termdesc = data["termdesc"];
            // this.termrate = data["termrate"];
            // this.audtdate = data["audtdate"] ? moment(data["audtdate"].toString()) : <any>undefined;
            // this.audtuser = data["audtuser"];
            // this.active = data["active"];
            // this.
            // this.id = data["id"];
            this.termDesc = data["termDesc"];
            this.termRate = data["termRate"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.audtUser = data["audtUser"];
            this.active = data["active"];
            this.termAccId = data["termAccId"];
            this.termId = data["termId"];
            this.id = data["id"];
            this.accountName = data["accountName"];
        }
    }

    static fromJS(data: any): ARTermDto {
        data = typeof data === 'object' ? data : {};
        let result = new ARTermDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        // data["termdesc"] = this.termdesc;
        // data["termrate"] = this.termrate;
        // data["audtdate"] = this.audtdate ? this.audtdate.toISOString() : <any>undefined;
        // data["audtuser"] = this.audtuser;
        // data["active"] = this.active;
        // data["id"] = this.id;


        data["termDesc"] = this.termDesc;
        data["termRate"] = this.termRate;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["audtUser"] = this.audtUser;
        data["active"] = this.active;
        data["termId"] = this.termId;
        data["termAccId"] = this.termAccId;
        data["id"] = this.id;
        data["accountName"] = this.accountName;
        return data;
    }
}

export interface IARTermDto {
    // termdesc: string | undefined;
    // termrate: number | undefined;
    // audtdate: moment.Moment | undefined;
    // audtuser: string | undefined;
    // inactive: boolean | undefined;
    // termType: number | undefined;
    // taxStatus: number | undefined;
    // id: number | undefined;
    
    termDesc: string | undefined;
    termRate: number | undefined;
    audtDate: moment.Moment | undefined;
    audtUser: string | undefined;
    active: boolean;
    termAccId: string | undefined;
    termId: number | undefined;
    id: number | undefined;
    accountName: string | undefined;
}

export class GetARTermForEditOutput implements IGetARTermForEditOutput {
    arTerm!: CreateOrEditARTermDto | undefined;

    constructor(data?: IGetARTermForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.arTerm = data["arTerm"] ? CreateOrEditARTermDto.fromJS(data["arTerm"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetARTermForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetARTermForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["arTerm"] = this.arTerm ? this.arTerm.toJSON() : <any>undefined;
        return data;
    }
}

export interface IGetARTermForEditOutput {
    arTerm: CreateOrEditARTermDto | undefined;
}

export class CreateOrEditARTermDto implements ICreateOrEditARTermDto {
    // termdesc!: string | undefined;
    // termrate!: number | undefined;
    // audtdate!: moment.Moment | undefined;
    // audtuser!: string | undefined;
    // inactive!: boolean;
    // termType!: number | undefined;
    // taxStatus!: number | undefined;
    // id!: number | undefined;
    accountName: string | undefined;
    termDesc: string | undefined;
    termRate: number | undefined;
    audtDate: moment.Moment | undefined;
    audtUser: string | undefined;
    active: boolean;
    termAccId: string | undefined;
    termId: number | undefined;
    id: number | undefined;
   
    constructor(data?: ICreateOrEditARTermDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.termDesc = data["termDesc"];
            this.termRate = data["termRate"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.audtUser = data["audtUser"];
            this.active = data["active"];
            this.termAccId = data["termAccId"];
            this.termId = data["termId"];
            this.id = data["id"];
            this.accountName = data["accountName"];
        }
    }

    static fromJS(data: any): CreateOrEditARTermDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditARTermDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger
        data = typeof data === 'object' ? data : {};
        data["termDesc"] = this.termDesc;
        data["termRate"] = this.termRate;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["audtUser"] = this.audtUser;
        data["active"] = this.active;
        data["termId"] = this.termId;
        data["termAccId"] = this.termAccId;
        data["id"] = this.id;
        data["accountName"] = this.accountName;
        return data;
    }
}

export interface ICreateOrEditARTermDto {
    termDesc: string | undefined;
    termRate: number | undefined;
    audtDate: moment.Moment | undefined;
    audtUser: string | undefined;
    active: boolean;
    termAccId: string | undefined;
    termId: number | undefined;
    id: number | undefined;
    accountName: string | undefined;
}