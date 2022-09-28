import * as moment from 'moment';
import { IPagedResultDtoOfToolTypeFindersDto, IToolTypeFindersDto } from '../interfaces/ToolType-finders-interface';

export class PagedResultDtoOfToolTypeFindersDto implements IPagedResultDtoOfToolTypeFindersDto {
    totalCount!: number | undefined;
    items!: ToolTypeFindersDto[] | undefined;

    constructor(data?: IPagedResultDtoOfToolTypeFindersDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalCount = data["count"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [] as any;
                for (let item of data["items"])
                    this.items!.push(ToolTypeFindersDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfToolTypeFindersDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfToolTypeFindersDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["count"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        return data;
    }
}

export class ToolTypeFindersDto implements IToolTypeFindersDto {
    id!: string | undefined;
    displayName!: string | undefined;
    typeId!:string | undefined;
    unitCost!: string | undefined;
    uom!: string | undefined;
    constructor(data?: IToolTypeFindersDto) {
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
        }
    }

    static fromJS(data: any): ToolTypeFindersDto {
        data = typeof data === 'object' ? data : {};
        let result = new ToolTypeFindersDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
     
        data["displayName"] = this.displayName;
        data["id"] = this.id;
        return data;
    }
}