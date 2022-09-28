import * as moment from 'moment';
import { IPagedResultDtoOfGetChequeBookForViewDto, IGetChequeBookForViewDto, IChequeBookDto, IGetChequeBookForEditOutput, ICreateOrEditChequeBookDto } from '../interface/chequeBooks-interface';
import { CreateOrEditChequeBookDetailDto } from './chequeBookDetails-dto';

export class PagedResultDtoOfGetChequeBookForViewDto implements IPagedResultDtoOfGetChequeBookForViewDto {
    totalCount!: number | undefined;
    items!: GetChequeBookForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetChequeBookForViewDto) {
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
                    this.items!.push(GetChequeBookForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetChequeBookForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetChequeBookForViewDto();
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

export class GetChequeBookForViewDto implements IGetChequeBookForViewDto {
    chequeBook!: ChequeBookDto | undefined;

    constructor(data?: IGetChequeBookForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.chequeBook = data["chequeBook"] ? ChequeBookDto.fromJS(data["chequeBook"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetChequeBookForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetChequeBookForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["chequeBook"] = this.chequeBook ? this.chequeBook.toJSON() : <any>undefined;
        return data;
    }
}

export class ChequeBookDto implements IChequeBookDto {
    docNo!: number | undefined;
    docDate!: moment.Moment | undefined;
    bankid!: string | undefined;
    bankAccNo!: string | undefined;
    fromChNo!: string | undefined;
    toChNo!: string | undefined;
    noofCh!: number | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IChequeBookDto) {
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
            this.bankid = data["bankid"];
            this.bankAccNo = data["bankAccNo"];
            this.fromChNo = data["fromChNo"];
            this.toChNo = data["toChNo"];
            this.noofCh = data["noofCh"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"] ? moment(data["audtDate"].toString()) : <any>undefined;
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"] ? moment(data["createDate"].toString()) : <any>undefined;
            this.id = data["id"];
        }
    }

    static fromJS(data: any): ChequeBookDto {
        data = typeof data === 'object' ? data : {};
        let result = new ChequeBookDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate ? this.docDate.toISOString() : <any>undefined;
        data["bankid"] = this.bankid;
        data["bankAccNo"] = this.bankAccNo;
        data["fromChNo"] = this.fromChNo;
        data["toChNo"] = this.toChNo;
        data["noofCh"] = this.noofCh;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? this.audtDate.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? this.createDate.toISOString() : <any>undefined;
        data["id"] = this.id;
        return data;
    }
}

export class GetChequeBookForEditOutput implements IGetChequeBookForEditOutput {
    chequeBook!: CreateOrEditChequeBookDto | undefined;

    constructor(data?: IGetChequeBookForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.chequeBook = data["chequeBook"] ? CreateOrEditChequeBookDto.fromJS(data["chequeBook"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetChequeBookForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetChequeBookForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["chequeBook"] = this.chequeBook ? this.chequeBook.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditChequeBookDto implements ICreateOrEditChequeBookDto {
    flag!: boolean | undefined;
    docNo!: number | undefined;
    docDate!: moment.Moment | undefined;
    bankid!: string;
    bankName!: string;
    bankAccNo!: string | undefined;
    fromChNo!: string | undefined;
    toChNo!: string | undefined;
    noofCh!: number | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;
    chequeBookDetail!: CreateOrEditChequeBookDetailDto[] | undefined;

    constructor(data?: ICreateOrEditChequeBookDto) {
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
            this.docDate = data["docDate"];
            this.bankid = data["bankid"];
            this.bankName = data["bankName"];
            this.bankAccNo = data["bankAccNo"];
            this.fromChNo = data["fromChNo"];
            this.toChNo = data["toChNo"];
            this.noofCh = data["noofCh"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
            this.chequeBookDetail = [] as any;
            for (let item of data["chequeBookDetail"])
                this.chequeBookDetail!.push(CreateOrEditChequeBookDetailDto.fromJS(item));
        }
    }

    static fromJS(data: any): CreateOrEditChequeBookDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditChequeBookDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["flag"] = this.flag;
        data["docNo"] = this.docNo;
        data["docDate"] = this.docDate ? moment(this.docDate).toISOString(true) : <any>undefined;
        data["bankid"] = this.bankid;
        data["bankName"] = this.bankName;
        data["bankAccNo"] = this.bankAccNo;
        data["fromChNo"] = this.fromChNo;
        data["toChNo"] = this.toChNo;
        data["noofCh"] = this.noofCh;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? moment(this.audtDate).toISOString(true) : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? moment(this.createDate).toISOString(true) : <any>undefined;
        data["id"] = this.id;
        if (this.chequeBookDetail && this.chequeBookDetail.constructor === Array) {
            data["chequeBookDetail"] = [];
            for (let item of this.chequeBookDetail)
                data["chequeBookDetail"].push(item);
        }
        return data;
    }
}
