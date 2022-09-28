import * as moment from 'moment';
import { IPagedResultDtoOfOERETHeaderDto, IOERETHeaderDto, ICreateOrEditOERETHeaderDto } from '../interfaces/oeretHeader-interface';

export class PagedResultDtoOfOERETHeaderDto implements IPagedResultDtoOfOERETHeaderDto {
    totalCount!: number | undefined;
    items!: OERETHeaderDto[] | undefined;

    constructor(data?: IPagedResultDtoOfOERETHeaderDto) {
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
                    this.items!.push(OERETHeaderDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfOERETHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfOERETHeaderDto();
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

export class OERETHeaderDto implements IOERETHeaderDto {
    locID!: number | undefined;
    locDesc!: string | undefined;
    docNo!: number | undefined;
    docDate!: moment.Moment | undefined;
    paymentDate!: moment.Moment | undefined;
    typeID!: string | undefined; 
    typeDesc!: string | undefined; 
    custID!: number | undefined;
    customerName!: string | undefined; 
    priceList!: string | undefined; 
    narration!: string | undefined;
    ogp!: string | undefined;
    totalQty!: number | undefined;
    amount!: number | undefined;
    tax!: number | undefined;
    addTax!: number | undefined;
    disc!: number | undefined; 
    tradeDisc!: number | undefined; 
    margin!: number | undefined; 
    freight!: number | undefined; 
    ordNo!: number | undefined;
    totAmt!: number | undefined;
    posted!: boolean;
    sDocNo!: number | undefined;
    linkDetID!: number | undefined;
    active!:boolean;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;
    approved!: boolean;
    approvedBy!:string | undefined;
    approvedDate!:moment.Moment | undefined;
    

    constructor(data?: IOERETHeaderDto) {
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
            this.locDesc=data["locDesc"];
            this.docNo = data["docNo"];
            this.docDate = data["docDate"] ? moment(data["docDate"].toString()) : <any>undefined;
            this.paymentDate = data["paymentDate"]? moment(data["paymentDate"].toString()) : <any>undefined;
            this.typeID = data["typeID"];
            this.typeDesc = data["typeDesc"];
            this.custID = data["custID"];
            this.customerName = data["customerName"];
            this.priceList = data["priceList"];
            this.narration = data["narration"];
            this.ogp =data["ogp"];
            this.totalQty =data["totalQty"];
            this.amount = data["amount"];
            this.tax = data["tax"];
            this.addTax = data["addTax"];
            this.disc = data["disc"];
            this.tradeDisc = data["tradeDisc"];
            this.freight = data["freight"];
            this.margin = data["margin"];
            this.ordNo = data["ordNo"];
            this.totAmt = data["totAmt"];
            this.posted= data["posted"];
            this.sDocNo= data["sDocNo"];
            this.linkDetID = data["linkDetID"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.id = data["id"];
            this.approved = data["approved"];
            this.approvedBy = data["approvedBy"];
            this.approvedDate = data["approvedDate"];
        }
    }

    static fromJS(data: any): OERETHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new OERETHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {debugger
        debugger;
        data = typeof data === 'object' ? data : {};
        data["locID"]=this.locID;
        data["locDesc"]=this.locDesc;
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["paymentDate"]= this.paymentDate ? this.paymentDate.toISOString() : <any>undefined;;
        data["typeID"]= this.typeID;
        data["typeDesc"]= this.typeDesc;
        data["custID"]= this.custID;
        data["customerName"]= this.customerName;
        data["priceList"]= this.priceList;
        data["narration"] = this.narration;
        data["ogp"]= this.ogp;
        data["totalQty"]= this.totalQty;
        data["amount"]= this.amount ;
        data["tax"]= this.tax; 
        data["addTax"]= this.addTax;
        data["disc"]= this.disc;
        data["tradeDisc"]= this.tradeDisc;
        data["freight"]= this.freight;
        data["margin"]= this.margin;
        data["ordNo"]= this.ordNo;
        data["totAmt"]= this.totAmt;
        data["posted"]=this.posted;
        data["sDocNo"]=this.sDocNo;
        data["linkDetID"]=this.linkDetID;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        data["approved"] = this.approved;
        data["approvedBy"] = this.approvedBy;
        data["approvedDate"] = this.approvedDate;
        return data; 
    }
}

export class CreateOrEditOERETHeaderDto implements ICreateOrEditOERETHeaderDto {
    locID!: number | undefined;
        docNo!: number | undefined;
        docDate!: moment.Moment | undefined;
        paymentDate!: moment.Moment | undefined;
        typeID!: string | undefined; 
        custID!: number | undefined;
        priceList!: string | undefined; 
        narration!: string | undefined;
        ogp!: string | undefined;
        totalQty!: number | undefined;
        amount!: number | undefined;
        tax!: number | undefined;
        addTax!: number | undefined;
        disc!: number | undefined; 
        tradeDisc!: number | undefined; 
        margin!: number | undefined; 
        freight!: number | undefined; 
        ordNo!: number | undefined;
        totAmt!: number | undefined;
        posted!: boolean;
        sDocNo!: number | undefined;
        linkDetID!: number | undefined;
        active!:boolean;
        audtUser!: string | undefined;
        audtDate!: moment.Moment | undefined;
        createdBy!: string | undefined;
        createDate!: moment.Moment | undefined;
        id!: number | undefined;
        approved!:boolean;
        approvedBy!:string | undefined;
        approvedDate!:moment.Moment | undefined;
    constructor(data?: ICreateOrEditOERETHeaderDto) {
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
            this.docDate = data["docDate"] ? moment(data["docDate"].toString()) : <any>undefined;
            this.paymentDate = data["paymentDate"]? moment(data["paymentDate"].toString()) : <any>undefined;
            this.typeID = data["typeID"];
            this.custID = data["custID"];
            this.priceList = data["priceList"];
            this.narration = data["narration"];
            this.ogp =data["ogp"];
            this.totalQty =data["totalQty"];
            this.amount = data["amount"];
            this.tax = data["tax"];
            this.addTax = data["addTax"];
            this.disc = data["disc"];
            this.tradeDisc = data["tradeDisc"];
            this.freight = data["freight"];
            this.margin = data["margin"];
            this.ordNo = data["ordNo"];
            this.totAmt = data["totAmt"];
            this.posted= data["posted"];
            this.sDocNo= data["sDocNo"];
            this.linkDetID = data["linkDetID"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.id = data["id"];
            this.approved = data["approved"];
            this.approvedBy = data["approvedBy"];
            this.approvedDate = data["approvedDate"];
        }
    }

    static fromJS(data: any): CreateOrEditOERETHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditOERETHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["locID"]=this.locID;
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["paymentDate"]= this.paymentDate ? this.paymentDate.toISOString() : <any>undefined;;
        data["typeID"]= this.typeID;
        data["custID"]= this.custID;
        data["priceList"]= this.priceList;
        data["narration"] = this.narration;
        data["ogp"]= this.ogp;
        data["totalQty"]= this.totalQty;
        data["amount"]= this.amount ;
        data["tax"]= this.tax; 
        data["addTax"]= this.addTax;
        data["disc"]= this.disc;
        data["tradeDisc"]= this.tradeDisc;
        data["freight"]= this.freight;
        data["margin"]= this.margin;
        data["ordNo"]= this.ordNo;
        data["totAmt"]= this.totAmt;
        data["posted"]=this.posted;
        data["sDocNo"]=this.sDocNo;
        data["linkDetID"]=this.linkDetID;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        data["approved"] = this.approved;
        data["approvedBy"] = this.approvedBy;
        data["approvedDate"] = this.approvedDate;
        return data; 
    }
}
