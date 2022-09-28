import * as moment from 'moment';
import { IGLINVHeaderDto, ICreateOrEditGLINVHeaderDto, IPagedResultDtoOfGLINVHeaderDto } from '../interface/glinvHeader-interface';

export class PagedResultDtoOfGLINVHeaderDto implements IPagedResultDtoOfGLINVHeaderDto {
    totalCount!: number | undefined;
    items!: GLINVHeaderDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGLINVHeaderDto) {
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
                    this.items!.push(GLINVHeaderDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGLINVHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGLINVHeaderDto();
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

export class GLINVHeaderDto implements IGLINVHeaderDto {
    docNo!: number;
    typeID!: string;
    docStatus!: string;
    locID!: number | undefined;
    locDesc!: string | undefined;
    bankID!: string;
    accountID!: string;
    configID!: number;
    docDate!: moment.Moment;
    postDate!: moment.Moment;
    narration!: string;
    curID!: string;
    curRate!: number;
    creditLimit!:number | undefined;
    closingBalance!:number | undefined;
    chequeNo!: string;
    refNo!: string;
    payReason!:string | undefined;
    paymentOption!:string | undefined;
    partyInvNo!: string;
    partyInvDate!: moment.Moment;
    taxAuth!: string | undefined;
    taxAuthDesc!: string | undefined;
    taxClass!: number | undefined;
    taxClassDesc!: string | undefined;
    taxRate!: number | undefined;
    taxAccID!: string | undefined;
    taxAccDesc!: string | undefined;
    taxAmount!: number | undefined;
    postedStock!: boolean;
    postedStockBy!: string;
    postedStockDate!: moment.Moment;
    cprID!:number | undefined;
    cprNo!:string | undefined;
    cprDate!:moment.Moment | undefined;
    posted!: boolean;
    postedBy!: string;
    postedDate!: moment.Moment;
    createdBy!: string;
    createDate!: moment.Moment;
    audtUser!: string;
    audtDate!: moment.Moment;
    id!: number;
    chNumber!:string | undefined;
	chType!:number | undefined;
    arClass!:number | undefined;
    arRate!:number | undefined;
    arAccID!: string;
    arAmount!:number | undefined;
    icTaxAuth!: string;
    icTaxClass!:number | undefined;
    icTaxRate!:number | undefined;
    icTaxAccID!: string;
    icTaxAmount!:number | undefined;
    invAmount!:number | undefined;
    constructor(data?: IGLINVHeaderDto) {
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
            this.docNo = data["docNo"];
            this.docStatus = data["docStatus"];
            this.typeID = data["typeID"];
            this.locID = data["locID"];
            this.locDesc = data["locDesc"];
            this.bankID = data["bankID"];
            this.accountID = data["accountID"];
            this.configID = data["configID"];
            this.docDate = data["docDate"] ? moment(data["docDate"].toString()) : <any>undefined;
            this.postDate = data["postDate"] ? moment(data["postDate"].toString()) : <any>undefined;
            this.narration = data["narration"];
            this.curID=data["curID"];
            this.curRate = data["curRate"];
            this.closingBalance = data["closingBalance"];
            this.creditLimit = data["creditLimit"];
            this.chequeNo = data["chequeNo"];
            this.refNo = data["refNo"];
            this.payReason = data["payReason"];
            this.paymentOption = data["paymentOption"];
            this.partyInvNo = data["partyInvNo"];
            this.partyInvDate = data["partyInvDate"] ? moment(data["partyInvDate"].toString()) : <any>undefined;
            this.taxAuth = data["taxAuth"];
            this.taxAuthDesc = data["taxAuthDesc"];
            this.taxClass = data["taxClass"];
            this.taxClassDesc = data["taxClassDesc"];
            this.taxRate = data["taxRate"];
            this.taxAccID = data["taxAccID"];
            this.taxAccDesc = data["taxAccDesc"];
            this.taxAmount = data["taxAmount"];
            this.postedStock = data["postedStock"];
            this.postedStockBy = data["postedStockBy"];
            this.postedStockDate = data["postedStockDate"] ? moment(data["postedStockDate"].toString()) : <any>undefined;
            this.cprID=data["cprID"];
            this.cprNo=data["cprNo"];
            this.cprDate=data["cprDate"];
            this.posted = data["posted"];
            this.postedBy = data["postedBy"];
            this.postedDate = data["postedDate"] ? moment(data["postedDate"].toString()) : <any>undefined;
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.id = data["id"];
            this.chNumber = data["chNumber"];
            this.chType = data["chType"];
            this.arClass = data["arClass"];
            this.arRate = data["arRate"];
            this.arAccID = data["arAccID"];
            this.arAmount = data["arAmount"];
            this.icTaxAuth = data["icTaxAuth"];
            this.icTaxClass = data["icTaxClass"];
            this.icTaxRate = data["icTaxRate"];
            this.icTaxAccID = data["icTaxAccID"];
            this.icTaxAmount = data["icTaxAmount"];
            this.invAmount = data["invAmount"];
        }
    }

    static fromJS(data: any): GLINVHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new GLINVHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {debugger
        debugger;
        data = typeof data === 'object' ? data : {};
        data["docNo"] = this.docNo;
        data["docStatus"] = this.docStatus;
        data["typeID"] = this.typeID;
        data["locID"] = this.locID;
        data["locDesc"] = this.locDesc;
        data["bankID"] = this.bankID;
        data["accountID"] = this.accountID;
        data["configID"] = this.configID;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["postDate"] = this.postDate ? this.postDate.toISOString() : <any>undefined;
        data["narration"] = this.narration;
        data["curID"]=this.curID;
        data["curRate"] = this.curRate;
        data["closingBalance"] = this.closingBalance;
        data["creditLimit"] = this.creditLimit;
        data["chequeNo"] = this.chequeNo;
        data["refNo"] = this.refNo;
        data["payReason"]=this.payReason;
        data["paymentOption"]=this.paymentOption;
        data["partyInvNo"] = this.partyInvNo;
        data["partyInvDate"] = this.partyInvDate ? this.partyInvDate.toISOString() : <any>undefined;
        data["postedStock"] = this.postedStock;
        data["postedStockBy"] = this.postedStockBy;
        data["postedStockDate"] = this.postedStockDate ? this.postedStockDate.toISOString() : <any>undefined;
        data["taxAuth"]= this.taxAuth;
        data["taxAuthDesc"]= this.taxAuthDesc;
        data["taxClass"]= this.taxClass;
        data["taxClassDesc"]= this.taxClassDesc;
        data["taxRate"]= this.taxRate;
        data["taxAccID"]= this.taxAccID;
        data["taxAccDesc"]= this.taxAccDesc;
        data["taxAmount"]= this.taxAmount;
        data["posted"] = this.posted;
        data["postedBy"] = this.postedBy;
        data["postedDate"] = this.postedDate ? this.postedDate.toISOString() : <any>undefined;
        data["cprID"]=this.cprID;
        data["cprNo"]=this.cprNo;
        data["cprDate"]=this.cprDate;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        data["chNumber"] =this.chNumber;
        data["chType"] =  this.chType;
        data["arClass"] = this.arClass;
        data["arRate"] = this.arRate;
        data["arAccID"] = this.arAccID;
        data["arAmount"] = this.arAmount;
        data["icTaxAuth"] = this.icTaxAuth;
        data["icTaxClass"] = this.icTaxClass;
        data["icTaxRate"] = this.icTaxRate;
        data["icTaxAccID"] = this.icTaxAccID;
        data["icTaxAmount"] = this.icTaxAmount;
        data["invAmount"] = this.invAmount;
        return data; 
    }
}

export class CreateOrEditGLINVHeaderDto implements ICreateOrEditGLINVHeaderDto {
    accountID: string;
    docNo!: number;
    docStatus!: string;
    typeID!: string;
    locID!: number | undefined;
    bankID!: string;
    configID!: number;
    docDate!: moment.Moment;
    postDate!: moment.Moment;
    narration!: string;
    curID!: string;
    curRate!: number;
    creditLimit!:number | undefined;
    closingBalance!:number | undefined;
    chequeNo!: string;
    refNo!: string;
    payReason!:string | undefined;
    paymentOption!:string | undefined;
    partyInvNo!: string;
    partyInvDate!: moment.Moment;
    taxAuth!: string | undefined;
    taxClass!: number | undefined;
    taxRate!: number | undefined;
    taxAccID!: string | undefined;
    taxAmount!: number | undefined;
    postedStock!: boolean;
    postedStockBy!: string;
    postedStockDate!: moment.Moment;
    cprID!:number | undefined;
    cprNo!:string | undefined;
    cprDate!:moment.Moment | undefined;
    posted!: boolean;
    postedBy!: string;
    postedDate!: moment.Moment;
    createdBy!: string;
    createDate!: moment.Moment;
    audtUser!: string;
    audtDate!: moment.Moment;
    id!: number;
    chNumber!:string | undefined;
    chType!:number | undefined;
    arClass!:number | undefined;
    arRate!:number | undefined;
    arAccID!: string;
    arAmount!:number | undefined;
    icTaxAuth!: string;
    icTaxClass!:number | undefined;
    icTaxRate!:number | undefined;
    icTaxAccID!: string;
    icTaxAmount!:number | undefined;
    invAmount!:number | undefined;
    constructor(data?: ICreateOrEditGLINVHeaderDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.docNo = data["docNo"];
            this.typeID = data["typeID"];
            this.docStatus = data["docStatus"];
            this.locID = data["locID"];
            this.bankID = data["bankID"];
            this.accountID = data["accountID"];
            this.configID = data["configID"];
            this.docDate = data["docDate"] ? moment(data["docDate"].toString()) : <any>undefined;
            this.postDate = data["postDate"] ? moment(data["postDate"].toString()) : <any>undefined;
            this.narration = data["narration"]; 
            this.curID = data["curID"];
            this.curRate = data["curRate"];
            this.closingBalance = data["closingBalance"];
            this.creditLimit = data["creditLimit"];
            this.chequeNo = data["chequeNo"];
            this.refNo = data["refNo"];
            this.payReason = data["payReason"];
            this.paymentOption = data["paymentOption"];
            this.partyInvNo = data["partyInvNo"];
            this.partyInvDate = data["partyInvDate"] ? moment(data["partyInvDate"].toString()) : <any>undefined;
            this.taxAuth = data["taxAuth"];
            this.taxClass = data["taxClass"];
            this.taxRate = data["taxRate"];
            this.taxAccID = data["taxAccID"];
            this.taxAmount = data["taxAmount"];
            this.postedStock = data["postedStock"];
            this.postedStockBy = data["postedStockBy"];
            this.postedStockDate = data["postedStockDate"] ? moment(data["postedStockDate"].toString()) : <any>undefined;
            this.cprID=data["cprID"];
            this.cprNo=data["cprNo"];
            this.cprDate=data["cprDate"];
            this.posted = data["posted"];
            this.postedBy = data["postedBy"];
            this.postedDate = data["postedDate"] ? moment(data["postedDate"].toString()) : <any>undefined;
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.id = data["id"];
            this.chNumber = data["chNumber"];
            this.chType = data["chType"];
            this.arClass = data["arClass"];
            this.arRate = data["arRate"];
            this.arAccID = data["arAccID"];
            this.arAmount = data["arAmount"];
            this.icTaxAuth = data["icTaxAuth"];
            this.icTaxClass = data["icTaxClass"];
            this.icTaxRate = data["icTaxRate"];
            this.icTaxAccID = data["icTaxAccID"];
            this.icTaxAmount = data["icTaxAmount"];
            this.invAmount = data["invAmount"];
        }
    }

    static fromJS(data: any): CreateOrEditGLINVHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditGLINVHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["docNo"] = this.docNo;
        data["typeID"] = this.typeID;
        data["docStatus"] = this.docStatus;
        data["locID"] = this.locID;
        data["bankID"] = this.bankID;
        data["accountID"] = this.accountID;
        data["configID"] = this.configID;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["postDate"] = this.postDate ? this.postDate.toISOString() : <any>undefined;
        data["narration"] = this.narration;
        data["curID"]=this.curID;
        data["curRate"] = this.curRate;
        data["closingBalance"] = this.closingBalance;
        data["creditLimit"] = this.creditLimit;
        data["chequeNo"] = this.chequeNo;
        data["refNo"] = this.refNo;
        data["payReason"]=this.payReason;
        data["paymentOption"]=this.paymentOption;
        data["partyInvNo"] = this.partyInvNo;
        data["partyInvDate"] = this.partyInvDate ? this.partyInvDate.toISOString() : <any>undefined;
        data["taxAuth"]= this.taxAuth;
        data["taxClass"]= this.taxClass;
        data["taxRate"]= this.taxRate;
        data["taxAccID"]= this.taxAccID;
        data["taxAmount"]= this.taxAmount;
        data["postedStock"] = this.postedStock;
        data["postedStockBy"] = this.postedStockBy;
        data["postedStockDate"] = this.postedStockDate ? this.postedStockDate.toISOString() : <any>undefined;
        data["cprID"]=this.cprID;
        data["cprNo"]=this.cprNo;
        data["cprDate"]=this.cprDate;
        data["posted"] = this.posted;
        data["postedBy"] = this.postedBy;
        data["postedDate"] = this.postedDate ? this.postedDate.toISOString() : <any>undefined;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        data["chNumber"] = this.chNumber;
        data["chType"] = this.chType;
        data["arClass"] = this.arClass;
        data["arRate"] = this.arRate;
        data["arAccID"] = this.arAccID;
        data["arAmount"] = this.arAmount;
        data["icTaxAuth"] = this.icTaxAuth;
        data["icTaxClass"] = this.icTaxClass;
        data["icTaxRate"] = this.icTaxRate;
        data["icTaxAccID"] = this.icTaxAccID;
        data["icTaxAmount"] = this.icTaxAmount;
        data["invAmount"] = this.invAmount;
        return data; 
    }
}
