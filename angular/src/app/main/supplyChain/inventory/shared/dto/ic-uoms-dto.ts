import * as moment from 'moment';
import { IICUOMDto, IPagedResultDtoOfICUOMDto } from '../interface/ic-uoms-interface';

export class PagedResultDtoOfICUOMDto implements IPagedResultDtoOfICUOMDto {
    totalCount!: number | undefined;
    items!: ICUOMDto[] | undefined;

    constructor(data?: IPagedResultDtoOfICUOMDto) {
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
                    this.items!.push(ICUOMDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfICUOMDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfICUOMDto();
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

export class ICUOMDto implements IICUOMDto {
    unit!: string | undefined;
    unitdesc!: string | undefined;
    conver: number | undefined;
    active!: boolean;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IICUOMDto) {
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
            this.unit = data["unit"];
            this.unitdesc = data["unitdesc"];
            this.conver = data["conver"];
            this.active = data["active"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"]? moment(data["createDate"].toString()) : <any>undefined;
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"]? moment(data["audtDate"].toString()) : <any>undefined;
            this.id = data["id"];
        }
    }

    static fromJS(data: any): ICUOMDto {
        data = typeof data === 'object' ? data : {};
        let result = new ICUOMDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["unit"]=this.unit;
        data["unitdesc"]=this.unitdesc;
        data["conver"]=this.conver;
        data["active"]=this.active;
        data["createdBy"]=this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["audtUser"]=this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["id"]=this.id;
        return data; 
    }
}
