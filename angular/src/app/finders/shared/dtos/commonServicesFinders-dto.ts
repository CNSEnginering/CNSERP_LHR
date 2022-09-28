import * as moment from 'moment';
import { IPagedResultDtoOfCommonServiceFindersDto, ICommonServiceFindersDto } from '../interfaces/commonServices-finders-interface';
import { UrlHandlingStrategy } from '@angular/router';

export class PagedResultDtoOfCommonServiceFindersDto implements IPagedResultDtoOfCommonServiceFindersDto {
    totalCount!: number | undefined;
    items!: CommonServiceFindersDto[] | undefined;

    constructor(data?: IPagedResultDtoOfCommonServiceFindersDto) {
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
                    this.items!.push(CommonServiceFindersDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfCommonServiceFindersDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfCommonServiceFindersDto();
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

export class CommonServiceFindersDto implements ICommonServiceFindersDto {
    id!: string | undefined;
    displayName!: string | undefined;
    accountID!: string | undefined;
    docType!: number | undefined;
    availableLimit!: number | undefined;
    currRate!: number | undefined;
    taxRate!: number | undefined;
    detId!: number | undefined;
    narration! : string  | undefined;
    bKAccountID! :string |undefined;

    constructor(data?: ICommonServiceFindersDto) {
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
            this.accountID = data["accountID"];
            this.docType = data["docType"];
            this.availableLimit= data["availableLimit"];
            this.currRate= data["currRate"];
            this.taxRate= data["taxRate"];
            this.id = data["id"];
            this.detId = data["detId"];
            this.narration = data["narration"];
            this.bKAccountID=data["bkAccountID"]
        }
    }

    static fromJS(data: any): CommonServiceFindersDto {
        data = typeof data === 'object' ? data : {};
        let result = new CommonServiceFindersDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["displayName"]=this.displayName;
        data["accountID"]=this.accountID;
        data["docType"]=this.docType;
        data["availableLimit"]= this.availableLimit;
        data["currRate"]= this.currRate;
        data["taxRate"]= this.taxRate;
        data["id"]=this.id;
        data["detId"]=this.detId;
        data["narration"]=this.narration;
        data["bkAccountID"]=this.bKAccountID;
        return data; 
    }
}