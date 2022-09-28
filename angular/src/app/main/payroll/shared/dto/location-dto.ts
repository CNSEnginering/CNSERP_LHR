import * as moment from 'moment';
import {  ILocationDto, IPagedResultDtoOfGetLocationForViewDto, IGetLocationForViewDto, IGetLocationForEditOutput, ICreateOrEditLocationDto } from '../interface/location-interface';
export class PagedResultDtoOfGetLocationForViewDto implements IPagedResultDtoOfGetLocationForViewDto {
    totalCount!: number | undefined;
    items!: GetLocationForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetLocationForViewDto) {
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
                    this.items!.push(GetLocationForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetLocationForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetLocationForViewDto();
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

export class GetLocationForViewDto implements IGetLocationForViewDto {
    location!: LocationDto | undefined;

    constructor(data?: IGetLocationForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
       
        if (data) {
            this.location = data["location"] ? LocationDto.fromJS(data["location"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetLocationForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetLocationForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["location"] = this.location ? this.location.toJSON() : <any>undefined;
        return data;
    }
}

export class LocationDto implements ILocationDto {
    locID!: number | undefined;
    location!: string | undefined;
    active!: boolean;;
    audtUser! :string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ILocationDto) {
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
            this.location = data["location"];
            this.locID = data["locID"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): LocationDto {
        debugger
        data = typeof data === 'object' ? data : {};
        let result = new LocationDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["location"] = this.location;
        data["locID"] = this.locID;
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

export class GetLocationForEditOutput implements IGetLocationForEditOutput {
    location!: CreateOrEditLocationDto | undefined;

    constructor(data?: IGetLocationForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.location = data["location"] ? CreateOrEditLocationDto.fromJS(data["location"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetLocationForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetLocationForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["location"] = this.location ? this.location.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditLocationDto implements ICreateOrEditLocationDto {
    locID!: number | undefined;
    location!: string | undefined;
    active!: boolean;;
    audtUser! :string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditLocationDto) {
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
            this.location = data["location"];
            this.locID = data["locID"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditLocationDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditLocationDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["location"] = this.location;
        data["locID"] = this.locID;
        data["active"]=this.active;
        data["audtUser"]=this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"]=this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        return data;
    }
}