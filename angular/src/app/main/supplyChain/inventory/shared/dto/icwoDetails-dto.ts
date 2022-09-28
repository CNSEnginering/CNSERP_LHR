import * as moment from 'moment';
import { IPagedResultDtoOfICWODetailDto, IICWODetailDto, ICreateOrEditICWODetailDto } from '../interface/icwoDetail-interface';

export class PagedResultDtoOfICWODetailDto implements IPagedResultDtoOfICWODetailDto {
    totalCount!: number | undefined;
    items!: ICWODetailDto[] | undefined;

    constructor(data?: IPagedResultDtoOfICWODetailDto) {
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
                    this.items!.push(ICWODetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfICWODetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfICWODetailDto();
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

export class ICWODetailDto implements IICWODetailDto {
    detID!: number | undefined;
    docNo!: number | undefined;
    locID!: number | undefined;
    itemID!: string | undefined;
    itemDesc!: string | undefined;
    unit!: string | undefined;
    conver!: number | undefined;
    qty!: number| undefined;
    cost!: number | undefined;
    amount!: number | undefined;
    remarks!: string | undefined;
    id!: number | undefined;
    subCCID!: string | undefined;
    subCCName!: string | undefined;

    constructor(data?: IICWODetailDto) {
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
            this.locID = data["locID"];
            this.itemID = data["itemID"];
            this.itemDesc = data["itemDesc"];
            this.unit = data["unit"];
            this.conver = data["conver"];
            this.qty = data["qty"];
            this.cost = data["cost"];
            this.amount = data["amount"];
            this.remarks = data["remarks"];
            this.id = data["id"];
            this.subCCID = data["subCCID"];
            this.subCCName = data["subCCName"];
        }
    }

    static fromJS(data: any): ICWODetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new ICWODetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["docNo"] = this.docNo;
        data["locID"] = this.locID;
        data["itemID"] = this.itemID;
        data["itemDesc"] = this.itemDesc;
        data["unit"] = this.unit;
        data["conver"] = this.conver;
        data["qty"] = this.qty;
        data["cost"] = this.cost;
        data["amount"] = this.amount;
        data["remarks"] = this.remarks;
        data["id"] = this.id;
        data["subCCID"] = this.subCCID;
        data["subCCName"] = this.subCCName;
        return data; 
    }
}

export class CreateOrEditICWODetailDto implements ICreateOrEditICWODetailDto {
    detID!: number | undefined;
    docNo!: number | undefined;
    locID!: number | undefined;
    itemID!: string | undefined;
    unit!: string | undefined;
    conver!: number | undefined;
    qty!: number| undefined;
    cost!: number | undefined;
    amount!: number | undefined;
    remarks!: string | undefined;
    id!: number | undefined;
    subCCID!: string | undefined;
    subCCName!: string | undefined;

    constructor(data?: ICreateOrEditICWODetailDto) {
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
            this.locID = data["locID"];
            this.itemID = data["itemID"];
            this.unit = data["unit"];
            this.conver = data["conver"];
            this.qty = data["qty"];
            this.cost = data["cost"];
            this.amount = data["amount"];
            this.remarks = data["remarks"];
            this.id = data["id"];
            this.subCCID = data["subCCID"];
            this.subCCName = data["subCCName"];
        }
    }

    static fromJS(data: any): CreateOrEditICWODetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditICWODetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["docNo"] = this.docNo;
        data["locID"] = this.locID;
        data["itemID"] = this.itemID;
        data["unit"] = this.unit;
        data["conver"] = this.conver;
        data["qty"] = this.qty;
        data["cost"] = this.cost;
        data["amount"] = this.amount;
        data["remarks"] = this.remarks;
        data["id"] = this.id;
        data["subCCID"] = this.subCCID;
        data["subCCName"] = this.subCCName;
        return data; 
    }
}
