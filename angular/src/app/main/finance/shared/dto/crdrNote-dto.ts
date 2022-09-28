import * as moment from 'moment';
import {  ICRDRNoteDto, IPagedResultDtoOfGetCRDRNoteForViewDto, IGetCRDRNoteForViewDto, IGetCRDRNoteForEditOutput, ICreateOrEditCRDRNoteDto } from '../interface/crdrNote-interface';
export class PagedResultDtoOfGetCRDRNoteForViewDto implements IPagedResultDtoOfGetCRDRNoteForViewDto {
    totalCount!: number | undefined;
    items!: GetCRDRNoteForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetCRDRNoteForViewDto) {
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
                    this.items!.push(GetCRDRNoteForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetCRDRNoteForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetCRDRNoteForViewDto();
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

export class GetCRDRNoteForViewDto implements IGetCRDRNoteForViewDto {
    crdrNote!: CRDRNoteDto | undefined;

    constructor(data?: IGetCRDRNoteForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
       
        if (data) {
            this.crdrNote = data["crdrNote"] ? CRDRNoteDto.fromJS(data["crdrNote"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetCRDRNoteForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetCRDRNoteForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["crdrNote"] = this.crdrNote ? this.crdrNote.toJSON() : <any>undefined;
        return data;
    }
}

export class CRDRNoteDto implements ICRDRNoteDto {
    locID!: number | undefined;
    docNo!: number | undefined;
    docDate!: moment.Moment | undefined;
    postingDate!: moment.Moment | undefined;
    paymentDate!: moment.Moment | undefined;
    typeID!: number | undefined;
    transType!: string | undefined;
    accountID!: string | undefined;
    subAccID!: number | undefined;
    invoiceNo!: number | undefined;
    partyInvNo!: string | undefined;
    partyInvDate!: moment.Moment | undefined;    
    partyInvAmount!: number | undefined;
    amount!: number | undefined;
    reason!: string | undefined;
    stkAccID!: string | undefined;
    posted!: boolean | undefined;
    linkDetID!: number | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    partyName!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICRDRNoteDto) {
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
            this.locID = data["locID"];
            this.docNo = data["docNo"];
            this.docDate = data["docDate"];
            this.postingDate = data["postingDate"];
            this.paymentDate = data["paymentDate"];
            this.typeID = data["typeID"];
            this.transType = data["transType"];
            this.accountID = data["accountID"];
            this.subAccID = data["subAccID"];
            this.invoiceNo = data["invoiceNo"];
            this.partyInvNo = data["partyInvNo"];
            this.partyInvDate = data["partyInvDate"];
            this.partyInvAmount = data["partyInvAmount"];
            this.amount = data["amount"];
            this.reason = data["reason"];
            this.stkAccID = data["stkAccID"];
            this.posted = data["posted"];
            this.linkDetID = data["linkDetID"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
            this.partyName = data["partyName"];
        }
    }

    static fromJS(data: any): CRDRNoteDto {
        debugger
        data = typeof data === 'object' ? data : {};
        let result = new CRDRNoteDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["locID"] = this.locID;
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate;
        data["postingDate"] = this.postingDate;
        data["paymentDate"] = this.paymentDate;
        data["typeID"] = this.typeID;
        data["transType"] = this.transType;
        data["accountID"] = this.accountID;
        data["subAccID"] = this.subAccID;
        data["invoiceNo"] = this.invoiceNo;
        data["partyInvNo"] = this.partyInvNo;
        data["partyInvDate"] = this.partyInvDate;
        data["partyInvAmount"] = this.partyInvAmount;
        data["amount"] = this.amount;
        data["reason"] = this.reason;
        data["stkAccID"] = this.stkAccID;
        data["posted"] = this.posted;
        data["linkDetID"] = this.linkDetID;
        data["active"]=this.active;
        data["audtUser"]=this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"]=this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        data["partyName"] = this.partyName;
        return data;
    }
}

export class GetCRDRNoteForEditOutput implements IGetCRDRNoteForEditOutput {
    crdrNote!: CreateOrEditCRDRNoteDto | undefined;

    constructor(data?: IGetCRDRNoteForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.crdrNote = data["crdrNote"] ? CreateOrEditCRDRNoteDto.fromJS(data["crdrNote"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetCRDRNoteForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetCRDRNoteForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["crdrNote"] = this.crdrNote ? this.crdrNote.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditCRDRNoteDto implements ICreateOrEditCRDRNoteDto {
    locID!: number | undefined;
    docNo!: number | undefined;
    docDate!: moment.Moment | undefined;
    postingDate!: moment.Moment | undefined;
    paymentDate!: moment.Moment | undefined;
    typeID!: number | undefined;
    transType!: string | undefined;
    accountID!: string | undefined;
    subAccID!: number | undefined;
    invoiceNo!: number | undefined;
    partyInvNo!: string | undefined;
    partyInvDate!: moment.Moment | undefined;    
    partyInvAmount!: number | undefined;
    amount!: number | undefined;
    reason!: string | undefined;
    stkAccID!: string | undefined;
    posted!: boolean | undefined;
    linkDetID!: number | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;
    partyName!: string | undefined;


    constructor(data?: ICreateOrEditCRDRNoteDto) {
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
            this.locID = data["locID"];
            this.docNo = data["docNo"];
            this.docDate = data["docDate"];
            this.postingDate = data["postingDate"];
            this.paymentDate = data["paymentDate"];
            this.typeID = data["typeID"];
            this.transType = data["transType"];
            this.accountID = data["accountID"];
            this.subAccID = data["subAccID"];
            this.invoiceNo = data["invoiceNo"];
            this.partyInvNo = data["partyInvNo"];
            this.partyInvDate = data["partyInvDate"];
            this.partyInvAmount = data["partyInvAmount"];
            this.amount = data["amount"];
            this.reason = data["reason"];
            this.stkAccID = data["stkAccID"];
            this.posted = data["posted"];
            this.linkDetID = data["linkDetID"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
            this.partyName = data["partyName"];
        }
    }

    static fromJS(data: any): CreateOrEditCRDRNoteDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditCRDRNoteDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["locID"] = this.locID;
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate;
        data["postingDate"] = this.postingDate;
        data["paymentDate"] = this.paymentDate;
        data["typeID"] = this.typeID;
        data["transType"] = this.transType;
        data["accountID"] = this.accountID;
        data["subAccID"] = this.subAccID;
        data["invoiceNo"] = this.invoiceNo;
        data["partyInvNo"] = this.partyInvNo;
        data["partyInvDate"] = this.partyInvDate;
        data["partyInvAmount"] = this.partyInvAmount;
        data["amount"] = this.amount;
        data["reason"] = this.reason;
        data["stkAccID"] = this.stkAccID;
        data["posted"] = this.posted;
        data["linkDetID"] = this.linkDetID;
        data["active"]=this.active;
        data["audtUser"]=this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"]=this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        data["partyName"] = this.partyName;
        return data;
    }
}