import * as moment from 'moment';
import { IICADJDetailDto, ICreateOrEditICADJDetailDto, IPagedResultDtoOfICADJDetailDto } from '../interface/icadjDetail-interface';

export class PagedResultDtoOfICADJDetailDto implements IPagedResultDtoOfICADJDetailDto {
    totalCount!: number | undefined;
    items!: ICADJDetailDto[] | undefined;

    constructor(data?: IPagedResultDtoOfICADJDetailDto) {
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
                    this.items!.push(ICADJDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfICADJDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfICADJDetailDto();
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

export class ICADJDetailDto implements IICADJDetailDto {
    detID!: number | undefined;
    docNo!: number | undefined;
    itemID!: string | undefined;
    itemDesc!: string | undefined;
    unit!: string | undefined;
    conver!: number | undefined;
    type:string | undefined;
    qty!: number| undefined;
    cost!: number | undefined;
    amount!: number | undefined;
    remarks!: string | undefined;
    id!: number | undefined;

    constructor(data?: IICADJDetailDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.detID = data["detID"];
            this.docNo = data["docNo"];
            this.itemID = data["itemID"];
            this.itemDesc=data["itemDesc"];
            this.unit = data["unit"];
            this.conver = data["conver"];
            this.type = data["type"];
            this.qty = data["qty"];
            this.cost = data["cost"];
            this.amount = data["amount"];
            this.remarks = data["remarks"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): ICADJDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new ICADJDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["docNo"] = this.docNo;
        data["itemID"] = this.itemID;
        data["itemDesc"] = this.itemDesc;
        data["unit"] = this.unit;
        data["conver"] = this.conver;
        data["type"] = this.type;
        data["qty"] = this.qty;
        data["cost"] = this.cost;
        data["amount"] = this.amount;
        data["remarks"] = this.remarks;
        data["id"] = this.id;
        return data; 
    }
}

export class CreateOrEditICADJDetailDto implements ICreateOrEditICADJDetailDto {
    detID!: number | undefined;
    docNo!: number | undefined;
    itemID!: string | undefined;
    itemDesc!:string|undefined;
    unit!: string | undefined;
    conver!: number | undefined;
    type!:string | undefined;
    qty!: number| undefined;
    cost!: number | undefined;
    amount!: number | undefined;
    remarks!: string | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditICADJDetailDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.detID = data["detID"];
            this.docNo = data["docNo"];
            this.itemID = data["itemID"];
            this.unit = data["unit"];
            this.conver = data["conver"];
            this.type = data["type"];
            this.qty = data["qty"];
            this.cost = data["cost"];
            this.amount = data["amount"];
            this.remarks = data["remarks"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditICADJDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditICADJDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["docNo"] = this.docNo;
        data["itemID"] = this.itemID;
        data["itemDesc"] = this.itemDesc;
        data["unit"] = this.unit;
        data["conver"] = this.conver;
        data["type"] = this.type;
        data["qty"] = this.qty;
        data["cost"] = this.cost;
        data["amount"] = this.amount;
        data["remarks"] = this.remarks;
        data["id"] = this.id;
        return data; 
    }
}
