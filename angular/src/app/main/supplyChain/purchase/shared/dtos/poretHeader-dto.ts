import * as moment from 'moment';
import { IPagedResultDtoOfPORETHeaderDto, IPORETHeaderDto, ICreateOrEditPORETHeaderDto } from '../interfaces/poretHeader-interface';

export class PagedResultDtoOfPORETHeaderDto implements IPagedResultDtoOfPORETHeaderDto {
    totalCount!: number | undefined;
    items!: PORETHeaderDto[] | undefined;

    constructor(data?: IPagedResultDtoOfPORETHeaderDto) {
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
                    this.items!.push(PORETHeaderDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfPORETHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfPORETHeaderDto();
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

export class PORETHeaderDto implements IPORETHeaderDto {
    locID!: number | undefined;
    docNo!: number | undefined;
    docDate!: moment.Moment | undefined;
    accountID!: string | undefined; 
    accDesc!: string | undefined; 
    subAccID!: number | undefined;
    subAccDesc!: string | undefined; 
    narration!: string | undefined;
    igpNo!: string | undefined;
    billNo!: string | undefined;
    billDate!: moment.Moment | undefined;
    billAmt!: number | undefined;
    totalQty!: number | undefined;
    totalAmt!:number | undefined;
    posted!: boolean;
    approved!:boolean;
    approvedBy!:string | undefined;
    approvedDate!:moment.Moment | undefined;
    linkDetID!: number | undefined;
    recDocNo!: number | undefined;
    poDocNo!: number | undefined;
    ordNo!: string | undefined;
    freight!: number | undefined;
    addExp!: number | undefined;
    ccid!: string | undefined;
    ccDesc!: string | undefined; 
    addDisc!: number | undefined;
    addLeak!: number | undefined;
    addFreight!: number | undefined;
    onHold!: boolean;
    active!: boolean;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;
    

    constructor(data?: IPORETHeaderDto) {
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
            this.docDate = data["docDate"] ? moment(data["docDate"].toString()) : <any>undefined;;
            this.accountID = data["accountID"];
            this.accDesc = data["accDesc"];
            this.subAccID = data["subAccID"];
            this.subAccDesc = data["subAccDesc"];
            this.narration = data["narration"];
            this.igpNo =data["igpNo"]
            this.billNo =data["billNo"]
            this.billDate = data["billDate"] ? moment(data["billDate"].toString()) : <any>undefined;
            this.billAmt = data["billAmt"];
            this.totalQty = data["totalQty"];
            this.totalAmt = data["totalAmt"];
            this.posted= data["posted"];
            this.approved = data["approved"];
            this.approvedBy = data["approvedBy"];
            this.approvedDate = data["approvedDate"] ? moment(data["approvedDate"].toString()) : <any>undefined;
            this.linkDetID = data["linkDetID"];
            this.recDocNo = data["recDocNo"];
            this.ordNo = data["ordNo"];
            this.recDocNo = data["recDocNo"];
            this.poDocNo = data["poDocNo"];
            this.freight = data["freight"];
            this.addExp = data["addExp"];
            this.ccid = data["ccid"];
            this.ccDesc = data["ccDesc"];
            this.addDisc = data["addDisc"];
            this.addLeak = data["addLeak"];
            this.addFreight = data["addFreight"];
            this.onHold = data["onHold"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.id = data["id"];
        }
    }

    static fromJS(data: any): PORETHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new PORETHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {debugger
        debugger;
        data = typeof data === 'object' ? data : {};
        data["locID"]=this.locID;
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["accountID"]= this.accountID;
        data["accDesc"]= this.accDesc;
        data["subAccID"]= this.subAccID;
        data["subAccDesc"]= this.subAccDesc;
        data["narration"] = this.narration;
        data["igpNo"]= this.igpNo;
        data["billNo"]= this.billNo;
        data["billDate"]= this.billDate ? this.billDate.toISOString() : <any>undefined;
        data["billAmt"]= this.billAmt; 
        data["totalQty"]= this.totalQty;
        data["totalAmt"]= this.totalAmt;
        data["posted"]=this.posted;
        data["approved"] = this.approved;
        data["approvedBy"] = this.approvedBy;
        data["approvedDate"] = this.approvedDate ? this.approvedDate.toISOString() : <any>undefined;
        data["linkDetID"]=this.linkDetID;
        data["recDocNo"]=this.recDocNo;
        data["ordNo"]= this.ordNo;
        data["recDocNo"]= this.recDocNo;
        data["poDocNo"]= this.poDocNo;
        data["freight"]= this.freight;
        data["addExp"]= this.addExp;
        data["ccid"]= this.ccid;
        data["ccDesc"]= this.ccDesc;
        data["addDisc"]= this.addDisc;
        data["addLeak"]= this.addLeak;
        data["addFreight"]= this.addFreight;
        data["onHold"]= this.onHold;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        return data; 
    }
}

export class CreateOrEditPORETHeaderDto implements ICreateOrEditPORETHeaderDto {
    locID!: number | undefined;
    docNo!: number | undefined;
    docDate!: moment.Moment | undefined;
    accountID!: string | undefined; 
    subAccID!: number | undefined;
    narration!: string | undefined;
    igpNo!: string | undefined;
    billNo!: string | undefined;
    billDate!: moment.Moment | undefined;
    billAmt!: number | undefined;
    totalQty!: number | undefined;
    totalAmt!:number | undefined;
    posted!: boolean;
    approved!:boolean;
    approvedBy!:string | undefined;
    approvedDate!:moment.Moment | undefined;
    linkDetID!: number | undefined;
    recDocNo!: number | undefined;
    ordNo!: string | undefined;
    freight!: number | undefined;
    addExp!: number | undefined;
    ccid!: string | undefined;
    addDisc!: number | undefined;
    addLeak!: number | undefined;
    addFreight!: number | undefined;
    onHold!: boolean;
    active!: boolean; 
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditPORETHeaderDto) {
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
            this.docDate = data["docDate"] ? moment(data["docDate"].toString()) : <any>undefined;;
            this.accountID = data["accountID"];
            this.subAccID = data["subAccID"];
            this.narration = data["narration"];
            this.igpNo =data["igpNo"]
            this.billNo =data["billNo"]
            this.billDate = data["billDate"] ? moment(data["billDate"].toString()) : <any>undefined;
            this.billAmt = data["billAmt"];
            this.totalQty = data["totalQty"];
            this.totalAmt = data["totalAmt"];
            this.posted= data["posted"];
            this.approved = data["approved"];
            this.approvedBy = data["approvedBy"];
            this.approvedDate = data["approvedDate"] ? moment(data["approvedDate"].toString()) : <any>undefined;
            this.linkDetID = data["linkDetID"];
            this.recDocNo = data["recDocNo"];
            this.ordNo = data["ordNo"];
            this.recDocNo = data["recDocNo"];
            this.freight = data["freight"];
            this.addExp = data["addExp"];
            this.ccid = data["ccid"];
            this.addDisc = data["addDisc"];
            this.addLeak = data["addLeak"];
            this.addFreight = data["addFreight"];
            this.onHold = data["onHold"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditPORETHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditPORETHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["locID"]=this.locID;
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["accountID"]= this.accountID;
        data["subAccID"]= this.subAccID;
        data["narration"] = this.narration;
        data["igpNo"]= this.igpNo;
        data["billNo"]= this.billNo;
        data["billDate"]= this.billDate ? this.billDate.toISOString() : <any>undefined;
        data["billAmt"]= this.billAmt; 
        data["totalQty"]= this.totalQty;
        data["totalAmt"]= this.totalAmt;
        data["posted"]=this.posted;
        data["approved"] = this.approved;
        data["approvedBy"] = this.approvedBy;
        data["approvedDate"] = this.approvedDate ? this.approvedDate.toISOString() : <any>undefined;
        data["linkDetID"]=this.linkDetID;
        data["recDocNo"]=this.recDocNo;
        data["ordNo"]= this.ordNo;
        data["recDocNo"]= this.recDocNo;
        data["freight"]= this.freight;
        data["addExp"]= this.addExp;
        data["ccid"]= this.ccid;
        data["addDisc"]= this.addDisc;
        data["addLeak"]= this.addLeak;
        data["addFreight"]= this.addFreight;
        data["onHold"]= this.onHold;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        return data; 
    }
}
