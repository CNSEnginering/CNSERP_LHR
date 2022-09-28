import * as moment from 'moment';
import { IPagedResultDtoOfGetChequeBookDetailForViewDto, IGetChequeBookDetailForViewDto, IChequeBookDetailDto, IGetChequeBookDetailForEditOutput, ICreateOrEditChequeBookDetailDto } from '../interface/chequeBookDetails-interface';


export class PagedResultDtoOfGetChequeBookDetailForViewDto implements IPagedResultDtoOfGetChequeBookDetailForViewDto {
    totalCount!: number | undefined;
    items!: GetChequeBookDetailForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetChequeBookDetailForViewDto) {
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
                    this.items!.push(GetChequeBookDetailForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetChequeBookDetailForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetChequeBookDetailForViewDto();
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

export class GetChequeBookDetailForViewDto implements IGetChequeBookDetailForViewDto {
    chequeBookDetail!: ChequeBookDetailDto | undefined;

    constructor(data?: IGetChequeBookDetailForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.chequeBookDetail = data["chequeBookDetail"] ? ChequeBookDetailDto.fromJS(data["chequeBookDetail"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetChequeBookDetailForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetChequeBookDetailForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["chequeBookDetail"] = this.chequeBookDetail ? this.chequeBookDetail.toJSON() : <any>undefined;
        return data;
    }
}

export class ChequeBookDetailDto implements IChequeBookDetailDto {
    detID!: number | undefined;
    docNo!: number | undefined;
    bankid!: string | undefined;
    bankAccNo!: string | undefined;
    fromChNo!: string | undefined;
    toChNo!: string | undefined;
    booKID!: string | undefined;
    voucherNo!: number | undefined;
    voucherDate!: moment.Moment | undefined;
    narration!: string | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IChequeBookDetailDto) {
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
            this.docNo = data["docNo"];
            this.bankid = data["bankid"];
            this.bankAccNo = data["bankAccNo"];
            this.fromChNo = data["fromChNo"];
            this.toChNo = data["toChNo"];
            this.booKID = data["booKID"];
            this.voucherNo = data["voucherNo"];
            this.voucherDate = data["voucherDate"];
            this.narration = data["narration"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): ChequeBookDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new ChequeBookDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["docNo"] = this.docNo;
        data["bankid"] = this.bankid;
        data["bankAccNo"] = this.bankAccNo;
        data["fromChNo"] = this.fromChNo;
        data["toChNo"] = this.toChNo;
        data["booKID"] = this.booKID;
        data["voucherNo"] = this.voucherNo;
        data["voucherDate"] = this.voucherDate ? moment(this.voucherDate).toISOString(true) : <any>undefined;
        data["narration"] = this.narration;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? moment(this.audtDate).toISOString(true) : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? moment(this.createDate).toISOString(true) : <any>undefined;
        data["id"] = this.id;
        return data;
    }
}

export class GetChequeBookDetailForEditOutput implements IGetChequeBookDetailForEditOutput {
    chequeBookDetail!: CreateOrEditChequeBookDetailDto | undefined;

    constructor(data?: IGetChequeBookDetailForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.chequeBookDetail = data["chequeBookDetail"] ? CreateOrEditChequeBookDetailDto.fromJS(data["chequeBookDetail"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetChequeBookDetailForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetChequeBookDetailForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["chequeBookDetail"] = this.chequeBookDetail ? this.chequeBookDetail.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditChequeBookDetailDto implements ICreateOrEditChequeBookDetailDto {
    detID!: number | undefined;
    docNo!: number | undefined;
    bankid!: string;
    bankAccNo!: string | undefined;
    fromChNo!: string | undefined;
    toChNo!: string | undefined;
    booKID!: string | undefined;
    voucherNo!: number | undefined;
    voucherDate!: moment.Moment | undefined;
    narration!: string | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditChequeBookDetailDto) {
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
            this.docNo = data["docNo"];
            this.bankid = data["bankid"];
            this.bankAccNo = data["bankAccNo"];
            this.fromChNo = data["fromChNo"];
            this.toChNo = data["toChNo"];
            this.booKID = data["booKID"];
            this.voucherNo = data["voucherNo"];
            this.voucherDate = data["voucherDate"] ? moment(data["voucherDate"].toString()) : <any>undefined;
            this.narration = data["narration"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditChequeBookDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditChequeBookDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["docNo"] = this.docNo;
        data["bankid"] = this.bankid;
        data["bankAccNo"] = this.bankAccNo;
        data["fromChNo"] = this.fromChNo;
        data["toChNo"] = this.toChNo;
        data["booKID"] = this.booKID;
        data["voucherNo"] = this.voucherNo;
        data["voucherDate"] = this.voucherDate ? this.voucherDate.toISOString() : <any>undefined;
        data["narration"] = this.narration;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        return data;
    }
}
