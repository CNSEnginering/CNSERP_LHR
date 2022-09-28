import * as moment from 'moment';
import { IGetEducationForViewDto, IEducationDto, IGetEducationForEditOutput, ICreateOrEditEducationDto, IPagedResultDtoOfGetEducationForViewDto } from '../interface/education-interface';

export class PagedResultDtoOfGetEducationForViewDto implements IPagedResultDtoOfGetEducationForViewDto {
    totalCount!: number | undefined;
    items!: GetEducationForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetEducationForViewDto) {
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
                    this.items!.push(GetEducationForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetEducationForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetEducationForViewDto();
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

export class GetEducationForViewDto implements IGetEducationForViewDto {
    education!: EducationDto | undefined;

    constructor(data?: IGetEducationForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.education = data["education"] ? EducationDto.fromJS(data["education"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEducationForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetEducationForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["education"] = this.education ? this.education.toJSON() : <any>undefined;
        return data; 
    }
}

export class EducationDto implements IEducationDto {
    edID!: number | undefined;
    eduction!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IEducationDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.edID = data["edID"];
            this.eduction = data["eduction"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): EducationDto {
        data = typeof data === 'object' ? data : {};
        let result = new EducationDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["edID"] = this.edID;
        data["eduction"] = this.eduction;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data; 
    }
}

export class GetEducationForEditOutput implements IGetEducationForEditOutput {
    education!: CreateOrEditEducationDto | undefined;

    constructor(data?: IGetEducationForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.education = data["education"] ? CreateOrEditEducationDto.fromJS(data["education"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEducationForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetEducationForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["education"] = this.education ? this.education.toJSON() : <any>undefined;
        return data; 
    }
}

export class CreateOrEditEducationDto implements ICreateOrEditEducationDto {
    edID!: number;
    eduction!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditEducationDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.edID = data["edID"];
            this.eduction = data["eduction"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditEducationDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditEducationDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["edID"] = this.edID;
        data["eduction"] = this.eduction;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data; 
    }
}