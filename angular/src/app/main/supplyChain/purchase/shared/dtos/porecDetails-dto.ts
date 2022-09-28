import { IPagedResultDtoOfPORECDetailDto, IPORECDetailDto, ICreateOrEditPORECDetailDto } from "../interfaces/porecDetail-interface";

export class PagedResultDtoOfPORECDetailDto implements IPagedResultDtoOfPORECDetailDto {
    totalCount!: number | undefined;
    items!: PORECDetailDto[] | undefined;

    constructor(data?: IPagedResultDtoOfPORECDetailDto) {
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
                    this.items!.push(PORECDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfPORECDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfPORECDetailDto();
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

export class PORECDetailDto implements IPORECDetailDto {
    detID!: number | undefined;
    locID!: number | undefined;
    docNo!: number | undefined;
    itemID!: string | undefined;
    itemDesc!: string | undefined;
    unit!: string | undefined;
    conver!: number | undefined;
    poQty!: number| undefined;
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

    constructor(data?: IPORECDetailDto) {
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
            this.itemDesc = data["itemDesc"];
            this.unit = data["unit"];
            this.conver = data["conver"];
            this.poQty = data["poQty"];
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
        }
    }

    static fromJS(data: any): PORECDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new PORECDetailDto();
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
        data["poQty"] = this.poQty;
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
        return data; 
    }
}

export class CreateOrEditPORECDetailDto implements ICreateOrEditPORECDetailDto {
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
    constructor(data?: ICreateOrEditPORECDetailDto) {
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
        }
    }

    static fromJS(data: any): CreateOrEditPORECDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditPORECDetailDto();
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
        return data; 
    }
}
