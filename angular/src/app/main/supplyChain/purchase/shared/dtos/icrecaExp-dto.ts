import { IPagedResultDtoOfICRECAExpDto, IICRECAExpDto, ICreateOrEditICRECAExpDto } from "../interfaces/icrecaExp-interface";

export class PagedResultDtoOfICRECAExpDto implements IPagedResultDtoOfICRECAExpDto {
    totalCount!: number | undefined;
    items!: ICRECAExpDto[] | undefined;

    constructor(data?: IPagedResultDtoOfICRECAExpDto) {
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
                    this.items!.push(ICRECAExpDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfICRECAExpDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfICRECAExpDto();
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

export class ICRECAExpDto implements IICRECAExpDto {
    detID!: number | undefined;
    locID!: number | undefined;
    docNo!: number | undefined;
    expType!: string | undefined;
    accountID!: string | undefined;
    accountName!: string | undefined;
    amount!: number | undefined;
    id!: number | undefined;

    constructor(data?: IICRECAExpDto) {
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
            this.expType = data["expType"];
            this.accountID = data["accountID"];
            this.accountName = data["accountName"];
            this.amount = data["amount"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): ICRECAExpDto {
        data = typeof data === 'object' ? data : {};
        let result = new ICRECAExpDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["locID"] = this.locID;
        data["docNo"] = this.docNo;
        data["expType"] = this.expType;
        data["accountID"] = this.accountID;
        data["accountName"] = this.accountName;
        data["amount"] = this.amount;
        data["id"] = this.id;
        return data; 
    }
}

export class CreateOrEditICRECAExpDto implements ICreateOrEditICRECAExpDto {
    detID!: number | undefined;
    locID!: number | undefined;
    docNo!: number | undefined;
    expType!: string | undefined;
    accountID!: string | undefined;
    amount!: number | undefined;
    id!: number | undefined;
    constructor(data?: ICreateOrEditICRECAExpDto) {
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
            this.expType = data["expType"];
            this.accountID = data["accountID"];
            this.amount = data["amount"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditICRECAExpDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditICRECAExpDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["locID"] = this.locID;
        data["docNo"] = this.docNo;
        data["expType"] = this.expType;
        data["accountID"] = this.accountID;
        data["amount"] = this.amount;
        data["id"] = this.id;
        return data; 
    }
}
