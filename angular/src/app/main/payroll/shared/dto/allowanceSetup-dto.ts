import * as moment from 'moment';
import { IPagedResultDtoOfGetAllowanceSetupForViewDto, IGetAllowanceSetupForViewDto, IAllowanceSetupDto, IGetAllowanceSetupForEditOutput, ICreateOrEditAllowanceSetupDto } from '../interface/allowanceSetup-interface';

export class PagedResultDtoOfGetAllowanceSetupForViewDto implements IPagedResultDtoOfGetAllowanceSetupForViewDto {
    totalCount!: number | undefined;
    items!: GetAllowanceSetupForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetAllowanceSetupForViewDto) {
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
                    this.items!.push(GetAllowanceSetupForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetAllowanceSetupForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetAllowanceSetupForViewDto();
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

export class GetAllowanceSetupForViewDto implements IGetAllowanceSetupForViewDto {
    allowanceSetup!: AllowanceSetupDto | undefined;

    constructor(data?: IGetAllowanceSetupForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.allowanceSetup = data["allowanceSetup"] ? AllowanceSetupDto.fromJS(data["allowanceSetup"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetAllowanceSetupForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetAllowanceSetupForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["allowanceSetup"] = this.allowanceSetup ? this.allowanceSetup.toJSON() : <any>undefined;
        return data;
    }
}

export class AllowanceSetupDto implements IAllowanceSetupDto {
    docID!: number | undefined;
    fuelRate!: number | undefined;
    fuelDate?: string;
    milageRate!: number | undefined;
    repairRate!: number | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: IAllowanceSetupDto) {
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
            this.fuelRate = data["fuelRate"];
            this.fuelDate = data["fuelDate"];
            this.milageRate = data["milageRate"];
            this.repairRate = data["repairRate"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): AllowanceSetupDto {
        data = typeof data === 'object' ? data : {};
        let result = new AllowanceSetupDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["docID"] = this.docID;
        data["fuelRate"] = this.fuelRate;
        data["fuelDate"] = this.fuelDate;
        data["milageRate"] = this.milageRate;
        data["repairRate"] = this.repairRate;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? moment(this.audtDate).toISOString(true) : <any>undefined;;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? moment(this.createDate).toISOString(true) : <any>undefined;;
        data["id"] = this.id;
        return data;
    }
}

export class GetAllowanceSetupForEditOutput implements IGetAllowanceSetupForEditOutput {
    allowanceSetup!: CreateOrEditAllowanceSetupDto | undefined;

    constructor(data?: IGetAllowanceSetupForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.allowanceSetup = data["allowanceSetup"] ? CreateOrEditAllowanceSetupDto.fromJS(data["allowanceSetup"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetAllowanceSetupForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetAllowanceSetupForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["allowanceSetup"] = this.allowanceSetup ? this.allowanceSetup.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditAllowanceSetupDto implements ICreateOrEditAllowanceSetupDto {
    docID!: number | undefined;
    fuelRate!: number | undefined;
    fuelDate?: string|Date|null;
    milageRate!: number | undefined;
    perLiterMilage!:number|undefined;
    repairRate!: number | undefined;
    active!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditAllowanceSetupDto) {
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
            this.fuelRate = data["fuelRate"];
            this.fuelDate = data["fuelDate"];
            this.milageRate = data["milageRate"];
            this.repairRate = data["repairRate"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.perLiterMilage=data["perLiterMilage"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditAllowanceSetupDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditAllowanceSetupDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["docID"] = this.docID;
        data["fuelRate"] = this.fuelRate;
        data["fuelDate"] = this.fuelDate;
        data["milageRate"] = this.milageRate;
        data["repairRate"] = this.repairRate;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate ? moment(this.audtDate).toISOString(true) : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate ? moment(this.createDate).toISOString(true) : <any>undefined;
        data["id"] = this.id;
        data["perLiterMilage"]=this.perLiterMilage;
        return data;
    }
}
