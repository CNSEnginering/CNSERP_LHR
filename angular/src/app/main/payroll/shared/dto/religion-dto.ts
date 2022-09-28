import * as moment from 'moment';
import {  IReligionDto, IPagedResultDtoOfGetReligionForViewDto, IGetReligionForViewDto, IGetReligionForEditOutput, ICreateOrEditReligionDto } from '../interface/religion-interface';
export class PagedResultDtoOfGetReligionForViewDto implements IPagedResultDtoOfGetReligionForViewDto {
    totalCount!: number | undefined;
    items!: GetReligionForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetReligionForViewDto) {
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
                    this.items!.push(GetReligionForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetReligionForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetReligionForViewDto();
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

export class GetReligionForViewDto implements IGetReligionForViewDto {
    religion!: ReligionDto | undefined;

    constructor(data?: IGetReligionForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
       
        if (data) {
            this.religion = data["religion"] ? ReligionDto.fromJS(data["religion"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetReligionForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetReligionForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["religion"] = this.religion ? this.religion.toJSON() : <any>undefined;
        return data;
    }
}

export class ReligionDto implements IReligionDto {
    religionID!: number | undefined;
    religion!: string | undefined;
    active!: boolean;;
    audtUser! :string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IReligionDto) {
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
            this.religion = data["religion"];
            this.religionID = data["religionID"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): ReligionDto {
        debugger
        data = typeof data === 'object' ? data : {};
        let result = new ReligionDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["religion"] = this.religion;
        data["religionID"] = this.religionID;
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

export class GetReligionForEditOutput implements IGetReligionForEditOutput {
    religion!: CreateOrEditReligionDto | undefined;

    constructor(data?: IGetReligionForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.religion = data["religion"] ? CreateOrEditReligionDto.fromJS(data["religion"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetReligionForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetReligionForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["religion"] = this.religion ? this.religion.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditReligionDto implements ICreateOrEditReligionDto {
    religionID!: number | undefined;
    religion!: string | undefined;
    active!: boolean;;
    audtUser! :string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditReligionDto) {
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
            this.religion = data["religion"];
            this.religionID = data["religionID"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditReligionDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditReligionDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["religion"] = this.religion;
        data["religionID"] = this.religionID;
        data["active"]=this.active;
        data["audtUser"]=this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"]=this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}