import * as moment from 'moment';
import { IPagedResultDtoOfGetAllowancesForViewDto, IGetAllowancesForViewDto, IAllowancesDto, IGetAllowancesForEditOutput, ICreateOrEditAllowancesDto } from '../interface/allowances-interface';
import { CreateOrEditAllowancesDetailDto } from './allowanceDetails-dto';

export class PagedResultDtoOfGetAllowancesForViewDto implements IPagedResultDtoOfGetAllowancesForViewDto {
    totalCount!: number | undefined;
    items!: GetAllowancesForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetAllowancesForViewDto) {
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
                    this.items!.push(GetAllowancesForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetAllowancesForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetAllowancesForViewDto();
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

export class GetAllowancesForViewDto implements IGetAllowancesForViewDto {
    allowances!: AllowancesDto | undefined;

    constructor(data?: IGetAllowancesForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.allowances = data["allowances"] ? AllowancesDto.fromJS(data["allowances"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetAllowancesForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetAllowancesForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["allowances"] = this.allowances ? this.allowances.toJSON() : <any>undefined;
        return data;
    }
}

export class AllowancesDto implements IAllowancesDto {
    docID!: number | undefined;
    docdate!: moment.Moment | undefined;
    docMonth!: number | undefined;
    docYear!: number | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IAllowancesDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.docID = data["docID"];
            this.docdate = data["docdate"];
            this.docMonth = data["docMonth"];
            this.docYear = data["docYear"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): AllowancesDto {
        data = typeof data === 'object' ? data : {};
        let result = new AllowancesDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["docID"] = this.docID;
        data["docdate"] = this.docdate;
        data["docMonth"] = this.docMonth;
        data["docYear"] = this.docYear;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        
        return data;
    }
}

export class GetAllowancesForEditOutput implements IGetAllowancesForEditOutput {
    allowances!: CreateOrEditAllowancesDto | undefined;

    constructor(data?: IGetAllowancesForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.allowances = data["allowances"] ? CreateOrEditAllowancesDto.fromJS(data["allowances"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetAllowancesForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetAllowancesForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["allowances"] = this.allowances ? this.allowances.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditAllowancesDto implements ICreateOrEditAllowancesDto {
    flag!: boolean | undefined;
    allowancesDetail: CreateOrEditAllowancesDetailDto[] | undefined;
    docID!: number | undefined;
    docdate!: moment.Moment;
    docMonth!: number | undefined;
    docYear!: number | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditAllowancesDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.flag = data["flag"];
            this.docID = data["docID"];
            this.docdate = data["docdate"];
            this.docMonth = data["docMonth"];
            this.docYear = data["docYear"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
            this.allowancesDetail = [] as any;
            for (let item of data["allowancesDetail"])
                this.allowancesDetail!.push(CreateOrEditAllowancesDetailDto.fromJS(item));
        }
    }

    static fromJS(data: any): CreateOrEditAllowancesDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditAllowancesDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["flag"] = this.flag;
        data["docID"] = this.docID;
        data["docdate"] = this.docdate;
        data["docMonth"] = this.docMonth;
        data["docYear"] = this.docYear;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        if (this.allowancesDetail && this.allowancesDetail.constructor === Array) {
            data["allowancesDetail"] = [];
            for (let item of this.allowancesDetail)
                data["allowancesDetail"].push(item);
        }
        return data;
    }
}
