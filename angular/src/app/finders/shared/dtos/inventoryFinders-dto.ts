import * as moment from 'moment';
import { IPagedResultDtoOfInventoryFindersDto, IInventoryFindersDto } from '../interfaces/inventory-finders-interface';

export class PagedResultDtoOfInventoryFindersDto implements IPagedResultDtoOfInventoryFindersDto {
    totalCount!: number | undefined;
    items!: InventoryFindersDto[] | undefined;

    constructor(data?: IPagedResultDtoOfInventoryFindersDto) {
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
                    this.items!.push(InventoryFindersDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfInventoryFindersDto {
        debugger
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfInventoryFindersDto();
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

export class InventoryFindersDto implements IInventoryFindersDto {
    id!: string | undefined;
    displayName!: string | undefined;
    termRate!: number | undefined;
    unit!: string | undefined;
    conver!: number | undefined;
    option5!: string | undefined;
    rate!: number | undefined;
    manfDate:Date|string;
    expiryDate:Date|string;
    constructor(data?: IInventoryFindersDto) {
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
            this.displayName = data["displayName"];
            this.id = data["id"];
            this.unit=data["unit"];
            this.conver=data["conver"];
            this.option5=data["option5"];
            this.rate=data["rate"];
            this.expiryDate=data["expiryDate"];
            this.manfDate=data["manfDate"];
        }
    }

    static fromJS(data: any): InventoryFindersDto {
       
        data = typeof data === 'object' ? data : {};
        let result = new InventoryFindersDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["displayName"]=this.displayName;
        data["id"]=this.id;
        data["unit"]=this.unit;
        data["conver"]=this.conver;
        data["option5"]=this.option5;
        data["rate"]=this.rate;
        data["expiryDate"]=this.expiryDate;
        data["manfDate"]=this.manfDate;
        return data; 
    }
}