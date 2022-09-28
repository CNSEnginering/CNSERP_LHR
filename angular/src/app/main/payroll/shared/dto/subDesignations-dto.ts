import * as moment from 'moment';
import { IPagedResultDtoOfGetSubDesignationsForViewDto, IGetSubDesignationsForViewDto, ISubDesignationsDto, IGetSubDesignationsForEditOutput, ICreateOrEditSubDesignationsDto } from '../interface/subDesignations-interface';

export class PagedResultDtoOfGetSubDesignationsForViewDto implements IPagedResultDtoOfGetSubDesignationsForViewDto {
    totalCount!: number | undefined;
    items!: GetSubDesignationsForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetSubDesignationsForViewDto) {
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
                    this.items!.push(GetSubDesignationsForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetSubDesignationsForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetSubDesignationsForViewDto();
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

export class GetSubDesignationsForViewDto implements IGetSubDesignationsForViewDto {
    subDesignations!: SubDesignationsDto | undefined;

    constructor(data?: IGetSubDesignationsForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.subDesignations = data["subDesignations"] ? SubDesignationsDto.fromJS(data["subDesignations"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetSubDesignationsForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetSubDesignationsForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["subDesignations"] = this.subDesignations ? this.subDesignations.toJSON() : <any>undefined;
        return data;
    }
}

export class SubDesignationsDto implements ISubDesignationsDto {
    subDesignationID!: number | undefined;
    subDesignation!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ISubDesignationsDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.subDesignationID = data["subDesignationID"];
            this.subDesignation = data["subDesignation"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): SubDesignationsDto {
        data = typeof data === 'object' ? data : {};
        let result = new SubDesignationsDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["subDesignationID"] = this.subDesignationID;
        data["subDesignation"] = this.subDesignation;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}

export class GetSubDesignationsForEditOutput implements IGetSubDesignationsForEditOutput {
    subDesignations!: CreateOrEditSubDesignationsDto | undefined;

    constructor(data?: IGetSubDesignationsForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.subDesignations = data["subDesignations"] ? CreateOrEditSubDesignationsDto.fromJS(data["subDesignations"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetSubDesignationsForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetSubDesignationsForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["subDesignations"] = this.subDesignations ? this.subDesignations.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditSubDesignationsDto implements ICreateOrEditSubDesignationsDto {
    subDesignationID!: number;
    subDesignation!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditSubDesignationsDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.subDesignationID = data["subDesignationID"];
            this.subDesignation = data["subDesignation"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditSubDesignationsDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditSubDesignationsDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["subDesignationID"] = this.subDesignationID;
        data["subDesignation"] = this.subDesignation;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}

