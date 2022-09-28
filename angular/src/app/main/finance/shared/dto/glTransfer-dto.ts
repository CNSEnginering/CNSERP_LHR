import * as moment from 'moment';
import {  IGLTransferDto, IPagedResultDtoOfGetGLTransferForViewDto, IGetGLTransferForViewDto, IGetGLTransferForEditOutput, ICreateOrEditGLTransferDto } from '../interface/glTransfer-interface';
export class PagedResultDtoOfGetGLTransferForViewDto implements IPagedResultDtoOfGetGLTransferForViewDto {
    totalCount!: number | undefined;
    items!: GetGLTransferForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetGLTransferForViewDto) {
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
                    this.items!.push(GetGLTransferForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetGLTransferForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetGLTransferForViewDto();
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

export class GetGLTransferForViewDto implements IGetGLTransferForViewDto {
    glTransfer!: GLTransferDto | undefined;

    constructor(data?: IGetGLTransferForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
       
        if (data) {
            this.glTransfer = data["glTransfer"] ? GLTransferDto.fromJS(data["glTransfer"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetGLTransferForViewDto { 
        data = typeof data === 'object' ? data : {};
        let result = new GetGLTransferForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["glTransfer"] = this.glTransfer ? this.glTransfer.toJSON() : <any>undefined;
        return data;
    }
}

export class GLTransferDto implements IGLTransferDto {
    docid!: number | undefined;
    docdate!: moment.Moment | undefined;
    transferdate!: moment.Moment | undefined;
    description!: string | undefined;
    fromlocid!: number | undefined;
    frombankid!: string | undefined;
    fromconfigid!: number | undefined;
    frombankaccid!: string | undefined;
    fromaccid!: string | undefined;
    tolocid!: number | undefined;
    tobankid!: string | undefined;
    toconfigid!: number | undefined;
    tobankaccid!: string | undefined;
    toaccid!: string | undefined;
    status!: boolean | undefined;
    transferamount!: number | undefined;
    gllinkidfrom!: number | undefined;
    gllinkidto!: number | undefined;
    gldocidfrom!: number | undefined;
    gldocidto!: number | undefined;
    audtuser!: string | undefined;
    audtdate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createdOn!: moment.Moment | undefined;
    id!: number | undefined;
    linkDetIDBP!: number | undefined;
    linkDetIDBR!: number | undefined;
    linkDetIDCP!: number | undefined;
    linkDetIDCR!: number | undefined;
    posted!: boolean | undefined;
    chNumber!: string | undefined;
    chType!: number | undefined;
    
    constructor(data?: IGLTransferDto) {
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
            this.docid = data["docid"];
            this.docdate = data["docdate"];
            this.transferdate = data["transferdate"];
            this.description = data["description"];
            this.fromlocid = data["fromlocid"];
            this.frombankid = data["frombankid"];
            this.fromconfigid = data["fromconfigid"];
            this.frombankaccid = data["frombankaccid"];
            this.fromaccid = data["fromaccid"];
            this.tolocid = data["tolocid"];
            this.tobankid = data["tobankid"];
            this.toconfigid = data["toconfigid"];
            this.tobankaccid = data["tobankaccid"];
            this.toaccid = data["toaccid"];
            this.status = data["status"];
            this.transferamount = data["transferamount"];
            this.gllinkidfrom = data["gllinkidfrom"];
            this.gllinkidto = data["gllinkidto"];
            this.gldocidfrom = data["gldocidfrom"];
            this.gldocidto = data["gldocidto"];
            this.audtuser = data["audtuser"];
            this.audtdate = data["audtdate"];
            this.createdBy = data["createdBy"];
            this.createdOn = data["createdOn"];
            this.id = data["id"];
            this.linkDetIDBP = data["linkDetIDBP"];
            this.linkDetIDBR = data["linkDetIDBR"];
            this.linkDetIDCP = data["linkDetIDCP"];
            this.linkDetIDCR = data["linkDetIDCR"];
            this.posted = data["posted"];
            this.chNumber = data["chNumber"];
            this.chType = data["chType"];
        }
    }

    static fromJS(data: any): GLTransferDto {
        debugger
        data = typeof data === 'object' ? data : {};
        let result = new GLTransferDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["docid"] = this.docid;
        data["docdate"] = this.docdate;
        data["transferdate"] = this.transferdate;
        data["description"] = this.description;
        data["fromlocid"] = this.fromlocid;
        data["frombankid"] = this.frombankid;
        data["fromconfigid"] = this.fromconfigid;
        data["frombankaccid"] = this.frombankaccid;
        data["fromaccid"] = this.fromaccid;
        data["tolocid"] = this.tolocid;
        data["tobankid"] = this.tobankid;
        data["toconfigid"] = this.toconfigid;
        data["tobankaccid"] = this.tobankaccid;
        data["toaccid"] = this.toaccid;
        data["status"] = this.status;
        data["transferamount"] = this.transferamount;
        data["gllinkidfrom"] = this.gllinkidfrom;
        data["audtuser"]=this.audtuser;
        data["audtdate"] = this.audtdate;
        data["createdBy"]=this.createdBy;
        data["createdOn"] = this.createdOn;
        data["id"] = this.id;
        data["linkDetIDBP"] = this.linkDetIDBP;
        data["posted"] = this.posted;
        data["linkDetIDBR"] = this.linkDetIDBR;
        data["chType"] = this.chType;
        data["chNumber"] = this.chNumber;
        data["linkDetIDCP"] = this.linkDetIDCP;
        data["linkDetIDCR"] = this.linkDetIDCR;
        return data;
    }
}

export class GetGLTransferForEditOutput implements IGetGLTransferForEditOutput {
    glTransfer!: CreateOrEditGLTransferDto | undefined;

    constructor(data?: IGetGLTransferForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.glTransfer = data["glTransfer"] ? CreateOrEditGLTransferDto.fromJS(data["glTransfer"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetGLTransferForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetGLTransferForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["glTransfer"] = this.glTransfer ? this.glTransfer.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditGLTransferDto implements ICreateOrEditGLTransferDto {
    docid!: number | undefined;
    docdate!: moment.Moment | undefined;
    transferdate!: moment.Moment | undefined;
    description!: string | undefined;
    fromlocid!: number | undefined;
    frombankid!: string | undefined;
    fromconfigid!: number | undefined;
    frombankaccid!: string | undefined;
    fromaccid!: string | undefined;
    tolocid!: number | undefined;
    tobankid!: string | undefined;
    toconfigid!: number | undefined;
    tobankaccid!: string | undefined;
    toaccid!: string | undefined;
    status!: boolean | undefined;
    transferamount!: number | undefined;
    gllinkidfrom!: number | undefined;
    gllinkidto!: number | undefined;
    gldocidfrom!: number | undefined;
    gldocidto!: number | undefined;
    audtuser!: string | undefined;
    audtdate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createdOn!: moment.Moment | undefined;
    id!: number | undefined;
    linkDetIDBP!: number | undefined;
    linkDetIDBR!: number | undefined;
    linkDetIDCP!: number | undefined;
    linkDetIDCR!: number | undefined;
    posted!: boolean | undefined;
    chNumber!: string | undefined;
    chType!: number | undefined;
    constructor(data?: ICreateOrEditGLTransferDto) {
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
            this.docid = data["docid"];
            this.docdate = data["docdate"];
            this.transferdate = data["transferdate"];
            this.description = data["description"];
            this.fromlocid = data["fromlocid"];
            this.frombankid = data["frombankid"];
            this.fromconfigid = data["fromconfigid"];
            this.frombankaccid = data["frombankaccid"];
            this.fromaccid = data["fromaccid"];
            this.tolocid = data["tolocid"];
            this.tobankid = data["tobankid"];
            this.toconfigid = data["toconfigid"];
            this.tobankaccid = data["tobankaccid"];
            this.toaccid = data["toaccid"];
            this.status = data["status"];
            this.transferamount = data["transferamount"];
            this.gllinkidfrom = data["gllinkidfrom"];
            this.gllinkidto = data["gllinkidto"];
            this.gldocidfrom = data["gldocidfrom"];
            this.gldocidto = data["gldocidto"];
            this.audtuser = data["audtuser"];
            this.audtdate = data["audtdate"];
            this.createdBy = data["createdBy"];
            this.createdOn = data["createdOn"];
            this.id = data["id"];
            this.linkDetIDBP = data["linkDetIDBP"];
            this.linkDetIDBR = data["linkDetIDBR"];
            this.linkDetIDCP = data["linkDetIDCP"];
            this.linkDetIDCR = data["linkDetIDCR"];
            this.posted = data["posted"];
            this.chNumber = data["chNumber"];
            this.chType = data["chType"];
        }
    }

    static fromJS(data: any): CreateOrEditGLTransferDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditGLTransferDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["docid"] = this.docid;
        data["docdate"] = this.docdate;
        data["transferdate"] = this.transferdate;
        data["description"] = this.description;
        data["fromlocid"] = this.fromlocid;
        data["frombankid"] = this.frombankid;
        data["fromconfigid"] = this.fromconfigid;
        data["frombankaccid"] = this.frombankaccid;
        data["fromaccid"] = this.fromaccid;
        data["tolocid"] = this.tolocid;
        data["tobankid"] = this.tobankid;
        data["toconfigid"] = this.toconfigid;
        data["tobankaccid"] = this.tobankaccid;
        data["toaccid"] = this.toaccid;
        data["status"] = this.status;
        data["transferamount"] = this.transferamount;
        data["gllinkidfrom"] = this.gllinkidfrom;
        data["audtuser"]=this.audtuser;
        data["audtdate"] = this.audtdate;
        data["createdBy"]=this.createdBy;
        data["createdOn"] = this.createdOn;
        data["id"] = this.id;
        data["linkDetIDBP"] = this.linkDetIDBP;
        data["linkDetIDBR"] = this.linkDetIDBR;
        data["posted"] = this.posted;
        data["chNumber"] = this.chNumber;
        data["chType"] = this.chType;
        data["linkDetIDCP"]  = this.linkDetIDCP;
        data["linkDetIDCR"]  = this.linkDetIDCR;
        return data;
    }
}