import * as moment from 'moment';
import { IPagedResultDtoOfPayRollFindersDto, IPayRollFindersDto } from '../interfaces/payRoll-finders-interface';

export class PagedResultDtoOfPayRollFindersDto implements IPagedResultDtoOfPayRollFindersDto {
    totalCount!: number | undefined;
    items!: PayRollFindersDto[] | undefined;

    constructor(data?: IPagedResultDtoOfPayRollFindersDto) {
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
                    this.items!.push(PayRollFindersDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfPayRollFindersDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfPayRollFindersDto();
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

export class PayRollFindersDto implements IPayRollFindersDto {
    id!: string | undefined;
    displayName!: string | undefined;
    dutyHours!: string | undefined;
    joiningDate!: string | undefined;
    constructor(data?: IPayRollFindersDto) {
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
            this.dutyHours = data["dutyHours"];
            this.joiningDate = data["joiningDate"];
        }
    }

    static fromJS(data: any): PayRollFindersDto {
        data = typeof data === 'object' ? data : {};
        let result = new PayRollFindersDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["displayName"]=this.displayName;
        data["id"]=this.id;
        data["dutyHours"]=this.dutyHours;
        data["joiningDate"]=this.joiningDate;
        return data; 
    }
}