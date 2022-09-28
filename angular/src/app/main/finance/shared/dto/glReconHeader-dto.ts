import * as moment from 'moment';
import { IGLReconHeaderDto, ICreateOrEditGLReconHeaderDto, IPagedResultDtoOfGLReconHeaderDto, IGetBankReconcileForEditOutput } from '../interface/glReconHeader-interface';
import { CreateOrEditGLReconDetailsDto } from './glReconDetails-dto';

export class PagedResultDtoOfGLReconHeaderDto implements IPagedResultDtoOfGLReconHeaderDto {
    totalCount!: number | undefined;
    items!: GLReconHeaderDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGLReconHeaderDto) {
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
                    this.items!.push(GLReconHeaderDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGLReconHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGLReconHeaderDto();
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

export class GLReconHeaderDto implements IGLReconHeaderDto {
    docID: string | undefined;
    docNo: number | undefined;
    docDate!: moment.Moment | undefined;
    bankID!: string | undefined;
    bankName!: string | undefined;
    beginBalance!: number | undefined;
    endBalance!: number | undefined;
    reconcileAmt: number | undefined;
    diffAmount!: number | undefined;
    statementAmt: number | undefined;
    clDepAmt!: number | undefined;
    clPayAmt!: number | undefined;
    unClDepAmt!: number | undefined;
    unClPayAmt!: number | undefined;
    clItems!: number | undefined;
    unClItems!: number | undefined;
    narration!: string | undefined;
    completed!: boolean;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createdDate!: moment.Moment | undefined;
    id!: number | undefined;
    approved: boolean | undefined;

    constructor(data?: IGLReconHeaderDto) {
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
            this.docID = data["docID"];
            this.docDate = data["docDate"] ? moment(data["docDate"].toString()) : <any>undefined;
            this.bankID = data["bankID"];
            this.bankName = data["bankName"];
            this.beginBalance = data["beginBalance"];
            this.endBalance = data["endBalance"];
            this.reconcileAmt = data["reconcileAmt"];
            this.diffAmount = data["diffAmount"];
            this.statementAmt = data["statementAmt"];
            this.clDepAmt = data["clDepAmt"];
            this.clPayAmt = data["clPayAmt"];
            this.unClDepAmt = data["unClDepAmt"];
            this.unClPayAmt = data["unClPayAmt"];
            this.clItems = data["clItems"];
            this.unClItems = data["unClItems"];
            this.narration = data["narration"];
            this.completed = data["completed"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createdDate = data["createdDate"] ? moment(data["createdDate"].toString()) : <any>undefined;
            this.id = data["id"];
            this.docNo = data["docNo"];

            this.approved = data["approved"];
        }
    }

    static fromJS(data: any): GLReconHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new GLReconHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["docID"] = this.docID;
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["bankID"] = this.bankID;
        data["bankName"] = this.bankName;
        data["beginBalance"] = this.beginBalance;
        data["endBalance"] = this.endBalance;
        data["reconcileAmt"] = this.reconcileAmt;
        data["diffAmount"] = this.diffAmount;
        data["statementAmt"] = this.statementAmt;
        data["clDepAmt"] = this.clDepAmt;
        data["clPayAmt"] = this.clPayAmt;
        data["unClDepAmt"] = this.unClDepAmt;
        data["unClPayAmt"] = this.unClPayAmt;
        data["clItems"] = this.clItems;
        data["unClItems"] = this.unClItems;
        data["narration"] = this.narration;
        data["completed"] = this.completed;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createdDate"] = this.createdDate ? this.createdDate.toISOString() : <any>undefined;
        data["id"] = this.id;

        data["approved"]=this.approved;
        return data;
    }
}

export class CreateOrEditGLReconHeaderDto implements ICreateOrEditGLReconHeaderDto {
    flag!: boolean | undefined
    bankReconcileDetail: CreateOrEditGLReconDetailsDto[] | undefined;
    docID: string | undefined;
    docNo: number | undefined;
    docDate!: moment.Moment | undefined;
    bankID!: string | undefined;
    bankName!: string | undefined;
    beginBalance!: number | undefined;
    endBalance!: number | undefined;
    reconcileAmt: number | undefined;
    diffAmount!: number | undefined;
    statementAmt: number | undefined;
    clDepAmt!: number | undefined;
    clPayAmt!: number | undefined;
    unClDepAmt!: number | undefined;
    unClPayAmt!: number | undefined;
    clItems!: number | undefined;
    unClItems!: number | undefined;
    narration!: string | undefined;
    completed!: boolean;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createdDate!: moment.Moment | undefined;
    id!: number | undefined;
    approved!: boolean | undefined;

    constructor(data?: ICreateOrEditGLReconHeaderDto) {
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
            this.docID = data["docID"];
            this.docDate = data["docDate"] ? moment(data["docDate"].toString()) : <any>undefined;
            this.bankID = data["bankID"];
            this.bankName = data["bankName"];
            this.beginBalance = data["beginBalance"];
            this.endBalance = data["endBalance"];
            this.reconcileAmt = data["reconcileAmt"];
            this.diffAmount = data["diffAmount"];
            this.statementAmt = data["statementAmt"];
            this.clDepAmt = data["clDepAmt"];
            this.clPayAmt = data["clPayAmt"];
            this.unClDepAmt = data["unClDepAmt"];
            this.unClPayAmt = data["unClPayAmt"];
            this.clItems = data["clItems"];
            this.unClItems = data["unClItems"];
            this.narration = data["narration"];
            this.completed = data["completed"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createdDate = data["createdDate"] ? moment(data["createdDate"].toString()) : <any>undefined;
            this.id = data["id"];
            this.docNo = data["docNo"];
            this.approved = data["approved"];
            this.bankReconcileDetail = [] as any;
            for (let item of data["bankReconcileDetail"])
                this.bankReconcileDetail!.push(CreateOrEditGLReconDetailsDto.fromJS(item));
        }
    }

    static fromJS(data: any): CreateOrEditGLReconHeaderDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditGLReconHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["flag"] = this.flag;
        data["docID"] = this.docID;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["bankID"] = this.bankID;
        data["bankName"] = this.bankName;
        data["beginBalance"] = this.beginBalance;
        data["endBalance"] = this.endBalance;
        data["reconcileAmt"] = this.reconcileAmt;
        data["diffAmount"] = this.diffAmount;
        data["statementAmt"] = this.statementAmt;
        data["clDepAmt"] = this.clDepAmt;
        data["clPayAmt"] = this.clPayAmt;
        data["unClDepAmt"] = this.unClDepAmt;
        data["unClPayAmt"] = this.unClPayAmt;
        data["clItems"] = this.clItems;
        data["unClItems"] = this.unClItems;
        data["narration"] = this.narration;
        data["completed"] = this.completed;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createdDate"] = this.createdDate ? this.createdDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        data["docNo"] = this.docNo;
        data["approved"] = this.approved;
        if (this.bankReconcileDetail && this.bankReconcileDetail.constructor === Array) {
            data["bankReconcileDetail"] = [];
            for (let item of this.bankReconcileDetail)
                data["bankReconcileDetail"].push(item.toJSON());
        }
        return data;
    }
}

export class PagedResultDtoOfGetGLReconHeaderForViewDto implements IPagedResultDtoOfGetGLReconHeaderForViewDto {
    totalCount!: number | undefined;
    items!: GetGLReconHeaderForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetGLReconHeaderForViewDto) {
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
                    this.items!.push(GetGLReconHeaderForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetGLReconHeaderForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetGLReconHeaderForViewDto();
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

export interface IPagedResultDtoOfGetGLReconHeaderForViewDto {
    totalCount: number | undefined;
    items: GetGLReconHeaderForViewDto[] | undefined;
}

export class GetGLReconHeaderForViewDto implements IGetGLReconHeaderForViewDto {
    gLReconHeader!: GLReconHeaderDto | undefined;

    constructor(data?: IGetGLReconHeaderForViewDto) {
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
            this.gLReconHeader = data["bankReconcile"] ? GLReconHeaderDto.fromJS(data["bankReconcile"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetGLReconHeaderForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetGLReconHeaderForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["bankReconcile"] = this.gLReconHeader ? this.gLReconHeader.toJSON() : <any>undefined;
        return data;
    }
}

export interface IGetGLReconHeaderForViewDto {
    gLReconHeader: GLReconHeaderDto | undefined;
}

export class GetBankReconcileForEditOutput implements IGetBankReconcileForEditOutput {
    gLReconHeader: CreateOrEditGLReconHeaderDto | undefined;


    constructor(data?: IGetBankReconcileForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.gLReconHeader = data["bankReconcile"] ? CreateOrEditGLReconHeaderDto.fromJS(data["bankReconcile"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetBankReconcileForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetBankReconcileForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};

        data["bankReconcile"] = this.gLReconHeader ? this.gLReconHeader.toJSON() : <any>undefined;
        return data;
    }
}