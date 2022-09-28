import * as moment from 'moment';
import { IPagedResultDtoOfGLReconDetailsDto, IGLReconDetailsDto, ICreateOrEditGLReconDetailsDto, IlistResultDtoOfBankReconcileDetail } from '../interface/glReconDetails-interface';

export class PagedResultDtoOfGLReconDetailsDto implements IPagedResultDtoOfGLReconDetailsDto {
    totalCount!: number | undefined;
    items!: GLReconDetailsDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGLReconDetailsDto) {
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
                    this.items!.push(GLReconDetailsDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGLReconDetailsDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGLReconDetailsDto();
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

export class GLReconDetailsDto implements IGLReconDetailsDto {
    detID: number | undefined;
    bookID!: string | undefined;
    configID!: number | undefined;
    voucherID!: number | undefined;
    voucherDate!: moment.Moment | undefined;
    clearingDate!: moment.Moment | undefined;
    amount!: number | undefined;
    glDetID!: number | undefined;
    fmtDocNo:number | undefined;
    include!: boolean | undefined;
    id: number | undefined;

    constructor(data?: IGLReconDetailsDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.detID = data["detID"];
            this.bookID = data["bookID"];
            this.configID = data["configID"];
            this.voucherID = data["voucherID"];
            this.voucherDate = data["voucherDate"];
            this.clearingDate = data["clearingDate"];
            this.amount = data["amount"];
            this.fmtDocNo=data["fmtDocNo"];
            this.include = data["include"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): GLReconDetailsDto {
        data = typeof data === 'object' ? data : {};
        let result = new GLReconDetailsDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["bookID"] = this.bookID;
        data["configID"] = this.configID;
        data["voucherID"] = this.voucherID;
        data["voucherDate"] = this.voucherDate;
        data["clearingDate"] = this.clearingDate;
        data["amount"] = this.amount;
        data["include"] = this.include;
        data["id"] = this.id;
        data["fmtDocNo"] = this.fmtDocNo;
        return data;
    }
}

export class CreateOrEditGLReconDetailsDto implements ICreateOrEditGLReconDetailsDto {
    detID: number | undefined;
    bookID!: string | undefined;
    configID!: string | undefined;
    voucherID!: number | undefined;
    voucherDate!: moment.Moment | undefined;
    clearingDate!: moment.Moment | undefined;
    Dr!: number | undefined;
    Cr!: number | undefined;
    include!: boolean | undefined;
    id: number | undefined;
    glDetID: number | undefined;
    chNumber: string | undefined;
    fmtDocNo:number | undefined;

    constructor(data?: ICreateOrEditGLReconDetailsDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.detID = data["detID"];
            this.bookID = data["bookID"];
            this.configID = data["configID"];
            this.voucherID = data["voucherID"];
            this.voucherDate = data["voucherDate"];
            this.clearingDate = data["clearingDate"];
            this.Dr = data["dr"];
            this.Cr = data["cr"];
            this.include = data["include"];
            this.id = data["id"];
            this.glDetID = data["glDetID"];
            this.chNumber=data["chNumber"];
            this.fmtDocNo = data["fmtDocNo"];
        }
    }

    static fromJS(data: any): CreateOrEditGLReconDetailsDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditGLReconDetailsDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["bookID"] = this.bookID;
        data["configID"] = this.configID;
        data["voucherID"] = this.voucherID;
        data["voucherDate"] = this.voucherDate;
        data["clearingDate"] = this.clearingDate;
        data["dr"] = this.Dr;
        data["cr"] = this.Cr;
        data["include"] = this.include;
        data["id"] = this.id;
        data["glDetID"] = this.glDetID;
        data["chNumber"]=this.chNumber;
        data["fmtDocNo"] = this.fmtDocNo;
        return data;
    }
}


export class listResultDtoOfBankReconcileDetail implements IlistResultDtoOfBankReconcileDetail {
    items!: CreateOrEditGLReconDetailsDto[] | undefined;

    constructor(data?: IlistResultDtoOfBankReconcileDetail) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [] as any;
                for (let item of data["items"])
                    this.items!.push(CreateOrEditGLReconDetailsDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): listResultDtoOfBankReconcileDetail {
        data = typeof data === 'object' ? data : {};
        let result = new listResultDtoOfBankReconcileDetail();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        return data;
    }
}
