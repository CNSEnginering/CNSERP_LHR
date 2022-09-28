import * as moment from 'moment';
import { ICreateOrEditGLINVDetailDto, IGLINVDetailDto, IPagedResultDtoOfGLINVDetailDto } from '../interface/glinvdetails-interface';

export class PagedResultDtoOfGLINVDetailDto implements IPagedResultDtoOfGLINVDetailDto {
    totalCount!: number | undefined;
    items!: GLINVDetailDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGLINVDetailDto) {
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
                    this.items!.push(GLINVDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGLINVDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGLINVDetailDto();
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

export class GLINVDetailDto implements IGLINVDetailDto {
    detID!: number;
    accountID!: string;
    accountDesc!: string | undefined;
    subAccID!: number;
    subAccDesc!: string | undefined;
    narration!: string;
    debit!: number;
    credit!: number;
    amount!: number;
    isAuto!: boolean;
    id!: number;
    

    constructor(data?: IGLINVDetailDto) {
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
            this.accountID = data["accountID"];
            this.accountDesc = data["accountDesc"];
            this.subAccID = data["subAccID"];
            this.subAccDesc = data["subAccDesc"];
            this.narration = data["narration"];
            this.amount = data["amount"];
            this.isAuto = data["isAuto"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): GLINVDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new GLINVDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["detID"] = this.detID;
        data["accountID"] = this.accountID;
        data["accountDesc"] = this.accountDesc;
        data["subAccID"] = this.subAccID;
        data["subAccDesc"] = this.subAccDesc;
        data["narration"] = this.narration;
        data["amount"] = this.amount;
        data["isAuto"] = this.isAuto;
        data["id"] = this.id;
        return data; 
    }
}

export class CreateOrEditGLINVDetailDto implements ICreateOrEditGLINVDetailDto {
    detID!: number;
    accountID!: string;
    accountDesc!: string | undefined;
    subAccID!: number | undefined;
    subAccDesc!: string | undefined;
    narration!: string | undefined;
    amount!: number | undefined;
    isAuto!: boolean | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditGLINVDetailDto) {
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
            this.accountID = data["accountID"];
            this.accountDesc = data["accountDesc"];
            this.subAccID = data["subAccID"];
            this.subAccDesc = data["subAccDesc"];
            this.narration = data["narration"];
            this.amount = data["amount"];
            this.isAuto = data["isAuto"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CreateOrEditGLINVDetailDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditGLINVDetailDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        debugger;
        data["detID"] = this.detID;
        data["accountID"] = this.accountID;
        data["accountDesc"] = this.accountDesc;
        data["subAccID"] = this.subAccID;
        data["subAccDesc"] = this.subAccDesc;
        data["narration"] = this.narration;
        data["amount"] = this.amount;
        data["isAuto"] = this.isAuto;
        data["id"] = this.id;
        return data; 
    }
}
