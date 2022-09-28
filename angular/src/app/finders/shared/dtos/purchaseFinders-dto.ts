import * as moment from 'moment';
import { IPagedResultDtoOfPurchaseFindersDto, IPurchaseFindersDto } from '../interfaces/purchase-finders-interface';

export class PagedResultDtoOfPurchaseFindersDto implements IPagedResultDtoOfPurchaseFindersDto {
    totalCount!: number | undefined;
    items!: PurchaseFindersDto[] | undefined;

    constructor(data?: IPagedResultDtoOfPurchaseFindersDto) {
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
                    this.items!.push(PurchaseFindersDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfPurchaseFindersDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfPurchaseFindersDto();
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

export class PurchaseFindersDto implements IPurchaseFindersDto {
    id!: string | undefined;
    displayName!: string | undefined;
    location!: string | undefined;

    constructor(data?: IPurchaseFindersDto) {
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
            this.location = data["location"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): PurchaseFindersDto {
        data = typeof data === 'object' ? data : {};
        let result = new PurchaseFindersDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["displayName"]=this.displayName;
        data["location"]=this.location;
        data["id"]=this.id;
        return data; 
    }
}