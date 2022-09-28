import { ISectionDto, ICreateOrEditSectionDto, IGetSectionForViewDto, IGetSectionForEditOutput, IPagedResultDtoOfGetSectionForViewDto } from "../interface/section-interface";
import * as moment from 'moment';

export class SectionDto implements ISectionDto {
    secID!: number | undefined;
    secName!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ISectionDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.secID = data["secID"];
            this.secName = data["secName"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): SectionDto {
        data = typeof data === 'object' ? data : {};
        let result = new SectionDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["secID"] = this.secID;
        data["secName"] = this.secName;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}

export class CreateOrEditSectionDto implements ICreateOrEditSectionDto {
    secID!: number;

    secName!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;
    deptID!:number | undefined;
    deptName!: string | undefined;
    sortId!:number|undefined;
    constructor(data?: ICreateOrEditSectionDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.secID = data["secID"];
            this.secName = data["secName"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
            this.deptID=data["deptID"];
            this.deptName=data["deptName"];
            this.sortId=data["sortId"];
        }
    }

    static fromJS(data: any): CreateOrEditSectionDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditSectionDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["secID"] = this.secID;
        data["secName"] = this.secName;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        data["deptID"]=this.deptID;
        data["sortId"]=this.sortId;
        return data;
    }
}

export class GetSectionForViewDto implements IGetSectionForViewDto {
    section!: SectionDto | undefined;

    constructor(data?: IGetSectionForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.section = data["section"] ? SectionDto.fromJS(data["section"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetSectionForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetSectionForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["section"] = this.section ? this.section.toJSON() : <any>undefined;
        return data;
    }
}

export class GetSectionForEditOutput implements IGetSectionForEditOutput {
    section!: CreateOrEditSectionDto | undefined;

    constructor(data?: IGetSectionForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        debugger 
        if (data) {
            this.section = data["section"] ? CreateOrEditSectionDto.fromJS(data["section"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetSectionForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetSectionForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["section"] = this.section ? this.section.toJSON() : <any>undefined;
        return data;
    }
}

export class PagedResultDtoOfGetSectionForViewDto implements IPagedResultDtoOfGetSectionForViewDto {
    totalCount!: number | undefined;
    items!: GetSectionForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetSectionForViewDto) {
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
                    this.items!.push(GetSectionForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetSectionForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetSectionForViewDto();
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