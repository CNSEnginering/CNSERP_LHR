import * as moment from 'moment';
import { ICreateOrEditICCNSHeaderDto, IICCNSHeaderDto, IPagedResultDtoOfICCNSHeaderDto } from '../interface/ICCNSHeader-interface';

export class PagedResultDtoOfICCNSHeaderDto implements IPagedResultDtoOfICCNSHeaderDto {
    totalCount!: number | undefined;
    items!: ICCNSHeaderDto[] | undefined;

    constructor(data?: IPagedResultDtoOfICCNSHeaderDto) {
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
            this.totalCount = data["totalCount"]; 
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [] as any;
                for (let item of data["items"])
                    this.items!.push(ICCNSHeaderDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfICCNSHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfICCNSHeaderDto();
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

export class ICCNSHeaderDto implements IICCNSHeaderDto {
    docNo!:number | undefined;
    docDate!: moment.Moment | undefined;
    type!:number | undefined;
    narration!: string | undefined;
    ccid!:string | undefined;
    ccDesc!:string | undefined;
    locID!:number | undefined;
    totalQty!:number | undefined;
    totalAmt!:number | undefined;
    posted!:boolean;
    postedBy!:string | undefined;
    postedDate!:moment.Moment | undefined;
    approved!:boolean;
    approvedBy!:string | undefined;
    approvedDate!:moment.Moment | undefined;
    linkDetID!:number | undefined;
    ordNo!:string | undefined;
    active!:boolean;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    id!: number | undefined;
    

    constructor(data?: IICCNSHeaderDto) {
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
            this.docDate = data["docDate"] ? moment(data["docDate"].toString()) : <any>undefined;
            this.type = data["type"];
            this.narration = data["narration"];
            this.ccid=data["ccid"];
            this.ccDesc=data["ccDesc"];
            this.locID=data["locID"];
            this.totalQty = data["totalQty"];
            this.totalAmt = data["totalAmt"];
            this.posted = data["posted"];
            this.postedBy = data["postedBy"];
            this.postedDate = data["postedDate"] ? moment(data["postedDate"].toString()) : <any>undefined;
            this.approved = data["approved"];
            this.approvedBy = data["approvedBy"];
            this.approvedDate = data["approvedDate"] ? moment(data["approvedDate"].toString()) : <any>undefined;
            this.linkDetID = data["linkDetID"];
            this.ordNo = data["ordNo"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.id = data["id"];
        }
    }

    static fromJS(data: any): ICCNSHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new ICCNSHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {debugger
        debugger;
        data = typeof data === 'object' ? data : {};
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["type"] = this.type;
        data["narration"] = this.narration;
        data["ccid"]=this.ccid;
        data["ccDesc"]=this.ccDesc;
        data["locID"]=this.locID;
        data["totalQty"] = this.totalQty;
        data["totalAmt"] = this.totalAmt;
        data["posted"] = this.posted;
        data["postedBy"] = this.postedBy;
        data["postedDate"] = this.postedDate ? this.postedDate.toISOString() : <any>undefined;
        data["approved"] = this.approved;
        data["approvedBy"] = this.approvedBy;
        data["approvedDate"] = this.approvedDate ? this.approvedDate.toISOString() : <any>undefined;
        data["linkDetID"] = this.linkDetID;
        data["ordNo"] = this.ordNo;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        return data; 
    }
}

export class CreateOrEditICCNSHeaderDto implements ICreateOrEditICCNSHeaderDto {
    docNo!:number | undefined;
    docDate!: moment.Moment | undefined;
    type!:number | undefined;
    narration!: string | undefined;
    ccid!:string | undefined;
    locID!:number | undefined;
    totalQty!:number | undefined;
    totalAmt!:number | undefined;
    posted!:boolean;
    postedBy!:string | undefined;
    postedDate!:moment.Moment | undefined;
    approved!:boolean;
    approvedBy!:string | undefined;
    approvedDate!:moment.Moment | undefined;
    linkDetID!:number | undefined;
    ordNo!:string | undefined;
    active!:boolean;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditICCNSHeaderDto) {
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
            this.docNo = data["docNo"];
            this.docDate = data["docDate"] ? moment(data["docDate"].toString()) : <any>undefined;
            this.type = data["type"];
            this.narration = data["narration"];
            this.ccid=data["ccid"];
            this.locID=data["locID"];
            this.totalQty = data["totalQty"];
            this.totalAmt = data["totalAmt"];
            this.posted = data["posted"];
            this.postedBy = data["postedBy"];
            this.postedDate = data["postedDate"] ? moment(data["postedDate"].toString()) : <any>undefined;
            this.approved = data["approved"];
            this.approvedBy = data["approvedBy"];
            this.approvedDate = data["approvedDate"] ? moment(data["approvedDate"].toString()) : <any>undefined;
            this.linkDetID = data["linkDetID"];
            this.ordNo = data["ordNo"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditICCNSHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditICCNSHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["type"] = this.type;
        data["narration"] = this.narration;
        data["ccid"]=this.ccid;
        data["locID"]=this.locID;
        data["totalQty"] = this.totalQty;
        data["totalAmt"] = this.totalAmt;
        data["posted"] = this.posted;
        data["postedBy"] = this.postedBy;
        data["postedDate"] = this.postedDate ? this.postedDate.toISOString() : <any>undefined;
        data["approved"] = this.approved;
        data["approvedBy"] = this.approvedBy;
        data["approvedDate"] = this.approvedDate ? this.approvedDate.toISOString() : <any>undefined;
        data["linkDetID"] = this.linkDetID;
        data["ordNo"] = this.ordNo;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        return data; 
    }
}
