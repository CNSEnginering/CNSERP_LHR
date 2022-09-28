import * as moment from 'moment';
import { IGLSecurityHeaderDto, ICreateOrEditGLSecurityHeaderDto, IPagedResultDtoOfGLSecurityHeaderDto, IGetGLSecurityHeaderForEditOutput, IGetGLSecurityHeaderForViewDto, IPagedResultDtoOfGetGLSecurityHeaderForViewDto } from '../interface/gLSecurityHeader-interface';
import { CreateOrEditGLSecurityDetailDto } from './glSecurityDetail-dto';

export class PagedResultDtoOfGLSecurityHeaderDto implements IPagedResultDtoOfGLSecurityHeaderDto {
    totalCount!: number | undefined;
    items!: GLSecurityHeaderDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGLSecurityHeaderDto) {
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
                    this.items!.push(GLSecurityHeaderDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGLSecurityHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGLSecurityHeaderDto();
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

export class GLSecurityHeaderDto implements IGLSecurityHeaderDto {
    userID!: string | undefined;
    userName!: string | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createdDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IGLSecurityHeaderDto) {
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
            this.userID = data["userID"];
            this.userName = data["userName"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createdDate = data["createdDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): GLSecurityHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new GLSecurityHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["userID"] = this.userID;
        data["userName"] = this.userName;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createdDate"] = this.createdDate;
        data["id"] = this.id;
        return data;
    }
}

export class CreateOrEditGLSecurityHeaderDto implements ICreateOrEditGLSecurityHeaderDto {
    flag!: boolean |undefined
    glSecurityDetail!: CreateOrEditGLSecurityDetailDto [] | undefined;
    userID!: string | undefined;
    userName!: string | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createdDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditGLSecurityHeaderDto) {
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
            this.userID = data["userID"];
            this.userName = data["userName"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createdDate = data["createdDate"];
            this.id = data["id"];
            this.glSecurityDetail = [] as any;
                for (let item of data["glSecurityDetail"])
                    this.glSecurityDetail!.push(CreateOrEditGLSecurityDetailDto.fromJS(item));
        }
    }

    static fromJS(data: any): CreateOrEditGLSecurityHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditGLSecurityHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["flag"] = this.flag;
        data["userID"] = this.userID;
        data["userName"] = this.userName;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createdDate"] = this.createdDate;
        data["id"] = this.id;
        if (this.glSecurityDetail && this.glSecurityDetail.constructor === Array) {
            data["glSecurityDetail"] = [];
            for (let item of this.glSecurityDetail)
                data["glSecurityDetail"].push(item);
        }
        return data;
    }
}

export class PagedResultDtoOfGetGLSecurityHeaderForViewDto implements IPagedResultDtoOfGetGLSecurityHeaderForViewDto {
    totalCount!: number | undefined;
    items!: GetGLSecurityHeaderForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetGLSecurityHeaderForViewDto) {
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
                    this.items!.push(GetGLSecurityHeaderForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetGLSecurityHeaderForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetGLSecurityHeaderForViewDto();
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

export class GetGLSecurityHeaderForViewDto implements IGetGLSecurityHeaderForViewDto {
    glSecurityHeader!: GLSecurityHeaderDto | undefined;

    constructor(data?: IGetGLSecurityHeaderForViewDto) {
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
            this.glSecurityHeader = data["glSecurityHeader"] ? GLSecurityHeaderDto.fromJS(data["glSecurityHeader"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetGLSecurityHeaderForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetGLSecurityHeaderForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["glSecurityHeader"] = this.glSecurityHeader ? this.glSecurityHeader.toJSON() : <any>undefined;
        return data;
    }
}

export class GetGLSecurityHeaderForEditOutput implements  IGetGLSecurityHeaderForEditOutput{
  
    glSecurityHeader: CreateOrEditGLSecurityHeaderDto | undefined;


    constructor(data?: IGetGLSecurityHeaderForEditOutput) {
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
            this.glSecurityHeader = data["glSecurityHeader"] ? CreateOrEditGLSecurityHeaderDto.fromJS(data["glSecurityHeader"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetGLSecurityHeaderForEditOutput {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new GetGLSecurityHeaderForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
       
        data["glSecurityHeader"] = this.glSecurityHeader ? this.glSecurityHeader.toJSON() : <any>undefined;
        return data;
    }
}