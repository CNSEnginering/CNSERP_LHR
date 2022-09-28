import * as moment from 'moment';
import { IPagedResultDtoOfPOPODetailDto, IPOPODetailDto, ICreateOrEditPOPODetailDto } from '../interfaces/popoDetail-interface';

export class PagedResultDtoOfPOPODetailDto implements IPagedResultDtoOfPOPODetailDto {
    totalCount!: number | undefined;
    items!: POPODetailDto[] | undefined;

    constructor(data?: IPagedResultDtoOfPOPODetailDto) {
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
                    this.items!.push(POPODetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfPOPODetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfPOPODetailDto();
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

export class POPODetailDto implements IPOPODetailDto {
    detID!: number | undefined;
    locID!: number | undefined;
    docNo!: number | undefined;
    itemID!: string | undefined;
    itemDesc!: string | undefined;
    unit!: string | undefined;
    conver!: number | undefined;
    qty!: number| undefined;
    rate!: number | undefined;
    amount!: number | undefined;
    taxAuth!: string | undefined;
    taxClass!: number | undefined;
    taxClassDesc!: string | undefined;
    taxRate!: number | undefined;
    taxAmt!: number | undefined;
    remarks!: string | undefined;
    netAmount!: number | undefined;
    id!: number | undefined;
    transType!: number | undefined;
    poDocNo!: boolean | false;

    constructor(data?: IPOPODetailDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        debugger
        if (data) {
            this.detID = data["detID"];
            this.locID = data["locID"];
            this.docNo = data["docNo"];
            this.itemID = data["itemID"];
            this.itemDesc = data["itemDesc"];
            this.unit = data["unit"];
            this.conver = data["conver"];
            this.qty = data["qty"];
            this.rate = data["rate"];
            this.amount = data["amount"];
            this.taxAuth= data["taxAuth"];
            this.taxClass= data["taxClass"];
            this.taxClassDesc= data["taxClassDesc"];
            this.taxRate= data["taxRate"];
            this.taxAmt= data["taxAmt"];
            this.remarks = data["remarks"];
            this.netAmount= data["netAmount"];
            this.id = data["id"];
            this.transType = data["transType"];
            this.poDocNo = data["poDocNo"];
        }
    }

    static fromJS(data: any): POPODetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new POPODetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["locID"] = this.locID;
        data["docNo"] = this.docNo;
        data["itemID"] = this.itemID;
        data["itemDesc"] = this.itemDesc;
        data["unit"] = this.unit;
        data["conver"] = this.conver;
        data["qty"] = this.qty;
        data["rate"] = this.rate;
        data["amount"] = this.amount;
        data["taxAuth"]= this.taxAuth;
        data["taxClass"]= this.taxClass;
        data["taxClassDesc"]= this.taxClassDesc;
        data["taxRate"]= this.taxRate;
        data["taxAmt"]= this.taxAmt;
        data["remarks"] = this.remarks;
        data["netAmount"]= this.netAmount;
        data["id"] = this.id;
        data["transType"] = this.transType;
        data["poDocNo"]=this.poDocNo;
        return data; 
    }
}

export class CreateOrEditPOPODetailDto implements ICreateOrEditPOPODetailDto {
    detID!: number | undefined;
    locID!: number | undefined;
    docNo!: number | undefined;
    itemID!: string | undefined;
    unit!: string | undefined;
    conver!: number | undefined;
    qty!: number| undefined;
    rate!: number | undefined;
    amount!: number | undefined;
    taxAuth!: string | undefined;
    taxClass!: number | undefined;
    taxRate!: number | undefined;
    taxAmt!: number | undefined;
    remarks!: string | undefined;
    netAmount!: number | undefined;
    id!: number | undefined;
    transType!: number | undefined;
    constructor(data?: ICreateOrEditPOPODetailDto) {
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
            this.locID = data["locID"];
            this.docNo = data["docNo"];
            this.itemID = data["itemID"];
            this.unit = data["unit"];
            this.conver = data["conver"];
            this.qty = data["qty"];
            this.rate = data["rate"];
            this.amount = data["amount"];
            this.taxAuth= data["taxAuth"];
            this.taxClass= data["taxClass"];
            this.taxRate= data["taxRate"];
            this.taxAmt= data["taxAmt"];
            this.remarks = data["remarks"];
            this.netAmount= data["netAmount"];
            this.id = data["id"];
            this.transType = data["transType"];
        }
    }

    static fromJS(data: any): CreateOrEditPOPODetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditPOPODetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["locID"] = this.locID;
        data["docNo"] = this.docNo;
        data["itemID"] = this.itemID;
        data["unit"] = this.unit;
        data["conver"] = this.conver;
        data["qty"] = this.qty;
        data["rate"] = this.rate;
        data["amount"] = this.amount;
        data["taxAuth"]= this.taxAuth;
        data["taxClass"]= this.taxClass;
        data["taxRate"]= this.taxRate;
        data["taxAmt"]= this.taxAmt;
        data["remarks"] = this.remarks;
        data["netAmount"]= this.netAmount;
        data["id"] = this.id;
        data["transType"] = this.transType;
        return data; 
    }
}
