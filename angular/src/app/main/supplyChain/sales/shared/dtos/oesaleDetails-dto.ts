import { IPagedResultDtoOfOESALEDetailDto, IOESALEDetailDto, ICreateOrEditOESALEDetailDto } from "../interfaces/oesaleDetail-interface";

export class PagedResultDtoOfOESALEDetailDto implements IPagedResultDtoOfOESALEDetailDto {
    totalCount!: number | undefined;
    items!: OESALEDetailDto[] | undefined;

    constructor(data?: IPagedResultDtoOfOESALEDetailDto) {
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
                    this.items!.push(OESALEDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfOESALEDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfOESALEDetailDto();
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

export class OESALEDetailDto implements IOESALEDetailDto {
    detID!:number | undefined;
    locID!:number | undefined;
    docNo!:number | undefined;
    itemID!: string | undefined; 
    itemDesc!: string | undefined; 
    unit!: string | undefined; 
    conver!: number | undefined;
    qty!: number | undefined;
    inHand!: number | undefined;
    rate!: number | undefined;
    amount!: number | undefined;
    exlTaxAmount !: number | undefined;
    disc!: number | undefined;
    taxAuth!: string | undefined;
    taxClass!: number | undefined;
    taxClassDesc!: string | undefined;
    taxRate!: number | undefined;
    taxAmt!: number | undefined;
    remarks!: string | undefined;
    netAmount!: string | undefined;
    id!: number | undefined;

    constructor(data?: IOESALEDetailDto) {
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
            this.qty = data["qty"];
            this.rate = data["rate"];
            this.exlTaxAmount = data["exlTaxAmount"];
            this.amount = data["amount"];
            this.disc = data["disc"];
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

    static fromJS(data: any): OESALEDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new OESALEDetailDto();
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
        data["exlTaxAmount"] = this.exlTaxAmount;
        data["amount"] = this.amount;
        data["disc"] = this.disc;
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

export class CreateOrEditOESALEDetailDto implements ICreateOrEditOESALEDetailDto {
    detID!:number | undefined;
    locID!:number | undefined;
    docNo!:number | undefined;
    itemID!: string | undefined; 
    unit!: string | undefined; 
    conver!: number | undefined;
    qty!: number | undefined;
    rate!: number | undefined;
    amount!: number | undefined;
    exlTaxAmount!: number | undefined;
    disc!: number | undefined;
    taxAuth!: string | undefined;
    taxClass!: number | undefined;
    taxRate!: number | undefined;
    taxAmt!: number | undefined;
    remarks!: string | undefined;
    netAmount!: string | undefined;
    id!: number | undefined;
    constructor(data?: ICreateOrEditOESALEDetailDto) {
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
            this.exlTaxAmount = data["exlTaxAmount"];
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

    static fromJS(data: any): CreateOrEditOESALEDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditOESALEDetailDto();
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
        data["exlTaxAmount"] = this.exlTaxAmount;
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
