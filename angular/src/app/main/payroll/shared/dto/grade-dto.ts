import * as moment from 'moment';
import {  IGradeDto, IPagedResultDtoOfGetGradeForViewDto, IGetGradeForViewDto, IGetGradeForEditOutput, ICreateOrEditGradeDto } from '../interface/grade-interface';
export class PagedResultDtoOfGetGradeForViewDto implements IPagedResultDtoOfGetGradeForViewDto {
    totalCount!: number | undefined;
    items!: GetGradeForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetGradeForViewDto) {
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
                    this.items!.push(GetGradeForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetGradeForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetGradeForViewDto();
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

export class GetGradeForViewDto implements IGetGradeForViewDto {
    grade!: GradeDto | undefined;

    constructor(data?: IGetGradeForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
       
        if (data) {
            this.grade = data["grade"] ? GradeDto.fromJS(data["grade"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetGradeForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetGradeForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["grade"] = this.grade ? this.grade.toJSON() : <any>undefined;
        return data;
    }
}

export class GradeDto implements IGradeDto {
    gradeID!: number | undefined;
    gradeName!: string | undefined;
    type!: number | undefined;
    active!:  boolean;
    audtUser! :string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IGradeDto) {
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
            this.gradeName = data["gradeName"];
            this.gradeID = data["gradeID"];
            this.type = data["type"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): GradeDto {
        debugger
        data = typeof data === 'object' ? data : {};
        let result = new GradeDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["gradeName"] = this.gradeName;
        data["gradeID"] = this.gradeID;
        data["type"] = this.type;
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

export class GetGradeForEditOutput implements IGetGradeForEditOutput {
    grade!: CreateOrEditGradeDto | undefined;

    constructor(data?: IGetGradeForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.grade = data["grade"] ? CreateOrEditGradeDto.fromJS(data["grade"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetGradeForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetGradeForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["grade"] = this.grade ? this.grade.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditGradeDto implements ICreateOrEditGradeDto {
    gradeID!: number | undefined;
    gradeName!: string | undefined;
    type!: number | undefined;
    active!:  boolean;
    audtUser! :string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditGradeDto) {
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
            this.gradeName = data["gradeName"];
            this.gradeID = data["gradeID"];
            this.type = data["type"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditGradeDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditGradeDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["gradeName"] = this.gradeName;
        data["gradeID"] = this.gradeID;
        data["type"] = this.type;
        data["active"]=this.active;
        data["id"] = this.id;
        data["audtUser"]=this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"]=this.createdBy;
        data["createDate"] = this.createDate;
        return data;
    }
}