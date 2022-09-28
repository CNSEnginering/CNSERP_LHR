import * as moment from 'moment';
import { IPagedResultDtoOfGetLCExpensesDetailForViewDto, IGetLCExpensesDetailForViewDto, ILCExpensesDetailDto, IGetLCExpensesDetailForEditOutput, ICreateOrEditLCExpensesDetailDto, IPagedResultDtoOfLCExpensesDetailsDto, IListResultDtoOfLCExpenses } from '../interface/lcExpensesDetail-interface';


export class PagedResultDtoOfGetLCExpensesDetailForViewDto implements IPagedResultDtoOfGetLCExpensesDetailForViewDto {
    totalCount!: number | undefined;
    items!: GetLCExpensesDetailForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetLCExpensesDetailForViewDto) {
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
                    this.items!.push(GetLCExpensesDetailForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetLCExpensesDetailForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetLCExpensesDetailForViewDto();
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

export class PagedResultDtoOfLCExpensesDetailDto implements IPagedResultDtoOfLCExpensesDetailsDto {
    totalCount!: number | undefined;
    items!: LCExpensesDetailDto[] | undefined;

    constructor(data?: IPagedResultDtoOfLCExpensesDetailsDto) {
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
                    this.items!.push(LCExpensesDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfLCExpensesDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfLCExpensesDetailDto();
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

export class GetLCExpensesDetailForViewDto implements IGetLCExpensesDetailForViewDto {
    lcExpensesDetail!: LCExpensesDetailDto | undefined;

    constructor(data?: IGetLCExpensesDetailForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.lcExpensesDetail = data["lcExpensesDetail"] ? LCExpensesDetailDto.fromJS(data["lcExpensesDetail"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetLCExpensesDetailForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetLCExpensesDetailForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["lcExpensesDetail"] = this.lcExpensesDetail ? this.lcExpensesDetail.toJSON() : <any>undefined;
        return data;
    }
}

export class LCExpensesDetailDto implements ILCExpensesDetailDto {
    detID!: number | undefined;
    locID!: number | undefined;
    docNo!: number | undefined;
    expDesc!: string | undefined;
    amount!: number | undefined;
    id!: number | undefined;

    constructor(data?: ILCExpensesDetailDto) {
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
            this.locID = data["locID"];
            this.docNo = data["docNo"];
            this.expDesc = data["expDesc"];
            this.amount = data["amount"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): LCExpensesDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new LCExpensesDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["locID"] = this.locID;
        data["docNo"] = this.docNo;
        data["expDesc"] = this.expDesc;
        data["amount"] = this.amount;
        data["id"] = this.id;
        return data;
    }
}

export class GetLCExpensesDetailForEditOutput implements IGetLCExpensesDetailForEditOutput {
    lcExpensesDetail!: CreateOrEditLCExpensesDetailDto | undefined;

    constructor(data?: IGetLCExpensesDetailForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.lcExpensesDetail = data["lcExpensesDetail"] ? CreateOrEditLCExpensesDetailDto.fromJS(data["lcExpensesDetail"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetLCExpensesDetailForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetLCExpensesDetailForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["lcExpensesDetail"] = this.lcExpensesDetail ? this.lcExpensesDetail.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditLCExpensesDetailDto implements ICreateOrEditLCExpensesDetailDto {
    detID!: number | undefined;
    locID!: number | undefined;
    docNo!: number | undefined;
    expDesc!: string | undefined;
    amount!: number | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditLCExpensesDetailDto) {
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
            this.detID = data["detID"];
            this.locID = data["locID"];
            this.docNo = data["docNo"];
            this.expDesc = data["expDesc"];
            this.amount = data["amount"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditLCExpensesDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditLCExpensesDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["locID"] = this.locID;
        data["docNo"] = this.docNo;
        data["expDesc"] = this.expDesc;
        data["amount"] = this.amount;
        data["id"] = this.id;
        return data;
    }
}

export class ListResultDtoOfLCExpenses implements IListResultDtoOfLCExpenses {
    items!: CreateOrEditLCExpensesDetailDto[] | undefined;

    constructor(data?: IListResultDtoOfLCExpenses) {
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
                    this.items!.push(CreateOrEditLCExpensesDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ListResultDtoOfLCExpenses {
        data = typeof data === 'object' ? data : {};
        let result = new ListResultDtoOfLCExpenses();
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

