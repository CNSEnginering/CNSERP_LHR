import * as moment from 'moment';
import { IPagedResultDtoOfICCNSDetailDto, IICCNSDetailDto, ICreateOrEditICCNSDetailDto } from '../interface/iccnsDetail-interface';

export class PagedResultDtoOfICCNSDetailDto implements IPagedResultDtoOfICCNSDetailDto {
    totalCount!: number | undefined;
    items!: ICCNSDetailDto[] | undefined;

    constructor(data?: IPagedResultDtoOfICCNSDetailDto) {
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
                    this.items!.push(ICCNSDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfICCNSDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfICCNSDetailDto();
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

export class ICCNSDetailDto implements IICCNSDetailDto {
    detID!: number | undefined;
    docNo!: number | undefined;
    itemID!: string | undefined;
    itemDesc!: string | undefined;
    unit!: string | undefined;
    conver!: number | undefined;
    qty!: number| undefined;
    Wqty!: number| undefined;
    qtyInHand!: number| undefined
    cost!: number | undefined;
    amount!: number | undefined;
    remarks!: string | undefined;
    engNo!: string | undefined;
    subCCID!: string | undefined;
    subCCName!: string | undefined;
    id!: number | undefined;

    constructor(data?: IICCNSDetailDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        debugger;
        if (data) {
            this.detID = data["detID"];
            this.docNo = data["docNo"];
            this.itemID = data["itemID"];
            this.itemDesc = data["itemDesc"];
            this.unit = data["unit"];
            this.conver = data["conver"];
            this.qty = data["qty"];
            this.Wqty = data["qty"];
            this.qtyInHand = data["qtyInHand"];
            this.cost = data["cost"];
            this.amount = data["amount"];
            this.remarks = data["remarks"];
            this.engNo = data["engNo"];
            this.subCCID = data["subCCID"];
            this.subCCName = data["subCCName"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): ICCNSDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new ICCNSDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["docNo"] = this.docNo;
        data["itemID"] = this.itemID;
        data["itemDesc"] = this.itemDesc;
        data["unit"] = this.unit;
        data["conver"] = this.conver;
        data["qty"] = this.qty;
        data["Wqty"] = this.qty;
        data["qtyInHand"] = this.qtyInHand;
        data["cost"] = this.cost;
        data["amount"] = this.amount;
        data["remarks"] = this.remarks;
        data["engNo"] = this.engNo;
        data["subCCID"] = this.subCCID;
        data["subCCName"] = this.subCCName;
        data["id"] = this.id;
        return data; 
    }
}

export class CreateOrEditICCNSDetailDto implements ICreateOrEditICCNSDetailDto {
    detID!: number | undefined;
    docNo!: number | undefined;
    itemID!: string | undefined;
    unit!: string | undefined;
    conver!: number | undefined;
    qty!: number| undefined;
    Wqty!: number| undefined;
    cost!: number | undefined;
    amount!: number | undefined;
    remarks!: string | undefined;
    engNo!: string | undefined;
    subCCID!: string | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditICCNSDetailDto) {
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
            this.qty = data["qty"];
            this.Wqty = data["Wqty"];
            this.cost = data["cost"];
            this.amount = data["amount"];
            this.remarks = data["remarks"];
            this.engNo = data["engNo"];
            this.subCCID = data["subCCID"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditICCNSDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditICCNSDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["docNo"] = this.docNo;
        data["itemID"] = this.itemID;
        data["unit"] = this.unit;
        data["conver"] = this.conver;
        data["qty"] = this.qty;
        data["cost"] = this.cost;
        data["amount"] = this.amount;
        data["Wqty"] = this.Wqty;
        data["remarks"] = this.remarks;
        data["engNo"] = this.engNo;
        data["subCCID"] = this.subCCID;
        data["id"] = this.id;
        return data; 
    }
}
