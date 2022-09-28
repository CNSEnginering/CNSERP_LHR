import * as moment from 'moment';
import { IPagedResultDtoOfPOPOHeaderDto, IPOPOHeaderDto, ICreateOrEditPOPOHeaderDto } from '../interfaces/popoHeader-interface';

export class PagedResultDtoOfPOPOHeaderDto implements IPagedResultDtoOfPOPOHeaderDto {
    totalCount!: number | undefined;
    items!: POPOHeaderDto[] | undefined;

    constructor(data?: IPagedResultDtoOfPOPOHeaderDto) {
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
                    this.items!.push(POPOHeaderDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfPOPOHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfPOPOHeaderDto();
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

export class POPOHeaderDto implements IPOPOHeaderDto {
    locID!:number | undefined;
    docNo!:number | undefined;
    docDate!: Date | undefined;
    arrivalDate!: Date | undefined;
    reqNo!: number | undefined;
    accountID!: string | undefined;
    accDesc!: string | undefined;
    subAccID!: number | undefined;
    subAccDesc!: string | undefined;
    totalQty!:number | undefined;
    totalAmt!:number | undefined;
    ordNo!: string | undefined;
    ccid!: string | undefined;
    narration!: string | undefined;
    whTermID!: number | undefined;
    whTermDesc!: string | undefined;
    whRate!: number | undefined;
    taxAuth!: string | undefined;
    taxAuthDesc!: string | undefined;
    taxClass!: number | undefined;
    taxClassDesc!: string | undefined;
    taxRate!: number | undefined;
    taxAmount!: number | undefined;
    onHold!: boolean;
    active!:boolean;
    completed!:boolean;
    audtUser!: string | undefined;
    audtDate!:Date;
    createdBy!: string | undefined;
    createDate!: Date;
    approved!:boolean | undefined;
    id!: number | undefined;
    terms!:string | undefined;
    itemPrice!: string | undefined;

    constructor(data?: IPOPOHeaderDto) {
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
            this.locID=data["locID"];
            this.docNo = data["docNo"];
            this.docDate = data["docDate"];
            this.arrivalDate = data["arrivalDate"];
            this.reqNo = data["reqNo"];
            this.accountID = data["accountID"];
            this.accDesc = data["accDesc"];
            this.subAccID = data["subAccID"];
            this.subAccDesc = data["subAccDesc"];
            this.totalQty = data["totalQty"];
            this.totalAmt = data["totalAmt"];
            this.ordNo = data["ordNo"];
            this.ccid = data["ccid"];
            this.narration = data["narration"];
            this.whTermID = data["whTermID"];
            this.whTermDesc = data["whTermDesc"];
            this.whRate = data["whRate"];
            this.taxAuth = data["taxAuth"];
            this.taxAuthDesc = data["taxAuthDesc"];
            this.taxClass = data["taxClass"];
            this.taxClassDesc = data["taxClassDesc"];
            this.taxRate = data["taxRate"];
            this.taxAmount = data["taxAmount"];
            this.onHold = data["onHold"];
            this.active = data["active"];
            this.completed = data["completed"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.approved=data["approved"];
            this.id = data["id"];
            this.terms= data["terms"];
            this.itemPrice = data["itemPrice"];
        }
    }

    static fromJS(data: any): POPOHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new POPOHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {debugger
        debugger;
        data = typeof data === 'object' ? data : {};
        data["locID"]=this.locID;
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate;
        data["arrivalDate"]= this.arrivalDate;
        data["reqNo"]= this.reqNo;
        data["accountID"]= this.accountID;
        data["accDesc"]= this.accDesc;
        data["subAccID"]= this.subAccID; 
        data["subAccDesc"]= this.subAccDesc; 
        data["totalQty"]= this.totalQty;
        data["totalAmt"]= this.totalAmt;
        data["ordNo"]= this.ordNo;
        data["ccid"]= this.ccid;
        data["narration"] = this.narration;
        data["whTermID"]= this.whTermID;
        data["whTermDesc"]= this.whTermDesc;
        data["whRate"]= this.whRate;
        data["taxAuth"]= this.taxAuth;
        data["taxAuthDesc"]= this.taxAuthDesc;
        data["taxClass"]= this.taxClass;
        data["taxClassDesc"]= this.taxClassDesc;
        data["taxRate"]= this.taxRate;
        data["taxAmount"]= this.taxAmount;
        data["onHold"]= this.onHold;
        data["active"] = this.active;
        data["completed"] = this.completed;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ;
        data["approved"]=this.approved;
        data["id"] = this.id;
        data["terms"]=this.terms;
        data["itemPrice"]=this.itemPrice;
        return data; 
    }
}

export class CreateOrEditPOPOHeaderDto implements ICreateOrEditPOPOHeaderDto {
    locID!:number | undefined;
    docNo!:number | undefined;
    docDate!: Date | undefined;
    arrivalDate!: Date| undefined;
    reqNo!: number | undefined;
    accountID!: string | undefined;
    subAccID!: number | undefined;
    totalQty!:number | undefined;
    totalAmt!:number | undefined;
    ordNo!: string | undefined;
    ccid!: string | undefined;
    narration!: string | undefined;
    whTermID!: number | undefined;
    whRate!: number | undefined;
    taxAuth!: string | undefined;
    taxClass!: number | undefined;
    taxRate!: number | undefined;
    taxAmount!: number | undefined;
    onHold!: boolean;
    active:boolean=true;
    audtUser!: string | undefined;
    audtDate!: Date ;
    createdBy!: string | undefined;
    createDate!: Date;
    completed!:boolean | undefined;
    id!: number | undefined;
    terms!:string | undefined;
    approved!:boolean | undefined;
    itemPrice!:string | undefined;


    constructor(data?: ICreateOrEditPOPOHeaderDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.locID=data["locID"];
            this.docNo = data["docNo"];
            this.docDate = data["docDate"];
            this.arrivalDate = data["arrivalDate"] ;
            this.reqNo = data["reqNo"];
            this.accountID = data["accountID"];
            this.subAccID = data["subAccID"];
            this.totalQty = data["totalQty"];
            this.totalAmt = data["totalAmt"];
            this.ordNo = data["ordNo"];
            this.ccid = data["ccid"];
            this.narration = data["narration"];
            this.whTermID = data["whTermID"];
            this.whRate = data["whRate"];
            this.taxAuth = data["taxAuth"];
            this.taxClass = data["taxClass"];
            this.taxRate = data["taxRate"];
            this.taxAmount = data["taxAmount"];
            this.onHold = data["onHold"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.completed = data["completed"];
            this.id = data["id"];
            this.terms = data["terms"];
            this.approved=data["approved"];
            this.itemPrice=data["itemPrice"];
        }
    }

    static fromJS(data: any): CreateOrEditPOPOHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditPOPOHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["locID"]=this.locID;
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate;
        data["arrivalDate"]= this.arrivalDate ;
        data["reqNo"]= this.reqNo;
        data["accountID"]= this.accountID;
        data["subAccID"]= this.subAccID; 
        data["totalQty"]= this.totalQty;
        data["totalAmt"]= this.totalAmt;
        data["ordNo"]= this.ordNo;
        data["ccid"]= this.ccid;
        data["narration"] = this.narration;
        data["whTermID"]= this.whTermID;
        data["whRate"]= this.whRate;
        data["taxAuth"]= this.taxAuth;
        data["taxClass"]= this.taxClass;
        data["taxRate"]= this.taxRate;
        data["taxAmount"]= this.taxAmount;
        data["onHold"]= this.onHold;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ;
        data["completed"] = this.completed;
        data["id"] = this.id;
        data["terms"]=this.terms;
        data["approved"]=this.approved;
        data["itemPrice"]=this.itemPrice;
        return data; 
    }
}
