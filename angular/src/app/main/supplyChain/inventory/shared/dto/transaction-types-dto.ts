import * as moment from 'moment';
import { ITransactionTypeDto, IPagedResultDtoOfTransactionTypeDto } from '../interface/transaction-types-interface';

export class PagedResultDtoOfTransactionTypeDto implements IPagedResultDtoOfTransactionTypeDto {
    totalCount!: number | undefined;
    items!: TransactionTypeDto[] | undefined;

    constructor(data?: IPagedResultDtoOfTransactionTypeDto) {
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
                    this.items!.push(TransactionTypeDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfTransactionTypeDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfTransactionTypeDto();
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

export class TransactionTypeDto implements ITransactionTypeDto {
    typeId!: string | undefined;
    description!: string | undefined;
    active!: boolean;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ITransactionTypeDto) {
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
            this.typeId = data["typeId"];
            this.description = data["description"];
            this.active = data["active"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"]? moment(data["createDate"].toString()) : <any>undefined;
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"]? moment(data["audtDate"].toString()) : <any>undefined;
            this.id = data["id"];
        }
    }

    static fromJS(data: any): TransactionTypeDto {
        data = typeof data === 'object' ? data : {};
        let result = new TransactionTypeDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["typeId"]=this.typeId;
        data["description"]=this.description;
        data["active"]=this.active;
        data["createdBy"]=this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["audtUser"]=this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["id"]=this.id;
        return data; 
    }
}
