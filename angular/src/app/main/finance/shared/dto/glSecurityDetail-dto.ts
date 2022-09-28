import * as moment from 'moment';
import { IPagedResultDtoOfGetGLSecurityDetailForViewDto, IGetGLSecurityDetailForViewDto, IGLSecurityDetailDto, IGetGLSecurityDetailForEditOutput, ICreateOrEditGLSecurityDetailDto, IPagedResultDtoOfGLSecurityDetailsDto } from '../interface/glSecurityDetail-interface';


export class PagedResultDtoOfGetGLSecurityDetailForViewDto implements IPagedResultDtoOfGetGLSecurityDetailForViewDto {
    totalCount!: number | undefined;
    items!: GetGLSecurityDetailForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetGLSecurityDetailForViewDto) {
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
                    this.items!.push(GetGLSecurityDetailForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetGLSecurityDetailForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetGLSecurityDetailForViewDto();
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

export class PagedResultDtoOfGLSecurityDetailDto implements IPagedResultDtoOfGLSecurityDetailsDto {
    totalCount!: number | undefined;
    items!: GLSecurityDetailDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGLSecurityDetailsDto) {
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
                    this.items!.push(GLSecurityDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGLSecurityDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGLSecurityDetailDto();
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

export class GetGLSecurityDetailForViewDto implements IGetGLSecurityDetailForViewDto {
    glSecurityDetail!: GLSecurityDetailDto | undefined;

    constructor(data?: IGetGLSecurityDetailForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.glSecurityDetail = data["glSecurityDetail"] ? GLSecurityDetailDto.fromJS(data["glSecurityDetail"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetGLSecurityDetailForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetGLSecurityDetailForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["glSecurityDetail"] = this.glSecurityDetail ? this.glSecurityDetail.toJSON() : <any>undefined;
        return data;
    }
}

export class GLSecurityDetailDto implements IGLSecurityDetailDto {
    detID!: number | undefined;
    userID!: string | undefined;
    canSee!: boolean | undefined;
    beginAcc!: string | undefined;
    endAcc!: string | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IGLSecurityDetailDto) {
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
            this.userID = data["userID"];
            this.canSee = data["canSee"];
            this.beginAcc = data["beginAcc"];
            this.endAcc = data["endAcc"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): GLSecurityDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new GLSecurityDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["userID"] = this.userID;
        data["canSee"] = this.canSee;
        data["beginAcc"] = this.beginAcc;
        data["endAcc"] = this.endAcc;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["id"] = this.id;
        return data;
    }
}

export class GetGLSecurityDetailForEditOutput implements IGetGLSecurityDetailForEditOutput {
    glSecurityDetail!: CreateOrEditGLSecurityDetailDto | undefined;

    constructor(data?: IGetGLSecurityDetailForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.glSecurityDetail = data["glSecurityDetail"] ? CreateOrEditGLSecurityDetailDto.fromJS(data["glSecurityDetail"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetGLSecurityDetailForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetGLSecurityDetailForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["glSecurityDetail"] = this.glSecurityDetail ? this.glSecurityDetail.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditGLSecurityDetailDto implements ICreateOrEditGLSecurityDetailDto {
    detID!: number | undefined;
    userID!: string | undefined;
    canSee!: boolean | undefined;
    beginAcc!: string | undefined;
    endAcc!: string | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditGLSecurityDetailDto) {
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
            this.detID = data["detID"];
            this.userID = data["userID"];
            this.canSee = data["canSee"];
            this.beginAcc = data["beginAcc"];
            this.endAcc = data["endAcc"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditGLSecurityDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditGLSecurityDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["userID"] = this.userID;
        data["canSee"] = this.canSee;
        data["beginAcc"] = this.beginAcc;
        data["endAcc"] = this.endAcc;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["id"] = this.id;
        return data;
    }
}

