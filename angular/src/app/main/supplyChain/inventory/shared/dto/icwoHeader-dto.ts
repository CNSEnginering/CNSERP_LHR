import * as moment from 'moment';
import { IPagedResultDtoOfICWOHeaderDto, IICWOHeaderDto, ICreateOrEditICWOHeaderDto } from '../interface/icwoHeader-interface';

export class PagedResultDtoOfICWOHeaderDto implements IPagedResultDtoOfICWOHeaderDto {
    totalCount!: number | undefined;
    items!: ICWOHeaderDto[] | undefined;

    constructor(data?: IPagedResultDtoOfICWOHeaderDto) {
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
                    this.items!.push(ICWOHeaderDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfICWOHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfICWOHeaderDto();
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

export class ICWOHeaderDto implements IICWOHeaderDto {
    docNo!:number | undefined;
    docDate!: moment.Moment | undefined;
    narration!: string | undefined;
    ccid!:string | undefined;
    ccDesc!:string | undefined;
    locID!:number | undefined;
    totalQty!:number | undefined;
    totalAmt!:number | undefined;
    approved!:boolean;
    approvedBy!:string | undefined;
    approvedDate!:moment.Moment | undefined;
    refrence!:string | undefined;
    active!:boolean;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    id!: number | undefined;
    posted!: boolean;
    qutationDoc:string|undefined;

    constructor(data?: IICWOHeaderDto) {
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
            this.ccid=data["ccid"];
            this.ccDesc=data["ccDesc"];
            this.locID=data["locID"];
            this.totalQty = data["totalQty"];
            this.totalAmt = data["totalAmt"];
            this.approved = data["approved"];
            this.approvedBy = data["approvedBy"];
            this.approvedDate = data["approvedDate"] ? moment(data["approvedDate"].toString()) : <any>undefined;
            this.refrence = data["refrence"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.id = data["id"];
            this.posted = data["posted"];
            this.qutationDoc=data["qutationDoc"];
        }
    }

    static fromJS(data: any): ICWOHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new ICWOHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {debugger
        debugger;
        data = typeof data === 'object' ? data : {};
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["narration"] = this.narration;
        data["ccid"]=this.ccid;
        data["ccDesc"]=this.ccDesc;
        data["locID"]=this.locID;
        data["totalQty"] = this.totalQty;
        data["totalAmt"] = this.totalAmt;
        data["approved"] = this.approved;
        data["approvedBy"] = this.approvedBy;
        data["approvedDate"] = this.approvedDate ? this.approvedDate.toISOString() : <any>undefined;
        data["refrence"] = this.refrence;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        data["poisted"] = this.posted;
        data["qutationDoc"]=this.qutationDoc;
        return data; 
    }
}

export class CreateOrEditICWOHeaderDto implements ICreateOrEditICWOHeaderDto {
    docNo!:number | undefined;
    docDate!: moment.Moment | undefined;
    narration!: string | undefined;
    ccid!:string | undefined;
    locID!:number | undefined;
    totalQty!:number | undefined;
    totalAmt!:number | undefined;
    approved!:boolean;
    approvedBy!:string | undefined;
    approvedDate!:moment.Moment | undefined;
    refrence!:string | undefined;
    active!:boolean;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    id!: number | undefined;
    posted!: boolean;
    qutationDoc:string|undefined;
    constructor(data?: ICreateOrEditICWOHeaderDto) {
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
            this.narration = data["narration"];
            this.ccid=data["ccid"];
            this.locID=data["locID"];
            this.totalQty = data["totalQty"];
            this.totalAmt = data["totalAmt"];
            this.approved = data["approved"];
            this.approvedBy = data["approvedBy"];
            this.approvedDate = data["approvedDate"] ? moment(data["approvedDate"].toString()) : <any>undefined;
            this.refrence = data["refrence"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.id = data["id"];
            this.posted = data["posted"];
            this.qutationDoc=data["qutationDoc"];
        }
    }

    static fromJS(data: any): CreateOrEditICWOHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditICWOHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["narration"] = this.narration;
        data["ccid"]=this.ccid;
        data["locID"]=this.locID;
        data["totalQty"] = this.totalQty;
        data["totalAmt"] = this.totalAmt;
        data["approved"] = this.approved;
        data["approvedBy"] = this.approvedBy;
        data["approvedDate"] = this.approvedDate ? this.approvedDate.toISOString() : <any>undefined;
        data["refrence"] = this.refrence;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        data["posted"] = this.posted;
        data["qutationDoc"]=this.qutationDoc;
        return data; 
    }
}
