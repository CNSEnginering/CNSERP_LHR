import * as moment from 'moment';
import { IPagedResultDtoOfOESALEHeaderDto, IOESALEHeaderDto, ICreateOrEditOESALEHeaderDto } from '../interfaces/oesaleHeader-interface';

export class PagedResultDtoOfOESALEHeaderDto implements IPagedResultDtoOfOESALEHeaderDto {
    totalCount!: number | undefined;
    items!: OESALEHeaderDto[] | undefined;

    constructor(data?: IPagedResultDtoOfOESALEHeaderDto) {
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
                    this.items!.push(OESALEHeaderDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfOESALEHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfOESALEHeaderDto();
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

export class OESALEHeaderDto implements IOESALEHeaderDto {
    locID!: number | undefined;
    locDesc!: string | undefined;
    docNo!: number | undefined;
    docDate!: moment.Moment | undefined;
    paymentDate!: moment.Moment | undefined;
    refID!: string | undefined; 
    refName!: string | undefined; 
    typeID!: string | undefined; 
    typeDesc!: string | undefined; 
    custID!: number | undefined;
    customerName!: string | undefined; 
    delvTerms!: string | undefined; 
    priceList!: string | undefined; 
    narration!: string | undefined;
    ogp!: string | undefined;
    basicStyle!: string | undefined;
    license!: string | undefined;
    totalQty!: number | undefined;
    amount!: number | undefined;
    exlTaxAmount !: number | undefined;
    tax!: number | undefined;
    addTax!: number | undefined;
    disc!: number | undefined; 
    tradeDisc!: number | undefined; 
    margin!: number | undefined; 
    freight!: number | undefined; 
    ordNo!: number | undefined;
    totAmt!: number | undefined;
    posted!: boolean;
    active!:boolean;
    linkDetID!: number | undefined;
    approved!:boolean;
    approvedBy!:string | undefined;
    approvedDate!:moment.Moment | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;
    qutationDoc:string|undefined;
    driverName!: string | undefined;
    routDesc!: string | undefined;
    vehicleNo!: string | undefined;
    vehicleType!: number | undefined;
    routID!: number | undefined;
    driverCtrlAcc !: string | undefined;
    driverID !: number | undefined;
    driverSubAccID !: number | undefined;


    constructor(data?: IOESALEHeaderDto) {
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
            this.refID = data["refID"];
            this.refName = data["refName"];
            this.typeID = data["typeID"];
            this.typeDesc = data["typeDesc"];
            this.custID = data["custID"];
            this.customerName = data["customerName"];
            this.priceList = data["priceList"];
            this.narration = data["narration"];
            this.ogp =data["ogp"];
            this.basicStyle =data["basicStyle"];
            this.license =data["license"];
            this.totalQty =data["totalQty"];
            this.amount = data["amount"];
            this.exlTaxAmount = data["exlTaxAmount"];
            this.tax = data["tax"];
            this.addTax = data["addTax"];
            this.disc = data["disc"];
            this.tradeDisc = data["tradeDisc"];
            this.freight = data["freight"];
            this.margin = data["margin"];
            this.ordNo = data["ordNo"];
            this.totAmt = data["totAmt"];
            this.posted= data["posted"];
            this.linkDetID = data["linkDetID"];
            this.active = data["active"];
            this.approved = data["approved"];
            this.approvedBy = data["approvedBy"];
            this.approvedDate = data["approvedDate"] ? moment(data["approvedDate"].toString()) : <any>undefined;
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.id = data["id"];
            this.qutationDoc=data["qutationDoc"];
            this.delvTerms=data["delvTerms"];
            this.driverName=data["driverName"];
            this.vehicleNo=data["vehicleNo"];
            this.vehicleType=data["vehicleType"];
            this.routID=data["routID"];
            this.routDesc=data["routDesc"];
            this.driverCtrlAcc=data["driverCtrlAcc"];
            this.driverID=data["driverID"];
            this.driverSubAccID=data["driverSubAccID"]
        }
    }

    static fromJS(data: any): OESALEHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new OESALEHeaderDto();
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
        data["refID"]= this.refID;
        data["refName"]= this.refName;
        data["typeID"]= this.typeID;
        data["typeDesc"]= this.typeDesc;
        data["custID"]= this.custID;
        data["customerName"]= this.customerName;
        data["priceList"]= this.priceList;
        data["narration"] = this.narration;
        data["ogp"]= this.ogp;
        data["basicStyle"]= this.basicStyle;
        data["license"]= this.license;
        data["totalQty"]= this.totalQty;
        data["amount"]= this.amount ;
        data["exlTaxAmount"]= this.exlTaxAmount ;
        data["tax"]= this.tax; 
        data["addTax"]= this.addTax;
        data["disc"]= this.disc;
        data["tradeDisc"]= this.tradeDisc;
        data["freight"]= this.freight;
        data["margin"]= this.margin;
        data["ordNo"]= this.ordNo;
        data["totAmt"]= this.totAmt;
        data["posted"]=this.posted;
        data["linkDetID"]=this.linkDetID;
        data["active"] = this.active;
        data["approved"] = this.approved;
        data["approvedBy"] = this.approvedBy;
        data["approvedDate"] = this.approvedDate ? this.approvedDate.toISOString() : <any>undefined;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        data["qutationDoc"]=this.qutationDoc;
        data["delvTerms"]=this.delvTerms;
        data["driverName"]=this.driverName;
        data["vehicleNo"]=this.vehicleNo;
        data["vehicleType"] = this.vehicleType;
        data["routID"] = this.routID;
        data["routDesc"]=this.routDesc;
        data["driverCtrlAcc"]=this.driverCtrlAcc;
        data["driverID"]=this.driverID;
        data["driverSubAccID"]=this.driverSubAccID;
        return data; 
    }
}

export class CreateOrEditOESALEHeaderDto implements ICreateOrEditOESALEHeaderDto {
    locID!: number | undefined;
        docNo!: number | undefined;
        docDate!: moment.Moment | undefined;
        paymentDate!: moment.Moment | undefined;
        refID!: string | undefined; 
        typeID!: string | undefined; 
        custID!: number | undefined;
        priceList!: string | undefined; 
        narration!: string | undefined;
        ogp!: string | undefined;
        basicStyle!: string | undefined;
        license!: string | undefined;
        delvTerms!: string | undefined;
        totalQty!: number | undefined;
        amount!: number | undefined;
        exlTaxAmount !: number | undefined;
        tax!: number | undefined;
        addTax!: number | undefined;
        disc!: number | undefined; 
        tradeDisc!: number | undefined; 
        margin!: number | undefined; 
        freight!: number | undefined; 
        ordNo!: number | undefined;
        totAmt!: number | undefined;
        posted!: boolean;
        linkDetID!: number | undefined;
        active!:boolean;
        approved!:boolean;
        approvedBy!:string | undefined;
        approvedDate!:moment.Moment | undefined;
        audtUser!: string | undefined;
        audtDate!: moment.Moment | undefined;
        createdBy!: string | undefined;
        createDate!: moment.Moment | undefined;
        id!: number | undefined;
        qutationDoc:string|undefined;
        driverName!: string | undefined;
        vehicleNo!: string | undefined;
        routDesc!: string | undefined;
        vehicleType!: number | undefined;
        routID!: number | undefined;
        driverCtrlAcc!: string | undefined;
        driverID !: number | undefined;
        driverSubAccID !: number | undefined;
        
        

    constructor(data?: ICreateOrEditOESALEHeaderDto) {
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
            this.refID = data["refID"];
            this.typeID = data["typeID"];
            this.custID = data["custID"];
            this.priceList = data["priceList"];
            this.narration = data["narration"];
            this.delvTerms = data["delvTerms"];
            this.ogp =data["ogp"];
            this.basicStyle =data["basicStyle"];
            this.license =data["license"];
            this.totalQty =data["totalQty"];
            this.amount = data["amount"];
            this.exlTaxAmount = data["exlTaxAmount"];
            this.tax = data["tax"];
            this.addTax = data["addTax"];
            this.disc = data["disc"];
            this.tradeDisc = data["tradeDisc"];
            this.freight = data["freight"];
            this.margin = data["margin"];
            this.ordNo = data["ordNo"];
            this.totAmt = data["totAmt"];
            this.posted= data["posted"];
            this.linkDetID = data["linkDetID"];
            this.active = data["active"];
            this.approved = data["approved"];
            this.approvedBy = data["approvedBy"];
            this.approvedDate = data["approvedDate"] ? moment(data["approvedDate"].toString()) : <any>undefined;
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.id = data["id"];
            this.qutationDoc=data["qutationDoc"];
            this.driverName = data["driverName"];
            this.vehicleNo = data["vehicleNo"];
            this.vehicleType = data["vehicleType"];
            this.routID = data["routID"];
            this.routDesc = data["routDesc"];
            this.driverCtrlAcc = data["driverCtrlAcc"];
            this.driverID = data["driverID"];
            this.driverSubAccID=data["driverSubAccID"];
            
            
        }
    }

    static fromJS(data: any): CreateOrEditOESALEHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditOESALEHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["locID"]=this.locID;
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["paymentDate"]= this.paymentDate ? this.paymentDate.toISOString() : <any>undefined;;
        data["refID"]= this.refID;
        data["typeID"]= this.typeID;
        data["custID"]= this.custID;
        data["priceList"]= this.priceList;
        data["narration"] = this.narration;
        data["delvTerms"] = this.delvTerms;
        data["ogp"]= this.ogp;
        data["basicStyle"]= this.basicStyle;
        data["license"]= this.license;
        data["totalQty"]= this.totalQty;
        data["amount"]= this.amount ;
        data["exlTaxAmount"]= this.exlTaxAmount ;
        data["tax"]= this.tax; 
        data["addTax"]= this.addTax;
        data["disc"]= this.disc;
        data["tradeDisc"]= this.tradeDisc;
        data["freight"]= this.freight;
        data["margin"]= this.margin;
        data["ordNo"]= this.ordNo;
        data["totAmt"]= this.totAmt;
        data["posted"]=this.posted;
        data["linkDetID"]=this.linkDetID;
        data["active"] = this.active;
        data["approved"] = this.approved;
        data["approvedBy"] = this.approvedBy;
        data["approvedDate"] = this.approvedDate ? this.approvedDate.toISOString() : <any>undefined;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        data["qutationDoc"]=this.qutationDoc;
        data["driverName"] = this.driverName;
        data["vehicleNo"] = this.vehicleNo;
        data["vehicleType"] = this.vehicleType;       
        data["routID"] = this.routID;       
        data["routDesc"] = this.routDesc;  
        data["driverCtrlAcc"]=this.driverCtrlAcc; 
        data["driverID"]=this.driverID;  
        data["driverSubAccID"] = this.driverSubAccID; 
        return data; 
    }
}
