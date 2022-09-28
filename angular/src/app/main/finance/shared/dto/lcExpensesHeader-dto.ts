import * as moment from "moment";
import {
    ILCExpensesHeaderDto,
    ICreateOrEditLCExpensesHeaderDto,
    IPagedResultDtoOfLCExpensesHeaderDto,
    IGetLCExpensesHeaderForEditOutput,
    IGetLCExpensesHeaderForViewDto,
    IPagedResultDtoOfGetLCExpensesHeaderForViewDto
} from "../interface/lcExpensesHeader-interface";
import { CreateOrEditLCExpensesDetailDto } from "./lcExpensesDetail-dto";

export class PagedResultDtoOfLCExpensesHeaderDto
    implements IPagedResultDtoOfLCExpensesHeaderDto {
    totalCount!: number | undefined;
    items!: LCExpensesHeaderDto[] | undefined;

    constructor(data?: IPagedResultDtoOfLCExpensesHeaderDto) {
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
                    this.items!.push(LCExpensesHeaderDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfLCExpensesHeaderDto {
        data = typeof data === "object" ? data : {};
        let result = new PagedResultDtoOfLCExpensesHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === "object" ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items) data["items"].push(item.toJSON());
        }
        return data;
    }
}

export class LCExpensesHeaderDto implements ILCExpensesHeaderDto {
    locID!: number | undefined;
    docNo!: number | undefined;
    docDate!: moment.Moment | undefined;
    typeID!: string | undefined;
    accountID!: string | undefined;
    subAccID!: number | undefined;
    payableAccID!: string | undefined;
    lcNumber!: string | undefined;
    active!: string | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;
    linkDetID!: number | undefined;
    posted!: boolean | undefined;
    constructor(data?: ILCExpensesHeaderDto) {
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
            this.typeID = data["typeID"];
            this.docDate = data["docDate"];
            this.accountID = data["accountID"];
            this.subAccID = data["subAccID"];
            this.payableAccID = data["payableAccID"];
            this.lcNumber = data["lcNumber"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
            this.linkDetID = data["linkDetID"];
            this.posted = data["posted"];
        }
    }

    static fromJS(data: any): LCExpensesHeaderDto {
        data = typeof data === "object" ? data : {};
        let result = new LCExpensesHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === "object" ? data : {};
        data["locID"] = this.locID;
        data["docNo"] = this.docNo;
        data["typeID"] = this.typeID;
        data["docDate"] = this.docDate;
        data["accountID"] = this.accountID;
        data["subAccID"] = this.subAccID;
        data["payableAccID"] = this.payableAccID;
        data["lcNumber"] = this.lcNumber;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        data["linkDetID"] = this.linkDetID;
        data["posted"] = this.posted;
        return data;
    }
}

export class CreateOrEditLCExpensesHeaderDto
    implements ICreateOrEditLCExpensesHeaderDto {
    flag!: boolean | undefined;
    lcExpensesDetail!: CreateOrEditLCExpensesDetailDto[] | undefined;
    locID!: number | undefined;
    docNo!: number | undefined;
    typeID!: string | undefined;
    docDate!: moment.Moment | undefined;
    accountID!: string | undefined;
    subAccID!: number | undefined;
    payableAccID!: string | undefined;
    lcNumber!: string | undefined;
    active!: string | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;
    linkDetID!: number | undefined;
    posted!: boolean | undefined;
    narration!: string | undefined;

    constructor(data?: ICreateOrEditLCExpensesHeaderDto) {
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
            this.typeID = data["typeID"];
            this.docDate = data["docDate"];
            this.accountID = data["accountID"];
            this.subAccID = data["subAccID"];
            this.payableAccID = data["payableAccID"];
            this.lcNumber = data["lcNumber"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
            this.linkDetID = data["linkDetID"];
            this.posted = data["posted"];
            this.narration = data["narration"];
            this.lcExpensesDetail = [] as any;

            for (let item of data["lcExpensesDetail"])
                this.lcExpensesDetail!.push(
                    CreateOrEditLCExpensesDetailDto.fromJS(item)
                );
        }
    }

    static fromJS(data: any): CreateOrEditLCExpensesHeaderDto {
        data = typeof data === "object" ? data : {};
        let result = new CreateOrEditLCExpensesHeaderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === "object" ? data : {};
        data["flag"] = this.flag;
        data["locID"] = this.locID;
        data["docNo"] = this.docNo;
        data["typeID"] = this.typeID;
        data["docDate"] = this.docDate;
        data["accountID"] = this.accountID;
        data["subAccID"] = this.subAccID;
        data["payableAccID"] = this.payableAccID;
        data["lcNumber"] = this.lcNumber;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        data["linkDetID"] = this.linkDetID;
        data["posted"] = this.posted;
        data["narration"] = this.narration;
        if (
            this.lcExpensesDetail &&
            this.lcExpensesDetail.constructor === Array
        ) {
            data["lcExpensesDetail"] = [];
            for (let item of this.lcExpensesDetail)
                data["lcExpensesDetail"].push(item);
        }
        return data;
    }
}

export class PagedResultDtoOfGetLCExpensesHeaderForViewDto
    implements IPagedResultDtoOfGetLCExpensesHeaderForViewDto {
    totalCount!: number | undefined;
    items!: GetLCExpensesHeaderForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetLCExpensesHeaderForViewDto) {
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
                    this.items!.push(
                        GetLCExpensesHeaderForViewDto.fromJS(item)
                    );
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetLCExpensesHeaderForViewDto {
        data = typeof data === "object" ? data : {};
        let result = new PagedResultDtoOfGetLCExpensesHeaderForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === "object" ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items) data["items"].push(item.toJSON());
        }
        return data;
    }
}

export class GetLCExpensesHeaderForViewDto
    implements IGetLCExpensesHeaderForViewDto {
    lcExpensesHeader!: LCExpensesHeaderDto | undefined;

    constructor(data?: IGetLCExpensesHeaderForViewDto) {
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
            this.lcExpensesHeader = data["lcExpensesHeader"]
                ? LCExpensesHeaderDto.fromJS(data["lcExpensesHeader"])
                : <any>undefined;
        }
    }

    static fromJS(data: any): GetLCExpensesHeaderForViewDto {
        data = typeof data === "object" ? data : {};
        let result = new GetLCExpensesHeaderForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === "object" ? data : {};
        data["lcExpensesHeader"] = this.lcExpensesHeader
            ? this.lcExpensesHeader.toJSON()
            : <any>undefined;
        return data;
    }
}

export class GetLCExpensesHeaderForEditOutput
    implements IGetLCExpensesHeaderForEditOutput {
    lcExpensesHeader: CreateOrEditLCExpensesHeaderDto | undefined;

    constructor(data?: IGetLCExpensesHeaderForEditOutput) {
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
            this.lcExpensesHeader = data["lcExpensesHeader"]
                ? CreateOrEditLCExpensesHeaderDto.fromJS(
                    data["lcExpensesHeader"]
                )
                : <any>undefined;
        }
    }

    static fromJS(data: any): GetLCExpensesHeaderForEditOutput {
        debugger;
        data = typeof data === "object" ? data : {};
        let result = new GetLCExpensesHeaderForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === "object" ? data : {};

        data["lcExpensesHeader"] = this.lcExpensesHeader
            ? this.lcExpensesHeader.toJSON()
            : <any>undefined;
        return data;
    }
}
