import * as moment from "moment";
import {
    IPagedResultDtoOfICLocationDto,
    IICLocationDto
} from "../interface/ic-locations-interface";

export class PagedResultDtoOfICLocationDto
    implements IPagedResultDtoOfICLocationDto {
    totalCount!: number | undefined;
    items!: ICLocationDto[] | undefined;

    constructor(data?: IPagedResultDtoOfICLocationDto) {
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
                    this.items!.push(ICLocationDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfICLocationDto {
        data = typeof data === "object" ? data : {};
        let result = new PagedResultDtoOfICLocationDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === "object" ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items) data["items"].push(item.toJSON());
        }
        return data;
    }
}

export class ICLocationDto implements IICLocationDto {
    locID!: number | undefined;
    parentID!: number | undefined;
    locName!: string | undefined;
    locShort!: string | undefined;
    address!: string | undefined;
    city!: string | undefined;
    allowRec: boolean=true;
    allowNeg!: boolean;
    active: boolean = true;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    eLoc1!: number | undefined;
    eLoc2!: number | undefined;
    eLoc3!: number | undefined;
    eLoc4!: number | undefined;
    eLoc5!: number | undefined;
    id!: number | undefined;

    constructor(data?: IICLocationDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.locID = data["locID"];
            this.parentID = data["parentID"];
            this.locName = data["locName"];
            this.locShort = data["locShort"];
            this.address = data["address"];
            this.city = data["city"];
            this.allowRec = data["allowRec"];
            this.allowNeg = data["allowNeg"];
            this.active = data["active"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"]
                ? moment(data["createDate"].toString())
                : <any>undefined;
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"]
                ? moment(data["audtDate"].toString())
                : <any>undefined;
            this.id = data["id"];
            this.eLoc1 = data["eLoc1"];
            this.eLoc2 = data["eLoc2"];
            this.eLoc3 = data["eLoc3"];
            this.eLoc4 = data["eLoc4"];
            this.eLoc5 = data["eLoc5"];
        }
    }

    static fromJS(data: any): ICLocationDto {
        data = typeof data === "object" ? data : {};
        let result = new ICLocationDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === "object" ? data : {};
        data["locID"] = this.locID;
        data["parentID"] = this.parentID;
        data["locName"] = this.locName;
        data["locShort"] = this.locShort;
        data["address"] = this.address;
        data["city"] = this.city;
        data["allowRec"] = this.allowRec;
        data["allowNeg"] = this.allowNeg;
        data["active"] = this.active;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate
            ? this.createDate.toISOString()
            : <any>undefined;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate
            ? this.audtDate.toISOString()
            : <any>undefined;
        data["id"] = this.id;
        data["eLoc1"] = this.eLoc1;
        data["eLoc2"] = this.eLoc2;
        data["eLoc3"] = this.eLoc3;
        data["eLoc4"] = this.eLoc4;
        data["eLoc5"] = this.eLoc5;
        return data;
    }
}
