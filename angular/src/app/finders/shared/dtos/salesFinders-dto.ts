import * as moment from 'moment';
import { IPagedResultDtoOfSalesFindersDto, ISalesFindersDto } from '../interfaces/sales-finders-interface';

export class PagedResultDtoOfSalesFindersDto implements IPagedResultDtoOfSalesFindersDto {
    totalCount!: number | undefined;
    items!: SalesFindersDto[] | undefined;

    constructor(data?: IPagedResultDtoOfSalesFindersDto) {
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
                    this.items!.push(SalesFindersDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfSalesFindersDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfSalesFindersDto();
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

export class SalesFindersDto implements ISalesFindersDto {
    id!: string | undefined;
    displayName!: string | undefined;
    location!: string | undefined;

    constructor(data?: ISalesFindersDto) {
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

    static fromJS(data: any): SalesFindersDto {
        data = typeof data === 'object' ? data : {};
        let result = new SalesFindersDto();
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