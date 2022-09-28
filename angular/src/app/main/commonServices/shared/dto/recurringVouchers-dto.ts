import * as moment from 'moment';
import { IPagedResultDtoOfGetRecurringVoucherForViewDto, IGetRecurringVoucherForViewDto, IRecurringVoucherDto, IGetRecurringVoucherForEditOutput, ICreateOrEditRecurringVoucherDto } from '../interface/recurringVouchers-interface';

export class PagedResultDtoOfGetRecurringVoucherForViewDto implements IPagedResultDtoOfGetRecurringVoucherForViewDto {
    totalCount!: number | undefined;
    items!: GetRecurringVoucherForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetRecurringVoucherForViewDto) {
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
                    this.items!.push(GetRecurringVoucherForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetRecurringVoucherForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetRecurringVoucherForViewDto();
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

export class GetRecurringVoucherForViewDto implements IGetRecurringVoucherForViewDto {
    recurringVoucher!: RecurringVoucherDto | undefined;

    constructor(data?: IGetRecurringVoucherForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.recurringVoucher = data["recurringVoucher"] ? RecurringVoucherDto.fromJS(data["recurringVoucher"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetRecurringVoucherForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetRecurringVoucherForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["recurringVoucher"] = this.recurringVoucher ? this.recurringVoucher.toJSON() : <any>undefined;
        return data; 
    }
}

export class RecurringVoucherDto implements IRecurringVoucherDto {
    docNo!: number | undefined;
    bookID!: string | undefined;
    voucherNo!: number | undefined;
    fmtVoucherNo!: string | undefined;
    voucherDate!: moment.Moment | undefined;
    voucherMonth!: number | undefined;
    configID!: number | undefined;
    reference!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IRecurringVoucherDto) {
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
            this.bookID = data["bookID"];
            this.voucherNo = data["voucherNo"];
            this.fmtVoucherNo = data["fmtVoucherNo"];
            this.voucherDate = data["voucherDate"];
            this.voucherMonth = data["voucherMonth"];
            this.configID = data["configID"];
            this.reference = data["reference"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): RecurringVoucherDto {
        data = typeof data === 'object' ? data : {};
        let result = new RecurringVoucherDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["docNo"] = this.docNo;
        data["bookID"] = this.bookID;
        data["voucherNo"] = this.voucherNo;
        data["fmtVoucherNo"] = this.fmtVoucherNo;
        data["voucherDate"] = this.voucherDate ? moment(this.voucherDate).toISOString(true) : <any>undefined;
        data["voucherMonth"] = this.voucherMonth;
        data["configID"] = this.configID;
        data["reference"] = this.reference;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? moment(this.audtDate).toISOString(true) : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? moment(this.createDate).toISOString(true) : <any>undefined;
        data["id"] = this.id;
        return data; 
    }
}

export class GetRecurringVoucherForEditOutput implements IGetRecurringVoucherForEditOutput {
    recurringVoucher!: CreateOrEditRecurringVoucherDto | undefined;

    constructor(data?: IGetRecurringVoucherForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.recurringVoucher = data["recurringVoucher"] ? CreateOrEditRecurringVoucherDto.fromJS(data["recurringVoucher"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetRecurringVoucherForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetRecurringVoucherForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["recurringVoucher"] = this.recurringVoucher ? this.recurringVoucher.toJSON() : <any>undefined;
        return data; 
    }
}

export class CreateOrEditRecurringVoucherDto implements ICreateOrEditRecurringVoucherDto {
    docNo!: number | undefined;
    bookID!: string | undefined;
    voucherNo!: number | undefined;
    fmtVoucherNo!: string | undefined;
    voucherDate!: moment.Moment | undefined;
    voucherMonth!: number | undefined;
    configID!: number | undefined;
    reference!: string | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditRecurringVoucherDto) {
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
            this.bookID = data["bookID"];
            this.voucherNo = data["voucherNo"];
            this.fmtVoucherNo = data["fmtVoucherNo"];
            this.voucherDate = data["voucherDate"];
            this.voucherMonth = data["voucherMonth"];
            this.configID = data["configID"];
            this.reference = data["reference"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditRecurringVoucherDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditRecurringVoucherDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["docNo"] = this.docNo;
        data["bookID"] = this.bookID;
        data["voucherNo"] = this.voucherNo;
        data["fmtVoucherNo"] = this.fmtVoucherNo;
        data["voucherDate"] = this.voucherDate ? moment(this.voucherDate).toISOString(true) : <any>undefined;
        data["voucherMonth"] = this.voucherMonth;
        data["configID"] = this.configID;
        data["reference"] = this.reference;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? moment(this.audtDate).toISOString(true) : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? moment(this.createDate).toISOString(true) : <any>undefined;
        data["id"] = this.id;
        return data; 
    }
}
