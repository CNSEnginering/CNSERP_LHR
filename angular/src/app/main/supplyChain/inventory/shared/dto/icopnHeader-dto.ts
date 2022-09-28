import * as moment from 'moment';
import { IPagedResultDtoOfICOPNHeaderDto, IICOPNHeaderDto, ICreateOrEditICOPNHeaderDto } from '../interface/icopnHeader-interface';

export class PagedResultDtoOfICOPNHeaderDto implements IPagedResultDtoOfICOPNHeaderDto {
    totalCount!: number | undefined;
    items!: ICOPNHeaderDto[] | undefined;

    constructor(data?: IPagedResultDtoOfICOPNHeaderDto) {
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
                    this.items!.push(ICOPNHeaderDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfICOPNHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfICOPNHeaderDto();
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

export class ICOPNHeaderDto implements IICOPNHeaderDto {
    docNo!:number | undefined;
    docDate!: moment.Moment | undefined;
    narration!: string | undefined;
    locID!:number | undefined;
    totalItems!:number | undefined;
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
    

    constructor(data?: IICOPNHeaderDto) {
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
            this.narration = data["narration"];
            this.locID=data["locID"];
            this.totalItems = data["totalItems"];
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

    static fromJS(data: any): ICOPNHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new ICOPNHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {debugger
        debugger;
        data = typeof data === 'object' ? data : {};
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["narration"] = this.narration;
        data["locID"]=this.locID;
        data["totalItems"] = this.totalItems;
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

export class CreateOrEditICOPNHeaderDto implements ICreateOrEditICOPNHeaderDto {
    docNo!:number | undefined;
    docDate!: moment.Moment | undefined;
    narration!: string | undefined;
    locID!:number | undefined;
    totalItems!:number | undefined;
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

    constructor(data?: ICreateOrEditICOPNHeaderDto) {
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
            this.docDate = data["docDate"] ? moment(data["docDate"].toString()) : <any>undefined;
            this.narration = data["narration"];
            this.locID=data["locID"];
            this.totalItems = data["totalItems"];
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

    static fromJS(data: any): CreateOrEditICOPNHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditICOPNHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["narration"] = this.narration;
        data["locID"]=this.locID;
        data["totalItems"] = this.totalItems;
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
