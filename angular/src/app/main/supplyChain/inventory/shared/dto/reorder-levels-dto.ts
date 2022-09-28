import * as moment from 'moment';
import { IPagedResultDtoOfReorderLevelDto, IReorderLevelDto } from '../interface/reorder-levels-interface';

export class PagedResultDtoOfReorderLevelDto implements IPagedResultDtoOfReorderLevelDto {
    totalCount!: number | undefined;
    items!: ReorderLevelDto[] | undefined;

    constructor(data?: IPagedResultDtoOfReorderLevelDto) {
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
                    this.items!.push(ReorderLevelDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfReorderLevelDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfReorderLevelDto();
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

export class ReorderLevelDto implements IReorderLevelDto {
    locId!: number | undefined;
    itemId!: string | undefined;
    minLevel!: number | undefined;
    maxLevel!: number | undefined;
    ordLevel!: number | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    id!: number | undefined;
    itemName!:string |undefined;

    constructor(data?: IReorderLevelDto) {
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
            this.locId = data["locId"];
            this.itemId = data["itemId"];
            this.minLevel = data["minLevel"];
            this.maxLevel = data["maxLevel"];
            this.ordLevel = data["ordLevel"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"]? moment(data["createDate"].toString()) : <any>undefined;
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"]? moment(data["audtDate"].toString()) : <any>undefined;
            this.id = data["id"];
            this.itemName=data["itemName"];
        }
    }

    static fromJS(data: any): ReorderLevelDto {
        data = typeof data === 'object' ? data : {};
        let result = new ReorderLevelDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["locId"]=this.locId;
        data["itemId"]=this.itemId;
        data["minLevel"]=this.minLevel;
        data["maxLevel"]=this.maxLevel;
        data["ordLevel"]=this.ordLevel;
        data["createdBy"]=this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["audtUser"]=this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["id"]=this.id;
        data["itemName"]=this.itemName;
        return data; 
    }
}
